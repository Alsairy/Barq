const API_BASE_URL = (import.meta as any).env.VITE_API_BASE_URL || 'http://localhost:5000';
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
    
    const config: RequestInit = {
      headers: {
        'Content-Type': 'application/json',
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
  getWorkflowTemplates: () => apiService.get<{ data: any[] }>('/api/workflows/templates'),
  getWorkflowInstances: () => apiService.get<{ data: any[] }>('/api/workflows/instances'),
  startWorkflow: (workflowId: string, data: any) => apiService.post(`/api/workflows/${workflowId}/start`, data),
  getWorkflowById: (id: string) => apiService.get(`/api/workflows/${id}`),
  updateWorkflow: (id: string, data: any) => apiService.put(`/api/workflows/${id}`, data),
  deleteWorkflow: (id: string) => apiService.delete(`/api/workflows/${id}`)
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
