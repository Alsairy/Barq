import { useState, useEffect } from 'react';
import { 
  Settings, 
  Copy,
  Edit,
  Plus,
  Zap,
  Target,
  Clock,
  RefreshCw,
  TestTube
} from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../../components/ui/card';
import { Button } from '../../../components/ui/button';
import { Badge } from '../../../components/ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../../components/ui/tabs';
import { LoadingSpinner } from '../../../components/ui/loading-spinner';
import { Input } from '../../../components/ui/input';
import { Label } from '../../../components/ui/label';

interface ModelConfiguration {
  id: string;
  name: string;
  provider: string;
  model: string;
  version: string;
  parameters: {
    temperature: number;
    maxTokens: number;
    topP: number;
    frequencyPenalty: number;
    presencePenalty: number;
    timeout: number;
    retries: number;
  };
  isActive: boolean;
  lastModified: string;
  createdBy: string;
}

interface PromptTemplate {
  id: string;
  name: string;
  description: string;
  category: string;
  template: string;
  variables: string[];
  version: string;
  isActive: boolean;
  usageCount: number;
  lastUsed: string;
  createdBy: string;
}

interface OptimizationRecommendation {
  id: string;
  type: 'performance' | 'cost' | 'quality';
  title: string;
  description: string;
  impact: 'low' | 'medium' | 'high';
  effort: 'low' | 'medium' | 'high';
  estimatedImprovement: string;
  recommendation: string;
}

