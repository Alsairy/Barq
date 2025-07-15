import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { 
  BarChart3, 
  TrendingUp, 
  TrendingDown, 
  DollarSign, 
  Clock, 
  Users, 
  Target,
  Download,
  RefreshCw
} from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card';
import { Button } from '../../components/ui/button';
import { Badge } from '../../components/ui/badge';
import { Progress } from '../../components/ui/progress';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../components/ui/tabs';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '../../components/ui/select';
import { LoadingSpinner } from '../../components/ui/loading-spinner';

interface AnalyticsData {
  performance: {
    overallProgress: number;
    tasksCompleted: number;
    totalTasks: number;
    onTimeDelivery: number;
    budgetUtilization: number;
    teamEfficiency: number;
  };
  budget: {
    allocated: number;
    spent: number;
    remaining: number;
    burnRate: number;
    projectedOverrun: number;
    costPerTask: number;
  };
  timeline: {
    originalEndDate: string;
    currentProjectedEndDate: string;
    daysAhead: number;
    daysBehind: number;
    milestoneCompletion: number;
  };
  quality: {
    defectRate: number;
    customerSatisfaction: number;
    codeQuality: number;
    testCoverage: number;
    reviewApprovalRate: number;
  };
  resources: {
    teamUtilization: number;
    averageHoursPerTask: number;
    overtimeHours: number;
    skillGapAnalysis: {
      skill: string;
      demand: number;
      supply: number;
      gap: number;
    }[];
  };
  trends: {
    velocityTrend: number[];
    burndownData: { date: string; planned: number; actual: number }[];
    qualityTrend: number[];
    budgetTrend: number[];
  };
}

interface KPI {
  label: string;
  value: string | number;
  change: number;
  trend: 'up' | 'down' | 'stable';
  icon: React.ComponentType<any>;
  color: string;
}

