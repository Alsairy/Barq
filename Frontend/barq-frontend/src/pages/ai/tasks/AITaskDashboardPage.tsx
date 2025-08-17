import { useState, useEffect } from 'react';
import { 
  Play, 
  Pause, 
  Square, 
  Clock, 
  CheckCircle, 
  XCircle, 
  AlertTriangle,
  BarChart3,
  TrendingUp,
  Activity,
  Zap,
  Target,
  RefreshCw,
  Filter,
  MoreHorizontal
} from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../../components/ui/card';
import { Button } from '../../../components/ui/button';
import { Badge } from '../../../components/ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../../components/ui/tabs';
import { LoadingSpinner } from '../../../components/ui/loading-spinner';
import { Input } from '../../../components/ui/input';
import { Label } from '../../../components/ui/label';

interface AITask {
  id: string;
  name: string;
  type: 'text-generation' | 'image-analysis' | 'data-processing' | 'translation' | 'summarization';
  status: 'pending' | 'running' | 'completed' | 'failed' | 'paused';
  priority: 'low' | 'medium' | 'high' | 'critical';
  provider: string;
  startTime: string;
  endTime?: string;
  duration?: number;
  progress: number;
  inputTokens: number;
  outputTokens: number;
  cost: number;
  qualityScore?: number;
  errorMessage?: string;
  retryCount: number;
  maxRetries: number;
}

interface TaskMetrics {
  totalTasks: number;
  runningTasks: number;
  completedTasks: number;
  failedTasks: number;
  averageResponseTime: number;
  totalCost: number;
  averageQualityScore: number;
  throughput: number;
}

interface QueueStats {
  pending: number;
  running: number;
  capacity: number;
  estimatedWaitTime: number;
}

