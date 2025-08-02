import { useState } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from '@/components/ui/dialog'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { Progress } from '@/components/ui/progress'
import { 
  Search, 
  Eye, 
  Edit, 
  CheckCircle, 
  Clock, 
  Star, 
  AlertTriangle
} from 'lucide-react'
import { QualityAssessment, QualityAssessmentType, QualityAssessmentStatus, CompleteQualityAssessmentRequest } from '@/types/api'

const mockAssessments: QualityAssessment[] = [
  {
    id: '1',
    aiRequestId: '1',
    assessorId: 'assessor1',
    type: QualityAssessmentType.Manual,
    status: QualityAssessmentStatus.Completed,
    qualityScore: 92,
    comments: 'Excellent quality output with minor formatting improvements needed',
    recommendations: 'Consider standardizing the output format for consistency',
    completedAt: '2024-08-02T14:30:00Z',
    qualityCriteria: 'Accuracy, Relevance, Clarity, Completeness',
    assessmentResults: 'High quality content that meets all requirements',
    requiresReview: false,
    createdAt: '2024-08-01T10:00:00Z',
    updatedAt: '2024-08-02T14:30:00Z'
  },
  {
    id: '2',
    aiRequestId: '2',
    assessorId: 'assessor2',
    type: QualityAssessmentType.Automated,
    status: QualityAssessmentStatus.InProgress,
    qualityScore: 0,
    qualityCriteria: 'Data accuracy, Statistical validity, Visualization quality',
    assessmentResults: '',
    requiresReview: true,
    createdAt: '2024-08-01T15:00:00Z',
    updatedAt: '2024-08-02T16:00:00Z'
  },
  {
    id: '3',
    aiRequestId: '3',
    assessorId: 'assessor3',
    type: QualityAssessmentType.Expert,
    status: QualityAssessmentStatus.Pending,
    qualityScore: 0,
    qualityCriteria: 'Security compliance, Code quality, Performance impact',
    assessmentResults: '',
    requiresReview: false,
    createdAt: '2024-08-02T09:00:00Z',
    updatedAt: '2024-08-02T09:00:00Z'
  }
]

