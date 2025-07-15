import { useState, useEffect } from 'react';
import { 
  BarChart3, 
  TrendingUp, 
  Star,
  AlertTriangle,
  CheckCircle,
  Target,
  RefreshCw,
  Filter,
  Download,
  Eye
} from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../../components/ui/card';
import { Button } from '../../../components/ui/button';
import { Badge } from '../../../components/ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../../components/ui/tabs';
import { LoadingSpinner } from '../../../components/ui/loading-spinner';
import { Input } from '../../../components/ui/input';
import { Label } from '../../../components/ui/label';

interface QualityMetrics {
  overallScore: number;
  accuracyScore: number;
  relevanceScore: number;
  consistencyScore: number;
  userSatisfaction: number;
  totalEvaluations: number;
  improvementTrend: number;
  benchmarkComparison: number;
}

interface QualityAlert {
  id: string;
  type: 'warning' | 'error' | 'info';
  title: string;
  description: string;
  timestamp: string;
  severity: 'low' | 'medium' | 'high' | 'critical';
  resolved: boolean;
}

interface FeedbackData {
  id: string;
  taskId: string;
  taskName: string;
  rating: number;
  feedback: string;
  timestamp: string;
  user: string;
  category: string;
  qualityScore: number;
}

interface BenchmarkData {
  provider: string;
  accuracy: number;
  relevance: number;
  consistency: number;
  responseTime: number;
  cost: number;
  overallScore: number;
}

