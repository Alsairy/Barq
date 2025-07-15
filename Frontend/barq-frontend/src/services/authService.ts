import axios, { AxiosResponse } from 'axios';
import {
  LoginRequest,
  RegisterRequest,
  ForgotPasswordRequest,
  ResetPasswordRequest,
  ChangePasswordRequest,
  MfaSetupRequest,
  MfaVerificationRequest,
  AuthenticationResponse,
  MfaSetupResponse,
  MfaVerificationResponse,
  BackupCodesResponse,
  PasswordResetResponse,
  PasswordChangeResponse,
  ApiResponse,
} from '../types/auth';

const API_BASE_URL = (import.meta as any).env.VITE_API_BASE_URL || 'https://localhost:5001/api';

const authApi = axios.create({
  baseURL: `${API_BASE_URL}/auth`,
  headers: {
    'Content-Type': 'application/json',
  },
});

authApi.interceptors.request.use((config) => {
  const token = localStorage.getItem('accessToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

authApi.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401) {
      const refreshToken = localStorage.getItem('refreshToken');
      if (refreshToken) {
        try {
          const response = await authApi.post('/refresh-token', { refreshToken });
          const { accessToken } = response.data.data;
          localStorage.setItem('accessToken', accessToken);
          error.config.headers.Authorization = `Bearer ${accessToken}`;
          return authApi.request(error.config);
        } catch (refreshError) {
          localStorage.removeItem('accessToken');
          localStorage.removeItem('refreshToken');
          window.location.href = '/login';
        }
      } else {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        window.location.href = '/login';
      }
    }
    return Promise.reject(error);
  }
);

export const authService = {
  async login(request: LoginRequest): Promise<AuthenticationResponse> {
    const response: AxiosResponse<ApiResponse<AuthenticationResponse>> = await authApi.post('/login', {
      request,
    });
    return response.data.data!;
  },

  async register(request: RegisterRequest): Promise<AuthenticationResponse> {
    const response: AxiosResponse<ApiResponse<AuthenticationResponse>> = await authApi.post('/register', {
      request,
    });
    return response.data.data!;
  },

  async logout(userId: string): Promise<void> {
    await authApi.post('/logout', { userId });
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
  },

  async refreshToken(refreshToken: string): Promise<AuthenticationResponse> {
    const response: AxiosResponse<ApiResponse<AuthenticationResponse>> = await authApi.post('/refresh-token', {
      refreshToken,
    });
    return response.data.data!;
  },

  async forgotPassword(request: ForgotPasswordRequest): Promise<PasswordResetResponse> {
    const response: AxiosResponse<ApiResponse<PasswordResetResponse>> = await authApi.post('/forgot-password', request);
    return response.data.data!;
  },

  async resetPassword(request: ResetPasswordRequest): Promise<PasswordResetResponse> {
    const response: AxiosResponse<ApiResponse<PasswordResetResponse>> = await authApi.post('/reset-password', {
      token: request.token,
      newPassword: request.newPassword,
    });
    return response.data.data!;
  },

  async changePassword(request: ChangePasswordRequest): Promise<PasswordChangeResponse> {
    const response: AxiosResponse<ApiResponse<PasswordChangeResponse>> = await authApi.post('/change-password', request);
    return response.data.data!;
  },

  async setupMfa(request: MfaSetupRequest): Promise<MfaSetupResponse> {
    const response: AxiosResponse<ApiResponse<MfaSetupResponse>> = await authApi.post('/setup-mfa', request);
    return response.data.data!;
  },

  async verifyMfa(request: MfaVerificationRequest): Promise<MfaVerificationResponse> {
    const response: AxiosResponse<ApiResponse<MfaVerificationResponse>> = await authApi.post('/verify-mfa', request);
    return response.data.data!;
  },

  async disableMfa(userId: string, password: string): Promise<{ success: boolean; message: string }> {
    const response: AxiosResponse<ApiResponse<{ isDisabled: boolean }>> = await authApi.post('/disable-mfa', {
      userId,
      password,
    });
    return {
      success: response.data.data!.isDisabled,
      message: response.data.message,
    };
  },

  async getMfaBackupCodes(userId: string): Promise<BackupCodesResponse> {
    const response: AxiosResponse<ApiResponse<BackupCodesResponse>> = await authApi.get(`/mfa-backup-codes/${userId}`);
    return response.data.data!;
  },

  async regenerateBackupCodes(userId: string): Promise<BackupCodesResponse> {
    const response: AxiosResponse<ApiResponse<BackupCodesResponse>> = await authApi.post('/regenerate-backup-codes', {
      userId,
    });
    return response.data.data!;
  },
};
