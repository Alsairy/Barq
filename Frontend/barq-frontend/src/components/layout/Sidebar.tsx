import { NavLink } from 'react-router-dom';
import { 
  LayoutDashboard, 
  FolderOpen, 
  Bot, 
  Workflow,
  Settings,
  ChevronLeft,
  ChevronRight
} from 'lucide-react';
import { useAppSelector, useAppDispatch } from '../../hooks/redux';
import { toggleSidebar } from '../../store/slices/uiSlice';

const navigation = [
  { name: 'Dashboard', href: '/dashboard', icon: LayoutDashboard },
  { name: 'Projects', href: '/projects', icon: FolderOpen },
  { name: 'AI Management', href: '/ai', icon: Bot },
  { name: 'Workflows', href: '/workflows', icon: Workflow },
  { name: 'Settings', href: '/settings', icon: Settings },
];

export function Sidebar() {
  const { sidebarCollapsed } = useAppSelector((state) => state.ui);
  const dispatch = useAppDispatch();

  return (
    <div className={`bg-gray-900 text-white transition-all duration-300 ${
      sidebarCollapsed ? 'w-16' : 'w-64'
    }`}>
      <div className="flex items-center justify-between p-4">
        {!sidebarCollapsed && (
          <span className="text-lg font-semibold">Navigation</span>
        )}
        <button
          onClick={() => dispatch(toggleSidebar())}
          className="p-1 rounded hover:bg-gray-800"
        >
          {sidebarCollapsed ? (
            <ChevronRight className="h-4 w-4" />
          ) : (
            <ChevronLeft className="h-4 w-4" />
          )}
        </button>
      </div>
      
      <nav className="mt-8">
        <div className="space-y-1">
          {navigation.map((item) => (
            <NavLink
              key={item.name}
              to={item.href}
              className={({ isActive }) =>
                `flex items-center px-4 py-2 text-sm font-medium transition-colors ${
                  isActive
                    ? 'bg-gray-800 text-white'
                    : 'text-gray-300 hover:bg-gray-700 hover:text-white'
                }`
              }
            >
              <item.icon className="h-5 w-5 flex-shrink-0" />
              {!sidebarCollapsed && (
                <span className="ml-3">{item.name}</span>
              )}
            </NavLink>
          ))}
        </div>
      </nav>
    </div>
  );
}
