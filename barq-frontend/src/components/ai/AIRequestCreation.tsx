import { useState, useEffect } from 'react';
import { Plus, Bot, FileText, Code, TestTube, BookOpen, Eye } from 'lucide-react';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Label } from '../ui/label';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';
import { LoadingSpinner } from '../ui/loading-spinner';
import { FileUpload } from '../ui/file-upload';
import { FileUploadItem } from '../../services/fileService';
import { aiService, AIProvider, AIRequestCreateRequest } from '../../services/aiService';
import { fileService } from '../../services/fileService';
import { cn } from '../../lib/utils';

interface AIRequestCreationProps {
  onRequestCreated: (requestId: string) => void;
  onCancel: () => void;
  initialRequestType?: string;
  className?: string;
}

const REQUEST_TYPES = [
  {
    id: 'CodeGeneration',
    name: 'Code Generation',
    description: 'Generate code, implement features, or create applications',
    icon: <Code className="h-5 w-5" />,
    color: 'bg-blue-500',
  },
  {
    id: 'DocumentGeneration',
    name: 'BRD Generation',
    description: 'Create business requirements documents and specifications',
    icon: <FileText className="h-5 w-5" />,
    color: 'bg-green-500',
  },
  {
    id: 'Testing',
    name: 'Testing',
    description: 'Generate tests, perform code review, or quality assurance',
    icon: <TestTube className="h-5 w-5" />,
    color: 'bg-purple-500',
  },
  {
    id: 'DocumentGeneration',
    name: 'Documentation',
    description: 'Create documentation, guides, or technical writing',
    icon: <BookOpen className="h-5 w-5" />,
    color: 'bg-orange-500',
  },
  {
    id: 'QualityAssurance',
    name: 'Code Review',
    description: 'Review code, analyze architecture, or provide feedback',
    icon: <Eye className="h-5 w-5" />,
    color: 'bg-red-500',
  },
];

const PRIORITY_LEVELS = [
  { id: 'Low', name: 'Low', color: 'text-green-600 bg-green-100' },
  { id: 'Normal', name: 'Medium', color: 'text-yellow-600 bg-yellow-100' },
  { id: 'High', name: 'High', color: 'text-orange-600 bg-orange-100' },
  { id: 'Critical', name: 'Critical', color: 'text-red-600 bg-red-100' },
];

