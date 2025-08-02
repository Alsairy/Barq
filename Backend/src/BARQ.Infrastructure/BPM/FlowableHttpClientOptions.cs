using System;

namespace BARQ.Infrastructure.BPM
{
    /// <summary>
    /// </summary>
    public class FlowableHttpClientOptions
    {
        /// <summary>
        /// </summary>
        public const string FlowableClientSection = "FlowableHttpClientOptions";
        
        /// <summary>
        /// </summary>
        public string BaseUrlExternalWorker { get; set; } = string.Empty;
        
        /// <summary>
        /// </summary>
        public string BaseUrlCmmnApi { get; set; } = string.Empty;
        
        /// <summary>
        /// </summary>
        public string BaseUrlProcessApi { get; set; } = string.Empty;
        
        /// <summary>
        /// </summary>
        public int ConnectionTimeoutSeconds { get; set; } = 30;
        
        /// <summary>
        /// </summary>
        public int MaxRetryAttempts { get; set; } = 3;
        
        /// <summary>
        /// </summary>
        public int CircuitBreakerThreshold { get; set; } = 5;
        
        /// <summary>
        /// </summary>
        public int CircuitBreakerRecoverySeconds { get; set; } = 60;
    }
}
