 import { useState } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Label } from '../../components/ui/label';
import { Badge } from '../../components/ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../components/ui/tabs';
import { Progress } from '../../components/ui/progress';
import {
  GitBranch,
  GitCommit,
  GitPullRequest,
  Play,
  CheckCircle,
  XCircle,
  Clock,
  Settings,
  Webhook,
  RefreshCw,
  ExternalLink,
  AlertTriangle,
  Database,
  Code,
  Bug,
  Zap,
  MoreHorizontal
} from 'lucide-react';

interface GitRepository {
  id: string;
  name: string;
  url: string;
  branch: string;
  lastCommit: {
    hash: string;
    message: string;
    author: string;
    timestamp: string;
  };
  status: 'connected' | 'disconnected' | 'syncing';
  pullRequests: number;
  issues: number;
}

interface CIPipeline {
  id: string;
  name: string;
  repository: string;
  status: 'running' | 'success' | 'failed' | 'pending';
  lastRun: string;
  duration: number;
  branch: string;
  commit: string;
  stages: PipelineStage[];
}

interface PipelineStage {
  id: string;
  name: string;
  status: 'running' | 'success' | 'failed' | 'pending' | 'skipped';
  duration?: number;
  logs?: string;
}

interface IssueTracker {
  id: string;
  name: string;
  type: 'jira' | 'github' | 'gitlab' | 'azure-devops';
  url: string;
  status: 'connected' | 'disconnected' | 'syncing';
  openIssues: number;
  closedIssues: number;
  lastSync: string;
}

interface WebhookConfig {
  id: string;
  name: string;
  url: string;
  events: string[];
  status: 'active' | 'inactive' | 'failed';
  lastTriggered?: string;
  deliveries: number;
  failures: number;
}

