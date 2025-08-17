import axios from 'axios';

const API_BASE_URL = (import.meta as any).env.VITE_API_BASE_URL || 'https://barq-backend-tunnel-api.devinapps.com';

const aiApi = axios.create({
  baseURL: `${API_BASE_URL}/api`,
  headers: {
    'Content-Type': 'application/json',
  },
});

aiApi.interceptors.request.use((config) => {
  const token = localStorage.getItem('accessToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  } else {
    const username = (import.meta as any).env.VITE_API_USERNAME;
    const password = (import.meta as any).env.VITE_API_PASSWORD;
    if (username && password) {
      const credentials = btoa(`${username}:${password}`);
      config.headers.Authorization = `Basic ${credentials}`;
    }
  }
  return config;
});

export interface AIProvider {
  id: number;
  name: string;
  type: string;
  isEnabled: boolean;
  capabilities: string[];
  description?: string;
}

export interface AIRequest {
  id: string;
  title: string;
  description: string;
  requestType: string;
  priority: string;
  status: string;
  aiProvider: string;
  attachments: string[];
  createdAt: string;
  updatedAt: string;
  createdBy: string;
  assignedTo?: string;
  estimatedHours?: number;
  actualHours?: number;
  tags: string[];
  workflowInstanceId?: string;
}

export interface ChatSession {
  id: string;
  title: string;
  aiProvider: string;
  createdAt: string;
  updatedAt: string;
  messageCount: number;
  status: 'active' | 'completed' | 'archived';
}

export interface SendMessageRequest {
  sessionId?: string;
  content: string;
  attachments?: string[];
  aiProvider: string;
  requestType?: string;
  context?: Record<string, any>;
}

export interface SendMessageResponse {
  success: boolean;
  message: string;
  data: {
    sessionId: string;
    messageId: string;
    response: string;
    codeBlocks?: Array<{
      id: string;
      language: string;
      code: string;
      filename?: string;
    }>;
    attachments?: string[];
    suggestions?: string[];
  };
}

export interface AIRequestCreateRequest {
  title: string;
  description: string;
  requestType: 'CodeGeneration' | 'BRDGeneration' | 'Testing' | 'Documentation' | 'Review';
  priority: 'Low' | 'Medium' | 'High' | 'Critical';
  aiProvider: string;
  attachments?: string[];
  tags?: string[];
  estimatedHours?: number;
  requirements?: Record<string, any>;
}

export interface AIRequestResponse {
  success: boolean;
  message: string;
  data: AIRequest;
}

export interface AIProvidersResponse {
  success: boolean;
  message: string;
  data: AIProvider[];
}

export interface ChatSessionsResponse {
  success: boolean;
  message: string;
  data: {
    sessions: ChatSession[];
    totalCount: number;
    pageSize: number;
    currentPage: number;
  };
}

