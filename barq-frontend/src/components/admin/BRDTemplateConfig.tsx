import { useState, useEffect } from 'react';
import { FileText, Plus, Edit, Trash2, Save, X } from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Label } from '../ui/label';
import { Textarea } from '../ui/textarea';
import { Badge } from '../ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../ui/tabs';

interface BRDTemplateConfiguration {
  id: string;
  name: string;
  description?: string;
  documentStructure: DocumentSection[];
  sprintConfiguration: SprintConfig;
  userStoryTemplates: UserStoryTemplate[];
  acceptanceCriteria: AcceptanceCriteriaTemplate[];
  stakeholderAnalysis: StakeholderTemplate[];
  businessProcessModeling: ProcessModelTemplate[];
  isActive: boolean;
  priority: number;
}

interface DocumentSection {
  name: string;
  order: number;
  required: boolean;
  template: string;
}

interface SprintConfig {
  duration: number;
  planningHours: number;
  reviewHours: number;
  retrospectiveHours: number;
  dailyStandupMinutes: number;
}

interface UserStoryTemplate {
  format: string;
  requiredFields: string[];
  estimationMethod: 'story-points' | 'hours' | 'tshirt';
}

interface AcceptanceCriteriaTemplate {
  format: 'given-when-then' | 'checklist' | 'scenario';
  template: string;
}

interface StakeholderTemplate {
  role: string;
  responsibilities: string[];
  influence: 'high' | 'medium' | 'low';
}

interface ProcessModelTemplate {
  name: string;
  steps: string[];
  swimlanes: string[];
}

