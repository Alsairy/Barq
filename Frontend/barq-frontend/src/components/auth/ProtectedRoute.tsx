import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAppSelector } from '../../hooks/redux';

interface ProtectedRouteProps {
  children: React.ReactNode;
}

export function ProtectedRoute({ children }: ProtectedRouteProps) {
  const { isAuthenticated, token } = useAppSelector((state) => state.auth);

  if (!isAuthenticated && !token) {
    return <Navigate to="/auth/login" replace />;
  }

  return <>{children}</>;
}
