import {
  AIRequest,
  WorkflowInstance,
  QualityAssessment,
  DashboardStats,
  CreateAIRequestRequest,
  UpdateAIRequestRequest,
  CompleteQualityAssessmentRequest,
  ApiResponse,
  PaginatedResponse,
} from '../types/api';

const API_BASE_URL = 'http://localhost:5000/api';

class ApiService {
  private async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<ApiResponse<T>> {
    const url = `${API_BASE_URL}${endpoint}`;
    
    const defaultHeaders = {
      'Content-Type': 'application/json',
    };

    const config: RequestInit = {
      ...options,
      headers: {
        ...defaultHeaders,
        ...options.headers,
      },
    };

    try {
      const response = await fetch(url, config);
      const data = await response.json();

      if (!response.ok) {
        throw new Error(data.message || `HTTP error! status: ${response.status}`);
      }

      return data;
    } catch (error) {
      console.error('API request failed:', error);
      throw error;
    }
  }

  async getDashboardStats(): Promise<ApiResponse<DashboardStats>> {
    return this.request<DashboardStats>('/dashboard/stats');
  }

  async getAIRequests(
    page: number = 1,
    pageSize: number = 10,
    search?: string,
    status?: string,
    priority?: string
  ): Promise<ApiResponse<PaginatedResponse<AIRequest>>> {
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: pageSize.toString(),
    });

    if (search) params.append('search', search);
    if (status) params.append('status', status);
    if (priority) params.append('priority', priority);

    return this.request<PaginatedResponse<AIRequest>>(`/ai-requests?${params}`);
  }

  async getAIRequest(id: string): Promise<ApiResponse<AIRequest>> {
    return this.request<AIRequest>(`/ai-requests/${id}`);
  }

  async createAIRequest(request: CreateAIRequestRequest): Promise<ApiResponse<AIRequest>> {
    return this.request<AIRequest>('/ai-requests', {
      method: 'POST',
      body: JSON.stringify(request),
    });
  }

  async updateAIRequest(id: string, request: UpdateAIRequestRequest): Promise<ApiResponse<AIRequest>> {
    return this.request<AIRequest>(`/ai-requests/${id}`, {
      method: 'PUT',
      body: JSON.stringify(request),
    });
  }

  async deleteAIRequest(id: string): Promise<ApiResponse<void>> {
    return this.request<void>(`/ai-requests/${id}`, {
      method: 'DELETE',
    });
  }

  async getWorkflows(
    page: number = 1,
    pageSize: number = 10,
    search?: string,
    status?: string,
    templateType?: string
  ): Promise<ApiResponse<PaginatedResponse<WorkflowInstance>>> {
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: pageSize.toString(),
    });

    if (search) params.append('search', search);
    if (status) params.append('status', status);
    if (templateType) params.append('templateType', templateType);

    return this.request<PaginatedResponse<WorkflowInstance>>(`/workflows?${params}`);
  }

  async getWorkflow(id: string): Promise<ApiResponse<WorkflowInstance>> {
    return this.request<WorkflowInstance>(`/workflows/${id}`);
  }

  async startWorkflow(templateId: string, variables?: Record<string, any>): Promise<ApiResponse<WorkflowInstance>> {
    return this.request<WorkflowInstance>('/workflows/start', {
      method: 'POST',
      body: JSON.stringify({ templateId, variables }),
    });
  }

  async pauseWorkflow(id: string): Promise<ApiResponse<void>> {
    return this.request<void>(`/workflows/${id}/pause`, {
      method: 'POST',
    });
  }

  async resumeWorkflow(id: string): Promise<ApiResponse<void>> {
    return this.request<void>(`/workflows/${id}/resume`, {
      method: 'POST',
    });
  }

  async cancelWorkflow(id: string): Promise<ApiResponse<void>> {
    return this.request<void>(`/workflows/${id}/cancel`, {
      method: 'POST',
    });
  }

  async getQualityAssessments(
    page: number = 1,
    pageSize: number = 10,
    status?: string,
    type?: string
  ): Promise<ApiResponse<PaginatedResponse<QualityAssessment>>> {
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: pageSize.toString(),
    });

    if (status) params.append('status', status);
    if (type) params.append('type', type);

    return this.request<PaginatedResponse<QualityAssessment>>(`/quality-assessments?${params}`);
  }

  async getQualityAssessment(id: string): Promise<ApiResponse<QualityAssessment>> {
    return this.request<QualityAssessment>(`/quality-assessments/${id}`);
  }

  async completeQualityAssessment(
    id: string,
    request: CompleteQualityAssessmentRequest
  ): Promise<ApiResponse<QualityAssessment>> {
    return this.request<QualityAssessment>(`/quality-assessments/${id}/complete`, {
      method: 'POST',
      body: JSON.stringify(request),
    });
  }
}

export const apiService = new ApiService();
export default apiService;