export function AIRequestCreation({
  onRequestCreated,
  onCancel,
  initialRequestType = 'CodeGeneration',
  className
}: AIRequestCreationProps) {
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    requestType: initialRequestType,
    priority: 'Normal',
    aiProvider: 'DevinAI',
    estimatedHours: '',
    tags: '',
  });

  const [providers, setProviders] = useState<AIProvider[]>([]);
  const [, setAttachedFiles] = useState<FileUploadItem[]>([]);
  const [uploadedFileIds, setUploadedFileIds] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadProviders();
  }, []);

  const loadProviders = async () => {
    try {
      setIsLoading(true);
      const response = await aiService.getAvailableProviders();
      if (response.success) {
        const enabledProviders = response.data.filter(p => p.isEnabled);
        setProviders(enabledProviders);
        
        const devinProvider = enabledProviders.find(p => p.name === 'DevinAI');
        if (devinProvider) {
          setFormData(prev => ({ ...prev, aiProvider: devinProvider.name }));
        } else if (enabledProviders.length > 0) {
          setFormData(prev => ({ ...prev, aiProvider: enabledProviders[0].name }));
        }
      }
    } catch (err) {
      setError('Failed to load AI providers');
      console.error('Error loading providers:', err);
    } finally {
      setIsLoading(false);
    }
  };

  const handleInputChange = (field: string, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    setError(null);
  };

  const handleFileUpload = async (files: FileUploadItem[]): Promise<FileUploadItem[]> => {
    try {
      const uploadResults = await fileService.uploadMultipleFiles(files);
      const fileIds = uploadResults.map(result => result.data.fileId);
      setUploadedFileIds(prev => [...prev, ...fileIds]);
      
      return files.map(file => ({
        ...file,
        status: 'completed' as const,
        progress: 100
      }));
    } catch (error) {
      return files.map(file => ({
        ...file,
        status: 'error' as const,
        error: 'Upload failed'
      }));
    }
  };

  const handleFileRemove = (fileId: string) => {
    setAttachedFiles(prev => prev.filter(f => f.id !== fileId));
    setUploadedFileIds(prev => prev.filter(id => id !== fileId));
  };

  const validateForm = (): string | null => {
    if (!formData.title.trim()) return 'Title is required';
    if (!formData.description.trim()) return 'Description is required';
    if (!formData.requestType) return 'Request type is required';
    if (!formData.priority) return 'Priority is required';
    if (!formData.aiProvider) return 'AI provider is required';
    return null;
  };

  const handleSubmit = async () => {
    const validationError = validateForm();
    if (validationError) {
      setError(validationError);
      return;
    }

    setIsSubmitting(true);
    setError(null);

    try {
      const request: AIRequestCreateRequest = {
        title: formData.title.trim(),
        description: formData.description.trim(),
        requestType: formData.requestType as any,
        priority: formData.priority as any,
        aiProvider: formData.aiProvider,
        attachments: uploadedFileIds,
        tags: formData.tags ? formData.tags.split(',').map(tag => tag.trim()).filter(Boolean) : [],
        estimatedHours: formData.estimatedHours ? parseInt(formData.estimatedHours) : undefined,
      };

      const response = await aiService.createAIRequest(request);
      
      if (response.success) {
        const submitResponse = await aiService.submitAIRequest(response.data.id);
        if (submitResponse.success) {
          onRequestCreated(response.data.id);
        } else {
          setError(submitResponse.message || 'Failed to submit AI request for workflow processing');
        }
      } else {
        setError(response.message || 'Failed to create AI request');
      }
    } catch (err) {
      console.error('Error creating AI request:', err);
      if (err instanceof Error) {
        setError(err.message);
      } else if (typeof err === 'object' && err !== null && 'response' in err) {
        const axiosError = err as any;
        if (axiosError.response?.data?.errors) {
          const validationErrors = Object.entries(axiosError.response.data.errors)
            .map(([field, messages]) => `${field}: ${Array.isArray(messages) ? messages.join(', ') : messages}`)
            .join('; ');
          setError(`Validation errors: ${validationErrors}`);
        } else if (axiosError.response?.data?.message) {
          setError(axiosError.response.data.message);
        } else {
          setError(`Request failed with status ${axiosError.response?.status || 'unknown'}`);
        }
      } else {
        setError('Failed to create AI request');
      }
    } finally {
      setIsSubmitting(false);
    }
  };


  return (
    <div className={cn('max-w-4xl mx-auto p-6', className)}>
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center space-x-2">
            <Plus className="h-5 w-5" />
            <span>Create AI Request</span>
          </CardTitle>
          <CardDescription>
            Create a new AI-powered request for code generation, documentation, or other tasks
          </CardDescription>
        </CardHeader>
        
        <CardContent className="space-y-6">
          {error && (
            <div className="p-3 bg-red-50 border border-red-200 rounded-md text-red-700 text-sm">
              {error}
            </div>
          )}

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="space-y-4">
              <div>
                <Label htmlFor="title">Request Title</Label>
                <Input
                  id="title"
                  value={formData.title}
                  onChange={(e) => handleInputChange('title', e.target.value)}
                  placeholder="e.g., Build a React dashboard component"
                  className="mt-1"
                />
              </div>

              <div>
                <Label htmlFor="description">Description</Label>
                <textarea
                  id="description"
                  value={formData.description}
                  onChange={(e) => handleInputChange('description', e.target.value)}
                  placeholder="Describe what you want the AI to help you with..."
                  rows={4}
                  className="mt-1 w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 resize-none"
                />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Label htmlFor="priority">Priority</Label>
                  <select
                    id="priority"
                    value={formData.priority}
                    onChange={(e) => handleInputChange('priority', e.target.value)}
                    className="mt-1 w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500"
                  >
                    {PRIORITY_LEVELS.map(priority => (
                      <option key={priority.id} value={priority.id}>
                        {priority.name}
                      </option>
                    ))}
                  </select>
                </div>

                <div>
                  <Label htmlFor="estimatedHours">Estimated Hours</Label>
                  <Input
                    id="estimatedHours"
                    type="number"
                    value={formData.estimatedHours}
                    onChange={(e) => handleInputChange('estimatedHours', e.target.value)}
                    placeholder="Optional"
                    className="mt-1"
                  />
                </div>
              </div>

              <div>
                <Label htmlFor="tags">Tags (comma-separated)</Label>
                <Input
                  id="tags"
                  value={formData.tags}
                  onChange={(e) => handleInputChange('tags', e.target.value)}
                  placeholder="e.g., react, dashboard, frontend"
                  className="mt-1"
                />
              </div>
            </div>

            <div className="space-y-4">
              <div>
                <Label>Request Type</Label>
                <div className="mt-2 grid grid-cols-1 gap-2">
                  {REQUEST_TYPES.map(type => (
                    <button
                      key={type.id}
                      type="button"
                      onClick={() => handleInputChange('requestType', type.id)}
                      className={cn(
                        'flex items-center space-x-3 p-3 rounded-lg border-2 transition-colors text-left',
                        formData.requestType === type.id
                          ? 'border-blue-500 bg-blue-50 dark:bg-blue-950/20'
                          : 'border-gray-200 hover:border-gray-300 dark:border-gray-700'
                      )}
                    >
                      <div className={cn('p-2 rounded-md text-white', type.color)}>
                        {type.icon}
                      </div>
                      <div>
                        <div className="font-medium text-sm">{type.name}</div>
                        <div className="text-xs text-gray-500 dark:text-gray-400">
                          {type.description}
                        </div>
                      </div>
                    </button>
                  ))}
                </div>
              </div>

              <div>
                <Label htmlFor="aiProvider">AI Provider</Label>
                {isLoading ? (
                  <div className="mt-1 flex items-center space-x-2 p-2">
                    <LoadingSpinner className="h-4 w-4" />
                    <span className="text-sm text-gray-500">Loading providers...</span>
                  </div>
                ) : (
                  <select
                    id="aiProvider"
                    value={formData.aiProvider}
                    onChange={(e) => handleInputChange('aiProvider', e.target.value)}
                    className="mt-1 w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500"
                  >
                    {providers.map(provider => (
                      <option key={provider.id} value={provider.name}>
                        {aiService.getAIProviderIcon(provider.name)} {provider.name}
                      </option>
                    ))}
                  </select>
                )}
              </div>
            </div>
          </div>

          <div>
            <Label>File Attachments</Label>
            <div className="mt-2">
              <FileUpload
                onFilesSelected={setAttachedFiles}
                onFileRemove={handleFileRemove}
                onUpload={handleFileUpload}
                multiple={true}
                maxFiles={10}
              />
            </div>
          </div>

          <div className="flex items-center justify-between pt-4 border-t border-gray-200 dark:border-gray-700">
            <Button variant="outline" onClick={onCancel}>
              Cancel
            </Button>
            
            <Button
              onClick={handleSubmit}
              disabled={isSubmitting}
              className="min-w-[120px]"
            >
              {isSubmitting ? (
                <>
                  <LoadingSpinner className="mr-2 h-4 w-4" />
                  Creating...
                </>
              ) : (
                <>
                  <Bot className="mr-2 h-4 w-4" />
                  Create Request
                </>
              )}
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
