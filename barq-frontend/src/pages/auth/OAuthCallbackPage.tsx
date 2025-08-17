import React, { useEffect, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { authService } from '../../services/authService';
import { LoadingSpinner } from '../../components/ui/loading-spinner';
import { Alert } from '../../components/ui/alert';

export const OAuthCallbackPage: React.FC = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const handleOAuthCallback = async () => {
      try {
        setLoading(true);
        setError(null);

        const code = searchParams.get('code');
        const state = searchParams.get('state');
        const error = searchParams.get('error');
        const errorDescription = searchParams.get('error_description');

        if (error) {
          throw new Error(errorDescription || error || 'OAuth authentication failed');
        }

        if (!code) {
          throw new Error('Authorization code not found in callback URL');
        }

        const authResponse = await authService.handleOAuthCallback(code, state || undefined);

        if (authResponse.success && authResponse.accessToken) {
          localStorage.setItem('accessToken', authResponse.accessToken);
          if (authResponse.refreshToken) {
            localStorage.setItem('refreshToken', authResponse.refreshToken);
          }
          
          if (authResponse.requiresMfa) {
            navigate('/auth/mfa', { 
              state: { 
                mfaToken: authResponse.mfaToken,
                userId: authResponse.userId 
              } 
            });
          } else {
            navigate('/dashboard');
          }
        } else {
          throw new Error(authResponse.message || 'Authentication failed');
        }
      } catch (err) {
        console.error('OAuth callback error:', err);
        setError(err instanceof Error ? err.message : 'An unexpected error occurred');
        
        setTimeout(() => {
          navigate('/auth/login', { 
            state: { 
              error: 'OAuth authentication failed. Please try again.' 
            } 
          });
        }, 3000);
      } finally {
        setLoading(false);
      }
    };

    handleOAuthCallback();
  }, [navigate, searchParams]);

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <div className="text-center">
          <LoadingSpinner size="lg" />
          <p className="mt-4 text-gray-600">Processing authentication...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <div className="max-w-md w-full">
          <Alert variant="destructive">
            <h3 className="font-semibold">Authentication Failed</h3>
            <p className="mt-2">{error}</p>
            <p className="mt-2 text-sm">Redirecting to login page...</p>
          </Alert>
        </div>
      </div>
    );
  }

  return null;
};
