import { useState } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Label } from '../../components/ui/label';
import { Badge } from '../../components/ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../components/ui/tabs';
import { Progress } from '../../components/ui/progress';
import { 
  Users, 
  MessageSquare, 
  FileText, 
  Calendar, 
  Settings, 
  RefreshCw, 
  CheckCircle, 
  XCircle, 
  Clock, 
  AlertTriangle,
  ExternalLink,
  Upload,
  RefreshCcw,
  Database,
  Zap
} from 'lucide-react';

interface CRMIntegration {
  id: string;
  name: string;
  type: 'salesforce' | 'hubspot' | 'pipedrive' | 'zoho';
  status: 'connected' | 'disconnected' | 'syncing' | 'error';
  lastSync: string;
  contactsCount: number;
  companiesCount: number;
  dealsCount: number;
  syncProgress?: number;
  errorMessage?: string;
}

interface CommunicationPlatform {
  id: string;
  name: string;
  type: 'slack' | 'teams' | 'discord' | 'telegram';
  status: 'connected' | 'disconnected' | 'syncing';
  channelsCount: number;
  messagesCount: number;
  lastActivity: string;
  webhookUrl?: string;
}

interface DocumentSystem {
  id: string;
  name: string;
  type: 'sharepoint' | 'googledrive' | 'dropbox' | 'onedrive';
  status: 'connected' | 'disconnected' | 'syncing';
  documentsCount: number;
  storageUsed: number;
  storageLimit: number;
  lastSync: string;
  syncedFolders: string[];
}

interface CalendarIntegration {
  id: string;
  name: string;
  type: 'outlook' | 'google' | 'apple' | 'caldav';
  status: 'connected' | 'disconnected' | 'syncing';
  calendarsCount: number;
  eventsCount: number;
  upcomingEvents: number;
  lastSync: string;
}

