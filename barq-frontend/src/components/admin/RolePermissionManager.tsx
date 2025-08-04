import { useState, useEffect } from 'react';
import { Shield, Users, Settings, Lock, Save, UserCheck, AlertCircle } from 'lucide-react';
import { Card, CardContent, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Checkbox } from '../ui/checkbox';
import { Badge } from '../ui/badge';
import { ScrollArea } from '../ui/scroll-area';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../ui/tabs';
import { Input } from '../ui/input';
import { Textarea } from '../ui/textarea';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '../ui/dialog';
import { Label } from '../ui/label';

interface Permission {
  id: string;
  name: string;
  description: string;
  category: string;
  isSystemCritical: boolean;
  requiredForBasicAccess: boolean;
}

interface Role {
  id: string;
  name: string;
  description: string;
  permissions: string[];
  userCount: number;
  isSystemRole: boolean;
  createdAt: Date;
  lastModified: Date;
}

interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  roles: string[];
  isActive: boolean;
  lastLogin?: Date;
}

const permissions: Permission[] = [
  { 
    id: 'admin.config.read', 
    name: 'View Admin Configuration', 
    description: 'Can view admin configuration settings and system parameters', 
    category: 'Admin Configuration',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'admin.config.write', 
    name: 'Modify Admin Configuration', 
    description: 'Can modify admin configuration settings and system parameters', 
    category: 'Admin Configuration',
    isSystemCritical: true,
    requiredForBasicAccess: false
  },
  { 
    id: 'admin.config.technology-stack', 
    name: 'Manage Technology Stack', 
    description: 'Can configure technology stack preferences and constraints', 
    category: 'Admin Configuration',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'admin.config.brd-templates', 
    name: 'Manage BRD Templates', 
    description: 'Can create and modify Business Requirements Document templates', 
    category: 'Admin Configuration',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'admin.config.proposals', 
    name: 'Manage Proposal Templates', 
    description: 'Can create and modify proposal generation templates', 
    category: 'Admin Configuration',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'admin.config.presentations', 
    name: 'Manage Presentation Templates', 
    description: 'Can create and modify presentation generation templates', 
    category: 'Admin Configuration',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },

  { 
    id: 'workflow.create', 
    name: 'Create Workflows', 
    description: 'Can create new workflow definitions and templates', 
    category: 'Workflow Management',
    isSystemCritical: false,
    requiredForBasicAccess: true
  },
  { 
    id: 'workflow.execute', 
    name: 'Execute Workflows', 
    description: 'Can start and run workflow instances', 
    category: 'Workflow Management',
    isSystemCritical: false,
    requiredForBasicAccess: true
  },
  { 
    id: 'workflow.approve', 
    name: 'Approve Workflows', 
    description: 'Can approve workflow executions and results', 
    category: 'Workflow Management',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'workflow.reject', 
    name: 'Reject Workflows', 
    description: 'Can reject workflow executions and provide feedback', 
    category: 'Workflow Management',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'workflow.monitor', 
    name: 'Monitor Workflows', 
    description: 'Can view workflow status and execution details', 
    category: 'Workflow Management',
    isSystemCritical: false,
    requiredForBasicAccess: true
  },
  { 
    id: 'workflow.delete', 
    name: 'Delete Workflows', 
    description: 'Can delete workflow definitions and instances', 
    category: 'Workflow Management',
    isSystemCritical: true,
    requiredForBasicAccess: false
  },

  { 
    id: 'ai.request.create', 
    name: 'Create AI Requests', 
    description: 'Can create AI generation requests for code, BRDs, proposals, etc.', 
    category: 'AI Requests',
    isSystemCritical: false,
    requiredForBasicAccess: true
  },
  { 
    id: 'ai.request.approve', 
    name: 'Approve AI Requests', 
    description: 'Can approve AI generation results and deliverables', 
    category: 'AI Requests',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'ai.request.reject', 
    name: 'Reject AI Requests', 
    description: 'Can reject AI generation results and request revisions', 
    category: 'AI Requests',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'ai.request.monitor', 
    name: 'Monitor AI Requests', 
    description: 'Can view AI request status and processing details', 
    category: 'AI Requests',
    isSystemCritical: false,
    requiredForBasicAccess: true
  },
  { 
    id: 'ai.agent.select', 
    name: 'Select AI Agents', 
    description: 'Can choose and configure AI agents for specific tasks', 
    category: 'AI Requests',
    isSystemCritical: false,
    requiredForBasicAccess: true
  },
  { 
    id: 'ai.agent.configure', 
    name: 'Configure AI Agents', 
    description: 'Can modify AI agent settings and parameters', 
    category: 'AI Requests',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },

  { 
    id: 'document.upload', 
    name: 'Upload Documents', 
    description: 'Can upload files and documents to the system', 
    category: 'Document Management',
    isSystemCritical: false,
    requiredForBasicAccess: true
  },
  { 
    id: 'document.download', 
    name: 'Download Documents', 
    description: 'Can download files and documents from the system', 
    category: 'Document Management',
    isSystemCritical: false,
    requiredForBasicAccess: true
  },
  { 
    id: 'document.preview', 
    name: 'Preview Documents', 
    description: 'Can preview document contents without downloading', 
    category: 'Document Management',
    isSystemCritical: false,
    requiredForBasicAccess: true
  },
  { 
    id: 'document.delete', 
    name: 'Delete Documents', 
    description: 'Can delete documents and files from the system', 
    category: 'Document Management',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'document.share', 
    name: 'Share Documents', 
    description: 'Can share documents with other users and external parties', 
    category: 'Document Management',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },

  { 
    id: 'repository.connect', 
    name: 'Connect Repositories', 
    description: 'Can connect to external code repositories (GitHub, GitLab, etc.)', 
    category: 'Repository Integration',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'repository.analyze', 
    name: 'Analyze Repositories', 
    description: 'Can perform code analysis and quality assessments on repositories', 
    category: 'Repository Integration',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'repository.browse', 
    name: 'Browse Repository Contents', 
    description: 'Can browse and view repository files and structure', 
    category: 'Repository Integration',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'repository.clone', 
    name: 'Clone Repositories', 
    description: 'Can clone repositories for local development and analysis', 
    category: 'Repository Integration',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },

  { 
    id: 'user.create', 
    name: 'Create Users', 
    description: 'Can create new user accounts and profiles', 
    category: 'User Management',
    isSystemCritical: true,
    requiredForBasicAccess: false
  },
  { 
    id: 'user.read', 
    name: 'View Users', 
    description: 'Can view user profiles and account information', 
    category: 'User Management',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'user.update', 
    name: 'Update Users', 
    description: 'Can modify user profiles and account settings', 
    category: 'User Management',
    isSystemCritical: true,
    requiredForBasicAccess: false
  },
  { 
    id: 'user.delete', 
    name: 'Delete Users', 
    description: 'Can delete user accounts and associated data', 
    category: 'User Management',
    isSystemCritical: true,
    requiredForBasicAccess: false
  },
  { 
    id: 'user.roles.assign', 
    name: 'Assign User Roles', 
    description: 'Can assign and modify user roles and permissions', 
    category: 'User Management',
    isSystemCritical: true,
    requiredForBasicAccess: false
  },

  { 
    id: 'system.monitor', 
    name: 'Monitor System', 
    description: 'Can view system health, performance metrics, and logs', 
    category: 'System Administration',
    isSystemCritical: false,
    requiredForBasicAccess: false
  },
  { 
    id: 'system.backup', 
    name: 'System Backup', 
    description: 'Can create and manage system backups', 
    category: 'System Administration',
    isSystemCritical: true,
    requiredForBasicAccess: false
  },
  { 
    id: 'system.restore', 
    name: 'System Restore', 
    description: 'Can restore system from backups', 
    category: 'System Administration',
    isSystemCritical: true,
    requiredForBasicAccess: false
  },
  { 
    id: 'system.maintenance', 
    name: 'System Maintenance', 
    description: 'Can perform system maintenance tasks and updates', 
    category: 'System Administration',
    isSystemCritical: true,
    requiredForBasicAccess: false
  }
];

