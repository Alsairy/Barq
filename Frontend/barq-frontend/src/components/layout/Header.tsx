import { Bell, Settings, User } from 'lucide-react';
import { useAppSelector } from '../../hooks/redux';

export function Header() {
  const { user } = useAppSelector((state) => state.auth);

  return (
    <header className="bg-white shadow-sm border-b border-gray-200">
      <div className="flex items-center justify-between px-6 py-4">
        <div className="flex items-center space-x-4">
          <h1 className="text-xl font-semibold text-gray-900">BARQ</h1>
        </div>
        
        <div className="flex items-center space-x-4">
          <button className="p-2 text-gray-400 hover:text-gray-600">
            <Bell className="h-5 w-5" />
          </button>
          <button className="p-2 text-gray-400 hover:text-gray-600">
            <Settings className="h-5 w-5" />
          </button>
          <div className="flex items-center space-x-2">
            <User className="h-5 w-5 text-gray-400" />
            <span className="text-sm text-gray-700">
              {user?.firstName} {user?.lastName}
            </span>
          </div>
        </div>
      </div>
    </header>
  );
}
