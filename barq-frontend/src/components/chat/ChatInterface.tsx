import React, { useState, useRef, useEffect } from 'react';
import { Send, Paperclip, Code, User, Bot, Copy, Download, Eye } from 'lucide-react';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { vscDarkPlus } from 'react-syntax-highlighter/dist/esm/styles/prism';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { LoadingSpinner } from '../ui/loading-spinner';
import { FileUpload } from '../ui/file-upload';
import { FileUploadItem } from '../../services/fileService';
import { FilePreview } from '../files/FilePreview';
import { fileService, FileMetadata } from '../../services/fileService';
import { signalRService } from '../../services/signalRService';
import { cn } from '../../lib/utils';

export interface ChatMessage {
  id: string;
  content: string;
  sender: 'user' | 'ai';
  timestamp: Date;
  attachments?: FileMetadata[];
  codeBlocks?: CodeBlock[];
  isTyping?: boolean;
}

export interface CodeBlock {
  id: string;
  language: string;
  code: string;
  filename?: string;
}

interface ChatInterfaceProps {
  messages: ChatMessage[];
  onSendMessage: (content: string, attachments?: FileUploadItem[]) => void;
  onFileUpload: (files: FileUploadItem[]) => Promise<FileMetadata[]>;
  isLoading?: boolean;
  placeholder?: string;
  aiProvider?: string;
  className?: string;
  sessionId?: string | null;
  onFilePreview?: (file: File) => void;
  onUrlPreview?: (url: string) => void;
}

