import { Bot, Settings, BarChart, Plus } from 'lucide-react';

export function AIManagementPage() {
  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-gray-900">AI Management</h1>
        <button className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 flex items-center space-x-2">
          <Plus className="h-4 w-4" />
          <span>New AI Task</span>
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
          <div className="flex items-center space-x-3 mb-4">
            <Bot className="h-8 w-8 text-blue-600" />
            <div>
              <h3 className="text-lg font-medium text-gray-900">AI Providers</h3>
              <p className="text-sm text-gray-600">Manage AI service providers</p>
            </div>
          </div>
          <div className="space-y-2">
            <div className="flex items-center justify-between">
              <span className="text-sm text-gray-600">OpenAI</span>
              <span className="px-2 py-1 text-xs bg-green-100 text-green-800 rounded-full">Active</span>
            </div>
            <div className="flex items-center justify-between">
              <span className="text-sm text-gray-600">Anthropic</span>
              <span className="px-2 py-1 text-xs bg-green-100 text-green-800 rounded-full">Active</span>
            </div>
            <div className="flex items-center justify-between">
              <span className="text-sm text-gray-600">Google AI</span>
              <span className="px-2 py-1 text-xs bg-gray-100 text-gray-800 rounded-full">Inactive</span>
            </div>
          </div>
        </div>

        <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
          <div className="flex items-center space-x-3 mb-4">
            <BarChart className="h-8 w-8 text-green-600" />
            <div>
              <h3 className="text-lg font-medium text-gray-900">Task Analytics</h3>
              <p className="text-sm text-gray-600">AI task performance metrics</p>
            </div>
          </div>
          <div className="space-y-3">
            <div>
              <div className="flex justify-between text-sm mb-1">
                <span>Success Rate</span>
                <span>94%</span>
              </div>
              <div className="w-full bg-gray-200 rounded-full h-2">
                <div className="bg-green-600 h-2 rounded-full" style={{ width: '94%' }}></div>
              </div>
            </div>
            <div>
              <div className="flex justify-between text-sm mb-1">
                <span>Avg Response Time</span>
                <span>2.3s</span>
              </div>
            </div>
          </div>
        </div>

        <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
          <div className="flex items-center space-x-3 mb-4">
            <Settings className="h-8 w-8 text-purple-600" />
            <div>
              <h3 className="text-lg font-medium text-gray-900">Configuration</h3>
              <p className="text-sm text-gray-600">AI system settings</p>
            </div>
          </div>
          <div className="space-y-2">
            <button className="w-full text-left p-2 rounded-md border border-gray-200 hover:bg-gray-50 text-sm">
              Model Settings
            </button>
            <button className="w-full text-left p-2 rounded-md border border-gray-200 hover:bg-gray-50 text-sm">
              API Keys
            </button>
            <button className="w-full text-left p-2 rounded-md border border-gray-200 hover:bg-gray-50 text-sm">
              Rate Limits
            </button>
          </div>
        </div>
      </div>

      <div className="bg-white rounded-lg shadow-sm border border-gray-200">
        <div className="p-6 border-b border-gray-200">
          <h3 className="text-lg font-medium text-gray-900">Recent AI Tasks</h3>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Task</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Type</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Duration</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Created</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {[1, 2, 3, 4, 5].map((task) => (
                <tr key={task}>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    Code Review Task {task}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">
                    Code Analysis
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className="px-2 py-1 text-xs font-medium bg-green-100 text-green-800 rounded-full">
                      Completed
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">
                    2.3s
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">
                    2 hours ago
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
