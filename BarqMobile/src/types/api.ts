export interface AIRequest {
  id: string;
  title: string;
  description: string;
  type: AIRequestType;
  priority: Priority;
  status: AIRequestStatus;
  requesterId: string;
  requesterName: string;
  createdAt: string;
  updatedAt: string;
  dueDate?: string;
  estimatedCompletionTime?: number;
  actualCompletionTime?: number;
  tags: string[];
  metadata: Record<string, any>;
}

export interface WorkflowInstance {
  id: string;
  templateId: string;
  templateName: string;
  status: WorkflowStatus;
  progress: number;
  startedAt: string;
  completedAt?: string;
  duration?: number;
  currentStep?: string;
  assignedTo?: string;
  priority: Priority;
  metadata: Record<string, any>;
}

export interface QualityAssessment {
  id: string;
  aiRequestId: string;
  assessorId: string;
  assessorName: string;
  type: QualityAssessmentType;
  status: QualityAssessmentStatus;
  score?: number;
  feedback?: string;
  criteria: QualityCriteria[];
  createdAt: string;
  completedAt?: string;
  dueDate?: string;
}

export interface QualityCriteria {
  id: string;
  name: string;
  description: string;
  weight: number;
  score?: number;
  comments?: string;
}

export interface DashboardStats {
  aiRequests: {
    total: number;
    pending: number;
    approved: number;
    rejected: number;
    inProgress: number;
  };
  workflows: {
    active: number;
    completed: number;
    failed: number;
  };
  qualityAssessments: {
    pending: number;
    completed: number;
    averageScore: number;
  };
}

export type AIRequestType = 
  | 'text-generation'
  | 'image-generation'
  | 'data-analysis'
  | 'code-generation'
  | 'translation'
  | 'summarization'
  | 'classification';

export type Priority = 'low' | 'normal' | 'high' | 'critical';

export type AIRequestStatus = 
  | 'draft'
  | 'submitted'
  | 'under-review'
  | 'approved'
  | 'rejected'
  | 'in-progress'
  | 'completed'
  | 'cancelled';

export type WorkflowStatus = 
  | 'active'
  | 'suspended'
  | 'completed'
  | 'terminated'
  | 'failed';

export type QualityAssessmentType = 
  | 'automated'
  | 'manual'
  | 'peer-review'
  | 'expert-review';

export type QualityAssessmentStatus = 
  | 'pending'
  | 'in-progress'
  | 'completed'
  | 'overdue';

export interface CreateAIRequestRequest {
  title: string;
  description: string;
  type: AIRequestType;
  priority: Priority;
  dueDate?: string;
  tags: string[];
  metadata: Record<string, any>;
}

export interface UpdateAIRequestRequest {
  title?: string;
  description?: string;
  priority?: Priority;
  dueDate?: string;
  tags?: string[];
  metadata?: Record<string, any>;
}

export interface CompleteQualityAssessmentRequest {
  score: number;
  feedback: string;
  criteria: {
    id: string;
    score: number;
    comments?: string;
  }[];
}

export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
  errors?: string[];
}

export interface PaginatedResponse<T> {
  data: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}
