import { useState } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Label } from '../../components/ui/label';
import { Badge } from '../../components/ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../components/ui/tabs';
import { Progress } from '../../components/ui/progress';
import { 
  Shield, 
  Lock, 
  Key, 
  Eye, 
  AlertTriangle, 
  CheckCircle, 
  XCircle, 
  Clock, 
  Settings, 
  RefreshCw,
  FileText,
  Users,
  Download,
  Database,
  UserCheck,
  AlertCircle
} from 'lucide-react';

interface SecurityPolicy {
  id: string;
  name: string;
  type: 'authentication' | 'authorization' | 'encryption' | 'audit' | 'compliance';
  status: 'active' | 'inactive' | 'pending' | 'violation';
  severity: 'low' | 'medium' | 'high' | 'critical';
  description: string;
  lastUpdated: string;
  violationCount: number;
  complianceScore: number;
}

interface SecurityAlert {
  id: string;
  title: string;
  type: 'threat' | 'vulnerability' | 'compliance' | 'access' | 'data';
  severity: 'low' | 'medium' | 'high' | 'critical';
  status: 'open' | 'investigating' | 'resolved' | 'false_positive';
  description: string;
  timestamp: string;
  affectedSystems: string[];
  assignedTo?: string;
}

interface ComplianceFramework {
  id: string;
  name: string;
  type: 'gdpr' | 'hipaa' | 'sox' | 'iso27001' | 'pci_dss';
  status: 'compliant' | 'non_compliant' | 'partial' | 'pending';
  score: number;
  lastAssessment: string;
  requirements: number;
  compliantRequirements: number;
  criticalFindings: number;
}

interface AuditLog {
  id: string;
  timestamp: string;
  user: string;
  action: string;
  resource: string;
  result: 'success' | 'failure' | 'warning';
  ipAddress: string;
  userAgent: string;
  details: string;
}

