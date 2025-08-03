import { useState, useEffect } from 'react';
import {
  Play,
  Pause,
  Square,
  RotateCcw,
  Clock,
  CheckCircle,
  XCircle,
  Activity,
  BarChart3,
  TrendingUp,
  RefreshCw,
  Download,
  Eye,
} from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../../components/ui/card';
import { Button } from '../../../components/ui/button';
import { Badge } from '../../../components/ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../../components/ui/tabs';
import { Input } from '../../../components/ui/input';
import { Label } from '../../../components/ui/label';
import { LoadingSpinner } from '../../../components/ui/loading-spinner';

interface WorkflowExecution {
  id: string;
  workflowId: string;
  workflowName: string;
  status: 'running' | 'completed' | 'failed' | 'paused' | 'cancelled';
  startedAt: string;
  completedAt?: string;
  duration?: number;
  progress: number;
  currentStep: string;
  totalSteps: number;
  completedSteps: number;
  triggeredBy: string;
  priority: 'low' | 'medium' | 'high' | 'critical';
  executionContext: {
    environment: string;
    version: string;
    inputs: Record<string, any>;
    outputs?: Record<string, any>;
  };
  steps: WorkflowStepExecution[];
  errors?: WorkflowExecutionError[];
  metrics: {
    cpuUsage: number;
    memoryUsage: number;
    networkCalls: number;
    dataProcessed: number;
  };
}

interface WorkflowStepExecution {
  id: string;
  stepName: string;
  stepType: string;
  status: 'pending' | 'running' | 'completed' | 'failed' | 'skipped';
  startedAt?: string;
  completedAt?: string;
  duration?: number;
  inputs?: Record<string, any>;
  outputs?: Record<string, any>;
  errorMessage?: string;
  retryCount: number;
  maxRetries: number;
}

interface WorkflowExecutionError {
  id: string;
  stepId: string;
  errorType: string;
  message: string;
  stackTrace?: string;
  timestamp: string;
  severity: 'low' | 'medium' | 'high' | 'critical';
  resolved: boolean;
}

interface ExecutionMetrics {
  totalExecutions: number;
  runningExecutions: number;
  completedExecutions: number;
  failedExecutions: number;
  averageExecutionTime: number;
  successRate: number;
  resourceUtilization: number;
}

