import React, { useState, useEffect, useRef } from 'react';
import { Send, Paperclip, Copy, Check, Bot, User, FileText, Terminal, Loader2, Plus, X } from 'lucide-react';
import { Button } from '../ui/button';
import { Badge } from '../ui/badge';
import { ScrollArea } from '../ui/scroll-area';
import { Textarea } from '../ui/textarea';

interface ChatMessage {
  id: string;
  type: 'user' | 'ai' | 'system';
  content: string;
  timestamp: Date;
  attachments?: FileAttachment[];
  codeSnippets?: CodeSnippet[];
  isStreaming?: boolean;
  metadata?: {
    model?: string;
    tokens?: number;
    executionTime?: number;
  };
}

interface FileAttachment {
  id: string;
  name: string;
  type: string;
  size: number;
  url: string;
}

interface CodeSnippet {
  id: string;
  language: string;
  code: string;
  filename?: string;
}

interface ChatSession {
  id: string;
  name: string;
  messages: ChatMessage[];
  createdAt: Date;
  lastActivity: Date;
  context?: string;
}

interface AgentConfig {
  id: string;
  name: string;
  description: string;
  capabilities: string[];
  model: string;
  temperature: number;
  maxTokens: number;
}

export function DevinChatInterface() {
  const [sessions, setSessions] = useState<ChatSession[]>([]);
  const [currentSession, setCurrentSession] = useState<ChatSession | null>(null);
  const [inputValue, setInputValue] = useState('');
  const [isTyping, setIsTyping] = useState(false);
  const [selectedAgent, setSelectedAgent] = useState<AgentConfig>({
    id: 'devin-code-generation',
    name: 'Devin Code Generation',
    description: 'AI-powered code generation with full-stack capabilities',
    capabilities: ['Code Generation', 'Architecture Design', 'Testing', 'Documentation'],
    model: 'gpt-4-turbo',
    temperature: 0.1,
    maxTokens: 4000
  });
  const [attachments, setAttachments] = useState<FileAttachment[]>([]);
  const [copiedMessageId, setCopiedMessageId] = useState<string | null>(null);
  const [isExpanded, setIsExpanded] = useState(false);
  
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const availableAgents: AgentConfig[] = [
    {
      id: 'devin-code-generation',
      name: 'Devin Code Generation',
      description: 'AI-powered code generation with full-stack capabilities',
      capabilities: ['Code Generation', 'Architecture Design', 'Testing', 'Documentation'],
      model: 'gpt-4-turbo',
      temperature: 0.1,
      maxTokens: 4000
    },
    {
      id: 'devin-brd-generation',
      name: 'Devin BRD Generation',
      description: 'Business Requirements Document generation and analysis',
      capabilities: ['BRD Creation', 'Requirements Analysis', 'Stakeholder Management'],
      model: 'gpt-4',
      temperature: 0.2,
      maxTokens: 3000
    },
    {
      id: 'devin-proposal-generation',
      name: 'Devin Proposal Generation',
      description: 'Technical and financial proposal creation',
      capabilities: ['Proposal Writing', 'Technical Specifications', 'Financial Modeling'],
      model: 'gpt-4',
      temperature: 0.3,
      maxTokens: 3500
    },
    {
      id: 'devin-presentation-generation',
      name: 'Devin Presentation Generation',
      description: 'Professional presentation creation and design',
      capabilities: ['Slide Creation', 'Visual Design', 'Content Structuring'],
      model: 'gpt-4',
      temperature: 0.4,
      maxTokens: 2500
    }
  ];

  useEffect(() => {
    loadSessions();
  }, []);

  useEffect(() => {
    scrollToBottom();
  }, [currentSession?.messages]);

  const loadSessions = async () => {
    const mockSessions: ChatSession[] = [
      {
        id: '1',
        name: 'Code Generation Project',
        messages: [
          {
            id: '1',
            type: 'system',
            content: 'Welcome to Devin Code Generation! I can help you create high-quality code following best practices and your organization\'s standards.',
            timestamp: new Date(Date.now() - 3600000)
          }
        ],
        createdAt: new Date(Date.now() - 3600000),
        lastActivity: new Date(Date.now() - 1800000),
        context: 'Full-stack development with .NET 8 and React'
      }
    ];
    setSessions(mockSessions);
    if (mockSessions.length > 0) {
      setCurrentSession(mockSessions[0]);
    }
  };

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  const handleSendMessage = async () => {
    if (!inputValue.trim() && attachments.length === 0) return;
    if (!currentSession) return;

    const userMessage: ChatMessage = {
      id: Date.now().toString(),
      type: 'user',
      content: inputValue,
      timestamp: new Date(),
      attachments: attachments.length > 0 ? [...attachments] : undefined
    };

    const updatedSession = {
      ...currentSession,
      messages: [...currentSession.messages, userMessage],
      lastActivity: new Date()
    };

    setCurrentSession(updatedSession);
    setSessions(sessions.map(s => s.id === currentSession.id ? updatedSession : s));
    setInputValue('');
    setAttachments([]);
    setIsTyping(true);

    setTimeout(() => {
      const aiResponse: ChatMessage = {
        id: (Date.now() + 1).toString(),
        type: 'ai',
        content: generateContextualResponse(inputValue, selectedAgent),
        timestamp: new Date(),
        metadata: {
          model: selectedAgent.model,
          tokens: Math.floor(Math.random() * 1000) + 500,
          executionTime: Math.floor(Math.random() * 3000) + 1000
        }
      };

      const finalSession = {
        ...updatedSession,
        messages: [...updatedSession.messages, aiResponse]
      };

      setCurrentSession(finalSession);
      setSessions(sessions.map(s => s.id === currentSession.id ? finalSession : s));
      setIsTyping(false);
    }, 2000);
  };

  const generateContextualResponse = (input: string, agent: AgentConfig): string => {
    const lowerInput = input.toLowerCase();
    
    if (agent.id === 'devin-code-generation') {
      if (lowerInput.includes('react') || lowerInput.includes('component')) {
        return `I'll help you create a React component. Based on your requirements, I'll generate a modern, TypeScript-based component following best practices:

\`\`\`typescript
import React, { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from './ui/card';
import { Button } from './ui/button';

interface MyComponentProps {
  title: string;
  onAction?: () => void;
}

export function MyComponent({ title, onAction }: MyComponentProps) {
  const [isActive, setIsActive] = useState(false);

  return (
    <Card className="w-full max-w-md">
      <CardHeader>
        <CardTitle>{title}</CardTitle>
      </CardHeader>
      <CardContent>
        <Button 
          onClick={() => {
            setIsActive(!isActive);
            onAction?.();
          }}
          variant={isActive ? "default" : "outline"}
        >
          {isActive ? 'Active' : 'Inactive'}
        </Button>
      </CardContent>
    </Card>
  );
}
\`\`\`

This component includes:
- TypeScript interfaces for type safety
- Modern React hooks (useState)
- Proper prop handling with optional callbacks
- Consistent styling with your UI library
- Accessible button states

Would you like me to add any specific functionality or modify the styling?`;
      }
      
      if (lowerInput.includes('.net') || lowerInput.includes('api') || lowerInput.includes('controller')) {
        return `I'll create a .NET 8 API controller for you. Here's a comprehensive implementation following clean architecture principles:

\`\`\`csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MyController : ControllerBase
{
    private readonly IMyService _myService;
    private readonly ILogger<MyController> _logger;

    public MyController(IMyService myService, ILogger<MyController> logger)
    {
        _myService = myService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<MyDto>>>> GetAll()
    {
        try
        {
            var result = await _myService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<MyDto>>.Success(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving items");
            return StatusCode(500, ApiResponse<IEnumerable<MyDto>>.Error("Internal server error"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<MyDto>>> Create([FromBody] CreateMyRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<MyDto>.Error("Invalid request data"));

        try
        {
            var result = await _myService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, 
                ApiResponse<MyDto>.Success(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating item");
            return StatusCode(500, ApiResponse<MyDto>.Error("Internal server error"));
        }
    }
}
\`\`\`

This controller includes:
- Proper dependency injection
- Comprehensive error handling
- Structured API responses
- Authorization attributes
- Async/await patterns
- Logging integration

Would you like me to add specific endpoints or modify the error handling strategy?`;
      }
    }

    return `I understand you're working on "${input}". As ${agent.name}, I'm ready to help you with ${agent.capabilities.join(', ').toLowerCase()}. 

Let me know more details about what you'd like to accomplish, and I'll provide you with:
- Detailed implementation guidance
- Code examples and best practices
- Architecture recommendations
- Testing strategies

What specific aspect would you like to focus on first?`;
  };

  const handleFileAttachment = (event: React.ChangeEvent<HTMLInputElement>) => {
    const files = event.target.files;
    if (!files) return;

    Array.from(files).forEach(file => {
      const attachment: FileAttachment = {
        id: Date.now().toString() + Math.random().toString(36).substr(2, 9),
        name: file.name,
        type: file.type,
        size: file.size,
        url: URL.createObjectURL(file)
      };
      setAttachments(prev => [...prev, attachment]);
    });

    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  };

  const removeAttachment = (attachmentId: string) => {
    setAttachments(prev => prev.filter(a => a.id !== attachmentId));
  };

  const copyMessage = async (content: string, messageId: string) => {
    try {
      await navigator.clipboard.writeText(content);
      setCopiedMessageId(messageId);
      setTimeout(() => setCopiedMessageId(null), 2000);
    } catch (err) {
      console.error('Failed to copy message:', err);
    }
  };

  const createNewSession = () => {
    const newSession: ChatSession = {
      id: Date.now().toString(),
      name: `New Chat ${sessions.length + 1}`,
      messages: [
        {
          id: '1',
          type: 'system',
          content: `Welcome to ${selectedAgent.name}! I'm ready to help you with ${selectedAgent.capabilities.join(', ').toLowerCase()}.`,
          timestamp: new Date()
        }
      ],
      createdAt: new Date(),
      lastActivity: new Date(),
      context: selectedAgent.description
    };

    setSessions([newSession, ...sessions]);
    setCurrentSession(newSession);
  };

  const formatTimestamp = (timestamp: Date) => {
    return timestamp.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  };

  const renderMessage = (message: ChatMessage) => {
    const isUser = message.type === 'user';
    const isSystem = message.type === 'system';

    return (
      <div key={message.id} className={`flex ${isUser ? 'justify-end' : 'justify-start'} mb-4`}>
        <div className={`max-w-[85%] ${isUser ? 'order-2' : 'order-1'}`}>
          <div className={`flex items-start gap-3 ${isUser ? 'flex-row-reverse' : 'flex-row'}`}>
            <div className={`flex-shrink-0 w-8 h-8 rounded-full flex items-center justify-center ${
              isUser ? 'bg-blue-500' : isSystem ? 'bg-gray-500' : 'bg-green-500'
            }`}>
              {isUser ? <User className="w-4 h-4 text-white" /> : 
               isSystem ? <Terminal className="w-4 h-4 text-white" /> :
               <Bot className="w-4 h-4 text-white" />}
            </div>
            
            <div className={`flex-1 ${isUser ? 'text-right' : 'text-left'}`}>
              <div className={`inline-block p-4 rounded-lg ${
                isUser ? 'bg-blue-500 text-white' : 
                isSystem ? 'bg-gray-100 text-gray-800 border' :
                'bg-white text-gray-900 border shadow-sm'
              }`}>
                <div className="prose prose-sm max-w-none">
                  {message.content.includes('```') ? (
                    <div className="space-y-2">
                      {message.content.split('```').map((part, index) => {
                        if (index % 2 === 0) {
                          return <div key={index} className="whitespace-pre-wrap">{part}</div>;
                        } else {
                          const lines = part.split('\n');
                          const language = lines[0];
                          const code = lines.slice(1).join('\n');
                          return (
                            <div key={index} className="relative">
                              <div className="bg-gray-900 text-gray-100 p-4 rounded-md overflow-x-auto">
                                <div className="flex items-center justify-between mb-2">
                                  <span className="text-xs text-gray-400">{language}</span>
                                  <Button
                                    size="sm"
                                    variant="ghost"
                                    className="h-6 px-2 text-gray-400 hover:text-white"
                                    onClick={() => copyMessage(code, message.id)}
                                  >
                                    {copiedMessageId === message.id ? 
                                      <Check className="w-3 h-3" /> : 
                                      <Copy className="w-3 h-3" />
                                    }
                                  </Button>
                                </div>
                                <pre className="text-sm"><code>{code}</code></pre>
                              </div>
                            </div>
                          );
                        }
                      })}
                    </div>
                  ) : (
                    <div className="whitespace-pre-wrap">{message.content}</div>
                  )}
                </div>
                
                {message.attachments && message.attachments.length > 0 && (
                  <div className="mt-3 space-y-2">
                    {message.attachments.map(attachment => (
                      <div key={attachment.id} className="flex items-center gap-2 p-2 bg-gray-50 rounded">
                        <FileText className="w-4 h-4" />
                        <span className="text-sm">{attachment.name}</span>
                        <span className="text-xs text-gray-500">
                          ({(attachment.size / 1024).toFixed(1)} KB)
                        </span>
                      </div>
                    ))}
                  </div>
                )}
              </div>
              
              <div className={`flex items-center gap-2 mt-1 text-xs text-gray-500 ${
                isUser ? 'justify-end' : 'justify-start'
              }`}>
                <span>{formatTimestamp(message.timestamp)}</span>
                {message.metadata && (
                  <>
                    <span>•</span>
                    <span>{message.metadata.model}</span>
                    {message.metadata.tokens && (
                      <>
                        <span>•</span>
                        <span>{message.metadata.tokens} tokens</span>
                      </>
                    )}
                  </>
                )}
                {!isUser && (
                  <Button
                    size="sm"
                    variant="ghost"
                    className="h-4 px-1 ml-2"
                    onClick={() => copyMessage(message.content, message.id)}
                  >
                    {copiedMessageId === message.id ? 
                      <Check className="w-3 h-3" /> : 
                      <Copy className="w-3 h-3" />
                    }
                  </Button>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  };

  return (
    <div className={`flex h-full ${isExpanded ? 'fixed inset-0 z-50 bg-white' : 'relative'}`}>
      {/* Sidebar */}
      <div className="w-80 border-r bg-gray-50 flex flex-col">
        <div className="p-4 border-b">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-lg font-semibold">Devin Assistant</h2>
            <Button size="sm" onClick={createNewSession}>
              <Plus className="w-4 h-4 mr-1" />
              New Chat
            </Button>
          </div>
          
          <div className="space-y-2">
            <label className="text-sm font-medium">Agent</label>
            <select
              value={selectedAgent.id}
              onChange={(e) => {
                const agent = availableAgents.find(a => a.id === e.target.value);
                if (agent) setSelectedAgent(agent);
              }}
              className="w-full p-2 border rounded-md text-sm"
            >
              {availableAgents.map(agent => (
                <option key={agent.id} value={agent.id}>{agent.name}</option>
              ))}
            </select>
          </div>
        </div>

        <ScrollArea className="flex-1">
          <div className="p-2 space-y-2">
            {sessions.map(session => (
              <div
                key={session.id}
                onClick={() => setCurrentSession(session)}
                className={`p-3 rounded-lg cursor-pointer transition-colors ${
                  currentSession?.id === session.id 
                    ? 'bg-blue-100 border-blue-200 border' 
                    : 'bg-white hover:bg-gray-100 border'
                }`}
              >
                <div className="font-medium text-sm truncate">{session.name}</div>
                <div className="text-xs text-gray-500 mt-1">
                  {session.messages.length} messages • {formatTimestamp(session.lastActivity)}
                </div>
                {session.context && (
                  <div className="text-xs text-gray-400 mt-1 truncate">{session.context}</div>
                )}
              </div>
            ))}
          </div>
        </ScrollArea>
      </div>

      {/* Main Chat Area */}
      <div className="flex-1 flex flex-col">
        {currentSession ? (
          <>
            <div className="p-4 border-b bg-white">
              <div className="flex items-center justify-between">
                <div>
                  <h3 className="font-semibold">{currentSession.name}</h3>
                  <div className="flex items-center gap-2 mt-1">
                    <Badge variant="secondary">{selectedAgent.name}</Badge>
                    <span className="text-sm text-gray-500">
                      {selectedAgent.capabilities.join(' • ')}
                    </span>
                  </div>
                </div>
                <Button
                  size="sm"
                  variant="ghost"
                  onClick={() => setIsExpanded(!isExpanded)}
                >
                  {isExpanded ? <X className="w-4 h-4" /> : <Terminal className="w-4 h-4" />}
                </Button>
              </div>
            </div>

            <ScrollArea className="flex-1 p-4">
              <div className="max-w-4xl mx-auto">
                {currentSession.messages.map(renderMessage)}
                
                {isTyping && (
                  <div className="flex justify-start mb-4">
                    <div className="flex items-start gap-3">
                      <div className="flex-shrink-0 w-8 h-8 rounded-full bg-green-500 flex items-center justify-center">
                        <Bot className="w-4 h-4 text-white" />
                      </div>
                      <div className="bg-white border shadow-sm p-4 rounded-lg">
                        <div className="flex items-center gap-2">
                          <Loader2 className="w-4 h-4 animate-spin" />
                          <span className="text-sm text-gray-600">Devin is thinking...</span>
                        </div>
                      </div>
                    </div>
                  </div>
                )}
                
                <div ref={messagesEndRef} />
              </div>
            </ScrollArea>

            <div className="p-4 border-t bg-white">
              {attachments.length > 0 && (
                <div className="mb-3 flex flex-wrap gap-2">
                  {attachments.map(attachment => (
                    <div key={attachment.id} className="flex items-center gap-2 bg-gray-100 px-3 py-1 rounded-full text-sm">
                      <FileText className="w-4 h-4" />
                      <span>{attachment.name}</span>
                      <Button
                        size="sm"
                        variant="ghost"
                        className="h-4 w-4 p-0"
                        onClick={() => removeAttachment(attachment.id)}
                      >
                        <X className="w-3 h-3" />
                      </Button>
                    </div>
                  ))}
                </div>
              )}

              <div className="flex gap-2">
                <div className="flex-1">
                  <Textarea
                    value={inputValue}
                    onChange={(e) => setInputValue(e.target.value)}
                    placeholder={`Message ${selectedAgent.name}...`}
                    onKeyDown={(e) => {
                      if (e.key === 'Enter' && !e.shiftKey) {
                        e.preventDefault();
                        handleSendMessage();
                      }
                    }}
                    className="min-h-[60px] resize-none"
                  />
                </div>
                <div className="flex flex-col gap-2">
                  <input
                    ref={fileInputRef}
                    type="file"
                    multiple
                    onChange={handleFileAttachment}
                    className="hidden"
                    accept=".txt,.md,.js,.ts,.jsx,.tsx,.py,.java,.cs,.cpp,.c,.h,.css,.html,.json,.xml,.yaml,.yml,.sql,.sh,.bat,.ps1,.pdf,.doc,.docx,.png,.jpg,.jpeg,.gif,.svg"
                  />
                  <Button
                    size="sm"
                    variant="outline"
                    onClick={() => fileInputRef.current?.click()}
                    disabled={isTyping}
                  >
                    <Paperclip className="w-4 h-4" />
                  </Button>
                  <Button
                    size="sm"
                    onClick={handleSendMessage}
                    disabled={(!inputValue.trim() && attachments.length === 0) || isTyping}
                  >
                    <Send className="w-4 h-4" />
                  </Button>
                </div>
              </div>
            </div>
          </>
        ) : (
          <div className="flex-1 flex items-center justify-center">
            <div className="text-center">
              <Bot className="w-16 h-16 text-gray-400 mx-auto mb-4" />
              <h3 className="text-lg font-semibold text-gray-600 mb-2">Welcome to Devin Assistant</h3>
              <p className="text-gray-500 mb-4">Select a chat session or create a new one to get started</p>
              <Button onClick={createNewSession}>
                <Plus className="w-4 h-4 mr-2" />
                Start New Chat
              </Button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
