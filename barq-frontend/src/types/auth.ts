export interface LoginRequest {
  email: string;
  password: string;
  rememberMe?: boolean;
}

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
  organizationName?: string;
  acceptTerms: boolean;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  token: string;
  newPassword: string;
  confirmPassword: string;
}

export interface ChangePasswordRequest {
  userId: string;
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

export interface MfaSetupRequest {
  userId: string;
}

export interface MfaVerificationRequest {
  userId: string;
  code: string;
  backupCode?: string;
}

export interface AuthenticationResponse {
  success: boolean;
  accessToken?: string;
  refreshToken?: string;
  expiresAt?: string;
  requiresMfa: boolean;
  mfaToken?: string;
  message?: string;
  userId?: string;
  userEmail?: string;
  user?: UserProfile;
  roles: string[];
}

export interface UserProfile {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  organizationId?: string;
  organizationName?: string;
  tenantId?: string;
  roles: string[];
  permissions: string[];
  isEmailVerified: boolean;
  isMfaEnabled: boolean;
  lastLoginAt?: string;
  createdAt: string;
}

export interface MfaSetupResponse {
  success: boolean;
  message: string;
  qrCodeUrl?: string;
  manualEntryKey?: string;
  backupCodes?: string[];
}

export interface MfaVerificationResponse {
  success: boolean;
  message: string;
  isVerified: boolean;
  accessToken?: string;
  user?: UserProfile;
}

export interface BackupCodesResponse {
  success: boolean;
  message: string;
  backupCodes: string[];
}

export interface PasswordResetResponse {
  success: boolean;
  message: string;
  resetInitiated: boolean;
  resetToken?: string;
}

export interface PasswordChangeResponse {
  success: boolean;
  message: string;
  passwordChanged: boolean;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
  errors?: string[];
}

export interface AuthState {
  isAuthenticated: boolean;
  user: UserProfile | null;
  token: string | null;
  accessToken: string | null;
  refreshToken: string | null;
  tenantId: string | null;
  isLoading: boolean;
  error: string | null;
  requiresMfa: boolean;
  mfaToken: string | null;
}

export interface SsoProvider {
  id: string;
  name: string;
  type: 'saml' | 'oauth' | 'oidc';
  iconUrl?: string;
  isEnabled: boolean;
}

export interface SocialProvider {
  id: string;
  name: string;
  type: 'google' | 'microsoft' | 'github' | 'linkedin';
  iconUrl?: string;
  isEnabled: boolean;
}
