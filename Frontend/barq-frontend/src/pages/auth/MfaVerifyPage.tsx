import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { Shield, AlertCircle, RefreshCw } from 'lucide-react';
import { Button } from '../../components/ui';
import { Input } from '../../components/ui';
import { Label } from '../../components/ui';
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '../../components/ui';
import { Alert, AlertDescription } from '../../components/ui';
import { LoadingSpinner } from '../../components/ui/loading-spinner';
import { verifyMfaAsync, clearError } from '../../store/slices/authSlice';
import { RootState, AppDispatch } from '../../store';
import { MfaVerificationRequest } from '../../types/auth';

export function MfaVerifyPage() {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { isLoading, error, mfaToken, requiresMfa } = useSelector((state: RootState) => state.auth);

  const [formData, setFormData] = useState<MfaVerificationRequest>({
    userId: '', // This should be set from the auth state
    code: '',
    backupCode: '',
  });
  const [useBackupCode, setUseBackupCode] = useState(false);
  const [validationErrors, setValidationErrors] = useState<Record<string, string>>({});

  const validateForm = (): boolean => {
    const errors: Record<string, string> = {};

    if (useBackupCode) {
      if (!formData.backupCode?.trim()) {
        errors.backupCode = 'Backup code is required';
      } else if (formData.backupCode.length !== 8) {
        errors.backupCode = 'Backup code must be 8 characters';
      }
    } else {
      if (!formData.code.trim()) {
        errors.code = 'Verification code is required';
      } else if (formData.code.length !== 6) {
        errors.code = 'Verification code must be 6 digits';
      } else if (!/^\d{6}$/.test(formData.code)) {
        errors.code = 'Verification code must contain only numbers';
      }
    }

    setValidationErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    if (!mfaToken) {
      setValidationErrors({ general: 'MFA session expired. Please log in again.' });
      navigate('/auth/login');
      return;
    }

    dispatch(clearError());
    const result = await dispatch(verifyMfaAsync({
      ...formData,
      userId: mfaToken, // Using mfaToken as userId for now
    }));
    
    if (verifyMfaAsync.fulfilled.match(result)) {
      navigate('/dashboard');
    }
  };


  const handleCodeInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value.replace(/\D/g, '').slice(0, 6);
    setFormData(prev => ({ ...prev, code: value }));
    
    if (validationErrors.code) {
      setValidationErrors(prev => ({ ...prev, code: '' }));
    }
  };

  const handleBackupCodeInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value.toUpperCase().slice(0, 8);
    setFormData(prev => ({ ...prev, backupCode: value }));
    
    if (validationErrors.backupCode) {
      setValidationErrors(prev => ({ ...prev, backupCode: '' }));
    }
  };

  if (!requiresMfa) {
    navigate('/dashboard');
    return null;
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 to-indigo-100 p-4">
      <Card className="w-full max-w-md">
        <CardHeader className="space-y-1 text-center">
          <div className="mx-auto w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center mb-4">
            <Shield className="h-6 w-6 text-blue-600" />
          </div>
          <CardTitle className="text-2xl font-bold">Two-Factor Authentication</CardTitle>
          <CardDescription>
            {useBackupCode 
              ? 'Enter one of your backup codes to continue'
              : 'Enter the 6-digit code from your authenticator app'
            }
          </CardDescription>
        </CardHeader>
        
        <form onSubmit={handleSubmit}>
          <CardContent className="space-y-4">
            {(error || validationErrors.general) && (
              <Alert variant="destructive">
                <AlertCircle className="h-4 w-4" />
                <AlertDescription>{error || validationErrors.general}</AlertDescription>
              </Alert>
            )}

            {!useBackupCode ? (
              <div className="space-y-2">
                <Label htmlFor="code">Verification Code</Label>
                <Input
                  id="code"
                  type="text"
                  placeholder="000000"
                  value={formData.code}
                  onChange={handleCodeInput}
                  className={`text-center text-2xl tracking-widest ${validationErrors.code ? 'border-red-500' : ''}`}
                  maxLength={6}
                  disabled={isLoading}
                  autoComplete="one-time-code"
                />
                {validationErrors.code && (
                  <p className="text-sm text-red-500">{validationErrors.code}</p>
                )}
                <p className="text-sm text-gray-600 text-center">
                  Open your authenticator app and enter the 6-digit code
                </p>
              </div>
            ) : (
              <div className="space-y-2">
                <Label htmlFor="backupCode">Backup Code</Label>
                <Input
                  id="backupCode"
                  type="text"
                  placeholder="XXXXXXXX"
                  value={formData.backupCode}
                  onChange={handleBackupCodeInput}
                  className={`text-center text-xl tracking-widest ${validationErrors.backupCode ? 'border-red-500' : ''}`}
                  maxLength={8}
                  disabled={isLoading}
                />
                {validationErrors.backupCode && (
                  <p className="text-sm text-red-500">{validationErrors.backupCode}</p>
                )}
                <p className="text-sm text-gray-600 text-center">
                  Enter one of your 8-character backup codes
                </p>
              </div>
            )}

            <div className="text-center">
              <button
                type="button"
                onClick={() => {
                  setUseBackupCode(!useBackupCode);
                  setFormData(prev => ({ ...prev, code: '', backupCode: '' }));
                  setValidationErrors({});
                }}
                className="text-sm text-blue-600 hover:text-blue-500 underline"
                disabled={isLoading}
              >
                {useBackupCode 
                  ? 'Use authenticator app instead' 
                  : 'Use backup code instead'
                }
              </button>
            </div>
          </CardContent>

          <CardFooter className="flex flex-col space-y-4">
            <Button
              type="submit"
              className="w-full"
              disabled={isLoading}
            >
              {isLoading ? (
                <>
                  <LoadingSpinner className="mr-2 h-4 w-4" />
                  Verifying...
                </>
              ) : (
                'Verify'
              )}
            </Button>

            <div className="flex items-center justify-center space-x-4 text-sm">
              <button
                type="button"
                onClick={() => navigate('/auth/login')}
                className="text-gray-600 hover:text-gray-500"
                disabled={isLoading}
              >
                Back to login
              </button>
              <span className="text-gray-300">•</span>
              <button
                type="button"
                onClick={() => {
                  window.location.reload();
                }}
                className="text-blue-600 hover:text-blue-500 flex items-center"
                disabled={isLoading}
              >
                <RefreshCw className="h-3 w-3 mr-1" />
                Refresh
              </button>
            </div>
          </CardFooter>
        </form>
      </Card>
    </div>
  );
}
