import { useState, useEffect } from 'react';
import { 
  Search, 
  Filter, 
  CheckCircle, 
  XCircle, 
  Clock, 
  RefreshCw, 
  Download, 
  ThumbsUp, 
  ThumbsDown, 
  Zap, 
  Code, 
  FileText, 
  AlertTriangle,
  MessageSquare,
  Send
} from 'lucide-react';
import { Card, CardContent, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Badge } from '../ui/badge';
import { Progress } from '../ui/progress';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../ui/tabs';
import { ScrollArea } from '../ui/scroll-area';
import { Textarea } from '../ui/textarea';

interface WorkflowStep {
  id: string;
  name: string;
  status: 'pending' | 'running' | 'completed' | 'failed';
  progress: number;
  startTime?: Date;
  endTime?: Date;
  estimatedDuration?: number;
  actualDuration?: number;
  output?: string;
  logs?: string[];
}

interface WorkflowFile {
  id: string;
  name: string;
  type: string;
  size: number;
  url: string;
  isGenerated: boolean;
  content?: string;
}

interface WorkflowExecution {
  id: string;
  name: string;
  type: 'code-generation' | 'brd-generation' | 'proposal-generation' | 'presentation-generation' | 'testing-security';
  status: 'pending' | 'running' | 'completed' | 'failed' | 'approved' | 'rejected' | 'cancelled';
  priority: 'low' | 'medium' | 'high' | 'urgent';
  requestedBy: string;
  assignedTo: string;
  description: string;
  requirements: string[];
  deliverables: string[];
  estimatedCost: number;
  createdAt: Date;
  completedAt?: Date;
  approvedAt?: Date;
  rejectedAt?: Date;
  approvedBy?: string;
  rejectedBy?: string;
  rejectionReason?: string;
  steps: WorkflowStep[];
  files: WorkflowFile[];
}

interface WorkflowComment {
  id: string;
  workflowId: string;
  author: string;
  content: string;
  createdAt: Date;
  type: 'comment' | 'approval' | 'rejection';
}

