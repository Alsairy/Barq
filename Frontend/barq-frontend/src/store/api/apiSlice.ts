import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { RootState } from '../index';

const baseQuery = fetchBaseQuery({
  baseUrl: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5001',
  prepareHeaders: (headers, { getState }) => {
    const token = (getState() as RootState).auth.token;
    const tenantId = (getState() as RootState).auth.tenantId;
    
    if (token) {
      headers.set('authorization', `Bearer ${token}`);
    }
    
    if (tenantId) {
      headers.set('X-Tenant-ID', tenantId);
    }
    
    headers.set('Content-Type', 'application/json');
    return headers;
  },
});

export const apiSlice = createApi({
  reducerPath: 'api',
  baseQuery,
  tagTypes: [
    'User',
    'Organization', 
    'Project',
    'Workflow',
    'AITask',
    'Notification',
    'Integration'
  ],
  endpoints: () => ({}),
});
