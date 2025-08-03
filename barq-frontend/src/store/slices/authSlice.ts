import { createSlice, PayloadAction, createAsyncThunk } from '@reduxjs/toolkit';
import { AuthState, UserProfile, LoginRequest, RegisterRequest, MfaVerificationRequest } from '../../types/auth';
import { authService } from '../../services/authService';

const initialState: AuthState = {
  isAuthenticated: false,
  user: null,
  token: localStorage.getItem('accessToken'),
  accessToken: localStorage.getItem('accessToken'),
  refreshToken: localStorage.getItem('refreshToken'),
  tenantId: null,
  isLoading: false,
  error: null,
  requiresMfa: false,
  mfaToken: null,
};

export const loginAsync = createAsyncThunk(
  'auth/login',
  async (request: LoginRequest, { rejectWithValue }) => {
    try {
      const response = await authService.login(request);
      if (response.accessToken) {
        localStorage.setItem('accessToken', response.accessToken);
      }
      if (response.refreshToken) {
        localStorage.setItem('refreshToken', response.refreshToken);
      }
      return response;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || 'Login failed');
    }
  }
);

export const registerAsync = createAsyncThunk(
  'auth/register',
  async (request: RegisterRequest, { rejectWithValue }) => {
    try {
      const response = await authService.register(request);
      if (response.accessToken) {
        localStorage.setItem('accessToken', response.accessToken);
      }
      if (response.refreshToken) {
        localStorage.setItem('refreshToken', response.refreshToken);
      }
      return response;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || 'Registration failed');
    }
  }
);

export const verifyMfaAsync = createAsyncThunk(
  'auth/verifyMfa',
  async (request: MfaVerificationRequest, { rejectWithValue }) => {
    try {
      const response = await authService.verifyMfa(request);
      if (response.accessToken) {
        localStorage.setItem('accessToken', response.accessToken);
      }
      return response;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || 'MFA verification failed');
    }
  }
);

export const logoutAsync = createAsyncThunk(
  'auth/logout',
  async (userId: string, { rejectWithValue }) => {
    try {
      await authService.logout(userId);
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.message || 'Logout failed');
    }
  }
);

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
    setUser: (state, action: PayloadAction<UserProfile>) => {
      state.user = action.payload;
      state.isAuthenticated = true;
    },
    clearAuth: (state) => {
      state.isAuthenticated = false;
      state.user = null;
      state.token = null;
      state.accessToken = null;
      state.refreshToken = null;
      state.tenantId = null;
      state.requiresMfa = false;
      state.mfaToken = null;
      state.error = null;
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
    },
    setTokens: (state, action: PayloadAction<{ accessToken: string; refreshToken?: string }>) => {
      state.token = action.payload.accessToken;
      state.accessToken = action.payload.accessToken;
      if (action.payload.refreshToken) {
        state.refreshToken = action.payload.refreshToken;
      }
      localStorage.setItem('accessToken', action.payload.accessToken);
      if (action.payload.refreshToken) {
        localStorage.setItem('refreshToken', action.payload.refreshToken);
      }
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(loginAsync.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(loginAsync.fulfilled, (state, action) => {
        state.isLoading = false;
        if (action.payload.requiresMfa) {
          state.requiresMfa = true;
          state.mfaToken = action.payload.mfaToken || null;
        } else {
          state.isAuthenticated = true;
          state.user = action.payload.user || null;
          state.token = action.payload.accessToken || null;
          state.accessToken = action.payload.accessToken || null;
          state.refreshToken = action.payload.refreshToken || null;
          state.tenantId = action.payload.user?.organizationId || null;
          state.requiresMfa = false;
          state.mfaToken = null;
        }
      })
      .addCase(loginAsync.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload as string;
        state.isAuthenticated = false;
        state.requiresMfa = false;
        state.mfaToken = null;
      })
      .addCase(registerAsync.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(registerAsync.fulfilled, (state, action) => {
        state.isLoading = false;
        state.isAuthenticated = true;
        state.user = action.payload.user || null;
        state.token = action.payload.accessToken || null;
        state.accessToken = action.payload.accessToken || null;
        state.refreshToken = action.payload.refreshToken || null;
        state.tenantId = action.payload.user?.organizationId || null;
      })
      .addCase(registerAsync.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload as string;
        state.isAuthenticated = false;
      })
      .addCase(verifyMfaAsync.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(verifyMfaAsync.fulfilled, (state, action) => {
        state.isLoading = false;
        if (action.payload.isVerified) {
          state.isAuthenticated = true;
          state.user = action.payload.user || null;
          state.token = action.payload.accessToken || null;
          state.accessToken = action.payload.accessToken || null;
          state.tenantId = action.payload.user?.organizationId || null;
          state.requiresMfa = false;
          state.mfaToken = null;
        }
      })
      .addCase(verifyMfaAsync.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload as string;
      })
      .addCase(logoutAsync.fulfilled, (state) => {
        state.isAuthenticated = false;
        state.user = null;
        state.token = null;
        state.accessToken = null;
        state.refreshToken = null;
        state.tenantId = null;
        state.requiresMfa = false;
        state.mfaToken = null;
        state.isLoading = false;
        state.error = null;
      });
  },
});

export const { clearError, setUser, clearAuth, setTokens } = authSlice.actions;
export default authSlice.reducer;