export function ProjectAnalyticsPage() {
  const { projectId } = useParams<{ projectId: string }>();
  const [analyticsData, setAnalyticsData] = useState<AnalyticsData | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [timeRange, setTimeRange] = useState('30d');
  const [refreshing, setRefreshing] = useState(false);

  useEffect(() => {
    const fetchAnalyticsData = async () => {
      try {
        setIsLoading(true);
        await new Promise(resolve => setTimeout(resolve, 1000));
        
        const mockData: AnalyticsData = {
          performance: {
            overallProgress: 68,
            tasksCompleted: 89,
            totalTasks: 156,
            onTimeDelivery: 85,
            budgetUtilization: 72,
            teamEfficiency: 91
          },
          budget: {
            allocated: 500000,
            spent: 340000,
            remaining: 160000,
            burnRate: 15000,
            projectedOverrun: 25000,
            costPerTask: 3820
          },
          timeline: {
            originalEndDate: '2024-12-31',
            currentProjectedEndDate: '2025-01-15',
            daysAhead: 0,
            daysBehind: 15,
            milestoneCompletion: 75
          },
          quality: {
            defectRate: 2.3,
            customerSatisfaction: 4.2,
            codeQuality: 87,
            testCoverage: 82,
            reviewApprovalRate: 94
          },
          resources: {
            teamUtilization: 85,
            averageHoursPerTask: 12.5,
            overtimeHours: 45,
            skillGapAnalysis: [
              { skill: 'React Development', demand: 80, supply: 90, gap: -10 },
              { skill: 'DevOps', demand: 70, supply: 50, gap: 20 },
              { skill: 'UI/UX Design', demand: 60, supply: 75, gap: -15 },
              { skill: 'Backend Development', demand: 85, supply: 80, gap: 5 }
            ]
          },
          trends: {
            velocityTrend: [12, 15, 18, 16, 20, 22, 19, 24, 21, 25, 23, 27],
            burndownData: [
              { date: '2024-01-01', planned: 100, actual: 100 },
              { date: '2024-01-15', planned: 85, actual: 88 },
              { date: '2024-02-01', planned: 70, actual: 75 },
              { date: '2024-02-15', planned: 55, actual: 62 },
              { date: '2024-03-01', planned: 40, actual: 48 },
              { date: '2024-03-15', planned: 25, actual: 35 },
              { date: '2024-04-01', planned: 10, actual: 22 },
              { date: '2024-04-15', planned: 0, actual: 12 }
            ],
            qualityTrend: [82, 85, 83, 87, 89, 86, 88, 90, 87, 89, 91, 87],
            budgetTrend: [95, 88, 82, 78, 75, 72, 69, 65, 62, 58, 55, 52]
          }
        };
        
        setAnalyticsData(mockData);
      } catch (error) {
        console.error('Failed to fetch analytics data:', error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchAnalyticsData();
  }, [projectId, timeRange]);

  const handleRefresh = async () => {
    setRefreshing(true);
    await new Promise(resolve => setTimeout(resolve, 1000));
    setRefreshing(false);
  };

  const kpis: KPI[] = analyticsData ? [
    {
      label: 'Overall Progress',
      value: `${analyticsData.performance.overallProgress}%`,
      change: 5.2,
      trend: 'up',
      icon: Target,
      color: 'text-blue-600'
    },
    {
      label: 'Budget Utilization',
      value: `${analyticsData.performance.budgetUtilization}%`,
      change: -2.1,
      trend: 'down',
      icon: DollarSign,
      color: 'text-green-600'
    },
    {
      label: 'Team Efficiency',
      value: `${analyticsData.performance.teamEfficiency}%`,
      change: 8.3,
      trend: 'up',
      icon: Users,
      color: 'text-purple-600'
    },
    {
      label: 'On-Time Delivery',
      value: `${analyticsData.performance.onTimeDelivery}%`,
      change: -1.5,
      trend: 'down',
      icon: Clock,
      color: 'text-orange-600'
    }
  ] : [];

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <LoadingSpinner className="h-8 w-8" />
      </div>
    );
  }

  if (!analyticsData) {
    return (
      <div className="container mx-auto py-6">
        <Card>
          <CardContent className="flex items-center justify-center py-12">
            <div className="text-center">
              <BarChart3 className="h-12 w-12 text-gray-400 mx-auto mb-4" />
              <h3 className="text-lg font-semibold mb-2">No Analytics Data</h3>
              <p className="text-gray-600">Analytics data is not available for this project</p>
            </div>
          </CardContent>
        </Card>
      </div>
    );
  }

  return (
    <div className="container mx-auto py-6 space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Project Analytics</h1>
          <p className="text-gray-600">Comprehensive insights and performance metrics</p>
        </div>
        <div className="flex gap-2">
          <Select value={timeRange} onValueChange={setTimeRange}>
            <SelectTrigger className="w-[140px]">
              <SelectValue />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="7d">Last 7 days</SelectItem>
              <SelectItem value="30d">Last 30 days</SelectItem>
              <SelectItem value="90d">Last 90 days</SelectItem>
              <SelectItem value="1y">Last year</SelectItem>
            </SelectContent>
          </Select>
          <Button variant="outline" onClick={handleRefresh} disabled={refreshing}>
            <RefreshCw className={`h-4 w-4 mr-2 ${refreshing ? 'animate-spin' : ''}`} />
            Refresh
          </Button>
          <Button variant="outline">
            <Download className="h-4 w-4 mr-2" />
            Export
          </Button>
        </div>
      </div>

      {/* KPI Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {kpis.map((kpi, index) => (
          <Card key={index}>
            <CardContent className="p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm font-medium text-gray-600">{kpi.label}</p>
                  <p className="text-2xl font-bold">{kpi.value}</p>
                  <div className="flex items-center mt-1">
                    {kpi.trend === 'up' ? (
                      <TrendingUp className="h-4 w-4 text-green-600 mr-1" />
                    ) : kpi.trend === 'down' ? (
                      <TrendingDown className="h-4 w-4 text-red-600 mr-1" />
                    ) : null}
                    <span className={`text-sm ${
                      kpi.trend === 'up' ? 'text-green-600' : 
                      kpi.trend === 'down' ? 'text-red-600' : 'text-gray-600'
                    }`}>
                      {kpi.change > 0 ? '+' : ''}{kpi.change}%
                    </span>
                    <span className="text-xs text-gray-500 ml-1">vs last period</span>
                  </div>
                </div>
                <kpi.icon className={`h-8 w-8 ${kpi.color}`} />
              </div>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Analytics Tabs */}
      <Tabs defaultValue="performance" className="space-y-6">
        <TabsList>
          <TabsTrigger value="performance">Performance</TabsTrigger>
          <TabsTrigger value="budget">Budget</TabsTrigger>
          <TabsTrigger value="timeline">Timeline</TabsTrigger>
          <TabsTrigger value="quality">Quality</TabsTrigger>
          <TabsTrigger value="resources">Resources</TabsTrigger>
        </TabsList>

        <TabsContent value="performance" className="space-y-6">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <Card>
              <CardHeader>
                <CardTitle>Task Completion</CardTitle>
                <CardDescription>Progress on project tasks</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="text-center">
                  <div className="text-3xl font-bold text-blue-600">
                    {analyticsData.performance.tasksCompleted}/{analyticsData.performance.totalTasks}
                  </div>
                  <p className="text-sm text-gray-600">Tasks Completed</p>
                </div>
                <Progress 
                  value={(analyticsData.performance.tasksCompleted / analyticsData.performance.totalTasks) * 100} 
                  className="h-3"
                />
                <div className="grid grid-cols-2 gap-4 pt-4">
                  <div className="text-center">
                    <p className="text-lg font-semibold text-green-600">{analyticsData.performance.onTimeDelivery}%</p>
                    <p className="text-xs text-gray-600">On-Time Delivery</p>
                  </div>
                  <div className="text-center">
                    <p className="text-lg font-semibold text-purple-600">{analyticsData.performance.teamEfficiency}%</p>
                    <p className="text-xs text-gray-600">Team Efficiency</p>
                  </div>
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Velocity Trend</CardTitle>
                <CardDescription>Team velocity over time</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="h-[200px] flex items-center justify-center text-gray-500">
                  <div className="text-center">
                    <BarChart3 className="h-12 w-12 mx-auto mb-2" />
                    <p>Velocity chart visualization</p>
                    <p className="text-sm">Chart.js integration needed</p>
                  </div>
                </div>
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        <TabsContent value="budget" className="space-y-6">
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
            <Card>
              <CardHeader>
                <CardTitle>Budget Overview</CardTitle>
                <CardDescription>Financial status</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="space-y-2">
                  <div className="flex justify-between">
                    <span className="text-sm">Allocated</span>
                    <span className="font-medium">${(analyticsData.budget.allocated / 1000).toFixed(0)}k</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-sm">Spent</span>
                    <span className="font-medium">${(analyticsData.budget.spent / 1000).toFixed(0)}k</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-sm">Remaining</span>
                    <span className="font-medium">${(analyticsData.budget.remaining / 1000).toFixed(0)}k</span>
                  </div>
                </div>
                <Progress value={(analyticsData.budget.spent / analyticsData.budget.allocated) * 100} className="h-2" />
                <div className="pt-2">
                  <p className="text-sm text-gray-600">
                    Burn Rate: ${(analyticsData.budget.burnRate / 1000).toFixed(0)}k/month
                  </p>
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Cost Analysis</CardTitle>
                <CardDescription>Cost breakdown and efficiency</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="text-center">
                  <div className="text-2xl font-bold text-green-600">
                    ${analyticsData.budget.costPerTask.toLocaleString()}
                  </div>
                  <p className="text-sm text-gray-600">Cost per Task</p>
                </div>
                {analyticsData.budget.projectedOverrun > 0 && (
                  <div className="p-3 bg-red-50 rounded-lg">
                    <div className="flex items-center gap-2">
                      <TrendingUp className="h-4 w-4 text-red-500" />
                      <span className="text-sm font-medium text-red-700">
                        Projected Overrun: ${(analyticsData.budget.projectedOverrun / 1000).toFixed(0)}k
                      </span>
                    </div>
                  </div>
                )}
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Budget Trend</CardTitle>
                <CardDescription>Spending pattern over time</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="h-[150px] flex items-center justify-center text-gray-500">
                  <div className="text-center">
                    <TrendingDown className="h-8 w-8 mx-auto mb-2" />
                    <p className="text-sm">Budget trend chart</p>
                  </div>
                </div>
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        <TabsContent value="timeline" className="space-y-6">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <Card>
              <CardHeader>
                <CardTitle>Timeline Status</CardTitle>
                <CardDescription>Project schedule analysis</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="space-y-3">
                  <div className="flex justify-between items-center">
                    <span className="text-sm">Original End Date</span>
                    <span className="font-medium">
                      {new Date(analyticsData.timeline.originalEndDate).toLocaleDateString()}
                    </span>
                  </div>
                  <div className="flex justify-between items-center">
                    <span className="text-sm">Projected End Date</span>
                    <span className="font-medium">
                      {new Date(analyticsData.timeline.currentProjectedEndDate).toLocaleDateString()}
                    </span>
                  </div>
                  <div className="flex justify-between items-center">
                    <span className="text-sm">Schedule Status</span>
                    <Badge variant={analyticsData.timeline.daysBehind > 0 ? 'destructive' : 'default'}>
                      {analyticsData.timeline.daysBehind > 0 
                        ? `${analyticsData.timeline.daysBehind} days behind`
                        : analyticsData.timeline.daysAhead > 0
                        ? `${analyticsData.timeline.daysAhead} days ahead`
                        : 'On schedule'
                      }
                    </Badge>
                  </div>
                </div>
                <div className="pt-4">
                  <div className="flex justify-between text-sm mb-2">
                    <span>Milestone Completion</span>
                    <span>{analyticsData.timeline.milestoneCompletion}%</span>
                  </div>
                  <Progress value={analyticsData.timeline.milestoneCompletion} className="h-2" />
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Burndown Chart</CardTitle>
                <CardDescription>Work remaining over time</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="h-[200px] flex items-center justify-center text-gray-500">
                  <div className="text-center">
                    <TrendingDown className="h-12 w-12 mx-auto mb-2" />
                    <p>Burndown chart visualization</p>
                    <p className="text-sm">Chart.js integration needed</p>
                  </div>
                </div>
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        <TabsContent value="quality" className="space-y-6">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <Card>
              <CardHeader>
                <CardTitle>Quality Metrics</CardTitle>
                <CardDescription>Code and delivery quality indicators</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-2 gap-4">
                  <div className="text-center p-3 bg-blue-50 rounded-lg">
                    <p className="text-2xl font-bold text-blue-600">{analyticsData.quality.codeQuality}%</p>
                    <p className="text-sm text-blue-700">Code Quality</p>
                  </div>
                  <div className="text-center p-3 bg-green-50 rounded-lg">
                    <p className="text-2xl font-bold text-green-600">{analyticsData.quality.testCoverage}%</p>
                    <p className="text-sm text-green-700">Test Coverage</p>
                  </div>
                  <div className="text-center p-3 bg-purple-50 rounded-lg">
                    <p className="text-2xl font-bold text-purple-600">{analyticsData.quality.reviewApprovalRate}%</p>
                    <p className="text-sm text-purple-700">Review Approval</p>
                  </div>
                  <div className="text-center p-3 bg-orange-50 rounded-lg">
                    <p className="text-2xl font-bold text-orange-600">{analyticsData.quality.defectRate}%</p>
                    <p className="text-sm text-orange-700">Defect Rate</p>
                  </div>
                </div>
                <div className="pt-4">
                  <div className="flex items-center justify-between">
                    <span className="text-sm">Customer Satisfaction</span>
                    <div className="flex items-center gap-2">
                      <span className="font-medium">{analyticsData.quality.customerSatisfaction}/5.0</span>
                      <div className="flex">
                        {[1, 2, 3, 4, 5].map((star) => (
                          <span
                            key={star}
                            className={`text-sm ${
                              star <= analyticsData.quality.customerSatisfaction
                                ? 'text-yellow-400'
                                : 'text-gray-300'
                            }`}
                          >
                            ★
                          </span>
                        ))}
                      </div>
                    </div>
                  </div>
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Quality Trend</CardTitle>
                <CardDescription>Quality metrics over time</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="h-[200px] flex items-center justify-center text-gray-500">
                  <div className="text-center">
                    <TrendingUp className="h-12 w-12 mx-auto mb-2" />
                    <p>Quality trend chart</p>
                    <p className="text-sm">Chart.js integration needed</p>
                  </div>
                </div>
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        <TabsContent value="resources" className="space-y-6">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <Card>
              <CardHeader>
                <CardTitle>Resource Utilization</CardTitle>
                <CardDescription>Team capacity and efficiency</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="space-y-3">
                  <div>
                    <div className="flex justify-between text-sm mb-1">
                      <span>Team Utilization</span>
                      <span>{analyticsData.resources.teamUtilization}%</span>
                    </div>
                    <Progress value={analyticsData.resources.teamUtilization} className="h-2" />
                  </div>
                  <div className="grid grid-cols-2 gap-4 pt-4">
                    <div className="text-center">
                      <p className="text-lg font-semibold">{analyticsData.resources.averageHoursPerTask}h</p>
                      <p className="text-xs text-gray-600">Avg Hours/Task</p>
                    </div>
                    <div className="text-center">
                      <p className="text-lg font-semibold">{analyticsData.resources.overtimeHours}h</p>
                      <p className="text-xs text-gray-600">Overtime Hours</p>
                    </div>
                  </div>
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Skill Gap Analysis</CardTitle>
                <CardDescription>Team skills vs project requirements</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="space-y-3">
                  {analyticsData.resources.skillGapAnalysis.map((skill, index) => (
                    <div key={index} className="space-y-2">
                      <div className="flex justify-between text-sm">
                        <span>{skill.skill}</span>
                        <Badge variant={skill.gap > 0 ? 'destructive' : 'default'}>
                          {skill.gap > 0 ? `+${skill.gap}` : skill.gap} gap
                        </Badge>
                      </div>
                      <div className="grid grid-cols-2 gap-2">
                        <div>
                          <div className="text-xs text-gray-600 mb-1">Supply</div>
                          <Progress value={skill.supply} className="h-1" />
                        </div>
                        <div>
                          <div className="text-xs text-gray-600 mb-1">Demand</div>
                          <Progress value={skill.demand} className="h-1" />
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              </CardContent>
            </Card>
          </div>
        </TabsContent>
      </Tabs>
    </div>
  );
}
