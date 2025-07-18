import React, { useState } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Badge } from '../ui/badge';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { AlertTriangle, CheckCircle, XCircle, Clock, Search } from 'lucide-react';

interface Alert {
  id: string;
  title: string;
  description: string;
  severity: 'low' | 'medium' | 'high' | 'critical';
  status: 'open' | 'acknowledged' | 'resolved';
  timestamp: string;
  source: string;
  category: string;
}

interface AlertsListProps {
  alerts: Alert[];
  onResolve?: (alertId: string) => void;
  onAcknowledge?: (alertId: string) => void;
  showSearch?: boolean;
  showFilter?: boolean;
  maxHeight?: string;
}

const getSeverityIcon = (severity: string) => {
  switch (severity) {
    case 'critical':
      return <XCircle className="h-4 w-4 text-red-600" />;
    case 'high':
      return <AlertTriangle className="h-4 w-4 text-orange-600" />;
    case 'medium':
      return <AlertTriangle className="h-4 w-4 text-yellow-600" />;
    case 'low':
      return <AlertTriangle className="h-4 w-4 text-blue-600" />;
    default:
      return <AlertTriangle className="h-4 w-4 text-gray-400" />;
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

const getStatusIcon = (status: string) => {
  switch (status) {
    case 'resolved':
      return <CheckCircle className="h-4 w-4 text-green-600" />;
    case 'acknowledged':
      return <Clock className="h-4 w-4 text-yellow-600" />;
    case 'open':
      return <AlertTriangle className="h-4 w-4 text-red-600" />;
    default:
      return <AlertTriangle className="h-4 w-4 text-gray-400" />;
  }
};

const getStatusColor = (status: string) => {
  switch (status) {
    case 'resolved':
      return 'bg-green-100 text-green-800 border-green-200';
    case 'acknowledged':
      return 'bg-yellow-100 text-yellow-800 border-yellow-200';
    case 'open':
      return 'bg-red-100 text-red-800 border-red-200';
    default:
      return 'bg-gray-100 text-gray-800 border-gray-200';
  }
};

export function AlertsList({
  alerts,
  onResolve,
  onAcknowledge,
  showSearch = true,
  showFilter = true,
  maxHeight = '400px'
}: AlertsListProps) {
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedSeverity, setSelectedSeverity] = useState<string>('all');
  const [selectedStatus, setSelectedStatus] = useState<string>('all');

  const filteredAlerts = alerts.filter(alert => {
    const matchesSearch = searchTerm === '' || 
      alert.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
      alert.description.toLowerCase().includes(searchTerm.toLowerCase()) ||
      alert.source.toLowerCase().includes(searchTerm.toLowerCase());
    
    const matchesSeverity = selectedSeverity === 'all' || alert.severity === selectedSeverity;
    const matchesStatus = selectedStatus === 'all' || alert.status === selectedStatus;
    
    return matchesSearch && matchesSeverity && matchesStatus;
  });

  return (
    <Card>
      <CardHeader>
        <CardTitle>Alerts</CardTitle>
        <CardDescription>
          {filteredAlerts.length} of {alerts.length} alerts
        </CardDescription>
        
        {(showSearch || showFilter) && (
          <div className="flex items-center space-x-2">
            {showSearch && (
              <div className="relative flex-1">
                <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
                <Input
                  placeholder="Search alerts..."
                  value={searchTerm}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) => setSearchTerm(e.target.value)}
                  className="pl-8"
                />
              </div>
            )}
            
            {showFilter && (
              <>
                <select
                  value={selectedSeverity}
                  onChange={(e) => setSelectedSeverity(e.target.value)}
                  className="px-3 py-2 border border-gray-300 rounded-md text-sm"
                >
                  <option value="all">All Severities</option>
                  <option value="critical">Critical</option>
                  <option value="high">High</option>
                  <option value="medium">Medium</option>
                  <option value="low">Low</option>
                </select>
                
                <select
                  value={selectedStatus}
                  onChange={(e) => setSelectedStatus(e.target.value)}
                  className="px-3 py-2 border border-gray-300 rounded-md text-sm"
                >
                  <option value="all">All Statuses</option>
                  <option value="open">Open</option>
                  <option value="acknowledged">Acknowledged</option>
                  <option value="resolved">Resolved</option>
                </select>
              </>
            )}
          </div>
        )}
      </CardHeader>
      
      <CardContent>
        <div 
          className="space-y-3 overflow-y-auto"
          style={{ maxHeight }}
        >
          {filteredAlerts.length === 0 ? (
            <div className="text-center text-muted-foreground py-8">
              No alerts match your criteria
            </div>
          ) : (
            filteredAlerts.map((alert) => (
              <div
                key={alert.id}
                className="border rounded-lg p-4 space-y-3 hover:bg-gray-50 transition-colors"
              >
                <div className="flex items-start justify-between">
                  <div className="flex items-start space-x-3 flex-1">
                    {getSeverityIcon(alert.severity)}
                    <div className="flex-1">
                      <div className="flex items-center space-x-2 mb-1">
                        <h4 className="font-medium">{alert.title}</h4>
                        <Badge className={getSeverityColor(alert.severity)}>
                          {alert.severity.toUpperCase()}
                        </Badge>
                        <Badge className={getStatusColor(alert.status)}>
                          {getStatusIcon(alert.status)}
                          <span className="ml-1 capitalize">{alert.status}</span>
                        </Badge>
                      </div>
                      <p className="text-sm text-muted-foreground mb-2">
                        {alert.description}
                      </p>
                      <div className="flex items-center space-x-4 text-xs text-muted-foreground">
                        <span>Source: {alert.source}</span>
                        <span>Category: {alert.category}</span>
                        <span>{new Date(alert.timestamp).toLocaleString()}</span>
                      </div>
                    </div>
                  </div>
                  
                  <div className="flex items-center space-x-2 ml-4">
                    {alert.status === 'open' && onAcknowledge && (
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => onAcknowledge(alert.id)}
                      >
                        Acknowledge
                      </Button>
                    )}
                    {alert.status !== 'resolved' && onResolve && (
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => onResolve(alert.id)}
                      >
                        <CheckCircle className="h-4 w-4 mr-2" />
                        Resolve
                      </Button>
                    )}
                  </div>
                </div>
              </div>
            ))
          )}
        </div>
      </CardContent>
    </Card>
  );
}
