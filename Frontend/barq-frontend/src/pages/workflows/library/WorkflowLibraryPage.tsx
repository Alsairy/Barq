import { useState, useEffect } from 'react';
import {
  Search,
  Plus,
  Copy,
  Edit,
  Trash2,
  Upload,
  Star,
  StarOff,
  Eye,
  Play,
  Tag,
  BookOpen,
  Folder,
  Share,
  Lock,
} from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../../components/ui/card';
import { Button } from '../../../components/ui/button';
import { Badge } from '../../../components/ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../../components/ui/tabs';
import { Input } from '../../../components/ui/input';
import { Label } from '../../../components/ui/label';
import { LoadingSpinner } from '../../../components/ui/loading-spinner';

interface WorkflowTemplate {
  id: string;
  name: string;
  description: string;
  category: string;
  version: string;
  isPublic: boolean;
  isFavorite: boolean;
  createdBy: string;
  createdAt: string;
  lastModified: string;
  usageCount: number;
  rating: number;
  ratingCount: number;
  tags: string[];
  complexity: 'simple' | 'medium' | 'complex';
  estimatedDuration: number;
  permissions: {
    canView: boolean;
    canEdit: boolean;
    canExecute: boolean;
    canShare: boolean;
  };
  versions: WorkflowVersion[];
  collaborators: string[];
}

interface WorkflowVersion {
  id: string;
  version: string;
  description: string;
  createdBy: string;
  createdAt: string;
  isActive: boolean;
  changeLog: string[];
}

interface WorkflowCategory {
  id: string;
  name: string;
  description: string;
  icon: string;
  workflowCount: number;
  isExpanded: boolean;
}

