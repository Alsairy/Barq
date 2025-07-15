import { useState } from 'react';
import { 
  Activity, 
  AlertTriangle, 
  CheckCircle, 
  Globe, 
  RefreshCw, 
  Settings, 
  TrendingUp, 
  Zap,
  XCircle,
  BarChart3,
  Shield,
  Database,
  Wifi,
  Pause,
  MoreHorizontal
} from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card';
import { Button } from '../../components/ui/button';
import { Badge } from '../../components/ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../components/ui/tabs';
import { LoadingSpinner } from '../../components/ui/loading-spinner';
import { Input } from '../../components/ui/input';
import { Label } from '../../components/ui/label';
import { Progress } from '../../components/ui/progress';
import { Alert, AlertDescription } from '../../components/ui/alert';
import {
  useGetHealthDashboardQuery,
  useGetIntegrationEndpointsQuery,
  useGetIntegrationMetricsQuery,
  useGetActiveAlertsQuery,
  useGetQueueStatusQuery,
  useTestConnectionMutation,
  useResolveAlertMutation
} from '../../store/api/integrationApi';

export default function IntegrationDashboardPage() {
  const [searchTerm, setSearchTerm] = useState('');
  const [protocolFilter, setProtocolFilter] = useState<string>('all');
  const [statusFilter, setStatusFilter] = useState<string>('all');
  const [selectedTimeRange, setSelectedTimeRange] = useState('24h');

  const { data: healthDashboard, isLoading: healthLoading, refetch: refetchHealth } = useGetHealthDashboardQuery();
  const { data: endpoints, isLoading: endpointsLoading, refetch: refetchEndpoints } = useGetIntegrationEndpointsQuery({
    search: searchTerm || undefined,
    protocol: protocolFilter !== 'all' ? protocolFilter : undefined,
    status: statusFilter !== 'all' ? statusFilter : undefined,
  });
  const { data: metrics, isLoading: metricsLoading } = useGetIntegrationMetricsQuery({
    fromDate: getTimeRangeStart(selectedTimeRange),
    toDate: new Date().toISOString(),
  });
  const { data: activeAlerts } = useGetActiveAlertsQuery();
  const { data: queueStatus } = useGetQueueStatusQuery();

  const [testConnection] = useTestConnectionMutation();
  const [resolveAlert] = useResolveAlertMutation();

  function getTimeRangeStart(range: string): string {
    const now = new Date();
    switch (range) {
      case '1h':
        return new Date(now.getTime() - 60 * 60 * 1000).toISOString();
      case '24h':
        return new Date(now.getTime() - 24 * 60 * 60 * 1000).toISOString();
      case '7d':
        return new Date(now.getTime() - 7 * 24 * 60 * 60 * 1000).toISOString();
      case '30d':
        return new Date(now.getTime() - 30 * 24 * 60 * 60 * 1000).toISOString();
      default:
        return new Date(now.getTime() - 24 * 60 * 60 * 1000).toISOString();
    }
  }

  const getStatusIcon = (isHealthy: boolean, isActive: boolean) => {
    if (!isActive) return <Pause className="h-4 w-4 text-gray-500" />;
    if (isHealthy) return <CheckCircle className="h-4 w-4 text-green-500" />;
    return <XCircle className="h-4 w-4 text-red-500" />;
  };

  const getStatusColor = (isHealthy: boolean, isActive: boolean) => {
    if (!isActive) return 'bg-gray-100 text-gray-800';
    if (isHealthy) return 'bg-green-100 text-green-800';
    return 'bg-red-100 text-red-800';
  };

  const getProtocolIcon = (protocol: string) => {
    switch (protocol.toLowerCase()) {
      case 'rest':
        return <Globe className="h-4 w-4" />;
      case 'soap':
        return <Database className="h-4 w-4" />;
      case 'graphql':
        return <BarChart3 className="h-4 w-4" />;
      case 'websocket':
        return <Wifi className="h-4 w-4" />;
      default:
        return <Activity className="h-4 w-4" />;
    }
  };

  const getSeverityColor = (severity: string) => {
    switch (severity) {
      case 'critical':
        return 'bg-red-100 text-red-800 border-red-200';
      case 'high':
        return 'bg-orange-100 text-orange-800 border-orange-200';
      case 'medium':
        return 'bg-yellow-100 text-yellow-800 border-yellow-200';
      case 'low':
        return 'bg-blue-100 text-blue-800 border-blue-200';
      default:
        return 'bg-gray-100 text-gray-800 border-gray-200';
    }
  };

  const handleTestConnection = async (endpointId: string) => {
    try {
      const result = await testConnection(endpointId).unwrap();
      if (result.success) {
        alert(`Connection test successful! Response time: ${result.responseTime}ms`);
      } else {
        alert(`Connection test failed: ${result.errorMessage}`);
      }
    } catch {
      alert('Connection test failed: Network error');
    }
  };

  const handleResolveAlert = async (alertId: string) => {
    try {
      await resolveAlert(alertId).unwrap();
      alert('Alert resolved successfully');
    } catch {
      alert('Failed to resolve alert');
    }
  };

  const handleRefreshAll = () => {
    refetchHealth();
    refetchEndpoints();
  };

  const filteredEndpoints = endpoints?.filter(endpoint => {
    const matchesSearch = endpoint.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         endpoint.url.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesProtocol = protocolFilter === 'all' || endpoint.protocol === protocolFilter;
    const matchesStatus = statusFilter === 'all' || 
                         (statusFilter === 'healthy' && endpoint.isHealthy && endpoint.isActive) ||
                         (statusFilter === 'unhealthy' && (!endpoint.isHealthy || !endpoint.isActive)) ||
                         (statusFilter === 'inactive' && !endpoint.isActive);
    
    return matchesSearch && matchesProtocol && matchesStatus;
  }) || [];

  if (healthLoading || endpointsLoading || metricsLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Integration Dashboard</h1>
          <p className="text-muted-foreground">
            Monitor and manage all external system integrations with real-time health checks
          </p>
        </div>
        <div className="flex items-center space-x-2">
          <Button variant="outline" onClick={handleRefreshAll}>
            <RefreshCw className="h-4 w-4 mr-2" />
            Refresh
          </Button>
          <Button>
            <Settings className="h-4 w-4 mr-2" />
            Configure
          </Button>
        </div>
      </div>

      {/* Health Overview Cards */}
      {healthDashboard && (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Total Endpoints</CardTitle>
              <Activity className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{healthDashboard.totalEndpoints}</div>
              <p className="text-xs text-muted-foreground">
                {healthDashboard.healthyEndpoints} healthy, {healthDashboard.unhealthyEndpoints} unhealthy
              </p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Health Score</CardTitle>
              <Shield className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{healthDashboard.overallHealthScore}%</div>
              <div className="mt-2">
                <Progress value={healthDashboard.overallHealthScore} className="h-2" />
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Active Alerts</CardTitle>
              <AlertTriangle className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{healthDashboard.activeAlerts}</div>
              <p className="text-xs text-muted-foreground">
                {healthDashboard.criticalAlerts?.length || 0} critical alerts
              </p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Success Rate</CardTitle>
              <TrendingUp className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">
                {metrics ? `${metrics.successRate.toFixed(1)}%` : 'N/A'}
              </div>
              <p className="text-xs text-muted-foreground">
                Last {selectedTimeRange}
              </p>
            </CardContent>
          </Card>
        </div>
      )}

      {/* Critical Alerts */}
      {activeAlerts && activeAlerts.length > 0 && (
        <Alert className="border-red-200 bg-red-50">
          <AlertTriangle className="h-4 w-4 text-red-600" />
          <AlertDescription className="text-red-800">
            <div className="flex items-center justify-between">
              <span>
                {activeAlerts.filter(alert => alert.severity === 'critical').length} critical alerts require attention
              </span>
              <Button variant="outline" size="sm" className="text-red-700 border-red-300">
                View All Alerts
              </Button>
            </div>
          </AlertDescription>
        </Alert>
      )}

      <Tabs defaultValue="endpoints" className="space-y-4">
        <TabsList>
          <TabsTrigger value="endpoints">Endpoints</TabsTrigger>
          <TabsTrigger value="metrics">Performance Metrics</TabsTrigger>
          <TabsTrigger value="alerts">Alerts & Monitoring</TabsTrigger>
          <TabsTrigger value="queues">Message Queues</TabsTrigger>
        </TabsList>

        <TabsContent value="endpoints" className="space-y-4">
          {/* Filters */}
          <div className="flex items-center space-x-4">
            <div className="flex-1">
              <Input
                placeholder="Search endpoints..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="max-w-sm"
              />
            </div>
            <select
              value={protocolFilter}
              onChange={(e) => setProtocolFilter(e.target.value)}
              className="px-3 py-2 border border-gray-300 rounded-md"
            >
              <option value="all">All Protocols</option>
              <option value="REST">REST</option>
              <option value="SOAP">SOAP</option>
              <option value="GraphQL">GraphQL</option>
              <option value="WebSocket">WebSocket</option>
            </select>
            <select
              value={statusFilter}
              onChange={(e) => setStatusFilter(e.target.value)}
              className="px-3 py-2 border border-gray-300 rounded-md"
            >
              <option value="all">All Status</option>
              <option value="healthy">Healthy</option>
              <option value="unhealthy">Unhealthy</option>
              <option value="inactive">Inactive</option>
            </select>
          </div>

          {/* Endpoints List */}
          <div className="space-y-4">
            {filteredEndpoints.map((endpoint) => (
              <Card key={endpoint.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-3">
                      {getProtocolIcon(endpoint.protocol)}
                      <div>
                        <CardTitle className="text-lg">{endpoint.name}</CardTitle>
                        <CardDescription>{endpoint.description}</CardDescription>
                      </div>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Badge className={getStatusColor(endpoint.isHealthy, endpoint.isActive)}>
                        {getStatusIcon(endpoint.isHealthy, endpoint.isActive)}
                        <span className="ml-1">
                          {!endpoint.isActive ? 'Inactive' : endpoint.isHealthy ? 'Healthy' : 'Unhealthy'}
                        </span>
                      </Badge>
                      <Badge variant="outline">{endpoint.protocol}</Badge>
                    </div>
                  </div>
                </CardHeader>
                <CardContent>
                  <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                    <div>
                      <Label className="text-sm font-medium">URL</Label>
                      <p className="text-sm text-muted-foreground truncate">{endpoint.url}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Last Health Check</Label>
                      <p className="text-sm text-muted-foreground">
                        {new Date(endpoint.lastHealthCheck).toLocaleString()}
                      </p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Timeout</Label>
                      <p className="text-sm text-muted-foreground">{endpoint.timeout}ms</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Authentication</Label>
                      <p className="text-sm text-muted-foreground">{endpoint.authentication.type}</p>
                    </div>
                  </div>
                  
                  <div className="flex items-center justify-between mt-4">
                    <div className="flex items-center space-x-2">
                      {endpoint.tags.map((tag) => (
                        <Badge key={tag} variant="secondary" className="text-xs">
                          {tag}
                        </Badge>
                      ))}
                    </div>
                    <div className="flex items-center space-x-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleTestConnection(endpoint.id)}
                      >
                        <Zap className="h-4 w-4 mr-2" />
                        Test Connection
                      </Button>
                      <Button variant="outline" size="sm">
                        <Settings className="h-4 w-4 mr-2" />
                        Configure
                      </Button>
                      <Button variant="outline" size="sm">
                        <MoreHorizontal className="h-4 w-4" />
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="metrics" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">Performance Metrics</h3>
            <select
              value={selectedTimeRange}
              onChange={(e) => setSelectedTimeRange(e.target.value)}
              className="px-3 py-2 border border-gray-300 rounded-md"
            >
              <option value="1h">Last Hour</option>
              <option value="24h">Last 24 Hours</option>
              <option value="7d">Last 7 Days</option>
              <option value="30d">Last 30 Days</option>
            </select>
          </div>

          {metrics && (
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
              <Card>
                <CardHeader>
                  <CardTitle>Request Volume</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="text-2xl font-bold">{metrics.totalRequests.toLocaleString()}</div>
                  <p className="text-sm text-muted-foreground">
                    {metrics.successfulRequests.toLocaleString()} successful, {metrics.failedRequests.toLocaleString()} failed
                  </p>
                </CardContent>
              </Card>

              <Card>
                <CardHeader>
                  <CardTitle>Average Response Time</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="text-2xl font-bold">{metrics.averageResponseTime.toFixed(0)}ms</div>
                  <p className="text-sm text-muted-foreground">
                    Across all endpoints
                  </p>
                </CardContent>
              </Card>

              <Card>
                <CardHeader>
                  <CardTitle>Success Rate</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="text-2xl font-bold">{metrics.successRate.toFixed(1)}%</div>
                  <div className="mt-2">
                    <Progress value={metrics.successRate} className="h-2" />
                  </div>
                </CardContent>
              </Card>
            </div>
          )}

          <Card>
            <CardHeader>
              <CardTitle>Endpoint Usage</CardTitle>
              <CardDescription>Request distribution across endpoints</CardDescription>
            </CardHeader>
            <CardContent>
              {metrics && Object.entries(metrics.endpointUsage).length > 0 ? (
                <div className="space-y-2">
                  {Object.entries(metrics.endpointUsage).map(([endpoint, count]) => (
                    <div key={endpoint} className="flex items-center justify-between">
                      <span className="text-sm">{endpoint}</span>
                      <span className="text-sm font-medium">{count.toLocaleString()}</span>
                    </div>
                  ))}
                </div>
              ) : (
                <p className="text-muted-foreground">No usage data available</p>
              )}
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="alerts" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">Active Alerts</h3>
            <Button variant="outline">
              <Settings className="h-4 w-4 mr-2" />
              Manage Alert Rules
            </Button>
          </div>

          {activeAlerts && activeAlerts.length > 0 ? (
            <div className="space-y-4">
              {activeAlerts.map((alert) => (
                <Card key={alert.id} className={`border-l-4 ${getSeverityColor(alert.severity)}`}>
                  <CardHeader>
                    <div className="flex items-center justify-between">
                      <div className="flex items-center space-x-2">
                        <AlertTriangle className="h-5 w-5" />
                        <CardTitle className="text-lg">{alert.title}</CardTitle>
                        <Badge className={getSeverityColor(alert.severity)}>
                          {alert.severity.toUpperCase()}
                        </Badge>
                      </div>
                      <div className="flex items-center space-x-2">
                        <Button
                          variant="outline"
                          size="sm"
                          onClick={() => handleResolveAlert(alert.id)}
                        >
                          Resolve
                        </Button>
                        <Button variant="outline" size="sm">
                          <MoreHorizontal className="h-4 w-4" />
                        </Button>
                      </div>
                    </div>
                  </CardHeader>
                  <CardContent>
                    <p className="text-sm text-muted-foreground mb-2">{alert.description}</p>
                    <div className="flex items-center justify-between text-xs text-muted-foreground">
                      <span>Created: {new Date(alert.createdAt).toLocaleString()}</span>
                      <span>Endpoint: {alert.endpointId}</span>
                    </div>
                  </CardContent>
                </Card>
              ))}
            </div>
          ) : (
            <Card>
              <CardContent className="flex items-center justify-center py-12">
                <div className="text-center">
                  <CheckCircle className="h-12 w-12 text-green-500 mx-auto mb-4" />
                  <h3 className="text-lg font-semibold mb-2">No Active Alerts</h3>
                  <p className="text-muted-foreground">All integrations are running smoothly</p>
                </div>
              </CardContent>
            </Card>
          )}
        </TabsContent>

        <TabsContent value="queues" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">Message Queue Status</h3>
            <Button variant="outline">
              <RefreshCw className="h-4 w-4 mr-2" />
              Refresh Queues
            </Button>
          </div>

          {queueStatus && queueStatus.length > 0 ? (
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
              {queueStatus.map((queue) => (
                <Card key={queue.queueName}>
                  <CardHeader>
                    <CardTitle className="text-lg">{queue.queueName}</CardTitle>
                  </CardHeader>
                  <CardContent className="space-y-4">
                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <Label className="text-sm font-medium">Pending</Label>
                        <p className="text-2xl font-bold">{queue.pendingMessages}</p>
                      </div>
                      <div>
                        <Label className="text-sm font-medium">Processing</Label>
                        <p className="text-2xl font-bold">{queue.processingMessages}</p>
                      </div>
                    </div>
                    
                    <div>
                      <Label className="text-sm font-medium">Failed Messages</Label>
                      <p className="text-lg font-semibold text-red-600">{queue.failedMessages}</p>
                    </div>
                    
                    <div>
                      <Label className="text-sm font-medium">Avg Processing Time</Label>
                      <p className="text-sm text-muted-foreground">{queue.averageProcessingTime.toFixed(0)}ms</p>
                    </div>
                    
                    {queue.lastProcessedAt && (
                      <div>
                        <Label className="text-sm font-medium">Last Processed</Label>
                        <p className="text-sm text-muted-foreground">
                          {new Date(queue.lastProcessedAt).toLocaleString()}
                        </p>
                      </div>
                    )}
                  </CardContent>
                </Card>
              ))}
            </div>
          ) : (
            <Card>
              <CardContent className="flex items-center justify-center py-12">
                <div className="text-center">
                  <Database className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                  <h3 className="text-lg font-semibold mb-2">No Message Queues</h3>
                  <p className="text-muted-foreground">No message queues are currently configured</p>
                </div>
              </CardContent>
            </Card>
          )}
        </TabsContent>
      </Tabs>
    </div>
  );
}