export function BRDTemplateConfig() {
  const [configurations, setConfigurations] = useState<BRDTemplateConfiguration[]>([]);
  const [selectedConfig, setSelectedConfig] = useState<BRDTemplateConfiguration | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [isCreating, setIsCreating] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadConfigurations();
  }, []);

  const loadConfigurations = async () => {
    try {
      setLoading(true);
      const mockConfigurations: BRDTemplateConfiguration[] = [
        {
          id: '1',
          name: 'Enterprise BRD Template',
          description: 'Comprehensive BRD template for enterprise projects',
          documentStructure: [
            { name: 'Executive Summary', order: 1, required: true, template: 'Brief overview of the project...' },
            { name: 'Business Objectives', order: 2, required: true, template: 'Primary business goals...' },
            { name: 'Functional Requirements', order: 3, required: true, template: 'Detailed functional requirements...' },
            { name: 'Non-Functional Requirements', order: 4, required: true, template: 'Performance, security, scalability...' },
            { name: 'User Stories', order: 5, required: true, template: 'As a [user], I want [goal] so that [benefit]...' }
          ],
          sprintConfiguration: {
            duration: 14,
            planningHours: 4,
            reviewHours: 2,
            retrospectiveHours: 1,
            dailyStandupMinutes: 15
          },
          userStoryTemplates: [
            {
              format: 'As a [role], I want [feature] so that [benefit]',
              requiredFields: ['role', 'feature', 'benefit', 'acceptance_criteria'],
              estimationMethod: 'story-points'
            }
          ],
          acceptanceCriteria: [
            {
              format: 'given-when-then',
              template: 'Given [context], When [action], Then [outcome]'
            }
          ],
          stakeholderAnalysis: [
            { role: 'Product Owner', responsibilities: ['Define requirements', 'Prioritize backlog'], influence: 'high' },
            { role: 'Scrum Master', responsibilities: ['Facilitate ceremonies', 'Remove blockers'], influence: 'medium' }
          ],
          businessProcessModeling: [
            {
              name: 'User Registration Process',
              steps: ['User visits registration page', 'User fills form', 'System validates', 'Account created'],
              swimlanes: ['User', 'System', 'Database']
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
    const newConfig: BRDTemplateConfiguration = {
      id: '',
      name: '',
      description: '',
      documentStructure: [],
      sprintConfiguration: {
        duration: 14,
        planningHours: 4,
        reviewHours: 2,
        retrospectiveHours: 1,
        dailyStandupMinutes: 15
      },
      userStoryTemplates: [],
      acceptanceCriteria: [],
      stakeholderAnalysis: [],
      businessProcessModeling: [],
      isActive: true,
      priority: 1
    };
    setSelectedConfig(newConfig);
    setIsCreating(true);
    setIsEditing(true);
  };

  const handleEditConfig = (config: BRDTemplateConfiguration) => {
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
            <FileText className="mx-auto h-8 w-8 text-muted-foreground animate-pulse" />
            <p className="mt-2 text-muted-foreground">Loading BRD templates...</p>
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold">BRD Template Configuration</h2>
          <p className="text-muted-foreground">Manage Business Requirements Document templates and structures</p>
        </div>
        <Button onClick={handleCreateConfig}>
          <Plus className="mr-2 h-4 w-4" />
          New BRD Template
        </Button>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="space-y-4">
          <h3 className="text-lg font-semibold">Existing Templates</h3>
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
                    <span className="text-muted-foreground">Document Sections:</span>
                    <span className="font-medium">{config.documentStructure.length}</span>
                  </div>
                  <div className="flex items-center justify-between text-sm">
                    <span className="text-muted-foreground">Sprint Duration:</span>
                    <span className="font-medium">{config.sprintConfiguration.duration} days</span>
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
                {isCreating ? 'Create New BRD Template' : 'Edit BRD Template'}
              </CardTitle>
              <CardDescription>
                Configure BRD template structure and sprint settings
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-6">
              <div className="space-y-4">
                <div>
                  <Label htmlFor="name">Template Name</Label>
                  <Input
                    id="name"
                    value={selectedConfig.name}
                    onChange={(e) => setSelectedConfig({
                      ...selectedConfig,
                      name: e.target.value
                    })}
                    placeholder="Enter template name"
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
                    placeholder="Enter template description"
                  />
                </div>

                <Tabs defaultValue="structure" className="w-full">
                  <TabsList className="grid w-full grid-cols-4">
                    <TabsTrigger value="structure">Document Structure</TabsTrigger>
                    <TabsTrigger value="sprint">Sprint Config</TabsTrigger>
                    <TabsTrigger value="stories">User Stories</TabsTrigger>
                    <TabsTrigger value="stakeholders">Stakeholders</TabsTrigger>
                  </TabsList>

                  <TabsContent value="structure" className="space-y-4">
                    <div>
                      <Label>Document Sections</Label>
                      <p className="text-sm text-muted-foreground mb-2">
                        Configure the structure and order of BRD sections
                      </p>
                      {/* Document structure configuration would go here */}
                    </div>
                  </TabsContent>

                  <TabsContent value="sprint" className="space-y-4">
                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <Label htmlFor="duration">Sprint Duration (days)</Label>
                        <Input
                          id="duration"
                          type="number"
                          value={selectedConfig.sprintConfiguration.duration}
                          onChange={(e) => setSelectedConfig({
                            ...selectedConfig,
                            sprintConfiguration: {
                              ...selectedConfig.sprintConfiguration,
                              duration: parseInt(e.target.value) || 14
                            }
                          })}
                        />
                      </div>
                      <div>
                        <Label htmlFor="planning">Planning Hours</Label>
                        <Input
                          id="planning"
                          type="number"
                          value={selectedConfig.sprintConfiguration.planningHours}
                          onChange={(e) => setSelectedConfig({
                            ...selectedConfig,
                            sprintConfiguration: {
                              ...selectedConfig.sprintConfiguration,
                              planningHours: parseInt(e.target.value) || 4
                            }
                          })}
                        />
                      </div>
                    </div>
                  </TabsContent>

                  <TabsContent value="stories" className="space-y-4">
                    <p className="text-sm text-muted-foreground">
                      Configure user story templates and acceptance criteria
                    </p>
                    {/* User story configuration would go here */}
                  </TabsContent>

                  <TabsContent value="stakeholders" className="space-y-4">
                    <p className="text-sm text-muted-foreground">
                      Define stakeholder roles and responsibilities
                    </p>
                    {/* Stakeholder configuration would go here */}
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
                  Save Template
                </Button>
              </div>
            </CardContent>
          </Card>
        )}
      </div>
    </div>
  );
}
