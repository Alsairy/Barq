import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

const API_BASE_URL = (import.meta as any).env.VITE_API_BASE_URL || 'https://barq-backend-tunnel-api.devinapps.com';

export interface ChatMessage {
  sessionId: string;
  messageId: string;
  content: string;
  sender: 'user' | 'ai';
  timestamp: Date;
  codeBlocks?: Array<{
    id: string;
    language: string;
    code: string;
    filename?: string;
  }>;
  attachments?: Array<{
    id: string;
    fileName: string;
    fileSize: number;
    fileType: string;
    downloadUrl: string;
  }>;
}

export interface AITaskProgress {
  taskId: string;
  status: 'pending' | 'processing' | 'completed' | 'failed';
  progress: number;
  message?: string;
  result?: any;
}

class SignalRService {
  private connection: HubConnection | null = null;
  private messageHandlers: Map<string, (message: ChatMessage) => void> = new Map();
  private progressHandlers: Map<string, (progress: AITaskProgress) => void> = new Map();

  async connect(): Promise<void> {
    if (this.connection?.state === 'Connected') {
      return;
    }

    this.connection = new HubConnectionBuilder()
      .withUrl(`${API_BASE_URL}/hubs/notification`, {
        withCredentials: true,
        headers: this.getAuthHeaders()
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    this.connection.on('ReceiveAIMessage', (message: ChatMessage) => {
      const handler = this.messageHandlers.get(message.sessionId);
      if (handler) {
        handler(message);
      }
    });

    this.connection.on('AITaskProgress', (progress: AITaskProgress) => {
      const handler = this.progressHandlers.get(progress.taskId);
      if (handler) {
        handler(progress);
      }
    });

    this.connection.on('ReceiveNotification', (notification: any) => {
      console.log('Received notification:', notification);
    });

    this.connection.onreconnecting(() => {
      console.log('SignalR connection lost, attempting to reconnect...');
    });

    this.connection.onreconnected(() => {
      console.log('SignalR connection reestablished');
    });

    this.connection.onclose(() => {
      console.log('SignalR connection closed');
    });

    try {
      await this.connection.start();
      console.log('SignalR connection established');
    } catch (error) {
      console.error('Failed to establish SignalR connection:', error);
      throw error;
    }
  }

  async disconnect(): Promise<void> {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
    }
  }

  async joinChatSession(sessionId: string): Promise<void> {
    if (!this.connection || this.connection.state !== 'Connected') {
      await this.connect();
    }

    try {
      await this.connection!.invoke('JoinGroup', `chat_${sessionId}`);
      console.log(`Joined chat session: ${sessionId}`);
    } catch (error) {
      console.error('Failed to join chat session:', error);
      throw error;
    }
  }

  async leaveChatSession(sessionId: string): Promise<void> {
    if (!this.connection || this.connection.state !== 'Connected') {
      return;
    }

    try {
      await this.connection.invoke('LeaveGroup', `chat_${sessionId}`);
      console.log(`Left chat session: ${sessionId}`);
    } catch (error) {
      console.error('Failed to leave chat session:', error);
    }
  }

  onChatMessage(sessionId: string, handler: (message: ChatMessage) => void): void {
    this.messageHandlers.set(sessionId, handler);
  }

  onTaskProgress(taskId: string, handler: (progress: AITaskProgress) => void): void {
    this.progressHandlers.set(taskId, handler);
  }

  removeChatMessageHandler(sessionId: string): void {
    this.messageHandlers.delete(sessionId);
  }

  removeTaskProgressHandler(taskId: string): void {
    this.progressHandlers.delete(taskId);
  }

  private getAuthHeaders(): Record<string, string> {
    const headers: Record<string, string> = {};
    
    const token = localStorage.getItem('accessToken');
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    } else {
      const username = (import.meta as any).env.VITE_API_USERNAME;
      const password = (import.meta as any).env.VITE_API_PASSWORD;
      if (username && password) {
        const credentials = btoa(`${username}:${password}`);
        headers['Authorization'] = `Basic ${credentials}`;
      }
    }
    
    return headers;
  }

  get isConnected(): boolean {
    return this.connection?.state === 'Connected';
  }

  get connectionState(): string {
    return this.connection?.state || 'Disconnected';
  }
}

export const signalRService = new SignalRService();
