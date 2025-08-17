import { useState, useEffect } from 'react';
import { Bot, Code, FileText, Zap, Settings, TrendingUp, DollarSign, Clock, CheckCircle } from 'lucide-react';
import { Card, CardContent, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Badge } from '../ui/badge';
import { Progress } from '../ui/progress';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../ui/tabs';
import { Slider } from '../ui/slider';
import { Label } from '../ui/label';
import { Input } from '../ui/input';
import { Textarea } from '../ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '../ui/select';

interface AIAgent {
  id: string;
  name: string;
  description: string;
  capabilities: string[];
  icon: React.ReactNode;
  isActive: boolean;
  performance: {
    accuracy: number;
    speed: number;
    cost: number;
  };
  pricing: {
    model: 'per-request' | 'per-token' | 'subscription';
    rate: number;
    currency: string;
  };
  configuration: {
    temperature: number;
    maxTokens: number;
    topP: number;
    frequencyPenalty: number;
    presencePenalty: number;
  };
  usage: {
    totalRequests: number;
    successRate: number;
    avgResponseTime: number;
  };
  features: string[];
  limitations: string[];
}

interface AgentConfiguration {
  agentId: string;
  temperature: number;
  maxTokens: number;
  topP: number;
  frequencyPenalty: number;
  presencePenalty: number;
  systemPrompt: string;
  customInstructions: string;
  outputFormat: string;
  contextWindow: number;
}

