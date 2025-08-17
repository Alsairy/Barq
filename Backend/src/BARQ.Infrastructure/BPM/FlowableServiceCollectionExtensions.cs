using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;
using BARQ.Core.Interfaces;

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
            
            services.AddScoped<IFlowableProcessHttpClient, FlowableProcessHttpClient>();
            services.AddScoped<IFlowableCaseHttpClient, FlowableCaseHttpClient>();
            services.AddScoped<IFlowableExternalWorkerHttpClient, FlowableExternalWorkerHttpClient>();
            
            services.AddScoped<IWorkflowService, FlowableWorkflowService>();
            services.AddScoped<ISLAConfigurationService, SLAConfigurationService>();
            services.AddScoped<IEscalationService, EscalationService>();
            services.AddScoped<IDelegationService, DelegationService>();
            services.AddScoped<IBpmnMigrationService, BpmnMigrationService>();
            services.AddScoped<IAIRequestService, BARQ.Application.Services.AIRequests.AIRequestService>();
            services.AddScoped<IQualityAssuranceService, BARQ.Application.Services.QualityAssurance.QualityAssuranceService>();
            
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
            });
            
            services.AddHttpClient<IFlowableCaseHttpClient, FlowableCaseHttpClient>(options =>
            {
                options.BaseAddress = new Uri(httpClientOptions.BaseUrlCmmnApi);
            });
            
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
            });
            
            return services;
        }
        
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => 
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
