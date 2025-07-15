import { apiSlice } from './apiSlice';

export interface WorkflowTemplate {
  id: string;
  name: string;
  description: string;
  category: string;
  version: string;
  isPublic: boolean;
  createdBy: string;
  createdAt: string;
  lastModified: string;
  usageCount: number;
  rating: number;
  ratingCount: number;
  tags: string[];
  complexity: 'simple' | 'medium' | 'complex';
  estimatedDuration: number;
  nodes: any[];
  edges: any[];
}

export interface WorkflowExecution {
  id: string;
  workflowId: string;
  workflowName: string;
  status: 'pending' | 'running' | 'completed' | 'failed' | 'paused';
  progress: number;
  startedAt: string;
  completedAt?: string;
  duration?: number;
  priority: 'low' | 'medium' | 'high' | 'critical';
  executedBy: string;
  currentStep?: string;
  totalSteps: number;
  completedSteps: number;
  errorMessage?: string;
  context: Record<string, any>;
}

export interface WorkflowMetrics {
  totalExecutions: number;
  runningExecutions: number;
  completedExecutions: number;
  failedExecutions: number;
  averageExecutionTime: number;
  successRate: number;
}

export const workflowApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    getWorkflowTemplates: builder.query<WorkflowTemplate[], { search?: string; category?: string; sortBy?: string }>({
      query: ({ search, category, sortBy } = {}) => ({
        url: '/api/workflows/templates',
        params: { search, category, sortBy },
      }),
      providesTags: ['Workflow'],
    }),

    getWorkflowTemplate: builder.query<WorkflowTemplate, string>({
      query: (id) => `/api/workflows/templates/${id}`,
      providesTags: (_result, _error, id) => [{ type: 'Workflow', id }],
    }),

    createWorkflowTemplate: builder.mutation<WorkflowTemplate, Partial<WorkflowTemplate>>({
      query: (template) => ({
        url: '/api/workflows/templates',
        method: 'POST',
        body: template,
      }),
      invalidatesTags: ['Workflow'],
    }),

    updateWorkflowTemplate: builder.mutation<WorkflowTemplate, { id: string; template: Partial<WorkflowTemplate> }>({
      query: ({ id, template }) => ({
        url: `/api/workflows/templates/${id}`,
        method: 'PUT',
        body: template,
      }),
      invalidatesTags: (_result, _error, { id }) => [{ type: 'Workflow', id }],
    }),

    deleteWorkflowTemplate: builder.mutation<void, string>({
      query: (id) => ({
        url: `/api/workflows/templates/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Workflow'],
    }),

    executeWorkflow: builder.mutation<{ executionId: string }, { templateId: string; context?: Record<string, any> }>({
      query: ({ templateId, context }) => ({
        url: `/api/workflows/templates/${templateId}/execute`,
        method: 'POST',
        body: { context },
      }),
      invalidatesTags: ['Workflow'],
    }),

    getWorkflowExecutions: builder.query<WorkflowExecution[], { status?: string; search?: string }>({
      query: ({ status, search } = {}) => ({
        url: '/api/workflows/executions',
        params: { status, search },
      }),
      providesTags: ['Workflow'],
    }),

    getWorkflowExecution: builder.query<WorkflowExecution, string>({
      query: (id) => `/api/workflows/executions/${id}`,
      providesTags: (_result, _error, id) => [{ type: 'Workflow', id }],
    }),

    controlWorkflowExecution: builder.mutation<void, { id: string; action: 'pause' | 'resume' | 'stop' | 'restart' }>({
      query: ({ id, action }) => ({
        url: `/api/workflows/executions/${id}/${action}`,
        method: 'POST',
      }),
      invalidatesTags: (_result, _error, { id }) => [{ type: 'Workflow', id }],
    }),

    getWorkflowMetrics: builder.query<WorkflowMetrics, void>({
      query: () => '/api/workflows/metrics',
      providesTags: ['Workflow'],
    }),

    validateWorkflow: builder.mutation<{ isValid: boolean; errors: string[] }, { nodes: any[]; edges: any[] }>({
      query: ({ nodes, edges }) => ({
        url: '/api/workflows/validate',
        method: 'POST',
        body: { nodes, edges },
      }),
    }),

    exportWorkflow: builder.mutation<Blob, string>({
      query: (id) => ({
        url: `/api/workflows/templates/${id}/export`,
        method: 'GET',
        responseHandler: (response) => response.blob(),
      }),
    }),

    importWorkflow: builder.mutation<WorkflowTemplate, FormData>({
      query: (formData) => ({
        url: '/api/workflows/templates/import',
        method: 'POST',
        body: formData,
      }),
      invalidatesTags: ['Workflow'],
    }),
  }),
});

export const {
  useGetWorkflowTemplatesQuery,
  useGetWorkflowTemplateQuery,
  useCreateWorkflowTemplateMutation,
  useUpdateWorkflowTemplateMutation,
  useDeleteWorkflowTemplateMutation,
  useExecuteWorkflowMutation,
  useGetWorkflowExecutionsQuery,
  useGetWorkflowExecutionQuery,
  useControlWorkflowExecutionMutation,
  useGetWorkflowMetricsQuery,
  useValidateWorkflowMutation,
  useExportWorkflowMutation,
  useImportWorkflowMutation,
} = workflowApi;
