import { useState } from 'react';
import { Settings, Users, FileType, Code, FileText, Presentation, TestTube, Shield, Database } from 'lucide-react';
import { CodeGenerationConfig } from '../../components/admin/CodeGenerationConfig';
import { BRDTemplateConfig } from '../../components/admin/BRDTemplateConfig';
import { ProposalConfig } from '../../components/admin/ProposalConfig';
import { PresentationConfig } from '../../components/admin/PresentationConfig';
import { TestingSecurityConfig } from '../../components/admin/TestingSecurityConfig';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../components/ui/tabs';
import { Badge } from '../../components/ui/badge';
import { Button } from '../../components/ui/button';

interface AdminOverviewProps {}

function AdminOverview({}: AdminOverviewProps) {
  return (
    <div className="space-y-6">
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Active Configurations</CardTitle>
            <Settings className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">12</div>
            <p className="text-xs text-muted-foreground">+2 from last month</p>
          </CardContent>
        </Card>
        
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Code Generation Templates</CardTitle>
            <Code className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">8</div>
            <p className="text-xs text-muted-foreground">+1 from last week</p>
          </CardContent>
        </Card>
        
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">BRD Templates</CardTitle>
            <FileText className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">5</div>
            <p className="text-xs text-muted-foreground">No change</p>
          </CardContent>
        </Card>
        
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Active Users</CardTitle>
            <Users className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">247</div>
            <p className="text-xs text-muted-foreground">+12 from yesterday</p>
          </CardContent>
        </Card>
      </div>
      
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <Card>
          <CardHeader>
            <CardTitle>Recent Configuration Changes</CardTitle>
            <CardDescription>Latest updates to system configurations</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <div className="flex items-center space-x-4">
                <div className="w-2 h-2 bg-green-500 rounded-full"></div>
                <div className="flex-1">
                  <p className="text-sm font-medium">Code Generation - .NET 8 Template Updated</p>
                  <p className="text-xs text-muted-foreground">2 hours ago</p>
                </div>
                <Badge variant="secondary">Active</Badge>
              </div>
              <div className="flex items-center space-x-4">
                <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
                <div className="flex-1">
                  <p className="text-sm font-medium">BRD Template - Sprint Planning Enhanced</p>
                  <p className="text-xs text-muted-foreground">1 day ago</p>
                </div>
                <Badge variant="secondary">Active</Badge>
              </div>
              <div className="flex items-center space-x-4">
                <div className="w-2 h-2 bg-orange-500 rounded-full"></div>
                <div className="flex-1">
                  <p className="text-sm font-medium">Security Configuration Updated</p>
                  <p className="text-xs text-muted-foreground">3 days ago</p>
                </div>
                <Badge variant="outline">Pending</Badge>
              </div>
            </div>
          </CardContent>
        </Card>
        
        <Card>
          <CardHeader>
            <CardTitle>System Health</CardTitle>
            <CardDescription>Current status of configuration systems</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <div className="flex items-center justify-between">
                <div className="flex items-center space-x-2">
                  <Database className="h-4 w-4" />
                  <span className="text-sm">Database Connection</span>
                </div>
                <Badge className="bg-green-100 text-green-800">Healthy</Badge>
              </div>
              <div className="flex items-center justify-between">
                <div className="flex items-center space-x-2">
                  <Shield className="h-4 w-4" />
                  <span className="text-sm">Security Services</span>
                </div>
                <Badge className="bg-green-100 text-green-800">Healthy</Badge>
              </div>
              <div className="flex items-center justify-between">
                <div className="flex items-center space-x-2">
                  <Code className="h-4 w-4" />
                  <span className="text-sm">AI Services</span>
                </div>
                <Badge className="bg-yellow-100 text-yellow-800">Warning</Badge>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}

export function AdminDashboardPage() {
  const [activeTab, setActiveTab] = useState('overview');
  
  return (
    <div className="space-y-6 p-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Admin Configuration Dashboard</h1>
          <p className="text-muted-foreground">Manage system configurations and user experience settings</p>
        </div>
        <Button>
          <Settings className="mr-2 h-4 w-4" />
          Global Settings
        </Button>
      </div>
      
      <Tabs value={activeTab} onValueChange={setActiveTab} className="space-y-4">
        <TabsList className="grid w-full grid-cols-6">
          <TabsTrigger value="overview" className="flex items-center space-x-2">
            <Settings className="h-4 w-4" />
            <span>Overview</span>
          </TabsTrigger>
          <TabsTrigger value="code-generation" className="flex items-center space-x-2">
            <Code className="h-4 w-4" />
            <span>Code Generation</span>
          </TabsTrigger>
          <TabsTrigger value="brd-templates" className="flex items-center space-x-2">
            <FileText className="h-4 w-4" />
            <span>BRD Templates</span>
          </TabsTrigger>
          <TabsTrigger value="proposals" className="flex items-center space-x-2">
            <FileType className="h-4 w-4" />
            <span>Proposals</span>
          </TabsTrigger>
          <TabsTrigger value="presentations" className="flex items-center space-x-2">
            <Presentation className="h-4 w-4" />
            <span>Presentations</span>
          </TabsTrigger>
          <TabsTrigger value="testing" className="flex items-center space-x-2">
            <TestTube className="h-4 w-4" />
            <span>Testing &amp; Security</span>
          </TabsTrigger>
        </TabsList>
        
        <TabsContent value="overview" className="space-y-4">
          <AdminOverview />
        </TabsContent>
        
        <TabsContent value="code-generation" className="space-y-4">
          <CodeGenerationConfig />
        </TabsContent>
        
        <TabsContent value="brd-templates" className="space-y-4">
          <BRDTemplateConfig />
        </TabsContent>
        
        <TabsContent value="proposals" className="space-y-4">
          <ProposalConfig />
        </TabsContent>
        
        <TabsContent value="presentations" className="space-y-4">
          <PresentationConfig />
        </TabsContent>
        
        <TabsContent value="testing" className="space-y-4">
          <TestingSecurityConfig />
        </TabsContent>
      </Tabs>
    </div>
  );
}
