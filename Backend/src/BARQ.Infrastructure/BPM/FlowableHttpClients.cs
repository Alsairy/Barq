using System;
using System.Threading.Tasks;
using System.Net.Http;

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
            await Task.Delay(100); // Simulate API call
            return Guid.NewGuid().ToString();
        }

        public async Task<object> GetProcessInstanceAsync(string processInstanceId)
        {
            await Task.Delay(100); // Simulate API call
            return new { id = processInstanceId, status = "active" };
        }

        public async Task CompleteTaskAsync(string taskId, object variables = null)
        {
            await Task.Delay(100); // Simulate API call
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
            await Task.Delay(100); // Simulate API call
            return Guid.NewGuid().ToString();
        }

        public async Task<object> GetCaseInstanceAsync(string caseInstanceId)
        {
            await Task.Delay(100); // Simulate API call
            return new { id = caseInstanceId, status = "active" };
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
            await Task.Delay(100); // Simulate API call
            return Array.Empty<object>();
        }

        public async Task CompleteExternalTaskAsync(string taskId, object variables = null)
        {
            await Task.Delay(100); // Simulate API call
        }
    }
}
