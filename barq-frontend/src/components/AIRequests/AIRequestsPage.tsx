import { useState } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { 
  Plus, 
  Search, 
  Eye, 
  Edit, 
  Trash2
} from 'lucide-react'
import { AIRequest, AIRequestType, AIRequestPriority, AIRequestStatus, CreateAIRequestRequest } from '@/types/api'

const mockAIRequests: AIRequest[] = [
  {
    id: '1',
    title: 'Content Generation for Marketing Campaign',
    description: 'Generate marketing content for Q4 product launch',
    requestType: AIRequestType.TextGeneration,
    priority: AIRequestPriority.High,
    status: AIRequestStatus.Approved,
    requestData: '{"campaign": "Q4 Launch", "target_audience": "Enterprise"}',
    requesterId: 'user1',
    assignedToId: 'user2',
    dueDate: '2024-08-15T00:00:00Z',
    createdAt: '2024-08-01T10:00:00Z',
    updatedAt: '2024-08-02T14:30:00Z',
    workflowInstanceId: 'wf-001'
  },
  {
    id: '2',
    title: 'Data Analysis Report Generation',
    description: 'Analyze sales data and generate insights report',
    requestType: AIRequestType.DataAnalysis,
    priority: AIRequestPriority.Normal,
    status: AIRequestStatus.InProgress,
    requestData: '{"data_source": "sales_db", "period": "Q3_2024"}',
    requesterId: 'user3',
    dueDate: '2024-08-20T00:00:00Z',
    createdAt: '2024-08-01T15:00:00Z',
    updatedAt: '2024-08-02T16:00:00Z',
    workflowInstanceId: 'wf-002'
  },
  {
    id: '3',
    title: 'Code Review Assistant',
    description: 'AI-powered code review for security vulnerabilities',
    requestType: AIRequestType.CodeGeneration,
    priority: AIRequestPriority.Critical,
    status: AIRequestStatus.UnderReview,
    requestData: '{"repository": "main-app", "branch": "feature/security-update"}',
    requesterId: 'user4',
    dueDate: '2024-08-10T00:00:00Z',
    createdAt: '2024-08-02T09:00:00Z',
    updatedAt: '2024-08-02T09:00:00Z',
    workflowInstanceId: 'wf-003'
  }
]

