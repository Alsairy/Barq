import { useState, useEffect } from 'react';
import { Code, Plus, Edit, Trash2, Save, X } from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Label } from '../ui/label';
import { Textarea } from '../ui/textarea';
import { Badge } from '../ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../ui/tabs';

interface CodeGenerationConfiguration {
  id: string;
  name: string;
  description?: string;
  technologyStack: TechnologyStack;
  architecturalPatterns: ArchitecturalPattern[];
  codingStandards: CodingStandard[];
  qualityRequirements: QualityRequirement[];
  versionRequirements: VersionRequirement[];
  isActive: boolean;
  priority: number;
}

interface TechnologyStack {
  backend: string[];
  frontend: string[];
  database: string[];
  cloud: string[];
  testing: string[];
}

interface ArchitecturalPattern {
  name: string;
  description: string;
  enforced: boolean;
}

interface CodingStandard {
  category: string;
  rules: string[];
  severity: 'error' | 'warning' | 'info';
}

interface QualityRequirement {
  metric: string;
  threshold: number;
  unit: string;
}

interface VersionRequirement {
  technology: string;
  minVersion: string;
  maxVersion?: string;
  preferred: string;
}

export function CodeGenerationConfig() {
  const [configurations, setConfigurations] = useState<CodeGenerationConfiguration[]>([]);
  const [selectedConfig, setSelectedConfig] = useState<CodeGenerationConfiguration | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [isCreating, setIsCreating] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadConfigurations();
  }, []);

  const loadConfigurations = async () => {
    try {
      setLoading(true);
      const mockConfigurations: CodeGenerationConfiguration[] = [
        {
          id: '1',
          name: '.NET 8 Enterprise Stack',
          description: 'Standard enterprise configuration for .NET 8 applications',
          technologyStack: {
            backend: ['.NET 8', 'ASP.NET Core', 'Entity Framework Core'],
            frontend: ['React', 'TypeScript', 'Tailwind CSS'],
            database: ['SQL Server', 'Redis'],
            cloud: ['Azure', 'Docker'],
            testing: ['xUnit', 'Moq', 'FluentAssertions']
          },
          architecturalPatterns: [
            { name: 'Clean Architecture', description: 'Layered architecture with dependency inversion', enforced: true },
            { name: 'CQRS', description: 'Command Query Responsibility Segregation', enforced: false },
            { name: 'Repository Pattern', description: 'Data access abstraction', enforced: true }
          ],
          codingStandards: [
            { category: 'Naming', rules: ['PascalCase for classes', 'camelCase for variables'], severity: 'error' },
            { category: 'Documentation', rules: ['XML comments for public APIs'], severity: 'warning' }
          ],
          qualityRequirements: [
            { metric: 'Code Coverage', threshold: 80, unit: '%' },
            { metric: 'Cyclomatic Complexity', threshold: 10, unit: 'max' }
          ],
          versionRequirements: [
            { technology: '.NET', minVersion: '8.0', preferred: '8.0.1' },
            { technology: 'React', minVersion: '18.0', preferred: '18.2.0' }
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
    const newConfig: CodeGenerationConfiguration = {
      id: '',
      name: '',
      description: '',
      technologyStack: {
        backend: [],
        frontend: [],
        database: [],
        cloud: [],
        testing: []
      },
      architecturalPatterns: [],
      codingStandards: [],
      qualityRequirements: [],
      versionRequirements: [],
      isActive: true,
      priority: 1
    };
    setSelectedConfig(newConfig);
    setIsCreating(true);
    setIsEditing(true);
  };

  const handleEditConfig = (config: CodeGenerationConfiguration) => {
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
            <Code className="mx-auto h-8 w-8 text-muted-foreground animate-pulse" />
            <p className="mt-2 text-muted-foreground">Loading configurations...</p>
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold">Code Generation Configuration</h2>
          <p className="text-muted-foreground">Manage AI-powered code generation templates and constraints</p>
        </div>
        <Button onClick={handleCreateConfig}>
          <Plus className="mr-2 h-4 w-4" />
          New Configuration
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
                <div className="flex flex-wrap gap-2">
                  {config.technologyStack.backend.slice(0, 3).map((tech) => (
                    <Badge key={tech} variant="outline">{tech}</Badge>
                  ))}
                  {config.technologyStack.backend.length > 3 && (
                    <Badge variant="outline">+{config.technologyStack.backend.length - 3} more</Badge>
                  )}
                </div>
              </CardContent>
            </Card>
          ))}
        </div>

        {(isEditing && selectedConfig) && (
          <Card>
            <CardHeader>
              <CardTitle>
                {isCreating ? 'Create New Configuration' : 'Edit Configuration'}
              </CardTitle>
              <CardDescription>
                Configure code generation parameters and constraints
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

                <Tabs defaultValue="technology" className="w-full">
                  <TabsList className="grid w-full grid-cols-4">
                    <TabsTrigger value="technology">Technology Stack</TabsTrigger>
                    <TabsTrigger value="patterns">Architecture</TabsTrigger>
                    <TabsTrigger value="standards">Standards</TabsTrigger>
                    <TabsTrigger value="quality">Quality</TabsTrigger>
                  </TabsList>

                  <TabsContent value="technology" className="space-y-4">
                    <div>
                      <Label>Backend Technologies</Label>
                      <Input
                        placeholder="Enter technologies (comma-separated)"
                        value={selectedConfig.technologyStack.backend.join(', ')}
                        onChange={(e) => setSelectedConfig({
                          ...selectedConfig,
                          technologyStack: {
                            ...selectedConfig.technologyStack,
                            backend: e.target.value.split(',').map(s => s.trim()).filter(Boolean)
                          }
                        })}
                      />
                    </div>
                    <div>
                      <Label>Frontend Technologies</Label>
                      <Input
                        placeholder="Enter technologies (comma-separated)"
                        value={selectedConfig.technologyStack.frontend.join(', ')}
                        onChange={(e) => setSelectedConfig({
                          ...selectedConfig,
                          technologyStack: {
                            ...selectedConfig.technologyStack,
                            frontend: e.target.value.split(',').map(s => s.trim()).filter(Boolean)
                          }
                        })}
                      />
                    </div>
                  </TabsContent>

                  <TabsContent value="patterns" className="space-y-4">
                    <p className="text-sm text-muted-foreground">
                      Configure architectural patterns and design principles
                    </p>
                    {/* Architecture patterns configuration would go here */}
                  </TabsContent>

                  <TabsContent value="standards" className="space-y-4">
                    <p className="text-sm text-muted-foreground">
                      Define coding standards and conventions
                    </p>
                    {/* Coding standards configuration would go here */}
                  </TabsContent>

                  <TabsContent value="quality" className="space-y-4">
                    <p className="text-sm text-muted-foreground">
                      Set quality requirements and thresholds
                    </p>
                    {/* Quality requirements configuration would go here */}
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
