import { createBrowserRouter, Navigate } from 'react-router-dom';
import { Layout } from '../components/layout/Layout';
import { AuthLayout } from '../components/layout/AuthLayout';
import { ProtectedRoute } from '../components/auth/ProtectedRoute';

import { LoginPage } from '../pages/auth/LoginPage';
import { RegisterPage } from '../pages/auth/RegisterPage';
import { ForgotPasswordPage } from '../pages/auth/ForgotPasswordPage';
import { MfaVerifyPage } from '../pages/auth/MfaVerifyPage';
import { ResetPasswordPage } from '../pages/auth/ResetPasswordPage';

import { DashboardPage } from '../pages/dashboard/DashboardPage';

import { ProjectsPage } from '../pages/projects/ProjectsPage';
import { ProjectDetailPage } from '../pages/projects/ProjectDetailPage';

import { AIManagementPage } from '../pages/ai/AIManagementPage';
import { AITasksPage } from '../pages/ai/AITasksPage';

import { WorkflowsPage } from '../pages/workflows/WorkflowsPage';
import { WorkflowDesignerPage } from '../pages/workflows/WorkflowDesignerPage';

import { UserProfilePage } from '../pages/profile/UserProfilePage';
import { OrganizationUsersPage } from '../pages/admin/OrganizationUsersPage';

import IntegrationDashboardPage from '../pages/integration/IntegrationDashboardPage';
import DevToolsIntegrationPage from '../pages/integration/DevToolsIntegrationPage';
import BusinessSystemsPage from '../pages/integration/BusinessSystemsPage';
import SecurityCompliancePage from '../pages/integration/SecurityCompliancePage';

export const router = createBrowserRouter([
  {
    path: '/auth',
    element: <AuthLayout />,
    children: [
      {
        path: 'login',
        element: <LoginPage />,
      },
      {
        path: 'register',
        element: <RegisterPage />,
      },
      {
        path: 'forgot-password',
        element: <ForgotPasswordPage />,
      },
      {
        path: 'reset-password',
        element: <ResetPasswordPage />,
      },
      {
        path: 'mfa-verify',
        element: <MfaVerifyPage />,
      },
      {
        index: true,
        element: <Navigate to="/auth/login" replace />,
      },
    ],
  },
  {
    path: '/',
    element: (
      <ProtectedRoute>
        <Layout />
      </ProtectedRoute>
    ),
    children: [
      {
        index: true,
        element: <Navigate to="/dashboard" replace />,
      },
      {
        path: 'dashboard',
        element: <DashboardPage />,
      },
      {
        path: 'projects',
        children: [
          {
            index: true,
            element: <ProjectsPage />,
          },
          {
            path: ':id',
            element: <ProjectDetailPage />,
          },
        ],
      },
      {
        path: 'ai',
        children: [
          {
            index: true,
            element: <AIManagementPage />,
          },
          {
            path: 'tasks',
            element: <AITasksPage />,
          },
        ],
      },
      {
        path: 'workflows',
        children: [
          {
            index: true,
            element: <WorkflowsPage />,
          },
          {
            path: 'designer',
            element: <WorkflowDesignerPage />,
          },
        ],
      },
      {
        path: 'profile',
        element: <UserProfilePage />,
      },
      {
        path: 'admin',
        children: [
          {
            path: 'users',
            element: <OrganizationUsersPage />,
          },
        ],
      },
      {
        path: 'integration',
        children: [
          {
            index: true,
            element: <IntegrationDashboardPage />,
          },
          {
            path: 'dev-tools',
            element: <DevToolsIntegrationPage />,
          },
          {
            path: 'business-systems',
            element: <BusinessSystemsPage />,
          },
          {
            path: 'security',
            element: <SecurityCompliancePage />,
          },
        ],
      },
    ],
  },
  {
    path: '*',
    element: <Navigate to="/dashboard" replace />,
  },
]);
