using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;

namespace BARQ.Infrastructure.BPM
{
    /// <summary>
    /// </summary>
    public interface IFlowableProcessHttpClient
    {
        /// <summary>
        /// </summary>
        Task<string> StartProcessInstanceAsync(string processDefinitionKey, object variables = null);
        
        /// <summary>
        /// </summary>
        Task<object> GetProcessInstanceAsync(string processInstanceId);
        
        /// <summary>
        /// </summary>
        Task CompleteTaskAsync(string taskId, object variables = null);
    }

    /// <summary>
    /// </summary>
    public interface IFlowableCaseHttpClient
    {
        /// <summary>
        /// </summary>
        Task<string> StartCaseInstanceAsync(string caseDefinitionKey, object variables = null);
        
        /// <summary>
        /// </summary>
        Task<object> GetCaseInstanceAsync(string caseInstanceId);
    }

    /// <summary>
    /// </summary>
    public interface IFlowableExternalWorkerHttpClient
    {
        /// <summary>
        /// </summary>
        Task<object[]> FetchAndLockAsync(string workerId, int maxTasks);
        
        /// <summary>
        /// </summary>
        Task CompleteExternalTaskAsync(string taskId, object variables = null);
    }

    /// <summary>
    /// </summary>
    public class FlowableProcessHttpClient : IFlowableProcessHttpClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// </summary>
        public FlowableProcessHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> StartProcessInstanceAsync(string processDefinitionKey, object variables = null)
        {
            var requestBody = new
            {
                processDefinitionKey,
                variables = variables ?? new { }
            };

            var response = await _httpClient.PostAsJsonAsync("/runtime/process-instances", requestBody);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<dynamic>();
            return result?.id?.ToString() ?? throw new InvalidOperationException("Failed to start process instance");
        }

        public async Task<object> GetProcessInstanceAsync(string processInstanceId)
        {
            var response = await _httpClient.GetAsync($"/runtime/process-instances/{processInstanceId}");
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<object>() ?? 
                   throw new InvalidOperationException($"Process instance {processInstanceId} not found");
        }

        public async Task CompleteTaskAsync(string taskId, object variables = null)
        {
            var requestBody = new
            {
                action = "complete",
                variables = variables ?? new { }
            };

            var response = await _httpClient.PostAsJsonAsync($"/runtime/tasks/{taskId}", requestBody);
            response.EnsureSuccessStatusCode();
        }
    }

    /// <summary>
    /// </summary>
    public class FlowableCaseHttpClient : IFlowableCaseHttpClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// </summary>
        public FlowableCaseHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> StartCaseInstanceAsync(string caseDefinitionKey, object variables = null)
        {
            var requestBody = new
            {
                caseDefinitionKey,
                variables = variables ?? new { }
            };

            var response = await _httpClient.PostAsJsonAsync("/cmmn-runtime/case-instances", requestBody);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<dynamic>();
            return result?.id?.ToString() ?? throw new InvalidOperationException("Failed to start case instance");
        }

        public async Task<object> GetCaseInstanceAsync(string caseInstanceId)
        {
            var response = await _httpClient.GetAsync($"/cmmn-runtime/case-instances/{caseInstanceId}");
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<object>() ?? 
                   throw new InvalidOperationException($"Case instance {caseInstanceId} not found");
        }
    }

    /// <summary>
    /// </summary>
    public class FlowableExternalWorkerHttpClient : IFlowableExternalWorkerHttpClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// </summary>
        public FlowableExternalWorkerHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<object[]> FetchAndLockAsync(string workerId, int maxTasks)
        {
            var requestBody = new
            {
                workerId,
                maxTasks,
                usePriority = true,
                asyncResponseTimeout = 30000
            };

            var response = await _httpClient.PostAsJsonAsync("/external-job/jobs/fetch-and-lock", requestBody);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<object[]>() ?? Array.Empty<object>();
        }

        public async Task CompleteExternalTaskAsync(string taskId, object variables = null)
        {
            var requestBody = new
            {
                workerId = "barq-worker",
                variables = variables ?? new { }
            };

            var response = await _httpClient.PostAsJsonAsync($"/external-job/jobs/{taskId}/complete", requestBody);
            response.EnsureSuccessStatusCode();
        }
    }
}
