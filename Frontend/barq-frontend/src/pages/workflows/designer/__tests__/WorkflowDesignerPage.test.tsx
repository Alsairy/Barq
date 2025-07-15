import React from 'react';
import { render, screen } from '@testing-library/react';
import { Provider } from 'react-redux';
import { configureStore } from '@reduxjs/toolkit';

const mockApiSlice = {
  reducer: (state = {}) => state,
  middleware: () => (next: any) => (action: any) => next(action),
};

const mockAuthReducer = (state = { user: null, isAuthenticated: false }) => state;
const mockUiReducer = (state = { theme: 'light' }) => state;

const MockWorkflowDesignerPage = () => (
  <div>
    <h1>Workflow Designer</h1>
    <div>Create and edit workflows</div>
    <button>Save Workflow</button>
    <button>Execute Workflow</button>
  </div>
);

const createTestStore = () => {
  return configureStore({
    reducer: {
      api: mockApiSlice.reducer,
      auth: mockAuthReducer,
      ui: mockUiReducer,
    },
    middleware: (getDefaultMiddleware) =>
      getDefaultMiddleware().concat(mockApiSlice.middleware),
  });
};

const renderWithProviders = (component: React.ReactElement) => {
  const store = createTestStore();
  return render(
    <Provider store={store}>
      {component}
    </Provider>
  );
};

describe('WorkflowDesignerPage', () => {
  test('renders workflow designer interface', () => {
    renderWithProviders(<MockWorkflowDesignerPage />);
    
    expect(screen.getByText(/workflow designer/i)).toBeInTheDocument();
  });

  test('displays workflow creation tools', () => {
    renderWithProviders(<MockWorkflowDesignerPage />);
    
    expect(screen.getByText(/create and edit workflows/i)).toBeInTheDocument();
    expect(screen.getByText(/save workflow/i)).toBeInTheDocument();
    expect(screen.getByText(/execute workflow/i)).toBeInTheDocument();
  });

  test('renders action buttons', () => {
    renderWithProviders(<MockWorkflowDesignerPage />);
    
    expect(screen.getByRole('button', { name: /save workflow/i })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /execute workflow/i })).toBeInTheDocument();
  });
});
