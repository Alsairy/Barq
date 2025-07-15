import { Save, Play, ArrowLeft, Plus } from 'lucide-react';

export function WorkflowDesignerPage() {
  return (
    <div className="h-screen flex flex-col">
      <div className="bg-white border-b border-gray-200 px-6 py-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center space-x-4">
            <button className="p-2 hover:bg-gray-100 rounded-md">
              <ArrowLeft className="h-5 w-5" />
            </button>
            <div>
              <h1 className="text-xl font-bold text-gray-900">Workflow Designer</h1>
              <p className="text-sm text-gray-600">Create and edit automated workflows</p>
            </div>
          </div>
          <div className="flex items-center space-x-2">
            <button className="bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700 flex items-center space-x-2">
              <Play className="h-4 w-4" />
              <span>Test Run</span>
            </button>
            <button className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 flex items-center space-x-2">
              <Save className="h-4 w-4" />
              <span>Save</span>
            </button>
          </div>
        </div>
      </div>

      <div className="flex-1 flex">
        <div className="w-64 bg-gray-50 border-r border-gray-200 p-4">
          <h3 className="text-sm font-medium text-gray-900 mb-4">Workflow Components</h3>
          <div className="space-y-2">
            <div className="p-3 bg-white rounded-md border border-gray-200 cursor-pointer hover:bg-gray-50">
              <div className="flex items-center space-x-2">
                <div className="w-3 h-3 bg-blue-600 rounded-full"></div>
                <span className="text-sm text-gray-900">Start</span>
              </div>
            </div>
            <div className="p-3 bg-white rounded-md border border-gray-200 cursor-pointer hover:bg-gray-50">
              <div className="flex items-center space-x-2">
                <div className="w-3 h-3 bg-green-600 rounded-full"></div>
                <span className="text-sm text-gray-900">AI Task</span>
              </div>
            </div>
            <div className="p-3 bg-white rounded-md border border-gray-200 cursor-pointer hover:bg-gray-50">
              <div className="flex items-center space-x-2">
                <div className="w-3 h-3 bg-yellow-600 rounded-full"></div>
                <span className="text-sm text-gray-900">Condition</span>
              </div>
            </div>
            <div className="p-3 bg-white rounded-md border border-gray-200 cursor-pointer hover:bg-gray-50">
              <div className="flex items-center space-x-2">
                <div className="w-3 h-3 bg-purple-600 rounded-full"></div>
                <span className="text-sm text-gray-900">Action</span>
              </div>
            </div>
            <div className="p-3 bg-white rounded-md border border-gray-200 cursor-pointer hover:bg-gray-50">
              <div className="flex items-center space-x-2">
                <div className="w-3 h-3 bg-red-600 rounded-full"></div>
                <span className="text-sm text-gray-900">End</span>
              </div>
            </div>
          </div>
        </div>

        <div className="flex-1 bg-gray-100 relative">
          <div className="absolute inset-0 p-8">
            <div className="bg-white rounded-lg shadow-sm border border-gray-200 h-full p-6">
              <div className="text-center text-gray-500 mt-20">
                <Plus className="h-12 w-12 mx-auto mb-4 text-gray-400" />
                <h3 className="text-lg font-medium text-gray-900 mb-2">Design Your Workflow</h3>
                <p className="text-gray-600">
                  Drag and drop components from the sidebar to create your automated workflow.
                </p>
              </div>
            </div>
          </div>
        </div>

        <div className="w-80 bg-white border-l border-gray-200 p-4">
          <h3 className="text-sm font-medium text-gray-900 mb-4">Properties</h3>
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Workflow Name
              </label>
              <input
                type="text"
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Enter workflow name"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Description
              </label>
              <textarea
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                rows={3}
                placeholder="Describe your workflow"
              ></textarea>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Trigger
              </label>
              <select className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500">
                <option>Manual</option>
                <option>Schedule</option>
                <option>Event</option>
              </select>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
