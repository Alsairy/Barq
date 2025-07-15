import { useState, useEffect } from 'react';
import { 
  Users, 
  UserPlus, 
  Search, 
  Edit, 
  Trash2, 
  Mail,
  Download,
  Upload
} from 'lucide-react';
import { Button } from '../../components/ui';
import { Input } from '../../components/ui';
import { Label } from '../../components/ui';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui';
import { Badge } from '../../components/ui';
import { Avatar, AvatarFallback, AvatarImage } from '../../components/ui';
import { LoadingSpinner } from '../../components/ui/loading-spinner';
import { DataTable } from '../../components/ui/data-table';

interface OrganizationUser {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  roles: string[];
  status: 'active' | 'inactive' | 'pending';
  lastLoginAt?: string;
  createdAt: string;
  department?: string;
  jobTitle?: string;
}

interface UserInvitation {
  email: string;
  roles: string[];
  department?: string;
  message?: string;
}

export function OrganizationUsersPage() {
  
  const [users, setUsers] = useState<OrganizationUser[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedRole, setSelectedRole] = useState<string>('all');
  const [selectedStatus, setSelectedStatus] = useState<string>('all');
  const [showInviteModal, setShowInviteModal] = useState(false);
  const [inviteData, setInviteData] = useState<UserInvitation>({
    email: '',
    roles: [],
    department: '',
    message: '',
  });

  useEffect(() => {
    const mockUsers: OrganizationUser[] = [
      {
        id: '1',
        email: 'john.doe@company.com',
        firstName: 'John',
        lastName: 'Doe',
        roles: ['Admin', 'Project Manager'],
        status: 'active',
        lastLoginAt: '2024-01-15T10:30:00Z',
        createdAt: '2024-01-01T00:00:00Z',
        department: 'Engineering',
        jobTitle: 'Senior Developer',
      },
      {
        id: '2',
        email: 'jane.smith@company.com',
        firstName: 'Jane',
        lastName: 'Smith',
        roles: ['Developer'],
        status: 'active',
        lastLoginAt: '2024-01-14T15:45:00Z',
        createdAt: '2024-01-05T00:00:00Z',
        department: 'Engineering',
        jobTitle: 'Frontend Developer',
      },
      {
        id: '3',
        email: 'pending.user@company.com',
        firstName: 'Pending',
        lastName: 'User',
        roles: ['Viewer'],
        status: 'pending',
        createdAt: '2024-01-10T00:00:00Z',
        department: 'Marketing',
        jobTitle: 'Marketing Specialist',
      },
    ];

    setTimeout(() => {
      setUsers(mockUsers);
      setIsLoading(false);
    }, 1000);
  }, []);

  const filteredUsers = users.filter(user => {
    const matchesSearch = 
      user.firstName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.lastName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.email.toLowerCase().includes(searchTerm.toLowerCase());
    
    const matchesRole = selectedRole === 'all' || user.roles.some(role => 
      role.toLowerCase().includes(selectedRole.toLowerCase())
    );
    
    const matchesStatus = selectedStatus === 'all' || user.status === selectedStatus;
    
    return matchesSearch && matchesRole && matchesStatus;
  });

  const handleInviteUser = async () => {
    console.log('Inviting user:', inviteData);
    setShowInviteModal(false);
    setInviteData({ email: '', roles: [], department: '', message: '' });
  };

  const handleDeleteUser = async (userId: string) => {
    console.log('Deleting user:', userId);
  };

  const handleEditUser = (userId: string) => {
    console.log('Editing user:', userId);
  };

  const handleBulkImport = () => {
    console.log('Bulk import users');
  };

  const handleExportUsers = () => {
    console.log('Export users');
  };

  const getStatusBadge = (status: string) => {
    switch (status) {
      case 'active':
        return <Badge variant="default" className="bg-green-100 text-green-800">Active</Badge>;
      case 'inactive':
        return <Badge variant="secondary">Inactive</Badge>;
      case 'pending':
        return <Badge variant="outline" className="border-yellow-500 text-yellow-700">Pending</Badge>;
      default:
        return <Badge variant="secondary">{status}</Badge>;
    }
  };

  const columns = [
    {
      header: 'User',
      accessorKey: 'user',
      cell: ({ row }: any) => {
        const user = row.original;
        return (
          <div className="flex items-center space-x-3">
            <Avatar className="h-8 w-8">
              <AvatarImage src="" alt={`${user.firstName} ${user.lastName}`} />
              <AvatarFallback>
                {user.firstName.charAt(0)}{user.lastName.charAt(0)}
              </AvatarFallback>
            </Avatar>
            <div>
              <div className="font-medium">{user.firstName} {user.lastName}</div>
              <div className="text-sm text-gray-500">{user.email}</div>
            </div>
          </div>
        );
      },
    },
    {
      header: 'Roles',
      accessorKey: 'roles',
      cell: ({ row }: any) => {
        const roles = row.original.roles;
        return (
          <div className="flex flex-wrap gap-1">
            {roles.map((role: string) => (
              <Badge key={role} variant="outline" className="text-xs">
                {role}
              </Badge>
            ))}
          </div>
        );
      },
    },
    {
      header: 'Department',
      accessorKey: 'department',
    },
    {
      header: 'Status',
      accessorKey: 'status',
      cell: ({ row }: any) => getStatusBadge(row.original.status),
    },
    {
      header: 'Last Login',
      accessorKey: 'lastLoginAt',
      cell: ({ row }: any) => {
        const lastLogin = row.original.lastLoginAt;
        return lastLogin 
          ? new Date(lastLogin).toLocaleDateString()
          : 'Never';
      },
    },
    {
      header: 'Actions',
      id: 'actions',
      cell: ({ row }: any) => {
        const user = row.original;
        return (
          <div className="flex items-center space-x-2">
            <Button
              variant="ghost"
              size="sm"
              onClick={() => handleEditUser(user.id)}
            >
              <Edit className="h-4 w-4" />
            </Button>
            <Button
              variant="ghost"
              size="sm"
              onClick={() => handleDeleteUser(user.id)}
              className="text-red-600 hover:text-red-700"
            >
              <Trash2 className="h-4 w-4" />
            </Button>
          </div>
        );
      },
    },
  ];

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <LoadingSpinner className="h-8 w-8" />
      </div>
    );
  }

  return (
    <div className="container mx-auto py-6 space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Organization Users</h1>
          <p className="text-gray-600">Manage team members and their permissions</p>
        </div>
        <div className="flex space-x-2">
          <Button variant="outline" onClick={handleBulkImport}>
            <Upload className="h-4 w-4 mr-2" />
            Import Users
          </Button>
          <Button variant="outline" onClick={handleExportUsers}>
            <Download className="h-4 w-4 mr-2" />
            Export
          </Button>
          <Button onClick={() => setShowInviteModal(true)}>
            <UserPlus className="h-4 w-4 mr-2" />
            Invite User
          </Button>
        </div>
      </div>

      {/* Filters */}
      <Card>
        <CardContent className="pt-6">
          <div className="flex flex-col md:flex-row gap-4">
            <div className="flex-1">
              <Label htmlFor="search">Search Users</Label>
              <div className="relative">
                <Search className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
                <Input
                  id="search"
                  placeholder="Search by name or email..."
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="pl-10"
                />
              </div>
            </div>
            <div>
              <Label htmlFor="role-filter">Role</Label>
              <select
                id="role-filter"
                value={selectedRole}
                onChange={(e) => setSelectedRole(e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="all">All Roles</option>
                <option value="admin">Admin</option>
                <option value="manager">Manager</option>
                <option value="developer">Developer</option>
                <option value="viewer">Viewer</option>
              </select>
            </div>
            <div>
              <Label htmlFor="status-filter">Status</Label>
              <select
                id="status-filter"
                value={selectedStatus}
                onChange={(e) => setSelectedStatus(e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="all">All Status</option>
                <option value="active">Active</option>
                <option value="inactive">Inactive</option>
                <option value="pending">Pending</option>
              </select>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Users Table */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center">
            <Users className="h-5 w-5 mr-2" />
            Team Members ({filteredUsers.length})
          </CardTitle>
          <CardDescription>
            Manage user access and permissions for your organization
          </CardDescription>
        </CardHeader>
        <CardContent>
          <DataTable
            columns={columns}
            data={filteredUsers}
            searchKey="email"
          />
        </CardContent>
      </Card>

      {/* Invite User Modal */}
      {showInviteModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <Card className="w-full max-w-md">
            <CardHeader>
              <CardTitle>Invite New User</CardTitle>
              <CardDescription>
                Send an invitation to join your organization
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div>
                <Label htmlFor="invite-email">Email Address</Label>
                <Input
                  id="invite-email"
                  type="email"
                  placeholder="user@company.com"
                  value={inviteData.email}
                  onChange={(e) => setInviteData(prev => ({ ...prev, email: e.target.value }))}
                />
              </div>
              <div>
                <Label htmlFor="invite-department">Department</Label>
                <Input
                  id="invite-department"
                  placeholder="Engineering"
                  value={inviteData.department}
                  onChange={(e) => setInviteData(prev => ({ ...prev, department: e.target.value }))}
                />
              </div>
              <div>
                <Label htmlFor="invite-message">Welcome Message (Optional)</Label>
                <textarea
                  id="invite-message"
                  placeholder="Welcome to our team!"
                  value={inviteData.message}
                  onChange={(e) => setInviteData(prev => ({ ...prev, message: e.target.value }))}
                  className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                  rows={3}
                />
              </div>
            </CardContent>
            <div className="flex justify-end space-x-2 p-6">
              <Button
                variant="outline"
                onClick={() => setShowInviteModal(false)}
              >
                Cancel
              </Button>
              <Button onClick={handleInviteUser}>
                <Mail className="h-4 w-4 mr-2" />
                Send Invitation
              </Button>
            </div>
          </Card>
        </div>
      )}
    </div>
  );
}
