export interface AIRequest {
  id: string;
  title: string;
  description: string;
  requestType: AIRequestType;
  priority: AIRequestPriority;
  status: AIRequestStatus;
  requestData: string;
  requesterId: string;
  assignedToId?: string;
  dueDate?: string;
  completedAt?: string;
  completionNotes?: string;
  createdAt: string;
  updatedAt: string;
  workflowInstanceId?: string;
}

export interface AIRequestApproval {
  id: string;
  aiRequestId: string;
  approverId: string;
  approverLevel: number;
  status: ApprovalStatus;
  comments?: string;
  approvedAt?: string;
  createdAt: string;
  updatedAt: string;
}

export interface QualityAssessment {
  id: string;
  aiRequestId: string;
  assessorId: string;
  type: QualityAssessmentType;
  status: QualityAssessmentStatus;
  qualityScore: number;
  comments?: string;
  recommendations?: string;
  completedAt?: string;
  qualityCriteria: string;
  assessmentResults: string;
  requiresReview: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface WorkflowInstance {
  id: string;
  templateId: string;
  status: WorkflowStatus;
  currentStepId?: string;
  initiatorId: string;
  workflowData?: string;
  startedAt: string;
  completedAt?: string;
  createdAt: string;
  updatedAt: string;
}

export enum AIRequestType {
  TextGeneration = 0,
  ImageGeneration = 1,
  DataAnalysis = 2,
  CodeGeneration = 3,
  Translation = 4,
  Summarization = 5,
  Classification = 6,
  Other = 7
}

export enum AIRequestPriority {
  Low = 0,
  Normal = 1,
  High = 2,
  Critical = 3
}

export enum AIRequestStatus {
  Draft = 0,
  Submitted = 1,
  UnderReview = 2,
  Approved = 3,
  Rejected = 4,
  InProgress = 5,
  Completed = 6,
  Cancelled = 7,
  QualityReview = 8
}

export enum ApprovalStatus {
  Pending = 0,
  Approved = 1,
  Rejected = 2,
  Delegated = 3
}

export enum QualityAssessmentType {
  Automated = 0,
  Manual = 1,
  Peer = 2,
  Expert = 3
}

export enum QualityAssessmentStatus {
  Pending = 0,
  InProgress = 1,
  Completed = 2,
  Approved = 3,
  Rejected = 4,
  RequiresReview = 5
}

export enum WorkflowStatus {
  NotStarted = 0,
  Running = 1,
  Suspended = 2,
  Completed = 3,
  Failed = 4,
  Cancelled = 5
}

export interface CreateAIRequestRequest {
  title: string;
  description: string;
  requestType: AIRequestType;
  priority: AIRequestPriority;
  requestData: string;
  dueDate?: string;
}

export interface UpdateAIRequestRequest {
  title?: string;
  description?: string;
  priority?: AIRequestPriority;
  requestData?: string;
  dueDate?: string;
  assignedToId?: string;
}

export interface CreateQualityAssessmentRequest {
  aiRequestId: string;
  assessorId: string;
  type: QualityAssessmentType;
  qualityCriteria: string;
}

export interface CompleteQualityAssessmentRequest {
  qualityScore: number;
  comments: string;
  recommendations: string;
  assessmentResults: string;
  requiresReview: boolean;
  status: QualityAssessmentStatus;
}
