import React, { useCallback, useState, useRef } from 'react';
import { Upload, X, File, Image, Code, FileText, AlertCircle, CheckCircle } from 'lucide-react';
import { cn } from '../../lib/utils';
import { Button } from './button';
import { LoadingSpinner } from './loading-spinner';

import { FileUploadItem } from '../../services/fileService';

interface FileUploadProps {
  onFilesSelected: (files: FileUploadItem[]) => void;
  onFileRemove: (fileId: string) => void;
  onUpload: (files: FileUploadItem[]) => Promise<FileUploadItem[]>;
  acceptedTypes?: string[];
  maxFileSize?: number;
  maxFiles?: number;
  multiple?: boolean;
  disabled?: boolean;
  className?: string;
  children?: React.ReactNode;
}

const DEFAULT_ACCEPTED_TYPES = [
  '.js', '.jsx', '.ts', '.tsx', '.py', '.java', '.cpp', '.c', '.cs', '.php', '.rb', '.go', '.rs',
  '.html', '.css', '.scss', '.json', '.xml', '.yaml', '.yml', '.md', '.txt',
  '.pdf', '.doc', '.docx', '.xls', '.xlsx', '.ppt', '.pptx',
  '.png', '.jpg', '.jpeg', '.gif', '.svg', '.webp',
  '.zip', '.tar', '.gz'
];

const DEFAULT_MAX_FILE_SIZE = 50 * 1024 * 1024; // 50MB

