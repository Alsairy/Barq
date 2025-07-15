import { apiSlice } from './apiSlice';

export interface Project {
  id: string;
  name: string;
  description: string;
  status: 'planning' | 'active' | 'on-hold' | 'completed' | 'cancelled';
  priority: 'low' | 'medium' | 'high' | 'critical';
  startDate: string;
  endDate?: string;
  budget?: number;
  progress: number;
  ownerId: string;
  ownerName: string;
  organizationId: string;
  tags: string[];
  createdAt: string;
  updatedAt: string;
  members: ProjectMember[];
  tasks: Task[];
  metrics: ProjectMetrics;
}

export interface ProjectMember {
  id: string;
  userId: string;
  userName: string;
  userEmail: string;
  role: 'owner' | 'admin' | 'member' | 'viewer';
  joinedAt: string;
  permissions: string[];
}

export interface Task {
  id: string;
  title: string;
  description: string;
  status: 'todo' | 'in-progress' | 'review' | 'done';
  priority: 'low' | 'medium' | 'high' | 'critical';
  assigneeId?: string;
  assigneeName?: string;
  dueDate?: string;
  estimatedHours?: number;
  actualHours?: number;
  tags: string[];
  createdAt: string;
  updatedAt: string;
  projectId: string;
}

export interface ProjectMetrics {
  totalTasks: number;
  completedTasks: number;
  inProgressTasks: number;
  overdueTasks: number;
  totalMembers: number;
  activeMembers: number;
  budgetUsed: number;
  timeSpent: number;
  velocity: number;
}

export interface TeamActivity {
  id: string;
  type: 'task_created' | 'task_updated' | 'task_completed' | 'member_added' | 'comment_added';
  description: string;
  userId: string;
  userName: string;
  timestamp: string;
  metadata: Record<string, any>;
}

