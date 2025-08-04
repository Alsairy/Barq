import { useState, useEffect } from 'react';
import { Shield, Plus, Edit, Trash2, Save, X } from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Label } from '../ui/label';
import { Textarea } from '../ui/textarea';
import { Badge } from '../ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../ui/tabs';

interface TestingSecurityConfiguration {
  id: string;
  name: string;
  description?: string;
  testingFrameworks: TestingFramework[];
  securityProtocols: SecurityProtocol[];
  complianceRequirements: ComplianceRequirement[];
  qualityGates: QualityGate[];
  automationSettings: AutomationSettings;
  reportingConfiguration: ReportingConfiguration;
  isActive: boolean;
  priority: number;
}

interface TestingFramework {
  name: string;
  type: 'unit' | 'integration' | 'e2e' | 'performance' | 'security';
  configuration: Record<string, any>;
  coverage: number;
}

interface SecurityProtocol {
  name: string;
  category: 'authentication' | 'authorization' | 'encryption' | 'audit';
  requirements: string[];
  implementation: string;
}

interface ComplianceRequirement {
  standard: 'GDPR' | 'HIPAA' | 'SOX' | 'PCI-DSS' | 'ISO27001';
  requirements: string[];
  validationRules: string[];
}

interface QualityGate {
  name: string;
  metric: string;
  threshold: number;
  operator: 'gt' | 'lt' | 'eq' | 'gte' | 'lte';
  blocking: boolean;
}

interface AutomationSettings {
  cicdIntegration: boolean;
  automaticScanning: boolean;
  scheduleFrequency: 'daily' | 'weekly' | 'monthly';
  notifications: string[];
}

interface ReportingConfiguration {
  format: 'html' | 'pdf' | 'json' | 'xml';
  recipients: string[];
  frequency: 'immediate' | 'daily' | 'weekly';
  includeMetrics: string[];
}