export default function WorkflowLibraryPage() {
  const [templates, setTemplates] = useState<WorkflowTemplate[]>([]);
  const [categories, setCategories] = useState<WorkflowCategory[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCategory, setSelectedCategory] = useState<string>('all');
  const [sortBy, setSortBy] = useState<string>('name');
  const [viewMode, setViewMode] = useState<'grid' | 'list'>('grid');
  const [showFavoritesOnly, setShowFavoritesOnly] = useState(false);

  useEffect(() => {
    fetchTemplates();
    fetchCategories();
  }, []);

  const fetchTemplates = async () => {
    setIsLoading(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      const mockTemplates: WorkflowTemplate[] = [
        {
          id: '1',
          name: 'Customer Onboarding',
          description: 'Complete customer onboarding process with document verification and account setup',
          category: 'Customer Management',
          version: '2.1.0',
          isPublic: true,
          isFavorite: true,
          createdBy: 'john.doe@example.com',
          createdAt: '2024-01-10T10:00:00Z',
          lastModified: '2024-01-15T14:30:00Z',
          usageCount: 245,
          rating: 4.8,
          ratingCount: 32,
          tags: ['onboarding', 'customer', 'verification', 'automation'],
          complexity: 'medium',
          estimatedDuration: 1800000,
          permissions: {
            canView: true,
            canEdit: true,
            canExecute: true,
            canShare: true,
          },
          versions: [
            {
              id: 'v1',
              version: '2.1.0',
              description: 'Added automated document verification',
              createdBy: 'john.doe@example.com',
              createdAt: '2024-01-15T14:30:00Z',
              isActive: true,
              changeLog: ['Added AI document verification', 'Improved error handling', 'Updated UI components']
            },
            {
              id: 'v2',
              version: '2.0.0',
              description: 'Major redesign with new approval process',
              createdBy: 'jane.smith@example.com',
              createdAt: '2024-01-10T10:00:00Z',
              isActive: false,
              changeLog: ['Redesigned approval workflow', 'Added multi-step verification']
            }
          ],
          collaborators: ['jane.smith@example.com', 'bob.wilson@example.com']
        },
        {
          id: '2',
          name: 'Invoice Processing',
          description: 'Automated invoice processing with approval workflow and payment scheduling',
          category: 'Finance',
          version: '1.5.2',
          isPublic: false,
          isFavorite: false,
          createdBy: 'finance@example.com',
          createdAt: '2024-01-05T09:00:00Z',
          lastModified: '2024-01-12T16:45:00Z',
          usageCount: 189,
          rating: 4.6,
          ratingCount: 28,
          tags: ['finance', 'invoice', 'approval', 'payment'],
          complexity: 'simple',
          estimatedDuration: 900000,
          permissions: {
            canView: true,
            canEdit: false,
            canExecute: true,
            canShare: false,
          },
          versions: [
            {
              id: 'v1',
              version: '1.5.2',
              description: 'Bug fixes and performance improvements',
              createdBy: 'finance@example.com',
              createdAt: '2024-01-12T16:45:00Z',
              isActive: true,
              changeLog: ['Fixed calculation errors', 'Improved performance']
            }
          ],
          collaborators: ['accounting@example.com']
        },
        {
          id: '3',
          name: 'Data Migration Pipeline',
          description: 'Complex data migration workflow with validation, transformation, and rollback capabilities',
          category: 'Data Management',
          version: '3.0.0-beta',
          isPublic: true,
          isFavorite: true,
          createdBy: 'data.team@example.com',
          createdAt: '2024-01-01T08:00:00Z',
          lastModified: '2024-01-14T11:20:00Z',
          usageCount: 67,
          rating: 4.2,
          ratingCount: 15,
          tags: ['data', 'migration', 'etl', 'validation'],
          complexity: 'complex',
          estimatedDuration: 7200000,
          permissions: {
            canView: true,
            canEdit: true,
            canExecute: false,
            canShare: true,
          },
          versions: [
            {
              id: 'v1',
              version: '3.0.0-beta',
              description: 'Beta release with new transformation engine',
              createdBy: 'data.team@example.com',
              createdAt: '2024-01-14T11:20:00Z',
              isActive: true,
              changeLog: ['New transformation engine', 'Enhanced validation', 'Rollback capabilities']
            }
          ],
          collaborators: ['dev.team@example.com', 'qa.team@example.com']
        }
      ];

      setTemplates(mockTemplates);
    } catch (error) {
      console.error('Failed to fetch templates:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const fetchCategories = async () => {
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
      
      const mockCategories: WorkflowCategory[] = [
        {
          id: 'customer',
          name: 'Customer Management',
          description: 'Workflows for customer onboarding, support, and management',
          icon: 'Users',
          workflowCount: 12,
          isExpanded: true
        },
        {
          id: 'finance',
          name: 'Finance',
          description: 'Financial processes, invoicing, and payment workflows',
          icon: 'DollarSign',
          workflowCount: 8,
          isExpanded: false
        },
        {
          id: 'data',
          name: 'Data Management',
          description: 'Data processing, migration, and analytics workflows',
          icon: 'Database',
          workflowCount: 15,
          isExpanded: false
        },
        {
          id: 'hr',
          name: 'Human Resources',
          description: 'Employee onboarding, performance reviews, and HR processes',
          icon: 'UserCheck',
          workflowCount: 6,
          isExpanded: false
        }
      ];

      setCategories(mockCategories);
    } catch (error) {
      console.error('Failed to fetch categories:', error);
    }
  };

  const getComplexityColor = (complexity: string) => {
    switch (complexity) {
      case 'simple':
        return 'bg-green-100 text-green-800';
      case 'medium':
        return 'bg-yellow-100 text-yellow-800';
      case 'complex':
        return 'bg-red-100 text-red-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const formatDuration = (duration: number) => {
    const hours = Math.floor(duration / 3600000);
    const minutes = Math.floor((duration % 3600000) / 60000);
    if (hours > 0) {
      return `${hours}h ${minutes}m`;
    }
    return `${minutes}m`;
  };

  const toggleFavorite = async (templateId: string) => {
    try {
      setTemplates(prev => prev.map(template => 
        template.id === templateId 
          ? { ...template, isFavorite: !template.isFavorite }
          : template
      ));
    } catch (error) {
      console.error('Failed to toggle favorite:', error);
    }
  };

  const duplicateTemplate = async (templateId: string) => {
    try {
      const template = templates.find(t => t.id === templateId);
      if (template) {
        const duplicatedTemplate: WorkflowTemplate = {
          ...template,
          id: `${templateId}-copy-${Date.now()}`,
          name: `${template.name} (Copy)`,
          createdAt: new Date().toISOString(),
          lastModified: new Date().toISOString(),
          usageCount: 0,
          version: '1.0.0',
          isFavorite: false,
        };
        setTemplates(prev => [duplicatedTemplate, ...prev]);
      }
    } catch (error) {
      console.error('Failed to duplicate template:', error);
    }
  };

  const deleteTemplate = async (templateId: string) => {
    try {
      setTemplates(prev => prev.filter(template => template.id !== templateId));
    } catch (error) {
      console.error('Failed to delete template:', error);
    }
  };

  const executeTemplate = async (templateId: string) => {
    try {
      console.log('Executing template:', templateId);
    } catch (error) {
      console.error('Failed to execute template:', error);
    }
  };

  const filteredTemplates = templates.filter(template => {
    const matchesSearch = template.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         template.description.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         template.tags.some(tag => tag.toLowerCase().includes(searchTerm.toLowerCase()));
    const matchesCategory = selectedCategory === 'all' || template.category === selectedCategory;
    const matchesFavorites = !showFavoritesOnly || template.isFavorite;
    return matchesSearch && matchesCategory && matchesFavorites;
  });

  const sortedTemplates = [...filteredTemplates].sort((a, b) => {
    switch (sortBy) {
      case 'name':
        return a.name.localeCompare(b.name);
      case 'created':
        return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
      case 'modified':
        return new Date(b.lastModified).getTime() - new Date(a.lastModified).getTime();
      case 'usage':
        return b.usageCount - a.usageCount;
      case 'rating':
        return b.rating - a.rating;
      default:
        return 0;
    }
  });

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Workflow Library</h1>
          <p className="text-muted-foreground">
            Browse, manage, and execute workflow templates
          </p>
        </div>
        <div className="flex items-center space-x-2">
          <Button variant="outline">
            <Upload className="h-4 w-4 mr-2" />
            Import
          </Button>
          <Button>
            <Plus className="h-4 w-4 mr-2" />
            Create Workflow
          </Button>
        </div>
      </div>

      <Tabs defaultValue="templates" className="space-y-4">
        <TabsList>
          <TabsTrigger value="templates">Templates</TabsTrigger>
          <TabsTrigger value="categories">Categories</TabsTrigger>
          <TabsTrigger value="shared">Shared with Me</TabsTrigger>
          <TabsTrigger value="versions">Version History</TabsTrigger>
        </TabsList>

        <TabsContent value="templates" className="space-y-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center space-x-4">
              <div className="relative">
                <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 h-4 w-4" />
                <Input
                  placeholder="Search workflows..."
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="pl-10 w-80"
                />
              </div>
              <select
                value={selectedCategory}
                onChange={(e) => setSelectedCategory(e.target.value)}
                className="px-3 py-2 border rounded-md"
              >
                <option value="all">All Categories</option>
                {categories.map((category) => (
                  <option key={category.id} value={category.name}>
                    {category.name}
                  </option>
                ))}
              </select>
              <select
                value={sortBy}
                onChange={(e) => setSortBy(e.target.value)}
                className="px-3 py-2 border rounded-md"
              >
                <option value="name">Sort by Name</option>
                <option value="created">Sort by Created</option>
                <option value="modified">Sort by Modified</option>
                <option value="usage">Sort by Usage</option>
                <option value="rating">Sort by Rating</option>
              </select>
            </div>
            <div className="flex items-center space-x-2">
              <Button
                variant={showFavoritesOnly ? "default" : "outline"}
                size="sm"
                onClick={() => setShowFavoritesOnly(!showFavoritesOnly)}
              >
                <Star className="h-4 w-4 mr-2" />
                Favorites
              </Button>
              <Button
                variant={viewMode === 'grid' ? "default" : "outline"}
                size="sm"
                onClick={() => setViewMode('grid')}
              >
                Grid
              </Button>
              <Button
                variant={viewMode === 'list' ? "default" : "outline"}
                size="sm"
                onClick={() => setViewMode('list')}
              >
                List
              </Button>
            </div>
          </div>

          <div className={viewMode === 'grid' ? 'grid gap-6 md:grid-cols-2 lg:grid-cols-3' : 'space-y-4'}>
            {sortedTemplates.map((template) => (
              <Card key={template.id} className="hover:shadow-lg transition-shadow">
                <CardHeader>
                  <div className="flex items-start justify-between">
                    <div className="flex-1">
                      <div className="flex items-center space-x-2">
                        <CardTitle className="text-lg">{template.name}</CardTitle>
                        {template.isFavorite && (
                          <Star className="h-4 w-4 text-yellow-500 fill-current" />
                        )}
                        {!template.isPublic && (
                          <Lock className="h-4 w-4 text-gray-500" />
                        )}
                      </div>
                      <CardDescription className="mt-1">
                        {template.description}
                      </CardDescription>
                    </div>
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => toggleFavorite(template.id)}
                    >
                      {template.isFavorite ? (
                        <StarOff className="h-4 w-4" />
                      ) : (
                        <Star className="h-4 w-4" />
                      )}
                    </Button>
                  </div>
                </CardHeader>
                <CardContent>
                  <div className="space-y-4">
                    <div className="flex items-center space-x-2">
                      <Badge variant="outline">{template.category}</Badge>
                      <Badge className={getComplexityColor(template.complexity)}>
                        {template.complexity}
                      </Badge>
                      <Badge variant="outline">v{template.version}</Badge>
                    </div>

                    <div className="flex flex-wrap gap-1">
                      {template.tags.slice(0, 3).map((tag) => (
                        <Badge key={tag} variant="secondary" className="text-xs">
                          <Tag className="h-3 w-3 mr-1" />
                          {tag}
                        </Badge>
                      ))}
                      {template.tags.length > 3 && (
                        <Badge variant="secondary" className="text-xs">
                          +{template.tags.length - 3}
                        </Badge>
                      )}
                    </div>

                    <div className="grid grid-cols-2 gap-4 text-sm">
                      <div>
                        <Label className="text-xs text-muted-foreground">Usage</Label>
                        <p className="font-medium">{template.usageCount} times</p>
                      </div>
                      <div>
                        <Label className="text-xs text-muted-foreground">Duration</Label>
                        <p className="font-medium">{formatDuration(template.estimatedDuration)}</p>
                      </div>
                      <div>
                        <Label className="text-xs text-muted-foreground">Rating</Label>
                        <div className="flex items-center space-x-1">
                          <Star className="h-3 w-3 text-yellow-500 fill-current" />
                          <span className="font-medium">{template.rating}</span>
                          <span className="text-muted-foreground">({template.ratingCount})</span>
                        </div>
                      </div>
                      <div>
                        <Label className="text-xs text-muted-foreground">Modified</Label>
                        <p className="font-medium">{new Date(template.lastModified).toLocaleDateString()}</p>
                      </div>
                    </div>

                    <div className="flex items-center justify-between pt-2">
                      <div className="flex items-center space-x-2">
                        {template.permissions.canExecute && (
                          <Button size="sm" onClick={() => executeTemplate(template.id)}>
                            <Play className="h-4 w-4 mr-1" />
                            Execute
                          </Button>
                        )}
                        {template.permissions.canView && (
                          <Button variant="outline" size="sm">
                            <Eye className="h-4 w-4 mr-1" />
                            View
                          </Button>
                        )}
                      </div>
                      <div className="flex items-center space-x-1">
                        {template.permissions.canEdit && (
                          <Button variant="ghost" size="sm">
                            <Edit className="h-4 w-4" />
                          </Button>
                        )}
                        <Button variant="ghost" size="sm" onClick={() => duplicateTemplate(template.id)}>
                          <Copy className="h-4 w-4" />
                        </Button>
                        {template.permissions.canShare && (
                          <Button variant="ghost" size="sm">
                            <Share className="h-4 w-4" />
                          </Button>
                        )}
                        <Button variant="ghost" size="sm" onClick={() => deleteTemplate(template.id)}>
                          <Trash2 className="h-4 w-4" />
                        </Button>
                      </div>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="categories" className="space-y-4">
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
            {categories.map((category) => (
              <Card key={category.id}>
                <CardHeader>
                  <div className="flex items-center space-x-2">
                    <div className="p-2 bg-blue-100 rounded-lg">
                      <BookOpen className="h-6 w-6 text-blue-600" />
                    </div>
                    <div>
                      <CardTitle>{category.name}</CardTitle>
                      <CardDescription>{category.workflowCount} workflows</CardDescription>
                    </div>
                  </div>
                </CardHeader>
                <CardContent>
                  <p className="text-sm text-muted-foreground">{category.description}</p>
                  <Button variant="outline" className="w-full mt-4" onClick={() => setSelectedCategory(category.name)}>
                    <Folder className="h-4 w-4 mr-2" />
                    Browse Category
                  </Button>
                </CardContent>
              </Card>
            ))}
          </div>
        </TabsContent>

        <TabsContent value="shared" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Shared Workflows</CardTitle>
              <CardDescription>
                Workflows that have been shared with you by other users
              </CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground">
                Shared workflow management with collaboration features would be implemented here.
              </p>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="versions" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Version History</CardTitle>
              <CardDescription>
                Track changes and manage versions of your workflow templates
              </CardDescription>
            </CardHeader>
            <CardContent>
              <p className="text-muted-foreground">
                Version control system with change tracking and rollback capabilities would be implemented here.
              </p>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
