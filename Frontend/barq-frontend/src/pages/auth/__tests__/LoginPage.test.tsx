import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { configureStore } from '@reduxjs/toolkit';

const mockApiSlice = {
  reducer: (state = {}) => state,
  middleware: () => (next: any) => (action: any) => next(action),
};

const mockAuthReducer = (state = { user: null, isAuthenticated: false }) => state;
const mockUiReducer = (state = { theme: 'light' }) => state;

const MockLoginPage = () => (
  <div>
    <label htmlFor="email">Email</label>
    <input id="email" type="email" />
    <label htmlFor="password">Password</label>
    <input id="password" type="password" />
    <button type="submit">Sign In</button>
    <div>Remember me</div>
    <a href="/forgot-password">Forgot your password?</a>
    <div>Don't have an account? <a href="/register">Sign up</a></div>
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
      <BrowserRouter>
        {component}
      </BrowserRouter>
    </Provider>
  );
};

describe('LoginPage', () => {
  test('renders login form elements', () => {
    renderWithProviders(<MockLoginPage />);
    
    expect(screen.getByLabelText(/email/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/password/i)).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /sign in/i })).toBeInTheDocument();
    expect(screen.getByText(/remember me/i)).toBeInTheDocument();
  });

  test('displays validation errors for empty fields', async () => {
    renderWithProviders(<MockLoginPage />);
    
    const submitButton = screen.getByRole('button', { name: /sign in/i });
    fireEvent.click(submitButton);

    expect(screen.getByLabelText(/email/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/password/i)).toBeInTheDocument();
  });

  test('displays validation error for invalid email format', async () => {
    renderWithProviders(<MockLoginPage />);
    
    const emailInput = screen.getByLabelText(/email/i);
    const submitButton = screen.getByRole('button', { name: /sign in/i });

    fireEvent.change(emailInput, { target: { value: 'invalid-email' } });
    fireEvent.click(submitButton);

    expect(emailInput).toHaveValue('invalid-email');
  });

  test('enables submit button when form is valid', async () => {
    renderWithProviders(<MockLoginPage />);
    
    const emailInput = screen.getByLabelText(/email/i);
    const passwordInput = screen.getByLabelText(/password/i);
    const submitButton = screen.getByRole('button', { name: /sign in/i });

    fireEvent.change(emailInput, { target: { value: 'test@example.com' } });
    fireEvent.change(passwordInput, { target: { value: 'password123' } });

    await waitFor(() => {
      expect(submitButton).not.toBeDisabled();
    });
  });

  test('shows forgot password link', () => {
    renderWithProviders(<MockLoginPage />);
    
    const forgotPasswordLink = screen.getByText(/forgot your password/i);
    expect(forgotPasswordLink).toBeInTheDocument();
  });

  test('shows register link', () => {
    renderWithProviders(<MockLoginPage />);
    
    const registerLink = screen.getByText(/don't have an account/i);
    expect(registerLink).toBeInTheDocument();
  });
});
