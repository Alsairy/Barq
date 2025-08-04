import { useState, useEffect } from 'react';
import { Upload, FileText, Image, Code, Download, Eye, Trash2, Search, Grid, List } from 'lucide-react';
import { Card, CardContent, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Badge } from '../ui/badge';
import { ScrollArea } from '../ui/scroll-area';
import { FileUpload } from '../ui/file-upload';
import { DocumentPreviewWrapper } from './DocumentPreviewWrapper';

interface UploadedDocument {
  id: string;
  name: string;
  type: string;
  size: number;
  url: string;
  uploadedAt: Date;
  category: 'code' | 'document' | 'image' | 'archive' | 'other';
  tags: string[];
  description?: string;
  metadata?: {
    author?: string;
    version?: string;
    lastModified?: Date;
  };
}

interface DocumentManagerProps {
  onDocumentSelect?: (document: UploadedDocument) => void;
  allowMultipleSelection?: boolean;
  filterByCategory?: string[];
  maxFileSize?: number;
  acceptedTypes?: string[];
}

export function DocumentManager({
  onDocumentSelect,
  allowMultipleSelection = false,
  filterByCategory,
  maxFileSize = 50 * 1024 * 1024,
  acceptedTypes = [
    '.txt', '.md', '.js', '.ts', '.jsx', '.tsx', '.py', '.java', '.cs', '.cpp', '.c', '.h',
    '.css', '.html', '.json', '.xml', '.yaml', '.yml', '.sql', '.sh', '.bat', '.ps1',
    '.pdf', '.doc', '.docx', '.xls', '.xlsx', '.ppt', '.pptx',
    '.png', '.jpg', '.jpeg', '.gif', '.svg', '.webp',
    '.zip', '.tar', '.gz', '.rar'
  ]
}: DocumentManagerProps) {
  const [documents, setDocuments] = useState<UploadedDocument[]>([]);
  const [selectedDocument, setSelectedDocument] = useState<UploadedDocument | null>(null);
  const [selectedDocuments, setSelectedDocuments] = useState<UploadedDocument[]>([]);
  const [searchQuery, setSearchQuery] = useState('');
  const [categoryFilter, setCategoryFilter] = useState<string>('all');
  const [viewMode, setViewMode] = useState<'grid' | 'list'>('grid');

  useEffect(() => {
    loadDocuments();
  }, []);

  const loadDocuments = async () => {
    const mockDocuments: UploadedDocument[] = [
      {
        id: '1',
        name: 'project-requirements.pdf',
        type: 'application/pdf',
        size: 2048576,
        url: '/documents/project-requirements.pdf',
        uploadedAt: new Date(Date.now() - 86400000),
        category: 'document',
        tags: ['requirements', 'project'],
        description: 'Project requirements document for Q1 2025',
        metadata: {
          author: 'John Doe',
          version: '1.2',
          lastModified: new Date(Date.now() - 3600000)
        }
      },
      {
        id: '2',
        name: 'api-documentation.md',
        type: 'text/markdown',
        size: 512000,
        url: '/documents/api-documentation.md',
        uploadedAt: new Date(Date.now() - 172800000),
        category: 'document',
        tags: ['api', 'documentation'],
        description: 'REST API documentation and examples'
      },
      {
        id: '3',
        name: 'UserService.cs',
        type: 'text/plain',
        size: 15360,
        url: '/documents/UserService.cs',
        uploadedAt: new Date(Date.now() - 259200000),
        category: 'code',
        tags: ['csharp', 'service'],
        description: 'User service implementation'
      }
    ];
    setDocuments(mockDocuments);
  };

  const categorizeFile = (file: File): UploadedDocument['category'] => {
    const extension = file.name.split('.').pop()?.toLowerCase();
    
    if (['js', 'jsx', 'ts', 'tsx', 'py', 'java', 'cs', 'cpp', 'c', 'h', 'css', 'html', 'json', 'xml', 'yaml', 'yml', 'sql', 'sh', 'bat', 'ps1'].includes(extension || '')) {
      return 'code';
    }
    
    if (['jpg', 'jpeg', 'png', 'gif', 'svg', 'webp'].includes(extension || '')) {
      return 'image';
    }
    
    if (['zip', 'tar', 'gz', 'rar'].includes(extension || '')) {
      return 'archive';
    }
    
    if (['pdf', 'doc', 'docx', 'txt', 'md', 'xls', 'xlsx', 'ppt', 'pptx'].includes(extension || '')) {
      return 'document';
    }
    
    return 'other';
  };

  const handleFilesSelected = (files: any[]) => {
    console.log('Files selected:', files);
  };

  const handleFileRemove = (fileId: string) => {
    console.log('File removed:', fileId);
  };

  const handleUpload = async (files: any[]): Promise<any[]> => {
    try {
      const newDocuments: UploadedDocument[] = files.map(fileItem => ({
        id: Date.now().toString() + Math.random().toString(36).substr(2, 9),
        name: fileItem.file.name,
        type: fileItem.file.type,
        size: fileItem.file.size,
        url: URL.createObjectURL(fileItem.file),
        uploadedAt: new Date(),
        category: categorizeFile(fileItem.file),
        tags: [],
        description: `Uploaded ${fileItem.file.name}`
      }));

      setDocuments(prev => [...prev, ...newDocuments]);
      
      return files.map(f => ({ ...f, status: 'completed' as const, progress: 100 }));
    } catch (error) {
      throw new Error('Upload failed');
    }
  };

  const handleDocumentClick = (document: UploadedDocument) => {
    if (allowMultipleSelection) {
      setSelectedDocuments(prev => {
        const isSelected = prev.some(d => d.id === document.id);
        if (isSelected) {
          return prev.filter(d => d.id !== document.id);
        } else {
          return [...prev, document];
        }
      });
    } else {
      setSelectedDocument(document);
      onDocumentSelect?.(document);
    }
  };

  const handleDownload = (document: UploadedDocument) => {
    const link = window.document.createElement('a');
    link.href = document.url;
    link.download = document.name;
    link.click();
  };

  const handleDelete = (documentId: string) => {
    setDocuments(prev => prev.filter(d => d.id !== documentId));
    if (selectedDocument?.id === documentId) {
      setSelectedDocument(null);
    }
    setSelectedDocuments(prev => prev.filter(d => d.id !== documentId));
  };

  const getFileIcon = (document: UploadedDocument) => {
    switch (document.category) {
      case 'code':
        return <Code className="h-5 w-5 text-blue-500" />;
      case 'image':
        return <Image className="h-5 w-5 text-green-500" />;
      case 'document':
        return <FileText className="h-5 w-5 text-orange-500" />;
      default:
        return <FileText className="h-5 w-5 text-gray-500" />;
    }
  };

  const formatFileSize = (bytes: number): string => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  };

  const filteredDocuments = documents.filter(doc => {
    const matchesSearch = doc.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
                         doc.description?.toLowerCase().includes(searchQuery.toLowerCase()) ||
                         doc.tags.some(tag => tag.toLowerCase().includes(searchQuery.toLowerCase()));
    
    const matchesCategory = categoryFilter === 'all' || doc.category === categoryFilter;
    const matchesFilterCategory = !filterByCategory || filterByCategory.includes(doc.category);
    
    return matchesSearch && matchesCategory && matchesFilterCategory;
  });

  const categories = [
    { value: 'all', label: 'All Files', count: documents.length },
    { value: 'code', label: 'Code Files', count: documents.filter(d => d.category === 'code').length },
    { value: 'document', label: 'Documents', count: documents.filter(d => d.category === 'document').length },
    { value: 'image', label: 'Images', count: documents.filter(d => d.category === 'image').length },
    { value: 'archive', label: 'Archives', count: documents.filter(d => d.category === 'archive').length }
  ];

  return (
    <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 h-full">
      {/* Upload and File List */}
      <div className="lg:col-span-2 space-y-6">
        {/* Upload Section */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Upload className="h-5 w-5" />
              Document Upload
            </CardTitle>
          </CardHeader>
          <CardContent>
            <FileUpload
              onFilesSelected={handleFilesSelected}
              onFileRemove={handleFileRemove}
              onUpload={handleUpload}
              acceptedTypes={acceptedTypes}
              maxFileSize={maxFileSize}
              maxFiles={10}
              multiple={true}
            />
          </CardContent>
        </Card>

        {/* Search and Filters */}
        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <CardTitle>Document Library</CardTitle>
              <div className="flex items-center gap-2">
                <Button
                  variant={viewMode === 'grid' ? 'default' : 'outline'}
                  size="sm"
                  onClick={() => setViewMode('grid')}
                >
                  <Grid className="h-4 w-4" />
                </Button>
                <Button
                  variant={viewMode === 'list' ? 'default' : 'outline'}
                  size="sm"
                  onClick={() => setViewMode('list')}
                >
                  <List className="h-4 w-4" />
                </Button>
              </div>
            </div>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <div className="flex gap-2">
                <div className="relative flex-1">
                  <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                  <Input
                    placeholder="Search documents..."
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                    className="pl-10"
                  />
                </div>
                <select
                  value={categoryFilter}
                  onChange={(e) => setCategoryFilter(e.target.value)}
                  className="px-3 py-2 border rounded-md text-sm"
                >
                  {categories.map(category => (
                    <option key={category.value} value={category.value}>
                      {category.label} ({category.count})
                    </option>
                  ))}
                </select>
              </div>

              <ScrollArea className="h-96">
                {viewMode === 'grid' ? (
                  <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
                    {filteredDocuments.map((document) => (
                      <div
                        key={document.id}
                        onClick={() => handleDocumentClick(document)}
                        className={`p-4 border rounded-lg cursor-pointer transition-colors hover:bg-gray-50 ${
                          selectedDocument?.id === document.id || selectedDocuments.some(d => d.id === document.id)
                            ? 'border-blue-500 bg-blue-50'
                            : 'border-gray-200'
                        }`}
                      >
                        <div className="flex items-start justify-between mb-2">
                          {getFileIcon(document)}
                          <div className="flex gap-1">
                            <Button
                              size="sm"
                              variant="ghost"
                              onClick={(e) => {
                                e.stopPropagation();
                                handleDownload(document);
                              }}
                            >
                              <Download className="h-3 w-3" />
                            </Button>
                            <Button
                              size="sm"
                              variant="ghost"
                              onClick={(e) => {
                                e.stopPropagation();
                                handleDelete(document.id);
                              }}
                            >
                              <Trash2 className="h-3 w-3" />
                            </Button>
                          </div>
                        </div>
                        <h4 className="font-medium text-sm truncate mb-1">{document.name}</h4>
                        <p className="text-xs text-gray-500 mb-2">{formatFileSize(document.size)}</p>
                        {document.description && (
                          <p className="text-xs text-gray-600 mb-2 line-clamp-2">{document.description}</p>
                        )}
                        <div className="flex flex-wrap gap-1">
                          {document.tags.slice(0, 2).map(tag => (
                            <Badge key={tag} variant="secondary" className="text-xs">
                              {tag}
                            </Badge>
                          ))}
                          {document.tags.length > 2 && (
                            <Badge variant="outline" className="text-xs">
                              +{document.tags.length - 2}
                            </Badge>
                          )}
                        </div>
                      </div>
                    ))}
                  </div>
                ) : (
                  <div className="space-y-2">
                    {filteredDocuments.map((document) => (
                      <div
                        key={document.id}
                        onClick={() => handleDocumentClick(document)}
                        className={`flex items-center gap-3 p-3 border rounded-lg cursor-pointer transition-colors hover:bg-gray-50 ${
                          selectedDocument?.id === document.id || selectedDocuments.some(d => d.id === document.id)
                            ? 'border-blue-500 bg-blue-50'
                            : 'border-gray-200'
                        }`}
                      >
                        {getFileIcon(document)}
                        <div className="flex-1 min-w-0">
                          <h4 className="font-medium text-sm truncate">{document.name}</h4>
                          <div className="flex items-center gap-2 text-xs text-gray-500">
                            <span>{formatFileSize(document.size)}</span>
                            <span>•</span>
                            <span>{document.uploadedAt.toLocaleDateString()}</span>
                          </div>
                        </div>
                        <div className="flex items-center gap-1">
                          {document.tags.slice(0, 2).map(tag => (
                            <Badge key={tag} variant="secondary" className="text-xs">
                              {tag}
                            </Badge>
                          ))}
                          <Button
                            size="sm"
                            variant="ghost"
                            onClick={(e) => {
                              e.stopPropagation();
                              handleDownload(document);
                            }}
                          >
                            <Download className="h-3 w-3" />
                          </Button>
                          <Button
                            size="sm"
                            variant="ghost"
                            onClick={(e) => {
                              e.stopPropagation();
                              handleDelete(document.id);
                            }}
                          >
                            <Trash2 className="h-3 w-3" />
                          </Button>
                        </div>
                      </div>
                    ))}
                  </div>
                )}
              </ScrollArea>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Preview Section */}
      <div className="lg:col-span-1">
        <Card className="h-full">
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Eye className="h-5 w-5" />
              Document Preview
            </CardTitle>
          </CardHeader>
          <CardContent className="h-full">
            {selectedDocument ? (
              <DocumentPreviewWrapper document={selectedDocument} />
            ) : (
              <div className="flex items-center justify-center h-64 text-center">
                <div>
                  <FileText className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                  <h3 className="text-lg font-medium text-gray-600 mb-2">No Document Selected</h3>
                  <p className="text-sm text-gray-500">
                    Click on a document to preview its contents
                  </p>
                </div>
              </div>
            )}
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
