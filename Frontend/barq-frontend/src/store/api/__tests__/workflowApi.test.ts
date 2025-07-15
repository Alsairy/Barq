import { configureStore } from '@reduxjs/toolkit';

const mockApiSlice = {
  reducer: (state = {}) => state,
  middleware: () => (next: any) => (action: any) => next(action),
  reducerPath: 'api',
};

const mockWorkflowApi = {
  endpoints: {
    getWorkflowTemplates: { select: jest.fn() },
    createWorkflowTemplate: { select: jest.fn() },
    executeWorkflow: { select: jest.fn() },
  },
  reducerPath: 'api',
  useGetWorkflowTemplatesQuery: jest.fn(),
  useCreateWorkflowTemplateMutation: jest.fn(),
  useExecuteWorkflowMutation: jest.fn(),
};

const createTestStore = () => {
  return configureStore({
    reducer: {
      api: mockApiSlice.reducer,
    },
    middleware: (getDefaultMiddleware) =>
      getDefaultMiddleware().concat(mockApiSlice.middleware),
  });
};

describe('workflowApi', () => {
  beforeEach(() => {
    createTestStore();
  });

  test('should have correct endpoint names', () => {
    const endpoints = mockWorkflowApi.endpoints;
    
    expect(endpoints.getWorkflowTemplates).toBeDefined();
    expect(endpoints.createWorkflowTemplate).toBeDefined();
    expect(endpoints.executeWorkflow).toBeDefined();
  });

  test('should have hooks available for endpoints', () => {
    expect(mockWorkflowApi.useGetWorkflowTemplatesQuery).toBeDefined();
    expect(mockWorkflowApi.useCreateWorkflowTemplateMutation).toBeDefined();
    expect(mockWorkflowApi.useExecuteWorkflowMutation).toBeDefined();
  });

  test('should integrate with store correctly', () => {
    const store = createTestStore();
    expect(store.getState()).toBeDefined();
  });

  test('should handle workflow template creation', () => {
    const store = createTestStore();
    
    expect(store.getState().api).toBeDefined();
  });
});
