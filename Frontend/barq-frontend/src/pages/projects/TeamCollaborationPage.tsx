import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { 
  MessageSquare, 
  Video, 
  Phone, 
  Calendar, 
  FileText, 
  Share2,
  Search,
  Plus,
  Send,
  Paperclip,
  Smile,
  MoreHorizontal,
  Settings,
  Clock
} from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Badge } from '../../components/ui/badge';
import { Avatar, AvatarFallback, AvatarImage } from '../../components/ui/avatar';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../components/ui/tabs';
import { ScrollArea } from '../../components/ui/scroll-area';
import { LoadingSpinner } from '../../components/ui/loading-spinner';

interface TeamMember {
  id: string;
  name: string;
  email: string;
  role: string;
  avatar?: string;
  status: 'online' | 'away' | 'busy' | 'offline';
  lastSeen?: string;
}

interface Message {
  id: string;
  senderId: string;
  senderName: string;
  senderAvatar?: string;
  content: string;
  timestamp: string;
  type: 'text' | 'file' | 'system';
  attachments?: {
    name: string;
    url: string;
    type: string;
  }[];
  reactions?: {
    emoji: string;
    users: string[];
  }[];
}

interface Channel {
  id: string;
  name: string;
  description: string;
  type: 'public' | 'private' | 'direct';
  memberCount: number;
  unreadCount: number;
  lastMessage?: Message;
}

interface Meeting {
  id: string;
  title: string;
  description: string;
  startTime: string;
  endTime: string;
  attendees: TeamMember[];
  status: 'upcoming' | 'ongoing' | 'completed';
  meetingUrl?: string;
}

