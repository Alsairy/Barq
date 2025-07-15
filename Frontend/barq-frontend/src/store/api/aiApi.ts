import { apiSlice } from './apiSlice';

export interface AIProvider {
  id: string;
  name: string;
  type: 'openai' | 'anthropic' | 'azure' | 'google' | 'custom';
  status: 'active' | 'inactive' | 'error';
  responseTime: number;
  uptime: number;
  requestsToday: number;
  costToday: number;
  configuration: Record<string, any>;
  capabilities: string[];
  rateLimit: {
    requestsPerMinute: number;
    requestsPerDay: number;
    tokensPerMinute: number;
  };
  createdAt: string;
  lastHealthCheck: string;
}

export interface AITask {
  id: string;
  name: string;
  description: string;
  type: 'text-generation' | 'image-generation' | 'analysis' | 'classification' | 'translation';
  status: 'pending' | 'running' | 'completed' | 'failed';
  priority: 'low' | 'medium' | 'high' | 'critical';
  providerId: string;
  providerName: string;
  progress: number;
  startedAt: string;
  completedAt?: string;
  duration?: number;
  inputTokens?: number;
  outputTokens?: number;
  cost?: number;
  result?: any;
  errorMessage?: string;
  createdBy: string;
  tags: string[];
}

export interface AIMetrics {
  totalTasks: number;
  runningTasks: number;
  completedTasks: number;
  failedTasks: number;
  totalCost: number;
  averageResponseTime: number;
  successRate: number;
  tokensUsed: number;
}

export interface QualityMetrics {
  overallScore: number;
  accuracy: number;
  relevance: number;
  userSatisfaction: number;
  totalFeedback: number;
  positiveFeedback: number;
  negativeFeedback: number;
}

export const aiApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    getAIProviders: builder.query<AIProvider[], { search?: string; type?: string }>({
      query: ({ search, type } = {}) => ({
        url: '/api/ai/providers',
        params: { search, type },
      }),
      providesTags: ['AITask'],
    }),

    getAIProvider: builder.query<AIProvider, string>({
      query: (id) => `/api/ai/providers/${id}`,
      providesTags: (result, error, id) => [{ type: 'AITask', id }],
    }),

    createAIProvider: builder.mutation<AIProvider, Partial<AIProvider>>({
      query: (provider) => ({
        url: '/api/ai/providers',
        method: 'POST',
        body: provider,
      }),
      invalidatesTags: ['AITask'],
    }),

    updateAIProvider: builder.mutation<AIProvider, { id: string; provider: Partial<AIProvider> }>({
      query: ({ id, provider }) => ({
        url: `/api/ai/providers/${id}`,
        method: 'PUT',
        body: provider,
      }),
      invalidatesTags: (result, error, { id }) => [{ type: 'AITask', id }],
    }),

    deleteAIProvider: builder.mutation<void, string>({
      query: (id) => ({
        url: `/api/ai/providers/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['AITask'],
    }),

    testAIProvider: builder.mutation<{ success: boolean; responseTime: number; error?: string }, string>({
      query: (id) => ({
        url: `/api/ai/providers/${id}/test`,
        method: 'POST',
      }),
    }),

    getAITasks: builder.query<AITask[], { status?: string; providerId?: string; search?: string }>({
      query: ({ status, providerId, search } = {}) => ({
        url: '/api/ai/tasks',
        params: { status, providerId, search },
      }),
      providesTags: ['AITask'],
    }),

    getAITask: builder.query<AITask, string>({
      query: (id) => `/api/ai/tasks/${id}`,
      providesTags: (result, error, id) => [{ type: 'AITask', id }],
    }),

    createAITask: builder.mutation<AITask, Partial<AITask>>({
      query: (task) => ({
        url: '/api/ai/tasks',
        method: 'POST',
        body: task,
      }),
      invalidatesTags: ['AITask'],
    }),

    cancelAITask: builder.mutation<void, string>({
      query: (id) => ({
        url: `/api/ai/tasks/${id}/cancel`,
        method: 'POST',
      }),
      invalidatesTags: (result, error, id) => [{ type: 'AITask', id }],
    }),

    retryAITask: builder.mutation<AITask, string>({
      query: (id) => ({
        url: `/api/ai/tasks/${id}/retry`,
        method: 'POST',
      }),
      invalidatesTags: (result, error, id) => [{ type: 'AITask', id }],
    }),

    getAIMetrics: builder.query<AIMetrics, { timeRange?: string }>({
      query: ({ timeRange } = {}) => ({
        url: '/api/ai/metrics',
        params: { timeRange },
      }),
      providesTags: ['AITask'],
    }),

    getQualityMetrics: builder.query<QualityMetrics, { timeRange?: string }>({
      query: ({ timeRange } = {}) => ({
        url: '/api/ai/quality/metrics',
        params: { timeRange },
      }),
      providesTags: ['AITask'],
    }),

    submitFeedback: builder.mutation<void, { taskId: string; rating: number; feedback?: string }>({
      query: ({ taskId, rating, feedback }) => ({
        url: `/api/ai/tasks/${taskId}/feedback`,
        method: 'POST',
        body: { rating, feedback },
      }),
      invalidatesTags: ['AITask'],
    }),

    getProviderMetrics: builder.query<Record<string, any>, string>({
      query: (providerId) => `/api/ai/providers/${providerId}/metrics`,
      providesTags: (result, error, providerId) => [{ type: 'AITask', id: providerId }],
    }),

    getAIConfiguration: builder.query<Record<string, any>, void>({
      query: () => '/api/ai/configuration',
      providesTags: ['AITask'],
    }),

    updateAIConfiguration: builder.mutation<void, Record<string, any>>({
      query: (config) => ({
        url: '/api/ai/configuration',
        method: 'PUT',
        body: config,
      }),
      invalidatesTags: ['AITask'],
    }),
  }),
});

export const {
  useGetAIProvidersQuery,
  useGetAIProviderQuery,
  useCreateAIProviderMutation,
  useUpdateAIProviderMutation,
  useDeleteAIProviderMutation,
  useTestAIProviderMutation,
  useGetAITasksQuery,
  useGetAITaskQuery,
  useCreateAITaskMutation,
  useCancelAITaskMutation,
  useRetryAITaskMutation,
  useGetAIMetricsQuery,
  useGetQualityMetricsQuery,
  useSubmitFeedbackMutation,
  useGetProviderMetricsQuery,
  useGetAIConfigurationQuery,
  useUpdateAIConfigurationMutation,
} = aiApi;
