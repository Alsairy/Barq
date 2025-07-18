import { apiSlice } from './apiSlice';

export interface IntegrationEndpoint {
  id: string;
  name: string;
  description: string;
  protocol: 'REST' | 'SOAP' | 'GraphQL' | 'WebSocket';
  url: string;
  isActive: boolean;
  isHealthy: boolean;
  lastHealthCheck: string;
  configuration: Record<string, any>;
  authentication: {
    type: 'none' | 'basic' | 'bearer' | 'oauth2' | 'api-key';
    credentials?: Record<string, string>;
  };
  timeout: number;
  retryPolicy: {
    maxRetries: number;
    retryDelay: number;
    backoffMultiplier: number;
  };
  tags: string[];
  createdAt: string;
  updatedAt: string;
}

export interface IntegrationMetrics {
  totalRequests: number;
  successfulRequests: number;
  failedRequests: number;
  successRate: number;
  averageResponseTime: number;
  endpointUsage: Record<string, number>;
  errorCounts: Record<string, number>;
  fromDate: string;
  toDate: string;
}

export interface IntegrationAlert {
  id: string;
  ruleId: string;
  title: string;
  description: string;
  severity: 'low' | 'medium' | 'high' | 'critical';
  endpointId: string;
  isResolved: boolean;
  createdAt: string;
  resolvedAt?: string;
  data: Record<string, any>;
}

export interface IntegrationAlertRule {
  id: string;
  name: string;
  description: string;
  condition: string;
  severity: 'low' | 'medium' | 'high' | 'critical';
  isActive: boolean;
  parameters: Record<string, any>;
  createdAt: string;
}

export interface IntegrationHealthDashboard {
  totalEndpoints: number;
  healthyEndpoints: number;
  unhealthyEndpoints: number;
  activeAlerts: number;
  overallHealthScore: number;
  recentlyFailedEndpoints: IntegrationEndpoint[];
  criticalAlerts: IntegrationAlert[];
  lastUpdated: string;
}

export interface IntegrationEvent {
  id: string;
  eventType: string;
  endpointId: string;
  level: 'info' | 'warning' | 'error';
  message: string;
  data: Record<string, any>;
  createdAt: string;
  tenantId: string;
}

export interface QueueStatus {
  queueName: string;
  pendingMessages: number;
  processingMessages: number;
  failedMessages: number;
  averageProcessingTime: number;
  lastProcessedAt?: string;
}

export interface ConnectionTestResult {
  success: boolean;
  responseTime: number;
  statusCode?: number;
  errorMessage?: string;
  details: Record<string, any>;
}

export interface DevToolIntegration {
  id: string;
  name: string;
  type: 'git' | 'ci-cd' | 'issue-tracking' | 'code-quality';
  provider: string;
  isConnected: boolean;
  configuration: Record<string, any>;
  lastSync: string;
  syncStatus: 'success' | 'failed' | 'in-progress';
  features: string[];
}

export interface BusinessSystemIntegration {
  id: string;
  name: string;
  type: 'crm' | 'communication' | 'document-management' | 'calendar';
  provider: string;
  isConnected: boolean;
  configuration: Record<string, any>;
  lastSync: string;
  syncStatus: 'success' | 'failed' | 'in-progress';
  dataMapping: Record<string, any>;
}

export interface SecurityConfiguration {
  id: string;
  name: string;
  type: 'authentication' | 'encryption' | 'access-control' | 'audit';
  isEnabled: boolean;
  configuration: Record<string, any>;
  lastUpdated: string;
  complianceStatus: 'compliant' | 'non-compliant' | 'pending';
}