export default function DevToolsIntegrationPage() {
  const [activeTab, setActiveTab] = useState('git');
  const [searchTerm, setSearchTerm] = useState('');

  const repositories: GitRepository[] = [
    {
      id: '1',
      name: 'barq-frontend',
      url: 'https://github.com/company/barq-frontend',
      branch: 'main',
      lastCommit: {
        hash: 'a1b2c3d',
        message: 'Add integration dashboard',
        author: 'John Doe',
        timestamp: '2024-01-15T10:30:00Z'
      },
      status: 'connected',
      pullRequests: 3,
      issues: 12
    },
    {
      id: '2',
      name: 'barq-backend',
      url: 'https://github.com/company/barq-backend',
      branch: 'develop',
      lastCommit: {
        hash: 'x9y8z7w',
        message: 'Fix authentication service',
        author: 'Jane Smith',
        timestamp: '2024-01-15T09:15:00Z'
      },
      status: 'connected',
      pullRequests: 1,
      issues: 8
    }
  ];

  const pipelines: CIPipeline[] = [
    {
      id: '1',
      name: 'Frontend Build & Deploy',
      repository: 'barq-frontend',
      status: 'success',
      lastRun: '2024-01-15T10:45:00Z',
      duration: 420,
      branch: 'main',
      commit: 'a1b2c3d',
      stages: [
        { id: '1', name: 'Build', status: 'success', duration: 120 },
        { id: '2', name: 'Test', status: 'success', duration: 180 },
        { id: '3', name: 'Deploy', status: 'success', duration: 120 }
      ]
    },
    {
      id: '2',
      name: 'Backend API Tests',
      repository: 'barq-backend',
      status: 'running',
      lastRun: '2024-01-15T11:00:00Z',
      duration: 0,
      branch: 'develop',
      commit: 'x9y8z7w',
      stages: [
        { id: '1', name: 'Build', status: 'success', duration: 90 },
        { id: '2', name: 'Unit Tests', status: 'running' },
        { id: '3', name: 'Integration Tests', status: 'pending' }
      ]
    }
  ];

  const issueTrackers: IssueTracker[] = [
    {
      id: '1',
      name: 'BARQ Project Board',
      type: 'jira',
      url: 'https://company.atlassian.net/browse/BARQ',
      status: 'connected',
      openIssues: 45,
      closedIssues: 123,
      lastSync: '2024-01-15T10:30:00Z'
    },
    {
      id: '2',
      name: 'GitHub Issues',
      type: 'github',
      url: 'https://github.com/company/barq/issues',
      status: 'connected',
      openIssues: 20,
      closedIssues: 87,
      lastSync: '2024-01-15T10:25:00Z'
    }
  ];

  const webhooks: WebhookConfig[] = [
    {
      id: '1',
      name: 'Slack Notifications',
      url: 'https://hooks.slack.com/services/...',
      events: ['push', 'pull_request', 'deployment'],
      status: 'active',
      lastTriggered: '2024-01-15T10:30:00Z',
      deliveries: 1247,
      failures: 3
    },
    {
      id: '2',
      name: 'Discord Alerts',
      url: 'https://discord.com/api/webhooks/...',
      events: ['build_failed', 'deployment_success'],
      status: 'active',
      lastTriggered: '2024-01-15T09:45:00Z',
      deliveries: 892,
      failures: 12
    }
  ];

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'connected':
      case 'success':
      case 'active':
        return <CheckCircle className="h-4 w-4 text-green-500" />;
      case 'running':
      case 'syncing':
        return <Clock className="h-4 w-4 text-blue-500" />;
      case 'failed':
      case 'disconnected':
      case 'inactive':
        return <XCircle className="h-4 w-4 text-red-500" />;
      case 'pending':
        return <Clock className="h-4 w-4 text-yellow-500" />;
      default:
        return <AlertTriangle className="h-4 w-4 text-gray-500" />;
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'connected':
      case 'success':
      case 'active':
        return 'bg-green-100 text-green-800 border-green-200';
      case 'running':
      case 'syncing':
        return 'bg-blue-100 text-blue-800 border-blue-200';
      case 'failed':
      case 'disconnected':
      case 'inactive':
        return 'bg-red-100 text-red-800 border-red-200';
      case 'pending':
        return 'bg-yellow-100 text-yellow-800 border-yellow-200';
      default:
        return 'bg-gray-100 text-gray-800 border-gray-200';
    }
  };

  const formatDuration = (seconds: number) => {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    return `${minutes}m ${remainingSeconds}s`;
  };

  const handleSyncRepository = (repoId: string) => {
    alert(`Syncing repository ${repoId}...`);
  };

  const handleTriggerPipeline = (pipelineId: string) => {
    alert(`Triggering pipeline ${pipelineId}...`);
  };

  const handleSyncIssues = (trackerId: string) => {
    alert(`Syncing issues from tracker ${trackerId}...`);
  };

  const handleTestWebhook = (webhookId: string) => {
    alert(`Testing webhook ${webhookId}...`);
  };

  return (
    <div className="container mx-auto p-6 space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">DevTools Integration</h1>
          <p className="text-muted-foreground">
            Manage Git repositories, CI/CD pipelines, issue tracking, and webhooks
          </p>
        </div>
        <Button>
          <Settings className="h-4 w-4 mr-2" />
          Integration Settings
        </Button>
      </div>

      <Tabs value={activeTab} onValueChange={setActiveTab} className="space-y-4">
        <TabsList>
          <TabsTrigger value="git">Git Repositories</TabsTrigger>
          <TabsTrigger value="cicd">CI/CD Pipelines</TabsTrigger>
          <TabsTrigger value="issues">Issue Tracking</TabsTrigger>
          <TabsTrigger value="webhooks">Webhooks</TabsTrigger>
        </TabsList>

        <TabsContent value="git" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">Git Repositories</h3>
            <div className="flex items-center space-x-2">
              <Input
                placeholder="Search repositories..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="max-w-sm"
              />
              <Button variant="outline">
                <GitBranch className="h-4 w-4 mr-2" />
                Add Repository
              </Button>
            </div>
          </div>

          <div className="grid gap-4 md:grid-cols-1 lg:grid-cols-2">
            {repositories.map((repo) => (
              <Card key={repo.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      <GitBranch className="h-5 w-5" />
                      <CardTitle className="text-lg">{repo.name}</CardTitle>
                    </div>
                    <Badge className={getStatusColor(repo.status)}>
                      {getStatusIcon(repo.status)}
                      <span className="ml-1 capitalize">{repo.status}</span>
                    </Badge>
                  </div>
                  <CardDescription>{repo.url}</CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <Label className="text-sm font-medium">Current Branch</Label>
                      <p className="text-sm text-muted-foreground">{repo.branch}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Last Commit</Label>
                      <p className="text-sm text-muted-foreground">{repo.lastCommit.hash}</p>
                    </div>
                  </div>

                  <div>
                    <Label className="text-sm font-medium">Latest Commit</Label>
                    <div className="flex items-center space-x-2 mt-1">
                      <GitCommit className="h-4 w-4" />
                      <span className="text-sm">{repo.lastCommit.message}</span>
                    </div>
                    <p className="text-xs text-muted-foreground mt-1">
                      by {repo.lastCommit.author} • {new Date(repo.lastCommit.timestamp).toLocaleString()}
                    </p>
                  </div>

                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-4">
                      <div className="flex items-center space-x-1">
                        <GitPullRequest className="h-4 w-4" />
                        <span className="text-sm">{repo.pullRequests} PRs</span>
                      </div>
                      <div className="flex items-center space-x-1">
                        <Bug className="h-4 w-4" />
                        <span className="text-sm">{repo.issues} Issues</span>
                      </div>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleSyncRepository(repo.id)}
                      >
                        <RefreshCw className="h-4 w-4 mr-2" />
                        Sync
                      </Button>
                      <Button variant="outline" size="sm">
                        <ExternalLink className="h-4 w-4 mr-2" />
                        View
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="cicd" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">CI/CD Pipelines</h3>
            <Button variant="outline">
              <Play className="h-4 w-4 mr-2" />
              Create Pipeline
            </Button>
          </div>

          <div className="space-y-4">
            {pipelines.map((pipeline) => (
              <Card key={pipeline.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      <Code className="h-5 w-5" />
                      <CardTitle className="text-lg">{pipeline.name}</CardTitle>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Badge className={getStatusColor(pipeline.status)}>
                        {getStatusIcon(pipeline.status)}
                        <span className="ml-1 capitalize">{pipeline.status}</span>
                      </Badge>
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleTriggerPipeline(pipeline.id)}
                      >
                        <Play className="h-4 w-4 mr-2" />
                        Run
                      </Button>
                    </div>
                  </div>
                  <CardDescription>
                    {pipeline.repository} • {pipeline.branch} • {pipeline.commit}
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="grid grid-cols-3 gap-4">
                    <div>
                      <Label className="text-sm font-medium">Last Run</Label>
                      <p className="text-sm text-muted-foreground">
                        {new Date(pipeline.lastRun).toLocaleString()}
                      </p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Duration</Label>
                      <p className="text-sm text-muted-foreground">
                        {pipeline.duration > 0 ? formatDuration(pipeline.duration) : 'Running...'}
                      </p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Branch</Label>
                      <p className="text-sm text-muted-foreground">{pipeline.branch}</p>
                    </div>
                  </div>

                  <div>
                    <Label className="text-sm font-medium mb-2 block">Pipeline Stages</Label>
                    <div className="space-y-2">
                      {pipeline.stages.map((stage) => (
                        <div key={stage.id} className="flex items-center space-x-3">
                          <div className="flex items-center space-x-2 flex-1">
                            {getStatusIcon(stage.status)}
                            <span className="text-sm font-medium">{stage.name}</span>
                            {stage.duration && (
                              <span className="text-xs text-muted-foreground">
                                ({formatDuration(stage.duration)})
                              </span>
                            )}
                          </div>
                          {stage.status === 'running' && (
                            <Progress value={50} className="w-20 h-2" />
                          )}
                        </div>
                      ))}
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="issues" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">Issue Tracking Systems</h3>
            <Button variant="outline">
              <Database className="h-4 w-4 mr-2" />
              Add Integration
            </Button>
          </div>

          <div className="grid gap-4 md:grid-cols-1 lg:grid-cols-2">
            {issueTrackers.map((tracker) => (
              <Card key={tracker.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      <Bug className="h-5 w-5" />
                      <CardTitle className="text-lg">{tracker.name}</CardTitle>
                    </div>
                    <Badge className={getStatusColor(tracker.status)}>
                      {getStatusIcon(tracker.status)}
                      <span className="ml-1 capitalize">{tracker.status}</span>
                    </Badge>
                  </div>
                  <CardDescription>
                    {tracker.type.toUpperCase()} • {tracker.url}
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <Label className="text-sm font-medium">Open Issues</Label>
                      <p className="text-2xl font-bold text-red-600">{tracker.openIssues}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Closed Issues</Label>
                      <p className="text-2xl font-bold text-green-600">{tracker.closedIssues}</p>
                    </div>
                  </div>

                  <div>
                    <Label className="text-sm font-medium">Last Sync</Label>
                    <p className="text-sm text-muted-foreground">
                      {new Date(tracker.lastSync).toLocaleString()}
                    </p>
                  </div>

                  <div className="flex items-center justify-between">
                    <div className="text-sm text-muted-foreground">
                      Total: {tracker.openIssues + tracker.closedIssues} issues
                    </div>
                    <div className="flex items-center space-x-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleSyncIssues(tracker.id)}
                      >
                        <RefreshCw className="h-4 w-4 mr-2" />
                        Sync
                      </Button>
                      <Button variant="outline" size="sm">
                        <ExternalLink className="h-4 w-4 mr-2" />
                        View
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="webhooks" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">Webhook Configuration</h3>
            <Button variant="outline">
              <Webhook className="h-4 w-4 mr-2" />
              Add Webhook
            </Button>
          </div>

          <div className="space-y-4">
            {webhooks.map((webhook) => (
              <Card key={webhook.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      <Webhook className="h-5 w-5" />
                      <CardTitle className="text-lg">{webhook.name}</CardTitle>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Badge className={getStatusColor(webhook.status)}>
                        {getStatusIcon(webhook.status)}
                        <span className="ml-1 capitalize">{webhook.status}</span>
                      </Badge>
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleTestWebhook(webhook.id)}
                      >
                        <Zap className="h-4 w-4 mr-2" />
                        Test
                      </Button>
                    </div>
                  </div>
                  <CardDescription>{webhook.url}</CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div>
                    <Label className="text-sm font-medium">Events</Label>
                    <div className="flex flex-wrap gap-1 mt-1">
                      {webhook.events.map((event) => (
                        <Badge key={event} variant="secondary" className="text-xs">
                          {event}
                        </Badge>
                      ))}
                    </div>
                  </div>

                  <div className="grid grid-cols-3 gap-4">
                    <div>
                      <Label className="text-sm font-medium">Deliveries</Label>
                      <p className="text-lg font-semibold">{webhook.deliveries.toLocaleString()}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Failures</Label>
                      <p className="text-lg font-semibold text-red-600">{webhook.failures}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Success Rate</Label>
                      <p className="text-lg font-semibold text-green-600">
                        {(((webhook.deliveries - webhook.failures) / webhook.deliveries) * 100).toFixed(1)}%
                      </p>
                    </div>
                  </div>

                  {webhook.lastTriggered && (
                    <div>
                      <Label className="text-sm font-medium">Last Triggered</Label>
                      <p className="text-sm text-muted-foreground">
                        {new Date(webhook.lastTriggered).toLocaleString()}
                      </p>
                    </div>
                  )}

                  <div className="flex items-center justify-between">
                    <div className="text-sm text-muted-foreground">
                      {webhook.failures > 0 && (
                        <span className="text-red-600">
                          {webhook.failures} recent failures
                        </span>
                      )}
                    </div>
                    <div className="flex items-center space-x-2">
                      <Button variant="outline" size="sm">
                        <Settings className="h-4 w-4 mr-2" />
                        Configure
                      </Button>
                      <Button variant="outline" size="sm">
                        <MoreHorizontal className="h-4 w-4" />
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>
      </Tabs>
    </div>
  );
}
