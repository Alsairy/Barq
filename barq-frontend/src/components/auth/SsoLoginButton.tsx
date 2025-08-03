import { Button } from '../ui/button';
import { LoadingSpinner } from '../ui/loading-spinner';
import { SsoProvider } from '../../types/auth';
import { authService } from '../../services/authService';

interface SsoLoginButtonProps {
  provider: SsoProvider;
  isLoading?: boolean;
  onLogin?: (providerId: string) => void;
  onError?: (error: string) => void;
  tenantIdentifier?: string;
}

export function SsoLoginButton({ 
  provider, 
  isLoading = false, 
  onLogin, 
  onError,
  tenantIdentifier = 'default'
}: SsoLoginButtonProps) {
  const handleClick = async () => {
    if (isLoading || !provider.isEnabled) return;

    try {
      if (onLogin) {
        onLogin(provider.id);
        return;
      }

      let response;
      
      switch (provider.type) {
        case 'oauth':
          response = await authService.initiateOAuth(provider.name, tenantIdentifier);
          if (response.success && response.authorizationUrl) {
            window.location.href = response.authorizationUrl;
          }
          break;
        case 'oidc':
          const oidcResponse = await authService.initiateOpenIdConnect(provider.name, tenantIdentifier);
          if (oidcResponse.success && oidcResponse.authorizationUrl) {
            window.location.href = oidcResponse.authorizationUrl;
          }
          break;
        case 'saml':
          const samlResponse = await authService.initiateSaml(provider.name, tenantIdentifier);
          if (samlResponse.success && samlResponse.samlRequestUrl) {
            window.location.href = samlResponse.samlRequestUrl;
          }
          break;
        default:
          throw new Error(`Unsupported SSO provider type: ${provider.type}`);
      }
      
      if (!response?.success) {
        throw new Error(response?.message || 'Failed to initiate SSO authentication');
      }
    } catch (error) {
      console.error('SSO initiation error:', error);
      const errorMessage = error instanceof Error ? error.message : 'Authentication failed';
      if (onError) {
        onError(errorMessage);
      } else {
        alert(`Authentication failed: ${errorMessage}`);
      }
    }
  };

  const getProviderIcon = () => {
    if (provider.iconUrl) {
      return <img src={provider.iconUrl} alt={provider.name} className="w-5 h-5" />;
    }
    
    switch (provider.type) {
      case 'saml':
        return <div className="w-5 h-5 bg-blue-600 rounded flex items-center justify-center text-white text-xs font-bold">S</div>;
      case 'oauth':
        return <div className="w-5 h-5 bg-green-600 rounded flex items-center justify-center text-white text-xs font-bold">O</div>;
      case 'oidc':
        return <div className="w-5 h-5 bg-purple-600 rounded flex items-center justify-center text-white text-xs font-bold">ID</div>;
      default:
        return <div className="w-5 h-5 bg-gray-600 rounded flex items-center justify-center text-white text-xs font-bold">?</div>;
    }
  };

  return (
    <Button
      variant="outline"
      className="w-full justify-start"
      onClick={handleClick}
      disabled={!provider.isEnabled || isLoading}
    >
      {isLoading ? (
        <LoadingSpinner className="mr-2 h-4 w-4" />
      ) : (
        <span className="mr-3">{getProviderIcon()}</span>
      )}
      Continue with {provider.name}
    </Button>
  );
}