export const integrationApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    getIntegrationEndpoints: builder.query<IntegrationEndpoint[], { search?: string; protocol?: string; status?: string }>({
      query: ({ search, protocol, status } = {}) => ({
        url: '/api/integration/endpoints',
        params: { search, protocol, status },
      }),
      providesTags: ['Integration'],
    }),

    getIntegrationEndpoint: builder.query<IntegrationEndpoint, string>({
      query: (id) => `/api/integration/endpoints/${id}`,
      providesTags: (_result, _error, id) => [{ type: 'Integration', id }],
    }),

    createIntegrationEndpoint: builder.mutation<IntegrationEndpoint, Partial<IntegrationEndpoint>>({
      query: (endpoint) => ({
        url: '/api/integration/endpoints',
        method: 'POST',
        body: endpoint,
      }),
      invalidatesTags: ['Integration'],
    }),

    updateIntegrationEndpoint: builder.mutation<IntegrationEndpoint, { id: string; endpoint: Partial<IntegrationEndpoint> }>({
      query: ({ id, endpoint }) => ({
        url: `/api/integration/endpoints/${id}`,
        method: 'PUT',
        body: endpoint,
      }),
      invalidatesTags: (_result, _error, { id }) => [{ type: 'Integration', id }],
    }),

    deleteIntegrationEndpoint: builder.mutation<void, string>({
      query: (id) => ({
        url: `/api/integration/endpoints/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Integration'],
    }),

    testConnection: builder.mutation<ConnectionTestResult, string>({
      query: (endpointId) => ({
        url: `/api/integration/endpoints/${endpointId}/test`,
        method: 'POST',
      }),
    }),

    getIntegrationMetrics: builder.query<IntegrationMetrics, { fromDate: string; toDate: string }>({
      query: ({ fromDate, toDate }) => ({
        url: '/api/integration/metrics',
        params: { fromDate, toDate },
      }),
      providesTags: ['Integration'],
    }),

    getHealthDashboard: builder.query<IntegrationHealthDashboard, void>({
      query: () => '/api/integration/health/dashboard',
      providesTags: ['Integration'],
    }),

    getIntegrationEvents: builder.query<IntegrationEvent[], { endpointId?: string; level?: string; limit?: number }>({
      query: ({ endpointId, level, limit = 100 } = {}) => ({
        url: '/api/integration/events',
        params: { endpointId, level, limit },
      }),
      providesTags: ['Integration'],
    }),

    getActiveAlerts: builder.query<IntegrationAlert[], void>({
      query: () => '/api/integration/alerts/active',
      providesTags: ['Integration'],
    }),

    getAlertRules: builder.query<IntegrationAlertRule[], void>({
      query: () => '/api/integration/alerts/rules',
      providesTags: ['Integration'],
    }),

    createAlertRule: builder.mutation<IntegrationAlertRule, Partial<IntegrationAlertRule>>({
      query: (rule) => ({
        url: '/api/integration/alerts/rules',
        method: 'POST',
        body: rule,
      }),
      invalidatesTags: ['Integration'],
    }),

    updateAlertRule: builder.mutation<IntegrationAlertRule, { id: string; rule: Partial<IntegrationAlertRule> }>({
      query: ({ id, rule }) => ({
        url: `/api/integration/alerts/rules/${id}`,
        method: 'PUT',
        body: rule,
      }),
      invalidatesTags: ['Integration'],
    }),

    resolveAlert: builder.mutation<void, string>({
      query: (alertId) => ({
        url: `/api/integration/alerts/${alertId}/resolve`,
        method: 'POST',
      }),
      invalidatesTags: ['Integration'],
    }),

    getQueueStatus: builder.query<QueueStatus[], void>({
      query: () => '/api/integration/queues/status',
      providesTags: ['Integration'],
    }),

    retryFailedMessage: builder.mutation<void, string>({
      query: (messageId) => ({
        url: `/api/integration/messages/${messageId}/retry`,
        method: 'POST',
      }),
      invalidatesTags: ['Integration'],
    }),

    getDevToolIntegrations: builder.query<DevToolIntegration[], { type?: string }>({
      query: ({ type } = {}) => ({
        url: '/api/integration/dev-tools',
        params: { type },
      }),
      providesTags: ['Integration'],
    }),

    createDevToolIntegration: builder.mutation<DevToolIntegration, Partial<DevToolIntegration>>({
      query: (integration) => ({
        url: '/api/integration/dev-tools',
        method: 'POST',
        body: integration,
      }),
      invalidatesTags: ['Integration'],
    }),

    updateDevToolIntegration: builder.mutation<DevToolIntegration, { id: string; integration: Partial<DevToolIntegration> }>({
      query: ({ id, integration }) => ({
        url: `/api/integration/dev-tools/${id}`,
        method: 'PUT',
        body: integration,
      }),
      invalidatesTags: ['Integration'],
    }),

    syncDevToolIntegration: builder.mutation<void, string>({
      query: (id) => ({
        url: `/api/integration/dev-tools/${id}/sync`,
        method: 'POST',
      }),
      invalidatesTags: ['Integration'],
    }),

    getBusinessSystemIntegrations: builder.query<BusinessSystemIntegration[], { type?: string }>({
      query: ({ type } = {}) => ({
        url: '/api/integration/business-systems',
        params: { type },
      }),
      providesTags: ['Integration'],
    }),

    createBusinessSystemIntegration: builder.mutation<BusinessSystemIntegration, Partial<BusinessSystemIntegration>>({
      query: (integration) => ({
        url: '/api/integration/business-systems',
        method: 'POST',
        body: integration,
      }),
      invalidatesTags: ['Integration'],
    }),

    updateBusinessSystemIntegration: builder.mutation<BusinessSystemIntegration, { id: string; integration: Partial<BusinessSystemIntegration> }>({
      query: ({ id, integration }) => ({
        url: `/api/integration/business-systems/${id}`,
        method: 'PUT',
        body: integration,
      }),
      invalidatesTags: ['Integration'],
    }),

    syncBusinessSystemIntegration: builder.mutation<void, string>({
      query: (id) => ({
        url: `/api/integration/business-systems/${id}/sync`,
        method: 'POST',
      }),
      invalidatesTags: ['Integration'],
    }),

    getSecurityConfigurations: builder.query<SecurityConfiguration[], { type?: string }>({
      query: ({ type } = {}) => ({
        url: '/api/integration/security/configurations',
        params: { type },
      }),
      providesTags: ['Integration'],
    }),

    updateSecurityConfiguration: builder.mutation<SecurityConfiguration, { id: string; configuration: Partial<SecurityConfiguration> }>({
      query: ({ id, configuration }) => ({
        url: `/api/integration/security/configurations/${id}`,
        method: 'PUT',
        body: configuration,
      }),
      invalidatesTags: ['Integration'],
    }),

    runComplianceCheck: builder.mutation<{ status: string; results: Record<string, any> }, string>({
      query: (configurationId) => ({
        url: `/api/integration/security/configurations/${configurationId}/compliance-check`,
        method: 'POST',
      }),
      invalidatesTags: ['Integration'],
    }),

    getAuditLogs: builder.query<any[], { fromDate?: string; toDate?: string; type?: string }>({
      query: ({ fromDate, toDate, type } = {}) => ({
        url: '/api/integration/security/audit-logs',
        params: { fromDate, toDate, type },
      }),
      providesTags: ['Integration'],
    }),
  }),
});

export const {
  useGetIntegrationEndpointsQuery,
  useGetIntegrationEndpointQuery,
  useCreateIntegrationEndpointMutation,
  useUpdateIntegrationEndpointMutation,
  useDeleteIntegrationEndpointMutation,
  useTestConnectionMutation,
  
  useGetIntegrationMetricsQuery,
  useGetHealthDashboardQuery,
  useGetIntegrationEventsQuery,
  
  useGetActiveAlertsQuery,
  useGetAlertRulesQuery,
  useCreateAlertRuleMutation,
  useUpdateAlertRuleMutation,
  useResolveAlertMutation,
  
  useGetQueueStatusQuery,
  useRetryFailedMessageMutation,
  
  useGetDevToolIntegrationsQuery,
  useCreateDevToolIntegrationMutation,
  useUpdateDevToolIntegrationMutation,
  useSyncDevToolIntegrationMutation,
  
  useGetBusinessSystemIntegrationsQuery,
  useCreateBusinessSystemIntegrationMutation,
  useUpdateBusinessSystemIntegrationMutation,
  useSyncBusinessSystemIntegrationMutation,
  
  useGetSecurityConfigurationsQuery,
  useUpdateSecurityConfigurationMutation,
  useRunComplianceCheckMutation,
  useGetAuditLogsQuery,
} = integrationApi;
