import React from 'react';
import { cn } from '../../lib/utils';

interface FocusVisibleProps extends React.HTMLAttributes<HTMLDivElement> {
  children: React.ReactNode;
  focusClassName?: string;
}

export function FocusVisible({ 
  children, 
  className, 
  focusClassName = 'ring-2 ring-blue-500 ring-offset-2',
  ...props 
}: FocusVisibleProps) {
  return (
    <div
      className={cn(
        'focus-visible:outline-none',
        `focus-visible:${focusClassName}`,
        className
      )}
      {...props}
    >
      {children}
    </div>
  );
}