export function WorkflowProcessor() {
  const [workflows, setWorkflows] = useState<WorkflowExecution[]>([]);
  const [selectedWorkflow, setSelectedWorkflow] = useState<WorkflowExecution | null>(null);
  const [activeTab, setActiveTab] = useState<string>('active');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [comments, setComments] = useState<WorkflowComment[]>([]);
  const [newComment, setNewComment] = useState<string>('');

  useEffect(() => {
    loadWorkflows();
  }, []);

  const loadWorkflows = () => {
    const mockWorkflows: WorkflowExecution[] = [
      {
        id: 'wf-001',
        name: 'E-commerce Platform Development',
        type: 'code-generation',
        status: 'completed',
        priority: 'high',
        requestedBy: 'John Smith',
        assignedTo: 'Devin Code Generator',
        description: 'Generate a complete e-commerce platform with React frontend, .NET backend, and SQL Server database',
        requirements: [
          'React frontend with TypeScript',
          '.NET 8 Web API backend',
          'SQL Server database',
          'Authentication and authorization',
          'Product catalog management',
          'Shopping cart functionality',
          'Order processing system',
          'Admin dashboard'
        ],
        deliverables: [
          'Complete React application',
          '.NET Web API project',
          'Database schema and migrations',
          'Authentication system',
          'API documentation',
          'Unit tests',
          'Deployment scripts'
        ],
        estimatedCost: 1200,
        createdAt: new Date('2025-01-15T10:00:00'),
        completedAt: new Date('2025-01-15T14:30:00'),
        steps: [
          {
            id: 'step-001',
            name: 'Project Setup',
            status: 'completed',
            progress: 100,
            startTime: new Date('2025-01-15T10:00:00'),
            endTime: new Date('2025-01-15T10:15:00'),
            estimatedDuration: 15,
            actualDuration: 15,
            output: 'Project structure created successfully.',
            logs: [
              'Creating project directories...',
              'Initializing React application...',
              'Setting up .NET Web API project...',
              'Configuring database connection...',
              'Project setup completed'
            ]
          },
          {
            id: 'step-002',
            name: 'Database Design',
            status: 'completed',
            progress: 100,
            startTime: new Date('2025-01-15T10:15:00'),
            endTime: new Date('2025-01-15T11:00:00'),
            estimatedDuration: 45,
            actualDuration: 45,
            output: 'Database schema designed and migrations created.',
            logs: [
              'Analyzing requirements...',
              'Designing entity relationships...',
              'Creating database migrations...',
              'Validating schema design...',
              'Database design completed'
            ]
          },
          {
            id: 'step-003',
            name: 'Backend Development',
            status: 'completed',
            progress: 100,
            startTime: new Date('2025-01-15T11:00:00'),
            endTime: new Date('2025-01-15T13:00:00'),
            estimatedDuration: 120,
            actualDuration: 120,
            output: 'Backend API completed with all endpoints.',
            logs: [
              'Implementing authentication controllers...',
              'Creating product management APIs...',
              'Building shopping cart services...',
              'Implementing order processing...',
              'Adding admin functionality...',
              'Backend development completed'
            ]
          },
          {
            id: 'step-004',
            name: 'Frontend Development',
            status: 'completed',
            progress: 100,
            startTime: new Date('2025-01-15T13:00:00'),
            endTime: new Date('2025-01-15T14:15:00'),
            estimatedDuration: 75,
            actualDuration: 75,
            output: 'React frontend completed with responsive design.',
            logs: [
              'Creating component library...',
              'Implementing authentication pages...',
              'Building product catalog...',
              'Developing shopping cart...',
              'Creating admin dashboard...',
              'Frontend development completed'
            ]
          },
          {
            id: 'step-005',
            name: 'Testing & Quality Assurance',
            status: 'completed',
            progress: 100,
            startTime: new Date('2025-01-15T14:15:00'),
            endTime: new Date('2025-01-15T14:30:00'),
            estimatedDuration: 15,
            actualDuration: 15,
            output: 'All tests passed. Code quality verified.',
            logs: [
              'Running unit tests...',
              'Performing integration tests...',
              'Code quality analysis...',
              'Security scan completed...',
              'Quality assurance completed'
            ]
          }
        ],
        files: [
          {
            id: 'file-001',
            name: 'ecommerce-app.tsx',
            type: 'application/typescript',
            size: 45600,
            url: '/api/files/ecommerce-app.tsx',
            isGenerated: true,
            content: `import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { Provider } from 'react-redux';
import { Toaster } from 'react-hot-toast';
import { store } from './store';
import { AuthProvider } from './contexts/AuthContext';
import { Header } from './components/Header';
import { Footer } from './components/Footer';
import { HomePage } from './pages/HomePage';
import { ProductsPage } from './pages/ProductsPage';
import { ProductDetailPage } from './pages/ProductDetailPage';
import { CartPage } from './pages/CartPage';
import { CheckoutPage } from './pages/CheckoutPage';
import { LoginPage } from './pages/LoginPage';
import { RegisterPage } from './pages/RegisterPage';
import { AdminDashboard } from './pages/admin/AdminDashboard';
import { ProtectedRoute } from './components/ProtectedRoute';

function App() {
  return (
    <Provider store={store}>
      <AuthProvider>
        <Router>
          <div className="min-h-screen bg-gray-50 flex flex-col">
            <Header />
            <main className="flex-1">
              <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/products" element={<ProductsPage />} />
                <Route path="/products/:id" element={<ProductDetailPage />} />
                <Route path="/cart" element={<CartPage />} />
                <Route path="/checkout" element={<CheckoutPage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route 
                  path="/admin/*" 
                  element={
                    <ProtectedRoute requiredRole="Admin">
                      <AdminDashboard />
                    </ProtectedRoute>
                  } 
                />
                <Route path="*" element={<Navigate to="/" replace />} />
              </Routes>
            </main>
            <Footer />
          </div>
          <Toaster position="top-right" />
        </Router>
      </AuthProvider>
    </Provider>
  );
}

export default App;`
          },
          {
            id: 'file-002',
            name: 'products-page.tsx',
            type: 'application/typescript',
            size: 32400,
            url: '/api/files/products-page.tsx',
            isGenerated: true,
            content: `import React, { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import { Search, Filter, Star, ShoppingCart } from 'lucide-react';
import { Card, CardContent } from '../components/ui/card';
import { Button } from '../components/ui/button';
import { Input } from '../components/ui/input';
import { Badge } from '../components/ui/badge';
import { useProducts } from '../hooks/useProducts';
import { useCart } from '../hooks/useCart';
import { Product } from '../types/product';

export function ProductsPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const [searchTerm, setSearchTerm] = useState(searchParams.get('search') || '');
  const [selectedCategory, setSelectedCategory] = useState(searchParams.get('category') || '');
  const [priceRange, setPriceRange] = useState({ min: 0, max: 1000 });
  const [sortBy, setSortBy] = useState('name');
  
  const { products, isLoading, categories } = useProducts({
    search: searchTerm,
    category: selectedCategory,
    minPrice: priceRange.min,
    maxPrice: priceRange.max,
    sortBy
  });
  
  const { addToCart } = useCart();

  const filteredProducts = products.filter(product => {
    const matchesSearch = product.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         product.description.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesCategory = !selectedCategory || product.categoryId === selectedCategory;
    const matchesPrice = product.price >= priceRange.min && product.price <= priceRange.max;
    
    return matchesSearch && matchesCategory && matchesPrice;
  });

  const handleSearchChange = (value: string) => {
    setSearchTerm(value);
    const newParams = new URLSearchParams(searchParams);
    if (value) {
      newParams.set('search', value);
    } else {
      newParams.delete('search');
    }
    setSearchParams(newParams);
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="mb-8">
        <h1 className="text-3xl font-bold mb-4">Products</h1>
        
        <div className="flex flex-col md:flex-row gap-4 mb-6">
          <div className="relative flex-1">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 h-4 w-4" />
            <Input
              placeholder="Search products..."
              value={searchTerm}
              onChange={(e) => handleSearchChange(e.target.value)}
              className="pl-10"
            />
          </div>
          <Button variant="outline">
            <Filter className="h-4 w-4 mr-2" />
            Filters
          </Button>
        </div>
      </div>

      {isLoading ? (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {Array.from({ length: 8 }).map((_, index) => (
            <Card key={index} className="animate-pulse">
              <div className="h-48 bg-gray-200 rounded-t-lg"></div>
              <CardContent className="p-4">
                <div className="h-4 bg-gray-200 rounded mb-2"></div>
                <div className="h-3 bg-gray-200 rounded mb-4"></div>
                <div className="h-6 bg-gray-200 rounded"></div>
              </CardContent>
            </Card>
          ))}
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {filteredProducts.map((product) => (
            <Card key={product.id} className="overflow-hidden hover:shadow-lg transition-shadow">
              <div className="relative">
                <img
                  src={product.imageUrl}
                  alt={product.name}
                  className="w-full h-48 object-cover"
                />
                {!product.inStock && (
                  <Badge variant="destructive" className="absolute top-2 right-2">
                    Out of Stock
                  </Badge>
                )}
              </div>
              <CardContent className="p-4">
                <h3 className="font-semibold text-lg mb-2">{product.name}</h3>
                <p className="text-gray-600 text-sm mb-3 line-clamp-2">{product.description}</p>
                
                <div className="flex items-center gap-1 mb-3">
                  {Array.from({ length: 5 }).map((_, index) => (
                    <Star
                      key={index}
                      className={\`h-4 w-4 \${
                        index < product.rating ? 'text-yellow-400 fill-current' : 'text-gray-300'
                      }\`}
                    />
                  ))}
                  <span className="text-sm text-gray-600 ml-1">({product.reviewCount})</span>
                </div>

                <div className="flex items-center justify-between">
                  <span className="text-xl font-bold text-green-600">
                    \${product.price.toFixed(2)}
                  </span>
                  <Button
                    onClick={() => addToCart(product.id)}
                    disabled={!product.inStock}
                    size="sm"
                  >
                    <ShoppingCart className="h-4 w-4 mr-1" />
                    Add to Cart
                  </Button>
                </div>
              </CardContent>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
}`
          }
        ]
      }
    ];

    setWorkflows(mockWorkflows);
    if (mockWorkflows.length > 0) {
      setSelectedWorkflow(mockWorkflows[0]);
      loadWorkflowComments(mockWorkflows[0].id);
    }
  };

  const loadWorkflowComments = (workflowId: string) => {
    const mockComments: WorkflowComment[] = [
      {
        id: 'comment-001',
        workflowId: workflowId,
        author: 'John Smith',
        content: 'The e-commerce platform looks great! The code quality is excellent and all requirements have been met.',
        createdAt: new Date('2025-01-15T14:50:00'),
        type: 'comment'
      },
      {
        id: 'comment-002',
        workflowId: workflowId,
        author: 'Technical Lead',
        content: 'Security analysis passed with flying colors. The authentication system is robust and follows best practices.',
        createdAt: new Date('2025-01-15T14:55:00'),
        type: 'comment'
      }
    ];
    setComments(mockComments);
  };

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'completed':
        return <CheckCircle className="h-4 w-4 text-green-500" />;
      case 'failed':
        return <XCircle className="h-4 w-4 text-red-500" />;
      case 'running':
        return <RefreshCw className="h-4 w-4 text-blue-500 animate-spin" />;
      case 'approved':
        return <ThumbsUp className="h-4 w-4 text-green-600" />;
      case 'rejected':
        return <ThumbsDown className="h-4 w-4 text-red-600" />;
      case 'cancelled':
        return <XCircle className="h-4 w-4 text-gray-500" />;
      default:
        return <Clock className="h-4 w-4 text-gray-400" />;
    }
  };

  const getTypeIcon = (type: string) => {
    switch (type) {
      case 'code-generation':
        return <Code className="h-4 w-4" />;
      case 'brd-generation':
        return <FileText className="h-4 w-4" />;
      case 'proposal-generation':
        return <Zap className="h-4 w-4" />;
      case 'presentation-generation':
        return <Zap className="h-4 w-4" />;
      case 'testing-security':
        return <AlertTriangle className="h-4 w-4" />;
      default:
        return <FileText className="h-4 w-4" />;
    }
  };

  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case 'urgent':
        return 'bg-red-100 text-red-800';
      case 'high':
        return 'bg-orange-100 text-orange-800';
      case 'medium':
        return 'bg-yellow-100 text-yellow-800';
      case 'low':
        return 'bg-green-100 text-green-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const handleApproveWorkflow = async (workflowId: string) => {
    setIsLoading(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      setWorkflows(prev => prev.map(workflow => 
        workflow.id === workflowId 
          ? { 
              ...workflow, 
              status: 'approved' as const,
              approvedAt: new Date(),
              approvedBy: 'Current User'
            }
          : workflow
      ));
      
      if (selectedWorkflow?.id === workflowId) {
        setSelectedWorkflow(prev => prev ? {
          ...prev,
          status: 'approved' as const,
          approvedAt: new Date(),
          approvedBy: 'Current User'
        } : null);
      }
    } catch (error) {
      console.error('Failed to approve workflow:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleRejectWorkflow = async (workflowId: string, reason: string) => {
    setIsLoading(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      setWorkflows(prev => prev.map(workflow => 
        workflow.id === workflowId 
          ? { 
              ...workflow, 
              status: 'rejected' as const,
              rejectedAt: new Date(),
              rejectedBy: 'Current User',
              rejectionReason: reason
            }
          : workflow
      ));
      
      if (selectedWorkflow?.id === workflowId) {
        setSelectedWorkflow(prev => prev ? {
          ...prev,
          status: 'rejected' as const,
          rejectedAt: new Date(),
          rejectedBy: 'Current User',
          rejectionReason: reason
        } : null);
      }
    } catch (error) {
      console.error('Failed to reject workflow:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleDownloadFile = (file: WorkflowFile) => {
    const blob = new Blob([file.content || ''], { type: file.type });
    const url = URL.createObjectURL(blob);
    
    const link = document.createElement('a');
    link.href = url;
    link.download = file.name;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    
    URL.revokeObjectURL(url);
  };

  const handleAddComment = async () => {
    if (!newComment.trim() || !selectedWorkflow) return;
    
    const comment: WorkflowComment = {
      id: `comment-${Date.now()}`,
      workflowId: selectedWorkflow.id,
      author: 'Current User',
      content: newComment,
      createdAt: new Date(),
      type: 'comment'
    };
    
    setComments(prev => [...prev, comment]);
    setNewComment('');
  };

  const formatDuration = (minutes: number): string => {
    if (minutes < 60) return `${minutes}m`;
    const hours = Math.floor(minutes / 60);
    const remainingMinutes = minutes % 60;
    return remainingMinutes > 0 ? `${hours}h ${remainingMinutes}m` : `${hours}h`;
  };

  const filteredWorkflows = workflows.filter(workflow => {
    switch (activeTab) {
      case 'active':
        return ['pending', 'running'].includes(workflow.status);
      case 'completed':
        return workflow.status === 'completed';
      case 'approved':
        return workflow.status === 'approved';
      case 'rejected':
        return workflow.status === 'rejected';
      default:
        return true;
    }
  });

  return (
    <div className="space-y-6">
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Zap className="h-5 w-5" />
            Workflow Processing & Acceptance
          </CardTitle>
        </CardHeader>
        <CardContent>
          <p className="text-sm text-gray-600">
            Monitor and manage AI-generated workflows with comprehensive approval and quality assurance processes.
          </p>
        </CardContent>
      </Card>

      <Tabs value={activeTab} onValueChange={setActiveTab}>
        <TabsList className="grid w-full grid-cols-5">
          <TabsTrigger value="active">Active ({workflows.filter(w => ['pending', 'running'].includes(w.status)).length})</TabsTrigger>
          <TabsTrigger value="completed">Completed ({workflows.filter(w => w.status === 'completed').length})</TabsTrigger>
          <TabsTrigger value="approved">Approved ({workflows.filter(w => w.status === 'approved').length})</TabsTrigger>
          <TabsTrigger value="rejected">Rejected ({workflows.filter(w => w.status === 'rejected').length})</TabsTrigger>
          <TabsTrigger value="all">All ({workflows.length})</TabsTrigger>
        </TabsList>

        <TabsContent value={activeTab} className="space-y-6">
          <div className="flex items-center gap-4">
            <div className="relative flex-1">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 h-4 w-4" />
              <Input
                placeholder="Search workflows..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="pl-10"
              />
            </div>
            <Button variant="outline">
              <Filter className="h-4 w-4 mr-2" />
              Filters
            </Button>
          </div>

          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <Card>
              <CardHeader>
                <CardTitle>Workflows</CardTitle>
              </CardHeader>
              <CardContent>
                <ScrollArea className="h-96">
                  <div className="space-y-3">
                    {filteredWorkflows.map((workflow) => (
                      <div
                        key={workflow.id}
                        className={`border rounded-lg p-4 cursor-pointer transition-colors ${
                          selectedWorkflow?.id === workflow.id 
                            ? 'ring-2 ring-blue-500 bg-blue-50' 
                            : 'hover:bg-gray-50'
                        }`}
                        onClick={() => {
                          setSelectedWorkflow(workflow);
                          loadWorkflowComments(workflow.id);
                        }}
                      >
                        <div className="flex items-start justify-between mb-2">
                          <div className="flex items-center gap-2">
                            {getStatusIcon(workflow.status)}
                            <span className="font-medium">{workflow.name}</span>
                          </div>
                          <Badge className={getPriorityColor(workflow.priority)}>
                            {workflow.priority}
                          </Badge>
                        </div>
                        
                        <div className="flex items-center gap-2 mb-2">
                          {getTypeIcon(workflow.type)}
                          <span className="text-sm text-gray-600 capitalize">
                            {workflow.type.replace('-', ' ')}
                          </span>
                        </div>
                        
                        <div className="text-xs text-gray-500 mb-2">
                          Requested by {workflow.requestedBy} • {workflow.createdAt.toLocaleDateString()}
                        </div>
                        
                        <Progress 
                          value={workflow.steps.filter(s => s.status === 'completed').length / workflow.steps.length * 100} 
                          className="h-2"
                        />
                        
                        <div className="text-xs text-gray-500 mt-1">
                          {workflow.steps.filter(s => s.status === 'completed').length} of {workflow.steps.length} steps completed
                        </div>
                      </div>
                    ))}
                  </div>
                </ScrollArea>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Workflow Details</CardTitle>
              </CardHeader>
              <CardContent>
                {selectedWorkflow ? (
                  <div className="space-y-6">
                    <div>
                      <div className="flex items-center justify-between mb-2">
                        <h3 className="text-lg font-semibold">{selectedWorkflow.name}</h3>
                        <div className="flex items-center gap-2">
                          {getStatusIcon(selectedWorkflow.status)}
                          <Badge className={getPriorityColor(selectedWorkflow.priority)}>
                            {selectedWorkflow.priority}
                          </Badge>
                        </div>
                      </div>
                      
                      <p className="text-sm text-gray-600 mb-4">{selectedWorkflow.description}</p>
                      
                      <div className="grid grid-cols-2 gap-4 text-sm">
                        <div>
                          <span className="font-medium">Type:</span> {selectedWorkflow.type.replace('-', ' ')}
                        </div>
                        <div>
                          <span className="font-medium">Requested by:</span> {selectedWorkflow.requestedBy}
                        </div>
                        <div>
                          <span className="font-medium">Assigned to:</span> {selectedWorkflow.assignedTo}
                        </div>
                        <div>
                          <span className="font-medium">Estimated cost:</span> ${selectedWorkflow.estimatedCost}
                        </div>
                      </div>
                    </div>

                    <div>
                      <h4 className="font-medium mb-2">Requirements</h4>
                      <ul className="text-sm text-gray-600 space-y-1">
                        {selectedWorkflow.requirements.map((req, index) => (
                          <li key={index} className="flex items-start gap-2">
                            <span className="text-blue-500 mt-1">•</span>
                            {req}
                          </li>
                        ))}
                      </ul>
                    </div>

                    <div>
                      <h4 className="font-medium mb-2">Progress Steps</h4>
                      <div className="space-y-2">
                        {selectedWorkflow.steps.map((step) => (
                          <div key={step.id} className="border rounded p-3">
                            <div className="flex items-center justify-between mb-2">
                              <div className="flex items-center gap-2">
                                {getStatusIcon(step.status)}
                                <span className="text-sm font-medium">{step.name}</span>
                              </div>
                              <span className="text-xs text-gray-500">
                                {step.progress}%
                              </span>
                            </div>
                            
                            <Progress value={step.progress} className="h-1 mb-2" />
                            
                            <div className="text-xs text-gray-500 space-y-1">
                              {step.estimatedDuration && (
                                <div>
                                  Estimated: {formatDuration(step.estimatedDuration)}
                                  {step.actualDuration && ` • Actual: ${formatDuration(step.actualDuration)}`}
                                </div>
                              )}
                              {step.output && (
                                <div className="text-green-600">{step.output}</div>
                              )}
                            </div>
                          </div>
                        ))}
                      </div>
                    </div>

                    {selectedWorkflow.files.length > 0 && (
                      <div>
                        <h4 className="font-medium mb-2">Generated Files</h4>
                        <div className="space-y-2">
                          {selectedWorkflow.files.map((file) => (
                            <div key={file.id} className="flex items-center justify-between p-2 border rounded">
                              <div className="flex items-center gap-2">
                                <FileText className="h-4 w-4 text-gray-500" />
                                <div>
                                  <div className="text-sm font-medium">{file.name}</div>
                                  <div className="text-xs text-gray-500">
                                    {(file.size / 1024).toFixed(1)} KB
                                  </div>
                                </div>
                              </div>
                              <Button
                                size="sm"
                                variant="outline"
                                onClick={() => handleDownloadFile(file)}
                              >
                                <Download className="h-4 w-4" />
                              </Button>
                            </div>
                          ))}
                        </div>
                      </div>
                    )}

                    {selectedWorkflow.status === 'completed' && (
                      <div className="flex gap-2">
                        <Button 
                          onClick={() => handleApproveWorkflow(selectedWorkflow.id)} 
                          className="flex-1"
                          disabled={isLoading}
                        >
                          <ThumbsUp className="h-4 w-4 mr-2" />
                          Approve
                        </Button>
                        <Button 
                          variant="outline" 
                          onClick={() => handleRejectWorkflow(selectedWorkflow.id, 'Quality issues identified')} 
                          className="flex-1"
                          disabled={isLoading}
                        >
                          <ThumbsDown className="h-4 w-4 mr-2" />
                          Reject
                        </Button>
                      </div>
                    )}

                    {(selectedWorkflow.status === 'approved' || selectedWorkflow.status === 'rejected') && (
                      <div className="p-3 rounded-lg bg-gray-50">
                        <div className="text-sm">
                          <span className="font-medium">
                            {selectedWorkflow.status === 'approved' ? 'Approved' : 'Rejected'} by:
                          </span>{' '}
                          {selectedWorkflow.status === 'approved' ? selectedWorkflow.approvedBy : selectedWorkflow.rejectedBy}
                        </div>
                        <div className="text-xs text-gray-500">
                          {selectedWorkflow.status === 'approved' 
                            ? selectedWorkflow.approvedAt?.toLocaleString()
                            : selectedWorkflow.rejectedAt?.toLocaleString()
                          }
                        </div>
                        {selectedWorkflow.rejectionReason && (
                          <div className="text-sm text-red-600 mt-1">
                            Reason: {selectedWorkflow.rejectionReason}
                          </div>
                        )}
                      </div>
                    )}

                    <div>
                      <h4 className="font-medium mb-2">Comments & Reviews</h4>
                      <div className="space-y-3 mb-4">
                        {comments.map((comment) => (
                          <div key={comment.id} className="border rounded p-3">
                            <div className="flex items-center gap-2 mb-1">
                              <MessageSquare className="h-4 w-4 text-gray-500" />
                              <span className="text-sm font-medium">{comment.author}</span>
                              <span className="text-xs text-gray-500">
                                {comment.createdAt.toLocaleString()}
                              </span>
                            </div>
                            <p className="text-sm text-gray-700">{comment.content}</p>
                          </div>
                        ))}
                      </div>
                      
                      <div className="flex gap-2">
                        <Textarea
                          placeholder="Add a comment or review..."
                          value={newComment}
                          onChange={(e) => setNewComment(e.target.value)}
                          className="flex-1"
                          rows={2}
                        />
                        <Button onClick={handleAddComment} disabled={!newComment.trim()}>
                          <Send className="h-4 w-4" />
                        </Button>
                      </div>
                    </div>
                  </div>
                ) : (
                  <div className="text-center text-gray-500 py-12">
                    <Zap className="h-12 w-12 mx-auto mb-4 text-gray-300" />
                    <h3 className="text-lg font-medium mb-2">No Workflow Selected</h3>
                    <p className="text-sm">Select a workflow from the list to view its details and manage approval process.</p>
                  </div>
                )}
              </CardContent>
            </Card>
          </div>
        </TabsContent>
      </Tabs>
    </div>
  );
}
