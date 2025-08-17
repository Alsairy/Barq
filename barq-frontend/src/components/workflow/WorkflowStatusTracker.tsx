import { useState, useEffect } from 'react';
import { CheckCircle, Clock, XCircle, AlertCircle, Play, Pause, ArrowRight } from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { LoadingSpinner } from '../ui/loading-spinner';
import { aiService } from '../../services/aiService';
import { cn } from '../../lib/utils';

interface WorkflowStep {
  id: string;
  name: string;
  description: string;
  status: 'pending' | 'active' | 'completed' | 'failed' | 'skipped';
  startedAt?: string;
  completedAt?: string;
  assignedTo?: string;
  comments?: string;
  canApprove?: boolean;
  canReject?: boolean;
}

interface WorkflowStatusTrackerProps {
  requestId: string;
  onStatusChange?: (status: string) => void;
  className?: string;
}

export function WorkflowStatusTracker({ 
  requestId, 
  onStatusChange, 
  className 
}: WorkflowStatusTrackerProps) {
  const [steps, setSteps] = useState<WorkflowStep[]>([]);
  const [currentStatus, setCurrentStatus] = useState<string>('');
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [actionLoading, setActionLoading] = useState<string | null>(null);

  useEffect(() => {
    loadWorkflowStatus();
    const interval = setInterval(loadWorkflowStatus, 5000); // Poll every 5 seconds
    return () => clearInterval(interval);
  }, [requestId]);

  const loadWorkflowStatus = async () => {
    try {
      const response = await aiService.getAIRequest(requestId);
      if (response.success) {
        setCurrentStatus(response.data.status);
        setSteps(getWorkflowSteps(response.data.status));
        onStatusChange?.(response.data.status);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load workflow status');
    } finally {
      setIsLoading(false);
    }
  };

  const getWorkflowSteps = (status: string): WorkflowStep[] => {
    const baseSteps: WorkflowStep[] = [
      {
        id: 'validation',
        name: 'Request Validation',
        description: 'Validating AI request parameters and requirements',
        status: 'completed'
      },
      {
        id: 'approval',
        name: 'Multi-Level Approval',
        description: 'Request approval by designated approvers',
        status: getStepStatus(status, ['Draft', 'Submitted'], 'UnderReview', ['Approved', 'Processing', 'Completed']),
        canApprove: status === 'UnderReview',
        canReject: status === 'UnderReview'
      },
      {
        id: 'processing',
        name: 'AI Processing',
        description: 'AI provider processing the request',
        status: getStepStatus(status, ['Draft', 'Submitted', 'UnderReview'], 'Processing', ['Completed'])
      },
      {
        id: 'quality',
        name: 'Quality Assurance',
        description: 'Quality review and validation of AI output',
        status: getStepStatus(status, ['Draft', 'Submitted', 'UnderReview', 'Processing'], 'QualityReview', ['Completed'])
      },
      {
        id: 'completion',
        name: 'Completion',
        description: 'Request completed and results delivered',
        status: status === 'Completed' ? 'completed' : 'pending'
      }
    ];

    if (status === 'Rejected') {
      baseSteps[1].status = 'failed';
      baseSteps.slice(2).forEach(step => step.status = 'skipped');
    } else if (status === 'Failed') {
      const processingIndex = baseSteps.findIndex(s => s.id === 'processing');
      if (processingIndex >= 0) {
        baseSteps[processingIndex].status = 'failed';
        baseSteps.slice(processingIndex + 1).forEach(step => step.status = 'skipped');
      }
    }

    return baseSteps;
  };

  const getStepStatus = (
    currentStatus: string, 
    pendingStates: string[], 
    activeState: string, 
    completedStates: string[]
  ): 'pending' | 'active' | 'completed' | 'failed' => {
    if (completedStates.includes(currentStatus)) return 'completed';
    if (currentStatus === activeState) return 'active';
    if (pendingStates.includes(currentStatus)) return 'pending';
    return 'pending';
  };

  const handleApprove = async () => {
    setActionLoading('approve');
    try {
      const response = await aiService.approveAIRequest(
        requestId, 
        '00000000-0000-0000-0000-000000000000', // Default approver ID for development
        'Approved via workflow interface'
      );
      if (response.success) {
        await loadWorkflowStatus();
      } else {
        setError(response.message || 'Failed to approve request');
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to approve request');
    } finally {
      setActionLoading(null);
    }
  };

  const handleReject = async () => {
    setActionLoading('reject');
    try {
      const response = await aiService.rejectAIRequest(
        requestId, 
        '00000000-0000-0000-0000-000000000000', // Default approver ID for development
        'Rejected via workflow interface'
      );
      if (response.success) {
        await loadWorkflowStatus();
      } else {
        setError(response.message || 'Failed to reject request');
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to reject request');
    } finally {
      setActionLoading(null);
    }
  };

  const getStepIcon = (status: WorkflowStep['status']) => {
    switch (status) {
      case 'completed':
        return <CheckCircle className="h-5 w-5 text-green-500" />;
      case 'active':
        return <Play className="h-5 w-5 text-blue-500" />;
      case 'failed':
        return <XCircle className="h-5 w-5 text-red-500" />;
      case 'skipped':
        return <Pause className="h-5 w-5 text-gray-400" />;
      default:
        return <Clock className="h-5 w-5 text-gray-400" />;
    }
  };

  const getStepColor = (status: WorkflowStep['status']) => {
    switch (status) {
      case 'completed':
        return 'border-green-200 bg-green-50 dark:border-green-800 dark:bg-green-950/20';
      case 'active':
        return 'border-blue-200 bg-blue-50 dark:border-blue-800 dark:bg-blue-950/20';
      case 'failed':
        return 'border-red-200 bg-red-50 dark:border-red-800 dark:bg-red-950/20';
      case 'skipped':
        return 'border-gray-200 bg-gray-50 dark:border-gray-700 dark:bg-gray-800/20';
      default:
        return 'border-gray-200 bg-white dark:border-gray-700 dark:bg-gray-900';
    }
  };

  if (isLoading) {
    return (
      <Card className={className}>
        <CardContent className="flex items-center justify-center p-6">
          <LoadingSpinner className="mr-2" />
          <span>Loading workflow status...</span>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card className={className}>
      <CardHeader>
        <CardTitle className="flex items-center space-x-2">
          <AlertCircle className="h-5 w-5" />
          <span>Workflow Progress</span>
        </CardTitle>
        <CardDescription>
          Current Status: <span className="font-medium">{currentStatus}</span>
        </CardDescription>
      </CardHeader>
      
      <CardContent>
        {error && (
          <div className="mb-4 p-3 bg-red-50 border border-red-200 rounded-md text-red-700 text-sm">
            {error}
          </div>
        )}

        <div className="space-y-4">
          {steps.map((step, index) => (
            <div key={step.id} className="relative">
              <div className={cn(
                'flex items-start space-x-4 p-4 rounded-lg border-2 transition-colors',
                getStepColor(step.status)
              )}>
                <div className="flex-shrink-0 mt-0.5">
                  {getStepIcon(step.status)}
                </div>
                
                <div className="flex-1 min-w-0">
                  <div className="flex items-center justify-between">
                    <h4 className="text-sm font-medium text-gray-900 dark:text-gray-100">
                      {step.name}
                    </h4>
                    
                    {step.canApprove && (
                      <div className="flex space-x-2">
                        <Button
                          size="sm"
                          variant="outline"
                          onClick={() => handleApprove()}
                          disabled={actionLoading !== null}
                          className="text-green-600 border-green-300 hover:bg-green-50"
                        >
                          {actionLoading === 'approve' ? (
                            <LoadingSpinner className="h-3 w-3" />
                          ) : (
                            'Approve'
                          )}
                        </Button>
                        <Button
                          size="sm"
                          variant="outline"
                          onClick={() => handleReject()}
                          disabled={actionLoading !== null}
                          className="text-red-600 border-red-300 hover:bg-red-50"
                        >
                          {actionLoading === 'reject' ? (
                            <LoadingSpinner className="h-3 w-3" />
                          ) : (
                            'Reject'
                          )}
                        </Button>
                      </div>
                    )}
                  </div>
                  
                  <p className="text-sm text-gray-500 dark:text-gray-400 mt-1">
                    {step.description}
                  </p>
                  
                  {step.comments && (
                    <p className="text-xs text-gray-600 dark:text-gray-300 mt-2 italic">
                      "{step.comments}"
                    </p>
                  )}
                </div>
              </div>
              
              {index < steps.length - 1 && (
                <div className="flex justify-center py-2">
                  <ArrowRight className="h-4 w-4 text-gray-400" />
                </div>
              )}
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
}
