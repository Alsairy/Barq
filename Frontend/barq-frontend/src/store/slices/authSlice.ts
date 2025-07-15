import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  roles: string[];
  permissions: string[];
}

interface AuthState {
  user: User | null;
  token: string | null;
  tenantId: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string | null;
}

const initialState: AuthState = {
  user: null,
  token: localStorage.getItem('token'),
  tenantId: localStorage.getItem('tenantId'),
  isAuthenticated: false,
  isLoading: false,
  error: null,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    loginStart: (state) => {
      state.isLoading = true;
      state.error = null;
    },
    loginSuccess: (state, action: PayloadAction<{ user: User; token: string; tenantId: string }>) => {
      state.user = action.payload.user;
      state.token = action.payload.token;
      state.tenantId = action.payload.tenantId;
      state.isAuthenticated = true;
      state.isLoading = false;
      state.error = null;
      localStorage.setItem('token', action.payload.token);
      localStorage.setItem('tenantId', action.payload.tenantId);
    },
    loginFailure: (state, action: PayloadAction<string>) => {
      state.user = null;
      state.token = null;
      state.tenantId = null;
      state.isAuthenticated = false;
      state.isLoading = false;
      state.error = action.payload;
      localStorage.removeItem('token');
      localStorage.removeItem('tenantId');
    },
    logout: (state) => {
      state.user = null;
      state.token = null;
      state.tenantId = null;
      state.isAuthenticated = false;
      state.isLoading = false;
      state.error = null;
      localStorage.removeItem('token');
      localStorage.removeItem('tenantId');
    },
    clearError: (state) => {
      state.error = null;
    },
  },
});

export const { loginStart, loginSuccess, loginFailure, logout, clearError } = authSlice.actions;
export default authSlice.reducer;