export function RolePermissionManager() {
  const [roles, setRoles] = useState<Role[]>([]);
  const [users, setUsers] = useState<User[]>([]);
  const [selectedRole, setSelectedRole] = useState<Role | null>(null);
  const [rolePermissions, setRolePermissions] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [activeTab, setActiveTab] = useState<string>('roles');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [isCreateRoleDialogOpen, setIsCreateRoleDialogOpen] = useState<boolean>(false);
  const [newRoleName, setNewRoleName] = useState<string>('');
  const [newRoleDescription, setNewRoleDescription] = useState<string>('');

  useEffect(() => {
    loadRoles();
    loadUsers();
  }, []);

  const loadRoles = () => {
    const mockRoles: Role[] = [
      {
        id: 'role-001',
        name: 'System Administrator',
        description: 'Full system access with all administrative privileges',
        permissions: [
          'admin.config.read', 'admin.config.write', 'admin.config.technology-stack',
          'admin.config.brd-templates', 'admin.config.proposals', 'admin.config.presentations',
          'workflow.create', 'workflow.execute', 'workflow.approve', 'workflow.reject',
          'workflow.monitor', 'workflow.delete', 'ai.request.create', 'ai.request.approve',
          'ai.request.reject', 'ai.request.monitor', 'ai.agent.select', 'ai.agent.configure',
          'document.upload', 'document.download', 'document.preview', 'document.delete',
          'document.share', 'repository.connect', 'repository.analyze', 'repository.browse',
          'repository.clone', 'user.create', 'user.read', 'user.update', 'user.delete',
          'user.roles.assign', 'system.monitor', 'system.backup', 'system.restore',
          'system.maintenance'
        ],
        userCount: 2,
        isSystemRole: true,
        createdAt: new Date('2025-01-01T00:00:00'),
        lastModified: new Date('2025-01-15T10:30:00')
      },
      {
        id: 'role-002',
        name: 'Project Manager',
        description: 'Can manage workflows, approve AI requests, and oversee project deliverables',
        permissions: [
          'workflow.create', 'workflow.execute', 'workflow.approve', 'workflow.reject',
          'workflow.monitor', 'ai.request.create', 'ai.request.approve', 'ai.request.reject',
          'ai.request.monitor', 'ai.agent.select', 'document.upload', 'document.download',
          'document.preview', 'document.share', 'repository.connect', 'repository.analyze',
          'repository.browse', 'user.read'
        ],
        userCount: 5,
        isSystemRole: false,
        createdAt: new Date('2025-01-02T00:00:00'),
        lastModified: new Date('2025-01-10T14:20:00')
      },
      {
        id: 'role-003',
        name: 'Developer',
        description: 'Can create and execute workflows, work with AI requests, and manage code repositories',
        permissions: [
          'workflow.create', 'workflow.execute', 'workflow.monitor', 'ai.request.create',
          'ai.request.monitor', 'ai.agent.select', 'document.upload', 'document.download',
          'document.preview', 'repository.connect', 'repository.analyze', 'repository.browse',
          'repository.clone'
        ],
        userCount: 12,
        isSystemRole: false,
        createdAt: new Date('2025-01-03T00:00:00'),
        lastModified: new Date('2025-01-12T09:15:00')
      },
      {
        id: 'role-004',
        name: 'Business Analyst',
        description: 'Can create BRD and proposal requests, manage documents, and monitor workflows',
        permissions: [
          'workflow.create', 'workflow.execute', 'workflow.monitor', 'ai.request.create',
          'ai.request.monitor', 'ai.agent.select', 'document.upload', 'document.download',
          'document.preview', 'document.share', 'admin.config.brd-templates',
          'admin.config.proposals'
        ],
        userCount: 8,
        isSystemRole: false,
        createdAt: new Date('2025-01-04T00:00:00'),
        lastModified: new Date('2025-01-08T16:45:00')
      },
      {
        id: 'role-005',
        name: 'Viewer',
        description: 'Read-only access to view workflows, documents, and basic system information',
        permissions: [
          'workflow.monitor', 'ai.request.monitor', 'document.download', 'document.preview',
          'repository.browse'
        ],
        userCount: 15,
        isSystemRole: false,
        createdAt: new Date('2025-01-05T00:00:00'),
        lastModified: new Date('2025-01-05T00:00:00')
      }
    ];
    setRoles(mockRoles);
  };

  const loadUsers = () => {
    const mockUsers: User[] = [
      {
        id: 'user-001',
        email: 'admin@barq.com',
        firstName: 'System',
        lastName: 'Administrator',
        roles: ['role-001'],
        isActive: true,
        lastLogin: new Date('2025-01-15T08:30:00')
      },
      {
        id: 'user-002',
        email: 'john.smith@barq.com',
        firstName: 'John',
        lastName: 'Smith',
        roles: ['role-002'],
        isActive: true,
        lastLogin: new Date('2025-01-15T09:15:00')
      },
      {
        id: 'user-003',
        email: 'jane.doe@barq.com',
        firstName: 'Jane',
        lastName: 'Doe',
        roles: ['role-003'],
        isActive: true,
        lastLogin: new Date('2025-01-14T16:20:00')
      },
      {
        id: 'user-004',
        email: 'mike.wilson@barq.com',
        firstName: 'Mike',
        lastName: 'Wilson',
        roles: ['role-004'],
        isActive: true,
        lastLogin: new Date('2025-01-13T11:45:00')
      },
      {
        id: 'user-005',
        email: 'sarah.johnson@barq.com',
        firstName: 'Sarah',
        lastName: 'Johnson',
        roles: ['role-005'],
        isActive: false,
        lastLogin: new Date('2025-01-10T14:30:00')
      }
    ];
    setUsers(mockUsers);
  };

  const handlePermissionChange = (permissionId: string, checked: boolean) => {
    if (checked) {
      setRolePermissions(prev => [...prev, permissionId]);
    } else {
      setRolePermissions(prev => prev.filter(id => id !== permissionId));
    }
  };

  const handleSavePermissions = async () => {
    if (!selectedRole) return;

    setIsLoading(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      setRoles(prev => prev.map(role => 
        role.id === selectedRole.id 
          ? { ...role, permissions: rolePermissions, lastModified: new Date() }
          : role
      ));
      
      setSelectedRole(prev => prev ? {
        ...prev,
        permissions: rolePermissions,
        lastModified: new Date()
      } : null);
      
      console.log('Permissions saved successfully');
    } catch (error) {
      console.error('Failed to save permissions:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleCreateRole = async () => {
    if (!newRoleName.trim()) return;

    setIsLoading(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      const newRole: Role = {
        id: `role-${Date.now()}`,
        name: newRoleName,
        description: newRoleDescription,
        permissions: [],
        userCount: 0,
        isSystemRole: false,
        createdAt: new Date(),
        lastModified: new Date()
      };
      
      setRoles(prev => [...prev, newRole]);
      setNewRoleName('');
      setNewRoleDescription('');
      setIsCreateRoleDialogOpen(false);
      
      console.log('Role created successfully');
    } catch (error) {
      console.error('Failed to create role:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleDeleteRole = async (roleId: string) => {
    const role = roles.find(r => r.id === roleId);
    if (!role || role.isSystemRole || role.userCount > 0) return;

    setIsLoading(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      setRoles(prev => prev.filter(r => r.id !== roleId));
      
      if (selectedRole?.id === roleId) {
        setSelectedRole(null);
        setRolePermissions([]);
      }
      
      console.log('Role deleted successfully');
    } catch (error) {
      console.error('Failed to delete role:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const groupedPermissions = permissions.reduce((acc, permission) => {
    if (!acc[permission.category]) {
      acc[permission.category] = [];
    }
    acc[permission.category].push(permission);
    return acc;
  }, {} as Record<string, Permission[]>);

  const filteredRoles = roles.filter(role =>
    role.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    role.description.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const filteredUsers = users.filter(user =>
    user.email.toLowerCase().includes(searchTerm.toLowerCase()) ||
    `${user.firstName} ${user.lastName}`.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const getRoleNames = (roleIds: string[]): string => {
    return roleIds.map(roleId => {
      const role = roles.find(r => r.id === roleId);
      return role ? role.name : 'Unknown Role';
    }).join(', ');
  };

  return (
    <div className="space-y-6">
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Shield className="h-5 w-5" />
            Role & Permission Management
          </CardTitle>
        </CardHeader>
        <CardContent>
          <p className="text-sm text-gray-600">
            Manage user roles and granular permissions to control access to system features and functionality.
          </p>
        </CardContent>
      </Card>

      <Tabs value={activeTab} onValueChange={setActiveTab}>
        <TabsList className="grid w-full grid-cols-2">
          <TabsTrigger value="roles">Roles & Permissions</TabsTrigger>
          <TabsTrigger value="users">User Role Assignment</TabsTrigger>
        </TabsList>

        <TabsContent value="roles" className="space-y-6">
          <div className="flex items-center justify-between">
            <Input
              placeholder="Search roles..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="max-w-sm"
            />
            <Dialog open={isCreateRoleDialogOpen} onOpenChange={setIsCreateRoleDialogOpen}>
              <DialogTrigger asChild>
                <Button>
                  <Users className="h-4 w-4 mr-2" />
                  Create Role
                </Button>
              </DialogTrigger>
              <DialogContent>
                <DialogHeader>
                  <DialogTitle>Create New Role</DialogTitle>
                </DialogHeader>
                <div className="space-y-4">
                  <div>
                    <Label htmlFor="roleName">Role Name</Label>
                    <Input
                      id="roleName"
                      value={newRoleName}
                      onChange={(e) => setNewRoleName(e.target.value)}
                      placeholder="Enter role name"
                    />
                  </div>
                  <div>
                    <Label htmlFor="roleDescription">Description</Label>
                    <Textarea
                      id="roleDescription"
                      value={newRoleDescription}
                      onChange={(e) => setNewRoleDescription(e.target.value)}
                      placeholder="Enter role description"
                      rows={3}
                    />
                  </div>
                  <div className="flex justify-end gap-2">
                    <Button variant="outline" onClick={() => setIsCreateRoleDialogOpen(false)}>
                      Cancel
                    </Button>
                    <Button onClick={handleCreateRole} disabled={!newRoleName.trim() || isLoading}>
                      Create Role
                    </Button>
                  </div>
                </div>
              </DialogContent>
            </Dialog>
          </div>

          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2">
                  <Users className="h-5 w-5" />
                  Roles ({filteredRoles.length})
                </CardTitle>
              </CardHeader>
              <CardContent>
                <ScrollArea className="h-96">
                  <div className="space-y-3">
                    {filteredRoles.map((role) => (
                      <div
                        key={role.id}
                        className={`border rounded-lg p-4 cursor-pointer transition-colors ${
                          selectedRole?.id === role.id 
                            ? 'ring-2 ring-blue-500 bg-blue-50' 
                            : 'hover:bg-gray-50'
                        }`}
                        onClick={() => {
                          setSelectedRole(role);
                          setRolePermissions(role.permissions);
                        }}
                      >
                        <div className="flex items-start justify-between mb-2">
                          <div className="flex-1 min-w-0">
                            <div className="flex items-center gap-2">
                              <h3 className="font-medium truncate">{role.name}</h3>
                              {role.isSystemRole && (
                                <Badge variant="secondary" className="text-xs">
                                  <Lock className="h-3 w-3 mr-1" />
                                  System
                                </Badge>
                              )}
                            </div>
                            <p className="text-sm text-gray-600 mt-1">{role.description}</p>
                          </div>
                        </div>
                        
                        <div className="flex items-center justify-between text-xs text-gray-500">
                          <div className="flex items-center gap-4">
                            <Badge variant="outline" className="text-xs">
                              <UserCheck className="h-3 w-3 mr-1" />
                              {role.userCount} users
                            </Badge>
                            <span>{role.permissions.length} permissions</span>
                          </div>
                          {!role.isSystemRole && role.userCount === 0 && (
                            <Button
                              size="sm"
                              variant="ghost"
                              onClick={(e) => {
                                e.stopPropagation();
                                handleDeleteRole(role.id);
                              }}
                              className="text-red-600 hover:text-red-700 h-6 w-6 p-0"
                            >
                              ×
                            </Button>
                          )}
                        </div>
                        
                        <div className="text-xs text-gray-400 mt-2">
                          Last modified: {role.lastModified.toLocaleDateString()}
                        </div>
                      </div>
                    ))}
                  </div>
                </ScrollArea>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2">
                  <Settings className="h-5 w-5" />
                  Permissions
                </CardTitle>
              </CardHeader>
              <CardContent>
                {selectedRole ? (
                  <div className="space-y-4">
                    <div className="flex items-center justify-between">
                      <div>
                        <h3 className="font-medium">{selectedRole.name}</h3>
                        <p className="text-sm text-gray-600">{selectedRole.description}</p>
                      </div>
                      {selectedRole.isSystemRole && (
                        <Badge variant="secondary">
                          <Lock className="h-3 w-3 mr-1" />
                          System Role
                        </Badge>
                      )}
                    </div>

                    <ScrollArea className="h-80">
                      <div className="space-y-4">
                        {Object.entries(groupedPermissions).map(([category, categoryPermissions]) => (
                          <div key={category}>
                            <h4 className="font-medium mb-3 text-sm text-gray-900">{category}</h4>
                            <div className="space-y-3 ml-4">
                              {categoryPermissions.map((permission) => (
                                <div key={permission.id} className="flex items-start space-x-3">
                                  <Checkbox
                                    checked={rolePermissions.includes(permission.id)}
                                    onCheckedChange={(checked) => handlePermissionChange(permission.id, !!checked)}
                                    disabled={selectedRole.isSystemRole}
                                    className="mt-1"
                                  />
                                  <div className="flex-1 min-w-0">
                                    <div className="flex items-center gap-2">
                                      <label className="text-sm font-medium cursor-pointer">
                                        {permission.name}
                                      </label>
                                      {permission.isSystemCritical && (
                                        <Badge variant="destructive" className="text-xs">
                                          <AlertCircle className="h-3 w-3 mr-1" />
                                          Critical
                                        </Badge>
                                      )}
                                      {permission.requiredForBasicAccess && (
                                        <Badge variant="secondary" className="text-xs">
                                          Basic
                                        </Badge>
                                      )}
                                    </div>
                                    <p className="text-xs text-gray-600 mt-1">{permission.description}</p>
                                  </div>
                                </div>
                              ))}
                            </div>
                          </div>
                        ))}
                      </div>
                    </ScrollArea>

                    {!selectedRole.isSystemRole && (
                      <Button 
                        onClick={handleSavePermissions} 
                        className="w-full"
                        disabled={isLoading}
                      >
                        <Save className="h-4 w-4 mr-2" />
                        {isLoading ? 'Saving...' : 'Save Permissions'}
                      </Button>
                    )}
                  </div>
                ) : (
                  <div className="text-center text-gray-500 py-12">
                    <Shield className="h-12 w-12 mx-auto mb-4 text-gray-300" />
                    <h3 className="text-lg font-medium mb-2">No Role Selected</h3>
                    <p className="text-sm">Select a role from the list to manage its permissions.</p>
                  </div>
                )}
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        <TabsContent value="users" className="space-y-6">
          <div className="flex items-center justify-between">
            <Input
              placeholder="Search users..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="max-w-sm"
            />
          </div>

          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Users className="h-5 w-5" />
                Users & Role Assignments ({filteredUsers.length})
              </CardTitle>
            </CardHeader>
            <CardContent>
              <ScrollArea className="h-96">
                <div className="space-y-3">
                  {filteredUsers.map((user) => (
                    <div key={user.id} className="border rounded-lg p-4">
                      <div className="flex items-start justify-between">
                        <div className="flex-1 min-w-0">
                          <div className="flex items-center gap-2 mb-1">
                            <h3 className="font-medium">{user.firstName} {user.lastName}</h3>
                            <Badge variant={user.isActive ? "default" : "secondary"}>
                              {user.isActive ? "Active" : "Inactive"}
                            </Badge>
                          </div>
                          <p className="text-sm text-gray-600 mb-2">{user.email}</p>
                          
                          <div className="flex items-center gap-2 mb-2">
                            <span className="text-xs text-gray-500">Roles:</span>
                            <span className="text-sm">{getRoleNames(user.roles)}</span>
                          </div>
                          
                          {user.lastLogin && (
                            <div className="text-xs text-gray-400">
                              Last login: {user.lastLogin.toLocaleString()}
                            </div>
                          )}
                        </div>
                        
                        <Button size="sm" variant="outline">
                          <Settings className="h-4 w-4 mr-1" />
                          Manage Roles
                        </Button>
                      </div>
                    </div>
                  ))}
                </div>
              </ScrollArea>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
