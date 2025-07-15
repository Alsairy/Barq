import { useState, useCallback, useRef } from 'react';
import ReactFlow, {
  Node,
  Edge,
  addEdge,
  Background,
  Controls,
  MiniMap,
  useNodesState,
  useEdgesState,
  Connection,
  ReactFlowProvider,
  Panel,
} from 'reactflow';
import 'reactflow/dist/style.css';
import {
  Save,
  Play,
  Square,
  Download,
  Upload,
  Undo,
  Redo,
  ZoomIn,
  ZoomOut,
  Grid,
  Settings,
  Share,
  FileText,
  GitBranch,
  Clock,
  CheckCircle,
  AlertTriangle,
  Users,
} from 'lucide-react';
import { Button } from '../../../components/ui/button';
import { Badge } from '../../../components/ui/badge';
import { Input } from '../../../components/ui/input';
import { Label } from '../../../components/ui/label';
import { Textarea } from '../../../components/ui/textarea';
import { LoadingSpinner } from '../../../components/ui/loading-spinner';


interface WorkflowValidationError {
  id: string;
  type: 'error' | 'warning';
  message: string;
  nodeId?: string;
  edgeId?: string;
}

const initialNodes: Node[] = [
  {
    id: '1',
    type: 'input',
    position: { x: 250, y: 25 },
    data: { label: 'Start' },
  },
];

const initialEdges: Edge[] = [];

const nodeTypes = {
  start: { label: 'Start', color: 'bg-green-100 border-green-300', icon: Play },
  end: { label: 'End', color: 'bg-red-100 border-red-300', icon: Square },
  task: { label: 'Task', color: 'bg-blue-100 border-blue-300', icon: FileText },
  decision: { label: 'Decision', color: 'bg-yellow-100 border-yellow-300', icon: GitBranch },
  delay: { label: 'Delay', color: 'bg-purple-100 border-purple-300', icon: Clock },
  approval: { label: 'Approval', color: 'bg-orange-100 border-orange-300', icon: CheckCircle },
  notification: { label: 'Notification', color: 'bg-indigo-100 border-indigo-300', icon: AlertTriangle },
  integration: { label: 'Integration', color: 'bg-teal-100 border-teal-300', icon: Settings },
  collaboration: { label: 'Collaboration', color: 'bg-pink-100 border-pink-300', icon: Users },
};