export function ChatInterface({
  messages,
  onSendMessage,
  onFileUpload,
  isLoading = false,
  placeholder = "Type your message or describe what you'd like me to help you with...",
  aiProvider = "DevinAI",
  className,
  sessionId
}: ChatInterfaceProps) {
  const [inputValue, setInputValue] = useState('');
  const [showFileUpload, setShowFileUpload] = useState(false);
  const [uploadedFiles, setUploadedFiles] = useState<FileUploadItem[]>([]);
  const [previewFile, setPreviewFile] = useState<FileMetadata | null>(null);
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  useEffect(() => {
    const initializeSignalR = async () => {
      try {
        await signalRService.connect();
        
        if (sessionId) {
          await signalRService.joinChatSession(sessionId);
          
          signalRService.onChatMessage(sessionId, (message) => {
            const chatMessage: ChatMessage = {
              id: message.messageId,
              content: message.content,
              sender: message.sender,
              timestamp: new Date(message.timestamp),
              codeBlocks: message.codeBlocks || [],
              attachments: message.attachments?.map(att => ({
                id: att.id,
                fileName: att.fileName,
                fileSize: att.fileSize,
                fileType: att.fileType,
                uploadedAt: new Date().toISOString(),
                uploadedBy: message.sender === 'ai' ? 'AI Assistant' : 'current-user',
                downloadUrl: att.downloadUrl,
                previewUrl: att.downloadUrl
              }))
            };
            
            console.log('Received real-time message:', chatMessage);
          });
        }
      } catch (error) {
        console.error('Failed to initialize SignalR:', error);
      }
    };

    initializeSignalR();

    return () => {
      if (sessionId) {
        signalRService.leaveChatSession(sessionId);
        signalRService.removeChatMessageHandler(sessionId);
      }
    };
  }, [sessionId]);

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  const handleSendMessage = async () => {
    if (!inputValue.trim() && uploadedFiles.length === 0) return;

    const messageContent = inputValue.trim();
    const attachments = uploadedFiles.length > 0 ? uploadedFiles : undefined;

    setInputValue('');
    setUploadedFiles([]);
    setShowFileUpload(false);

    onSendMessage(messageContent, attachments);
    
    setTimeout(() => {
      inputRef.current?.focus();
    }, 100);
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      handleSendMessage();
    }
  };

  const handleFileUploadComplete = async (files: FileUploadItem[]): Promise<FileUploadItem[]> => {
    try {
      await onFileUpload(files);
      const updatedFiles = files.map(file => ({
        ...file,
        status: 'completed' as const,
        progress: 100
      }));
      setUploadedFiles(updatedFiles);
      return updatedFiles;
    } catch (error) {
      console.error('File upload failed:', error);
      const failedFiles = files.map(file => ({
        ...file,
        status: 'error' as const,
        error: 'Upload failed'
      }));
      setUploadedFiles(failedFiles);
      return failedFiles;
    }
  };

  const handleFileRemove = (fileId: string) => {
    setUploadedFiles(prev => prev.filter(f => f.id !== fileId));
  };

  const copyToClipboard = async (text: string) => {
    try {
      await navigator.clipboard.writeText(text);
    } catch (err) {
      console.error('Failed to copy text:', err);
    }
  };

  const downloadCode = (codeBlock: CodeBlock) => {
    const blob = new Blob([codeBlock.code], { type: 'text/plain' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = codeBlock.filename || `code.${codeBlock.language}`;
    document.body.appendChild(a);
    a.click();
    URL.revokeObjectURL(url);
    document.body.removeChild(a);
  };

  const renderCodeBlock = (codeBlock: CodeBlock) => (
    <div key={codeBlock.id} className="my-4 rounded-lg border border-gray-200 dark:border-gray-700 overflow-hidden">
      <div className="flex items-center justify-between px-4 py-2 bg-gray-50 dark:bg-gray-800 border-b border-gray-200 dark:border-gray-700">
        <div className="flex items-center space-x-2">
          <Code className="h-4 w-4" />
          <span className="text-sm font-medium">{codeBlock.language}</span>
          {codeBlock.filename && (
            <span className="text-sm text-gray-500">• {codeBlock.filename}</span>
          )}
        </div>
        <div className="flex items-center space-x-2">
          <Button
            variant="ghost"
            size="sm"
            onClick={() => copyToClipboard(codeBlock.code)}
          >
            <Copy className="h-4 w-4" />
          </Button>
          <Button
            variant="ghost"
            size="sm"
            onClick={() => downloadCode(codeBlock)}
          >
            <Download className="h-4 w-4" />
          </Button>
        </div>
      </div>
      <SyntaxHighlighter
        language={codeBlock.language}
        style={vscDarkPlus}
        customStyle={{
          margin: 0,
          padding: '1rem',
          fontSize: '0.875rem',
          lineHeight: '1.5'
        }}
        showLineNumbers={true}
        wrapLines={true}
      >
        {codeBlock.code}
      </SyntaxHighlighter>
    </div>
  );

  const renderMessage = (message: ChatMessage) => (
    <div
      key={message.id}
      className={cn(
        'flex w-full mb-6',
        message.sender === 'user' ? 'justify-end' : 'justify-start'
      )}
    >
      <div
        className={cn(
          'flex max-w-[80%] space-x-3',
          message.sender === 'user' ? 'flex-row-reverse space-x-reverse' : 'flex-row'
        )}
      >
        <div
          className={cn(
            'flex-shrink-0 w-8 h-8 rounded-full flex items-center justify-center',
            message.sender === 'user'
              ? 'bg-blue-500 text-white'
              : 'bg-gray-200 dark:bg-gray-700 text-gray-600 dark:text-gray-300'
          )}
        >
          {message.sender === 'user' ? (
            <User className="h-4 w-4" />
          ) : (
            <Bot className="h-4 w-4" />
          )}
        </div>
        
        <div
          className={cn(
            'rounded-lg px-4 py-3 shadow-sm',
            message.sender === 'user'
              ? 'bg-blue-500 text-white'
              : 'bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700'
          )}
        >
          {message.isTyping ? (
            <div className="flex items-center space-x-2">
              <LoadingSpinner className="h-4 w-4" />
              <span className="text-sm">Thinking...</span>
            </div>
          ) : (
            <>
              <div className="whitespace-pre-wrap text-sm leading-relaxed">
                {message.content}
              </div>
              
              {message.codeBlocks?.map(renderCodeBlock)}
              
              {message.attachments && message.attachments.length > 0 && (
                <div className="mt-3 space-y-2">
                  <div className="text-xs opacity-75">Attachments:</div>
                  {message.attachments.map((file) => (
                    <div
                      key={file.id}
                      className="flex items-center space-x-2 p-2 bg-black bg-opacity-10 rounded cursor-pointer hover:bg-opacity-20"
                      onClick={() => setPreviewFile(file)}
                    >
                      <span className="text-sm">{fileService.getFileTypeIcon(file.fileName)}</span>
                      <span className="text-sm truncate">{file.fileName}</span>
                      <Eye className="h-3 w-3 ml-auto" />
                    </div>
                  ))}
                </div>
              )}
            </>
          )}
          
          <div className="text-xs opacity-50 mt-2">
            {message.timestamp.toLocaleTimeString()}
          </div>
        </div>
      </div>
    </div>
  );

  return (
    <div className={cn('flex flex-col h-full bg-white dark:bg-gray-900', className)}>
      <div className="flex-1 overflow-y-auto p-4 space-y-4">
        {messages.length === 0 ? (
          <div className="flex items-center justify-center h-full text-gray-500 dark:text-gray-400">
            <div className="text-center">
              <Bot className="h-12 w-12 mx-auto mb-4 opacity-50" />
              <h3 className="text-lg font-medium mb-2">Start a conversation with {aiProvider}</h3>
              <p className="text-sm">
                Upload files, ask questions, or describe what you'd like me to help you build.
              </p>
            </div>
          </div>
        ) : (
          messages.map(renderMessage)
        )}
        <div ref={messagesEndRef} />
      </div>

      {showFileUpload && (
        <div className="border-t border-gray-200 dark:border-gray-700 p-4">
          <FileUpload
            onFilesSelected={setUploadedFiles}
            onFileRemove={handleFileRemove}
            onUpload={handleFileUploadComplete}
            multiple={true}
            maxFiles={10}
            className="mb-4"
          />
        </div>
      )}

      <div className="border-t border-gray-200 dark:border-gray-700 p-4">
        <div className="flex items-end space-x-2">
          <Button
            variant="outline"
            size="sm"
            onClick={() => setShowFileUpload(!showFileUpload)}
            className={cn(
              'flex-shrink-0',
              showFileUpload && 'bg-blue-50 border-blue-200 text-blue-600'
            )}
          >
            <Paperclip className="h-4 w-4" />
          </Button>
          
          <div className="flex-1">
            <Input
              ref={inputRef}
              value={inputValue}
              onChange={(e) => setInputValue(e.target.value)}
              onKeyPress={handleKeyPress}
              placeholder={placeholder}
              disabled={isLoading}
              className="resize-none"
            />
          </div>
          
          <Button
            onClick={handleSendMessage}
            disabled={isLoading || (!inputValue.trim() && uploadedFiles.length === 0)}
            className="flex-shrink-0"
          >
            {isLoading ? (
              <LoadingSpinner className="h-4 w-4" />
            ) : (
              <Send className="h-4 w-4" />
            )}
          </Button>
        </div>
        
        {uploadedFiles.length > 0 && (
          <div className="mt-2 text-xs text-gray-500">
            {uploadedFiles.length} file{uploadedFiles.length > 1 ? 's' : ''} attached
          </div>
        )}
      </div>

      {previewFile && (
        <FilePreview
          file={previewFile}
          isOpen={!!previewFile}
          onClose={() => setPreviewFile(null)}
        />
      )}
    </div>
  );
}
