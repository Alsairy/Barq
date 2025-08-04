import { useState, useEffect } from 'react';
import { FileType, Plus, Edit, Trash2, Save, X } from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Label } from '../ui/label';
import { Textarea } from '../ui/textarea';
import { Badge } from '../ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../ui/tabs';

interface ProposalConfiguration {
  id: string;
  name: string;
  description?: string;
  proposalTemplates: ProposalTemplate[];
  technicalSpecifications: TechnicalSpec[];
  financialModeling: FinancialModel[];
  riskAssessment: RiskAssessment[];
  implementationPlanning: ImplementationPlan[];
  competitivePositioning: CompetitiveAnalysis[];
  isActive: boolean;
  priority: number;
}

interface ProposalTemplate {
  name: string;
  sections: string[];
  format: 'pdf' | 'docx' | 'html';
}

interface TechnicalSpec {
  category: string;
  requirements: string[];
  constraints: string[];
}

interface FinancialModel {
  type: 'cost-benefit' | 'roi' | 'npv' | 'payback';
  parameters: Record<string, any>;
}

interface RiskAssessment {
  category: string;
  risks: Risk[];
}

interface Risk {
  description: string;
  probability: 'low' | 'medium' | 'high';
  impact: 'low' | 'medium' | 'high';
  mitigation: string;
}

interface ImplementationPlan {
  phase: string;
  duration: number;
  resources: string[];
  deliverables: string[];
}

interface CompetitiveAnalysis {
  competitor: string;
  strengths: string[];
  weaknesses: string[];
  positioning: string;
}

export function ProposalConfig() {
  const [configurations, setConfigurations] = useState<ProposalConfiguration[]>([]);
  const [selectedConfig, setSelectedConfig] = useState<ProposalConfiguration | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [isCreating, setIsCreating] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadConfigurations();
  }, []);

  const loadConfigurations = async () => {
    try {
      setLoading(true);
      const mockConfigurations: ProposalConfiguration[] = [
        {
          id: '1',
          name: 'Enterprise Software Proposal',
          description: 'Standard template for enterprise software development proposals',
          proposalTemplates: [
            {
              name: 'Technical Proposal',
              sections: ['Executive Summary', 'Technical Approach', 'Timeline', 'Budget'],
              format: 'pdf'
            }
          ],
          technicalSpecifications: [
            {
              category: 'Architecture',
              requirements: ['Scalable microservices', 'Cloud-native deployment'],
              constraints: ['Budget limitations', 'Timeline constraints']
            }
          ],
          financialModeling: [
            {
              type: 'roi',
              parameters: { investment: 100000, returns: 150000, period: 12 }
            }
          ],
          riskAssessment: [
            {
              category: 'Technical',
              risks: [
                {
                  description: 'Technology adoption challenges',
                  probability: 'medium',
                  impact: 'high',
                  mitigation: 'Comprehensive training program'
                }
              ]
            }
          ],
          implementationPlanning: [
            {
              phase: 'Phase 1 - Foundation',
              duration: 3,
              resources: ['2 Senior Developers', '1 Architect'],
              deliverables: ['System Architecture', 'Core Framework']
            }
          ],
          competitivePositioning: [
            {
              competitor: 'Competitor A',
              strengths: ['Market presence', 'Brand recognition'],
              weaknesses: ['Higher cost', 'Legacy technology'],
              positioning: 'Modern, cost-effective alternative'
            }
          ],
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
    const newConfig: ProposalConfiguration = {
      id: '',
      name: '',
      description: '',
      proposalTemplates: [],
      technicalSpecifications: [],
      financialModeling: [],
      riskAssessment: [],
      implementationPlanning: [],
      competitivePositioning: [],
      isActive: true,
      priority: 1
    };
    setSelectedConfig(newConfig);
    setIsCreating(true);
    setIsEditing(true);
  };

  const handleEditConfig = (config: ProposalConfiguration) => {
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
            <FileType className="mx-auto h-8 w-8 text-muted-foreground animate-pulse" />
            <p className="mt-2 text-muted-foreground">Loading proposal configurations...</p>
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold">Proposal Configuration</h2>
          <p className="text-muted-foreground">Configure proposal generation templates and requirements</p>
        </div>
        <Button onClick={handleCreateConfig}>
          <Plus className="mr-2 h-4 w-4" />
          New Proposal Config
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
                    <span className="text-muted-foreground">Templates:</span>
                    <span className="font-medium">{config.proposalTemplates.length}</span>
                  </div>
                  <div className="flex items-center justify-between text-sm">
                    <span className="text-muted-foreground">Implementation Phases:</span>
                    <span className="font-medium">{config.implementationPlanning.length}</span>
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
                {isCreating ? 'Create New Proposal Configuration' : 'Edit Proposal Configuration'}
              </CardTitle>
              <CardDescription>
                Configure proposal templates and business requirements
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

                <Tabs defaultValue="templates" className="w-full">
                  <TabsList className="grid w-full grid-cols-4">
                    <TabsTrigger value="templates">Templates</TabsTrigger>
                    <TabsTrigger value="technical">Technical</TabsTrigger>
                    <TabsTrigger value="financial">Financial</TabsTrigger>
                    <TabsTrigger value="implementation">Implementation</TabsTrigger>
                  </TabsList>

                  <TabsContent value="templates" className="space-y-4">
                    <div>
                      <Label>Proposal Templates</Label>
                      <p className="text-sm text-muted-foreground mb-2">
                        Configure proposal document templates and formats
                      </p>
                      {/* Template configuration would go here */}
                    </div>
                  </TabsContent>

                  <TabsContent value="technical" className="space-y-4">
                    <div>
                      <Label>Technical Specifications</Label>
                      <p className="text-sm text-muted-foreground mb-2">
                        Define technical requirements and constraints
                      </p>
                      {/* Technical specs configuration would go here */}
                    </div>
                  </TabsContent>

                  <TabsContent value="financial" className="space-y-4">
                    <div>
                      <Label>Financial Modeling</Label>
                      <p className="text-sm text-muted-foreground mb-2">
                        Configure financial analysis and ROI calculations
                      </p>
                      {/* Financial modeling configuration would go here */}
                    </div>
                  </TabsContent>

                  <TabsContent value="implementation" className="space-y-4">
                    <div>
                      <Label>Implementation Planning</Label>
                      <p className="text-sm text-muted-foreground mb-2">
                        Define implementation phases and deliverables
                      </p>
                      {/* Implementation planning configuration would go here */}
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
