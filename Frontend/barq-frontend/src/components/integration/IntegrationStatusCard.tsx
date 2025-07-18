import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Badge } from '../ui/badge';
import { Button } from '../ui/button';
import { CheckCircle, XCircle, AlertTriangle, Clock, RefreshCw } from 'lucide-react';

interface IntegrationStatusCardProps {
  name: string;
  status: 'connected' | 'disconnected' | 'error' | 'pending';
  description?: string;
  lastSync?: string;
  onTest?: () => void;
  onRefresh?: () => void;
  metrics?: {
    uptime: string;
    responseTime: string;
    errorRate: string;
  };
}

const getStatusIcon = (status: string) => {
  switch (status) {
    case 'connected':
      return <CheckCircle className="h-4 w-4 text-green-600" />;
    case 'disconnected':
      return <XCircle className="h-4 w-4 text-red-600" />;
    case 'error':
      return <AlertTriangle className="h-4 w-4 text-yellow-600" />;
    case 'pending':
      return <Clock className="h-4 w-4 text-blue-600" />;
    default:
      return <XCircle className="h-4 w-4 text-gray-400" />;
  }
};

const getStatusColor = (status: string) => {
  switch (status) {
    case 'connected':
      return 'bg-green-100 text-green-800 border-green-200';
    case 'disconnected':
      return 'bg-red-100 text-red-800 border-red-200';
    case 'error':
      return 'bg-yellow-100 text-yellow-800 border-yellow-200';
    case 'pending':
      return 'bg-blue-100 text-blue-800 border-blue-200';
    default:
      return 'bg-gray-100 text-gray-800 border-gray-200';
  }
};

export function IntegrationStatusCard({
  name,
  status,
  description,
  lastSync,
  onTest,
  onRefresh,
  metrics
}: IntegrationStatusCardProps) {
  return (
    <Card>
      <CardHeader>
        <div className="flex items-center justify-between">
          <div className="flex items-center space-x-2">
            {getStatusIcon(status)}
            <CardTitle className="text-lg">{name}</CardTitle>
          </div>
          <Badge className={getStatusColor(status)}>
            {status.charAt(0).toUpperCase() + status.slice(1)}
          </Badge>
        </div>
        {description && (
          <CardDescription>{description}</CardDescription>
        )}
      </CardHeader>
      <CardContent className="space-y-4">
        {metrics && (
          <div className="grid grid-cols-3 gap-4">
            <div>
              <div className="text-sm font-medium text-muted-foreground">Uptime</div>
              <div className="text-lg font-semibold">{metrics.uptime}</div>
            </div>
            <div>
              <div className="text-sm font-medium text-muted-foreground">Response Time</div>
              <div className="text-lg font-semibold">{metrics.responseTime}</div>
            </div>
            <div>
              <div className="text-sm font-medium text-muted-foreground">Error Rate</div>
              <div className="text-lg font-semibold">{metrics.errorRate}</div>
            </div>
          </div>
        )}
        
        {lastSync && (
          <div className="text-sm text-muted-foreground">
            Last sync: {lastSync}
          </div>
        )}
        
        <div className="flex items-center space-x-2">
          {onTest && (
            <Button variant="outline" size="sm" onClick={onTest}>
              Test Connection
            </Button>
          )}
          {onRefresh && (
            <Button variant="outline" size="sm" onClick={onRefresh}>
              <RefreshCw className="h-4 w-4 mr-2" />
              Refresh
            </Button>
          )}
        </div>
      </CardContent>
    </Card>
  );
}
