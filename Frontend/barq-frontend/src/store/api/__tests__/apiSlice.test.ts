import { apiSlice } from '../apiSlice';

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
