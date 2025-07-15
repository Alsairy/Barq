import { useState, useEffect } from 'react';
import { 
  Settings, 
  Activity, 
  DollarSign, 
  TrendingUp, 
  TrendingDown,
  AlertTriangle,
  CheckCircle,
  XCircle,
  RefreshCw,
  Plus,
  Edit,
  Trash2,
  Eye,
  BarChart3
} from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../../components/ui/card';
import { Button } from '../../../components/ui/button';
import { Badge } from '../../../components/ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../../components/ui/tabs';
import { LoadingSpinner } from '../../../components/ui/loading-spinner';
import { Input } from '../../../components/ui/input';
import { Label } from '../../../components/ui/label';

interface AIProvider {
  id: string;
  name: string;
  type: 'openai' | 'anthropic' | 'azure' | 'google' | 'custom';
  status: 'active' | 'inactive' | 'error' | 'maintenance';
  endpoint: string;
  apiKeyMasked: string;
  responseTime: number;
  uptime: number;
  costPerRequest: number;
  requestsToday: number;
  costToday: number;
  errorRate: number;
  rateLimit: number;
  rateLimitUsed: number;
  lastHealthCheck: string;
  configuration: {
    timeout: number;
    retries: number;
    priority: number;
    failoverEnabled: boolean;
  };
}

interface ProviderMetrics {
  totalProviders: number;
  activeProviders: number;
  totalRequests: number;
  totalCost: number;
  averageResponseTime: number;
  overallUptime: number;
}