export default function AIRequestsPage() {
  const [requests, setRequests] = useState<AIRequest[]>(mockAIRequests)
  const [searchTerm, setSearchTerm] = useState('')
  const [statusFilter, setStatusFilter] = useState<string>('all')
  const [priorityFilter, setPriorityFilter] = useState<string>('all')
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false)
  const [newRequest, setNewRequest] = useState<CreateAIRequestRequest>({
    title: '',
    description: '',
    requestType: AIRequestType.TextGeneration,
    priority: AIRequestPriority.Normal,
    requestData: ''
  })

  const filteredRequests = requests.filter(request => {
    const matchesSearch = request.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         request.description.toLowerCase().includes(searchTerm.toLowerCase())
    const matchesStatus = statusFilter === 'all' || request.status.toString() === statusFilter
    const matchesPriority = priorityFilter === 'all' || request.priority.toString() === priorityFilter
    
    return matchesSearch && matchesStatus && matchesPriority
  })

  const getStatusBadge = (status: AIRequestStatus) => {
    const statusConfig = {
      [AIRequestStatus.Draft]: { variant: 'secondary' as const, label: 'Draft' },
      [AIRequestStatus.Submitted]: { variant: 'outline' as const, label: 'Submitted' },
      [AIRequestStatus.UnderReview]: { variant: 'default' as const, label: 'Under Review' },
      [AIRequestStatus.Approved]: { variant: 'default' as const, label: 'Approved' },
      [AIRequestStatus.Rejected]: { variant: 'destructive' as const, label: 'Rejected' },
      [AIRequestStatus.InProgress]: { variant: 'default' as const, label: 'In Progress' },
      [AIRequestStatus.Completed]: { variant: 'default' as const, label: 'Completed' },
      [AIRequestStatus.Cancelled]: { variant: 'secondary' as const, label: 'Cancelled' },
      [AIRequestStatus.QualityReview]: { variant: 'outline' as const, label: 'Quality Review' }
    }
    
    const config = statusConfig[status]
    return <Badge variant={config.variant}>{config.label}</Badge>
  }

  const getPriorityBadge = (priority: AIRequestPriority) => {
    const priorityConfig = {
      [AIRequestPriority.Low]: { variant: 'outline' as const, label: 'Low' },
      [AIRequestPriority.Normal]: { variant: 'secondary' as const, label: 'Normal' },
      [AIRequestPriority.High]: { variant: 'default' as const, label: 'High' },
      [AIRequestPriority.Critical]: { variant: 'destructive' as const, label: 'Critical' }
    }
    
    const config = priorityConfig[priority]
    return <Badge variant={config.variant}>{config.label}</Badge>
  }

  const handleCreateRequest = () => {
    const request: AIRequest = {
      id: Date.now().toString(),
      ...newRequest,
      status: AIRequestStatus.Draft,
      requesterId: 'current-user',
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString()
    }
    
    setRequests([...requests, request])
    setIsCreateDialogOpen(false)
    setNewRequest({
      title: '',
      description: '',
      requestType: AIRequestType.TextGeneration,
      priority: AIRequestPriority.Normal,
      requestData: ''
    })
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white">AI Requests</h1>
          <p className="mt-2 text-gray-600 dark:text-gray-400">
            Manage and track AI processing requests
          </p>
        </div>
        
        <Dialog open={isCreateDialogOpen} onOpenChange={setIsCreateDialogOpen}>
          <DialogTrigger asChild>
            <Button>
              <Plus className="h-4 w-4 mr-2" />
              New Request
            </Button>
          </DialogTrigger>
          <DialogContent className="sm:max-w-[600px]">
            <DialogHeader>
              <DialogTitle>Create New AI Request</DialogTitle>
              <DialogDescription>
                Submit a new AI processing request for review and approval.
              </DialogDescription>
            </DialogHeader>
            <div className="grid gap-4 py-4">
              <div className="grid gap-2">
                <Label htmlFor="title">Title</Label>
                <Input
                  id="title"
                  value={newRequest.title}
                  onChange={(e) => setNewRequest({...newRequest, title: e.target.value})}
                  placeholder="Enter request title"
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="description">Description</Label>
                <Textarea
                  id="description"
                  value={newRequest.description}
                  onChange={(e) => setNewRequest({...newRequest, description: e.target.value})}
                  placeholder="Describe your AI request"
                  rows={3}
                />
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div className="grid gap-2">
                  <Label htmlFor="type">Request Type</Label>
                  <Select value={newRequest.requestType.toString()} onValueChange={(value) => setNewRequest({...newRequest, requestType: parseInt(value) as AIRequestType})}>
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="0">Text Generation</SelectItem>
                      <SelectItem value="1">Image Generation</SelectItem>
                      <SelectItem value="2">Data Analysis</SelectItem>
                      <SelectItem value="3">Code Generation</SelectItem>
                      <SelectItem value="4">Translation</SelectItem>
                      <SelectItem value="5">Summarization</SelectItem>
                      <SelectItem value="6">Classification</SelectItem>
                      <SelectItem value="7">Other</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
                <div className="grid gap-2">
                  <Label htmlFor="priority">Priority</Label>
                  <Select value={newRequest.priority.toString()} onValueChange={(value) => setNewRequest({...newRequest, priority: parseInt(value) as AIRequestPriority})}>
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="0">Low</SelectItem>
                      <SelectItem value="1">Normal</SelectItem>
                      <SelectItem value="2">High</SelectItem>
                      <SelectItem value="3">Critical</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>
              <div className="grid gap-2">
                <Label htmlFor="requestData">Request Data (JSON)</Label>
                <Textarea
                  id="requestData"
                  value={newRequest.requestData}
                  onChange={(e) => setNewRequest({...newRequest, requestData: e.target.value})}
                  placeholder='{"key": "value"}'
                  rows={3}
                />
              </div>
            </div>
            <div className="flex justify-end gap-2">
              <Button variant="outline" onClick={() => setIsCreateDialogOpen(false)}>
                Cancel
              </Button>
              <Button onClick={handleCreateRequest}>
                Create Request
              </Button>
            </div>
          </DialogContent>
        </Dialog>
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
                  placeholder="Search requests..."
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
                  <SelectItem value="0">Draft</SelectItem>
                  <SelectItem value="1">Submitted</SelectItem>
                  <SelectItem value="2">Under Review</SelectItem>
                  <SelectItem value="3">Approved</SelectItem>
                  <SelectItem value="4">Rejected</SelectItem>
                  <SelectItem value="5">In Progress</SelectItem>
                  <SelectItem value="6">Completed</SelectItem>
                  <SelectItem value="7">Cancelled</SelectItem>
                  <SelectItem value="8">Quality Review</SelectItem>
                </SelectContent>
              </Select>
            </div>
            <div>
              <Label htmlFor="priority-filter">Priority</Label>
              <Select value={priorityFilter} onValueChange={setPriorityFilter}>
                <SelectTrigger className="w-[180px]">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">All Priorities</SelectItem>
                  <SelectItem value="0">Low</SelectItem>
                  <SelectItem value="1">Normal</SelectItem>
                  <SelectItem value="2">High</SelectItem>
                  <SelectItem value="3">Critical</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Requests Table */}
      <Card>
        <CardHeader>
          <CardTitle>AI Requests ({filteredRequests.length})</CardTitle>
          <CardDescription>
            Manage your AI processing requests and track their progress
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Title</TableHead>
                <TableHead>Type</TableHead>
                <TableHead>Priority</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Due Date</TableHead>
                <TableHead>Created</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filteredRequests.map((request) => (
                <TableRow key={request.id}>
                  <TableCell>
                    <div>
                      <div className="font-medium">{request.title}</div>
                      <div className="text-sm text-muted-foreground truncate max-w-[200px]">
                        {request.description}
                      </div>
                    </div>
                  </TableCell>
                  <TableCell>
                    <Badge variant="outline">
                      {AIRequestType[request.requestType]}
                    </Badge>
                  </TableCell>
                  <TableCell>{getPriorityBadge(request.priority)}</TableCell>
                  <TableCell>{getStatusBadge(request.status)}</TableCell>
                  <TableCell>
                    {request.dueDate ? new Date(request.dueDate).toLocaleDateString() : '-'}
                  </TableCell>
                  <TableCell>
                    {new Date(request.createdAt).toLocaleDateString()}
                  </TableCell>
                  <TableCell>
                    <div className="flex gap-2">
                      <Button variant="ghost" size="sm">
                        <Eye className="h-4 w-4" />
                      </Button>
                      <Button variant="ghost" size="sm">
                        <Edit className="h-4 w-4" />
                      </Button>
                      <Button variant="ghost" size="sm">
                        <Trash2 className="h-4 w-4" />
                      </Button>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  )
}
