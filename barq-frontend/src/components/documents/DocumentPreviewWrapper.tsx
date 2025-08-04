import React from 'react';
import { FileText, Download, ExternalLink, ZoomIn, ZoomOut, RotateCw } from 'lucide-react';
import { Button } from '../ui/button';
import { Badge } from '../ui/badge';
import { ScrollArea } from '../ui/scroll-area';

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

interface DocumentPreviewWrapperProps {
  document: UploadedDocument;
}

export function DocumentPreviewWrapper({ document }: DocumentPreviewWrapperProps) {
  const [content, setContent] = React.useState<string>('');
  const [isLoading, setIsLoading] = React.useState(false);
  const [error, setError] = React.useState<string | null>(null);
  const [zoom, setZoom] = React.useState(100);
  const [rotation, setRotation] = React.useState(0);

  React.useEffect(() => {
    loadDocumentContent();
  }, [document]);

  const loadDocumentContent = async () => {
    setIsLoading(true);
    setError(null);

    try {
      if (document.category === 'code' || document.type.startsWith('text/')) {
        const mockContent = generateMockContent(document);
        setContent(mockContent);
      }
    } catch (err) {
      setError('Failed to load document content');
    } finally {
      setIsLoading(false);
    }
  };

  const generateMockContent = (doc: UploadedDocument): string => {
    const extension = doc.name.split('.').pop()?.toLowerCase();
    
    if (extension === 'cs') {
      return `using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BARQ.Core.Entities;
using BARQ.Core.Services;

namespace BARQ.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}`;
    }
    
    if (extension === 'md') {
      return `# API Documentation

## Overview
This document provides comprehensive documentation for the BARQ API endpoints.

## Authentication
All API endpoints require authentication using JWT tokens.

### Login Endpoint
\`\`\`
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}
\`\`\`

## User Management

### Get All Users
\`\`\`
GET /api/users
Authorization: Bearer {token}
\`\`\``;
    }
    
    return `This is a preview of ${doc.name}.

The document contains ${doc.category} content and is available for download.

File Information:
- Name: ${doc.name}
- Category: ${doc.category}
- Type: ${doc.name.split('.').pop()?.toUpperCase()} file

To view the full content, please download the file.`;
  };

  const formatFileSize = (bytes: number): string => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  };

  const handleDownload = () => {
    const link = window.document.createElement('a');
    link.href = document.url;
    link.download = document.name;
    link.click();
  };

  const handleZoomIn = () => setZoom(prev => Math.min(prev + 25, 200));
  const handleZoomOut = () => setZoom(prev => Math.max(prev - 25, 50));
  const handleRotate = () => setRotation(prev => (prev + 90) % 360);

  const renderPreviewContent = () => {
    if (isLoading) {
      return (
        <div className="flex items-center justify-center h-64">
          <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-500"></div>
        </div>
      );
    }

    if (error) {
      return (
        <div className="flex items-center justify-center h-64 text-center">
          <div>
            <FileText className="h-12 w-12 text-red-400 mx-auto mb-4" />
            <h3 className="text-lg font-medium text-red-600 mb-2">Preview Error</h3>
            <p className="text-sm text-red-500">{error}</p>
          </div>
        </div>
      );
    }

    if (document.category === 'image') {
      return (
        <div className="text-center">
          <div className="relative inline-block">
            <img
              src={document.url}
              alt={document.name}
              className="max-w-full max-h-96 object-contain"
              style={{
                transform: `scale(${zoom / 100}) rotate(${rotation}deg)`,
                transition: 'transform 0.2s ease'
              }}
            />
          </div>
          <div className="flex items-center justify-center gap-2 mt-4">
            <Button size="sm" variant="outline" onClick={handleZoomOut}>
              <ZoomOut className="h-4 w-4" />
            </Button>
            <span className="text-sm text-gray-600">{zoom}%</span>
            <Button size="sm" variant="outline" onClick={handleZoomIn}>
              <ZoomIn className="h-4 w-4" />
            </Button>
            <Button size="sm" variant="outline" onClick={handleRotate}>
              <RotateCw className="h-4 w-4" />
            </Button>
          </div>
        </div>
      );
    }

    if (document.category === 'code' || document.type.startsWith('text/')) {
      const extension = document.name.split('.').pop()?.toLowerCase();
      return (
        <div className="space-y-4">
          <div className="flex items-center justify-between">
            <Badge variant="secondary">{extension?.toUpperCase()} File</Badge>
            <div className="flex gap-2">
              <Button size="sm" variant="outline" onClick={handleZoomOut}>
                <ZoomOut className="h-4 w-4" />
              </Button>
              <span className="text-sm text-gray-600 px-2 py-1">{zoom}%</span>
              <Button size="sm" variant="outline" onClick={handleZoomIn}>
                <ZoomIn className="h-4 w-4" />
              </Button>
            </div>
          </div>
          <ScrollArea className="h-96">
            <pre
              className="text-sm bg-gray-50 p-4 rounded-md overflow-x-auto"
              style={{ fontSize: `${zoom}%` }}
            >
              <code>{content}</code>
            </pre>
          </ScrollArea>
        </div>
      );
    }

    return (
      <div className="text-center space-y-4">
        <FileText className="h-16 w-16 text-gray-400 mx-auto" />
        <div>
          <h3 className="text-lg font-medium text-gray-900 mb-2">Preview Not Available</h3>
          <p className="text-sm text-gray-600 mb-4">
            This file type cannot be previewed. Download the file to view its contents.
          </p>
          <Button onClick={handleDownload}>
            <Download className="h-4 w-4 mr-2" />
            Download File
          </Button>
        </div>
      </div>
    );
  };

  return (
    <div className="space-y-4">
      {/* Document Info */}
      <div className="space-y-3">
        <div className="flex items-start justify-between">
          <div className="flex-1 min-w-0">
            <h3 className="font-medium text-lg truncate">{document.name}</h3>
            <div className="flex items-center gap-2 text-sm text-gray-500 mt-1">
              <span>{formatFileSize(document.size)}</span>
              <span>•</span>
              <span>{document.uploadedAt.toLocaleDateString()}</span>
            </div>
          </div>
          <div className="flex gap-1">
            <Button size="sm" variant="outline" onClick={handleDownload}>
              <Download className="h-4 w-4" />
            </Button>
            <Button size="sm" variant="outline" onClick={() => window.open(document.url, '_blank')}>
              <ExternalLink className="h-4 w-4" />
            </Button>
          </div>
        </div>

        {document.description && (
          <p className="text-sm text-gray-600">{document.description}</p>
        )}

        {document.metadata && (
          <div className="text-xs text-gray-500 space-y-1">
            {document.metadata.author && (
              <div>Author: {document.metadata.author}</div>
            )}
            {document.metadata.version && (
              <div>Version: {document.metadata.version}</div>
            )}
            {document.metadata.lastModified && (
              <div>Last Modified: {document.metadata.lastModified.toLocaleDateString()}</div>
            )}
          </div>
        )}

        {document.tags.length > 0 && (
          <div className="flex flex-wrap gap-1">
            {document.tags.map(tag => (
              <Badge key={tag} variant="secondary" className="text-xs">
                {tag}
              </Badge>
            ))}
          </div>
        )}
      </div>

      {/* Preview Content */}
      <div className="border rounded-lg p-4">
        {renderPreviewContent()}
      </div>
    </div>
  );
}