export const projectApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    getProjects: builder.query<Project[], { search?: string; status?: string; sortBy?: string }>({
      query: ({ search, status, sortBy } = {}) => ({
        url: '/api/projects',
        params: { search, status, sortBy },
      }),
      providesTags: ['Project'],
    }),

    getProject: builder.query<Project, string>({
      query: (id) => `/api/projects/${id}`,
      providesTags: (result, error, id) => [{ type: 'Project', id }],
    }),

    createProject: builder.mutation<Project, Partial<Project>>({
      query: (project) => ({
        url: '/api/projects',
        method: 'POST',
        body: project,
      }),
      invalidatesTags: ['Project'],
    }),

    updateProject: builder.mutation<Project, { id: string; project: Partial<Project> }>({
      query: ({ id, project }) => ({
        url: `/api/projects/${id}`,
        method: 'PUT',
        body: project,
      }),
      invalidatesTags: (result, error, { id }) => [{ type: 'Project', id }],
    }),

    deleteProject: builder.mutation<void, string>({
      query: (id) => ({
        url: `/api/projects/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Project'],
    }),

    getProjectTasks: builder.query<Task[], { projectId: string; status?: string; assigneeId?: string }>({
      query: ({ projectId, status, assigneeId }) => ({
        url: `/api/projects/${projectId}/tasks`,
        params: { status, assigneeId },
      }),
      providesTags: (result, error, { projectId }) => [{ type: 'Project', id: projectId }],
    }),

    createTask: builder.mutation<Task, { projectId: string; task: Partial<Task> }>({
      query: ({ projectId, task }) => ({
        url: `/api/projects/${projectId}/tasks`,
        method: 'POST',
        body: task,
      }),
      invalidatesTags: (result, error, { projectId }) => [{ type: 'Project', id: projectId }],
    }),

    updateTask: builder.mutation<Task, { projectId: string; taskId: string; task: Partial<Task> }>({
      query: ({ projectId, taskId, task }) => ({
        url: `/api/projects/${projectId}/tasks/${taskId}`,
        method: 'PUT',
        body: task,
      }),
      invalidatesTags: (result, error, { projectId }) => [{ type: 'Project', id: projectId }],
    }),

    deleteTask: builder.mutation<void, { projectId: string; taskId: string }>({
      query: ({ projectId, taskId }) => ({
        url: `/api/projects/${projectId}/tasks/${taskId}`,
        method: 'DELETE',
      }),
      invalidatesTags: (result, error, { projectId }) => [{ type: 'Project', id: projectId }],
    }),

    getProjectMembers: builder.query<ProjectMember[], string>({
      query: (projectId) => `/api/projects/${projectId}/members`,
      providesTags: (result, error, projectId) => [{ type: 'Project', id: projectId }],
    }),

    addProjectMember: builder.mutation<ProjectMember, { projectId: string; userId: string; role: string }>({
      query: ({ projectId, userId, role }) => ({
        url: `/api/projects/${projectId}/members`,
        method: 'POST',
        body: { userId, role },
      }),
      invalidatesTags: (result, error, { projectId }) => [{ type: 'Project', id: projectId }],
    }),

    updateProjectMember: builder.mutation<ProjectMember, { projectId: string; memberId: string; role: string }>({
      query: ({ projectId, memberId, role }) => ({
        url: `/api/projects/${projectId}/members/${memberId}`,
        method: 'PUT',
        body: { role },
      }),
      invalidatesTags: (result, error, { projectId }) => [{ type: 'Project', id: projectId }],
    }),

    removeProjectMember: builder.mutation<void, { projectId: string; memberId: string }>({
      query: ({ projectId, memberId }) => ({
        url: `/api/projects/${projectId}/members/${memberId}`,
        method: 'DELETE',
      }),
      invalidatesTags: (result, error, { projectId }) => [{ type: 'Project', id: projectId }],
    }),

    getProjectMetrics: builder.query<ProjectMetrics, string>({
      query: (projectId) => `/api/projects/${projectId}/metrics`,
      providesTags: (result, error, projectId) => [{ type: 'Project', id: projectId }],
    }),

    getTeamActivity: builder.query<TeamActivity[], { projectId: string; limit?: number }>({
      query: ({ projectId, limit = 50 }) => ({
        url: `/api/projects/${projectId}/activity`,
        params: { limit },
      }),
      providesTags: (result, error, { projectId }) => [{ type: 'Project', id: projectId }],
    }),

    getProjectAnalytics: builder.query<Record<string, any>, { projectId: string; timeRange?: string }>({
      query: ({ projectId, timeRange }) => ({
        url: `/api/projects/${projectId}/analytics`,
        params: { timeRange },
      }),
      providesTags: (result, error, { projectId }) => [{ type: 'Project', id: projectId }],
    }),

    exportProject: builder.mutation<Blob, string>({
      query: (projectId) => ({
        url: `/api/projects/${projectId}/export`,
        method: 'GET',
        responseHandler: (response) => response.blob(),
      }),
    }),

    duplicateProject: builder.mutation<Project, { projectId: string; name: string }>({
      query: ({ projectId, name }) => ({
        url: `/api/projects/${projectId}/duplicate`,
        method: 'POST',
        body: { name },
      }),
      invalidatesTags: ['Project'],
    }),
  }),
});

export const {
  useGetProjectsQuery,
  useGetProjectQuery,
  useCreateProjectMutation,
  useUpdateProjectMutation,
  useDeleteProjectMutation,
  useGetProjectTasksQuery,
  useCreateTaskMutation,
  useUpdateTaskMutation,
  useDeleteTaskMutation,
  useGetProjectMembersQuery,
  useAddProjectMemberMutation,
  useUpdateProjectMemberMutation,
  useRemoveProjectMemberMutation,
  useGetProjectMetricsQuery,
  useGetTeamActivityQuery,
  useGetProjectAnalyticsQuery,
  useExportProjectMutation,
  useDuplicateProjectMutation,
} = projectApi;
