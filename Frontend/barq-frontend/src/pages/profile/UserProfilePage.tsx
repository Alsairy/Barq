import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { User, Shield, Bell, Eye, Camera } from 'lucide-react';
import { Button } from '../../components/ui';
import { Input } from '../../components/ui';
import { Label } from '../../components/ui';
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '../../components/ui';
import { Alert, AlertDescription } from '../../components/ui';
import { Avatar, AvatarFallback, AvatarImage } from '../../components/ui';
import { Separator } from '../../components/ui';
import { Switch } from '../../components/ui';
import { Textarea } from '../../components/ui';
import { LoadingSpinner } from '../../components/ui/loading-spinner';
import { RootState } from '../../store';

interface ProfileFormData {
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  bio?: string;
  jobTitle?: string;
  department?: string;
  location?: string;
}

interface SecuritySettings {
  mfaEnabled: boolean;
  emailNotifications: boolean;
  smsNotifications: boolean;
  desktopNotifications: boolean;
  securityAlerts: boolean;
}


export function UserProfilePage() {
  const { user, isLoading } = useSelector((state: RootState) => state.auth);

  const [activeTab, setActiveTab] = useState<'profile' | 'security' | 'privacy' | 'activity'>('profile');
  const [isEditing, setIsEditing] = useState(false);
  const [isSaving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  const [profileData, setProfileData] = useState<ProfileFormData>({
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    bio: '',
    jobTitle: '',
    department: '',
    location: '',
  });

  const [securitySettings, setSecuritySettings] = useState<SecuritySettings>({
    mfaEnabled: false,
    emailNotifications: true,
    smsNotifications: false,
    desktopNotifications: true,
    securityAlerts: true,
  });


  useEffect(() => {
    if (user) {
      setProfileData({
        firstName: user.firstName || '',
        lastName: user.lastName || '',
        email: user.email || '',
        phone: '',
        bio: '',
        jobTitle: '',
        department: '',
        location: '',
      });
      setSecuritySettings(prev => ({
        ...prev,
        mfaEnabled: user.isMfaEnabled || false,
      }));
    }
  }, [user]);

  const handleProfileSave = async () => {
    setSaving(true);
    setError(null);
    
    try {
      await new Promise(resolve => setTimeout(resolve, 1000)); // Simulate API call
      setSuccess('Profile updated successfully');
      setIsEditing(false);
    } catch (error: any) {
      setError('Failed to update profile. Please try again.');
    } finally {
      setSaving(false);
    }
  };

  const handleSecuritySave = async () => {
    setSaving(true);
    setError(null);
    
    try {
      await new Promise(resolve => setTimeout(resolve, 1000)); // Simulate API call
      setSuccess('Security settings updated successfully');
    } catch (error: any) {
      setError('Failed to update security settings. Please try again.');
    } finally {
      setSaving(false);
    }
  };


  const handleAvatarUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    console.log('Uploading avatar:', file);
  };

  const renderProfileTab = () => (
    <div className="space-y-6">
      <Card>
        <CardHeader>
          <CardTitle>Profile Information</CardTitle>
          <CardDescription>
            Update your personal information and profile details
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-6">
          {/* Avatar Section */}
          <div className="flex items-center space-x-4">
            <Avatar className="h-20 w-20">
              <AvatarImage src="" alt={`${profileData.firstName} ${profileData.lastName}`} />
              <AvatarFallback className="text-lg">
                {profileData.firstName.charAt(0)}{profileData.lastName.charAt(0)}
              </AvatarFallback>
            </Avatar>
            <div className="space-y-2">
              <Label htmlFor="avatar-upload" className="cursor-pointer">
                <Button variant="outline" size="sm" asChild>
                  <span>
                    <Camera className="h-4 w-4 mr-2" />
                    Change Avatar
                  </span>
                </Button>
              </Label>
              <input
                id="avatar-upload"
                type="file"
                accept="image/*"
                onChange={handleAvatarUpload}
                className="hidden"
              />
              <p className="text-sm text-gray-500">JPG, PNG or GIF. Max size 2MB.</p>
            </div>
          </div>

          <Separator />

          {/* Basic Information */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="firstName">First Name</Label>
              <Input
                id="firstName"
                value={profileData.firstName}
                onChange={(e) => setProfileData(prev => ({ ...prev, firstName: e.target.value }))}
                disabled={!isEditing}
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="lastName">Last Name</Label>
              <Input
                id="lastName"
                value={profileData.lastName}
                onChange={(e) => setProfileData(prev => ({ ...prev, lastName: e.target.value }))}
                disabled={!isEditing}
              />
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="email">Email Address</Label>
            <Input
              id="email"
              type="email"
              value={profileData.email}
              onChange={(e) => setProfileData(prev => ({ ...prev, email: e.target.value }))}
              disabled={!isEditing}
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="phone">Phone Number</Label>
            <Input
              id="phone"
              type="tel"
              value={profileData.phone}
              onChange={(e) => setProfileData(prev => ({ ...prev, phone: e.target.value }))}
              disabled={!isEditing}
              placeholder="+1 (555) 123-4567"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="bio">Bio</Label>
            <Textarea
              id="bio"
              value={profileData.bio}
              onChange={(e) => setProfileData(prev => ({ ...prev, bio: e.target.value }))}
              disabled={!isEditing}
              placeholder="Tell us about yourself..."
              rows={3}
            />
          </div>

          {/* Professional Information */}
          <Separator />
          
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="jobTitle">Job Title</Label>
              <Input
                id="jobTitle"
                value={profileData.jobTitle}
                onChange={(e) => setProfileData(prev => ({ ...prev, jobTitle: e.target.value }))}
                disabled={!isEditing}
                placeholder="Software Engineer"
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="department">Department</Label>
              <Input
                id="department"
                value={profileData.department}
                onChange={(e) => setProfileData(prev => ({ ...prev, department: e.target.value }))}
                disabled={!isEditing}
                placeholder="Engineering"
              />
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="location">Location</Label>
            <Input
              id="location"
              value={profileData.location}
              onChange={(e) => setProfileData(prev => ({ ...prev, location: e.target.value }))}
              disabled={!isEditing}
              placeholder="San Francisco, CA"
            />
          </div>
        </CardContent>
        <CardFooter className="flex justify-between">
          {isEditing ? (
            <div className="flex space-x-2">
              <Button
                onClick={handleProfileSave}
                disabled={isSaving}
              >
                {isSaving ? (
                  <>
                    <LoadingSpinner className="mr-2 h-4 w-4" />
                    Saving...
                  </>
                ) : (
                  'Save Changes'
                )}
              </Button>
              <Button
                variant="outline"
                onClick={() => setIsEditing(false)}
                disabled={isSaving}
              >
                Cancel
              </Button>
            </div>
          ) : (
            <Button onClick={() => setIsEditing(true)}>
              Edit Profile
            </Button>
          )}
        </CardFooter>
      </Card>
    </div>
  );

  const renderSecurityTab = () => (
    <div className="space-y-6">
      <Card>
        <CardHeader>
          <CardTitle>Security Settings</CardTitle>
          <CardDescription>
            Manage your account security and authentication preferences
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-6">
          <div className="flex items-center justify-between">
            <div className="space-y-0.5">
              <Label>Two-Factor Authentication</Label>
              <p className="text-sm text-gray-500">
                Add an extra layer of security to your account
              </p>
            </div>
            <Switch
              checked={securitySettings.mfaEnabled}
              onCheckedChange={(checked) => 
                setSecuritySettings(prev => ({ ...prev, mfaEnabled: checked }))
              }
            />
          </div>

          <Separator />

          <div className="space-y-4">
            <h4 className="font-medium">Notification Preferences</h4>
            
            <div className="flex items-center justify-between">
              <div className="space-y-0.5">
                <Label>Email Notifications</Label>
                <p className="text-sm text-gray-500">
                  Receive security alerts via email
                </p>
              </div>
              <Switch
                checked={securitySettings.emailNotifications}
                onCheckedChange={(checked) => 
                  setSecuritySettings(prev => ({ ...prev, emailNotifications: checked }))
                }
              />
            </div>

            <div className="flex items-center justify-between">
              <div className="space-y-0.5">
                <Label>SMS Notifications</Label>
                <p className="text-sm text-gray-500">
                  Receive security alerts via SMS
                </p>
              </div>
              <Switch
                checked={securitySettings.smsNotifications}
                onCheckedChange={(checked) => 
                  setSecuritySettings(prev => ({ ...prev, smsNotifications: checked }))
                }
              />
            </div>

            <div className="flex items-center justify-between">
              <div className="space-y-0.5">
                <Label>Desktop Notifications</Label>
                <p className="text-sm text-gray-500">
                  Show browser notifications for security events
                </p>
              </div>
              <Switch
                checked={securitySettings.desktopNotifications}
                onCheckedChange={(checked) => 
                  setSecuritySettings(prev => ({ ...prev, desktopNotifications: checked }))
                }
              />
            </div>

            <div className="flex items-center justify-between">
              <div className="space-y-0.5">
                <Label>Security Alerts</Label>
                <p className="text-sm text-gray-500">
                  Get notified about suspicious account activity
                </p>
              </div>
              <Switch
                checked={securitySettings.securityAlerts}
                onCheckedChange={(checked) => 
                  setSecuritySettings(prev => ({ ...prev, securityAlerts: checked }))
                }
              />
            </div>
          </div>
        </CardContent>
        <CardFooter>
          <Button
            onClick={handleSecuritySave}
            disabled={isSaving}
          >
            {isSaving ? (
              <>
                <LoadingSpinner className="mr-2 h-4 w-4" />
                Saving...
              </>
            ) : (
              'Save Security Settings'
            )}
          </Button>
        </CardFooter>
      </Card>
    </div>
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
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Profile Settings</h1>
          <p className="text-gray-600">Manage your account settings and preferences</p>
        </div>
      </div>

      {(error || success) && (
        <Alert variant={error ? "destructive" : "default"}>
          <AlertDescription>{error || success}</AlertDescription>
        </Alert>
      )}

      {/* Tab Navigation */}
      <div className="border-b border-gray-200">
        <nav className="-mb-px flex space-x-8">
          {[
            { id: 'profile', label: 'Profile', icon: User },
            { id: 'security', label: 'Security', icon: Shield },
            { id: 'privacy', label: 'Privacy', icon: Eye },
            { id: 'activity', label: 'Activity', icon: Bell },
          ].map(({ id, label, icon: Icon }) => (
            <button
              key={id}
              onClick={() => setActiveTab(id as any)}
              className={`flex items-center py-2 px-1 border-b-2 font-medium text-sm ${
                activeTab === id
                  ? 'border-blue-500 text-blue-600'
                  : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
              }`}
            >
              <Icon className="h-4 w-4 mr-2" />
              {label}
            </button>
          ))}
        </nav>
      </div>

      {/* Tab Content */}
      {activeTab === 'profile' && renderProfileTab()}
      {activeTab === 'security' && renderSecurityTab()}
      {activeTab === 'privacy' && (
        <Card>
          <CardHeader>
            <CardTitle>Privacy Settings</CardTitle>
            <CardDescription>Control your privacy and data sharing preferences</CardDescription>
          </CardHeader>
          <CardContent>
            <p className="text-gray-500">Privacy settings coming soon...</p>
          </CardContent>
        </Card>
      )}
      {activeTab === 'activity' && (
        <Card>
          <CardHeader>
            <CardTitle>Account Activity</CardTitle>
            <CardDescription>View your recent account activity and login history</CardDescription>
          </CardHeader>
          <CardContent>
            <p className="text-gray-500">Activity log coming soon...</p>
          </CardContent>
        </Card>
      )}
    </div>
  );
}