export default function SecurityCompliancePage() {
  const [activeTab, setActiveTab] = useState('policies');
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedSeverity, setSelectedSeverity] = useState<string>('all');

  const securityPolicies: SecurityPolicy[] = [
    {
      id: '1',
      name: 'Multi-Factor Authentication',
      type: 'authentication',
      status: 'active',
      severity: 'high',
      description: 'Enforce MFA for all user accounts',
      lastUpdated: '2024-01-15T10:30:00Z',
      violationCount: 0,
      complianceScore: 98
    },
    {
      id: '2',
      name: 'Data Encryption at Rest',
      type: 'encryption',
      status: 'active',
      severity: 'critical',
      description: 'Encrypt all sensitive data stored in databases',
      lastUpdated: '2024-01-15T09:15:00Z',
      violationCount: 2,
      complianceScore: 95
    },
    {
      id: '3',
      name: 'Role-Based Access Control',
      type: 'authorization',
      status: 'violation',
      severity: 'medium',
      description: 'Implement RBAC for system access',
      lastUpdated: '2024-01-14T16:45:00Z',
      violationCount: 5,
      complianceScore: 87
    }
  ];

  const securityAlerts: SecurityAlert[] = [
    {
      id: '1',
      title: 'Suspicious Login Activity Detected',
      type: 'threat',
      severity: 'high',
      status: 'investigating',
      description: 'Multiple failed login attempts from unusual IP addresses',
      timestamp: '2024-01-15T10:45:00Z',
      affectedSystems: ['Authentication Service', 'User Management'],
      assignedTo: 'Security Team'
    },
    {
      id: '2',
      title: 'Outdated SSL Certificate',
      type: 'vulnerability',
      severity: 'medium',
      status: 'open',
      description: 'SSL certificate for api.barq.com expires in 7 days',
      timestamp: '2024-01-15T08:30:00Z',
      affectedSystems: ['API Gateway', 'Web Application'],
      assignedTo: 'DevOps Team'
    },
    {
      id: '3',
      title: 'GDPR Data Retention Violation',
      type: 'compliance',
      severity: 'critical',
      status: 'open',
      description: 'User data retained beyond policy limits',
      timestamp: '2024-01-15T07:15:00Z',
      affectedSystems: ['User Database', 'Analytics Service']
    }
  ];

  const complianceFrameworks: ComplianceFramework[] = [
    {
      id: '1',
      name: 'GDPR',
      type: 'gdpr',
      status: 'compliant',
      score: 94,
      lastAssessment: '2024-01-10T00:00:00Z',
      requirements: 25,
      compliantRequirements: 23,
      criticalFindings: 0
    },
    {
      id: '2',
      name: 'ISO 27001',
      type: 'iso27001',
      status: 'partial',
      score: 78,
      lastAssessment: '2024-01-08T00:00:00Z',
      requirements: 114,
      compliantRequirements: 89,
      criticalFindings: 3
    },
    {
      id: '3',
      name: 'SOX',
      type: 'sox',
      status: 'non_compliant',
      score: 65,
      lastAssessment: '2024-01-05T00:00:00Z',
      requirements: 18,
      compliantRequirements: 12,
      criticalFindings: 2
    }
  ];

  const auditLogs: AuditLog[] = [
    {
      id: '1',
      timestamp: '2024-01-15T10:45:00Z',
      user: 'john.doe@company.com',
      action: 'LOGIN',
      resource: 'Authentication Service',
      result: 'success',
      ipAddress: '192.168.1.100',
      userAgent: 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)',
      details: 'Successful login with MFA'
    },
    {
      id: '2',
      timestamp: '2024-01-15T10:30:00Z',
      user: 'admin@company.com',
      action: 'UPDATE_POLICY',
      resource: 'Security Policy: MFA',
      result: 'success',
      ipAddress: '10.0.0.50',
      userAgent: 'Mozilla/5.0 (macOS; Intel Mac OS X 10_15_7)',
      details: 'Updated MFA policy settings'
    },
    {
      id: '3',
      timestamp: '2024-01-15T10:15:00Z',
      user: 'unknown',
      action: 'LOGIN_ATTEMPT',
      resource: 'Authentication Service',
      result: 'failure',
      ipAddress: '203.0.113.45',
      userAgent: 'curl/7.68.0',
      details: 'Failed login attempt - invalid credentials'
    }
  ];

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'active':
      case 'compliant':
      case 'success':
        return <CheckCircle className="h-4 w-4 text-green-500" />;
      case 'investigating':
      case 'partial':
      case 'pending':
        return <Clock className="h-4 w-4 text-yellow-500" />;
      case 'violation':
      case 'non_compliant':
      case 'open':
      case 'failure':
        return <XCircle className="h-4 w-4 text-red-500" />;
      case 'inactive':
      case 'resolved':
        return <CheckCircle className="h-4 w-4 text-gray-500" />;
      default:
        return <AlertTriangle className="h-4 w-4 text-gray-500" />;
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'active':
      case 'compliant':
      case 'success':
        return 'bg-green-100 text-green-800 border-green-200';
      case 'investigating':
      case 'partial':
      case 'pending':
        return 'bg-yellow-100 text-yellow-800 border-yellow-200';
      case 'violation':
      case 'non_compliant':
      case 'open':
      case 'failure':
        return 'bg-red-100 text-red-800 border-red-200';
      case 'inactive':
      case 'resolved':
        return 'bg-gray-100 text-gray-800 border-gray-200';
      default:
        return 'bg-gray-100 text-gray-800 border-gray-200';
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

  const getTypeIcon = (type: string) => {
    switch (type) {
      case 'authentication':
        return <UserCheck className="h-5 w-5" />;
      case 'authorization':
        return <Users className="h-5 w-5" />;
      case 'encryption':
        return <Lock className="h-5 w-5" />;
      case 'audit':
        return <FileText className="h-5 w-5" />;
      case 'compliance':
        return <Shield className="h-5 w-5" />;
      case 'threat':
        return <AlertTriangle className="h-5 w-5" />;
      case 'vulnerability':
        return <AlertCircle className="h-5 w-5" />;
      case 'access':
        return <Key className="h-5 w-5" />;
      case 'data':
        return <Database className="h-5 w-5" />;
      default:
        return <Shield className="h-5 w-5" />;
    }
  };

  const handleResolveAlert = (alertId: string) => {
    alert(`Resolving alert ${alertId}...`);
  };

  const handleUpdatePolicy = (policyId: string) => {
    alert(`Updating policy ${policyId}...`);
  };

  const handleRunAssessment = (frameworkId: string) => {
    alert(`Running compliance assessment for ${frameworkId}...`);
  };

  const handleExportAuditLog = () => {
    alert('Exporting audit log...');
  };

  return (
    <div className="container mx-auto p-6 space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Security & Compliance</h1>
          <p className="text-muted-foreground">
            Monitor security policies, compliance frameworks, and audit trails
          </p>
        </div>
        <Button>
          <Shield className="h-4 w-4 mr-2" />
          Security Settings
        </Button>
      </div>

      <Tabs value={activeTab} onValueChange={setActiveTab} className="space-y-4">
        <TabsList>
          <TabsTrigger value="policies">Security Policies</TabsTrigger>
          <TabsTrigger value="alerts">Security Alerts</TabsTrigger>
          <TabsTrigger value="compliance">Compliance</TabsTrigger>
          <TabsTrigger value="audit">Audit Logs</TabsTrigger>
        </TabsList>

        <TabsContent value="policies" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">Security Policies</h3>
            <div className="flex items-center space-x-2">
              <Input
                placeholder="Search policies..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="max-w-sm"
              />
              <Button variant="outline">
                <Shield className="h-4 w-4 mr-2" />
                Create Policy
              </Button>
            </div>
          </div>

          <div className="grid gap-4 md:grid-cols-1 lg:grid-cols-2">
            {securityPolicies.map((policy) => (
              <Card key={policy.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      {getTypeIcon(policy.type)}
                      <CardTitle className="text-lg">{policy.name}</CardTitle>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Badge className={getSeverityColor(policy.severity)}>
                        {policy.severity.toUpperCase()}
                      </Badge>
                      <Badge className={getStatusColor(policy.status)}>
                        {getStatusIcon(policy.status)}
                        <span className="ml-1 capitalize">{policy.status}</span>
                      </Badge>
                    </div>
                  </div>
                  <CardDescription>
                    {policy.description}
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div>
                    <Label className="text-sm font-medium mb-2 block">Compliance Score</Label>
                    <div className="flex items-center space-x-2">
                      <Progress value={policy.complianceScore} className="flex-1 h-2" />
                      <span className="text-sm font-medium">{policy.complianceScore}%</span>
                    </div>
                  </div>

                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <Label className="text-sm font-medium">Violations</Label>
                      <p className={`text-lg font-semibold ${policy.violationCount > 0 ? 'text-red-600' : 'text-green-600'}`}>
                        {policy.violationCount}
                      </p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Last Updated</Label>
                      <p className="text-sm text-muted-foreground">
                        {new Date(policy.lastUpdated).toLocaleDateString()}
                      </p>
                    </div>
                  </div>

                  <div className="flex items-center justify-between">
                    <div className="text-sm text-muted-foreground">
                      Type: {policy.type.replace('_', ' ').toUpperCase()}
                    </div>
                    <div className="flex items-center space-x-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleUpdatePolicy(policy.id)}
                      >
                        <Settings className="h-4 w-4 mr-2" />
                        Configure
                      </Button>
                      <Button variant="outline" size="sm">
                        <Eye className="h-4 w-4 mr-2" />
                        View Details
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="alerts" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">Security Alerts</h3>
            <div className="flex items-center space-x-2">
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
              <Button variant="outline">
                <RefreshCw className="h-4 w-4 mr-2" />
                Refresh
              </Button>
            </div>
          </div>

          <div className="space-y-4">
            {securityAlerts.map((alert) => (
              <Card key={alert.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      {getTypeIcon(alert.type)}
                      <CardTitle className="text-lg">{alert.title}</CardTitle>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Badge className={getSeverityColor(alert.severity)}>
                        {alert.severity.toUpperCase()}
                      </Badge>
                      <Badge className={getStatusColor(alert.status)}>
                        {getStatusIcon(alert.status)}
                        <span className="ml-1 capitalize">{alert.status.replace('_', ' ')}</span>
                      </Badge>
                    </div>
                  </div>
                  <CardDescription>
                    {alert.description}
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <Label className="text-sm font-medium">Timestamp</Label>
                      <p className="text-sm text-muted-foreground">
                        {new Date(alert.timestamp).toLocaleString()}
                      </p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Assigned To</Label>
                      <p className="text-sm text-muted-foreground">
                        {alert.assignedTo || 'Unassigned'}
                      </p>
                    </div>
                  </div>

                  <div>
                    <Label className="text-sm font-medium">Affected Systems</Label>
                    <div className="flex flex-wrap gap-1 mt-1">
                      {alert.affectedSystems.map((system) => (
                        <Badge key={system} variant="secondary" className="text-xs">
                          {system}
                        </Badge>
                      ))}
                    </div>
                  </div>

                  <div className="flex items-center justify-between">
                    <div className="text-sm text-muted-foreground">
                      Type: {alert.type.toUpperCase()}
                    </div>
                    <div className="flex items-center space-x-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleResolveAlert(alert.id)}
                        disabled={alert.status === 'resolved'}
                      >
                        <CheckCircle className="h-4 w-4 mr-2" />
                        Resolve
                      </Button>
                      <Button variant="outline" size="sm">
                        <Eye className="h-4 w-4 mr-2" />
                        Investigate
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="compliance" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">Compliance Frameworks</h3>
            <Button variant="outline">
              <FileText className="h-4 w-4 mr-2" />
              Generate Report
            </Button>
          </div>

          <div className="grid gap-4 md:grid-cols-1 lg:grid-cols-2">
            {complianceFrameworks.map((framework) => (
              <Card key={framework.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      <Shield className="h-5 w-5" />
                      <CardTitle className="text-lg">{framework.name}</CardTitle>
                    </div>
                    <Badge className={getStatusColor(framework.status)}>
                      {getStatusIcon(framework.status)}
                      <span className="ml-1 capitalize">{framework.status.replace('_', ' ')}</span>
                    </Badge>
                  </div>
                  <CardDescription>
                    Last assessment: {new Date(framework.lastAssessment).toLocaleDateString()}
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div>
                    <Label className="text-sm font-medium mb-2 block">Compliance Score</Label>
                    <div className="flex items-center space-x-2">
                      <Progress value={framework.score} className="flex-1 h-2" />
                      <span className="text-sm font-medium">{framework.score}%</span>
                    </div>
                  </div>

                  <div className="grid grid-cols-3 gap-4">
                    <div>
                      <Label className="text-sm font-medium">Requirements</Label>
                      <p className="text-lg font-semibold">{framework.requirements}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Compliant</Label>
                      <p className="text-lg font-semibold text-green-600">{framework.compliantRequirements}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Critical Issues</Label>
                      <p className={`text-lg font-semibold ${framework.criticalFindings > 0 ? 'text-red-600' : 'text-green-600'}`}>
                        {framework.criticalFindings}
                      </p>
                    </div>
                  </div>

                  <div className="flex items-center justify-between">
                    <div className="text-sm text-muted-foreground">
                      {framework.compliantRequirements}/{framework.requirements} requirements met
                    </div>
                    <div className="flex items-center space-x-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleRunAssessment(framework.id)}
                      >
                        <RefreshCw className="h-4 w-4 mr-2" />
                        Run Assessment
                      </Button>
                      <Button variant="outline" size="sm">
                        <Eye className="h-4 w-4 mr-2" />
                        View Details
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="audit" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">Audit Logs</h3>
            <div className="flex items-center space-x-2">
              <Input
                placeholder="Search audit logs..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="max-w-sm"
              />
              <Button variant="outline" onClick={handleExportAuditLog}>
                <Download className="h-4 w-4 mr-2" />
                Export
              </Button>
            </div>
          </div>

          <div className="space-y-2">
            {auditLogs.map((log) => (
              <Card key={log.id}>
                <CardContent className="p-4">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-4">
                      {getStatusIcon(log.result)}
                      <div>
                        <div className="flex items-center space-x-2">
                          <span className="font-medium">{log.action}</span>
                          <Badge variant="secondary" className="text-xs">
                            {log.resource}
                          </Badge>
                        </div>
                        <div className="text-sm text-muted-foreground">
                          {log.user} • {log.ipAddress} • {new Date(log.timestamp).toLocaleString()}
                        </div>
                      </div>
                    </div>
                    <div className="text-right">
                      <Badge className={getStatusColor(log.result)}>
                        {log.result.toUpperCase()}
                      </Badge>
                      <div className="text-xs text-muted-foreground mt-1">
                        {log.details}
                      </div>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>
      </Tabs>
    </div>
  );
}
