import { Outlet } from 'react-router-dom';

export function AuthLayout() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center">
      <div className="max-w-md w-full space-y-8 p-8">
        <div className="text-center">
          <h1 className="text-3xl font-bold text-gray-900">BARQ</h1>
          <p className="text-gray-600 mt-2">AI-Powered Project Management</p>
        </div>
        <Outlet />
      </div>
    </div>
  );
}