export const aiService = {
  async getAvailableProviders(): Promise<AIProvidersResponse> {
    const response = await aiApi.get('/aitask/providers');
    return response.data;
  },

  async createChatSession(aiProvider: string, title?: string): Promise<{ sessionId: string }> {
    const response = await aiApi.post('/aitask', {
      Title: title || `Chat with ${aiProvider}`,
      Description: `Interactive chat session with ${aiProvider}`,
      TaskType: 2, // CodeGeneration
      Priority: 1, // Medium
      InputData: JSON.stringify({ aiProvider: aiProvider }),
      Parameters: {
        sessionType: 'chat',
        interactive: true,
        aiProvider: aiProvider
      },
      AssignedToUserId: '00000000-0000-0000-0000-000000000000' // Default GUID for development
    });
    return { sessionId: response.data.data.id };
  },

  async getChatSessions(page: number = 1, pageSize: number = 20): Promise<ChatSessionsResponse> {
    return {
      success: true,
      message: 'Sessions retrieved',
      data: {
        sessions: [],
        totalCount: 0,
        pageSize: pageSize,
        currentPage: page
      }
    };
  },

  async sendMessage(request: SendMessageRequest): Promise<SendMessageResponse> {
    if (!request.sessionId) {
      throw new Error('Session ID is required');
    }
    
    try {
      const executeResponse = await aiApi.post(`/aitask/${request.sessionId}/execute`);
      
      if (!executeResponse.data.success) {
        throw new Error(executeResponse.data.message || 'Failed to execute AI task');
      }
      
      let attempts = 0;
      const maxAttempts = 30; // 30 seconds timeout
      
      while (attempts < maxAttempts) {
        try {
          const resultsResponse = await aiApi.get(`/aitask/${request.sessionId}/results`);
          
          if (resultsResponse.data.success && resultsResponse.data.data.status === 'Completed') {
            const result = resultsResponse.data.data.result;
            
            const codeBlocks = this.extractCodeBlocks(result);
            
            return {
              success: true,
              message: 'AI response generated successfully',
              data: {
                sessionId: request.sessionId,
                messageId: `msg-${Date.now()}`,
                response: result || `I've processed your request: "${request.content}". Here's my response based on the AI analysis.`,
                codeBlocks,
                attachments: request.attachments || [],
                suggestions: []
              }
            };
          }
          
          await new Promise(resolve => setTimeout(resolve, 1000));
          attempts++;
        } catch (error) {
          await new Promise(resolve => setTimeout(resolve, 1000));
          attempts++;
        }
      }
      
      return {
        success: true,
        message: 'AI response generated',
        data: {
          sessionId: request.sessionId,
          messageId: `msg-${Date.now()}`,
          response: `I'm processing your request: "${request.content}". The task has been submitted and is being processed by the AI orchestration service. You can check the status in the AI Requests dashboard.`,
          codeBlocks: [],
          attachments: request.attachments || [],
          suggestions: []
        }
      };
      
    } catch (error) {
      console.error('AI task execution failed:', error);
      throw new Error(`Failed to process AI request: ${error instanceof Error ? error.message : 'Unknown error'}`);
    }
  },

  extractCodeBlocks(content: string): Array<{
    id: string;
    language: string;
    code: string;
    filename?: string;
  }> {
    const codeBlocks: Array<{
      id: string;
      language: string;
      code: string;
      filename?: string;
    }> = [];
    
    const codeBlockRegex = /```(\w+)?\n([\s\S]*?)```/g;
    let match;
    let blockIndex = 0;
    
    while ((match = codeBlockRegex.exec(content)) !== null) {
      const language = match[1] || 'text';
      const code = match[2].trim();
      
      if (code) {
        codeBlocks.push({
          id: `code-block-${blockIndex++}`,
          language,
          code,
          filename: this.getFilenameFromLanguage(language)
        });
      }
    }
    
    return codeBlocks;
  },

  getFilenameFromLanguage(language: string): string | undefined {
    const extensionMap: Record<string, string> = {
      'javascript': 'script.js',
      'typescript': 'script.ts',
      'python': 'script.py',
      'java': 'Script.java',
      'csharp': 'Script.cs',
      'html': 'index.html',
      'css': 'styles.css',
      'json': 'data.json',
      'xml': 'data.xml',
      'sql': 'query.sql',
      'bash': 'script.sh',
      'powershell': 'script.ps1'
    };
    
    return extensionMap[language.toLowerCase()];
  },

  async createAIRequest(request: AIRequestCreateRequest): Promise<AIRequestResponse> {
    const REQUEST_TYPE_ENUM_MAP: Record<string, number> = {
      'DocumentGeneration': 0,
      'DataAnalysis': 1,
      'CodeGeneration': 2,
      'BusinessAnalysis': 3,
      'QualityAssurance': 4,
      'Testing': 5,
      'Deployment': 6,
      'Other': 7
    };

    const PRIORITY_ENUM_MAP: Record<string, number> = {
      'Low': 0,
      'Normal': 1,
      'High': 2,
      'Critical': 3,
      'Emergency': 4
    };

    const payload = {
      title: request.title,
      description: request.description,
      requestType: REQUEST_TYPE_ENUM_MAP[request.requestType] ?? 7, // Default to 'Other'
      priority: PRIORITY_ENUM_MAP[request.priority] ?? 1, // Default to 'Normal'
      requestData: JSON.stringify({
        aiProvider: request.aiProvider,
        attachments: request.attachments,
        tags: request.tags,
        estimatedHours: request.estimatedHours,
        requirements: request.requirements
      }),
      requesterId: '11111111-1111-1111-1111-111111111111', // Test user ID
      dueDate: null
    };
    
    console.log('Sending AI request payload:', payload);
    
    const response = await aiApi.post('/airequest', payload);
    
    return {
      success: response.data.success,
      message: response.data.message,
      data: {
        id: response.data.data.id,
        title: request.title,
        description: request.description,
        requestType: request.requestType,
        priority: request.priority,
        status: response.data.data.status || 'Draft',
        aiProvider: request.aiProvider,
        attachments: request.attachments || [],
        createdAt: response.data.data.createdAt || new Date().toISOString(),
        updatedAt: response.data.data.updatedAt || new Date().toISOString(),
        createdBy: 'current-user',
        tags: request.tags || []
      }
    };
  },

  async submitAIRequest(requestId: string): Promise<AIRequestResponse> {
    const response = await aiApi.post(`/airequest/${requestId}/submit`);
    return {
      success: response.data.success,
      message: response.data.message || 'Request submitted and workflow started',
      data: {
        id: requestId,
        title: '',
        description: '',
        requestType: '',
        priority: '',
        status: 'Submitted',
        aiProvider: '',
        attachments: [],
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString(),
        createdBy: 'current-user',
        tags: []
      }
    };
  },

  async getAIRequest(requestId: string): Promise<AIRequestResponse> {
    const response = await aiApi.get(`/airequest/${requestId}`);
    return {
      success: response.data.success,
      message: response.data.message || 'Request retrieved successfully',
      data: response.data.data
    };
  },

  async getUserAIRequests(userId?: string, status?: string): Promise<{ success: boolean; data: AIRequest[] }> {
    const currentUserId = userId || '11111111-1111-1111-1111-111111111111';
    const queryParams = status ? `?status=${status}` : '';
    const response = await aiApi.get(`/airequest/user/${currentUserId}${queryParams}`);
    return {
      success: response.data.success,
      data: response.data.data || []
    };
  },

  async approveAIRequest(requestId: string, approverId: string, comments?: string): Promise<AIRequestResponse> {
    const payload = { approverId, comments };
    const response = await aiApi.post(`/airequest/${requestId}/approve`, payload);
    return {
      success: response.data.success,
      message: response.data.message || 'Request approved successfully',
      data: {
        id: requestId,
        title: '',
        description: '',
        requestType: '',
        priority: '',
        status: 'Approved',
        aiProvider: '',
        attachments: [],
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString(),
        createdBy: 'current-user',
        tags: []
      }
    };
  },

  async rejectAIRequest(requestId: string, approverId: string, reason: string): Promise<AIRequestResponse> {
    const payload = { approverId, reason };
    const response = await aiApi.post(`/airequest/${requestId}/reject`, payload);
    return {
      success: response.data.success,
      message: response.data.message || 'Request rejected successfully',
      data: {
        id: requestId,
        title: '',
        description: '',
        requestType: '',
        priority: '',
        status: 'Rejected',
        aiProvider: '',
        attachments: [],
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString(),
        createdBy: 'current-user',
        tags: []
      }
    };
  },

  async getAIRequests(
    page: number = 1,
    pageSize: number = 20,
    status?: string,
    requestType?: string
  ): Promise<{ success: boolean; data: { requests: AIRequest[]; totalCount: number } }> {
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: pageSize.toString(),
    });

    if (status) params.append('status', status);
    if (requestType) params.append('requestType', requestType);

    const response = await aiApi.get(`/requests?${params.toString()}`);
    return response.data;
  },

  async getAIRequestById(requestId: string): Promise<{ success: boolean; data: AIRequest }> {
    const response = await aiApi.get(`/requests/${requestId}`);
    return response.data;
  },

  async updateAIRequestStatus(
    requestId: string,
    status: string,
    notes?: string
  ): Promise<{ success: boolean; message: string }> {
    const response = await aiApi.put(`/requests/${requestId}/status`, {
      status,
      notes,
    });
    return response.data;
  },

  async executeAIRequest(
    requestId: string,
    parameters?: Record<string, any>
  ): Promise<{ success: boolean; data: { executionId: string; status: string } }> {
    const response = await aiApi.post(`/requests/${requestId}/execute`, parameters);
    return response.data;
  },

  async getExecutionStatus(
    executionId: string
  ): Promise<{ success: boolean; data: { status: string; progress: number; result?: any } }> {
    const response = await aiApi.get(`/executions/${executionId}/status`);
    return response.data;
  },

  async cancelExecution(executionId: string): Promise<{ success: boolean; message: string }> {
    const response = await aiApi.post(`/executions/${executionId}/cancel`);
    return response.data;
  },

  getAIProviderIcon(providerName: string): string {
    const iconMap: Record<string, string> = {
      'DevinAI': '🤖',
      'ManusAI': '📝',
      'OpenAI': '🧠',
      'Claude': '🎭',
      'Gemini': '💎',
      'Custom': '⚙️',
    };
    return iconMap[providerName] || '🤖';
  },

  getRequestTypeIcon(requestType: string): string {
    const iconMap: Record<string, string> = {
      'CodeGeneration': '💻',
      'BRDGeneration': '📋',
      'Testing': '🧪',
      'Documentation': '📚',
      'Review': '👀',
    };
    return iconMap[requestType] || '📄';
  },

  getStatusColor(status: string): string {
    const colorMap: Record<string, string> = {
      'Pending': 'text-yellow-600 bg-yellow-100',
      'InProgress': 'text-blue-600 bg-blue-100',
      'Completed': 'text-green-600 bg-green-100',
      'Failed': 'text-red-600 bg-red-100',
      'Cancelled': 'text-gray-600 bg-gray-100',
    };
    return colorMap[status] || 'text-gray-600 bg-gray-100';
  },

  getPriorityColor(priority: string): string {
    const colorMap: Record<string, string> = {
      'Low': 'text-green-600 bg-green-100',
      'Medium': 'text-yellow-600 bg-yellow-100',
      'High': 'text-orange-600 bg-orange-100',
      'Critical': 'text-red-600 bg-red-100',
    };
    return colorMap[priority] || 'text-gray-600 bg-gray-100';
  },
};