export default function AIQualityAnalyticsPage() {
  const [metrics, setMetrics] = useState<QualityMetrics | null>(null);
  const [alerts, setAlerts] = useState<QualityAlert[]>([]);
  const [feedback, setFeedback] = useState<FeedbackData[]>([]);
  const [benchmarks, setBenchmarks] = useState<BenchmarkData[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [timeRange] = useState('7d');

  useEffect(() => {
    fetchQualityMetrics();
    fetchQualityAlerts();
    fetchFeedbackData();
    fetchBenchmarkData();
  }, [timeRange]);

  const fetchQualityMetrics = async () => {
    setIsLoading(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      const mockMetrics: QualityMetrics = {
        overallScore: 87.5,
        accuracyScore: 89.2,
        relevanceScore: 86.8,
        consistencyScore: 85.1,
        userSatisfaction: 4.3,
        totalEvaluations: 2847,
        improvementTrend: 5.2,
        benchmarkComparison: 12.8
      };
      
      setMetrics(mockMetrics);
    } catch (error) {
      console.error('Failed to fetch quality metrics:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const fetchQualityAlerts = async () => {
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      
      const mockAlerts: QualityAlert[] = [
        {
          id: '1',
          type: 'warning',
          title: 'Quality Score Decline',
          description: 'Accuracy score has dropped below 85% threshold for text generation tasks',
          timestamp: '2024-01-15T10:30:00Z',
          severity: 'medium',
          resolved: false
        },
        {
          id: '2',
          type: 'error',
          title: 'High Error Rate',
          description: 'Image analysis tasks showing 15% error rate, exceeding acceptable limits',
          timestamp: '2024-01-15T09:45:00Z',
          severity: 'high',
          resolved: false
        },
        {
          id: '3',
          type: 'info',
          title: 'Benchmark Improvement',
          description: 'Translation tasks now performing 8% better than industry benchmark',
          timestamp: '2024-01-15T08:20:00Z',
          severity: 'low',
          resolved: true
        }
      ];
      
      setAlerts(mockAlerts);
    } catch (error) {
      console.error('Failed to fetch quality alerts:', error);
    }
  };

  const fetchFeedbackData = async () => {
    try {
      await new Promise(resolve => setTimeout(resolve, 700));
      
      const mockFeedback: FeedbackData[] = [
        {
          id: '1',
          taskId: 'task-001',
          taskName: 'Document Summarization',
          rating: 5,
          feedback: 'Excellent summary, captured all key points accurately',
          timestamp: '2024-01-15T10:15:00Z',
          user: 'john.doe@company.com',
          category: 'summarization',
          qualityScore: 94
        },
        {
          id: '2',
          taskId: 'task-002',
          taskName: 'Customer Query Analysis',
          rating: 3,
          feedback: 'Good analysis but missed some sentiment nuances',
          timestamp: '2024-01-15T09:30:00Z',
          user: 'jane.smith@company.com',
          category: 'analysis',
          qualityScore: 78
        },
        {
          id: '3',
          taskId: 'task-003',
          taskName: 'Code Review Assistant',
          rating: 4,
          feedback: 'Helpful suggestions, could be more specific about security issues',
          timestamp: '2024-01-15T08:45:00Z',
          user: 'dev.team@company.com',
          category: 'code-review',
          qualityScore: 85
        }
      ];
      
      setFeedback(mockFeedback);
    } catch (error) {
      console.error('Failed to fetch feedback data:', error);
    }
  };

  const fetchBenchmarkData = async () => {
    try {
      await new Promise(resolve => setTimeout(resolve, 600));
      
      const mockBenchmarks: BenchmarkData[] = [
        {
          provider: 'OpenAI GPT-4',
          accuracy: 89.2,
          relevance: 86.8,
          consistency: 85.1,
          responseTime: 1200,
          cost: 0.03,
          overallScore: 87.0
        },
        {
          provider: 'Anthropic Claude',
          accuracy: 87.5,
          relevance: 88.2,
          consistency: 86.9,
          responseTime: 950,
          cost: 0.025,
          overallScore: 87.5
        },
        {
          provider: 'Azure OpenAI',
          accuracy: 85.8,
          relevance: 84.1,
          consistency: 83.7,
          responseTime: 1350,
          cost: 0.028,
          overallScore: 84.5
        }
      ];
      
      setBenchmarks(mockBenchmarks);
    } catch (error) {
      console.error('Failed to fetch benchmark data:', error);
    }
  };

  const getAlertIcon = (type: string) => {
    switch (type) {
      case 'error':
        return <AlertTriangle className="h-4 w-4 text-red-500" />;
      case 'warning':
        return <AlertTriangle className="h-4 w-4 text-yellow-500" />;
      default:
        return <CheckCircle className="h-4 w-4 text-blue-500" />;
    }
  };

  const getAlertColor = (severity: string) => {
    switch (severity) {
      case 'critical':
        return 'bg-red-100 text-red-800 border-red-200';
      case 'high':
        return 'bg-orange-100 text-orange-800 border-orange-200';
      case 'medium':
        return 'bg-yellow-100 text-yellow-800 border-yellow-200';
      default:
        return 'bg-blue-100 text-blue-800 border-blue-200';
    }
  };

  const renderStars = (rating: number) => {
    return Array.from({ length: 5 }, (_, i) => (
      <Star
        key={i}
        className={`h-4 w-4 ${
          i < rating ? 'text-yellow-400 fill-current' : 'text-gray-300'
        }`}
      />
    ));
  };

  const handleRefresh = () => {
    fetchQualityMetrics();
    fetchQualityAlerts();
    fetchFeedbackData();
    fetchBenchmarkData();
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
          <h1 className="text-3xl font-bold tracking-tight">AI Quality Analytics</h1>
          <p className="text-muted-foreground">
            Monitor AI output quality with comprehensive analytics and improvement tracking
          </p>
        </div>
        <div className="flex items-center space-x-2">
          <Button variant="outline" onClick={handleRefresh}>
            <RefreshCw className="h-4 w-4 mr-2" />
            Refresh
          </Button>
          <Button variant="outline">
            <Download className="h-4 w-4 mr-2" />
            Export Report
          </Button>
        </div>
      </div>

      {/* Quality Metrics Overview */}
      {metrics && (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Overall Quality Score</CardTitle>
              <Target className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.overallScore}%</div>
              <p className="text-xs text-muted-foreground">
                <TrendingUp className="h-3 w-3 inline mr-1" />
                +{metrics.improvementTrend}% this week
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">User Satisfaction</CardTitle>
              <Star className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.userSatisfaction}/5</div>
              <p className="text-xs text-muted-foreground">
                From {metrics.totalEvaluations.toLocaleString()} evaluations
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Accuracy Score</CardTitle>
              <CheckCircle className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.accuracyScore}%</div>
              <p className="text-xs text-muted-foreground">
                Consistency: {metrics.consistencyScore}%
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Benchmark Performance</CardTitle>
              <BarChart3 className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">+{metrics.benchmarkComparison}%</div>
              <p className="text-xs text-muted-foreground">
                Above industry average
              </p>
            </CardContent>
          </Card>
        </div>
      )}

      {/* Quality Alerts */}
      <Card>
        <CardHeader>
          <CardTitle>Quality Alerts</CardTitle>
          <CardDescription>
            Real-time notifications for quality threshold violations and improvements
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="space-y-3">
            {alerts.map((alert) => (
              <div
                key={alert.id}
                className={`p-3 rounded-lg border ${getAlertColor(alert.severity)} ${
                  alert.resolved ? 'opacity-60' : ''
                }`}
              >
                <div className="flex items-start justify-between">
                  <div className="flex items-start space-x-2">
                    {getAlertIcon(alert.type)}
                    <div>
                      <h4 className="font-medium">{alert.title}</h4>
                      <p className="text-sm mt-1">{alert.description}</p>
                      <p className="text-xs mt-2 opacity-70">
                        {new Date(alert.timestamp).toLocaleString()}
                      </p>
                    </div>
                  </div>
                  <Badge variant={alert.resolved ? 'secondary' : 'destructive'}>
                    {alert.resolved ? 'Resolved' : alert.severity}
                  </Badge>
                </div>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>

      <Tabs defaultValue="feedback" className="space-y-4">
        <TabsList>
          <TabsTrigger value="feedback">User Feedback</TabsTrigger>
          <TabsTrigger value="benchmarks">Benchmarks</TabsTrigger>
          <TabsTrigger value="trends">Quality Trends</TabsTrigger>
          <TabsTrigger value="ab-testing">A/B Testing</TabsTrigger>
        </TabsList>

        <TabsContent value="feedback" className="space-y-4">
          <div className="flex items-center space-x-2">
            <Input
              placeholder="Search feedback..."
              className="max-w-sm"
            />
            <Button variant="outline">
              <Filter className="h-4 w-4 mr-2" />
              Filter
            </Button>
          </div>

          <div className="space-y-4">
            {feedback.map((item) => (
              <Card key={item.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div>
                      <CardTitle className="text-lg">{item.taskName}</CardTitle>
                      <CardDescription>
                        Task ID: {item.taskId} • {item.user} • {new Date(item.timestamp).toLocaleString()}
                      </CardDescription>
                    </div>
                    <div className="flex items-center space-x-2">
                      <div className="flex items-center">
                        {renderStars(item.rating)}
                      </div>
                      <Badge variant="secondary">
                        Quality: {item.qualityScore}%
                      </Badge>
                    </div>
                  </div>
                </CardHeader>
                <CardContent>
                  <p className="text-sm text-muted-foreground">{item.feedback}</p>
                  <div className="mt-3 flex items-center space-x-2">
                    <Badge variant="outline">{item.category}</Badge>
                    <Button variant="outline" size="sm">
                      <Eye className="h-4 w-4 mr-2" />
                      View Task
                    </Button>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="benchmarks" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Provider Performance Comparison</CardTitle>
              <CardDescription>
                Benchmark analysis across different AI providers and models
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="space-y-4">
                {benchmarks.map((benchmark, index) => (
                  <div key={index} className="p-4 border rounded-lg">
                    <div className="flex items-center justify-between mb-3">
                      <h4 className="font-medium">{benchmark.provider}</h4>
                      <Badge variant="secondary">
                        Overall: {benchmark.overallScore}%
                      </Badge>
                    </div>
                    <div className="grid gap-3 md:grid-cols-3 lg:grid-cols-5">
                      <div>
                        <Label className="text-xs font-medium">Accuracy</Label>
                        <p className="text-sm font-semibold">{benchmark.accuracy}%</p>
                      </div>
                      <div>
                        <Label className="text-xs font-medium">Relevance</Label>
                        <p className="text-sm font-semibold">{benchmark.relevance}%</p>
                      </div>
                      <div>
                        <Label className="text-xs font-medium">Consistency</Label>
                        <p className="text-sm font-semibold">{benchmark.consistency}%</p>
                      </div>
                      <div>
                        <Label className="text-xs font-medium">Response Time</Label>
                        <p className="text-sm font-semibold">{benchmark.responseTime}ms</p>
                      </div>
                      <div>
                        <Label className="text-xs font-medium">Cost</Label>
                        <p className="text-sm font-semibold">${benchmark.cost}</p>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="trends" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Quality Trends Analysis</CardTitle>
              <CardDescription>
                Historical quality metrics and improvement tracking over time
              </CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground">
                Quality trends visualization with historical data and improvement tracking would be implemented here.
              </p>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="ab-testing" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>A/B Testing Interface</CardTitle>
              <CardDescription>
                Model comparison and performance evaluation for optimization
              </CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground">
                A/B testing interface with model comparison and performance evaluation would be implemented here.
              </p>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
