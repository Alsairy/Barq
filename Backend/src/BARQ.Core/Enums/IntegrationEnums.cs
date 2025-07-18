namespace BARQ.Core.Enums;

/// <summary>
/// </summary>
public enum IntegrationProtocol
{
    /// <summary>
    /// </summary>
    REST = 0,
    /// <summary>
    /// </summary>
    SOAP = 1,
    /// <summary>
    /// </summary>
    GraphQL = 2,
    /// <summary>
    /// </summary>
    WebSocket = 3,
    /// <summary>
    /// </summary>
    gRPC = 4,
    /// <summary>
    /// </summary>
    MessageQueue = 5
}

/// <summary>
/// </summary>
public enum MessagePriority
{
    /// <summary>
    /// </summary>
    Low = 0,
    /// <summary>
    /// </summary>
    Normal = 1,
    /// <summary>
    /// </summary>
    High = 2,
    /// <summary>
    /// </summary>
    Critical = 3
}

/// <summary>
/// </summary>
public enum MessageStatus
{
    /// <summary>
    /// </summary>
    Pending = 0,
    /// <summary>
    /// </summary>
    Processing = 1,
    /// <summary>
    /// </summary>
    Completed = 2,
    /// <summary>
    /// </summary>
    Failed = 3,
    /// <summary>
    /// </summary>
    Retrying = 4,
    /// <summary>
    /// </summary>
    DeadLetter = 5
}

/// <summary>
/// </summary>
public enum IntegrationEventLevel
{
    /// <summary>
    /// </summary>
    Debug = 0,
    /// <summary>
    /// </summary>
    Info = 1,
    /// <summary>
    /// </summary>
    Warning = 2,
    /// <summary>
    /// </summary>
    Error = 3,
    /// <summary>
    /// </summary>
    Critical = 4
}

/// <summary>
/// </summary>
public enum IntegrationAlertSeverity
{
    /// <summary>
    /// </summary>
    Low = 0,
    /// <summary>
    /// </summary>
    Medium = 1,
    /// <summary>
    /// </summary>
    High = 2,
    /// <summary>
    /// </summary>
    Critical = 3
}

/// <summary>
/// </summary>
public enum IntegrationAuthenticationType
{
    /// <summary>
    /// </summary>
    None = 0,
    /// <summary>
    /// </summary>
    ApiKey = 1,
    /// <summary>
    /// </summary>
    Bearer = 2,
    /// <summary>
    /// </summary>
    Basic = 3,
    /// <summary>
    /// </summary>
    OAuth2 = 4,
    /// <summary>
    /// </summary>
    Certificate = 5
}