export function TeamCollaborationPage() {
  const { projectId } = useParams<{ projectId: string }>();
  const [teamMembers, setTeamMembers] = useState<TeamMember[]>([]);
  const [channels, setChannels] = useState<Channel[]>([]);
  const [messages, setMessages] = useState<Message[]>([]);
  const [meetings, setMeetings] = useState<Meeting[]>([]);
  const [selectedChannel, setSelectedChannel] = useState<string>('');
  const [messageInput, setMessageInput] = useState('');
  const [isLoading, setIsLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      try {
        setIsLoading(true);
        await new Promise(resolve => setTimeout(resolve, 1000));
        
        const mockTeamMembers: TeamMember[] = [
          {
            id: '1',
            name: 'John Doe',
            email: 'john@example.com',
            role: 'Project Manager',
            avatar: '/avatars/john.jpg',
            status: 'online'
          },
          {
            id: '2',
            name: 'Jane Smith',
            email: 'jane@example.com',
            role: 'Frontend Developer',
            avatar: '/avatars/jane.jpg',
            status: 'online'
          },
          {
            id: '3',
            name: 'Mike Johnson',
            email: 'mike@example.com',
            role: 'DevOps Engineer',
            avatar: '/avatars/mike.jpg',
            status: 'away',
            lastSeen: '2024-01-20T10:30:00Z'
          },
          {
            id: '4',
            name: 'Sarah Wilson',
            email: 'sarah@example.com',
            role: 'Technical Writer',
            avatar: '/avatars/sarah.jpg',
            status: 'busy'
          }
        ];

        const mockChannels: Channel[] = [
          {
            id: '1',
            name: 'general',
            description: 'General project discussions',
            type: 'public',
            memberCount: 4,
            unreadCount: 2
          },
          {
            id: '2',
            name: 'development',
            description: 'Development-related discussions',
            type: 'public',
            memberCount: 3,
            unreadCount: 0
          },
          {
            id: '3',
            name: 'design-review',
            description: 'Design reviews and feedback',
            type: 'private',
            memberCount: 2,
            unreadCount: 1
          }
        ];

        const mockMessages: Message[] = [
          {
            id: '1',
            senderId: '1',
            senderName: 'John Doe',
            senderAvatar: '/avatars/john.jpg',
            content: 'Good morning team! Let\'s review the sprint progress in today\'s standup.',
            timestamp: '2024-01-20T09:00:00Z',
            type: 'text'
          },
          {
            id: '2',
            senderId: '2',
            senderName: 'Jane Smith',
            senderAvatar: '/avatars/jane.jpg',
            content: 'I\'ve completed the dashboard UI. Here\'s the latest design:',
            timestamp: '2024-01-20T09:15:00Z',
            type: 'file',
            attachments: [
              {
                name: 'dashboard-mockup.png',
                url: '/files/dashboard-mockup.png',
                type: 'image'
              }
            ]
          },
          {
            id: '3',
            senderId: '3',
            senderName: 'Mike Johnson',
            senderAvatar: '/avatars/mike.jpg',
            content: 'Great work! The CI/CD pipeline is now configured and ready for testing.',
            timestamp: '2024-01-20T09:30:00Z',
            type: 'text',
            reactions: [
              { emoji: '👍', users: ['1', '2'] },
              { emoji: '🎉', users: ['4'] }
            ]
          }
        ];

        const mockMeetings: Meeting[] = [
          {
            id: '1',
            title: 'Daily Standup',
            description: 'Daily team sync and progress update',
            startTime: '2024-01-20T10:00:00Z',
            endTime: '2024-01-20T10:30:00Z',
            attendees: mockTeamMembers,
            status: 'upcoming',
            meetingUrl: 'https://meet.example.com/standup'
          },
          {
            id: '2',
            title: 'Sprint Planning',
            description: 'Plan tasks for the upcoming sprint',
            startTime: '2024-01-22T14:00:00Z',
            endTime: '2024-01-22T16:00:00Z',
            attendees: mockTeamMembers.slice(0, 3),
            status: 'upcoming'
          }
        ];

        setTeamMembers(mockTeamMembers);
        setChannels(mockChannels);
        setMessages(mockMessages);
        setMeetings(mockMeetings);
        setSelectedChannel(mockChannels[0]?.id || '');
      } catch (error) {
        console.error('Failed to fetch collaboration data:', error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
  }, [projectId]);

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'online': return 'bg-green-500';
      case 'away': return 'bg-yellow-500';
      case 'busy': return 'bg-red-500';
      default: return 'bg-gray-400';
    }
  };

  const handleSendMessage = () => {
    if (messageInput.trim()) {
      const newMessage: Message = {
        id: Date.now().toString(),
        senderId: '1', // Current user
        senderName: 'You',
        content: messageInput,
        timestamp: new Date().toISOString(),
        type: 'text'
      };
      setMessages([...messages, newMessage]);
      setMessageInput('');
    }
  };

  const filteredMembers = teamMembers.filter(member =>
    member.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    member.role.toLowerCase().includes(searchTerm.toLowerCase())
  );

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <LoadingSpinner className="h-8 w-8" />
      </div>
    );
  }

  return (
    <div className="container mx-auto py-6 space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Team Collaboration</h1>
          <p className="text-gray-600">Communicate and collaborate with your team</p>
        </div>
        <div className="flex gap-2">
          <Button variant="outline">
            <Video className="h-4 w-4 mr-2" />
            Start Meeting
          </Button>
          <Button>
            <Plus className="h-4 w-4 mr-2" />
            Invite Member
          </Button>
        </div>
      </div>

      <Tabs defaultValue="chat" className="space-y-6">
        <TabsList>
          <TabsTrigger value="chat">Chat</TabsTrigger>
          <TabsTrigger value="meetings">Meetings</TabsTrigger>
          <TabsTrigger value="team">Team</TabsTrigger>
          <TabsTrigger value="files">Files</TabsTrigger>
        </TabsList>

        <TabsContent value="chat" className="space-y-6">
          <div className="grid grid-cols-1 lg:grid-cols-4 gap-6 h-[600px]">
            {/* Channels Sidebar */}
            <Card className="lg:col-span-1">
              <CardHeader className="pb-3">
                <CardTitle className="text-sm">Channels</CardTitle>
              </CardHeader>
              <CardContent className="p-0">
                <ScrollArea className="h-[500px]">
                  <div className="space-y-1 p-3">
                    {channels.map((channel) => (
                      <div
                        key={channel.id}
                        className={`p-2 rounded cursor-pointer hover:bg-gray-100 ${
                          selectedChannel === channel.id ? 'bg-blue-100' : ''
                        }`}
                        onClick={() => setSelectedChannel(channel.id)}
                      >
                        <div className="flex items-center justify-between">
                          <div className="flex items-center gap-2">
                            <span className="text-gray-500">#</span>
                            <span className="text-sm font-medium">{channel.name}</span>
                          </div>
                          {channel.unreadCount > 0 && (
                            <Badge variant="destructive" className="text-xs">
                              {channel.unreadCount}
                            </Badge>
                          )}
                        </div>
                        <p className="text-xs text-gray-500 mt-1 truncate">
                          {channel.description}
                        </p>
                      </div>
                    ))}
                  </div>
                </ScrollArea>
              </CardContent>
            </Card>

            {/* Chat Area */}
            <Card className="lg:col-span-2">
              <CardHeader className="pb-3">
                <div className="flex items-center justify-between">
                  <div>
                    <CardTitle className="text-sm">
                      #{channels.find(c => c.id === selectedChannel)?.name || 'general'}
                    </CardTitle>
                    <CardDescription className="text-xs">
                      {channels.find(c => c.id === selectedChannel)?.memberCount} members
                    </CardDescription>
                  </div>
                  <Button variant="ghost" size="sm">
                    <Settings className="h-4 w-4" />
                  </Button>
                </div>
              </CardHeader>
              <CardContent className="p-0 flex flex-col h-[500px]">
                {/* Messages */}
                <ScrollArea className="flex-1 p-3">
                  <div className="space-y-4">
                    {messages.map((message) => (
                      <div key={message.id} className="flex items-start gap-3">
                        <Avatar className="h-8 w-8">
                          <AvatarImage src={message.senderAvatar} />
                          <AvatarFallback>
                            {message.senderName.split(' ').map(n => n[0]).join('')}
                          </AvatarFallback>
                        </Avatar>
                        <div className="flex-1 min-w-0">
                          <div className="flex items-center gap-2 mb-1">
                            <span className="text-sm font-medium">{message.senderName}</span>
                            <span className="text-xs text-gray-500">
                              {new Date(message.timestamp).toLocaleTimeString()}
                            </span>
                          </div>
                          <p className="text-sm text-gray-700">{message.content}</p>
                          {message.attachments && (
                            <div className="mt-2 space-y-1">
                              {message.attachments.map((attachment, index) => (
                                <div key={index} className="flex items-center gap-2 p-2 bg-gray-50 rounded">
                                  <Paperclip className="h-4 w-4 text-gray-400" />
                                  <span className="text-sm">{attachment.name}</span>
                                </div>
                              ))}
                            </div>
                          )}
                          {message.reactions && (
                            <div className="flex gap-1 mt-2">
                              {message.reactions.map((reaction, index) => (
                                <Badge key={index} variant="secondary" className="text-xs">
                                  {reaction.emoji} {reaction.users.length}
                                </Badge>
                              ))}
                            </div>
                          )}
                        </div>
                      </div>
                    ))}
                  </div>
                </ScrollArea>

                {/* Message Input */}
                <div className="p-3 border-t">
                  <div className="flex items-center gap-2">
                    <Input
                      placeholder="Type a message..."
                      value={messageInput}
                      onChange={(e) => setMessageInput(e.target.value)}
                      onKeyPress={(e) => e.key === 'Enter' && handleSendMessage()}
                      className="flex-1"
                    />
                    <Button variant="ghost" size="sm">
                      <Paperclip className="h-4 w-4" />
                    </Button>
                    <Button variant="ghost" size="sm">
                      <Smile className="h-4 w-4" />
                    </Button>
                    <Button size="sm" onClick={handleSendMessage}>
                      <Send className="h-4 w-4" />
                    </Button>
                  </div>
                </div>
              </CardContent>
            </Card>

            {/* Team Members Sidebar */}
            <Card className="lg:col-span-1">
              <CardHeader className="pb-3">
                <CardTitle className="text-sm">Team Members</CardTitle>
              </CardHeader>
              <CardContent className="p-0">
                <ScrollArea className="h-[500px]">
                  <div className="space-y-2 p-3">
                    {teamMembers.map((member) => (
                      <div key={member.id} className="flex items-center gap-3 p-2 rounded hover:bg-gray-50">
                        <div className="relative">
                          <Avatar className="h-8 w-8">
                            <AvatarImage src={member.avatar} />
                            <AvatarFallback>
                              {member.name.split(' ').map(n => n[0]).join('')}
                            </AvatarFallback>
                          </Avatar>
                          <div className={`absolute -bottom-1 -right-1 w-3 h-3 rounded-full border-2 border-white ${getStatusColor(member.status)}`} />
                        </div>
                        <div className="flex-1 min-w-0">
                          <p className="text-sm font-medium truncate">{member.name}</p>
                          <p className="text-xs text-gray-500 truncate">{member.role}</p>
                        </div>
                      </div>
                    ))}
                  </div>
                </ScrollArea>
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        <TabsContent value="meetings" className="space-y-6">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <Card>
              <CardHeader>
                <CardTitle>Upcoming Meetings</CardTitle>
                <CardDescription>Scheduled team meetings and events</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                {meetings.filter(m => m.status === 'upcoming').map((meeting) => (
                  <div key={meeting.id} className="p-4 border rounded-lg">
                    <div className="flex items-start justify-between mb-2">
                      <div>
                        <h4 className="font-medium">{meeting.title}</h4>
                        <p className="text-sm text-gray-600">{meeting.description}</p>
                      </div>
                      <Badge variant="outline">{meeting.status}</Badge>
                    </div>
                    <div className="flex items-center gap-4 text-sm text-gray-500 mb-3">
                      <div className="flex items-center gap-1">
                        <Calendar className="h-4 w-4" />
                        {new Date(meeting.startTime).toLocaleDateString()}
                      </div>
                      <div className="flex items-center gap-1">
                        <Clock className="h-4 w-4" />
                        {new Date(meeting.startTime).toLocaleTimeString()} - {new Date(meeting.endTime).toLocaleTimeString()}
                      </div>
                    </div>
                    <div className="flex items-center justify-between">
                      <div className="flex -space-x-2">
                        {meeting.attendees.slice(0, 3).map((attendee) => (
                          <Avatar key={attendee.id} className="h-6 w-6 border-2 border-white">
                            <AvatarImage src={attendee.avatar} />
                            <AvatarFallback className="text-xs">
                              {attendee.name.split(' ').map(n => n[0]).join('')}
                            </AvatarFallback>
                          </Avatar>
                        ))}
                        {meeting.attendees.length > 3 && (
                          <div className="h-6 w-6 rounded-full bg-gray-200 border-2 border-white flex items-center justify-center">
                            <span className="text-xs">+{meeting.attendees.length - 3}</span>
                          </div>
                        )}
                      </div>
                      <div className="flex gap-2">
                        {meeting.meetingUrl && (
                          <Button size="sm" variant="outline">
                            <Video className="h-4 w-4 mr-1" />
                            Join
                          </Button>
                        )}
                        <Button size="sm" variant="ghost">
                          <MoreHorizontal className="h-4 w-4" />
                        </Button>
                      </div>
                    </div>
                  </div>
                ))}
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Quick Actions</CardTitle>
                <CardDescription>Start a meeting or schedule one</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <Button className="w-full justify-start">
                  <Video className="h-4 w-4 mr-2" />
                  Start Instant Meeting
                </Button>
                <Button variant="outline" className="w-full justify-start">
                  <Calendar className="h-4 w-4 mr-2" />
                  Schedule Meeting
                </Button>
                <Button variant="outline" className="w-full justify-start">
                  <Phone className="h-4 w-4 mr-2" />
                  Start Audio Call
                </Button>
                <Button variant="outline" className="w-full justify-start">
                  <Share2 className="h-4 w-4 mr-2" />
                  Share Screen
                </Button>
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        <TabsContent value="team" className="space-y-6">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <Card>
              <CardHeader>
                <CardTitle>Team Directory</CardTitle>
                <CardDescription>Search and manage team members</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="relative">
                  <Search className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
                  <Input
                    placeholder="Search team members..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    className="pl-10"
                  />
                </div>
                <ScrollArea className="h-[400px]">
                  <div className="space-y-3">
                    {filteredMembers.map((member) => (
                      <div key={member.id} className="flex items-center justify-between p-3 border rounded-lg">
                        <div className="flex items-center gap-3">
                          <div className="relative">
                            <Avatar className="h-10 w-10">
                              <AvatarImage src={member.avatar} />
                              <AvatarFallback>
                                {member.name.split(' ').map(n => n[0]).join('')}
                              </AvatarFallback>
                            </Avatar>
                            <div className={`absolute -bottom-1 -right-1 w-3 h-3 rounded-full border-2 border-white ${getStatusColor(member.status)}`} />
                          </div>
                          <div>
                            <p className="font-medium">{member.name}</p>
                            <p className="text-sm text-gray-600">{member.role}</p>
                            <p className="text-xs text-gray-500">{member.email}</p>
                          </div>
                        </div>
                        <div className="flex gap-2">
                          <Button size="sm" variant="outline">
                            <MessageSquare className="h-4 w-4" />
                          </Button>
                          <Button size="sm" variant="outline">
                            <Video className="h-4 w-4" />
                          </Button>
                        </div>
                      </div>
                    ))}
                  </div>
                </ScrollArea>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Team Activity</CardTitle>
                <CardDescription>Recent team member activities</CardDescription>
              </CardHeader>
              <CardContent>
                <ScrollArea className="h-[400px]">
                  <div className="space-y-4">
                    <div className="flex items-start gap-3">
                      <Avatar className="h-8 w-8">
                        <AvatarImage src="/avatars/john.jpg" />
                        <AvatarFallback>JD</AvatarFallback>
                      </Avatar>
                      <div className="flex-1">
                        <p className="text-sm">
                          <span className="font-medium">John Doe</span> completed task "User Authentication"
                        </p>
                        <p className="text-xs text-gray-500">2 hours ago</p>
                      </div>
                    </div>
                    <div className="flex items-start gap-3">
                      <Avatar className="h-8 w-8">
                        <AvatarImage src="/avatars/jane.jpg" />
                        <AvatarFallback>JS</AvatarFallback>
                      </Avatar>
                      <div className="flex-1">
                        <p className="text-sm">
                          <span className="font-medium">Jane Smith</span> uploaded design files
                        </p>
                        <p className="text-xs text-gray-500">4 hours ago</p>
                      </div>
                    </div>
                    <div className="flex items-start gap-3">
                      <Avatar className="h-8 w-8">
                        <AvatarImage src="/avatars/mike.jpg" />
                        <AvatarFallback>MJ</AvatarFallback>
                      </Avatar>
                      <div className="flex-1">
                        <p className="text-sm">
                          <span className="font-medium">Mike Johnson</span> started working on CI/CD setup
                        </p>
                        <p className="text-xs text-gray-500">6 hours ago</p>
                      </div>
                    </div>
                  </div>
                </ScrollArea>
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        <TabsContent value="files" className="space-y-6">
          <Card>
            <CardHeader>
              <CardTitle>Shared Files</CardTitle>
              <CardDescription>Files shared within the team</CardDescription>
            </CardHeader>
            <CardContent>
              <div className="space-y-4">
                <div className="flex items-center justify-between p-3 border rounded-lg">
                  <div className="flex items-center gap-3">
                    <FileText className="h-8 w-8 text-blue-500" />
                    <div>
                      <p className="font-medium">Project Requirements.pdf</p>
                      <p className="text-sm text-gray-600">Shared by John Doe • 2.3 MB</p>
                    </div>
                  </div>
                  <Button size="sm" variant="outline">Download</Button>
                </div>
                <div className="flex items-center justify-between p-3 border rounded-lg">
                  <div className="flex items-center gap-3">
                    <FileText className="h-8 w-8 text-green-500" />
                    <div>
                      <p className="font-medium">Dashboard Mockups.fig</p>
                      <p className="text-sm text-gray-600">Shared by Jane Smith • 5.7 MB</p>
                    </div>
                  </div>
                  <Button size="sm" variant="outline">Download</Button>
                </div>
                <div className="flex items-center justify-between p-3 border rounded-lg">
                  <div className="flex items-center gap-3">
                    <FileText className="h-8 w-8 text-purple-500" />
                    <div>
                      <p className="font-medium">API Documentation.md</p>
                      <p className="text-sm text-gray-600">Shared by Sarah Wilson • 1.2 MB</p>
                    </div>
                  </div>
                  <Button size="sm" variant="outline">Download</Button>
                </div>
              </div>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