export default function AIProviderManagementPage() {
  const [providers, setProviders] = useState<AIProvider[]>([]);
  const [metrics, setMetrics] = useState<ProviderMetrics | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    fetchProviders();
    fetchMetrics();
  }, []);

  const fetchProviders = async () => {
    setIsLoading(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      const mockProviders: AIProvider[] = [
        {
          id: '1',
          name: 'OpenAI GPT-4',
          type: 'openai',
          status: 'active',
          endpoint: 'https://api.openai.com/v1',
          apiKeyMasked: 'sk-...abc123',
          responseTime: 1200,
          uptime: 99.8,
          costPerRequest: 0.03,
          requestsToday: 1250,
          costToday: 37.50,
          errorRate: 0.2,
          rateLimit: 10000,
          rateLimitUsed: 3500,
          lastHealthCheck: '2 minutes ago',
          configuration: {
            timeout: 30000,
            retries: 3,
            priority: 1,
            failoverEnabled: true
          }
        },
        {
          id: '2',
          name: 'Anthropic Claude',
          type: 'anthropic',
          status: 'active',
          endpoint: 'https://api.anthropic.com/v1',
          apiKeyMasked: 'sk-...def456',
          responseTime: 950,
          uptime: 99.9,
          costPerRequest: 0.025,
          requestsToday: 890,
          costToday: 22.25,
          errorRate: 0.1,
          rateLimit: 5000,
          rateLimitUsed: 1200,
          lastHealthCheck: '1 minute ago',
          configuration: {
            timeout: 25000,
            retries: 2,
            priority: 2,
            failoverEnabled: true
          }
        },
        {
          id: '3',
          name: 'Azure OpenAI',
          type: 'azure',
          status: 'error',
          endpoint: 'https://barq-ai.openai.azure.com',
          apiKeyMasked: 'key-...ghi789',
          responseTime: 0,
          uptime: 95.2,
          costPerRequest: 0.028,
          requestsToday: 0,
          costToday: 0,
          errorRate: 100,
          rateLimit: 8000,
          rateLimitUsed: 0,
          lastHealthCheck: '15 minutes ago',
          configuration: {
            timeout: 35000,
            retries: 5,
            priority: 3,
            failoverEnabled: false
          }
        }
      ];
      
      setProviders(mockProviders);
    } catch (error) {
      console.error('Failed to fetch providers:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const fetchMetrics = async () => {
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      
      const mockMetrics: ProviderMetrics = {
        totalProviders: 3,
        activeProviders: 2,
        totalRequests: 2140,
        totalCost: 59.75,
        averageResponseTime: 1075,
        overallUptime: 98.3
      };
      
      setMetrics(mockMetrics);
    } catch (error) {
      console.error('Failed to fetch metrics:', error);
    }
  };

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'active':
        return <CheckCircle className="h-4 w-4 text-green-500" />;
      case 'error':
        return <XCircle className="h-4 w-4 text-red-500" />;
      case 'maintenance':
        return <AlertTriangle className="h-4 w-4 text-yellow-500" />;
      default:
        return <XCircle className="h-4 w-4 text-gray-500" />;
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'active':
        return 'bg-green-100 text-green-800';
      case 'error':
        return 'bg-red-100 text-red-800';
      case 'maintenance':
        return 'bg-yellow-100 text-yellow-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const filteredProviders = providers.filter(provider =>
    provider.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    provider.type.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const handleRefresh = () => {
    fetchProviders();
    fetchMetrics();
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
          <h1 className="text-3xl font-bold tracking-tight">AI Provider Management</h1>
          <p className="text-muted-foreground">
            Configure and monitor AI providers with real-time performance metrics
          </p>
        </div>
        <div className="flex items-center space-x-2">
          <Button variant="outline" onClick={handleRefresh}>
            <RefreshCw className="h-4 w-4 mr-2" />
            Refresh
          </Button>
          <Button onClick={() => console.log('Add provider clicked')}>
            <Plus className="h-4 w-4 mr-2" />
            Add Provider
          </Button>
        </div>
      </div>

      {/* Metrics Overview */}
      {metrics && (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-6">
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Total Providers</CardTitle>
              <Settings className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.totalProviders}</div>
              <p className="text-xs text-muted-foreground">
                {metrics.activeProviders} active
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Requests Today</CardTitle>
              <Activity className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.totalRequests.toLocaleString()}</div>
              <p className="text-xs text-muted-foreground">
                <TrendingUp className="h-3 w-3 inline mr-1" />
                +12% from yesterday
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Cost Today</CardTitle>
              <DollarSign className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">${metrics.totalCost.toFixed(2)}</div>
              <p className="text-xs text-muted-foreground">
                <TrendingDown className="h-3 w-3 inline mr-1" />
                -5% from yesterday
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Avg Response Time</CardTitle>
              <BarChart3 className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.averageResponseTime}ms</div>
              <p className="text-xs text-muted-foreground">
                <TrendingDown className="h-3 w-3 inline mr-1" />
                -8% improvement
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Overall Uptime</CardTitle>
              <CheckCircle className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.overallUptime}%</div>
              <p className="text-xs text-muted-foreground">
                Last 30 days
              </p>
            </CardContent>
          </Card>
          
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Active Providers</CardTitle>
              <Activity className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.activeProviders}</div>
              <p className="text-xs text-muted-foreground">
                {((metrics.activeProviders / metrics.totalProviders) * 100).toFixed(0)}% operational
              </p>
            </CardContent>
          </Card>
        </div>
      )}

      <Tabs defaultValue="providers" className="space-y-4">
        <TabsList>
          <TabsTrigger value="providers">Providers</TabsTrigger>
          <TabsTrigger value="monitoring">Monitoring</TabsTrigger>
          <TabsTrigger value="cost-analysis">Cost Analysis</TabsTrigger>
          <TabsTrigger value="configuration">Configuration</TabsTrigger>
        </TabsList>

        <TabsContent value="providers" className="space-y-4">
          <div className="flex items-center space-x-2">
            <Input
              placeholder="Search providers..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="max-w-sm"
            />
          </div>

          <div className="grid gap-4">
            {filteredProviders.map((provider) => (
              <Card key={provider.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      {getStatusIcon(provider.status)}
                      <CardTitle className="text-lg">{provider.name}</CardTitle>
                      <Badge className={getStatusColor(provider.status)}>
                        {provider.status}
                      </Badge>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Button variant="outline" size="sm">
                        <Eye className="h-4 w-4 mr-2" />
                        View Details
                      </Button>
                      <Button variant="outline" size="sm">
                        <Edit className="h-4 w-4 mr-2" />
                        Configure
                      </Button>
                      <Button variant="outline" size="sm">
                        <Trash2 className="h-4 w-4" />
                      </Button>
                    </div>
                  </div>
                  <CardDescription>
                    {provider.endpoint} • Last checked: {provider.lastHealthCheck}
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                    <div>
                      <Label className="text-sm font-medium">Response Time</Label>
                      <p className="text-2xl font-bold">{provider.responseTime}ms</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Uptime</Label>
                      <p className="text-2xl font-bold">{provider.uptime}%</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Requests Today</Label>
                      <p className="text-2xl font-bold">{provider.requestsToday.toLocaleString()}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Cost Today</Label>
                      <p className="text-2xl font-bold">${provider.costToday.toFixed(2)}</p>
                    </div>
                  </div>
                  
                  <div className="mt-4 grid gap-4 md:grid-cols-2">
                    <div>
                      <Label className="text-sm font-medium">Rate Limit Usage</Label>
                      <div className="mt-1 w-full bg-gray-200 rounded-full h-2">
                        <div 
                          className="bg-blue-600 h-2 rounded-full" 
                          style={{ width: `${(provider.rateLimitUsed / provider.rateLimit) * 100}%` }}
                        ></div>
                      </div>
                      <p className="text-xs text-muted-foreground mt-1">
                        {provider.rateLimitUsed.toLocaleString()} / {provider.rateLimit.toLocaleString()}
                      </p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Error Rate</Label>
                      <p className="text-lg font-semibold text-red-600">{provider.errorRate}%</p>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="monitoring" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Real-time Monitoring</CardTitle>
              <CardDescription>
                Live performance metrics and health status for all AI providers
              </CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground">
                Real-time monitoring dashboard would be implemented here with charts and live metrics.
              </p>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="cost-analysis" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Cost Analysis & Budget Management</CardTitle>
              <CardDescription>
                Track usage costs and manage budgets across all AI providers
              </CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground">
                Cost analysis dashboard with budget tracking and optimization recommendations would be implemented here.
              </p>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="configuration" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Provider Configuration</CardTitle>
              <CardDescription>
                Configure failover settings, rate limits, and optimization parameters
              </CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground">
                Provider configuration interface with failover, load balancing, and optimization settings would be implemented here.
              </p>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
