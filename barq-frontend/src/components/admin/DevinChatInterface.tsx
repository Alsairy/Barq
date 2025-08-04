import React, { useState, useEffect, useRef } from 'react';
import { Send, Paperclip, Code, Copy, Check, Bot, User } from 'lucide-react';
import { Card, CardContent } from '../ui/card';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Badge } from '../ui/badge';
import { ScrollArea } from '../ui/scroll-area';

interface ChatMessage {
  id: string;
  type: 'user' | 'assistant';
  content: string;
  timestamp: Date;
  attachments?: ChatAttachment[];
  status?: 'sending' | 'sent' | 'error';
}

interface ChatAttachment {
  id: string;
  name: string;
  type: string;
  size: number;
  url?: string;
}

interface DevinSession {
  id: string;
  title: string;
  status: 'active' | 'completed' | 'paused';
  messages: ChatMessage[];
  createdAt: Date;
  lastActivity: Date;
}

export function DevinChatInterface() {
  const [sessions, setSessions] = useState<DevinSession[]>([]);
  const [activeSession, setActiveSession] = useState<DevinSession | null>(null);
  const [message, setMessage] = useState('');
  const [isTyping, setIsTyping] = useState(false);
  const [attachments, setAttachments] = useState<File[]>([]);
  const [copiedMessageId, setCopiedMessageId] = useState<string | null>(null);
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    loadSessions();
  }, []);

  useEffect(() => {
    scrollToBottom();
  }, [activeSession?.messages]);

  const loadSessions = async () => {
    try {
      const mockSessions: DevinSession[] = [
        {
          id: '1',
          title: 'Code Generation Configuration',
          status: 'active',
          messages: [
            {
              id: '1',
              type: 'user',
              content: 'I need help setting up a .NET 8 code generation configuration with clean architecture patterns.',
              timestamp: new Date(Date.now() - 3600000),
              status: 'sent'
            },
            {
              id: '2',
              type: 'assistant',
              content: 'I\'ll help you set up a comprehensive .NET 8 code generation configuration. Let me create a configuration that includes:\n\n1. **Technology Stack**: .NET 8, ASP.NET Core, Entity Framework Core\n2. **Architecture**: Clean Architecture with dependency inversion\n3. **Patterns**: Repository pattern, CQRS, MediatR\n4. **Quality Standards**: 80% code coverage, proper naming conventions\n\nWould you like me to proceed with creating this configuration?',
              timestamp: new Date(Date.now() - 3500000),
              status: 'sent'
            }
          ],
          createdAt: new Date(Date.now() - 3600000),
          lastActivity: new Date(Date.now() - 3500000)
        },
        {
          id: '2',
          title: 'BRD Template Setup',
          status: 'completed',
          messages: [
            {
              id: '3',
              type: 'user',
              content: 'Create a BRD template for enterprise software projects with agile methodology.',
              timestamp: new Date(Date.now() - 7200000),
              status: 'sent'
            },
            {
              id: '4',
              type: 'assistant',
              content: 'I\'ve created a comprehensive BRD template with the following sections:\n\n✅ Executive Summary\n✅ Business Objectives\n✅ Functional Requirements\n✅ User Stories with Acceptance Criteria\n✅ Sprint Planning Structure\n✅ Stakeholder Analysis\n\nThe template is now available in your BRD configuration settings.',
              timestamp: new Date(Date.now() - 7000000),
              status: 'sent'
            }
          ],
          createdAt: new Date(Date.now() - 7200000),
          lastActivity: new Date(Date.now() - 7000000)
        }
      ];
      setSessions(mockSessions);
      if (mockSessions.length > 0) {
        setActiveSession(mockSessions[0]);
      }
    } catch (error) {
      console.error('Failed to load sessions:', error);
    }
  };

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  const handleSendMessage = async () => {
    if (!message.trim() && attachments.length === 0) return;
    if (!activeSession) return;

    const newMessage: ChatMessage = {
      id: Date.now().toString(),
      type: 'user',
      content: message,
      timestamp: new Date(),
      attachments: attachments.map(file => ({
        id: Date.now().toString(),
        name: file.name,
        type: file.type,
        size: file.size
      })),
      status: 'sending'
    };

    const updatedSession = {
      ...activeSession,
      messages: [...activeSession.messages, newMessage],
      lastActivity: new Date()
    };
    setActiveSession(updatedSession);
    setSessions(sessions.map(s => s.id === activeSession.id ? updatedSession : s));

    setMessage('');
    setAttachments([]);

    setIsTyping(true);
    setTimeout(() => {
      const aiResponse: ChatMessage = {
        id: (Date.now() + 1).toString(),
        type: 'assistant',
        content: 'I understand your request. Let me analyze the requirements and create the appropriate configuration for you. This may take a few moments...',
        timestamp: new Date(),
        status: 'sent'
      };

      const finalSession = {
        ...updatedSession,
        messages: [...updatedSession.messages.map(m => 
          m.id === newMessage.id ? { ...m, status: 'sent' as const } : m
        ), aiResponse],
        lastActivity: new Date()
      };
      setActiveSession(finalSession);
      setSessions(sessions.map(s => s.id === activeSession.id ? finalSession : s));
      setIsTyping(false);
    }, 2000);
  };

  const handleFileAttachment = (event: React.ChangeEvent<HTMLInputElement>) => {
    const files = Array.from(event.target.files || []);
    setAttachments([...attachments, ...files]);
  };

  const removeAttachment = (index: number) => {
    setAttachments(attachments.filter((_, i) => i !== index));
  };

  const copyMessage = async (messageId: string, content: string) => {
    try {
      await navigator.clipboard.writeText(content);
      setCopiedMessageId(messageId);
      setTimeout(() => setCopiedMessageId(null), 2000);
    } catch (error) {
      console.error('Failed to copy message:', error);
    }
  };

  const createNewSession = () => {
    const newSession: DevinSession = {
      id: Date.now().toString(),
      title: 'New Configuration Session',
      status: 'active',
      messages: [],
      createdAt: new Date(),
      lastActivity: new Date()
    };
    setSessions([newSession, ...sessions]);
    setActiveSession(newSession);
  };

  return (
    <div className="flex h-[600px] border rounded-lg overflow-hidden">
      {/* Sessions Sidebar */}
      <div className="w-80 border-r bg-muted/30">
        <div className="p-4 border-b">
          <div className="flex items-center justify-between mb-4">
            <h3 className="font-semibold">Devin Sessions</h3>
            <Button size="sm" onClick={createNewSession}>
              <Code className="h-4 w-4 mr-2" />
              New Session
            </Button>
          </div>
        </div>
        <ScrollArea className="flex-1">
          <div className="p-2 space-y-2">
            {sessions.map((session) => (
              <Card
                key={session.id}
                className={`cursor-pointer transition-colors ${
                  activeSession?.id === session.id ? 'bg-primary/10 border-primary' : 'hover:bg-muted/50'
                }`}
                onClick={() => setActiveSession(session)}
              >
                <CardContent className="p-3">
                  <div className="flex items-center justify-between mb-2">
                    <h4 className="font-medium text-sm truncate">{session.title}</h4>
                    <Badge
                      variant={session.status === 'active' ? 'default' : 
                               session.status === 'completed' ? 'secondary' : 'outline'}
                      className="text-xs"
                    >
                      {session.status}
                    </Badge>
                  </div>
                  <p className="text-xs text-muted-foreground">
                    {session.messages.length} messages
                  </p>
                  <p className="text-xs text-muted-foreground">
                    {session.lastActivity.toLocaleTimeString()}
                  </p>
                </CardContent>
              </Card>
            ))}
          </div>
        </ScrollArea>
      </div>

      {/* Chat Interface */}
      <div className="flex-1 flex flex-col">
        {activeSession ? (
          <>
            {/* Chat Header */}
            <div className="p-4 border-b bg-background">
              <div className="flex items-center justify-between">
                <div>
                  <h2 className="font-semibold">{activeSession.title}</h2>
                  <p className="text-sm text-muted-foreground">
                    AI-powered configuration assistant
                  </p>
                </div>
                <Badge variant={activeSession.status === 'active' ? 'default' : 'secondary'}>
                  {activeSession.status}
                </Badge>
              </div>
            </div>

            {/* Messages */}
            <ScrollArea className="flex-1 p-4">
              <div className="space-y-4">
                {activeSession.messages.map((msg) => (
                  <div
                    key={msg.id}
                    className={`flex ${msg.type === 'user' ? 'justify-end' : 'justify-start'}`}
                  >
                    <div
                      className={`max-w-[80%] rounded-lg p-3 ${
                        msg.type === 'user'
                          ? 'bg-primary text-primary-foreground'
                          : 'bg-muted'
                      }`}
                    >
                      <div className="flex items-start space-x-2">
                        {msg.type === 'assistant' && (
                          <Bot className="h-5 w-5 mt-0.5 flex-shrink-0" />
                        )}
                        {msg.type === 'user' && (
                          <User className="h-5 w-5 mt-0.5 flex-shrink-0" />
                        )}
                        <div className="flex-1">
                          <div className="whitespace-pre-wrap text-sm">{msg.content}</div>
                          {msg.attachments && msg.attachments.length > 0 && (
                            <div className="mt-2 space-y-1">
                              {msg.attachments.map((attachment) => (
                                <div
                                  key={attachment.id}
                                  className="flex items-center space-x-2 text-xs bg-background/20 rounded p-2"
                                >
                                  <Paperclip className="h-3 w-3" />
                                  <span>{attachment.name}</span>
                                  <span className="text-muted-foreground">
                                    ({(attachment.size / 1024).toFixed(1)} KB)
                                  </span>
                                </div>
                              ))}
                            </div>
                          )}
                          <div className="flex items-center justify-between mt-2">
                            <span className="text-xs opacity-70">
                              {msg.timestamp.toLocaleTimeString()}
                            </span>
                            <Button
                              variant="ghost"
                              size="sm"
                              onClick={() => copyMessage(msg.id, msg.content)}
                              className="h-6 w-6 p-0"
                            >
                              {copiedMessageId === msg.id ? (
                                <Check className="h-3 w-3" />
                              ) : (
                                <Copy className="h-3 w-3" />
                              )}
                            </Button>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                ))}
                {isTyping && (
                  <div className="flex justify-start">
                    <div className="bg-muted rounded-lg p-3">
                      <div className="flex items-center space-x-2">
                        <Bot className="h-5 w-5" />
                        <div className="flex space-x-1">
                          <div className="w-2 h-2 bg-current rounded-full animate-bounce" />
                          <div className="w-2 h-2 bg-current rounded-full animate-bounce" style={{ animationDelay: '0.1s' }} />
                          <div className="w-2 h-2 bg-current rounded-full animate-bounce" style={{ animationDelay: '0.2s' }} />
                        </div>
                      </div>
                    </div>
                  </div>
                )}
                <div ref={messagesEndRef} />
              </div>
            </ScrollArea>

            {/* Input Area */}
            <div className="p-4 border-t bg-background">
              {attachments.length > 0 && (
                <div className="mb-3 flex flex-wrap gap-2">
                  {attachments.map((file, index) => (
                    <div
                      key={index}
                      className="flex items-center space-x-2 bg-muted rounded-md px-2 py-1 text-sm"
                    >
                      <Paperclip className="h-3 w-3" />
                      <span>{file.name}</span>
                      <Button
                        variant="ghost"
                        size="sm"
                        onClick={() => removeAttachment(index)}
                        className="h-4 w-4 p-0"
                      >
                        ×
                      </Button>
                    </div>
                  ))}
                </div>
              )}
              <div className="flex space-x-2">
                <Button
                  variant="outline"
                  size="sm"
                  onClick={() => fileInputRef.current?.click()}
                >
                  <Paperclip className="h-4 w-4" />
                </Button>
                <Input
                  value={message}
                  onChange={(e) => setMessage(e.target.value)}
                  placeholder="Describe your configuration requirements..."
                  onKeyPress={(e) => e.key === 'Enter' && !e.shiftKey && handleSendMessage()}
                  className="flex-1"
                />
                <Button onClick={handleSendMessage} disabled={!message.trim() && attachments.length === 0}>
                  <Send className="h-4 w-4" />
                </Button>
              </div>
              <input
                ref={fileInputRef}
                type="file"
                multiple
                onChange={handleFileAttachment}
                className="hidden"
                accept=".txt,.md,.json,.yaml,.yml,.xml,.csv"
              />
            </div>
          </>
        ) : (
          <div className="flex-1 flex items-center justify-center">
            <div className="text-center">
              <Bot className="mx-auto h-12 w-12 text-muted-foreground mb-4" />
              <h3 className="text-lg font-semibold mb-2">Welcome to Devin Assistant</h3>
              <p className="text-muted-foreground mb-4">
                Select a session or create a new one to start configuring your AI-powered development environment.
              </p>
              <Button onClick={createNewSession}>
                <Code className="h-4 w-4 mr-2" />
                Start New Session
              </Button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
