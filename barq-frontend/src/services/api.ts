const API_BASE_URL = (import.meta as any).env.VITE_API_BASE_URL || 'https://barq-application-tunnel-mhso4pv2.devinapps.com';
const API_USERNAME = (import.meta as any).env.VITE_API_USERNAME || 'user';
const API_PASSWORD = (import.meta as any).env.VITE_API_PASSWORD || '288284fe24969eaf87e98a985800230b';
const API_TIMEOUT = parseInt((import.meta as any).env.VITE_API_TIMEOUT || '30000');

class ApiService {
  private baseUrl: string;
  private timeout: number;

  constructor() {
    this.baseUrl = API_BASE_URL;
    this.timeout = API_TIMEOUT;
  }

  private async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<T> {
    const url = `${this.baseUrl}${endpoint}`;
    
    const authHeader = btoa(`${API_USERNAME}:${API_PASSWORD}`);
    
    const config: RequestInit = {
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Basic ${authHeader}`,
        ...options.headers,
      },
      ...options,
    };

    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), this.timeout);

    try {
      const response = await fetch(url, {
        ...config,
        signal: controller.signal,
      });

      clearTimeout(timeoutId);

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      return await response.json();
    } catch (error) {
      clearTimeout(timeoutId);
      throw error;
    }
  }

  async get<T>(endpoint: string): Promise<T> {
    return this.request<T>(endpoint, { method: 'GET' });
  }

  async post<T>(endpoint: string, data?: any): Promise<T> {
    return this.request<T>(endpoint, {
      method: 'POST',
      body: data ? JSON.stringify(data) : undefined,
    });
  }

  async put<T>(endpoint: string, data?: any): Promise<T> {
    return this.request<T>(endpoint, {
      method: 'PUT',
      body: data ? JSON.stringify(data) : undefined,
    });
  }

  async delete<T>(endpoint: string): Promise<T> {
    return this.request<T>(endpoint, { method: 'DELETE' });
  }
}

export const apiService = new ApiService();

export const workflowApi = {
  getWorkflowTemplates: () => apiService.get<{ success: boolean; data: any[] }>('/api/workflow/templates'),
  createWorkflowTemplate: (data: any) => apiService.post<{ success: boolean; data: any }>('/api/workflow/templates', data),
  updateWorkflowTemplate: (id: string, data: any) => apiService.put<{ success: boolean; data: any }>(`/api/workflow/templates/${id}`, data),
  
  getWorkflowInstances: () => apiService.get<{ success: boolean; data: any[] }>('/api/workflows/instances'),
  getWorkflowById: (id: string) => apiService.get<{ success: boolean; data: any }>(`/api/workflows/${id}`),
  createWorkflow: (data: any) => apiService.post<{ success: boolean; data: any }>('/api/workflow', data),
  
  startWorkflow: (workflowId: string, data: any) => apiService.post<{ success: boolean; data: any }>(`/api/workflow/${workflowId}/start`, data),
  pauseWorkflow: (workflowId: string) => apiService.post<{ success: boolean; message: string }>(`/api/workflow/${workflowId}/pause`, {}),
  resumeWorkflow: (workflowId: string) => apiService.post<{ success: boolean; message: string }>(`/api/workflow/${workflowId}/resume`, {}),
  cancelWorkflow: (workflowId: string, reason: string) => apiService.post<{ success: boolean; message: string }>(`/api/workflow/${workflowId}/cancel?reason=${encodeURIComponent(reason)}`, {}),
  
  getWorkflowStatus: (workflowId: string) => apiService.get<{ success: boolean; data: any }>(`/api/workflow/${workflowId}/status`),
  getWorkflowHistory: (workflowId: string) => apiService.get<{ success: boolean; data: any }>(`/api/workflow/${workflowId}/history`),
  
  approveWorkflow: (data: { workflowId: string; approverId: string; comments: string }) => 
    apiService.post<{ success: boolean; data: any }>('/api/workflow/approve', data),
  rejectWorkflow: (data: { workflowId: string; reviewerId: string; reason: string }) => 
    apiService.post<{ success: boolean; data: any }>('/api/workflow/reject', data),
  
  getProjectWorkflows: (projectId: string) => apiService.get<{ success: boolean; data: any[] }>(`/api/workflow/project/${projectId}`),
  
  getPendingApprovals: (userId: string) => apiService.get<{ success: boolean; data: any[] }>(`/api/workflow/pending-approvals/${userId}`),
  
  getWorkflowAnalytics: (params?: { projectId?: string; startDate?: string; endDate?: string }) => {
    const queryParams = new URLSearchParams()
    if (params?.projectId) queryParams.append('projectId', params.projectId)
    if (params?.startDate) queryParams.append('startDate', params.startDate)
    if (params?.endDate) queryParams.append('endDate', params.endDate)
    return apiService.get<{ success: boolean; data: any }>(`/api/workflow/analytics?${queryParams.toString()}`)
  },
  getWorkflowPerformance: (params?: { projectId?: string; startDate?: string; endDate?: string }) => {
    const queryParams = new URLSearchParams()
    if (params?.projectId) queryParams.append('projectId', params.projectId)
    if (params?.startDate) queryParams.append('startDate', params.startDate)
    if (params?.endDate) queryParams.append('endDate', params.endDate)
    return apiService.get<{ success: boolean; data: any }>(`/api/workflow/performance?${queryParams.toString()}`)
  },
  getSlaBreaches: (projectId?: string) => {
    const queryParams = projectId ? `?projectId=${projectId}` : ''
    return apiService.get<{ success: boolean; data: any }>(`/api/workflow/sla-breaches${queryParams}`)
  }
};

export const aiRequestApi = {
  getRequests: () => apiService.get<{ data: any[] }>('/api/ai-requests'),
  createRequest: (data: any) => apiService.post('/api/ai-requests', data),
  getRequestById: (id: string) => apiService.get(`/api/ai-requests/${id}`),
  updateRequest: (id: string, data: any) => apiService.put(`/api/ai-requests/${id}`, data),
  deleteRequest: (id: string) => apiService.delete(`/api/ai-requests/${id}`),
  approveRequest: (id: string) => apiService.post(`/api/ai-requests/${id}/approve`),
  rejectRequest: (id: string, reason: string) => apiService.post(`/api/ai-requests/${id}/reject`, { reason })
};

export const qualityAssuranceApi = {
  getAssessments: () => apiService.get<{ data: any[] }>('/api/quality-assurance/assessments'),
  createAssessment: (data: any) => apiService.post('/api/quality-assurance/assessments', data),
  getAssessmentById: (id: string) => apiService.get(`/api/quality-assurance/assessments/${id}`),
  completeAssessment: (id: string, data: any) => apiService.put(`/api/quality-assurance/assessments/${id}/complete`, data),
  updateAssessment: (id: string, data: any) => apiService.put(`/api/quality-assurance/assessments/${id}`, data),
  deleteAssessment: (id: string) => apiService.delete(`/api/quality-assurance/assessments/${id}`)
};

export const devinApi = {
  createChatSession: (data: { aiProvider: string; title?: string; context?: any }) => 
    apiService.post<{ success: boolean; data: { sessionId: string } }>('/api/aitask', {
      Title: data.title || `Chat with ${data.aiProvider}`,
      Description: `Interactive chat session with ${data.aiProvider}`,
      TaskType: 2, // CodeGeneration
      Priority: 1, // Medium
      InputData: JSON.stringify({ aiProvider: data.aiProvider, context: data.context }),
      Parameters: {
        sessionType: 'chat',
        interactive: true,
        aiProvider: data.aiProvider
      },
      AssignedToUserId: '00000000-0000-0000-0000-000000000000'
    }),

  sendMessage: (sessionId: string, message: string, attachments?: string[]) =>
    apiService.post<{ success: boolean; data: any }>(`/api/aitask/${sessionId}/execute`, {
      message,
      attachments: attachments || []
    }),

  getTaskResults: (taskId: string) =>
    apiService.get<{ success: boolean; data: { status: string; result: string; progress: number } }>(`/api/aitask/${taskId}/results`),

  getTaskStatus: (taskId: string) =>
    apiService.get<{ success: boolean; data: { status: string; progress: number } }>(`/api/aitask/${taskId}/status`),

  cancelTask: (taskId: string) =>
    apiService.post<{ success: boolean; message: string }>(`/api/aitask/${taskId}/cancel`, {}),

  uploadFile: (file: File, context?: any) => {
    const formData = new FormData();
    formData.append('file', file);
    if (context) {
      formData.append('context', JSON.stringify(context));
    }
    
    return fetch(`${API_BASE_URL}/api/files/upload`, {
      method: 'POST',
      headers: {
        'Authorization': `Basic ${btoa(`${API_USERNAME}:${API_PASSWORD}`)}`,
      },
      body: formData
    }).then(response => response.json());
  },

  processDocument: (fileId: string, processingType: 'analysis' | 'extraction' | 'generation') =>
    apiService.post<{ success: boolean; data: any }>('/api/documents/process', {
      fileId,
      processingType
    }),

  getAvailableProviders: () =>
    apiService.get<{ success: boolean; data: Array<{ id: string; name: string; type: string; capabilities: string[]; isEnabled: boolean }> }>('/api/aitask/providers'),

  getProviderHealth: (providerId: string) =>
    apiService.get<{ success: boolean; data: { status: string; responseTime: number; lastChecked: string } }>(`/api/aitask/providers/${providerId}/health`),

  generateCode: (data: {
    prompt: string;
    language?: string;
    framework?: string;
    requirements?: string[];
    files?: string[];
    aiProvider?: string;
  }) => apiService.post<{ success: boolean; data: { taskId: string; code?: string; files?: any[] } }>('/api/aitask', {
    Title: 'Code Generation Request',
    Description: data.prompt,
    TaskType: 2, // CodeGeneration
    Priority: 1, // Medium
    InputData: JSON.stringify({
      prompt: data.prompt,
      language: data.language,
      framework: data.framework,
      requirements: data.requirements,
      files: data.files,
      aiProvider: data.aiProvider || 'DevinAI'
    }),
    Parameters: {
      taskType: 'codeGeneration',
      language: data.language,
      framework: data.framework
    },
    AssignedToUserId: '00000000-0000-0000-0000-000000000000'
  }),

  generateBRD: (data: {
    requirements: string;
    businessContext?: string;
    stakeholders?: string[];
    files?: string[];
    template?: string;
  }) => apiService.post<{ success: boolean; data: { taskId: string; document?: string } }>('/api/aitask', {
    Title: 'Business Requirements Document Generation',
    Description: data.requirements,
    TaskType: 0, // RequirementsAnalysis
    Priority: 1, // Medium
    InputData: JSON.stringify({
      requirements: data.requirements,
      businessContext: data.businessContext,
      stakeholders: data.stakeholders,
      files: data.files,
      template: data.template,
      aiProvider: 'ManusAI'
    }),
    Parameters: {
      taskType: 'brdGeneration',
      template: data.template,
      outputFormat: 'pdf'
    },
    AssignedToUserId: '00000000-0000-0000-0000-000000000000'
  }),

  getTaskAnalytics: (params?: { projectId?: string; startDate?: string; endDate?: string }) => {
    const queryParams = new URLSearchParams();
    if (params?.projectId) queryParams.append('projectId', params.projectId);
    if (params?.startDate) queryParams.append('startDate', params.startDate);
    if (params?.endDate) queryParams.append('endDate', params.endDate);
    return apiService.get<{ success: boolean; data: any }>(`/api/aitask/analytics?${queryParams.toString()}`);
  },

  getCostAnalysis: (params?: { projectId?: string; startDate?: string; endDate?: string }) => {
    const queryParams = new URLSearchParams();
    if (params?.projectId) queryParams.append('projectId', params.projectId);
    if (params?.startDate) queryParams.append('startDate', params.startDate);
    if (params?.endDate) queryParams.append('endDate', params.endDate);
    return apiService.get<{ success: boolean; data: any }>(`/api/aitask/costs?${queryParams.toString()}`);
  },

  getQueueStatus: () =>
    apiService.get<{ success: boolean; data: { queuedTasks: number; processingTasks: number; averageWaitTime: number } }>('/api/aitask/queue/status'),

  executeBatchTasks: (tasks: Array<{ title: string; description: string; taskType: number; inputData: any }>) =>
    apiService.post<{ success: boolean; data: { batchId: string; totalTasks: number } }>('/api/aitask/batch', { tasks })
};