export default function BusinessSystemsPage() {
  const [activeTab, setActiveTab] = useState('crm');
  const [searchTerm, setSearchTerm] = useState('');

  const crmIntegrations: CRMIntegration[] = [
    {
      id: '1',
      name: 'Salesforce Production',
      type: 'salesforce',
      status: 'connected',
      lastSync: '2024-01-15T10:30:00Z',
      contactsCount: 15420,
      companiesCount: 3240,
      dealsCount: 892
    },
    {
      id: '2',
      name: 'HubSpot Marketing',
      type: 'hubspot',
      status: 'syncing',
      lastSync: '2024-01-15T10:25:00Z',
      contactsCount: 8930,
      companiesCount: 1850,
      dealsCount: 445,
      syncProgress: 65
    },
    {
      id: '3',
      name: 'Pipedrive Sales',
      type: 'pipedrive',
      status: 'error',
      lastSync: '2024-01-15T08:15:00Z',
      contactsCount: 5620,
      companiesCount: 1200,
      dealsCount: 234,
      errorMessage: 'API rate limit exceeded'
    }
  ];

  const communicationPlatforms: CommunicationPlatform[] = [
    {
      id: '1',
      name: 'Company Slack',
      type: 'slack',
      status: 'connected',
      channelsCount: 45,
      messagesCount: 12450,
      lastActivity: '2024-01-15T10:45:00Z',
      webhookUrl: 'https://hooks.slack.com/services/...'
    },
    {
      id: '2',
      name: 'Microsoft Teams',
      type: 'teams',
      status: 'connected',
      channelsCount: 28,
      messagesCount: 8920,
      lastActivity: '2024-01-15T10:30:00Z'
    },
    {
      id: '3',
      name: 'Development Discord',
      type: 'discord',
      status: 'disconnected',
      channelsCount: 12,
      messagesCount: 3450,
      lastActivity: '2024-01-14T18:20:00Z'
    }
  ];

  const documentSystems: DocumentSystem[] = [
    {
      id: '1',
      name: 'SharePoint Online',
      type: 'sharepoint',
      status: 'connected',
      documentsCount: 8920,
      storageUsed: 245.6,
      storageLimit: 1000,
      lastSync: '2024-01-15T10:30:00Z',
      syncedFolders: ['Projects', 'Templates', 'Shared Documents']
    },
    {
      id: '2',
      name: 'Google Drive Business',
      type: 'googledrive',
      status: 'syncing',
      documentsCount: 5640,
      storageUsed: 156.8,
      storageLimit: 500,
      lastSync: '2024-01-15T10:20:00Z',
      syncedFolders: ['Team Files', 'Client Documents']
    }
  ];

  const calendarIntegrations: CalendarIntegration[] = [
    {
      id: '1',
      name: 'Outlook 365',
      type: 'outlook',
      status: 'connected',
      calendarsCount: 8,
      eventsCount: 1240,
      upcomingEvents: 15,
      lastSync: '2024-01-15T10:35:00Z'
    },
    {
      id: '2',
      name: 'Google Calendar',
      type: 'google',
      status: 'connected',
      calendarsCount: 5,
      eventsCount: 890,
      upcomingEvents: 8,
      lastSync: '2024-01-15T10:30:00Z'
    }
  ];

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'connected':
        return <CheckCircle className="h-4 w-4 text-green-500" />;
      case 'syncing':
        return <Clock className="h-4 w-4 text-blue-500" />;
      case 'disconnected':
      case 'error':
        return <XCircle className="h-4 w-4 text-red-500" />;
      default:
        return <AlertTriangle className="h-4 w-4 text-gray-500" />;
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'connected':
        return 'bg-green-100 text-green-800 border-green-200';
      case 'syncing':
        return 'bg-blue-100 text-blue-800 border-blue-200';
      case 'disconnected':
      case 'error':
        return 'bg-red-100 text-red-800 border-red-200';
      default:
        return 'bg-gray-100 text-gray-800 border-gray-200';
    }
  };

  const getTypeIcon = (type: string) => {
    switch (type) {
      case 'salesforce':
      case 'hubspot':
      case 'pipedrive':
      case 'zoho':
        return <Users className="h-5 w-5" />;
      case 'slack':
      case 'teams':
      case 'discord':
      case 'telegram':
        return <MessageSquare className="h-5 w-5" />;
      case 'sharepoint':
      case 'googledrive':
      case 'dropbox':
      case 'onedrive':
        return <FileText className="h-5 w-5" />;
      case 'outlook':
      case 'google':
      case 'apple':
      case 'caldav':
        return <Calendar className="h-5 w-5" />;
      default:
        return <Database className="h-5 w-5" />;
    }
  };

  const formatStorageSize = (sizeGB: number) => {
    return `${sizeGB.toFixed(1)} GB`;
  };

  const handleSync = (integrationId: string, type: string) => {
    alert(`Syncing ${type} integration ${integrationId}...`);
  };

  const handleConfigure = (integrationId: string, type: string) => {
    alert(`Configuring ${type} integration ${integrationId}...`);
  };

  const handleTestConnection = (integrationId: string, type: string) => {
    alert(`Testing connection for ${type} integration ${integrationId}...`);
  };

  return (
    <div className="container mx-auto p-6 space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Business Systems Integration</h1>
          <p className="text-muted-foreground">
            Manage CRM, communication, document management, and calendar integrations
          </p>
        </div>
        <Button>
          <Settings className="h-4 w-4 mr-2" />
          Integration Settings
        </Button>
      </div>

      <Tabs value={activeTab} onValueChange={setActiveTab} className="space-y-4">
        <TabsList>
          <TabsTrigger value="crm">CRM Systems</TabsTrigger>
          <TabsTrigger value="communication">Communication</TabsTrigger>
          <TabsTrigger value="documents">Document Management</TabsTrigger>
          <TabsTrigger value="calendar">Calendar Integration</TabsTrigger>
        </TabsList>

        <TabsContent value="crm" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">CRM Integrations</h3>
            <div className="flex items-center space-x-2">
              <Input
                placeholder="Search CRM systems..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="max-w-sm"
              />
              <Button variant="outline">
                <Users className="h-4 w-4 mr-2" />
                Add CRM
              </Button>
            </div>
          </div>

          <div className="grid gap-4 md:grid-cols-1 lg:grid-cols-2">
            {crmIntegrations.map((crm) => (
              <Card key={crm.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      {getTypeIcon(crm.type)}
                      <CardTitle className="text-lg">{crm.name}</CardTitle>
                    </div>
                    <Badge className={getStatusColor(crm.status)}>
                      {getStatusIcon(crm.status)}
                      <span className="ml-1 capitalize">{crm.status}</span>
                    </Badge>
                  </div>
                  <CardDescription>
                    {crm.type.toUpperCase()} • Last sync: {new Date(crm.lastSync).toLocaleString()}
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  {crm.status === 'syncing' && crm.syncProgress && (
                    <div>
                      <div className="flex items-center justify-between mb-2">
                        <Label className="text-sm font-medium">Sync Progress</Label>
                        <span className="text-sm text-muted-foreground">{crm.syncProgress}%</span>
                      </div>
                      <Progress value={crm.syncProgress} className="h-2" />
                    </div>
                  )}

                  {crm.errorMessage && (
                    <div className="bg-red-50 border border-red-200 rounded-md p-3">
                      <div className="flex items-center space-x-2">
                        <AlertTriangle className="h-4 w-4 text-red-500" />
                        <span className="text-sm text-red-700">{crm.errorMessage}</span>
                      </div>
                    </div>
                  )}

                  <div className="grid grid-cols-3 gap-4">
                    <div>
                      <Label className="text-sm font-medium">Contacts</Label>
                      <p className="text-lg font-semibold">{crm.contactsCount.toLocaleString()}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Companies</Label>
                      <p className="text-lg font-semibold">{crm.companiesCount.toLocaleString()}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Deals</Label>
                      <p className="text-lg font-semibold">{crm.dealsCount.toLocaleString()}</p>
                    </div>
                  </div>

                  <div className="flex items-center justify-between">
                    <div className="text-sm text-muted-foreground">
                      Total records: {(crm.contactsCount + crm.companiesCount + crm.dealsCount).toLocaleString()}
                    </div>
                    <div className="flex items-center space-x-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleSync(crm.id, 'CRM')}
                        disabled={crm.status === 'syncing'}
                      >
                        <RefreshCcw className="h-4 w-4 mr-2" />
                        Sync
                      </Button>
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleTestConnection(crm.id, 'CRM')}
                      >
                        <Zap className="h-4 w-4 mr-2" />
                        Test
                      </Button>
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleConfigure(crm.id, 'CRM')}
                      >
                        <Settings className="h-4 w-4 mr-2" />
                        Configure
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="communication" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">Communication Platforms</h3>
            <Button variant="outline">
              <MessageSquare className="h-4 w-4 mr-2" />
              Add Platform
            </Button>
          </div>

          <div className="grid gap-4 md:grid-cols-1 lg:grid-cols-2">
            {communicationPlatforms.map((platform) => (
              <Card key={platform.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      {getTypeIcon(platform.type)}
                      <CardTitle className="text-lg">{platform.name}</CardTitle>
                    </div>
                    <Badge className={getStatusColor(platform.status)}>
                      {getStatusIcon(platform.status)}
                      <span className="ml-1 capitalize">{platform.status}</span>
                    </Badge>
                  </div>
                  <CardDescription>
                    {platform.type.toUpperCase()} • Last activity: {new Date(platform.lastActivity).toLocaleString()}
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <Label className="text-sm font-medium">Channels</Label>
                      <p className="text-lg font-semibold">{platform.channelsCount}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Messages</Label>
                      <p className="text-lg font-semibold">{platform.messagesCount.toLocaleString()}</p>
                    </div>
                  </div>

                  {platform.webhookUrl && (
                    <div>
                      <Label className="text-sm font-medium">Webhook URL</Label>
                      <p className="text-sm text-muted-foreground font-mono bg-gray-50 p-2 rounded">
                        {platform.webhookUrl}
                      </p>
                    </div>
                  )}

                  <div className="flex items-center justify-between">
                    <div className="text-sm text-muted-foreground">
                      {platform.status === 'connected' ? 'Active integration' : 'Integration inactive'}
                    </div>
                    <div className="flex items-center space-x-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleTestConnection(platform.id, 'Communication')}
                      >
                        <Zap className="h-4 w-4 mr-2" />
                        Test
                      </Button>
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleConfigure(platform.id, 'Communication')}
                      >
                        <Settings className="h-4 w-4 mr-2" />
                        Configure
                      </Button>
                      <Button variant="outline" size="sm">
                        <ExternalLink className="h-4 w-4 mr-2" />
                        Open
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="documents" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">Document Management Systems</h3>
            <Button variant="outline">
              <FileText className="h-4 w-4 mr-2" />
              Add Document System
            </Button>
          </div>

          <div className="grid gap-4 md:grid-cols-1 lg:grid-cols-2">
            {documentSystems.map((system) => (
              <Card key={system.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      {getTypeIcon(system.type)}
                      <CardTitle className="text-lg">{system.name}</CardTitle>
                    </div>
                    <Badge className={getStatusColor(system.status)}>
                      {getStatusIcon(system.status)}
                      <span className="ml-1 capitalize">{system.status}</span>
                    </Badge>
                  </div>
                  <CardDescription>
                    {system.type.toUpperCase()} • Last sync: {new Date(system.lastSync).toLocaleString()}
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <Label className="text-sm font-medium">Documents</Label>
                      <p className="text-lg font-semibold">{system.documentsCount.toLocaleString()}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Storage Used</Label>
                      <p className="text-lg font-semibold">
                        {formatStorageSize(system.storageUsed)} / {formatStorageSize(system.storageLimit)}
                      </p>
                    </div>
                  </div>

                  <div>
                    <Label className="text-sm font-medium mb-2 block">Storage Usage</Label>
                    <Progress 
                      value={(system.storageUsed / system.storageLimit) * 100} 
                      className="h-2"
                    />
                    <p className="text-xs text-muted-foreground mt-1">
                      {((system.storageUsed / system.storageLimit) * 100).toFixed(1)}% used
                    </p>
                  </div>

                  <div>
                    <Label className="text-sm font-medium">Synced Folders</Label>
                    <div className="flex flex-wrap gap-1 mt-1">
                      {system.syncedFolders.map((folder) => (
                        <Badge key={folder} variant="secondary" className="text-xs">
                          {folder}
                        </Badge>
                      ))}
                    </div>
                  </div>

                  <div className="flex items-center justify-between">
                    <div className="text-sm text-muted-foreground">
                      {system.syncedFolders.length} folders synced
                    </div>
                    <div className="flex items-center space-x-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleSync(system.id, 'Document')}
                        disabled={system.status === 'syncing'}
                      >
                        <RefreshCw className="h-4 w-4 mr-2" />
                        Sync
                      </Button>
                      <Button variant="outline" size="sm">
                        <Upload className="h-4 w-4 mr-2" />
                        Upload
                      </Button>
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleConfigure(system.id, 'Document')}
                      >
                        <Settings className="h-4 w-4 mr-2" />
                        Configure
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="calendar" className="space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-medium">Calendar Integrations</h3>
            <Button variant="outline">
              <Calendar className="h-4 w-4 mr-2" />
              Add Calendar
            </Button>
          </div>

          <div className="grid gap-4 md:grid-cols-1 lg:grid-cols-2">
            {calendarIntegrations.map((calendar) => (
              <Card key={calendar.id}>
                <CardHeader>
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      {getTypeIcon(calendar.type)}
                      <CardTitle className="text-lg">{calendar.name}</CardTitle>
                    </div>
                    <Badge className={getStatusColor(calendar.status)}>
                      {getStatusIcon(calendar.status)}
                      <span className="ml-1 capitalize">{calendar.status}</span>
                    </Badge>
                  </div>
                  <CardDescription>
                    {calendar.type.toUpperCase()} • Last sync: {new Date(calendar.lastSync).toLocaleString()}
                  </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="grid grid-cols-3 gap-4">
                    <div>
                      <Label className="text-sm font-medium">Calendars</Label>
                      <p className="text-lg font-semibold">{calendar.calendarsCount}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Total Events</Label>
                      <p className="text-lg font-semibold">{calendar.eventsCount.toLocaleString()}</p>
                    </div>
                    <div>
                      <Label className="text-sm font-medium">Upcoming</Label>
                      <p className="text-lg font-semibold text-blue-600">{calendar.upcomingEvents}</p>
                    </div>
                  </div>

                  <div className="flex items-center justify-between">
                    <div className="text-sm text-muted-foreground">
                      {calendar.upcomingEvents} events in next 7 days
                    </div>
                    <div className="flex items-center space-x-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleSync(calendar.id, 'Calendar')}
                        disabled={calendar.status === 'syncing'}
                      >
                        <RefreshCw className="h-4 w-4 mr-2" />
                        Sync
                      </Button>
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleTestConnection(calendar.id, 'Calendar')}
                      >
                        <Zap className="h-4 w-4 mr-2" />
                        Test
                      </Button>
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleConfigure(calendar.id, 'Calendar')}
                      >
                        <Settings className="h-4 w-4 mr-2" />
                        Configure
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
