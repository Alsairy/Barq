import { useState, useEffect } from 'react';
import { X, Download, Code, FileText, Image as ImageIcon } from 'lucide-react';
import { Button } from '../ui/button';
import { LoadingSpinner } from '../ui/loading-spinner';
import { fileService, FileMetadata } from '../../services/fileService';
import { cn } from '../../lib/utils';

interface FilePreviewProps {
  file: FileMetadata;
  isOpen: boolean;
  onClose: () => void;
  className?: string;
}

export function FilePreview({ file, isOpen, onClose, className }: FilePreviewProps) {
  const [previewContent, setPreviewContent] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (isOpen && file) {
      loadPreview();
    }
  }, [isOpen, file]);

  const loadPreview = async () => {
    setIsLoading(true);
    setError(null);
    
    try {
      if (fileService.isImageFile(file.fileName)) {
        setPreviewContent(file.previewUrl || file.downloadUrl);
      } else if (fileService.isCodeFile(file.fileName) || fileService.isDocumentFile(file.fileName)) {
        if (file.previewUrl) {
          setPreviewContent(file.previewUrl);
        } else {
          setPreviewContent(null);
        }
      } else {
        setPreviewContent(null);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load preview');
    } finally {
      setIsLoading(false);
    }
  };

  const handleDownload = async () => {
    try {
      const blob = await fileService.downloadFile(file.id);
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = file.fileName;
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
    } catch (err) {
      console.error('Download failed:', err);
    }
  };

  const getFileIcon = () => {
    if (fileService.isImageFile(file.fileName)) {
      return <ImageIcon className="h-6 w-6" />;
    } else if (fileService.isCodeFile(file.fileName)) {
      return <Code className="h-6 w-6" />;
    } else {
      return <FileText className="h-6 w-6" />;
    }
  };

  const renderPreviewContent = () => {
    if (isLoading) {
      return (
        <div className="flex items-center justify-center h-64">
          <LoadingSpinner className="h-8 w-8" />
        </div>
      );
    }

    if (error) {
      return (
        <div className="flex items-center justify-center h-64 text-red-500">
          <div className="text-center">
            <FileText className="h-12 w-12 mx-auto mb-2 opacity-50" />
            <p>{error}</p>
          </div>
        </div>
      );
    }

    if (!previewContent) {
      return (
        <div className="flex items-center justify-center h-64 text-gray-500 dark:text-gray-400">
          <div className="text-center">
            {getFileIcon()}
            <p className="mt-2">Preview not available</p>
            <p className="text-sm">Click download to view the file</p>
          </div>
        </div>
      );
    }

    if (fileService.isImageFile(file.fileName)) {
      return (
        <div className="flex items-center justify-center p-4">
          <img
            src={previewContent}
            alt={file.fileName}
            className="max-w-full max-h-96 object-contain rounded-lg"
          />
        </div>
      );
    }

    if (fileService.isCodeFile(file.fileName)) {
      return (
        <div className="p-4">
          <pre className="bg-gray-100 dark:bg-gray-800 p-4 rounded-lg overflow-auto max-h-96 text-sm">
            <code>{previewContent}</code>
          </pre>
        </div>
      );
    }

    return (
      <div className="p-4">
        <div className="bg-gray-100 dark:bg-gray-800 p-4 rounded-lg overflow-auto max-h-96">
          <div dangerouslySetInnerHTML={{ __html: previewContent }} />
        </div>
      </div>
    );
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
      <div className={cn(
        'bg-white dark:bg-gray-900 rounded-lg shadow-xl max-w-4xl max-h-[90vh] w-full mx-4 flex flex-col',
        className
      )}>
        <div className="flex items-center justify-between p-4 border-b border-gray-200 dark:border-gray-700">
          <div className="flex items-center space-x-3">
            {getFileIcon()}
            <div>
              <h3 className="text-lg font-semibold text-gray-900 dark:text-gray-100">
                {file.fileName}
              </h3>
              <p className="text-sm text-gray-500 dark:text-gray-400">
                {fileService.formatFileSize(file.fileSize)} • {file.fileType}
              </p>
            </div>
          </div>
          
          <div className="flex items-center space-x-2">
            <Button
              variant="outline"
              size="sm"
              onClick={handleDownload}
            >
              <Download className="h-4 w-4 mr-2" />
              Download
            </Button>
            <Button
              variant="ghost"
              size="sm"
              onClick={onClose}
            >
              <X className="h-4 w-4" />
            </Button>
          </div>
        </div>
        
        <div className="flex-1 overflow-auto">
          {renderPreviewContent()}
        </div>
        
        {file.description && (
          <div className="p-4 border-t border-gray-200 dark:border-gray-700">
            <p className="text-sm text-gray-600 dark:text-gray-300">
              {file.description}
            </p>
          </div>
        )}
      </div>
    </div>
  );
}
