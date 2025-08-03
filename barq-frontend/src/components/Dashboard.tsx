import { useState, useEffect } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { 
  Bot, 
  Workflow, 
  CheckCircle, 
  Clock, 
  TrendingUp, 
  AlertTriangle
} from 'lucide-react'
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, LineChart, Line } from 'recharts'

interface DashboardData {
  aiRequests: {
    total: number;
    pending: number;
    approved: number;
    rejected: number;
    inProgress: number;
  };
  workflows: {
    active: number;
    completed: number;
    failed: number;
  };
  qualityAssessments: {
    pending: number;
    completed: number;
    averageScore: number;
  };
}

interface ChartDataPoint {
  name: string;
  requests: number;
  completed: number;
}

interface QualityTrendPoint {
  name: string;
  score: number;
}

export default function Dashboard() {
  const [dashboardData, setDashboardData] = useState<DashboardData | null>(null)
  const [chartData, setChartData] = useState<ChartDataPoint[]>([])
  const [qualityTrendData, setQualityTrendData] = useState<QualityTrendPoint[]>([])
  const [isLoading, setIsLoading] = useState(true)

  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        setIsLoading(true)
        
        const [statsResponse, chartResponse, qualityResponse] = await Promise.all([
          fetch('/api/dashboard/stats'),
          fetch('/api/dashboard/chart-data'),
          fetch('/api/dashboard/quality-trends')
        ])

        const stats = await statsResponse.json()
        const chart = await chartResponse.json()
        const quality = await qualityResponse.json()

        setDashboardData(stats)
        setChartData(chart)
        setQualityTrendData(quality)
      } catch (error) {
        console.error('Failed to fetch dashboard data:', error)
        setDashboardData({
          aiRequests: { total: 0, pending: 0, approved: 0, rejected: 0, inProgress: 0 },
          workflows: { active: 0, completed: 0, failed: 0 },
          qualityAssessments: { pending: 0, completed: 0, averageScore: 0 }
        })
        setChartData([])
        setQualityTrendData([])
      } finally {
        setIsLoading(false)
      }
    }

    fetchDashboardData()
  }, [])

  if (isLoading) {
    return (
      <div className="space-y-8">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Dashboard</h1>
          <p className="mt-2 text-gray-600 dark:text-gray-400">Loading dashboard data...</p>
        </div>
      </div>
    )
  }

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Dashboard</h1>
        <p className="mt-2 text-gray-600 dark:text-gray-400">
          Overview of your AI request processing and workflow management
        </p>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Total AI Requests</CardTitle>
            <Bot className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{dashboardData?.aiRequests?.total || 0}</div>
            <p className="text-xs text-muted-foreground">
              <span className="text-green-600">+12%</span> from last month
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Active Workflows</CardTitle>
            <Workflow className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{dashboardData?.workflows?.active || 0}</div>
            <p className="text-xs text-muted-foreground">
              <span className="text-blue-600">+5</span> new this week
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Quality Score</CardTitle>
            <CheckCircle className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{dashboardData?.qualityAssessments?.averageScore || 0}%</div>
            <p className="text-xs text-muted-foreground">
              <span className="text-green-600">+2.5%</span> improvement
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Pending Reviews</CardTitle>
            <Clock className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{dashboardData?.aiRequests?.pending || 0}</div>
            <p className="text-xs text-muted-foreground">
              Requires attention
            </p>
          </CardContent>
        </Card>
      </div>

      {/* Charts */}
      <div className="grid grid-cols-1 gap-6 lg:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>AI Request Trends</CardTitle>
            <CardDescription>Monthly request volume and completion rates</CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={chartData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="name" />
                <YAxis />
                <Tooltip />
                <Bar dataKey="requests" fill="#3b82f6" name="Requests" />
                <Bar dataKey="completed" fill="#10b981" name="Completed" />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Quality Score Trend</CardTitle>
            <CardDescription>Weekly quality assessment scores</CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={qualityTrendData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="name" />
                <YAxis domain={[75, 95]} />
                <Tooltip />
                <Line type="monotone" dataKey="score" stroke="#8b5cf6" strokeWidth={2} />
              </LineChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      </div>

      {/* Recent Activity */}
      <div className="grid grid-cols-1 gap-6 lg:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>Recent AI Requests</CardTitle>
            <CardDescription>Latest submitted requests</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {[
                { title: 'Content Generation for Marketing', status: 'approved', priority: 'high' },
                { title: 'Data Analysis Report', status: 'pending', priority: 'normal' },
                { title: 'Code Review Assistant', status: 'in-progress', priority: 'high' },
                { title: 'Translation Service', status: 'completed', priority: 'low' }
              ].map((request, index) => (
                <div key={index} className="flex items-center justify-between">
                  <div>
                    <p className="text-sm font-medium">{request.title}</p>
                    <div className="flex items-center gap-2 mt-1">
                      <Badge variant={
                        request.status === 'approved' ? 'default' :
                        request.status === 'pending' ? 'secondary' :
                        request.status === 'in-progress' ? 'outline' : 'default'
                      }>
                        {request.status}
                      </Badge>
                      <Badge variant={request.priority === 'high' ? 'destructive' : 'outline'}>
                        {request.priority}
                      </Badge>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>System Alerts</CardTitle>
            <CardDescription>Important notifications and warnings</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <div className="flex items-start gap-3">
                <AlertTriangle className="h-5 w-5 text-yellow-500 mt-0.5" />
                <div>
                  <p className="text-sm font-medium">SLA Warning</p>
                  <p className="text-xs text-muted-foreground">3 requests approaching deadline</p>
                </div>
              </div>
              <div className="flex items-start gap-3">
                <CheckCircle className="h-5 w-5 text-green-500 mt-0.5" />
                <div>
                  <p className="text-sm font-medium">Quality Milestone</p>
                  <p className="text-xs text-muted-foreground">Average score exceeded 85%</p>
                </div>
              </div>
              <div className="flex items-start gap-3">
                <TrendingUp className="h-5 w-5 text-blue-500 mt-0.5" />
                <div>
                  <p className="text-sm font-medium">Performance Update</p>
                  <p className="text-xs text-muted-foreground">Processing time improved by 15%</p>
                </div>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  )
}