export default function WorkflowExecutionPage() {
  const [executions, setExecutions] = useState<WorkflowExecution[]>([]);
  const [metrics, setMetrics] = useState<ExecutionMetrics | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [statusFilter, setStatusFilter] = useState<string>('all');

  useEffect(() => {
    fetchExecutions();
    fetchMetrics();
    
    const interval = setInterval(() => {
      fetchExecutions();
      fetchMetrics();
    }, 5000);

    return () => clearInterval(interval);
  }, []);

  const fetchExecutions = async () => {
    setIsLoading(true);
    try {
      const response = await fetch('/api/workflows/executions');
      const data = await response.json();
      setExecutions(data);
    } catch (error) {
      console.error('Failed to fetch executions:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const fetchMetrics = async () => {
    try {
      const response = await fetch('/api/workflows/execution-metrics');
      const data = await response.json();
      setMetrics(data);
    } catch (error) {
      console.error('Failed to fetch metrics:', error);
    }
  };

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'running':
        return <Activity className="h-4 w-4 text-blue-600" />;
      case 'completed':
        return <CheckCircle className="h-4 w-4 text-green-600" />;
      case 'failed':
        return <XCircle className="h-4 w-4 text-red-600" />;
      case 'paused':
        return <Pause className="h-4 w-4 text-yellow-600" />;
      case 'cancelled':
        return <Square className="h-4 w-4 text-gray-600" />;
      default:
        return <Clock className="h-4 w-4 text-gray-600" />;
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'running':
        return 'bg-blue-100 text-blue-800';
      case 'completed':
        return 'bg-green-100 text-green-800';
      case 'failed':
        return 'bg-red-100 text-red-800';
      case 'paused':
        return 'bg-yellow-100 text-yellow-800';
      case 'cancelled':
        return 'bg-gray-100 text-gray-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case 'critical':
        return 'bg-red-100 text-red-800';
      case 'high':
        return 'bg-orange-100 text-orange-800';
      case 'medium':
        return 'bg-yellow-100 text-yellow-800';
      case 'low':
        return 'bg-green-100 text-green-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const controlExecution = async (executionId: string, action: 'pause' | 'resume' | 'stop' | 'restart') => {
    try {
      console.log(`${action} execution:`, executionId);
      await fetchExecutions();
    } catch (error) {
      console.error(`Failed to ${action} execution:`, error);
    }
  };

  const filteredExecutions = executions.filter(execution => {
    const matchesSearch = execution.workflowName.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         execution.triggeredBy.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesStatus = statusFilter === 'all' || execution.status === statusFilter;
    return matchesSearch && matchesStatus;
  });

  const formatDuration = (duration?: number) => {
    if (!duration) return 'N/A';
    const minutes = Math.floor(duration / 60000);
    const seconds = Math.floor((duration % 60000) / 1000);
    return `${minutes}m ${seconds}s`;
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Workflow Execution</h1>
          <p className="text-muted-foreground">
            Monitor and control workflow executions in real-time
          </p>
        </div>
        <div className="flex items-center space-x-2">
          <Button variant="outline" onClick={() => fetchExecutions()}>
            <RefreshCw className="h-4 w-4 mr-2" />
            Refresh
          </Button>
          <Button variant="outline">
            <Download className="h-4 w-4 mr-2" />
            Export Logs
          </Button>
        </div>
      </div>

      {/* Metrics Overview */}
      {metrics && (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-7">
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Total Executions</CardTitle>
              <BarChart3 className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.totalExecutions.toLocaleString()}</div>
              <p className="text-xs text-muted-foreground">
                All time executions
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Running</CardTitle>
              <Activity className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.runningExecutions}</div>
              <p className="text-xs text-muted-foreground">
                Currently executing
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Completed</CardTitle>
              <CheckCircle className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.completedExecutions.toLocaleString()}</div>
              <p className="text-xs text-muted-foreground">
                Successfully completed
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Failed</CardTitle>
              <XCircle className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.failedExecutions}</div>
              <p className="text-xs text-muted-foreground">
                Execution failures
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Avg Duration</CardTitle>
              <Clock className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{formatDuration(metrics.averageExecutionTime)}</div>
              <p className="text-xs text-muted-foreground">
                Average execution time
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Success Rate</CardTitle>
              <TrendingUp className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.successRate}%</div>
              <p className="text-xs text-muted-foreground">
                Execution success rate
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Resource Usage</CardTitle>
              <Activity className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.resourceUtilization}%</div>
              <p className="text-xs text-muted-foreground">
                System utilization
              </p>
            </CardContent>
          </Card>
        </div>
      )}

      <Tabs defaultValue="executions" className="space-y-4">
        <TabsList>
          <TabsTrigger value="executions">Active Executions</TabsTrigger>
          <TabsTrigger value="history">Execution History</TabsTrigger>
          <TabsTrigger value="monitoring">Real-time Monitoring</TabsTrigger>
          <TabsTrigger value="analytics">Analytics</TabsTrigger>
        </TabsList>

        <TabsContent value="executions" className="space-y-4">
          <div className="flex items-center space-x-2">
            <Input
              placeholder="Search executions..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="max-w-sm"
            />
            <select
              value={statusFilter}
              onChange={(e) => setStatusFilter(e.target.value)}
              className="px-3 py-2 border rounded-md"
            >
              <option value="all">All Status</option>
              <option value="running">Running</option>
              <option value="completed">Completed</option>
              <option value="failed">Failed</option>
              <option value="paused">Paused</option>
            </select>
          </div>

          <div className="grid gap-4">
            {filteredExecutions.map((execution) => (
              <Card key={execution.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      {getStatusIcon(execution.status)}
                      <CardTitle className="text-lg">{execution.workflowName}</CardTitle>
                      <Badge className={getStatusColor(execution.status)}>
                        {execution.status}
                      </Badge>
                      <Badge className={getPriorityColor(execution.priority)}>
                        {execution.priority}
                      </Badge>
                    </div>
                    <div className="flex items-center space-x-2">
                      {execution.status === 'running' && (
                        <>
                          <Button variant="outline" size="sm" onClick={() => controlExecution(execution.id, 'pause')}>
                            <Pause className="h-4 w-4" />
                          </Button>
                          <Button variant="outline" size="sm" onClick={() => controlExecution(execution.id, 'stop')}>
                            <Square className="h-4 w-4" />
                          </Button>
                        </>
                      )}
                      {execution.status === 'paused' && (
                        <Button variant="outline" size="sm" onClick={() => controlExecution(execution.id, 'resume')}>
                          <Play className="h-4 w-4" />
                        </Button>
                      )}
                      {(execution.status === 'failed' || execution.status === 'completed') && (
                        <Button variant="outline" size="sm" onClick={() => controlExecution(execution.id, 'restart')}>
                          <RotateCcw className="h-4 w-4" />
                        </Button>
                      )}
                      <Button variant="outline" size="sm" onClick={() => console.log('View execution:', execution.id)}>
                        <Eye className="h-4 w-4" />
                      </Button>
                    </div>
                  </div>
                  <CardDescription>
                    Started by {execution.triggeredBy} • {new Date(execution.startedAt).toLocaleString()}
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="space-y-4">
                    <div className="flex items-center justify-between">
                      <span className="text-sm text-muted-foreground">Progress</span>
                      <span className="text-sm font-medium">{execution.progress}%</span>
                    </div>
                    <div className="w-full bg-gray-200 rounded-full h-2">
                      <div 
                        className="bg-blue-600 h-2 rounded-full transition-all duration-300" 
                        style={{ width: `${execution.progress}%` }}
                      ></div>
                    </div>
                    
                    <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                      <div>
                        <Label className="text-sm font-medium">Current Step</Label>
                        <p className="text-sm">{execution.currentStep}</p>
                      </div>
                      <div>
                        <Label className="text-sm font-medium">Steps</Label>
                        <p className="text-sm">{execution.completedSteps} / {execution.totalSteps}</p>
                      </div>
                      <div>
                        <Label className="text-sm font-medium">Duration</Label>
                        <p className="text-sm">{formatDuration(execution.duration)}</p>
                      </div>
                      <div>
                        <Label className="text-sm font-medium">Environment</Label>
                        <p className="text-sm">{execution.executionContext.environment}</p>
                      </div>
                    </div>

                    {execution.errors && execution.errors.length > 0 && (
                      <div className="mt-4">
                        <Label className="text-sm font-medium text-red-600">Errors</Label>
                        <div className="mt-1 space-y-1">
                          {execution.errors.map((error) => (
                            <div key={error.id} className="p-2 bg-red-50 border border-red-200 rounded text-sm text-red-700">
                              <div className="font-medium">{error.errorType}</div>
                              <div>{error.message}</div>
                            </div>
                          ))}
                        </div>
                      </div>
                    )}
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="history" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Execution History</CardTitle>
              <CardDescription>
                Complete history of workflow executions with detailed logs and analytics
              </CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground">
                Execution history with filtering, sorting, and detailed analytics would be implemented here.
              </p>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="monitoring" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Real-time Monitoring</CardTitle>
              <CardDescription>
                Live monitoring dashboard with real-time metrics and system health
              </CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground">
                Real-time monitoring dashboard with live charts, system metrics, and health indicators would be implemented here.
              </p>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="analytics" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Execution Analytics</CardTitle>
              <CardDescription>
                Performance analytics, trends, and insights for workflow executions
              </CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground">
                Advanced analytics with performance trends, bottleneck analysis, and optimization recommendations would be implemented here.
              </p>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
