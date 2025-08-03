import { useState, useEffect } from 'react';
import { Plus, Search, Filter, RefreshCw } from 'lucide-react';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card';
import { LoadingSpinner } from '../../components/ui/loading-spinner';
import { AIRequestCreation } from '../../components/ai/AIRequestCreation';
import { WorkflowStatusTracker } from '../../components/workflow/WorkflowStatusTracker';
import { aiService, AIRequest } from '../../services/aiService';
import { cn } from '../../lib/utils';

interface AIRequestManagementPageProps {
  className?: string;
}

export function AIRequestManagementPage({ className }: AIRequestManagementPageProps) {
  const [requests, setRequests] = useState<AIRequest[]>([]);
  const [filteredRequests, setFilteredRequests] = useState<AIRequest[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [selectedRequest, setSelectedRequest] = useState<AIRequest | null>(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [statusFilter, setStatusFilter] = useState<string>('all');

  useEffect(() => {
    loadRequests();
  }, []);

  useEffect(() => {
    filterRequests();
  }, [requests, searchTerm, statusFilter]);

  const loadRequests = async () => {
    try {
      setIsLoading(true);
      const response = await aiService.getUserAIRequests();
      if (response.success) {
        setRequests(response.data);
      } else {
        setError('Failed to load AI requests');
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load AI requests');
    } finally {
      setIsLoading(false);
    }
  };

  const filterRequests = () => {
    let filtered = requests;

    if (searchTerm) {
      filtered = filtered.filter(request =>
        request.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
        request.description.toLowerCase().includes(searchTerm.toLowerCase())
      );
    }

    if (statusFilter !== 'all') {
      filtered = filtered.filter(request => request.status === statusFilter);
    }

    setFilteredRequests(filtered);
  };

  const handleRequestCreated = async (requestId: string) => {
    setShowCreateForm(false);
    await loadRequests();
    
    const newRequest = requests.find(r => r.id === requestId);
    if (newRequest) {
      setSelectedRequest(newRequest);
    }
  };

  const handleRequestSelect = (request: AIRequest) => {
    setSelectedRequest(request);
  };

  const getStatusColor = (status: string) => {
    const colorMap: Record<string, string> = {
      'Draft': 'bg-gray-100 text-gray-800',
      'Submitted': 'bg-blue-100 text-blue-800',
      'UnderReview': 'bg-yellow-100 text-yellow-800',
      'Approved': 'bg-green-100 text-green-800',
      'Processing': 'bg-purple-100 text-purple-800',
      'Completed': 'bg-green-100 text-green-800',
      'Rejected': 'bg-red-100 text-red-800',
      'Failed': 'bg-red-100 text-red-800',
    };
    return colorMap[status] || 'bg-gray-100 text-gray-800';
  };

  const getPriorityColor = (priority: string) => {
    const colorMap: Record<string, string> = {
      'Low': 'bg-green-100 text-green-800',
      'Medium': 'bg-yellow-100 text-yellow-800',
      'High': 'bg-orange-100 text-orange-800',
      'Critical': 'bg-red-100 text-red-800',
    };
    return colorMap[priority] || 'bg-gray-100 text-gray-800';
  };

  if (showCreateForm) {
    return (
      <div className={cn('h-full', className)}>
        <AIRequestCreation
          onRequestCreated={handleRequestCreated}
          onCancel={() => setShowCreateForm(false)}
        />
      </div>
    );
  }

  return (
    <div className={cn('h-full flex flex-col space-y-6 p-6', className)}>
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 dark:text-gray-100">
            AI Request Management
          </h1>
          <p className="text-gray-500 dark:text-gray-400">
            Create, track, and manage AI requests with workflow integration
          </p>
        </div>
        
        <Button onClick={() => setShowCreateForm(true)}>
          <Plus className="h-4 w-4 mr-2" />
          Create Request
        </Button>
      </div>

      <div className="flex items-center space-x-4">
        <div className="flex-1 relative">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
          <Input
            placeholder="Search requests..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="pl-10"
          />
        </div>
        
        <select
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value)}
          className="px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500"
        >
          <option value="all">All Status</option>
          <option value="Draft">Draft</option>
          <option value="Submitted">Submitted</option>
          <option value="UnderReview">Under Review</option>
          <option value="Approved">Approved</option>
          <option value="Processing">Processing</option>
          <option value="Completed">Completed</option>
          <option value="Rejected">Rejected</option>
          <option value="Failed">Failed</option>
        </select>
        
        <Button variant="outline" onClick={loadRequests} disabled={isLoading}>
          <RefreshCw className={cn('h-4 w-4', isLoading && 'animate-spin')} />
        </Button>
      </div>

      {error && (
        <div className="p-4 bg-red-50 border border-red-200 rounded-md text-red-700">
          {error}
        </div>
      )}

      <div className="flex-1 grid grid-cols-1 lg:grid-cols-2 gap-6 min-h-0">
        <div className="space-y-4 overflow-y-auto">
          <h2 className="text-lg font-semibold text-gray-900 dark:text-gray-100">
            Requests ({filteredRequests.length})
          </h2>
          
          {isLoading ? (
            <div className="flex items-center justify-center p-8">
              <LoadingSpinner className="mr-2" />
              <span>Loading requests...</span>
            </div>
          ) : filteredRequests.length === 0 ? (
            <Card>
              <CardContent className="flex flex-col items-center justify-center p-8 text-center">
                <div className="text-gray-400 mb-4">
                  <Filter className="h-12 w-12" />
                </div>
                <h3 className="text-lg font-medium text-gray-900 dark:text-gray-100 mb-2">
                  No requests found
                </h3>
                <p className="text-gray-500 dark:text-gray-400 mb-4">
                  {requests.length === 0 
                    ? "You haven't created any AI requests yet."
                    : "No requests match your current filters."
                  }
                </p>
                <Button onClick={() => setShowCreateForm(true)}>
                  <Plus className="h-4 w-4 mr-2" />
                  Create Your First Request
                </Button>
              </CardContent>
            </Card>
          ) : (
            filteredRequests.map((request) => (
              <Card
                key={request.id}
                className={cn(
                  'cursor-pointer transition-colors hover:bg-gray-50 dark:hover:bg-gray-800',
                  selectedRequest?.id === request.id && 'ring-2 ring-blue-500'
                )}
                onClick={() => handleRequestSelect(request)}
              >
                <CardHeader className="pb-3">
                  <div className="flex items-start justify-between">
                    <CardTitle className="text-base">{request.title}</CardTitle>
                    <div className="flex space-x-2">
                      <span className={cn('px-2 py-1 text-xs font-medium rounded-full', getStatusColor(request.status))}>
                        {request.status}
                      </span>
                      <span className={cn('px-2 py-1 text-xs font-medium rounded-full', getPriorityColor(request.priority))}>
                        {request.priority}
                      </span>
                    </div>
                  </div>
                  <CardDescription className="line-clamp-2">
                    {request.description}
                  </CardDescription>
                </CardHeader>
                
                <CardContent className="pt-0">
                  <div className="flex items-center justify-between text-sm text-gray-500 dark:text-gray-400">
                    <div className="flex items-center space-x-4">
                      <span>{aiService.getRequestTypeIcon(request.requestType)} {request.requestType}</span>
                      <span>{aiService.getAIProviderIcon(request.aiProvider)} {request.aiProvider}</span>
                    </div>
                    <span>{new Date(request.createdAt).toLocaleDateString()}</span>
                  </div>
                </CardContent>
              </Card>
            ))
          )}
        </div>

        <div className="space-y-4">
          {selectedRequest ? (
            <>
              <div className="flex items-center justify-between">
                <h2 className="text-lg font-semibold text-gray-900 dark:text-gray-100">
                  Request Details
                </h2>
                <Button
                  variant="outline"
                  size="sm"
                  onClick={() => setSelectedRequest(null)}
                >
                  Close
                </Button>
              </div>
              
              <Card>
                <CardHeader>
                  <CardTitle>{selectedRequest.title}</CardTitle>
                  <CardDescription>{selectedRequest.description}</CardDescription>
                </CardHeader>
                
                <CardContent className="space-y-4">
                  <div className="grid grid-cols-2 gap-4 text-sm">
                    <div>
                      <span className="font-medium text-gray-700 dark:text-gray-300">Type:</span>
                      <div className="flex items-center space-x-1 mt-1">
                        <span>{aiService.getRequestTypeIcon(selectedRequest.requestType)}</span>
                        <span>{selectedRequest.requestType}</span>
                      </div>
                    </div>
                    
                    <div>
                      <span className="font-medium text-gray-700 dark:text-gray-300">AI Provider:</span>
                      <div className="flex items-center space-x-1 mt-1">
                        <span>{aiService.getAIProviderIcon(selectedRequest.aiProvider)}</span>
                        <span>{selectedRequest.aiProvider}</span>
                      </div>
                    </div>
                    
                    <div>
                      <span className="font-medium text-gray-700 dark:text-gray-300">Priority:</span>
                      <span className={cn('px-2 py-1 text-xs font-medium rounded-full mt-1 inline-block', getPriorityColor(selectedRequest.priority))}>
                        {selectedRequest.priority}
                      </span>
                    </div>
                    
                    <div>
                      <span className="font-medium text-gray-700 dark:text-gray-300">Status:</span>
                      <span className={cn('px-2 py-1 text-xs font-medium rounded-full mt-1 inline-block', getStatusColor(selectedRequest.status))}>
                        {selectedRequest.status}
                      </span>
                    </div>
                    
                    <div>
                      <span className="font-medium text-gray-700 dark:text-gray-300">Created:</span>
                      <div className="mt-1">{new Date(selectedRequest.createdAt).toLocaleString()}</div>
                    </div>
                    
                    <div>
                      <span className="font-medium text-gray-700 dark:text-gray-300">Updated:</span>
                      <div className="mt-1">{new Date(selectedRequest.updatedAt).toLocaleString()}</div>
                    </div>
                  </div>
                  
                  {selectedRequest.tags && selectedRequest.tags.length > 0 && (
                    <div>
                      <span className="font-medium text-gray-700 dark:text-gray-300">Tags:</span>
                      <div className="flex flex-wrap gap-1 mt-1">
                        {selectedRequest.tags.map((tag, index) => (
                          <span key={index} className="px-2 py-1 text-xs bg-gray-100 dark:bg-gray-800 rounded-full">
                            {tag}
                          </span>
                        ))}
                      </div>
                    </div>
                  )}
                </CardContent>
              </Card>
              
              <WorkflowStatusTracker
                requestId={selectedRequest.id}
                onStatusChange={(status) => {
                  setSelectedRequest(prev => prev ? { ...prev, status } : null);
                  loadRequests(); // Refresh the list
                }}
              />
            </>
          ) : (
            <Card>
              <CardContent className="flex flex-col items-center justify-center p-8 text-center">
                <div className="text-gray-400 mb-4">
                  <Search className="h-12 w-12" />
                </div>
                <h3 className="text-lg font-medium text-gray-900 dark:text-gray-100 mb-2">
                  Select a Request
                </h3>
                <p className="text-gray-500 dark:text-gray-400">
                  Choose a request from the list to view its details and workflow status.
                </p>
              </CardContent>
            </Card>
          )}
        </div>
      </div>
    </div>
  );
}
