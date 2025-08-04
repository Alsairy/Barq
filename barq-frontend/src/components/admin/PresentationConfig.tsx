import { useState, useEffect } from 'react';
import { Presentation, Plus, Edit, Trash2, Save, X } from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Label } from '../ui/label';
import { Textarea } from '../ui/textarea';
import { Badge } from '../ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../ui/tabs';

interface PresentationConfiguration {
  id: string;
  name: string;
  description?: string;
  presentationTemplates: PresentationTemplate[];
  slideStructure: SlideStructure[];
  designBranding: DesignBranding;
  contentDepth: ContentDepth;
  audienceCustomization: AudienceCustomization[];
  interactiveElements: InteractiveElement[];
  isActive: boolean;
  priority: number;
}

interface PresentationTemplate {
  name: string;
  theme: string;
  slideCount: number;
  format: 'pptx' | 'pdf' | 'html';
}

interface SlideStructure {
  slideType: 'title' | 'content' | 'comparison' | 'chart' | 'conclusion';
  layout: string;
  requiredElements: string[];
}

interface DesignBranding {
  primaryColor: string;
  secondaryColor: string;
  fontFamily: string;
  logoPosition: 'top-left' | 'top-right' | 'bottom-left' | 'bottom-right';
}

interface ContentDepth {
  level: 'executive' | 'detailed' | 'technical';
  bulletPoints: number;
  detailLevel: string;
}

interface AudienceCustomization {
  audienceType: string;
  contentFocus: string[];
  languageStyle: 'formal' | 'casual' | 'technical';
}

interface InteractiveElement {
  type: 'poll' | 'quiz' | 'demo' | 'discussion';
  placement: string;
  duration: number;
}

export function PresentationConfig() {
  const [configurations, setConfigurations] = useState<PresentationConfiguration[]>([]);
  const [selectedConfig, setSelectedConfig] = useState<PresentationConfiguration | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [isCreating, setIsCreating] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadConfigurations();
  }, []);

  const loadConfigurations = async () => {
    try {
      setLoading(true);
      const mockConfigurations: PresentationConfiguration[] = [
        {
          id: '1',
          name: 'Executive Presentation Template',
          description: 'Professional template for executive-level presentations',
          presentationTemplates: [
            {
              name: 'Executive Summary',
              theme: 'Corporate Blue',
              slideCount: 15,
              format: 'pptx'
            }
          ],
          slideStructure: [
            {
              slideType: 'title',
              layout: 'centered',
              requiredElements: ['title', 'subtitle', 'presenter', 'date']
            },
            {
              slideType: 'content',
              layout: 'bullet-points',
              requiredElements: ['heading', 'content', 'footer']
            }
          ],
          designBranding: {
            primaryColor: '#1e40af',
            secondaryColor: '#64748b',
            fontFamily: 'Arial',
            logoPosition: 'top-right'
          },
          contentDepth: {
            level: 'executive',
            bulletPoints: 5,
            detailLevel: 'High-level overview with key metrics'
          },
          audienceCustomization: [
            {
              audienceType: 'C-Level Executives',
              contentFocus: ['ROI', 'Strategic Impact', 'Risk Mitigation'],
              languageStyle: 'formal'
            }
          ],
          interactiveElements: [
            {
              type: 'poll',
              placement: 'mid-presentation',
              duration: 2
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
    const newConfig: PresentationConfiguration = {
      id: '',
      name: '',
      description: '',
      presentationTemplates: [],
      slideStructure: [],
      designBranding: {
        primaryColor: '#1e40af',
        secondaryColor: '#64748b',
        fontFamily: 'Arial',
        logoPosition: 'top-right'
      },
      contentDepth: {
        level: 'executive',
        bulletPoints: 5,
        detailLevel: ''
      },
      audienceCustomization: [],
      interactiveElements: [],
      isActive: true,
      priority: 1
    };
    setSelectedConfig(newConfig);
    setIsCreating(true);
    setIsEditing(true);
  };

  const handleEditConfig = (config: PresentationConfiguration) => {
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
            <Presentation className="mx-auto h-8 w-8 text-muted-foreground animate-pulse" />
            <p className="mt-2 text-muted-foreground">Loading presentation configurations...</p>
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold">Presentation Configuration</h2>
          <p className="text-muted-foreground">Configure presentation templates and design settings</p>
        </div>
        <Button onClick={handleCreateConfig}>
          <Plus className="mr-2 h-4 w-4" />
          New Presentation Config
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
                    <span className="font-medium">{config.presentationTemplates.length}</span>
                  </div>
                  <div className="flex items-center justify-between text-sm">
                    <span className="text-muted-foreground">Content Level:</span>
                    <span className="font-medium capitalize">{config.contentDepth.level}</span>
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
                {isCreating ? 'Create New Presentation Configuration' : 'Edit Presentation Configuration'}
              </CardTitle>
              <CardDescription>
                Configure presentation templates and design settings
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
                    <TabsTrigger value="design">Design</TabsTrigger>
                    <TabsTrigger value="content">Content</TabsTrigger>
                    <TabsTrigger value="interactive">Interactive</TabsTrigger>
                  </TabsList>

                  <TabsContent value="templates" className="space-y-4">
                    <div>
                      <Label>Presentation Templates</Label>
                      <p className="text-sm text-muted-foreground mb-2">
                        Configure presentation templates and slide structures
                      </p>
                      {/* Template configuration would go here */}
                    </div>
                  </TabsContent>

                  <TabsContent value="design" className="space-y-4">
                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <Label htmlFor="primaryColor">Primary Color</Label>
                        <Input
                          id="primaryColor"
                          type="color"
                          value={selectedConfig.designBranding.primaryColor}
                          onChange={(e) => setSelectedConfig({
                            ...selectedConfig,
                            designBranding: {
                              ...selectedConfig.designBranding,
                              primaryColor: e.target.value
                            }
                          })}
                        />
                      </div>
                      <div>
                        <Label htmlFor="secondaryColor">Secondary Color</Label>
                        <Input
                          id="secondaryColor"
                          type="color"
                          value={selectedConfig.designBranding.secondaryColor}
                          onChange={(e) => setSelectedConfig({
                            ...selectedConfig,
                            designBranding: {
                              ...selectedConfig.designBranding,
                              secondaryColor: e.target.value
                            }
                          })}
                        />
                      </div>
                    </div>
                  </TabsContent>

                  <TabsContent value="content" className="space-y-4">
                    <div>
                      <Label>Content Depth Configuration</Label>
                      <p className="text-sm text-muted-foreground mb-2">
                        Configure content depth and audience customization
                      </p>
                      {/* Content configuration would go here */}
                    </div>
                  </TabsContent>

                  <TabsContent value="interactive" className="space-y-4">
                    <div>
                      <Label>Interactive Elements</Label>
                      <p className="text-sm text-muted-foreground mb-2">
                        Configure interactive elements and engagement features
                      </p>
                      {/* Interactive elements configuration would go here */}
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
