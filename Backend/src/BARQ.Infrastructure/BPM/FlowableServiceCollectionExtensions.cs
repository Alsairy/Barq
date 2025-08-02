using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;
using Flowable.Sdk;

namespace BARQ.Infrastructure.BPM
{
    /// <summary>
    /// </summary>
    public static class FlowableServiceCollectionExtensions
    {
        /// <summary>
        /// </summary>
        public static IServiceCollection AddFlowableBpmServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FlowableHttpClientOptions>(
                configuration.GetSection(FlowableHttpClientOptions.FlowableClientSection));
            
            var flowableOptions = configuration
                .GetSection(FlowableHttpClientOptions.FlowableClientSection)
                .Get<FlowableHttpClientOptions>() ?? new FlowableHttpClientOptions();
            
            services.AddFlowableHttpClients(configuration);
            services.AddFlowableExternalWorkerHttpClient(configuration);
            
            services.AddScoped<IWorkflowService, FlowableWorkflowService>();
            services.AddScoped<ISLAConfigurationService, SLAConfigurationService>();
            services.AddScoped<IEscalationService, EscalationService>();
            services.AddScoped<IDelegationService, DelegationService>();
            services.AddScoped<IBpmnMigrationService, BpmnMigrationService>();
            
            return services;
        }
        
        /// <summary>
        /// </summary>
        public static IServiceCollection AddFlowableHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var httpClientOptions = configuration
                .GetSection(FlowableHttpClientOptions.FlowableClientSection)
                .Get<FlowableHttpClientOptions>() ?? new FlowableHttpClientOptions();
            
            services.AddOptions<FlowableHttpClientOptions>().Bind(configuration);
            
            services.AddHttpClient<IFlowableProcessHttpClient, FlowableProcessHttpClient>(options =>
            {
                options.BaseAddress = new Uri(httpClientOptions.BaseUrlProcessApi);
            }).AddPolicyHandler(GetRetryPolicy(httpClientOptions))
              .AddPolicyHandler(GetCircuitBreakerPolicy(httpClientOptions));
            
            services.AddHttpClient<IFlowableCaseHttpClient, FlowableCaseHttpClient>(options =>
            {
                options.BaseAddress = new Uri(httpClientOptions.BaseUrlCmmnApi);
            }).AddPolicyHandler(GetRetryPolicy(httpClientOptions))
              .AddPolicyHandler(GetCircuitBreakerPolicy(httpClientOptions));
            
            return services;
        }
        
        /// <summary>
        /// </summary>
        public static IServiceCollection AddFlowableExternalWorkerHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var httpClientOptions = configuration
                .GetSection(FlowableHttpClientOptions.FlowableClientSection)
                .Get<FlowableHttpClientOptions>() ?? new FlowableHttpClientOptions();
            
            services.AddOptions<FlowableHttpClientOptions>().Bind(configuration);
            
            services.AddHttpClient<IFlowableExternalWorkerHttpClient, FlowableExternalWorkerHttpClient>(options =>
            {
                options.BaseAddress = new Uri(httpClientOptions.BaseUrlExternalWorker);
            }).AddPolicyHandler(GetRetryPolicy(httpClientOptions))
              .AddPolicyHandler(GetCircuitBreakerPolicy(httpClientOptions));
            
            return services;
        }
        
        /// <summary>
        /// </summary>
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(FlowableHttpClientOptions options)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(options.MaxRetryAttempts, 
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
        
        /// <summary>
        /// </summary>
        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(FlowableHttpClientOptions options)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<HttpRequestException>()
                .CircuitBreakerAsync(
                    options.CircuitBreakerThreshold, 
                    TimeSpan.FromSeconds(options.CircuitBreakerRecoverySeconds));
        }
    }
}