export function AgentSelector() {
  const [agents, setAgents] = useState<AIAgent[]>([]);
  const [selectedAgent, setSelectedAgent] = useState<AIAgent | null>(null);
  const [configuration, setConfiguration] = useState<AgentConfiguration | null>(null);
  const [activeTab, setActiveTab] = useState('agents');
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    loadDevinAgents();
  }, []);

  const loadDevinAgents = () => {
    const devinAgents: AIAgent[] = [
      {
        id: 'devin-code-generation',
        name: 'Devin Code Generation',
        description: 'Advanced AI agent specialized in generating high-quality, production-ready code with comprehensive testing and documentation',
        capabilities: [
          'Full-stack Development',
          'Code Generation',
          'Test Writing',
          'Code Review',
          'Refactoring',
          'Documentation',
          'API Design',
          'Database Schema',
          'Performance Optimization',
          'Security Analysis'
        ],
        icon: <Code className="h-6 w-6" />,
        isActive: true,
        performance: {
          accuracy: 98,
          speed: 92,
          cost: 85
        },
        pricing: {
          model: 'per-request',
          rate: 0.15,
          currency: 'USD'
        },
        configuration: {
          temperature: 0.2,
          maxTokens: 8192,
          topP: 0.9,
          frequencyPenalty: 0.1,
          presencePenalty: 0.1
        },
        usage: {
          totalRequests: 15420,
          successRate: 97.8,
          avgResponseTime: 2.3
        },
        features: [
          'Multi-language support (20+ languages)',
          'Automatic test generation',
          'Code quality analysis',
          'Security vulnerability detection',
          'Performance optimization suggestions',
          'Real-time collaboration',
          'Version control integration',
          'CI/CD pipeline generation'
        ],
        limitations: [
          'Complex architectural decisions may require human oversight',
          'Large codebases (>100k LOC) may need chunking'
        ]
      },
      {
        id: 'devin-brd-generation',
        name: 'Devin BRD Specialist',
        description: 'Expert AI agent for creating comprehensive Business Requirements Documents with stakeholder analysis and technical specifications',
        capabilities: [
          'Requirements Analysis',
          'Stakeholder Mapping',
          'Process Documentation',
          'Technical Specifications',
          'Risk Assessment',
          'Compliance Analysis',
          'User Story Creation',
          'Acceptance Criteria',
          'Workflow Design',
          'Impact Analysis'
        ],
        icon: <FileText className="h-6 w-6" />,
        isActive: true,
        performance: {
          accuracy: 95,
          speed: 88,
          cost: 75
        },
        pricing: {
          model: 'per-request',
          rate: 0.12,
          currency: 'USD'
        },
        configuration: {
          temperature: 0.3,
          maxTokens: 12288,
          topP: 0.85,
          frequencyPenalty: 0.0,
          presencePenalty: 0.0
        },
        usage: {
          totalRequests: 8750,
          successRate: 96.2,
          avgResponseTime: 3.1
        },
        features: [
          'Industry-specific templates',
          'Regulatory compliance checking',
          'Stakeholder impact analysis',
          'Risk matrix generation',
          'Traceability matrix creation',
          'Change impact assessment',
          'Approval workflow integration',
          'Version control and tracking'
        ],
        limitations: [
          'Domain-specific regulations may require expert review',
          'Complex organizational structures need manual validation'
        ]
      },
      {
        id: 'devin-proposal-generation',
        name: 'Devin Proposal Engine',
        description: 'Sophisticated AI agent for creating compelling business proposals, technical proposals, and project documentation',
        capabilities: [
          'Proposal Writing',
          'Technical Documentation',
          'Financial Modeling',
          'Risk Analysis',
          'Competitive Analysis',
          'Executive Summaries',
          'Implementation Planning',
          'Resource Estimation',
          'Timeline Creation',
          'ROI Calculation'
        ],
        icon: <TrendingUp className="h-6 w-6" />,
        isActive: true,
        performance: {
          accuracy: 93,
          speed: 85,
          cost: 80
        },
        pricing: {
          model: 'per-request',
          rate: 0.18,
          currency: 'USD'
        },
        configuration: {
          temperature: 0.4,
          maxTokens: 16384,
          topP: 0.9,
          frequencyPenalty: 0.2,
          presencePenalty: 0.1
        },
        usage: {
          totalRequests: 5230,
          successRate: 94.5,
          avgResponseTime: 4.2
        },
        features: [
          'Industry-specific proposal templates',
          'Financial modeling and projections',
          'Competitive landscape analysis',
          'Risk assessment matrices',
          'Implementation roadmaps',
          'Resource allocation planning',
          'Success metrics definition',
          'Stakeholder communication plans'
        ],
        limitations: [
          'Financial projections require validation by finance experts',
          'Legal terms and conditions need legal review'
        ]
      },
      {
        id: 'devin-presentation-designer',
        name: 'Devin Presentation Designer',
        description: 'Creative AI agent specialized in designing professional presentations with compelling narratives and visual elements',
        capabilities: [
          'Presentation Design',
          'Visual Storytelling',
          'Content Structuring',
          'Slide Layout',
          'Data Visualization',
          'Narrative Flow',
          'Brand Consistency',
          'Interactive Elements',
          'Animation Sequences',
          'Speaker Notes'
        ],
        icon: <Zap className="h-6 w-6" />,
        isActive: true,
        performance: {
          accuracy: 91,
          speed: 90,
          cost: 70
        },
        pricing: {
          model: 'per-request',
          rate: 0.10,
          currency: 'USD'
        },
        configuration: {
          temperature: 0.6,
          maxTokens: 10240,
          topP: 0.95,
          frequencyPenalty: 0.3,
          presencePenalty: 0.2
        },
        usage: {
          totalRequests: 3890,
          successRate: 92.1,
          avgResponseTime: 2.8
        },
        features: [
          'Professional slide templates',
          'Data visualization recommendations',
          'Brand guideline compliance',
          'Interactive presentation elements',
          'Speaker note generation',
          'Audience engagement strategies',
          'Multi-format export options',
          'Collaboration features'
        ],
        limitations: [
          'Complex animations may require design software expertise',
          'Brand-specific elements need manual customization'
        ]
      },
      {
        id: 'devin-testing-security',
        name: 'Devin Testing & Security',
        description: 'Comprehensive AI agent for automated testing, security analysis, and quality assurance across all development phases',
        capabilities: [
          'Test Case Generation',
          'Security Analysis',
          'Vulnerability Assessment',
          'Performance Testing',
          'Code Quality Analysis',
          'Compliance Checking',
          'Penetration Testing',
          'Load Testing',
          'API Testing',
          'UI/UX Testing'
        ],
        icon: <CheckCircle className="h-6 w-6" />,
        isActive: true,
        performance: {
          accuracy: 96,
          speed: 87,
          cost: 90
        },
        pricing: {
          model: 'per-request',
          rate: 0.20,
          currency: 'USD'
        },
        configuration: {
          temperature: 0.1,
          maxTokens: 6144,
          topP: 0.8,
          frequencyPenalty: 0.0,
          presencePenalty: 0.0
        },
        usage: {
          totalRequests: 12100,
          successRate: 98.5,
          avgResponseTime: 1.9
        },
        features: [
          'Automated test suite generation',
          'Security vulnerability scanning',
          'Performance bottleneck identification',
          'Compliance framework validation',
          'Continuous security monitoring',
          'Risk assessment automation',
          'Quality metrics tracking',
          'Integration with CI/CD pipelines'
        ],
        limitations: [
          'Zero-day vulnerabilities require manual research',
          'Complex business logic testing needs domain expertise'
        ]
      }
    ];

    setAgents(devinAgents);
    if (devinAgents.length > 0) {
      setSelectedAgent(devinAgents[0]);
      setConfiguration({
        agentId: devinAgents[0].id,
        ...devinAgents[0].configuration,
        systemPrompt: 'You are a highly skilled AI assistant specialized in software development. Provide accurate, efficient, and well-documented solutions.',
        customInstructions: '',
        outputFormat: 'markdown',
        contextWindow: 32768
      });
    }
  };

  const handleAgentSelect = (agent: AIAgent) => {
    setSelectedAgent(agent);
    setConfiguration({
      agentId: agent.id,
      ...agent.configuration,
      systemPrompt: getDefaultSystemPrompt(agent.id),
      customInstructions: '',
      outputFormat: 'markdown',
      contextWindow: 32768
    });
    setActiveTab('configuration');
  };

  const getDefaultSystemPrompt = (agentId: string): string => {
    const prompts: Record<string, string> = {
      'devin-code-generation': 'You are Devin, an expert software engineer. Generate high-quality, production-ready code with comprehensive testing, documentation, and best practices. Focus on clean architecture, security, and performance.',
      'devin-brd-generation': 'You are a senior business analyst with expertise in requirements gathering and documentation. Create comprehensive, clear, and actionable business requirements documents that align with stakeholder needs and technical constraints.',
      'devin-proposal-generation': 'You are a strategic consultant specializing in business proposals and technical documentation. Create compelling, data-driven proposals that clearly articulate value propositions, implementation strategies, and expected outcomes.',
      'devin-presentation-designer': 'You are a presentation design expert with strong visual communication skills. Create engaging, professional presentations with clear narratives, effective data visualization, and compelling visual elements.',
      'devin-testing-security': 'You are a security and quality assurance expert. Conduct thorough testing, identify vulnerabilities, and ensure compliance with security standards and best practices. Focus on comprehensive coverage and risk mitigation.'
    };
    return prompts[agentId] || 'You are a helpful AI assistant.';
  };

  const handleConfigurationChange = (field: keyof AgentConfiguration, value: any) => {
    if (!configuration) return;
    setConfiguration({
      ...configuration,
      [field]: value
    });
  };

  const handleSaveConfiguration = async () => {
    if (!configuration || !selectedAgent) return;
    
    setIsSaving(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 1500));
      
      setAgents(prev => prev.map(agent => 
        agent.id === selectedAgent.id 
          ? { ...agent, configuration: { ...configuration } }
          : agent
      ));
      
      console.log('Configuration saved:', configuration);
    } catch (error) {
      console.error('Failed to save configuration:', error);
    } finally {
      setIsSaving(false);
    }
  };

  const getPerformanceColor = (score: number): string => {
    if (score >= 90) return 'text-green-600';
    if (score >= 75) return 'text-yellow-600';
    return 'text-red-600';
  };

  const formatCurrency = (amount: number, currency: string): string => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: currency
    }).format(amount);
  };

  return (
    <div className="space-y-6">
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Bot className="h-5 w-5" />
            Devin AI Agent Selection
          </CardTitle>
        </CardHeader>
        <CardContent>
          <p className="text-sm text-gray-600 mb-4">
            Select and configure AI agents powered by Devin's advanced capabilities for your specific tasks.
          </p>
        </CardContent>
      </Card>

      <Tabs value={activeTab} onValueChange={setActiveTab}>
        <TabsList className="grid w-full grid-cols-3">
          <TabsTrigger value="agents">Available Agents</TabsTrigger>
          <TabsTrigger value="configuration">Configuration</TabsTrigger>
          <TabsTrigger value="analytics">Performance Analytics</TabsTrigger>
        </TabsList>

        <TabsContent value="agents" className="space-y-4">
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
            {agents.map((agent) => (
              <Card 
                key={agent.id} 
                className={`cursor-pointer transition-all hover:shadow-md ${
                  selectedAgent?.id === agent.id ? 'ring-2 ring-blue-500' : ''
                }`}
                onClick={() => handleAgentSelect(agent)}
              >
                <CardHeader className="pb-3">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      {agent.icon}
                      <CardTitle className="text-lg">{agent.name}</CardTitle>
                    </div>
                    {agent.isActive && (
                      <Badge variant="secondary" className="text-xs">
                        <CheckCircle className="h-3 w-3 mr-1" />
                        Active
                      </Badge>
                    )}
                  </div>
                </CardHeader>
                <CardContent className="space-y-4">
                  <p className="text-sm text-gray-600">{agent.description}</p>
                  
                  <div className="space-y-2">
                    <div className="flex items-center justify-between text-xs">
                      <span>Accuracy</span>
                      <span className={getPerformanceColor(agent.performance.accuracy)}>
                        {agent.performance.accuracy}%
                      </span>
                    </div>
                    <Progress value={agent.performance.accuracy} className="h-1" />
                  </div>

                  <div className="space-y-2">
                    <div className="flex items-center justify-between text-xs">
                      <span>Speed</span>
                      <span className={getPerformanceColor(agent.performance.speed)}>
                        {agent.performance.speed}%
                      </span>
                    </div>
                    <Progress value={agent.performance.speed} className="h-1" />
                  </div>

                  <div className="space-y-2">
                    <div className="flex items-center justify-between text-xs">
                      <span>Cost Efficiency</span>
                      <span className={getPerformanceColor(agent.performance.cost)}>
                        {agent.performance.cost}%
                      </span>
                    </div>
                    <Progress value={agent.performance.cost} className="h-1" />
                  </div>

                  <div className="flex items-center justify-between text-xs text-gray-500">
                    <div className="flex items-center gap-1">
                      <DollarSign className="h-3 w-3" />
                      {formatCurrency(agent.pricing.rate, agent.pricing.currency)}
                    </div>
                    <div className="flex items-center gap-1">
                      <Clock className="h-3 w-3" />
                      {agent.usage.avgResponseTime}s avg
                    </div>
                  </div>

                  <div className="flex flex-wrap gap-1">
                    {agent.capabilities.slice(0, 3).map((capability) => (
                      <Badge key={capability} variant="outline" className="text-xs">
                        {capability}
                      </Badge>
                    ))}
                    {agent.capabilities.length > 3 && (
                      <Badge variant="outline" className="text-xs">
                        +{agent.capabilities.length - 3} more
                      </Badge>
                    )}
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="configuration" className="space-y-4">
          {selectedAgent && configuration ? (
            <div className="grid gap-6 md:grid-cols-2">
              <Card>
                <CardHeader>
                  <CardTitle className="flex items-center gap-2">
                    <Settings className="h-5 w-5" />
                    Model Parameters
                  </CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="space-y-2">
                    <Label htmlFor="temperature">Temperature: {configuration.temperature}</Label>
                    <Slider
                      id="temperature"
                      min={0}
                      max={1}
                      step={0.1}
                      value={[configuration.temperature]}
                      onValueChange={(value) => handleConfigurationChange('temperature', value[0])}
                    />
                    <p className="text-xs text-gray-500">Controls randomness in responses</p>
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="maxTokens">Max Tokens</Label>
                    <Input
                      id="maxTokens"
                      type="number"
                      value={configuration.maxTokens}
                      onChange={(e) => handleConfigurationChange('maxTokens', parseInt(e.target.value))}
                      min={1}
                      max={32768}
                    />
                    <p className="text-xs text-gray-500">Maximum response length</p>
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="topP">Top P: {configuration.topP}</Label>
                    <Slider
                      id="topP"
                      min={0}
                      max={1}
                      step={0.05}
                      value={[configuration.topP]}
                      onValueChange={(value) => handleConfigurationChange('topP', value[0])}
                    />
                    <p className="text-xs text-gray-500">Controls diversity of responses</p>
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="frequencyPenalty">Frequency Penalty: {configuration.frequencyPenalty}</Label>
                    <Slider
                      id="frequencyPenalty"
                      min={0}
                      max={2}
                      step={0.1}
                      value={[configuration.frequencyPenalty]}
                      onValueChange={(value) => handleConfigurationChange('frequencyPenalty', value[0])}
                    />
                    <p className="text-xs text-gray-500">Reduces repetition</p>
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="presencePenalty">Presence Penalty: {configuration.presencePenalty}</Label>
                    <Slider
                      id="presencePenalty"
                      min={0}
                      max={2}
                      step={0.1}
                      value={[configuration.presencePenalty]}
                      onValueChange={(value) => handleConfigurationChange('presencePenalty', value[0])}
                    />
                    <p className="text-xs text-gray-500">Encourages topic diversity</p>
                  </div>
                </CardContent>
              </Card>

              <Card>
                <CardHeader>
                  <CardTitle>Agent Instructions</CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="space-y-2">
                    <Label htmlFor="systemPrompt">System Prompt</Label>
                    <Textarea
                      id="systemPrompt"
                      value={configuration.systemPrompt}
                      onChange={(e) => handleConfigurationChange('systemPrompt', e.target.value)}
                      rows={4}
                      placeholder="Define the agent's role and behavior..."
                    />
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="customInstructions">Custom Instructions</Label>
                    <Textarea
                      id="customInstructions"
                      value={configuration.customInstructions}
                      onChange={(e) => handleConfigurationChange('customInstructions', e.target.value)}
                      rows={3}
                      placeholder="Additional specific instructions..."
                    />
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="outputFormat">Output Format</Label>
                    <Select
                      value={configuration.outputFormat}
                      onValueChange={(value) => handleConfigurationChange('outputFormat', value)}
                    >
                      <SelectTrigger>
                        <SelectValue />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="markdown">Markdown</SelectItem>
                        <SelectItem value="json">JSON</SelectItem>
                        <SelectItem value="html">HTML</SelectItem>
                        <SelectItem value="plain">Plain Text</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="contextWindow">Context Window</Label>
                    <Input
                      id="contextWindow"
                      type="number"
                      value={configuration.contextWindow}
                      onChange={(e) => handleConfigurationChange('contextWindow', parseInt(e.target.value))}
                      min={1024}
                      max={128000}
                    />
                    <p className="text-xs text-gray-500">Maximum context length</p>
                  </div>

                  <Button 
                    onClick={handleSaveConfiguration}
                    disabled={isSaving}
                    className="w-full"
                  >
                    {isSaving ? 'Saving...' : 'Save Configuration'}
                  </Button>
                </CardContent>
              </Card>
            </div>
          ) : (
            <Card>
              <CardContent className="text-center py-8">
                <Bot className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                <p className="text-gray-600">Select an agent to configure its parameters</p>
              </CardContent>
            </Card>
          )}
        </TabsContent>

        <TabsContent value="analytics" className="space-y-4">
          {selectedAgent ? (
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
              <Card>
                <CardHeader>
                  <CardTitle className="text-lg">Usage Statistics</CardTitle>
                </CardHeader>
                <CardContent className="space-y-3">
                  <div className="flex justify-between">
                    <span className="text-sm text-gray-600">Total Requests</span>
                    <span className="font-semibold">{selectedAgent.usage.totalRequests.toLocaleString()}</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-sm text-gray-600">Success Rate</span>
                    <span className="font-semibold text-green-600">{selectedAgent.usage.successRate}%</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-sm text-gray-600">Avg Response Time</span>
                    <span className="font-semibold">{selectedAgent.usage.avgResponseTime}s</span>
                  </div>
                </CardContent>
              </Card>

              <Card>
                <CardHeader>
                  <CardTitle className="text-lg">Performance Metrics</CardTitle>
                </CardHeader>
                <CardContent className="space-y-3">
                  <div className="space-y-2">
                    <div className="flex justify-between text-sm">
                      <span>Accuracy</span>
                      <span className={getPerformanceColor(selectedAgent.performance.accuracy)}>
                        {selectedAgent.performance.accuracy}%
                      </span>
                    </div>
                    <Progress value={selectedAgent.performance.accuracy} />
                  </div>
                  <div className="space-y-2">
                    <div className="flex justify-between text-sm">
                      <span>Speed</span>
                      <span className={getPerformanceColor(selectedAgent.performance.speed)}>
                        {selectedAgent.performance.speed}%
                      </span>
                    </div>
                    <Progress value={selectedAgent.performance.speed} />
                  </div>
                  <div className="space-y-2">
                    <div className="flex justify-between text-sm">
                      <span>Cost Efficiency</span>
                      <span className={getPerformanceColor(selectedAgent.performance.cost)}>
                        {selectedAgent.performance.cost}%
                      </span>
                    </div>
                    <Progress value={selectedAgent.performance.cost} />
                  </div>
                </CardContent>
              </Card>

              <Card>
                <CardHeader>
                  <CardTitle className="text-lg">Capabilities</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="space-y-2">
                    {selectedAgent.capabilities.map((capability) => (
                      <Badge key={capability} variant="outline" className="mr-1 mb-1">
                        {capability}
                      </Badge>
                    ))}
                  </div>
                </CardContent>
              </Card>

              <Card className="md:col-span-2 lg:col-span-3">
                <CardHeader>
                  <CardTitle className="text-lg">Features & Limitations</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="grid md:grid-cols-2 gap-4">
                    <div>
                      <h4 className="font-medium text-green-700 mb-2">Features</h4>
                      <ul className="space-y-1">
                        {selectedAgent.features.map((feature, index) => (
                          <li key={index} className="text-sm flex items-start gap-2">
                            <CheckCircle className="h-4 w-4 text-green-500 mt-0.5 flex-shrink-0" />
                            {feature}
                          </li>
                        ))}
                      </ul>
                    </div>
                    <div>
                      <h4 className="font-medium text-orange-700 mb-2">Limitations</h4>
                      <ul className="space-y-1">
                        {selectedAgent.limitations.map((limitation, index) => (
                          <li key={index} className="text-sm flex items-start gap-2">
                            <div className="w-4 h-4 border border-orange-400 rounded-full mt-0.5 flex-shrink-0 flex items-center justify-center">
                              <div className="w-1 h-1 bg-orange-400 rounded-full"></div>
                            </div>
                            {limitation}
                          </li>
                        ))}
                      </ul>
                    </div>
                  </div>
                </CardContent>
              </Card>
            </div>
          ) : (
            <Card>
              <CardContent className="text-center py-8">
                <TrendingUp className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                <p className="text-gray-600">Select an agent to view performance analytics</p>
              </CardContent>
            </Card>
          )}
        </TabsContent>
      </Tabs>
    </div>
  );
}
