import { createBrowserRouter, Navigate } from 'react-router-dom';
import { Layout } from '../components/layout/Layout';
import { AuthLayout } from '../components/layout/AuthLayout';
import { ProtectedRoute } from '../components/auth/ProtectedRoute';

import { LoginPage } from '../pages/auth/LoginPage';
import { RegisterPage } from '../pages/auth/RegisterPage';
import { ForgotPasswordPage } from '../pages/auth/ForgotPasswordPage';

import { DashboardPage } from '../pages/dashboard/DashboardPage';

import { ProjectsPage } from '../pages/projects/ProjectsPage';
import { ProjectDetailPage } from '../pages/projects/ProjectDetailPage';

import { AIManagementPage } from '../pages/ai/AIManagementPage';
import { AITasksPage } from '../pages/ai/AITasksPage';

import { WorkflowsPage } from '../pages/workflows/WorkflowsPage';
import { WorkflowDesignerPage } from '../pages/workflows/WorkflowDesignerPage';

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
    ],
  },
  {
    path: '*',
    element: <Navigate to="/dashboard" replace />,
  },
]);
