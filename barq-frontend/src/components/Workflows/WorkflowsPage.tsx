import { useState, useEffect } from 'react'
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
  XCircle,
  Plus,
  Settings,
  BarChart3,
  AlertTriangle
} from 'lucide-react'
import { WorkflowInstance, WorkflowStatus } from '@/types/api'
import { workflowApi } from '@/services/api'
import { signalRService } from '@/services/signalRService'

export default function WorkflowsPage() {
  const [workflows, setWorkflows] = useState<WorkflowInstance[]>([])
  const [workflowTemplates, setWorkflowTemplates] = useState<Record<string, { name: string; steps: number }>>({})
  const [searchTerm, setSearchTerm] = useState('')
  const [statusFilter, setStatusFilter] = useState<string>('all')
  const [templateFilter, setTemplateFilter] = useState<string>('all')
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string>('')
  // const [selectedWorkflow, setSelectedWorkflow] = useState<WorkflowInstance | null>(null)
  // const [showWorkflowDetails, setShowWorkflowDetails] = useState(false)

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true)
        setError('')
        
        const [instancesResponse, templatesResponse] = await Promise.all([
          workflowApi.getWorkflowInstances(),
          workflowApi.getWorkflowTemplates()
        ])

        if (instancesResponse.success && instancesResponse.data) {
          setWorkflows(instancesResponse.data)
        }

        if (templatesResponse.success && templatesResponse.data) {
          const templateMap = templatesResponse.data.reduce((acc: Record<string, { name: string; steps: number }>, template: any) => {
            acc[template.id] = {
              name: template.name || 'Unknown Template',
              steps: template.stepCount || 1
            }
            return acc
          }, {})
          setWorkflowTemplates(templateMap)
        }
      } catch (error) {
        console.error('Failed to fetch workflow data:', error)
        setError('Failed to load workflow data. Please try again.')
        setWorkflows([])
        setWorkflowTemplates({})
      } finally {
        setLoading(false)
      }
    }

    fetchData()

    const setupSignalR = async () => {
      try {
        await signalRService.connect()
        
        console.log('SignalR connected for workflows page')
      } catch (error) {
        console.error('Failed to setup SignalR connection:', error)
      }
    }

    setupSignalR()

    return () => {
      signalRService.disconnect()
    }
  }, [])

  const filteredWorkflows = workflows.filter(workflow => {
    const templateName = workflowTemplates[workflow.templateId as keyof typeof workflowTemplates]?.name || 'Unknown'
    const matchesSearch = workflow.id.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         templateName.toLowerCase().includes(searchTerm.toLowerCase())
    const matchesStatus = statusFilter === 'all' || workflow.status.toString() === statusFilter
    const matchesTemplate = templateFilter === 'all' || workflow.templateId === templateFilter
    
    return matchesSearch && matchesStatus && matchesTemplate
  })

  const getStatusBadge = (status: WorkflowStatus) => {
    const statusConfig: Record<WorkflowStatus, { variant: 'secondary' | 'default' | 'outline' | 'destructive'; label: string; icon: any }> = {
      [WorkflowStatus.NotStarted]: { variant: 'secondary' as const, label: 'Not Started', icon: Clock },
      [WorkflowStatus.Running]: { variant: 'default' as const, label: 'Running', icon: Play },
      [WorkflowStatus.Suspended]: { variant: 'outline' as const, label: 'Suspended', icon: Pause },
      [WorkflowStatus.Completed]: { variant: 'default' as const, label: 'Completed', icon: CheckCircle },
      [WorkflowStatus.Failed]: { variant: 'destructive' as const, label: 'Failed', icon: XCircle },
      [WorkflowStatus.Cancelled]: { variant: 'secondary' as const, label: 'Cancelled', icon: Square },
      [WorkflowStatus.WaitingForApproval]: { variant: 'outline' as const, label: 'Waiting for Approval', icon: Clock }
    }
    
    const config = statusConfig[status] || statusConfig[WorkflowStatus.NotStarted]
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

  const handleWorkflowAction = async (workflowId: string, action: 'start' | 'pause' | 'resume' | 'cancel') => {
    try {
      let response
      switch (action) {
        case 'start':
          response = await workflowApi.startWorkflow(workflowId, {})
          break
        case 'pause':
          response = await workflowApi.pauseWorkflow(workflowId)
          break
        case 'resume':
          response = await workflowApi.resumeWorkflow(workflowId)
          break
        case 'cancel':
          response = await workflowApi.cancelWorkflow(workflowId, 'Cancelled by user')
          break
        default:
          return
      }

      if (response.success) {
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
      } else {
        setError(`Failed to ${action} workflow: ${(response as any).message || 'Unknown error'}`)
      }
    } catch (error) {
      console.error(`Failed to ${action} workflow:`, error)
      setError(`Failed to ${action} workflow. Please try again.`)
    }
  }

  const handleViewWorkflow = async (workflowId: string) => {
    try {
      const workflow = workflows.find(w => w.id === workflowId)
      if (workflow) {
        console.log('View workflow:', workflow)
      }
    } catch (error) {
      console.error('Failed to view workflow:', error)
      setError('Failed to load workflow details.')
    }
  }

  const handleApproveWorkflow = async (workflowId: string, comments?: string) => {
    try {
      const response = await workflowApi.approveWorkflow({
        workflowId,
        approverId: 'current-user-id', // This should come from auth context
        comments: comments || ''
      })

      if (response.success) {
        const updatedResponse = await workflowApi.getWorkflowInstances()
        if (updatedResponse.success && updatedResponse.data) {
          setWorkflows(updatedResponse.data)
        }
      } else {
        setError(`Failed to approve workflow: ${(response as any).message || 'Unknown error'}`)
      }
    } catch (error) {
      console.error('Failed to approve workflow:', error)
      setError('Failed to approve workflow. Please try again.')
    }
  }

  const handleRejectWorkflow = async (workflowId: string, reason: string) => {
    try {
      const response = await workflowApi.rejectWorkflow({
        workflowId,
        reviewerId: 'current-user-id', // This should come from auth context
        reason
      })

      if (response.success) {
        const updatedResponse = await workflowApi.getWorkflowInstances()
        if (updatedResponse.success && updatedResponse.data) {
          setWorkflows(updatedResponse.data)
        }
      } else {
        setError(`Failed to reject workflow: ${(response as any).message || 'Unknown error'}`)
      }
    } catch (error) {
      console.error('Failed to reject workflow:', error)
      setError('Failed to reject workflow. Please try again.')
    }
  }

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        <span className="ml-2">Loading workflows...</span>
      </div>
    )
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Workflows</h1>
          <p className="mt-2 text-gray-600 dark:text-gray-400">
            Monitor and manage workflow instances and their execution with Flowable BPM
          </p>
        </div>
        
        <div className="flex items-center space-x-2">
          <Button variant="outline" size="sm">
            <BarChart3 className="h-4 w-4 mr-2" />
            Analytics
          </Button>
          <Button variant="outline" size="sm">
            <Settings className="h-4 w-4 mr-2" />
            Templates
          </Button>
          <Button size="sm">
            <Plus className="h-4 w-4 mr-2" />
            New Workflow
          </Button>
        </div>
      </div>

      {error && (
        <div className="bg-red-50 dark:bg-red-950/20 border border-red-200 dark:border-red-800 rounded-lg p-4">
          <div className="flex items-center">
            <AlertTriangle className="h-5 w-5 text-red-600 mr-2" />
            <span className="text-red-800 dark:text-red-200">{error}</span>
          </div>
        </div>
      )}

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
                const duration = workflow.completedAt && workflow.startedAt
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
                        <Button 
                          variant="ghost" 
                          size="sm"
                          onClick={() => handleViewWorkflow(workflow.id)}
                          title="View Details"
                        >
                          <Eye className="h-4 w-4" />
                        </Button>
                        {workflow.status === WorkflowStatus.NotStarted && (
                          <Button 
                            variant="ghost" 
                            size="sm"
                            onClick={() => handleWorkflowAction(workflow.id, 'start')}
                            title="Start Workflow"
                          >
                            <Play className="h-4 w-4" />
                          </Button>
                        )}
                        {workflow.status === WorkflowStatus.Running && (
                          <Button 
                            variant="ghost" 
                            size="sm"
                            onClick={() => handleWorkflowAction(workflow.id, 'pause')}
                            title="Pause Workflow"
                          >
                            <Pause className="h-4 w-4" />
                          </Button>
                        )}
                        {workflow.status === WorkflowStatus.Suspended && (
                          <Button 
                            variant="ghost" 
                            size="sm"
                            onClick={() => handleWorkflowAction(workflow.id, 'resume')}
                            title="Resume Workflow"
                          >
                            <Play className="h-4 w-4" />
                          </Button>
                        )}
                        {(workflow.status === WorkflowStatus.Running || workflow.status === WorkflowStatus.Suspended) && (
                          <Button 
                            variant="ghost" 
                            size="sm"
                            onClick={() => handleWorkflowAction(workflow.id, 'cancel')}
                            title="Cancel Workflow"
                          >
                            <Square className="h-4 w-4" />
                          </Button>
                        )}
                        {workflow.status === WorkflowStatus.WaitingForApproval && (
                          <>
                            <Button 
                              variant="ghost" 
                              size="sm"
                              onClick={() => handleApproveWorkflow(workflow.id)}
                              title="Approve"
                              className="text-green-600 hover:text-green-700"
                            >
                              <CheckCircle className="h-4 w-4" />
                            </Button>
                            <Button 
                              variant="ghost" 
                              size="sm"
                              onClick={() => handleRejectWorkflow(workflow.id, 'Rejected by user')}
                              title="Reject"
                              className="text-red-600 hover:text-red-700"
                            >
                              <XCircle className="h-4 w-4" />
                            </Button>
                          </>
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
