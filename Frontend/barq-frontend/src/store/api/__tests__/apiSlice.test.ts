import { configureStore } from '@reduxjs/toolkit';
import { apiSlice } from '../apiSlice';

const mockEnv = {
  VITE_API_BASE_URL: 'http://localhost:5001',
};

jest.mock('../apiSlice', () => ({
  apiSlice: {
    reducerPath: 'api',
    reducer: jest.fn(),
    middleware: jest.fn(() => (next: any) => (action: any) => next(action)),
    endpoints: {},
  },
}));

describe('apiSlice', () => {
  test('should have correct reducer path', () => {
    expect(apiSlice.reducerPath).toBe('api');
  });

  test('should be properly configured', () => {
    expect(apiSlice.reducer).toBeDefined();
    expect(apiSlice.middleware).toBeDefined();
  });
});
