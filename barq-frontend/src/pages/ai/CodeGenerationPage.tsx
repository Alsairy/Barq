import { useState, useEffect } from 'react';
import { FileText } from 'lucide-react';
import { ChatInterface, ChatMessage } from '../../components/chat/ChatInterface';
import { AIRequestCreation } from '../../components/ai/AIRequestCreation';
import { DocumentPreview } from '../../components/documents/DocumentPreview';
import { Button } from '../../components/ui/button';
import { devinApi } from '../../services/api';
import { fileService, FileUploadItem, FileMetadata } from '../../services/fileService';
import { cn } from '../../lib/utils';

interface CodeGenerationPageProps {
  className?: string;
}

export function CodeGenerationPage({ className }: CodeGenerationPageProps) {
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [showRequestCreation, setShowRequestCreation] = useState(false);
  const [currentSessionId, setCurrentSessionId] = useState<string | null>(null);
  const [previewFile, setPreviewFile] = useState<File | null>(null);
  const [previewUrl, setPreviewUrl] = useState<string>('');
  const [showPreview, setShowPreview] = useState(false);

  useEffect(() => {
    initializeSession();
  }, []);

  const initializeSession = async () => {
    try {
      const response = await devinApi.createChatSession({
        aiProvider: 'DevinAI',
        title: 'Code Generation Session',
        context: { sessionType: 'code-generation' }
      });
      setCurrentSessionId(response.data.sessionId);
      
      const welcomeMessage: ChatMessage = {
        id: 'welcome',
        content: `Hi! I'm Devin, your AI coding assistant. I can help you with:

• **Code Generation** - Build applications, components, and features
• **Architecture Design** - Plan and structure your codebase
• **Code Review** - Analyze and improve existing code
• **Testing** - Create comprehensive test suites
• **Documentation** - Generate technical documentation

Upload your files, describe what you'd like to build, or ask me any coding questions!`,
        sender: 'ai',
        timestamp: new Date(),
        codeBlocks: []
      };
      
      setMessages([welcomeMessage]);
    } catch (error) {
      console.error('Failed to initialize session:', error);
    }
  };

  const handleSendMessage = async (content: string, attachments?: FileUploadItem[]) => {
    if (!currentSessionId) return;

    const userMessage: ChatMessage = {
      id: `user-${Date.now()}`,
      content,
      sender: 'user',
      timestamp: new Date(),
      attachments: attachments?.map(file => ({
        id: file.id,
        fileName: file.file.name,
        fileSize: file.file.size,
        fileType: file.file.type,
        uploadedAt: new Date().toISOString(),
        uploadedBy: 'current-user',
        downloadUrl: '',
        previewUrl: file.preview
      }))
    };

    setMessages(prev => [...prev, userMessage]);
    setIsLoading(true);

    const typingMessage: ChatMessage = {
      id: `ai-typing-${Date.now()}`,
      content: '',
      sender: 'ai',
      timestamp: new Date(),
      isTyping: true
    };

    setMessages(prev => [...prev, typingMessage]);

    try {
      const attachmentIds = attachments?.map(file => file.id) || [];
      
      const response = await devinApi.sendMessage(currentSessionId, content, attachmentIds);

      if (response.success) {
        const aiMessage: ChatMessage = {
          id: response.data.messageId,
          content: response.data.response,
          sender: 'ai',
          timestamp: new Date(),
          codeBlocks: response.data.codeBlocks || [],
          attachments: response.data.attachments?.map((id: string) => ({
            id,
            fileName: `generated-${id}`,
            fileSize: 0,
            fileType: 'text/plain',
            uploadedAt: new Date().toISOString(),
            uploadedBy: 'DevinAI',
            downloadUrl: `/api/files/${id}/download`
          }))
        };

        setMessages(prev => prev.filter(m => !m.isTyping).concat([aiMessage]));
      } else {
        throw new Error('Failed to send message');
      }
    } catch (error) {
      const errorMessage: ChatMessage = {
        id: `error-${Date.now()}`,
        content: `I apologize, but I encountered an error: ${error instanceof Error ? error.message : 'Unknown error'}. Please try again.`,
        sender: 'ai',
        timestamp: new Date()
      };

      setMessages(prev => prev.filter(m => !m.isTyping).concat([errorMessage]));
    } finally {
      setIsLoading(false);
    }
  };

  const handleFileUpload = async (files: FileUploadItem[]): Promise<FileMetadata[]> => {
    try {
      const uploadResults = await fileService.uploadMultipleFiles(files);
      return uploadResults.map(result => ({
        id: result.data.fileId,
        fileName: result.data.fileName,
        fileSize: result.data.fileSize,
        fileType: result.data.fileType,
        uploadedAt: new Date().toISOString(),
        uploadedBy: 'current-user',
        downloadUrl: result.data.downloadUrl,
        previewUrl: result.data.previewUrl,
      }));
    } catch (error) {
      throw new Error('Failed to upload files');
    }
  };

  const handleRequestCreated = async (requestId: string) => {
    setShowRequestCreation(false);
    
    const successMessage: ChatMessage = {
      id: `request-created-${Date.now()}`,
      content: `✅ **AI Request Created Successfully!**

Request ID: \`${requestId}\`

Your code generation request has been submitted and will be processed through our workflow system. You can:

• Continue chatting here for immediate assistance
• Track the formal request progress in the AI Requests dashboard
• Receive notifications when the request is completed

How else can I help you with your coding project?`,
      sender: 'ai',
      timestamp: new Date()
    };

    setMessages(prev => [...prev, successMessage]);
  };

  const handleFilePreview = (file: File) => {
    setPreviewFile(file);
    setShowPreview(true);
  };

  const handleUrlPreview = (url: string) => {
    setPreviewUrl(url);
    setShowPreview(true);
  };

  const handleClosePreview = () => {
    setShowPreview(false);
    setPreviewFile(null);
    setPreviewUrl('');
  };

  const handleDownload = () => {
    if (previewFile) {
      const url = URL.createObjectURL(previewFile);
      const link = document.createElement('a');
      link.href = url;
      link.download = previewFile.name;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      URL.revokeObjectURL(url);
    } else if (previewUrl) {
      window.open(previewUrl, '_blank');
    }
  };

  if (showRequestCreation) {
    return (
      <div className={cn('h-full', className)}>
        <AIRequestCreation
          onRequestCreated={handleRequestCreated}
          onCancel={() => setShowRequestCreation(false)}
          initialRequestType="CodeGeneration"
        />
      </div>
    );
  }

  return (
    <div className={cn('h-full flex flex-col bg-gradient-to-br from-slate-50 to-blue-50', className)}>
      <div className="flex-shrink-0 border-b border-slate-200/50 bg-white/80 backdrop-blur-sm shadow-sm">
        <div className="flex items-center justify-between p-6">
          <div className="flex items-center space-x-4">
            <div className="w-12 h-12 bg-gradient-to-r from-blue-600 to-purple-600 rounded-xl flex items-center justify-center shadow-lg">
              <span className="text-white text-xl font-bold">D</span>
            </div>
            <div>
              <h1 className="text-2xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">
                Code Generation with Devin
              </h1>
              <p className="text-slate-600 mt-1">
                AI-powered development assistant for modern applications
              </p>
            </div>
          </div>
          
          <div className="flex items-center space-x-4">
            <Button
              variant="outline"
              size="sm"
              onClick={() => setShowRequestCreation(true)}
              className="bg-white/80 hover:bg-white border-slate-200 hover:border-blue-300 transition-all"
            >
              <FileText className="h-4 w-4 mr-2" />
              Create Formal Request
            </Button>
            
            <div className="flex items-center space-x-2 px-4 py-2 bg-gradient-to-r from-green-50 to-emerald-50 rounded-full border border-green-200/50 shadow-sm">
              <div className="w-2 h-2 bg-green-500 rounded-full animate-pulse" />
              <span className="text-sm font-medium text-green-700">
                DevinAI Online
              </span>
            </div>
          </div>
        </div>
      </div>

      <div className="flex-1 min-h-0">
        <ChatInterface
          messages={messages}
          onSendMessage={handleSendMessage}
          onFileUpload={handleFileUpload}
          onFilePreview={handleFilePreview}
          onUrlPreview={handleUrlPreview}
          isLoading={isLoading}
          placeholder="Describe what you'd like me to build, upload your files, or ask any coding questions..."
          aiProvider="DevinAI"
          className="h-full"
          sessionId={currentSessionId}
        />
      </div>

      {showPreview && (
        <DocumentPreview
          file={previewFile}
          fileUrl={previewUrl}
          onClose={handleClosePreview}
          onDownload={handleDownload}
        />
      )}
    </div>
  );
}