export default function AIConfigurationPage() {
  const [configurations, setConfigurations] = useState<ModelConfiguration[]>([]);
  const [prompts, setPrompts] = useState<PromptTemplate[]>([]);
  const [recommendations, setRecommendations] = useState<OptimizationRecommendation[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    fetchConfigurations();
    fetchPromptTemplates();
    fetchOptimizationRecommendations();
  }, []);

  const fetchConfigurations = async () => {
    setIsLoading(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      const mockConfigurations: ModelConfiguration[] = [
        {
          id: '1',
          name: 'GPT-4 Production Config',
          provider: 'OpenAI',
          model: 'gpt-4',
          version: '1.0.0',
          parameters: {
            temperature: 0.7,
            maxTokens: 2048,
            topP: 0.9,
            frequencyPenalty: 0.0,
            presencePenalty: 0.0,
            timeout: 30000,
            retries: 3
          },
          isActive: true,
          lastModified: '2024-01-15T10:30:00Z',
          createdBy: 'admin@company.com'
        },
        {
          id: '2',
          name: 'Claude Creative Writing',
          provider: 'Anthropic',
          model: 'claude-3-opus',
          version: '2.1.0',
          parameters: {
            temperature: 0.9,
            maxTokens: 4096,
            topP: 0.95,
            frequencyPenalty: 0.1,
            presencePenalty: 0.1,
            timeout: 45000,
            retries: 2
          },
          isActive: true,
          lastModified: '2024-01-14T15:20:00Z',
          createdBy: 'content.team@company.com'
        },
        {
          id: '3',
          name: 'Azure OpenAI Analysis',
          provider: 'Azure',
          model: 'gpt-35-turbo',
          version: '1.5.0',
          parameters: {
            temperature: 0.3,
            maxTokens: 1024,
            topP: 0.8,
            frequencyPenalty: 0.0,
            presencePenalty: 0.0,
            timeout: 25000,
            retries: 5
          },
          isActive: false,
          lastModified: '2024-01-13T09:45:00Z',
          createdBy: 'analytics.team@company.com'
        }
      ];
      
      setConfigurations(mockConfigurations);
    } catch (error) {
      console.error('Failed to fetch configurations:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const fetchPromptTemplates = async () => {
    try {
      await new Promise(resolve => setTimeout(resolve, 800));
      
      const mockPrompts: PromptTemplate[] = [
        {
          id: '1',
          name: 'Document Summarization',
          description: 'Template for summarizing long documents with key points extraction',
          category: 'summarization',
          template: 'Please summarize the following document, highlighting the key points and main conclusions:\n\n{document_content}\n\nSummary should be approximately {word_count} words and focus on {focus_areas}.',
          variables: ['document_content', 'word_count', 'focus_areas'],
          version: '2.0',
          isActive: true,
          usageCount: 1247,
          lastUsed: '2024-01-15T10:15:00Z',
          createdBy: 'content.team@company.com'
        },
        {
          id: '2',
          name: 'Code Review Assistant',
          description: 'Template for automated code review and improvement suggestions',
          category: 'code-analysis',
          template: 'Review the following {language} code for:\n1. Security vulnerabilities\n2. Performance issues\n3. Best practices\n4. Code quality\n\nCode:\n```{language}\n{code_content}\n```\n\nProvide specific recommendations for improvement.',
          variables: ['language', 'code_content'],
          version: '1.3',
          isActive: true,
          usageCount: 892,
          lastUsed: '2024-01-15T09:30:00Z',
          createdBy: 'dev.team@company.com'
        },
        {
          id: '3',
          name: 'Customer Support Response',
          description: 'Template for generating helpful customer support responses',
          category: 'customer-service',
          template: 'Generate a helpful and professional response to the following customer inquiry:\n\nCustomer Message: {customer_message}\nProduct/Service: {product_service}\nCustomer Tier: {customer_tier}\n\nResponse should be {tone} and include {include_elements}.',
          variables: ['customer_message', 'product_service', 'customer_tier', 'tone', 'include_elements'],
          version: '1.8',
          isActive: true,
          usageCount: 2156,
          lastUsed: '2024-01-15T11:00:00Z',
          createdBy: 'support.team@company.com'
        }
      ];
      
      setPrompts(mockPrompts);
    } catch (error) {
      console.error('Failed to fetch prompt templates:', error);
    }
  };

  const fetchOptimizationRecommendations = async () => {
    try {
      await new Promise(resolve => setTimeout(resolve, 600));
      
      const mockRecommendations: OptimizationRecommendation[] = [
        {
          id: '1',
          type: 'performance',
          title: 'Reduce Temperature for Analytical Tasks',
          description: 'Lower temperature settings for data analysis tasks can improve consistency and accuracy',
          impact: 'medium',
          effort: 'low',
          estimatedImprovement: '15% accuracy improvement',
          recommendation: 'Set temperature to 0.3 for analytical and factual tasks'
        },
        {
          id: '2',
          type: 'cost',
          title: 'Optimize Token Usage',
          description: 'Implement dynamic max token limits based on task complexity',
          impact: 'high',
          effort: 'medium',
          estimatedImprovement: '25% cost reduction',
          recommendation: 'Use adaptive token limits: 512 for simple tasks, 2048 for complex ones'
        },
        {
          id: '3',
          type: 'quality',
          title: 'Implement Prompt Versioning',
          description: 'Version control for prompts will improve quality tracking and rollback capabilities',
          impact: 'medium',
          effort: 'high',
          estimatedImprovement: '20% quality consistency',
          recommendation: 'Implement Git-based prompt versioning with A/B testing'
        }
      ];
      
      setRecommendations(mockRecommendations);
    } catch (error) {
      console.error('Failed to fetch optimization recommendations:', error);
    }
  };

  const getImpactColor = (impact: string) => {
    switch (impact) {
      case 'high':
        return 'bg-red-100 text-red-800';
      case 'medium':
        return 'bg-yellow-100 text-yellow-800';
      default:
        return 'bg-green-100 text-green-800';
    }
  };

  const getEffortColor = (effort: string) => {
    switch (effort) {
      case 'high':
        return 'bg-red-100 text-red-800';
      case 'medium':
        return 'bg-yellow-100 text-yellow-800';
      default:
        return 'bg-green-100 text-green-800';
    }
  };

  const getTypeIcon = (type: string) => {
    switch (type) {
      case 'performance':
        return <Zap className="h-4 w-4" />;
      case 'cost':
        return <Target className="h-4 w-4" />;
      default:
        return <Settings className="h-4 w-4" />;
    }
  };

  const handleRefresh = () => {
    fetchConfigurations();
    fetchPromptTemplates();
    fetchOptimizationRecommendations();
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
          <h1 className="text-3xl font-bold tracking-tight">AI Configuration & Optimization</h1>
          <p className="text-muted-foreground">
            Configure AI models, manage prompts, and optimize performance
          </p>
        </div>
        <div className="flex items-center space-x-2">
          <Button variant="outline" onClick={handleRefresh}>
            <RefreshCw className="h-4 w-4 mr-2" />
            Refresh
          </Button>
          <Button>
            <Plus className="h-4 w-4 mr-2" />
            New Configuration
          </Button>
        </div>
      </div>

      <Tabs defaultValue="models" className="space-y-4">
        <TabsList>
          <TabsTrigger value="models">Model Configuration</TabsTrigger>
          <TabsTrigger value="prompts">Prompt Management</TabsTrigger>
          <TabsTrigger value="optimization">Optimization</TabsTrigger>
          <TabsTrigger value="testing">Testing Framework</TabsTrigger>
        </TabsList>

        <TabsContent value="models" className="space-y-4">
          <div className="grid gap-4">
            {configurations.map((config) => (
              <Card key={config.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div>
                      <CardTitle className="text-lg">{config.name}</CardTitle>
                      <CardDescription>
                        {config.provider} • {config.model} • Version {config.version}
                      </CardDescription>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Badge variant={config.isActive ? 'default' : 'secondary'}>
                        {config.isActive ? 'Active' : 'Inactive'}
                      </Badge>
                      <Button variant="outline" size="sm">
                        <Copy className="h-4 w-4 mr-2" />
                        Clone
                      </Button>
                      <Button variant="outline" size="sm">
                        <Edit className="h-4 w-4 mr-2" />
                        Edit
                      </Button>
                      <Button variant="outline" size="sm">
                        <TestTube className="h-4 w-4 mr-2" />
                        Test
                      </Button>
                    </div>
                  </div>
                </CardHeader>
                <CardContent>
                  <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                    <div>
                      <Label className="text-sm font-medium">Temperature</Label>
                      <p className="text-lg font-semibold">{config.parameters.temperature}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Max Tokens</Label>
                      <p className="text-lg font-semibold">{config.parameters.maxTokens.toLocaleString()}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Top P</Label>
                      <p className="text-lg font-semibold">{config.parameters.topP}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Timeout</Label>
                      <p className="text-lg font-semibold">{config.parameters.timeout / 1000}s</p>
                    </div>
                  </div>
                  
                  <div className="mt-4 text-sm text-muted-foreground">
                    Last modified: {new Date(config.lastModified).toLocaleString()} by {config.createdBy}
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="prompts" className="space-y-4">
          <div className="flex items-center justify-between">
            <Input
              placeholder="Search prompt templates..."
              className="max-w-sm"
            />
            <Button>
              <Plus className="h-4 w-4 mr-2" />
              New Template
            </Button>
          </div>

          <div className="grid gap-4">
            {prompts.map((prompt) => (
              <Card key={prompt.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div>
                      <CardTitle className="text-lg">{prompt.name}</CardTitle>
                      <CardDescription>{prompt.description}</CardDescription>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Badge variant="outline">{prompt.category}</Badge>
                      <Badge variant="secondary">v{prompt.version}</Badge>
                      <Button variant="outline" size="sm">
                        <Copy className="h-4 w-4 mr-2" />
                        Clone
                      </Button>
                      <Button variant="outline" size="sm">
                        <Edit className="h-4 w-4 mr-2" />
                        Edit
                      </Button>
                    </div>
                  </div>
                </CardHeader>
                <CardContent>
                  <div className="space-y-3">
                    <div>
                      <Label className="text-sm font-medium">Template</Label>
                      <div className="mt-1 p-3 bg-gray-50 rounded-md">
                        <code className="text-sm">{prompt.template}</code>
                      </div>
                    </div>
                    
                    <div className="grid gap-4 md:grid-cols-3">
                      <div>
                        <Label className="text-sm font-medium">Variables</Label>
                        <div className="mt-1 flex flex-wrap gap-1">
                          {prompt.variables.map((variable, index) => (
                            <Badge key={index} variant="outline" className="text-xs">
                              {variable}
                            </Badge>
                          ))}
                        </div>
                      </div>
                      <div>
                        <Label className="text-sm font-medium">Usage Count</Label>
                        <p className="text-lg font-semibold">{prompt.usageCount.toLocaleString()}</p>
                      </div>
                      <div>
                        <Label className="text-sm font-medium">Last Used</Label>
                        <p className="text-sm">{new Date(prompt.lastUsed).toLocaleString()}</p>
                      </div>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="optimization" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Optimization Recommendations</CardTitle>
              <CardDescription>
                AI-driven suggestions for improving performance, reducing costs, and enhancing quality
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="space-y-4">
                {recommendations.map((rec) => (
                  <div key={rec.id} className="p-4 border rounded-lg">
                    <div className="flex items-start justify-between">
                      <div className="flex items-start space-x-3">
                        <div className="p-2 bg-blue-100 rounded-md">
                          {getTypeIcon(rec.type)}
                        </div>
                        <div className="flex-1">
                          <h4 className="font-medium">{rec.title}</h4>
                          <p className="text-sm text-muted-foreground mt-1">{rec.description}</p>
                          <p className="text-sm font-medium mt-2">{rec.recommendation}</p>
                        </div>
                      </div>
                      <div className="flex items-center space-x-2">
                        <Badge className={getImpactColor(rec.impact)}>
                          {rec.impact} impact
                        </Badge>
                        <Badge className={getEffortColor(rec.effort)}>
                          {rec.effort} effort
                        </Badge>
                        <Button variant="outline" size="sm">
                          Apply
                        </Button>
                      </div>
                    </div>
                    <div className="mt-3 text-sm text-green-600 font-medium">
                      Estimated improvement: {rec.estimatedImprovement}
                    </div>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="testing" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Testing Framework</CardTitle>
              <CardDescription>
                Model validation, performance benchmarking, and configuration testing
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="grid gap-4 md:grid-cols-2">
                <Card>
                  <CardHeader>
                    <CardTitle className="text-lg">Model Validation</CardTitle>
                  </CardHeader>
                  <CardContent>
                    <p className="text-sm text-muted-foreground mb-4">
                      Test model configurations with validation datasets
                    </p>
                    <Button className="w-full">
                      <TestTube className="h-4 w-4 mr-2" />
                      Run Validation Tests
                    </Button>
                  </CardContent>
                </Card>
                
                <Card>
                  <CardHeader>
                    <CardTitle className="text-lg">Performance Benchmarking</CardTitle>
                  </CardHeader>
                  <CardContent>
                    <p className="text-sm text-muted-foreground mb-4">
                      Benchmark performance across different configurations
                    </p>
                    <Button className="w-full">
                      <Clock className="h-4 w-4 mr-2" />
                      Start Benchmark
                    </Button>
                  </CardContent>
                </Card>
              </div>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
