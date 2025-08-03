using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Xml.Linq;
using BARQ.Core.Services.Integration;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Enums;
using BARQ.Core.Services;

namespace BARQ.Application.Services.Integration;

public class MessageOrchestrationService : IMessageOrchestrationService
{
    private readonly ILogger<MessageOrchestrationService> _logger;
    private readonly ITenantProvider _tenantProvider;
    private readonly IIntegrationMonitoringService _monitoringService;
    private readonly ConcurrentDictionary<string, ConcurrentQueue<IntegrationMessage>> _messageQueues;
    private readonly ConcurrentDictionary<string, IntegrationMessage> _processingMessages;
    private readonly ConcurrentDictionary<string, IntegrationMessage> _failedMessages;
    private readonly Timer _processingTimer;

    public MessageOrchestrationService(
        ILogger<MessageOrchestrationService> logger,
        ITenantProvider tenantProvider,
        IIntegrationMonitoringService monitoringService)
    {
        _logger = logger;
        _tenantProvider = tenantProvider;
        _monitoringService = monitoringService;
        _messageQueues = new ConcurrentDictionary<string, ConcurrentQueue<IntegrationMessage>>();
        _processingMessages = new ConcurrentDictionary<string, IntegrationMessage>();
        _failedMessages = new ConcurrentDictionary<string, IntegrationMessage>();
        
        _processingTimer = new Timer(ProcessPendingMessages, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
    }

    public async Task<string> EnqueueMessageAsync(IntegrationMessage message, MessagePriority priority = MessagePriority.Normal)
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            message.TenantId = tenantId;
            message.Priority = priority;
            message.Status = MessageStatus.Pending;
            message.CreatedAt = DateTime.UtcNow;

            if (string.IsNullOrEmpty(message.Id))
            {
                message.Id = Guid.NewGuid().ToString();
            }

            var queueKey = $"{tenantId}:{message.QueueName}";
            var queue = _messageQueues.GetOrAdd(queueKey, _ => new ConcurrentQueue<IntegrationMessage>());

            if (priority == MessagePriority.Critical || priority == MessagePriority.High)
            {
                var tempQueue = new ConcurrentQueue<IntegrationMessage>();
                tempQueue.Enqueue(message);

                while (queue.TryDequeue(out var existingMessage))
                {
                    tempQueue.Enqueue(existingMessage);
                }

                _messageQueues.TryUpdate(queueKey, tempQueue, queue);
            }
            else
            {
                queue.Enqueue(message);
            }

            await _monitoringService.LogIntegrationEventAsync(new IntegrationEvent
            {
                EventType = "MESSAGE_ENQUEUED",
                EndpointId = message.QueueName,
                Description = $"Message {message.Id} enqueued with priority {priority}",
                Level = IntegrationEventLevel.Info,
                Data = new Dictionary<string, object>
                {
                    ["MessageId"] = message.Id,
                    ["QueueName"] = message.QueueName,
                    ["Priority"] = priority.ToString(),
                    ["MessageType"] = message.MessageType
                },
                TenantId = tenantId
            });

            _logger.LogInformation("Message {MessageId} enqueued to {QueueName} with priority {Priority}", 
                message.Id, message.QueueName, priority);

            return message.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enqueuing message {MessageId} to {QueueName}", message.Id, message.QueueName);
            throw;
        }
    }

    public async Task<IntegrationMessage?> DequeueMessageAsync(string queueName, CancellationToken cancellationToken = default)
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            var queueKey = $"{tenantId}:{queueName}";

            if (!_messageQueues.TryGetValue(queueKey, out var queue))
            {
                return null;
            }

            if (queue.TryDequeue(out var message))
            {
                message.Status = MessageStatus.Processing;
                message.ProcessedAt = DateTime.UtcNow;
                _processingMessages.TryAdd(message.Id, message);

                await _monitoringService.LogIntegrationEventAsync(new IntegrationEvent
                {
                    EventType = "MESSAGE_DEQUEUED",
                    EndpointId = queueName,
                    Description = $"Message {message.Id} dequeued for processing",
                    Level = IntegrationEventLevel.Info,
                    Data = new Dictionary<string, object>
                    {
                        ["MessageId"] = message.Id,
                        ["QueueName"] = queueName,
                        ["MessageType"] = message.MessageType
                    },
                    TenantId = tenantId
                });

                _logger.LogInformation("Message {MessageId} dequeued from {QueueName}", message.Id, queueName);
                return message;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error dequeuing message from {QueueName}", queueName);
            return null;
        }
    }

    public async Task<bool> ProcessMessageAsync(IntegrationMessage message)
    {
        try
        {
            _logger.LogInformation("Processing message {MessageId} of type {MessageType}", 
                message.Id, message.MessageType);

            var success = await ProcessMessageWithProvider(message);

            if (success)
            {
                message.Status = MessageStatus.Completed;
                message.ProcessedAt = DateTime.UtcNow;
                _processingMessages.TryRemove(message.Id, out _);

                await _monitoringService.LogIntegrationEventAsync(new IntegrationEvent
                {
                    EventType = "MESSAGE_PROCESSED",
                    EndpointId = message.QueueName,
                    Description = $"Message {message.Id} processed successfully",
                    Level = IntegrationEventLevel.Info,
                    Data = new Dictionary<string, object>
                    {
                        ["MessageId"] = message.Id,
                        ["MessageType"] = message.MessageType,
                        ["ProcessingTime"] = (DateTime.UtcNow - message.CreatedAt).TotalMilliseconds
                    },
                    TenantId = message.TenantId
                });

                _logger.LogInformation("Message {MessageId} processed successfully", message.Id);
                return true;
            }
            else
            {
                message.RetryCount++;
                if (message.RetryCount >= message.MaxRetries)
                {
                    message.Status = MessageStatus.DeadLetter;
                    _failedMessages.TryAdd(message.Id, message);
                    _processingMessages.TryRemove(message.Id, out _);

                    await _monitoringService.LogIntegrationEventAsync(new IntegrationEvent
                    {
                        EventType = "MESSAGE_DEAD_LETTER",
                        EndpointId = message.QueueName,
                        Description = $"Message {message.Id} moved to dead letter queue after {message.RetryCount} retries",
                        Level = IntegrationEventLevel.Error,
                        Data = new Dictionary<string, object>
                        {
                            ["MessageId"] = message.Id,
                            ["RetryCount"] = message.RetryCount,
                            ["ErrorMessage"] = message.ErrorMessage ?? "Processing failed"
                        },
                        TenantId = message.TenantId
                    });

                    _logger.LogError("Message {MessageId} moved to dead letter queue after {RetryCount} retries", 
                        message.Id, message.RetryCount);
                }
                else
                {
                    message.Status = MessageStatus.Retrying;
                    message.NextRetryAt = DateTime.UtcNow.AddSeconds(Math.Pow(2, message.RetryCount));
                    
                    var queueKey = $"{message.TenantId}:{message.QueueName}";
                    var queue = _messageQueues.GetOrAdd(queueKey, _ => new ConcurrentQueue<IntegrationMessage>());
                    queue.Enqueue(message);
                    _processingMessages.TryRemove(message.Id, out _);

                    _logger.LogWarning("Message {MessageId} requeued for retry {RetryCount}/{MaxRetries}", 
                        message.Id, message.RetryCount, message.MaxRetries);
                }

                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message {MessageId}", message.Id);
            message.ErrorMessage = ex.Message;
            return false;
        }
    }

    public async Task<MessageTransformationResult> TransformMessageAsync(IntegrationMessage message, string targetFormat)
    {
        try
        {
            _logger.LogInformation("Transforming message {MessageId} to format {TargetFormat}", 
                message.Id, targetFormat);

            var sourceFormat = DetectMessageFormat(message.Content);
            
            if (sourceFormat.Equals(targetFormat, StringComparison.OrdinalIgnoreCase))
            {
                return new MessageTransformationResult
                {
                    Success = true,
                    TransformedContent = message.Content,
                    SourceFormat = sourceFormat,
                    TargetFormat = targetFormat,
                    TransformedAt = DateTime.UtcNow
                };
            }

            var transformedContent = await PerformTransformation(message.Content, sourceFormat, targetFormat);

            var result = new MessageTransformationResult
            {
                Success = !string.IsNullOrEmpty(transformedContent),
                TransformedContent = transformedContent,
                SourceFormat = sourceFormat,
                TargetFormat = targetFormat,
                TransformedAt = DateTime.UtcNow
            };

            if (!result.Success)
            {
                result.ErrorMessage = $"Failed to transform from {sourceFormat} to {targetFormat}";
            }

            await _monitoringService.LogIntegrationEventAsync(new IntegrationEvent
            {
                EventType = "MESSAGE_TRANSFORMED",
                EndpointId = message.QueueName,
                Description = $"Message {message.Id} transformed from {sourceFormat} to {targetFormat}",
                Level = result.Success ? IntegrationEventLevel.Info : IntegrationEventLevel.Error,
                Data = new Dictionary<string, object>
                {
                    ["MessageId"] = message.Id,
                    ["SourceFormat"] = sourceFormat,
                    ["TargetFormat"] = targetFormat,
                    ["Success"] = result.Success
                },
                TenantId = message.TenantId
            });

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error transforming message {MessageId}", message.Id);
            return new MessageTransformationResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                SourceFormat = DetectMessageFormat(message.Content),
                TargetFormat = targetFormat,
                TransformedAt = DateTime.UtcNow
            };
        }
    }

    public Task<IEnumerable<QueueStatus>> GetQueueStatusAsync()
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            var tenantPrefix = $"{tenantId}:";
            var statuses = new List<QueueStatus>();

            foreach (var kvp in _messageQueues.Where(q => q.Key.StartsWith(tenantPrefix)))
            {
                var queueName = kvp.Key.Substring(tenantPrefix.Length);
                var queue = kvp.Value;

                var pendingCount = queue.Count;
                var processingCount = _processingMessages.Values.Count(m => m.QueueName == queueName && m.TenantId == tenantId);
                var failedCount = _failedMessages.Values.Count(m => m.QueueName == queueName && m.TenantId == tenantId);

                statuses.Add(new QueueStatus
                {
                    QueueName = queueName,
                    PendingMessages = pendingCount,
                    ProcessingMessages = processingCount,
                    CompletedMessages = 0,
                    FailedMessages = failedCount,
                    IsHealthy = pendingCount < 1000 && failedCount < 100,
                    LastUpdated = DateTime.UtcNow
                });
            }

            return Task.CompletedTask.ContinueWith(_ => (IEnumerable<QueueStatus>)statuses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting queue status");
            var emptyStatuses = new List<QueueStatus>();
            return Task.CompletedTask.ContinueWith(_ => (IEnumerable<QueueStatus>)emptyStatuses);
        }
    }

    public async Task<bool> RetryFailedMessageAsync(string messageId)
    {
        try
        {
            if (!_failedMessages.TryRemove(messageId, out var message))
            {
                _logger.LogWarning("Failed message {MessageId} not found for retry", messageId);
                return false;
            }

            message.Status = MessageStatus.Pending;
            message.RetryCount = 0;
            message.ErrorMessage = null;

            await EnqueueMessageAsync(message, message.Priority);

            _logger.LogInformation("Failed message {MessageId} requeued for retry", messageId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrying failed message {MessageId}", messageId);
            return false;
        }
    }

    private async Task<bool> ProcessMessageWithProvider(IntegrationMessage message)
    {
        try
        {
            switch (message.MessageType.ToUpper())
            {
                case "EMAIL":
                    return await ProcessEmailMessage(message);
                case "SMS":
                    return await ProcessSmsMessage(message);
                case "WEBHOOK":
                    return await ProcessWebhookMessage(message);
                case "API":
                    return await ProcessApiMessage(message);
                default:
                    _logger.LogWarning("Unknown message type: {MessageType}", message.MessageType);
                    return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message {MessageId} of type {MessageType}", 
                message.Id, message.MessageType);
            message.ErrorMessage = ex.Message;
            return false;
        }
    }

    private Task<bool> ProcessEmailMessage(IntegrationMessage message)
    {
        _logger.LogInformation("Processing email message {MessageId}", message.Id);
        return Task.CompletedTask.ContinueWith(_ => true);
    }

    private Task<bool> ProcessSmsMessage(IntegrationMessage message)
    {
        _logger.LogInformation("Processing SMS message {MessageId}", message.Id);
        return Task.CompletedTask.ContinueWith(_ => true);
    }

    private Task<bool> ProcessWebhookMessage(IntegrationMessage message)
    {
        _logger.LogInformation("Processing webhook message {MessageId}", message.Id);
        return Task.CompletedTask.ContinueWith(_ => true);
    }

    private Task<bool> ProcessApiMessage(IntegrationMessage message)
    {
        _logger.LogInformation("Processing API message {MessageId}", message.Id);
        return Task.CompletedTask.ContinueWith(_ => true);
    }

    private string DetectMessageFormat(string content)
    {
        if (string.IsNullOrEmpty(content))
            return "UNKNOWN";

        content = content.Trim();

        if (content.StartsWith("{") && content.EndsWith("}"))
            return "JSON";

        if (content.StartsWith("<") && content.EndsWith(">"))
            return "XML";

        if (content.Contains("=") && content.Contains("&"))
            return "FORM";

        return "TEXT";
    }

    private async Task<string?> PerformTransformation(string content, string sourceFormat, string targetFormat)
    {
        try
        {
            switch ($"{sourceFormat.ToUpper()}_TO_{targetFormat.ToUpper()}")
            {
                case "JSON_TO_XML":
                    return await TransformJsonToXml(content);
                case "XML_TO_JSON":
                    return await TransformXmlToJson(content);
                case "JSON_TO_FORM":
                    return await TransformJsonToForm(content);
                case "FORM_TO_JSON":
                    return await TransformFormToJson(content);
                default:
                    return content;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing transformation from {SourceFormat} to {TargetFormat}", 
                sourceFormat, targetFormat);
            return null;
        }
    }

    private Task<string> TransformJsonToXml(string jsonContent)
    {
        var jsonDoc = JsonDocument.Parse(jsonContent);
        return Task.CompletedTask.ContinueWith(_ => $"<root>{JsonElementToXml(jsonDoc.RootElement)}</root>");
    }

    private Task<string> TransformXmlToJson(string xmlContent)
    {
        var doc = XDocument.Parse(xmlContent);
        var jsonObject = XmlToJsonObject(doc.Root!);
        var json = JsonSerializer.Serialize(jsonObject);
        return Task.CompletedTask.ContinueWith(_ => json);
    }

    private object XmlToJsonObject(XElement? element)
    {
        if (element == null) return string.Empty;

        var result = new Dictionary<string, object>();
        
        foreach (var attr in element.Attributes())
        {
            result[$"@{attr.Name}"] = attr.Value;
        }

        var children = element.Elements().ToList();
        if (children.Any())
        {
            var groups = children.GroupBy(e => e.Name.LocalName);
            foreach (var group in groups)
            {
                var items = group.ToList();
                if (items.Count == 1)
                {
                    result[group.Key] = XmlToJsonObject(items[0]);
                }
                else
                {
                    result[group.Key] = items.Select(XmlToJsonObject).ToArray();
                }
            }
        }
        else if (!string.IsNullOrEmpty(element.Value))
        {
            return element.Value;
        }

        return result.Any() ? result : (element?.Value ?? string.Empty);
    }

    private Task<string> TransformJsonToForm(string jsonContent)
    {
        var jsonDoc = JsonDocument.Parse(jsonContent);
        var formPairs = new List<string>();
        
        foreach (var property in jsonDoc.RootElement.EnumerateObject())
        {
            formPairs.Add($"{property.Name}={Uri.EscapeDataString(property.Value.ToString())}");
        }
        
        return Task.CompletedTask.ContinueWith(_ => string.Join("&", formPairs));
    }

    private Task<string> TransformFormToJson(string formContent)
    {
        var pairs = formContent.Split('&');
        var jsonObject = new Dictionary<string, string>();
        
        foreach (var pair in pairs)
        {
            var parts = pair.Split('=', 2);
            if (parts.Length == 2)
            {
                jsonObject[parts[0]] = Uri.UnescapeDataString(parts[1]);
            }
        }
        
        return Task.CompletedTask.ContinueWith(_ => JsonSerializer.Serialize(jsonObject));
    }

    private string JsonElementToXml(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var objectXml = "";
                foreach (var property in element.EnumerateObject())
                {
                    objectXml += $"<{property.Name}>{JsonElementToXml(property.Value)}</{property.Name}>";
                }
                return objectXml;
            case JsonValueKind.Array:
                var arrayXml = "";
                foreach (var item in element.EnumerateArray())
                {
                    arrayXml += $"<item>{JsonElementToXml(item)}</item>";
                }
                return arrayXml;
            default:
                return element.ToString();
        }
    }

    private void ProcessPendingMessages(object? state)
    {
        try
        {
            var messagesToProcess = new List<IntegrationMessage>();
            
            foreach (var kvp in _messageQueues)
            {
                var queue = kvp.Value;
                var processedCount = 0;
                
                while (processedCount < 10 && queue.TryPeek(out var message))
                {
                    if (message.Status == MessageStatus.Pending)
                    {
                        if (queue.TryDequeue(out var dequeuedMessage))
                        {
                            messagesToProcess.Add(dequeuedMessage);
                            processedCount++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            foreach (var message in messagesToProcess)
            {
                _ = Task.Run(async () => await ProcessMessageAsync(message));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in background message processing");
        }
    }

    public void Dispose()
    {
        _processingTimer?.Dispose();
    }
}