export default function QualityAssurancePage() {
  const [assessments, setAssessments] = useState<QualityAssessment[]>(mockAssessments)
  const [searchTerm, setSearchTerm] = useState('')
  const [statusFilter, setStatusFilter] = useState<string>('all')
  const [typeFilter, setTypeFilter] = useState<string>('all')
  const [isCompleteDialogOpen, setIsCompleteDialogOpen] = useState(false)
  const [selectedAssessment, setSelectedAssessment] = useState<QualityAssessment | null>(null)
  const [completionData, setCompletionData] = useState<CompleteQualityAssessmentRequest>({
    qualityScore: 0,
    comments: '',
    recommendations: '',
    assessmentResults: '',
    requiresReview: false,
    status: QualityAssessmentStatus.Completed
  })

  const filteredAssessments = assessments.filter(assessment => {
    const matchesSearch = assessment.id.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         assessment.aiRequestId.toLowerCase().includes(searchTerm.toLowerCase())
    const matchesStatus = statusFilter === 'all' || assessment.status.toString() === statusFilter
    const matchesType = typeFilter === 'all' || assessment.type.toString() === typeFilter
    
    return matchesSearch && matchesStatus && matchesType
  })

  const getStatusBadge = (status: QualityAssessmentStatus) => {
    const statusConfig = {
      [QualityAssessmentStatus.Pending]: { variant: 'secondary' as const, label: 'Pending' },
      [QualityAssessmentStatus.InProgress]: { variant: 'default' as const, label: 'In Progress' },
      [QualityAssessmentStatus.Completed]: { variant: 'default' as const, label: 'Completed' },
      [QualityAssessmentStatus.Approved]: { variant: 'default' as const, label: 'Approved' },
      [QualityAssessmentStatus.Rejected]: { variant: 'destructive' as const, label: 'Rejected' },
      [QualityAssessmentStatus.RequiresReview]: { variant: 'outline' as const, label: 'Requires Review' }
    }
    
    const config = statusConfig[status]
    return <Badge variant={config.variant}>{config.label}</Badge>
  }

  const getTypeBadge = (type: QualityAssessmentType) => {
    const typeConfig = {
      [QualityAssessmentType.Automated]: { variant: 'outline' as const, label: 'Automated' },
      [QualityAssessmentType.Manual]: { variant: 'default' as const, label: 'Manual' },
      [QualityAssessmentType.Peer]: { variant: 'secondary' as const, label: 'Peer Review' },
      [QualityAssessmentType.Expert]: { variant: 'default' as const, label: 'Expert Review' }
    }
    
    const config = typeConfig[type]
    return <Badge variant={config.variant}>{config.label}</Badge>
  }

  const getQualityScoreColor = (score: number) => {
    if (score >= 90) return 'text-green-600'
    if (score >= 80) return 'text-blue-600'
    if (score >= 70) return 'text-yellow-600'
    return 'text-red-600'
  }

  const handleCompleteAssessment = () => {
    if (!selectedAssessment) return
    
    const updatedAssessment: QualityAssessment = {
      ...selectedAssessment,
      ...completionData,
      completedAt: new Date().toISOString(),
      updatedAt: new Date().toISOString()
    }
    
    setAssessments(assessments.map(a => a.id === selectedAssessment.id ? updatedAssessment : a))
    setIsCompleteDialogOpen(false)
    setSelectedAssessment(null)
    setCompletionData({
      qualityScore: 0,
      comments: '',
      recommendations: '',
      assessmentResults: '',
      requiresReview: false,
      status: QualityAssessmentStatus.Completed
    })
  }

  const openCompleteDialog = (assessment: QualityAssessment) => {
    setSelectedAssessment(assessment)
    setCompletionData({
      qualityScore: assessment.qualityScore,
      comments: assessment.comments || '',
      recommendations: assessment.recommendations || '',
      assessmentResults: assessment.assessmentResults,
      requiresReview: assessment.requiresReview,
      status: QualityAssessmentStatus.Completed
    })
    setIsCompleteDialogOpen(true)
  }

  const averageScore = assessments
    .filter(a => a.status === QualityAssessmentStatus.Completed && a.qualityScore > 0)
    .reduce((sum, a) => sum + a.qualityScore, 0) / 
    assessments.filter(a => a.status === QualityAssessmentStatus.Completed && a.qualityScore > 0).length || 0

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Quality Assurance</h1>
          <p className="mt-2 text-gray-600 dark:text-gray-400">
            Monitor and manage quality assessments for AI requests
          </p>
        </div>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Pending Assessments</CardTitle>
            <Clock className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              {assessments.filter(a => a.status === QualityAssessmentStatus.Pending).length}
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
              {assessments.filter(a => a.status === QualityAssessmentStatus.Completed).length}
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Average Score</CardTitle>
            <Star className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className={`text-2xl font-bold ${getQualityScoreColor(averageScore)}`}>
              {averageScore.toFixed(1)}%
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Requires Review</CardTitle>
            <AlertTriangle className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              {assessments.filter(a => a.requiresReview).length}
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
                  placeholder="Search assessments..."
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
                  <SelectItem value="0">Pending</SelectItem>
                  <SelectItem value="1">In Progress</SelectItem>
                  <SelectItem value="2">Completed</SelectItem>
                  <SelectItem value="3">Approved</SelectItem>
                  <SelectItem value="4">Rejected</SelectItem>
                  <SelectItem value="5">Requires Review</SelectItem>
                </SelectContent>
              </Select>
            </div>
            <div>
              <Label htmlFor="type-filter">Type</Label>
              <Select value={typeFilter} onValueChange={setTypeFilter}>
                <SelectTrigger className="w-[180px]">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">All Types</SelectItem>
                  <SelectItem value="0">Automated</SelectItem>
                  <SelectItem value="1">Manual</SelectItem>
                  <SelectItem value="2">Peer Review</SelectItem>
                  <SelectItem value="3">Expert Review</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Assessments Table */}
      <Card>
        <CardHeader>
          <CardTitle>Quality Assessments ({filteredAssessments.length})</CardTitle>
          <CardDescription>
            Review and manage quality assessments for AI request outputs
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Assessment ID</TableHead>
                <TableHead>AI Request</TableHead>
                <TableHead>Type</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Quality Score</TableHead>
                <TableHead>Created</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filteredAssessments.map((assessment) => (
                <TableRow key={assessment.id}>
                  <TableCell>
                    <div className="font-mono text-sm">{assessment.id}</div>
                  </TableCell>
                  <TableCell>
                    <div className="font-mono text-sm">{assessment.aiRequestId}</div>
                  </TableCell>
                  <TableCell>{getTypeBadge(assessment.type)}</TableCell>
                  <TableCell>{getStatusBadge(assessment.status)}</TableCell>
                  <TableCell>
                    {assessment.qualityScore > 0 ? (
                      <div className="flex items-center gap-2">
                        <div className={`font-bold ${getQualityScoreColor(assessment.qualityScore)}`}>
                          {assessment.qualityScore}%
                        </div>
                        <Progress value={assessment.qualityScore} className="w-[60px]" />
                      </div>
                    ) : (
                      <span className="text-muted-foreground">-</span>
                    )}
                  </TableCell>
                  <TableCell>
                    {new Date(assessment.createdAt).toLocaleDateString()}
                  </TableCell>
                  <TableCell>
                    <div className="flex gap-2">
                      <Button variant="ghost" size="sm">
                        <Eye className="h-4 w-4" />
                      </Button>
                      {(assessment.status === QualityAssessmentStatus.Pending || 
                        assessment.status === QualityAssessmentStatus.InProgress) && (
                        <Button 
                          variant="ghost" 
                          size="sm"
                          onClick={() => openCompleteDialog(assessment)}
                        >
                          <Edit className="h-4 w-4" />
                        </Button>
                      )}
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>

      {/* Complete Assessment Dialog */}
      <Dialog open={isCompleteDialogOpen} onOpenChange={setIsCompleteDialogOpen}>
        <DialogContent className="sm:max-w-[600px]">
          <DialogHeader>
            <DialogTitle>Complete Quality Assessment</DialogTitle>
            <DialogDescription>
              Provide your assessment results and quality score for this AI request.
            </DialogDescription>
          </DialogHeader>
          <div className="grid gap-4 py-4">
            <div className="grid gap-2">
              <Label htmlFor="qualityScore">Quality Score (0-100)</Label>
              <Input
                id="qualityScore"
                type="number"
                min="0"
                max="100"
                value={completionData.qualityScore}
                onChange={(e) => setCompletionData({...completionData, qualityScore: parseInt(e.target.value) || 0})}
              />
            </div>
            <div className="grid gap-2">
              <Label htmlFor="assessmentResults">Assessment Results</Label>
              <Textarea
                id="assessmentResults"
                value={completionData.assessmentResults}
                onChange={(e) => setCompletionData({...completionData, assessmentResults: e.target.value})}
                placeholder="Detailed assessment results..."
                rows={3}
              />
            </div>
            <div className="grid gap-2">
              <Label htmlFor="comments">Comments</Label>
              <Textarea
                id="comments"
                value={completionData.comments}
                onChange={(e) => setCompletionData({...completionData, comments: e.target.value})}
                placeholder="Additional comments..."
                rows={2}
              />
            </div>
            <div className="grid gap-2">
              <Label htmlFor="recommendations">Recommendations</Label>
              <Textarea
                id="recommendations"
                value={completionData.recommendations}
                onChange={(e) => setCompletionData({...completionData, recommendations: e.target.value})}
                placeholder="Recommendations for improvement..."
                rows={2}
              />
            </div>
            <div className="flex items-center space-x-2">
              <input
                type="checkbox"
                id="requiresReview"
                checked={completionData.requiresReview}
                onChange={(e) => setCompletionData({...completionData, requiresReview: e.target.checked})}
              />
              <Label htmlFor="requiresReview">Requires additional review</Label>
            </div>
            <div className="grid gap-2">
              <Label htmlFor="status">Final Status</Label>
              <Select 
                value={completionData.status.toString()} 
                onValueChange={(value) => setCompletionData({...completionData, status: parseInt(value) as QualityAssessmentStatus})}
              >
                <SelectTrigger>
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="2">Completed</SelectItem>
                  <SelectItem value="3">Approved</SelectItem>
                  <SelectItem value="4">Rejected</SelectItem>
                  <SelectItem value="5">Requires Review</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>
          <div className="flex justify-end gap-2">
            <Button variant="outline" onClick={() => setIsCompleteDialogOpen(false)}>
              Cancel
            </Button>
            <Button onClick={handleCompleteAssessment}>
              Complete Assessment
            </Button>
          </div>
        </DialogContent>
      </Dialog>
    </div>
  )
}
