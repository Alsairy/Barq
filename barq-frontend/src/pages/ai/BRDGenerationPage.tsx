import { useState, useEffect } from 'react';
import { FileText, Upload } from 'lucide-react';
import { ChatInterface, ChatMessage } from '../../components/chat/ChatInterface';
import { AIRequestCreation } from '../../components/ai/AIRequestCreation';
import { DocumentPreview } from '../../components/documents/DocumentPreview';
import { Button } from '../../components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '../../components/ui/card';
import { devinApi } from '../../services/api';
import { fileService, FileUploadItem, FileMetadata } from '../../services/fileService';
import { cn } from '../../lib/utils';

interface BRDGenerationPageProps {
  className?: string;
}

export function BRDGenerationPage({ className }: BRDGenerationPageProps) {
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
        aiProvider: 'ManusAI',
        title: 'BRD Generation Session',
        context: { sessionType: 'brd-generation' }
      });
      setCurrentSessionId(response.data.sessionId);
      
      const welcomeMessage: ChatMessage = {
        id: 'welcome',
        content: `Hello! I'm Manus, your AI business analyst. I specialize in creating comprehensive Business Requirements Documents (BRDs) and technical specifications.

I can help you with:

• **Business Requirements Documents** - Detailed functional and non-functional requirements
• **Technical Specifications** - System architecture and design documents  
• **User Stories & Acceptance Criteria** - Agile development requirements
• **Process Documentation** - Workflow and business process analysis
• **Stakeholder Analysis** - Requirements gathering and validation

Upload your project files, describe your business needs, or share existing documentation for analysis!`,
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
            fileName: `brd-document-${id}.pdf`,
            fileSize: 0,
            fileType: 'application/pdf',
            uploadedAt: new Date().toISOString(),
            uploadedBy: 'ManusAI',
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
        content: `I apologize, but I encountered an error: ${error instanceof Error ? error.message : 'Unknown error'}. Please try again or try uploading your files in a different format.`,
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
      content: `✅ **BRD Generation Request Created Successfully!**

Request ID: \`${requestId}\`

Your business requirements document request has been submitted and will be processed through our workflow system. You can:

• Continue our conversation here for immediate assistance
• Track the formal request progress in the AI Requests dashboard  
• Receive notifications when the BRD is completed and ready for download
• Review and approve the generated document through the workflow

What other business requirements would you like to discuss?`,
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
          initialRequestType="BRDGeneration"
        />
      </div>
    );
  }

  return (
    <div className={cn('h-full flex flex-col bg-gradient-to-br from-purple-50 to-pink-50', className)}>
      <div className="flex-shrink-0 border-b border-purple-200/50 bg-white/80 backdrop-blur-sm shadow-sm">
        <div className="flex items-center justify-between p-6">
          <div className="flex items-center space-x-4">
            <div className="w-12 h-12 bg-gradient-to-r from-purple-600 to-pink-600 rounded-xl flex items-center justify-center shadow-lg">
              <span className="text-white text-xl font-bold">M</span>
            </div>
            <div>
              <h1 className="text-2xl font-bold bg-gradient-to-r from-purple-600 to-pink-600 bg-clip-text text-transparent">
                BRD Generation with Manus
              </h1>
              <p className="text-slate-600 mt-1">
                AI-powered business analyst for comprehensive requirements documents
              </p>
            </div>
          </div>
          
          <div className="flex items-center space-x-4">
            <Button
              variant="outline"
              size="sm"
              onClick={() => setShowRequestCreation(true)}
              className="bg-white/80 hover:bg-white border-slate-200 hover:border-purple-300 transition-all"
            >
              <FileText className="h-4 w-4 mr-2" />
              Create Formal Request
            </Button>
            
            <div className="flex items-center space-x-2 px-4 py-2 bg-gradient-to-r from-green-50 to-emerald-50 rounded-full border border-green-200/50 shadow-sm">
              <div className="w-2 h-2 bg-green-500 rounded-full animate-pulse" />
              <span className="text-sm font-medium text-green-700">
                ManusAI Online
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
          placeholder="Describe your project requirements, upload existing documents, or ask about business analysis..."
          aiProvider="ManusAI"
          className="h-full"
          sessionId={currentSessionId}
        />
      </div>

      {messages.length <= 1 && (
        <div className="absolute inset-0 flex items-center justify-center pointer-events-none">
          <div className="max-w-2xl mx-auto p-8">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-8">
              <Card className="pointer-events-auto">
                <CardHeader className="pb-3">
                  <CardTitle className="flex items-center text-sm">
                    <FileText className="h-4 w-4 mr-2 text-green-500" />
                    Document Templates
                  </CardTitle>
                </CardHeader>
                <CardContent className="space-y-2">
                  <button
                    onClick={() => handleSendMessage("Create a BRD for an e-commerce platform with user management, product catalog, and payment processing")}
                    className="w-full text-left p-2 text-xs bg-gray-50 dark:bg-gray-800 rounded hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors"
                  >
                    "E-commerce Platform BRD"
                  </button>
                  <button
                    onClick={() => handleSendMessage("Generate user stories and acceptance criteria for a mobile app")}
                    className="w-full text-left p-2 text-xs bg-gray-50 dark:bg-gray-800 rounded hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors"
                  >
                    "Mobile App User Stories"
                  </button>
                  <button
                    onClick={() => handleSendMessage("Create technical specifications for API integration")}
                    className="w-full text-left p-2 text-xs bg-gray-50 dark:bg-gray-800 rounded hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors"
                  >
                    "API Integration Specs"
                  </button>
                </CardContent>
              </Card>

              <Card className="pointer-events-auto">
                <CardHeader className="pb-3">
                  <CardTitle className="flex items-center text-sm">
                    <Upload className="h-4 w-4 mr-2 text-blue-500" />
                    Upload Guidelines
                  </CardTitle>
                </CardHeader>
                <CardContent className="space-y-2 text-xs text-gray-600 dark:text-gray-400">
                  <div className="flex items-start space-x-2">
                    <div className="w-1 h-1 bg-gray-400 rounded-full mt-2 flex-shrink-0" />
                    <span>Upload existing requirements, wireframes, or project docs</span>
                  </div>
                  <div className="flex items-start space-x-2">
                    <div className="w-1 h-1 bg-gray-400 rounded-full mt-2 flex-shrink-0" />
                    <span>Supported formats: PDF, Word, Excel, PowerPoint</span>
                  </div>
                  <div className="flex items-start space-x-2">
                    <div className="w-1 h-1 bg-gray-400 rounded-full mt-2 flex-shrink-0" />
                    <span>Include stakeholder feedback and meeting notes</span>
                  </div>
                  <div className="flex items-start space-x-2">
                    <div className="w-1 h-1 bg-gray-400 rounded-full mt-2 flex-shrink-0" />
                    <span>Generated BRDs will be available for download</span>
                  </div>
                </CardContent>
              </Card>
            </div>
          </div>
        </div>
      )}

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
