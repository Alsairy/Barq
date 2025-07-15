import { useState } from 'react';
import { Button } from '../ui/button';
import { CheckCircle, XCircle, Loader2, Wifi } from 'lucide-react';

interface ConnectionTestButtonProps {
  onTest: () => Promise<boolean>;
  disabled?: boolean;
  size?: 'sm' | 'default' | 'lg';
  variant?: 'default' | 'outline' | 'secondary';
  className?: string;
}

export function ConnectionTestButton({
  onTest,
  disabled = false,
  size = 'default',
  variant = 'outline',
  className
}: ConnectionTestButtonProps) {
  const [isLoading, setIsLoading] = useState(false);
  const [lastResult, setLastResult] = useState<boolean | null>(null);

  const handleTest = async () => {
    setIsLoading(true);
    setLastResult(null);
    
    try {
      const result = await onTest();
      setLastResult(result);
    } catch {
      setLastResult(false);
    } finally {
      setIsLoading(false);
    }
  };

  const getIcon = () => {
    if (isLoading) {
      return <Loader2 className="h-4 w-4 animate-spin" />;
    }
    
    if (lastResult === true) {
      return <CheckCircle className="h-4 w-4 text-green-600" />;
    }
    
    if (lastResult === false) {
      return <XCircle className="h-4 w-4 text-red-600" />;
    }
    
    return <Wifi className="h-4 w-4" />;
  };

  const getButtonText = () => {
    if (isLoading) {
      return 'Testing...';
    }
    
    if (lastResult === true) {
      return 'Connection OK';
    }
    
    if (lastResult === false) {
      return 'Connection Failed';
    }
    
    return 'Test Connection';
  };

  const getButtonVariant = () => {
    if (lastResult === true) {
      return 'default';
    }
    
    if (lastResult === false) {
      return 'destructive';
    }
    
    return variant;
  };

  return (
    <Button
      onClick={handleTest}
      disabled={disabled || isLoading}
      size={size}
      variant={getButtonVariant()}
      className={className}
    >
      {getIcon()}
      <span className="ml-2">{getButtonText()}</span>
    </Button>
  );
}