export default function WorkflowDesignerPage() {
  const [nodes, setNodes, onNodesChange] = useNodesState(initialNodes);
  const [edges, setEdges, onEdgesChange] = useEdgesState(initialEdges);
  const [isLoading, setIsLoading] = useState(false);
  const [validationErrors, setValidationErrors] = useState<WorkflowValidationError[]>([]);
  const [workflowName, setWorkflowName] = useState('');
  const [workflowDescription, setWorkflowDescription] = useState('');
  const [isCollaborating, setIsCollaborating] = useState(false);
  const [collaborators] = useState<string[]>([]);
  const reactFlowWrapper = useRef<HTMLDivElement>(null);

  const onConnect = useCallback(
    (params: Connection) => setEdges((eds) => addEdge(params, eds)),
    [setEdges]
  );

  const onDragOver = useCallback((event: React.DragEvent) => {
    event.preventDefault();
    event.dataTransfer.dropEffect = 'move';
  }, []);

  const onDrop = useCallback(
    (event: React.DragEvent) => {
      event.preventDefault();

      const reactFlowBounds = reactFlowWrapper.current?.getBoundingClientRect();
      const type = event.dataTransfer.getData('application/reactflow');

      if (typeof type === 'undefined' || !type || !reactFlowBounds) {
        return;
      }

      const position = {
        x: event.clientX - reactFlowBounds.left,
        y: event.clientY - reactFlowBounds.top,
      };

      const newNode: Node = {
        id: `${type}-${Date.now()}`,
        type,
        position,
        data: { label: `${type} node` },
      };

      setNodes((nds) => nds.concat(newNode));
    },
    [setNodes]
  );

  const onDragStart = (event: React.DragEvent, nodeType: string) => {
    event.dataTransfer.setData('application/reactflow', nodeType);
    event.dataTransfer.effectAllowed = 'move';
  };

  const validateWorkflow = () => {
    const errors: WorkflowValidationError[] = [];
    
    const connectedNodeIds = new Set();
    edges.forEach(edge => {
      connectedNodeIds.add(edge.source);
      connectedNodeIds.add(edge.target);
    });

    nodes.forEach(node => {
      if (!connectedNodeIds.has(node.id) && nodes.length > 1) {
        errors.push({
          id: `disconnected-${node.id}`,
          type: 'warning',
          message: `Node "${node.data.label}" is not connected to the workflow`,
          nodeId: node.id,
        });
      }
    });

    const visited = new Set();
    const recursionStack = new Set();

    const hasCycle = (nodeId: string): boolean => {
      if (recursionStack.has(nodeId)) return true;
      if (visited.has(nodeId)) return false;

      visited.add(nodeId);
      recursionStack.add(nodeId);

      const outgoingEdges = edges.filter(edge => edge.source === nodeId);
      for (const edge of outgoingEdges) {
        if (hasCycle(edge.target)) return true;
      }

      recursionStack.delete(nodeId);
      return false;
    };

    for (const node of nodes) {
      if (hasCycle(node.id)) {
        errors.push({
          id: 'circular-dependency',
          type: 'error',
          message: 'Circular dependency detected in workflow',
        });
        break;
      }
    }

    setValidationErrors(errors);
    return errors.length === 0;
  };

  const saveWorkflow = async () => {
    if (!validateWorkflow()) {
      return;
    }

    setIsLoading(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      const workflowData = {
        name: workflowName,
        description: workflowDescription,
        nodes,
        edges,
        version: '1.0.0',
        createdAt: new Date().toISOString(),
      };

      console.log('Saving workflow:', workflowData);
      
    } catch (error) {
      console.error('Failed to save workflow:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const executeWorkflow = async () => {
    if (!validateWorkflow()) {
      return;
    }

    setIsLoading(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      const executionData = {
        workflowId: 'new-workflow',
        nodes,
        edges,
        startedAt: new Date().toISOString(),
      };

      console.log('Executing workflow:', executionData);
      
    } catch (error) {
      console.error('Failed to execute workflow:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const exportWorkflow = () => {
    const workflowData = {
      name: workflowName,
      description: workflowDescription,
      nodes,
      edges,
      exportedAt: new Date().toISOString(),
    };

    const dataStr = JSON.stringify(workflowData, null, 2);
    const dataUri = 'data:application/json;charset=utf-8,'+ encodeURIComponent(dataStr);
    
    const exportFileDefaultName = `${workflowName || 'workflow'}.json`;
    
    const linkElement = document.createElement('a');
    linkElement.setAttribute('href', dataUri);
    linkElement.setAttribute('download', exportFileDefaultName);
    linkElement.click();
  };

  const importWorkflow = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = (e) => {
      try {
        const workflowData = JSON.parse(e.target?.result as string);
        setWorkflowName(workflowData.name || '');
        setWorkflowDescription(workflowData.description || '');
        setNodes(workflowData.nodes || []);
        setEdges(workflowData.edges || []);
      } catch (error) {
        console.error('Failed to import workflow:', error);
      }
    };
    reader.readAsText(file);
  };

  const startCollaboration = () => {
    setIsCollaborating(true);
    console.log('Starting collaboration mode');
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  return (
    <div className="h-screen flex flex-col">
      <div className="flex items-center justify-between p-4 border-b">
        <div className="flex items-center space-x-4">
          <div>
            <h1 className="text-2xl font-bold">Workflow Designer</h1>
            <p className="text-muted-foreground">
              Create and design workflows with drag-and-drop interface
            </p>
          </div>
          {isCollaborating && (
            <Badge variant="outline" className="bg-green-50">
              <Users className="h-3 w-3 mr-1" />
              Collaborating ({collaborators.length + 1})
            </Badge>
          )}
        </div>
        
        <div className="flex items-center space-x-2">
          <Button variant="outline" size="sm" onClick={() => validateWorkflow()}>
            <CheckCircle className="h-4 w-4 mr-2" />
            Validate
          </Button>
          <Button variant="outline" size="sm" onClick={exportWorkflow}>
            <Download className="h-4 w-4 mr-2" />
            Export
          </Button>
          <label>
            <Button variant="outline" size="sm" asChild>
              <span>
                <Upload className="h-4 w-4 mr-2" />
                Import
              </span>
            </Button>
            <input
              type="file"
              accept=".json"
              onChange={importWorkflow}
              className="hidden"
            />
          </label>
          <Button variant="outline" size="sm" onClick={startCollaboration}>
            <Share className="h-4 w-4 mr-2" />
            Collaborate
          </Button>
          <Button variant="outline" onClick={saveWorkflow}>
            <Save className="h-4 w-4 mr-2" />
            Save
          </Button>
          <Button onClick={executeWorkflow}>
            <Play className="h-4 w-4 mr-2" />
            Execute
          </Button>
        </div>
      </div>

      <div className="flex flex-1">
        {/* Component Palette */}
        <div className="w-64 border-r bg-gray-50 p-4">
          <h3 className="font-medium mb-4">Component Palette</h3>
          <div className="space-y-2">
            {Object.entries(nodeTypes).map(([type, config]) => {
              const IconComponent = config.icon;
              return (
                <div
                  key={type}
                  className={`p-3 rounded-lg border-2 border-dashed cursor-move ${config.color} hover:shadow-md transition-shadow`}
                  draggable
                  onDragStart={(event) => onDragStart(event, type)}
                >
                  <div className="flex items-center space-x-2">
                    <IconComponent className="h-4 w-4" />
                    <span className="text-sm font-medium">{config.label}</span>
                  </div>
                </div>
              );
            })}
          </div>

          {/* Workflow Properties */}
          <div className="mt-6 space-y-4">
            <div>
              <Label htmlFor="workflow-name">Workflow Name</Label>
              <Input
                id="workflow-name"
                value={workflowName}
                onChange={(e) => setWorkflowName(e.target.value)}
                placeholder="Enter workflow name"
              />
            </div>
            <div>
              <Label htmlFor="workflow-description">Description</Label>
              <Textarea
                id="workflow-description"
                value={workflowDescription}
                onChange={(e) => setWorkflowDescription(e.target.value)}
                placeholder="Enter workflow description"
                rows={3}
              />
            </div>
          </div>

          {/* Validation Errors */}
          {validationErrors.length > 0 && (
            <div className="mt-6">
              <h4 className="font-medium text-red-600 mb-2">Validation Issues</h4>
              <div className="space-y-2">
                {validationErrors.map((error) => (
                  <div
                    key={error.id}
                    className={`p-2 rounded text-xs ${
                      error.type === 'error'
                        ? 'bg-red-50 text-red-700 border border-red-200'
                        : 'bg-yellow-50 text-yellow-700 border border-yellow-200'
                    }`}
                  >
                    {error.message}
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>

        {/* Workflow Canvas */}
        <div className="flex-1" ref={reactFlowWrapper}>
          <ReactFlowProvider>
            <ReactFlow
              nodes={nodes}
              edges={edges}
              onNodesChange={onNodesChange}
              onEdgesChange={onEdgesChange}
              onConnect={onConnect}
              onDrop={onDrop}
              onDragOver={onDragOver}
              fitView
            >
              <Controls />
              <MiniMap />
              <Background gap={12} size={1} />
              <Panel position="top-right">
                <div className="flex items-center space-x-2 bg-white p-2 rounded-lg shadow">
                  <Button variant="outline" size="sm">
                    <Undo className="h-4 w-4" />
                  </Button>
                  <Button variant="outline" size="sm">
                    <Redo className="h-4 w-4" />
                  </Button>
                  <Button variant="outline" size="sm">
                    <ZoomIn className="h-4 w-4" />
                  </Button>
                  <Button variant="outline" size="sm">
                    <ZoomOut className="h-4 w-4" />
                  </Button>
                  <Button variant="outline" size="sm">
                    <Grid className="h-4 w-4" />
                  </Button>
                </div>
              </Panel>
            </ReactFlow>
          </ReactFlowProvider>
        </div>
      </div>
    </div>
  );
}
