import { useState } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { Progress } from '@/components/ui/progress'
import { 
  Search, 
  Play, 
  Pause, 
  Square, 
  Eye, 
  Clock, 
  CheckCircle, 
  XCircle
} from 'lucide-react'
import { WorkflowInstance, WorkflowStatus } from '@/types/api'

const mockWorkflows: WorkflowInstance[] = [
  {
    id: 'wf-001',
    templateId: 'template-1',
    status: WorkflowStatus.Running,
    currentStepId: 'step-2',
    initiatorId: 'user1',
    workflowData: '{"requestId": "1", "approvalLevel": 2}',
    startedAt: '2024-08-01T10:00:00Z',
    createdAt: '2024-08-01T10:00:00Z',
    updatedAt: '2024-08-02T14:30:00Z'
  },
  {
    id: 'wf-002',
    templateId: 'template-2',
    status: WorkflowStatus.Completed,
    initiatorId: 'user3',
    workflowData: '{"requestId": "2", "analysisType": "sales"}',
    startedAt: '2024-08-01T15:00:00Z',
    completedAt: '2024-08-02T16:00:00Z',
    createdAt: '2024-08-01T15:00:00Z',
    updatedAt: '2024-08-02T16:00:00Z'
  },
  {
    id: 'wf-003',
    templateId: 'template-3',
    status: WorkflowStatus.Suspended,
    currentStepId: 'step-1',
    initiatorId: 'user4',
    workflowData: '{"requestId": "3", "reviewType": "security"}',
    startedAt: '2024-08-02T09:00:00Z',
    createdAt: '2024-08-02T09:00:00Z',
    updatedAt: '2024-08-02T09:00:00Z'
  },
  {
    id: 'wf-004',
    templateId: 'template-1',
    status: WorkflowStatus.Failed,
    currentStepId: 'step-3',
    initiatorId: 'user2',
    workflowData: '{"requestId": "4", "errorCode": "TIMEOUT"}',
    startedAt: '2024-08-01T08:00:00Z',
    createdAt: '2024-08-01T08:00:00Z',
    updatedAt: '2024-08-01T12:00:00Z'
  }
]

const workflowTemplates = {
  'template-1': { name: 'AI Request Approval', steps: 4 },
  'template-2': { name: 'Data Analysis Pipeline', steps: 3 },
  'template-3': { name: 'Security Review Process', steps: 5 }
}

