import { ArrowLeft, Users, BarChart } from 'lucide-react';

export function ProjectDetailPage() {
  return (
    <div className="space-y-6">
      <div className="flex items-center space-x-4">
        <button className="p-2 hover:bg-gray-100 rounded-md">
          <ArrowLeft className="h-5 w-5" />
        </button>
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Project Alpha</h1>
          <p className="text-gray-600">AI-powered customer service automation</p>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="lg:col-span-2 space-y-6">
          <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
            <h3 className="text-lg font-medium text-gray-900 mb-4">Project Overview</h3>
            <p className="text-gray-600 mb-4">
              This project aims to implement an AI-powered customer service automation system
              that will reduce response times and improve customer satisfaction.
            </p>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <span className="text-sm font-medium text-gray-500">Start Date</span>
                <p className="text-gray-900">Jan 15, 2024</p>
              </div>
              <div>
                <span className="text-sm font-medium text-gray-500">End Date</span>
                <p className="text-gray-900">Dec 31, 2024</p>
              </div>
            </div>
          </div>

          <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
            <h3 className="text-lg font-medium text-gray-900 mb-4">Recent Activity</h3>
            <div className="space-y-4">
              {[1, 2, 3].map((activity) => (
                <div key={activity} className="flex items-start space-x-3">
                  <div className="w-2 h-2 bg-blue-600 rounded-full mt-2"></div>
                  <div>
                    <p className="text-sm text-gray-900">Task completed: API integration</p>
                    <p className="text-xs text-gray-500">2 hours ago</p>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>

        <div className="space-y-6">
          <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
            <div className="flex items-center space-x-2 mb-4">
              <Users className="h-5 w-5 text-gray-400" />
              <h3 className="text-lg font-medium text-gray-900">Team Members</h3>
            </div>
            <div className="space-y-3">
              {[1, 2, 3].map((member) => (
                <div key={member} className="flex items-center space-x-3">
                  <div className="w-8 h-8 bg-gray-300 rounded-full"></div>
                  <div>
                    <p className="text-sm font-medium text-gray-900">Team Member {member}</p>
                    <p className="text-xs text-gray-500">Developer</p>
                  </div>
                </div>
              ))}
            </div>
          </div>

          <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
            <div className="flex items-center space-x-2 mb-4">
              <BarChart className="h-5 w-5 text-gray-400" />
              <h3 className="text-lg font-medium text-gray-900">Progress</h3>
            </div>
            <div className="space-y-3">
              <div>
                <div className="flex justify-between text-sm mb-1">
                  <span>Overall Progress</span>
                  <span>65%</span>
                </div>
                <div className="w-full bg-gray-200 rounded-full h-2">
                  <div className="bg-blue-600 h-2 rounded-full" style={{ width: '65%' }}></div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