export default function AITaskDashboardPage() {
  const [tasks, setTasks] = useState<AITask[]>([]);
  const [metrics, setMetrics] = useState<TaskMetrics | null>(null);
  const [queueStats, setQueueStats] = useState<QueueStats | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [statusFilter] = useState<string>('all');
  const [priorityFilter] = useState<string>('all');

  useEffect(() => {
    fetchTasks();
    fetchMetrics();
    fetchQueueStats();
    
    const interval = setInterval(() => {
      fetchTasks();
      fetchMetrics();
      fetchQueueStats();
    }, 5000);
    
    return () => clearInterval(interval);
  }, []);

  const fetchTasks = async () => {
    setIsLoading(true);
    try {
      const response = await fetch('/api/ai/tasks');
      const tasks = await response.json();
      setTasks(tasks);
    } catch (error) {
      console.error('Failed to fetch tasks:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const fetchMetrics = async () => {
    try {
      const response = await fetch('/api/ai/tasks/metrics');
      const metrics = await response.json();
      setMetrics(metrics);
    } catch (error) {
      console.error('Failed to fetch metrics:', error);
    }
  };

  const fetchQueueStats = async () => {
    try {
      const response = await fetch('/api/ai/tasks/queue-stats');
      const queueStats = await response.json();
      setQueueStats(queueStats);
    } catch (error) {
      console.error('Failed to fetch queue stats:', error);
    }
  };

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'running':
        return <Play className="h-4 w-4 text-blue-500" />;
      case 'completed':
        return <CheckCircle className="h-4 w-4 text-green-500" />;
      case 'failed':
        return <XCircle className="h-4 w-4 text-red-500" />;
      case 'paused':
        return <Pause className="h-4 w-4 text-yellow-500" />;
      default:
        return <Clock className="h-4 w-4 text-gray-500" />;
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
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const filteredTasks = tasks.filter(task => {
    const matchesSearch = task.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         task.type.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesStatus = statusFilter === 'all' || task.status === statusFilter;
    const matchesPriority = priorityFilter === 'all' || task.priority === priorityFilter;
    
    return matchesSearch && matchesStatus && matchesPriority;
  });

  const handleRefresh = () => {
    fetchTasks();
    fetchMetrics();
    fetchQueueStats();
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
          <h1 className="text-3xl font-bold tracking-tight">AI Task Dashboard</h1>
          <p className="text-muted-foreground">
            Monitor AI task execution with real-time performance metrics
          </p>
        </div>
        <div className="flex items-center space-x-2">
          <Button variant="outline" onClick={handleRefresh}>
            <RefreshCw className="h-4 w-4 mr-2" />
            Refresh
          </Button>
        </div>
      </div>

      {/* Metrics Overview */}
      {metrics && (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Total Tasks</CardTitle>
              <Activity className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.totalTasks}</div>
              <p className="text-xs text-muted-foreground">
                {metrics.runningTasks} currently running
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Success Rate</CardTitle>
              <Target className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">
                {((metrics.completedTasks / metrics.totalTasks) * 100).toFixed(1)}%
              </div>
              <p className="text-xs text-muted-foreground">
                <TrendingUp className="h-3 w-3 inline mr-1" />
                +2.5% from last week
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Avg Response Time</CardTitle>
              <Zap className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.averageResponseTime}s</div>
              <p className="text-xs text-muted-foreground">
                Across all providers
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Quality Score</CardTitle>
              <BarChart3 className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.averageQualityScore}</div>
              <p className="text-xs text-muted-foreground">
                Average across all tasks
              </p>
            </CardContent>
          </Card>
        </div>
      )}

      {/* Queue Statistics */}
      {queueStats && (
        <Card>
          <CardHeader>
            <CardTitle>Task Queue Status</CardTitle>
            <CardDescription>Real-time queue management and capacity monitoring</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="grid gap-4 md:grid-cols-4">
              <div>
                <Label className="text-sm font-medium">Pending Tasks</Label>
                <p className="text-2xl font-bold">{queueStats.pending}</p>
              </div>
              <div>
                <Label className="text-sm font-medium">Running Tasks</Label>
                <p className="text-2xl font-bold">{queueStats.running}</p>
              </div>
              <div>
                <Label className="text-sm font-medium">Capacity Usage</Label>
                <p className="text-2xl font-bold">
                  {Math.round((queueStats.running / queueStats.capacity) * 100)}%
                </p>
                <div className="mt-1 w-full bg-gray-200 rounded-full h-2">
                  <div 
                    className="bg-blue-600 h-2 rounded-full" 
                    style={{ width: `${(queueStats.running / queueStats.capacity) * 100}%` }}
                  ></div>
                </div>
              </div>
              <div>
                <Label className="text-sm font-medium">Est. Wait Time</Label>
                <p className="text-2xl font-bold">{queueStats.estimatedWaitTime}m</p>
              </div>
            </div>
          </CardContent>
        </Card>
      )}

      <Tabs defaultValue="tasks" className="space-y-4">
        <TabsList>
          <TabsTrigger value="tasks">Active Tasks</TabsTrigger>
          <TabsTrigger value="history">Task History</TabsTrigger>
          <TabsTrigger value="performance">Performance</TabsTrigger>
          <TabsTrigger value="optimization">Optimization</TabsTrigger>
        </TabsList>

        <TabsContent value="tasks" className="space-y-4">
          <div className="flex items-center space-x-2">
            <Input
              placeholder="Search tasks..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="max-w-sm"
            />
            <Button variant="outline">
              <Filter className="h-4 w-4 mr-2" />
              Filter
            </Button>
          </div>

          <div className="space-y-4">
            {filteredTasks.map((task) => (
              <Card key={task.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      {getStatusIcon(task.status)}
                      <CardTitle className="text-lg">{task.name}</CardTitle>
                      <Badge className={getStatusColor(task.status)}>
                        {task.status}
                      </Badge>
                      <Badge className={getPriorityColor(task.priority)}>
                        {task.priority}
                      </Badge>
                    </div>
                    <div className="flex items-center space-x-2">
                      {task.status === 'running' && (
                        <Button variant="outline" size="sm">
                          <Pause className="h-4 w-4 mr-2" />
                          Pause
                        </Button>
                      )}
                      {task.status === 'paused' && (
                        <Button variant="outline" size="sm">
                          <Play className="h-4 w-4 mr-2" />
                          Resume
                        </Button>
                      )}
                      {(task.status === 'running' || task.status === 'paused') && (
                        <Button variant="outline" size="sm">
                          <Square className="h-4 w-4 mr-2" />
                          Stop
                        </Button>
                      )}
                      <Button variant="outline" size="sm">
                        <MoreHorizontal className="h-4 w-4" />
                      </Button>
                    </div>
                  </div>
                  <CardDescription>
                    {task.type} • {task.provider} • Started: {new Date(task.startTime).toLocaleString()}
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  {task.status === 'running' && (
                    <div className="mb-4">
                      <div className="flex items-center justify-between mb-2">
                        <Label className="text-sm font-medium">Progress</Label>
                        <span className="text-sm text-muted-foreground">{task.progress}%</span>
                      </div>
                      <div className="w-full bg-gray-200 rounded-full h-2">
                        <div 
                          className="bg-blue-600 h-2 rounded-full transition-all duration-300" 
                          style={{ width: `${task.progress}%` }}
                        ></div>
                      </div>
                    </div>
                  )}
                  
                  <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                    <div>
                      <Label className="text-sm font-medium">Input Tokens</Label>
                      <p className="text-lg font-semibold">{task.inputTokens.toLocaleString()}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Output Tokens</Label>
                      <p className="text-lg font-semibold">{task.outputTokens.toLocaleString()}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Cost</Label>
                      <p className="text-lg font-semibold">${task.cost.toFixed(3)}</p>
                    </div>
                    {task.qualityScore && (
                      <div>
                        <Label className="text-sm font-medium">Quality Score</Label>
                        <p className="text-lg font-semibold">{task.qualityScore}/100</p>
                      </div>
                    )}
                  </div>
                  
                  {task.errorMessage && (
                    <div className="mt-4 p-3 bg-red-50 border border-red-200 rounded-md">
                      <div className="flex items-center">
                        <AlertTriangle className="h-4 w-4 text-red-500 mr-2" />
                        <span className="text-sm font-medium text-red-800">Error</span>
                      </div>
                      <p className="text-sm text-red-700 mt-1">{task.errorMessage}</p>
                      <p className="text-xs text-red-600 mt-1">
                        Retry {task.retryCount}/{task.maxRetries}
                      </p>
                    </div>
                  )}
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="history" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Task Execution History</CardTitle>
              <CardDescription>
                Detailed logs and performance trends for completed tasks
              </CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground">
                Task history with detailed execution logs and performance analytics would be implemented here.
              </p>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="performance" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Performance Analytics</CardTitle>
              <CardDescription>
                Response times, throughput, and error rate analysis
              </CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground">
                Performance monitoring dashboard with charts and metrics would be implemented here.
              </p>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="optimization" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Optimization Recommendations</CardTitle>
              <CardDescription>
                AI-driven suggestions for improving task performance and reducing costs
              </CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground">
                Optimization suggestions and automated performance improvement recommendations would be implemented here.
              </p>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