export default function WorkflowsPage() {
  const [workflows, setWorkflows] = useState<WorkflowInstance[]>(mockWorkflows)
  const [searchTerm, setSearchTerm] = useState('')
  const [statusFilter, setStatusFilter] = useState<string>('all')
  const [templateFilter, setTemplateFilter] = useState<string>('all')

  const filteredWorkflows = workflows.filter(workflow => {
    const templateName = workflowTemplates[workflow.templateId as keyof typeof workflowTemplates]?.name || 'Unknown'
    const matchesSearch = workflow.id.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         templateName.toLowerCase().includes(searchTerm.toLowerCase())
    const matchesStatus = statusFilter === 'all' || workflow.status.toString() === statusFilter
    const matchesTemplate = templateFilter === 'all' || workflow.templateId === templateFilter
    
    return matchesSearch && matchesStatus && matchesTemplate
  })

  const getStatusBadge = (status: WorkflowStatus) => {
    const statusConfig = {
      [WorkflowStatus.NotStarted]: { variant: 'secondary' as const, label: 'Not Started', icon: Clock },
      [WorkflowStatus.Running]: { variant: 'default' as const, label: 'Running', icon: Play },
      [WorkflowStatus.Suspended]: { variant: 'outline' as const, label: 'Suspended', icon: Pause },
      [WorkflowStatus.Completed]: { variant: 'default' as const, label: 'Completed', icon: CheckCircle },
      [WorkflowStatus.Failed]: { variant: 'destructive' as const, label: 'Failed', icon: XCircle },
      [WorkflowStatus.Cancelled]: { variant: 'secondary' as const, label: 'Cancelled', icon: Square }
    }
    
    const config = statusConfig[status]
    const Icon = config.icon
    return (
      <Badge variant={config.variant} className="flex items-center gap-1">
        <Icon className="h-3 w-3" />
        {config.label}
      </Badge>
    )
  }

  const getProgress = (workflow: WorkflowInstance) => {
    const template = workflowTemplates[workflow.templateId as keyof typeof workflowTemplates]
    if (!template) return 0
    
    if (workflow.status === WorkflowStatus.Completed) return 100
    if (workflow.status === WorkflowStatus.Failed || workflow.status === WorkflowStatus.Cancelled) return 0
    if (workflow.status === WorkflowStatus.NotStarted) return 0
    
    const currentStep = workflow.currentStepId ? parseInt(workflow.currentStepId.split('-')[1]) : 1
    return Math.round((currentStep / template.steps) * 100)
  }

  const handleWorkflowAction = (workflowId: string, action: 'start' | 'pause' | 'resume' | 'cancel') => {
    setWorkflows(workflows.map(workflow => {
      if (workflow.id === workflowId) {
        switch (action) {
          case 'start':
            return { ...workflow, status: WorkflowStatus.Running, startedAt: new Date().toISOString() }
          case 'pause':
            return { ...workflow, status: WorkflowStatus.Suspended }
          case 'resume':
            return { ...workflow, status: WorkflowStatus.Running }
          case 'cancel':
            return { ...workflow, status: WorkflowStatus.Cancelled }
          default:
            return workflow
        }
      }
      return workflow
    }))
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Workflows</h1>
          <p className="mt-2 text-gray-600 dark:text-gray-400">
            Monitor and manage workflow instances and their execution
          </p>
        </div>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Active Workflows</CardTitle>
            <Play className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              {workflows.filter(w => w.status === WorkflowStatus.Running).length}
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Completed</CardTitle>
            <CheckCircle className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              {workflows.filter(w => w.status === WorkflowStatus.Completed).length}
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Failed</CardTitle>
            <XCircle className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              {workflows.filter(w => w.status === WorkflowStatus.Failed).length}
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Suspended</CardTitle>
            <Pause className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              {workflows.filter(w => w.status === WorkflowStatus.Suspended).length}
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Filters */}
      <Card>
        <CardHeader>
          <CardTitle>Filters</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="flex gap-4">
            <div className="flex-1">
              <Label htmlFor="search">Search</Label>
              <div className="relative">
                <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
                <Input
                  id="search"
                  placeholder="Search workflows..."
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="pl-8"
                />
              </div>
            </div>
            <div>
              <Label htmlFor="status-filter">Status</Label>
              <Select value={statusFilter} onValueChange={setStatusFilter}>
                <SelectTrigger className="w-[180px]">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">All Statuses</SelectItem>
                  <SelectItem value="0">Not Started</SelectItem>
                  <SelectItem value="1">Running</SelectItem>
                  <SelectItem value="2">Suspended</SelectItem>
                  <SelectItem value="3">Completed</SelectItem>
                  <SelectItem value="4">Failed</SelectItem>
                  <SelectItem value="5">Cancelled</SelectItem>
                </SelectContent>
              </Select>
            </div>
            <div>
              <Label htmlFor="template-filter">Template</Label>
              <Select value={templateFilter} onValueChange={setTemplateFilter}>
                <SelectTrigger className="w-[200px]">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">All Templates</SelectItem>
                  <SelectItem value="template-1">AI Request Approval</SelectItem>
                  <SelectItem value="template-2">Data Analysis Pipeline</SelectItem>
                  <SelectItem value="template-3">Security Review Process</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Workflows Table */}
      <Card>
        <CardHeader>
          <CardTitle>Workflow Instances ({filteredWorkflows.length})</CardTitle>
          <CardDescription>
            Monitor workflow execution and manage instance lifecycle
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Workflow ID</TableHead>
                <TableHead>Template</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Progress</TableHead>
                <TableHead>Started</TableHead>
                <TableHead>Duration</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filteredWorkflows.map((workflow) => {
                const template = workflowTemplates[workflow.templateId as keyof typeof workflowTemplates]
                const progress = getProgress(workflow)
                const duration = workflow.completedAt 
                  ? Math.round((new Date(workflow.completedAt).getTime() - new Date(workflow.startedAt).getTime()) / (1000 * 60))
                  : workflow.startedAt 
                    ? Math.round((new Date().getTime() - new Date(workflow.startedAt).getTime()) / (1000 * 60))
                    : 0

                return (
                  <TableRow key={workflow.id}>
                    <TableCell>
                      <div className="font-mono text-sm">{workflow.id}</div>
                    </TableCell>
                    <TableCell>
                      <div>
                        <div className="font-medium">{template?.name || 'Unknown'}</div>
                        <div className="text-sm text-muted-foreground">
                          {workflow.currentStepId || 'Not started'}
                        </div>
                      </div>
                    </TableCell>
                    <TableCell>{getStatusBadge(workflow.status)}</TableCell>
                    <TableCell>
                      <div className="space-y-1">
                        <Progress value={progress} className="w-[100px]" />
                        <div className="text-xs text-muted-foreground">{progress}%</div>
                      </div>
                    </TableCell>
                    <TableCell>
                      {workflow.startedAt ? new Date(workflow.startedAt).toLocaleString() : '-'}
                    </TableCell>
                    <TableCell>
                      {duration > 0 ? `${duration}m` : '-'}
                    </TableCell>
                    <TableCell>
                      <div className="flex gap-2">
                        <Button variant="ghost" size="sm">
                          <Eye className="h-4 w-4" />
                        </Button>
                        {workflow.status === WorkflowStatus.NotStarted && (
                          <Button 
                            variant="ghost" 
                            size="sm"
                            onClick={() => handleWorkflowAction(workflow.id, 'start')}
                          >
                            <Play className="h-4 w-4" />
                          </Button>
                        )}
                        {workflow.status === WorkflowStatus.Running && (
                          <Button 
                            variant="ghost" 
                            size="sm"
                            onClick={() => handleWorkflowAction(workflow.id, 'pause')}
                          >
                            <Pause className="h-4 w-4" />
                          </Button>
                        )}
                        {workflow.status === WorkflowStatus.Suspended && (
                          <Button 
                            variant="ghost" 
                            size="sm"
                            onClick={() => handleWorkflowAction(workflow.id, 'resume')}
                          >
                            <Play className="h-4 w-4" />
                          </Button>
                        )}
                        {(workflow.status === WorkflowStatus.Running || workflow.status === WorkflowStatus.Suspended) && (
                          <Button 
                            variant="ghost" 
                            size="sm"
                            onClick={() => handleWorkflowAction(workflow.id, 'cancel')}
                          >
                            <Square className="h-4 w-4" />
                          </Button>
                        )}
                      </div>
                    </TableCell>
                  </TableRow>
                )
              })}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  )
}
