import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { 
  BarChart3, 
  Calendar, 
  Users, 
  AlertTriangle, 
  TrendingUp, 
  Clock,
  CheckCircle,
  XCircle,
  Activity
} from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card';
import { Progress } from '../../components/ui/progress';
import { Badge } from '../../components/ui/badge';
import { Button } from '../../components/ui/button';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../components/ui/tabs';
import { LoadingSpinner } from '../../components/ui/loading-spinner';

interface ProjectMetrics {
  id: string;
  name: string;
  description: string;
  status: 'active' | 'completed' | 'on-hold' | 'at-risk';
  progress: number;
  startDate: string;
  endDate: string;
  budget: {
    allocated: number;
    spent: number;
    remaining: number;
  };
  team: {
    totalMembers: number;
    activeMembers: number;
    utilization: number;
  };
  tasks: {
    total: number;
    completed: number;
    inProgress: number;
    overdue: number;
  };
  milestones: {
    total: number;
    completed: number;
    upcoming: number;
  };
  risks: {
    high: number;
    medium: number;
    low: number;
  };
}

interface KPI {
  label: string;
  value: string | number;
  change: number;
  trend: 'up' | 'down' | 'stable';
  icon: React.ComponentType<any>;
}

export function ProjectDashboardPage() {
  const { projectId } = useParams<{ projectId: string }>();
  const [project, setProject] = useState<ProjectMetrics | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchProjectData = async () => {
      try {
        setIsLoading(true);
        await new Promise(resolve => setTimeout(resolve, 1000));
        
        const mockProject: ProjectMetrics = {
          id: projectId || '1',
          name: 'BARQ Platform Development',
          description: 'Enterprise-grade business automation and requirements platform',
          status: 'active',
          progress: 68,
          startDate: '2024-01-15',
          endDate: '2024-12-31',
          budget: {
            allocated: 500000,
            spent: 340000,
            remaining: 160000
          },
          team: {
            totalMembers: 12,
            activeMembers: 10,
            utilization: 85
          },
          tasks: {
            total: 156,
            completed: 89,
            inProgress: 34,
            overdue: 8
          },
          milestones: {
            total: 8,
            completed: 5,
            upcoming: 2
          },
          risks: {
            high: 2,
            medium: 5,
            low: 3
          }
        };
        
        setProject(mockProject);
      } catch (err) {
        setError('Failed to load project data');
      } finally {
        setIsLoading(false);
      }
    };

    fetchProjectData();
  }, [projectId]);

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'active': return 'bg-green-500';
      case 'completed': return 'bg-blue-500';
      case 'on-hold': return 'bg-yellow-500';
      case 'at-risk': return 'bg-red-500';
      default: return 'bg-gray-500';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'active': return 'Active';
      case 'completed': return 'Completed';
      case 'on-hold': return 'On Hold';
      case 'at-risk': return 'At Risk';
      default: return 'Unknown';
    }
  };

  const kpis: KPI[] = project ? [
    {
      label: 'Overall Progress',
      value: `${project.progress}%`,
      change: 5.2,
      trend: 'up',
      icon: TrendingUp
    },
    {
      label: 'Tasks Completed',
      value: `${project.tasks.completed}/${project.tasks.total}`,
      change: 12,
      trend: 'up',
      icon: CheckCircle
    },
    {
      label: 'Team Utilization',
      value: `${project.team.utilization}%`,
      change: -2.1,
      trend: 'down',
      icon: Users
    },
    {
      label: 'Budget Remaining',
      value: `$${(project.budget.remaining / 1000).toFixed(0)}k`,
      change: -8.5,
      trend: 'down',
      icon: BarChart3
    }
  ] : [];

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <LoadingSpinner className="h-8 w-8" />
      </div>
    );
  }

  if (error || !project) {
    return (
      <div className="container mx-auto py-6">
        <Card>
          <CardContent className="flex items-center justify-center py-12">
            <div className="text-center">
              <XCircle className="h-12 w-12 text-red-500 mx-auto mb-4" />
              <h3 className="text-lg font-semibold mb-2">Error Loading Project</h3>
              <p className="text-gray-600">{error || 'Project not found'}</p>
              <Button className="mt-4" onClick={() => window.location.reload()}>
                Try Again
              </Button>
            </div>
          </CardContent>
        </Card>
      </div>
    );
  }

  return (
    <div className="container mx-auto py-6 space-y-6">
      {/* Project Header */}
      <div className="flex items-center justify-between">
        <div>
          <div className="flex items-center gap-3 mb-2">
            <h1 className="text-3xl font-bold">{project.name}</h1>
            <Badge className={`${getStatusColor(project.status)} text-white`}>
              {getStatusText(project.status)}
            </Badge>
          </div>
          <p className="text-gray-600">{project.description}</p>
        </div>
        <div className="flex gap-2">
          <Button variant="outline">
            <Calendar className="h-4 w-4 mr-2" />
            Schedule
          </Button>
          <Button>
            <Activity className="h-4 w-4 mr-2" />
            View Details
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
                    <span className={`text-sm ${
                      kpi.trend === 'up' ? 'text-green-600' : 
                      kpi.trend === 'down' ? 'text-red-600' : 'text-gray-600'
                    }`}>
                      {kpi.change > 0 ? '+' : ''}{kpi.change}%
                    </span>
                    <span className="text-xs text-gray-500 ml-1">vs last month</span>
                  </div>
                </div>
                <kpi.icon className="h-8 w-8 text-gray-400" />
              </div>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Main Dashboard Content */}
      <Tabs defaultValue="overview" className="space-y-6">
        <TabsList>
          <TabsTrigger value="overview">Overview</TabsTrigger>
          <TabsTrigger value="tasks">Tasks</TabsTrigger>
          <TabsTrigger value="team">Team</TabsTrigger>
          <TabsTrigger value="budget">Budget</TabsTrigger>
          <TabsTrigger value="risks">Risks</TabsTrigger>
        </TabsList>

        <TabsContent value="overview" className="space-y-6">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            {/* Progress Overview */}
            <Card>
              <CardHeader>
                <CardTitle>Project Progress</CardTitle>
                <CardDescription>Overall completion status</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div>
                  <div className="flex justify-between text-sm mb-2">
                    <span>Overall Progress</span>
                    <span>{project.progress}%</span>
                  </div>
                  <Progress value={project.progress} className="h-2" />
                </div>
                
                <div className="grid grid-cols-2 gap-4 pt-4">
                  <div className="text-center">
                    <p className="text-2xl font-bold text-green-600">{project.tasks.completed}</p>
                    <p className="text-sm text-gray-600">Completed Tasks</p>
                  </div>
                  <div className="text-center">
                    <p className="text-2xl font-bold text-blue-600">{project.tasks.inProgress}</p>
                    <p className="text-sm text-gray-600">In Progress</p>
                  </div>
                </div>
              </CardContent>
            </Card>

            {/* Timeline */}
            <Card>
              <CardHeader>
                <CardTitle>Project Timeline</CardTitle>
                <CardDescription>Key dates and milestones</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="flex justify-between items-center">
                  <span className="text-sm font-medium">Start Date</span>
                  <span className="text-sm">{new Date(project.startDate).toLocaleDateString()}</span>
                </div>
                <div className="flex justify-between items-center">
                  <span className="text-sm font-medium">End Date</span>
                  <span className="text-sm">{new Date(project.endDate).toLocaleDateString()}</span>
                </div>
                <div className="flex justify-between items-center">
                  <span className="text-sm font-medium">Milestones Completed</span>
                  <span className="text-sm">{project.milestones.completed}/{project.milestones.total}</span>
                </div>
                <div className="pt-2">
                  <div className="flex justify-between text-sm mb-2">
                    <span>Timeline Progress</span>
                    <span>{Math.round((project.milestones.completed / project.milestones.total) * 100)}%</span>
                  </div>
                  <Progress value={(project.milestones.completed / project.milestones.total) * 100} className="h-2" />
                </div>
              </CardContent>
            </Card>
          </div>

          {/* Team and Budget Overview */}
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <Card>
              <CardHeader>
                <CardTitle>Team Overview</CardTitle>
                <CardDescription>Team composition and utilization</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="flex justify-between items-center">
                  <span className="text-sm font-medium">Total Members</span>
                  <span className="text-sm">{project.team.totalMembers}</span>
                </div>
                <div className="flex justify-between items-center">
                  <span className="text-sm font-medium">Active Members</span>
                  <span className="text-sm">{project.team.activeMembers}</span>
                </div>
                <div className="pt-2">
                  <div className="flex justify-between text-sm mb-2">
                    <span>Team Utilization</span>
                    <span>{project.team.utilization}%</span>
                  </div>
                  <Progress value={project.team.utilization} className="h-2" />
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Budget Overview</CardTitle>
                <CardDescription>Financial status and spending</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="flex justify-between items-center">
                  <span className="text-sm font-medium">Allocated</span>
                  <span className="text-sm">${(project.budget.allocated / 1000).toFixed(0)}k</span>
                </div>
                <div className="flex justify-between items-center">
                  <span className="text-sm font-medium">Spent</span>
                  <span className="text-sm">${(project.budget.spent / 1000).toFixed(0)}k</span>
                </div>
                <div className="flex justify-between items-center">
                  <span className="text-sm font-medium">Remaining</span>
                  <span className="text-sm">${(project.budget.remaining / 1000).toFixed(0)}k</span>
                </div>
                <div className="pt-2">
                  <div className="flex justify-between text-sm mb-2">
                    <span>Budget Used</span>
                    <span>{Math.round((project.budget.spent / project.budget.allocated) * 100)}%</span>
                  </div>
                  <Progress value={(project.budget.spent / project.budget.allocated) * 100} className="h-2" />
                </div>
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        <TabsContent value="tasks">
          <Card>
            <CardHeader>
              <CardTitle>Task Management</CardTitle>
              <CardDescription>Detailed task breakdown and status</CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-gray-500">Task management interface coming soon...</p>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="team">
          <Card>
            <CardHeader>
              <CardTitle>Team Management</CardTitle>
              <CardDescription>Team members and collaboration tools</CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-gray-500">Team management interface coming soon...</p>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="budget">
          <Card>
            <CardHeader>
              <CardTitle>Budget Analysis</CardTitle>
              <CardDescription>Financial tracking and budget management</CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-gray-500">Budget analysis interface coming soon...</p>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="risks">
          <Card>
            <CardHeader>
              <CardTitle>Risk Assessment</CardTitle>
              <CardDescription>Project risks and mitigation strategies</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="grid grid-cols-3 gap-4">
                <div className="text-center p-4 bg-red-50 rounded-lg">
                  <AlertTriangle className="h-8 w-8 text-red-500 mx-auto mb-2" />
                  <p className="text-2xl font-bold text-red-600">{project.risks.high}</p>
                  <p className="text-sm text-red-700">High Risk</p>
                </div>
                <div className="text-center p-4 bg-yellow-50 rounded-lg">
                  <Clock className="h-8 w-8 text-yellow-500 mx-auto mb-2" />
                  <p className="text-2xl font-bold text-yellow-600">{project.risks.medium}</p>
                  <p className="text-sm text-yellow-700">Medium Risk</p>
                </div>
                <div className="text-center p-4 bg-green-50 rounded-lg">
                  <CheckCircle className="h-8 w-8 text-green-500 mx-auto mb-2" />
                  <p className="text-2xl font-bold text-green-600">{project.risks.low}</p>
                  <p className="text-sm text-green-700">Low Risk</p>
                </div>
              </div>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