export function FileUpload({
  onFilesSelected,
  onFileRemove,
  onUpload,
  acceptedTypes = DEFAULT_ACCEPTED_TYPES,
  maxFileSize = DEFAULT_MAX_FILE_SIZE,
  maxFiles = 10,
  multiple = true,
  disabled = false,
  className,
  children
}: FileUploadProps) {
  const [files, setFiles] = useState<FileUploadItem[]>([]);
  const [isDragOver, setIsDragOver] = useState(false);
  const [isUploading, setIsUploading] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const generateFileId = () => Math.random().toString(36).substr(2, 9);

  const getFileIcon = (file: File) => {
    const extension = file.name.split('.').pop()?.toLowerCase();
    
    if (['jpg', 'jpeg', 'png', 'gif', 'svg', 'webp'].includes(extension || '')) {
      return <Image className="h-4 w-4" />;
    }
    
    if (['js', 'jsx', 'ts', 'tsx', 'py', 'java', 'cpp', 'c', 'cs', 'php', 'rb', 'go', 'rs', 'html', 'css', 'scss'].includes(extension || '')) {
      return <Code className="h-4 w-4" />;
    }
    
    if (['pdf', 'doc', 'docx', 'txt', 'md'].includes(extension || '')) {
      return <FileText className="h-4 w-4" />;
    }
    
    return <File className="h-4 w-4" />;
  };

  const validateFile = (file: File): string | null => {
    if (file.size > maxFileSize) {
      return `File size exceeds ${Math.round(maxFileSize / 1024 / 1024)}MB limit`;
    }
    
    const extension = '.' + file.name.split('.').pop()?.toLowerCase();
    if (!acceptedTypes.includes(extension)) {
      return `File type ${extension} is not supported`;
    }
    
    return null;
  };

  const createFilePreview = async (file: File): Promise<string | undefined> => {
    if (file.type.startsWith('image/')) {
      return new Promise((resolve) => {
        const reader = new FileReader();
        reader.onload = (e) => resolve(e.target?.result as string);
        reader.readAsDataURL(file);
      });
    }
    return undefined;
  };

  const processFiles = async (fileList: FileList) => {
    const newFiles: FileUploadItem[] = [];
    
    for (let i = 0; i < fileList.length; i++) {
      const file = fileList[i];
      const error = validateFile(file);
      
      if (files.length + newFiles.length >= maxFiles) {
        break;
      }
      
      const preview = await createFilePreview(file);
      
      newFiles.push({
        id: generateFileId(),
        file,
        preview,
        progress: 0,
        status: error ? 'error' : 'pending',
        error: error || undefined
      });
    }
    
    const updatedFiles = [...files, ...newFiles];
    setFiles(updatedFiles);
    onFilesSelected(updatedFiles);
  };

  const handleDragOver = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    if (!disabled) {
      setIsDragOver(true);
    }
  }, [disabled]);

  const handleDragLeave = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragOver(false);
  }, []);

  const handleDrop = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragOver(false);
    
    if (disabled) return;
    
    const droppedFiles = e.dataTransfer.files;
    if (droppedFiles.length > 0) {
      processFiles(droppedFiles);
    }
  }, [disabled, files.length, maxFiles]);

  const handleFileSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
    const selectedFiles = e.target.files;
    if (selectedFiles && selectedFiles.length > 0) {
      processFiles(selectedFiles);
    }
    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  };

  const handleRemoveFile = (fileId: string) => {
    const updatedFiles = files.filter(f => f.id !== fileId);
    setFiles(updatedFiles);
    onFileRemove(fileId);
  };

  const handleUpload = async () => {
    const validFiles = files.filter(f => f.status === 'pending');
    if (validFiles.length === 0) return;
    
    setIsUploading(true);
    
    try {
      const updatedFiles = files.map(f => 
        f.status === 'pending' ? { ...f, status: 'uploading' as const } : f
      );
      setFiles(updatedFiles);
      
      await onUpload(validFiles);
      
      setFiles(prev => prev.map(f => 
        f.status === 'uploading' ? { ...f, status: 'completed', progress: 100 } : f
      ));
    } catch (error) {
      setFiles(prev => prev.map(f => 
        f.status === 'uploading' ? { 
          ...f, 
          status: 'error', 
          error: error instanceof Error ? error.message : 'Upload failed' 
        } : f
      ));
    } finally {
      setIsUploading(false);
    }
  };

  const openFileDialog = () => {
    if (!disabled && fileInputRef.current) {
      fileInputRef.current.click();
    }
  };

  return (
    <div className={cn('w-full', className)}>
      <div
        className={cn(
          'relative border-2 border-dashed rounded-lg p-6 transition-colors',
          isDragOver && !disabled
            ? 'border-blue-500 bg-blue-50 dark:bg-blue-950/20'
            : 'border-gray-300 dark:border-gray-600',
          disabled && 'opacity-50 cursor-not-allowed',
          !disabled && 'hover:border-gray-400 dark:hover:border-gray-500 cursor-pointer'
        )}
        onDragOver={handleDragOver}
        onDragLeave={handleDragLeave}
        onDrop={handleDrop}
        onClick={openFileDialog}
      >
        <input
          ref={fileInputRef}
          type="file"
          multiple={multiple}
          accept={acceptedTypes.join(',')}
          onChange={handleFileSelect}
          className="hidden"
          disabled={disabled}
        />
        
        {children || (
          <div className="text-center">
            <Upload className="mx-auto h-12 w-12 text-gray-400 mb-4" />
            <div className="text-lg font-medium text-gray-900 dark:text-gray-100 mb-2">
              Drop files here or click to browse
            </div>
            <div className="text-sm text-gray-500 dark:text-gray-400">
              Supports: {acceptedTypes.slice(0, 5).join(', ')}
              {acceptedTypes.length > 5 && ` and ${acceptedTypes.length - 5} more`}
            </div>
            <div className="text-xs text-gray-400 dark:text-gray-500 mt-1">
              Max file size: {Math.round(maxFileSize / 1024 / 1024)}MB
            </div>
          </div>
        )}
      </div>

      {files.length > 0 && (
        <div className="mt-4 space-y-2">
          <div className="flex items-center justify-between">
            <h4 className="text-sm font-medium text-gray-900 dark:text-gray-100">
              Selected Files ({files.length})
            </h4>
            {files.some(f => f.status === 'pending') && (
              <Button
                onClick={handleUpload}
                disabled={isUploading || disabled}
                size="sm"
              >
                {isUploading ? (
                  <>
                    <LoadingSpinner className="mr-2 h-4 w-4" />
                    Uploading...
                  </>
                ) : (
                  'Upload Files'
                )}
              </Button>
            )}
          </div>
          
          <div className="space-y-2 max-h-60 overflow-y-auto">
            {files.map((fileItem) => (
              <div
                key={fileItem.id}
                className="flex items-center space-x-3 p-3 bg-gray-50 dark:bg-gray-800 rounded-lg"
              >
                {fileItem.preview ? (
                  <img
                    src={fileItem.preview}
                    alt={fileItem.file.name}
                    className="h-10 w-10 object-cover rounded"
                  />
                ) : (
                  <div className="h-10 w-10 bg-gray-200 dark:bg-gray-700 rounded flex items-center justify-center">
                    {getFileIcon(fileItem.file)}
                  </div>
                )}
                
                <div className="flex-1 min-w-0">
                  <div className="text-sm font-medium text-gray-900 dark:text-gray-100 truncate">
                    {fileItem.file.name}
                  </div>
                  <div className="text-xs text-gray-500 dark:text-gray-400">
                    {(fileItem.file.size / 1024).toFixed(1)} KB
                  </div>
                  
                  {fileItem.status === 'uploading' && (
                    <div className="mt-1">
                      <div className="w-full bg-gray-200 dark:bg-gray-700 rounded-full h-1">
                        <div
                          className="bg-blue-500 h-1 rounded-full transition-all duration-300"
                          style={{ width: `${fileItem.progress}%` }}
                        />
                      </div>
                    </div>
                  )}
                  
                  {fileItem.error && (
                    <div className="text-xs text-red-500 mt-1">{fileItem.error}</div>
                  )}
                </div>
                
                <div className="flex items-center space-x-2">
                  {fileItem.status === 'completed' && (
                    <CheckCircle className="h-4 w-4 text-green-500" />
                  )}
                  {fileItem.status === 'error' && (
                    <AlertCircle className="h-4 w-4 text-red-500" />
                  )}
                  {fileItem.status === 'uploading' && (
                    <LoadingSpinner className="h-4 w-4" />
                  )}
                  
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={(e) => {
                      e.stopPropagation();
                      handleRemoveFile(fileItem.id);
                    }}
                    disabled={fileItem.status === 'uploading'}
                  >
                    <X className="h-4 w-4" />
                  </Button>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}
