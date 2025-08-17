import React, { useState, useEffect, useRef } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Label } from '../ui/label';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '../ui/select';
import { Textarea } from '../ui/textarea';
import { 
  Save, 
  Play, 
  Plus, 
  Trash2,
  GitBranch,
  CheckCircle,
  XCircle,
  Clock,
  User,
  Bot
} from 'lucide-react';
import { workflowApi } from '../../services/api';

interface WorkflowStep {
  id: string;
  name: string;
  type: 'start' | 'task' | 'approval' | 'ai-task' | 'decision' | 'end';
  description: string;
  assignee?: string;
  aiProvider?: string;
  conditions?: string;
  position: { x: number; y: number };
}

interface WorkflowConnection {
  id: string;
  from: string;
  to: string;
  condition?: string;
}

interface WorkflowDesignerProps {
  templateId?: string;
  onSave?: (template: any) => void;
  onCancel?: () => void;
  readonly?: boolean;
}

export const WorkflowDesigner: React.FC<WorkflowDesignerProps> = ({
  templateId,
  onSave,
  onCancel,
  readonly = false
}) => {
  const [workflowName, setWorkflowName] = useState('');
  const [workflowDescription, setWorkflowDescription] = useState('');
  const [steps, setSteps] = useState<WorkflowStep[]>([]);
  const [connections, setConnections] = useState<WorkflowConnection[]>([]);
  const [selectedStep, setSelectedStep] = useState<WorkflowStep | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const canvasRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (templateId) {
      loadWorkflowTemplate(templateId);
    } else {
      setSteps([{
        id: 'start-1',
        name: 'Start',
        type: 'start',
        description: 'Workflow start point',
        position: { x: 100, y: 100 }
      }]);
    }
  }, [templateId]);

  const loadWorkflowTemplate = async (id: string) => {
    try {
      setLoading(true);
      const response = await workflowApi.getWorkflowById(id);
      if (response.success && response.data) {
        const template = response.data;
        setWorkflowName(template.name || '');
        setWorkflowDescription(template.description || '');
        setSteps(template.steps || []);
        setConnections(template.connections || []);
      }
    } catch (error) {
      console.error('Failed to load workflow template:', error);
      setError('Failed to load workflow template');
    } finally {
      setLoading(false);
    }
  };

  const addStep = (type: WorkflowStep['type']) => {
    const newStep: WorkflowStep = {
      id: `${type}-${Date.now()}`,
      name: `New ${type.charAt(0).toUpperCase() + type.slice(1)}`,
      type,
      description: '',
      position: { x: 200 + steps.length * 150, y: 200 }
    };
    setSteps([...steps, newStep]);
    setSelectedStep(newStep);
  };

  const updateStep = (stepId: string, updates: Partial<WorkflowStep>) => {
    setSteps(steps.map(step => 
      step.id === stepId ? { ...step, ...updates } : step
    ));
    if (selectedStep?.id === stepId) {
      setSelectedStep({ ...selectedStep, ...updates });
    }
  };

  const deleteStep = (stepId: string) => {
    setSteps(steps.filter(step => step.id !== stepId));
    setConnections(connections.filter(conn => conn.from !== stepId && conn.to !== stepId));
    if (selectedStep?.id === stepId) {
      setSelectedStep(null);
    }
  };

  // const connectSteps = (fromId: string, toId: string) => {
  //   const newConnection: WorkflowConnection = {
  //     id: `${fromId}-${toId}`,
  //     from: fromId,
  //     to: toId
  //   };
  //   setConnections([...connections, newConnection]);
  // };

  const handleStepDrag = (step: WorkflowStep, event: React.MouseEvent) => {
    if (readonly) return;
    
    const startX = event.clientX - step.position.x;
    const startY = event.clientY - step.position.y;

    const handleMouseMove = (e: MouseEvent) => {
      updateStep(step.id, {
        position: {
          x: e.clientX - startX,
          y: e.clientY - startY
        }
      });
    };

    const handleMouseUp = () => {
      document.removeEventListener('mousemove', handleMouseMove);
      document.removeEventListener('mouseup', handleMouseUp);
    };

    document.addEventListener('mousemove', handleMouseMove);
    document.addEventListener('mouseup', handleMouseUp);
  };

  const saveWorkflow = async () => {
    try {
      setLoading(true);
      setError('');

      const workflowData = {
        name: workflowName,
        description: workflowDescription,
        steps,
        connections,
        bpmnXml: generateBpmnXml()
      };

      let response;
      if (templateId) {
        response = await workflowApi.updateWorkflowTemplate(templateId, workflowData);
      } else {
        response = await workflowApi.createWorkflowTemplate(workflowData);
      }

      if (response.success) {
        if (onSave) {
          onSave(response.data);
        }
      } else {
        setError((response as any).message || 'Failed to save workflow');
      }
    } catch (error) {
      console.error('Failed to save workflow:', error);
      setError('Failed to save workflow. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const generateBpmnXml = (): string => {
    let bpmn = `<?xml version="1.0" encoding="UTF-8"?>
<definitions xmlns="http://www.omg.org/spec/BPMN/20100524/MODEL"
             xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
             xmlns:flowable="http://flowable.org/bpmn"
             targetNamespace="http://barq.com/workflows">
  <process id="${workflowName.replace(/\s+/g, '_').toLowerCase()}" name="${workflowName}" isExecutable="true">
`;

    steps.forEach(step => {
      switch (step.type) {
        case 'start':
          bpmn += `    <startEvent id="${step.id}" name="${step.name}" />\n`;
          break;
        case 'task':
          bpmn += `    <userTask id="${step.id}" name="${step.name}" flowable:assignee="${step.assignee || 'admin'}" />\n`;
          break;
        case 'ai-task':
          bpmn += `    <serviceTask id="${step.id}" name="${step.name}" flowable:class="com.barq.workflow.AITaskDelegate">
      <extensionElements>
        <flowable:field name="aiProvider" stringValue="${step.aiProvider || 'DevinAI'}" />
      </extensionElements>
    </serviceTask>\n`;
          break;
        case 'approval':
          bpmn += `    <userTask id="${step.id}" name="${step.name}" flowable:assignee="${step.assignee || 'admin'}" />\n`;
          break;
        case 'decision':
          bpmn += `    <exclusiveGateway id="${step.id}" name="${step.name}" />\n`;
          break;
        case 'end':
          bpmn += `    <endEvent id="${step.id}" name="${step.name}" />\n`;
          break;
      }
    });

    connections.forEach(conn => {
      bpmn += `    <sequenceFlow id="flow_${conn.id}" sourceRef="${conn.from}" targetRef="${conn.to}"`;
      if (conn.condition) {
        bpmn += ` name="${conn.condition}"`;
      }
      bpmn += ` />\n`;
    });

    bpmn += `  </process>
</definitions>`;

    return bpmn;
  };

  const getStepIcon = (type: WorkflowStep['type']) => {
    switch (type) {
      case 'start': return <Play className="h-4 w-4" />;
      case 'task': return <User className="h-4 w-4" />;
      case 'ai-task': return <Bot className="h-4 w-4" />;
      case 'approval': return <CheckCircle className="h-4 w-4" />;
      case 'decision': return <GitBranch className="h-4 w-4" />;
      case 'end': return <XCircle className="h-4 w-4" />;
      default: return <Clock className="h-4 w-4" />;
    }
  };

  const getStepColor = (type: WorkflowStep['type']) => {
    switch (type) {
      case 'start': return 'bg-green-500';
      case 'task': return 'bg-blue-500';
      case 'ai-task': return 'bg-purple-500';
      case 'approval': return 'bg-orange-500';
      case 'decision': return 'bg-yellow-500';
      case 'end': return 'bg-red-500';
      default: return 'bg-gray-500';
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        <span className="ml-2">Loading workflow designer...</span>
      </div>
    );
  }

  return (
    <div className="h-full flex flex-col">
      {/* Header */}
      <div className="flex-shrink-0 border-b border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-900 p-4">
        <div className="flex items-center justify-between">
          <div className="flex-1 mr-4">
            <Input
              placeholder="Workflow Name"
              value={workflowName}
              onChange={(e) => setWorkflowName(e.target.value)}
              className="text-lg font-semibold mb-2"
              disabled={readonly}
            />
            <Textarea
              placeholder="Workflow Description"
              value={workflowDescription}
              onChange={(e) => setWorkflowDescription(e.target.value)}
              className="text-sm"
              rows={2}
              disabled={readonly}
            />
          </div>
          
          <div className="flex items-center space-x-2">
            {!readonly && (
              <>
                <Button variant="outline" size="sm" onClick={() => addStep('task')}>
                  <Plus className="h-4 w-4 mr-1" />
                  Task
                </Button>
                <Button variant="outline" size="sm" onClick={() => addStep('ai-task')}>
                  <Plus className="h-4 w-4 mr-1" />
                  AI Task
                </Button>
                <Button variant="outline" size="sm" onClick={() => addStep('approval')}>
                  <Plus className="h-4 w-4 mr-1" />
                  Approval
                </Button>
                <Button variant="outline" size="sm" onClick={() => addStep('decision')}>
                  <Plus className="h-4 w-4 mr-1" />
                  Decision
                </Button>
                <Button onClick={saveWorkflow} disabled={loading}>
                  <Save className="h-4 w-4 mr-1" />
                  Save
                </Button>
              </>
            )}
            {onCancel && (
              <Button variant="outline" onClick={onCancel}>
                Cancel
              </Button>
            )}
          </div>
        </div>
        
        {error && (
          <div className="mt-2 text-sm text-red-600 bg-red-50 dark:bg-red-950/20 p-2 rounded">
            {error}
          </div>
        )}
      </div>

      <div className="flex-1 flex">
        {/* Canvas */}
        <div className="flex-1 relative overflow-auto bg-gray-50 dark:bg-gray-900">
          <div
            ref={canvasRef}
            className="relative w-full h-full min-w-[1000px] min-h-[600px]"
            style={{ backgroundImage: 'radial-gradient(circle, #e5e7eb 1px, transparent 1px)', backgroundSize: '20px 20px' }}
          >
            {/* Render connections */}
            <svg className="absolute inset-0 w-full h-full pointer-events-none">
              {connections.map(conn => {
                const fromStep = steps.find(s => s.id === conn.from);
                const toStep = steps.find(s => s.id === conn.to);
                if (!fromStep || !toStep) return null;

                const x1 = fromStep.position.x + 50;
                const y1 = fromStep.position.y + 25;
                const x2 = toStep.position.x + 50;
                const y2 = toStep.position.y + 25;

                return (
                  <line
                    key={conn.id}
                    x1={x1}
                    y1={y1}
                    x2={x2}
                    y2={y2}
                    stroke="#6b7280"
                    strokeWidth="2"
                    markerEnd="url(#arrowhead)"
                  />
                );
              })}
              <defs>
                <marker
                  id="arrowhead"
                  markerWidth="10"
                  markerHeight="7"
                  refX="9"
                  refY="3.5"
                  orient="auto"
                >
                  <polygon
                    points="0 0, 10 3.5, 0 7"
                    fill="#6b7280"
                  />
                </marker>
              </defs>
            </svg>

            {/* Render steps */}
            {steps.map(step => (
              <div
                key={step.id}
                className={`absolute w-24 h-12 rounded-lg border-2 border-white shadow-lg cursor-pointer transition-all hover:shadow-xl ${
                  selectedStep?.id === step.id ? 'ring-2 ring-blue-500' : ''
                } ${getStepColor(step.type)} text-white flex items-center justify-center text-xs font-medium`}
                style={{
                  left: step.position.x,
                  top: step.position.y,
                  transform: selectedStep?.id === step.id ? 'scale(1.05)' : 'scale(1)'
                }}
                onMouseDown={(e) => handleStepDrag(step, e)}
                onClick={() => setSelectedStep(step)}
              >
                <div className="flex flex-col items-center">
                  {getStepIcon(step.type)}
                  <span className="mt-1 truncate w-full text-center">{step.name}</span>
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* Properties Panel */}
        {selectedStep && (
          <div className="w-80 border-l border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-900 p-4">
            <Card>
              <CardHeader>
                <CardTitle className="flex items-center">
                  {getStepIcon(selectedStep.type)}
                  <span className="ml-2">Step Properties</span>
                  {!readonly && (
                    <Button
                      variant="ghost"
                      size="sm"
                      className="ml-auto text-red-600 hover:text-red-700"
                      onClick={() => deleteStep(selectedStep.id)}
                    >
                      <Trash2 className="h-4 w-4" />
                    </Button>
                  )}
                </CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div>
                  <Label htmlFor="step-name">Name</Label>
                  <Input
                    id="step-name"
                    value={selectedStep.name}
                    onChange={(e) => updateStep(selectedStep.id, { name: e.target.value })}
                    disabled={readonly}
                  />
                </div>
                
                <div>
                  <Label htmlFor="step-description">Description</Label>
                  <Textarea
                    id="step-description"
                    value={selectedStep.description}
                    onChange={(e) => updateStep(selectedStep.id, { description: e.target.value })}
                    rows={3}
                    disabled={readonly}
                  />
                </div>

                {(selectedStep.type === 'task' || selectedStep.type === 'approval') && (
                  <div>
                    <Label htmlFor="step-assignee">Assignee</Label>
                    <Input
                      id="step-assignee"
                      value={selectedStep.assignee || ''}
                      onChange={(e) => updateStep(selectedStep.id, { assignee: e.target.value })}
                      placeholder="User ID or role"
                      disabled={readonly}
                    />
                  </div>
                )}

                {selectedStep.type === 'ai-task' && (
                  <div>
                    <Label htmlFor="ai-provider">AI Provider</Label>
                    <Select
                      value={selectedStep.aiProvider || 'DevinAI'}
                      onValueChange={(value) => updateStep(selectedStep.id, { aiProvider: value })}
                      disabled={readonly}
                    >
                      <SelectTrigger>
                        <SelectValue />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="DevinAI">Devin AI</SelectItem>
                        <SelectItem value="ManusAI">Manus AI</SelectItem>
                        <SelectItem value="OpenAI">OpenAI</SelectItem>
                        <SelectItem value="AzureAI">Azure AI</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                )}

                {selectedStep.type === 'decision' && (
                  <div>
                    <Label htmlFor="step-conditions">Conditions</Label>
                    <Textarea
                      id="step-conditions"
                      value={selectedStep.conditions || ''}
                      onChange={(e) => updateStep(selectedStep.id, { conditions: e.target.value })}
                      placeholder="Decision conditions (JSON format)"
                      rows={4}
                      disabled={readonly}
                    />
                  </div>
                )}
              </CardContent>
            </Card>
          </div>
        )}
      </div>
    </div>
  );
};

export default WorkflowDesigner;
