import { Button } from '../ui';
import { LoadingSpinner } from '../ui/loading-spinner';
import { SsoProvider } from '../../types/auth';

interface SsoLoginButtonProps {
  provider: SsoProvider;
  isLoading?: boolean;
  onLogin: (providerId: string) => void;
}

export function SsoLoginButton({ provider, isLoading = false, onLogin }: SsoLoginButtonProps) {
  const handleClick = () => {
    if (!isLoading && provider.isEnabled) {
      onLogin(provider.id);
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