export function TestingSecurityConfig() {
  const [configurations, setConfigurations] = useState<TestingSecurityConfiguration[]>([]);
  const [selectedConfig, setSelectedConfig] = useState<TestingSecurityConfiguration | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [isCreating, setIsCreating] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadConfigurations();
  }, []);

  const loadConfigurations = async () => {
    try {
      setLoading(true);
      const mockConfigurations: TestingSecurityConfiguration[] = [
        {
          id: '1',
          name: 'Enterprise Security &amp; Testing',
          description: 'Comprehensive security and testing configuration for enterprise applications',
          testingFrameworks: [
            {
              name: 'xUnit',
              type: 'unit',
              configuration: { parallel: true, timeout: 30000 },
              coverage: 85
            },
            {
              name: 'Playwright',
              type: 'e2e',
              configuration: { browsers: ['chromium', 'firefox'], headless: true },
              coverage: 70
            }
          ],
          securityProtocols: [
            {
              name: 'OAuth 2.0 + OIDC',
              category: 'authentication',
              requirements: ['Multi-factor authentication', 'Token refresh', 'Secure storage'],
              implementation: 'Azure AD integration with custom claims'
            },
            {
              name: 'AES-256 Encryption',
              category: 'encryption',
              requirements: ['Data at rest encryption', 'Data in transit encryption'],
              implementation: 'Azure Key Vault managed encryption'
            }
          ],
          complianceRequirements: [
            {
              standard: 'GDPR',
              requirements: ['Data portability', 'Right to be forgotten', 'Consent management'],
              validationRules: ['Personal data identification', 'Consent tracking', 'Data retention policies']
            }
          ],
          qualityGates: [
            {
              name: 'Code Coverage',
              metric: 'coverage_percentage',
              threshold: 80,
              operator: 'gte',
              blocking: true
            },
            {
              name: 'Security Vulnerabilities',
              metric: 'high_severity_vulnerabilities',
              threshold: 0,
              operator: 'eq',
              blocking: true
            }
          ],
          automationSettings: {
            cicdIntegration: true,
            automaticScanning: true,
            scheduleFrequency: 'daily',
            notifications: ['security-team@company.com', 'dev-team@company.com']
          },
          reportingConfiguration: {
            format: 'html',
            recipients: ['qa-team@company.com', 'security-team@company.com'],
            frequency: 'daily',
            includeMetrics: ['coverage', 'vulnerabilities', 'performance', 'compliance']
          },
          isActive: true,
          priority: 1
        }
      ];
      setConfigurations(mockConfigurations);
    } catch (error) {
      console.error('Failed to load configurations:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleCreateConfig = () => {
    const newConfig: TestingSecurityConfiguration = {
      id: '',
      name: '',
      description: '',
      testingFrameworks: [],
      securityProtocols: [],
      complianceRequirements: [],
      qualityGates: [],
      automationSettings: {
        cicdIntegration: false,
        automaticScanning: false,
        scheduleFrequency: 'daily',
        notifications: []
      },
      reportingConfiguration: {
        format: 'html',
        recipients: [],
        frequency: 'daily',
        includeMetrics: []
      },
      isActive: true,
      priority: 1
    };
    setSelectedConfig(newConfig);
    setIsCreating(true);
    setIsEditing(true);
  };

  const handleEditConfig = (config: TestingSecurityConfiguration) => {
    setSelectedConfig({ ...config });
    setIsEditing(true);
    setIsCreating(false);
  };

  const handleSaveConfig = async () => {
    if (!selectedConfig) return;

    try {
      if (isCreating) {
        const newConfig = { ...selectedConfig, id: Date.now().toString() };
        setConfigurations([...configurations, newConfig]);
      } else {
        setConfigurations(configurations.map(c => 
          c.id === selectedConfig.id ? selectedConfig : c
        ));
      }
      
      setIsEditing(false);
      setIsCreating(false);
      setSelectedConfig(null);
    } catch (error) {
      console.error('Failed to save configuration:', error);
    }
  };

  const handleDeleteConfig = async (configId: string) => {
    try {
      setConfigurations(configurations.filter(c => c.id !== configId));
    } catch (error) {
      console.error('Failed to delete configuration:', error);
    }
  };

  const handleCancel = () => {
    setIsEditing(false);
    setIsCreating(false);
    setSelectedConfig(null);
  };

  if (loading) {
    return (
      <Card>
        <CardContent className="flex items-center justify-center py-8">
          <div className="text-center">
            <Shield className="mx-auto h-8 w-8 text-muted-foreground animate-pulse" />
            <p className="mt-2 text-muted-foreground">Loading testing &amp; security configurations...</p>
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold">Testing &amp; Security Configuration</h2>
          <p className="text-muted-foreground">Configure testing frameworks and security settings</p>
        </div>
        <Button onClick={handleCreateConfig}>
          <Plus className="mr-2 h-4 w-4" />
          New Testing &amp; Security Config
        </Button>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="space-y-4">
          <h3 className="text-lg font-semibold">Existing Configurations</h3>
          {configurations.map((config) => (
            <Card key={config.id} className="cursor-pointer hover:shadow-md transition-shadow">
              <CardHeader className="pb-3">
                <div className="flex items-center justify-between">
                  <CardTitle className="text-base">{config.name}</CardTitle>
                  <div className="flex items-center space-x-2">
                    <Badge variant={config.isActive ? "default" : "secondary"}>
                      {config.isActive ? 'Active' : 'Inactive'}
                    </Badge>
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => handleEditConfig(config)}
                    >
                      <Edit className="h-4 w-4" />
                    </Button>
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => handleDeleteConfig(config.id)}
                    >
                      <Trash2 className="h-4 w-4" />
                    </Button>
                  </div>
                </div>
                {config.description && (
                  <CardDescription>{config.description}</CardDescription>
                )}
              </CardHeader>
              <CardContent className="pt-0">
                <div className="space-y-2">
                  <div className="flex items-center justify-between text-sm">
                    <span className="text-muted-foreground">Testing Frameworks:</span>
                    <span className="font-medium">{config.testingFrameworks.length}</span>
                  </div>
                  <div className="flex items-center justify-between text-sm">
                    <span className="text-muted-foreground">Security Protocols:</span>
                    <span className="font-medium">{config.securityProtocols.length}</span>
                  </div>
                  <div className="flex items-center justify-between text-sm">
                    <span className="text-muted-foreground">Quality Gates:</span>
                    <span className="font-medium">{config.qualityGates.length}</span>
                  </div>
                </div>
              </CardContent>
            </Card>
          ))}
        </div>

        {(isEditing && selectedConfig) && (
          <Card>
            <CardHeader>
              <CardTitle>
                {isCreating ? 'Create New Testing & Security Configuration' : 'Edit Testing & Security Configuration'}
              </CardTitle>
              <CardDescription>
                Configure testing frameworks, security protocols, and compliance requirements
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-6">
              <div className="space-y-4">
                <div>
                  <Label htmlFor="name">Configuration Name</Label>
                  <Input
                    id="name"
                    value={selectedConfig.name}
                    onChange={(e) => setSelectedConfig({
                      ...selectedConfig,
                      name: e.target.value
                    })}
                    placeholder="Enter configuration name"
                  />
                </div>

                <div>
                  <Label htmlFor="description">Description</Label>
                  <Textarea
                    id="description"
                    value={selectedConfig.description || ''}
                    onChange={(e) => setSelectedConfig({
                      ...selectedConfig,
                      description: e.target.value
                    })}
                    placeholder="Enter configuration description"
                  />
                </div>

                <Tabs defaultValue="testing" className="w-full">
                  <TabsList className="grid w-full grid-cols-4">
                    <TabsTrigger value="testing">Testing</TabsTrigger>
                    <TabsTrigger value="security">Security</TabsTrigger>
                    <TabsTrigger value="compliance">Compliance</TabsTrigger>
                    <TabsTrigger value="automation">Automation</TabsTrigger>
                  </TabsList>

                  <TabsContent value="testing" className="space-y-4">
                    <div>
                      <Label>Testing Frameworks</Label>
                      <p className="text-sm text-muted-foreground mb-2">
                        Configure testing frameworks and quality gates
                      </p>
                      {/* Testing frameworks configuration would go here */}
                    </div>
                  </TabsContent>

                  <TabsContent value="security" className="space-y-4">
                    <div>
                      <Label>Security Protocols</Label>
                      <p className="text-sm text-muted-foreground mb-2">
                        Configure security protocols and encryption settings
                      </p>
                      {/* Security protocols configuration would go here */}
                    </div>
                  </TabsContent>

                  <TabsContent value="compliance" className="space-y-4">
                    <div>
                      <Label>Compliance Requirements</Label>
                      <p className="text-sm text-muted-foreground mb-2">
                        Configure compliance standards and validation rules
                      </p>
                      {/* Compliance configuration would go here */}
                    </div>
                  </TabsContent>

                  <TabsContent value="automation" className="space-y-4">
                    <div>
                      <Label>Automation Settings</Label>
                      <p className="text-sm text-muted-foreground mb-2">
                        Configure CI/CD integration and automated scanning
                      </p>
                      {/* Automation settings configuration would go here */}
                    </div>
                  </TabsContent>
                </Tabs>
              </div>

              <div className="flex justify-end space-x-2">
                <Button variant="outline" onClick={handleCancel}>
                  <X className="mr-2 h-4 w-4" />
                  Cancel
                </Button>
                <Button onClick={handleSaveConfig}>
                  <Save className="mr-2 h-4 w-4" />
                  Save Configuration
                </Button>
              </div>
            </CardContent>
          </Card>
        )}
      </div>
    </div>
  );
}
