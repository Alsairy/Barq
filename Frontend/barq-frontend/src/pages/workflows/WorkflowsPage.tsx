import { useMemo } from 'react';
import { Plus, Play, Edit, Trash2, RefreshCw } from 'lucide-react';
import {
  useGetWorkflowTemplatesQuery,
  useGetWorkflowExecutionsQuery,
  useExecuteWorkflowMutation,
  WorkflowTemplate,
  WorkflowExecution,
} from '../../store/api/workflowApi';

export function WorkflowsPage() {
  const { data: templates = [], isLoading, error, refetch } = useGetWorkflowTemplatesQuery({});
  const { data: executions = [] } = useGetWorkflowExecutionsQuery({});
  const [executeWorkflow] = useExecuteWorkflowMutation();

  const workflows: WorkflowTemplate[] = templates || [];
  const recentExecutions: WorkflowExecution[] = useMemo(
    () => (executions || []).slice(0, 5),
    [executions]
  );

  const handleStartWorkflow = async (workflowId: string) => {
    try {
      await executeWorkflow({ templateId: workflowId, context: {} }).unwrap();
      await refetch();
    } catch (err) {
      console.error('Failed to start workflow:', err);
    }
  };

  const getStatusColor = (status: string) => {
    switch (status?.toLowerCase()) {
      case 'active':
      case 'running':
        return 'bg-green-100 text-green-800';
      case 'completed':
        return 'bg-blue-100 text-blue-800';
      case 'failed':
        return 'bg-red-100 text-red-800';
      case 'paused':
        return 'bg-yellow-100 text-yellow-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const formatDuration = (startTime?: string, endTime?: string) => {
    if (!startTime) return 'N/A';
    const start = new Date(startTime);
    const end = endTime ? new Date(endTime) : new Date();
    const duration = Math.floor((end.getTime() - start.getTime()) / 1000);
    return `${duration}s`;
  };

  const formatTimeAgo = (dateString?: string) => {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    const now = new Date();
    const diffInHours = Math.floor((now.getTime() - date.getTime()) / (1000 * 60 * 60));
    if (diffInHours < 1) return 'Less than 1 hour ago';
    return `${diffInHours} hours ago`;
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <RefreshCw className="h-8 w-8 animate-spin text-blue-600" />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-gray-900">Workflows</h1>
        <button className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 flex items-center space-x-2">
          <Plus className="h-4 w-4" />
          <span>Create Workflow</span>
        </button>
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded">
          {(error as any)?.toString?.() ?? 'Failed to load workflows'}
        </div>
      )}

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {workflows.map((workflow) => (
          <div key={workflow.id} className="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
            <div className="flex items-center justify-between mb-4">
              <h3 className="text-lg font-medium text-gray-900">{workflow.name}</h3>
              <span className={`px-2 py-1 text-xs font-medium rounded-full ${getStatusColor(workflow.isPublic ? 'active' : 'inactive')}`}>
                {workflow.isPublic ? 'Public' : 'Private'}
              </span>
            </div>
            
            <p className="text-sm text-gray-600 mb-4">
              {workflow.description || 'Automated workflow for processing and analyzing project data with AI assistance.'}
            </p>
            
            <div className="flex items-center justify-between text-sm text-gray-500 mb-4">
              <span>{workflow.version || 'v1.0'}</span>
              <span>Updated: {formatTimeAgo((workflow as any).lastModified)}</span>
            </div>
            
            <div className="mb-4">
              <div className="flex justify-between text-sm mb-1">
                <span>Category</span>
                <span>{workflow.category || 'General'}</span>
              </div>
            </div>
            
            <div className="flex items-center justify-between">
              <div className="flex items-center space-x-2">
                <button 
                  onClick={() => handleStartWorkflow(workflow.id)}
                  className="text-green-600 hover:text-green-800"
                  title="Start Workflow"
                >
                  <Play className="h-4 w-4" />
                </button>
                <button className="text-blue-600 hover:text-blue-800" title="Edit Workflow">
                  <Edit className="h-4 w-4" />
                </button>
                <button className="text-red-600 hover:text-red-800" title="Delete Workflow">
                  <Trash2 className="h-4 w-4" />
                </button>
              </div>
              <span className="text-xs text-gray-500">
                ID: {workflow.id}
              </span>
            </div>
          </div>
        ))}
      </div>

      <div className="bg-white rounded-lg shadow-sm border border-gray-200">
        <div className="p-6 border-b border-gray-200">
          <h3 className="text-lg font-medium text-gray-900">Recent Executions</h3>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Workflow</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Duration</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Started</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Completed</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {recentExecutions.map((execution) => (
                <tr key={execution.id}>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {execution.workflowName || `Workflow ${execution.workflowId}`}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className={`px-2 py-1 text-xs font-medium rounded-full ${getStatusColor(execution.status)}`}>
                      {execution.status}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">
                    {formatDuration(execution.startedAt, execution.completedAt)}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">
                    {formatTimeAgo(execution.startedAt)}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">
                    {formatTimeAgo(execution.completedAt)}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
