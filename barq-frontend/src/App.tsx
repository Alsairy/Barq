import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import { Provider } from 'react-redux'
import { Toaster } from 'sonner'
import { ThemeProvider } from 'next-themes'
import { store } from './store'
import Layout from './components/Layout'
import Dashboard from './components/Dashboard'
import AIRequestsPage from './components/AIRequests/AIRequestsPage'
import WorkflowsPage from './components/Workflows/WorkflowsPage'
import QualityAssurancePage from './components/QualityAssurance/QualityAssurancePage'
import { CodeGenerationPage } from './pages/ai/CodeGenerationPage'
import { BRDGenerationPage } from './pages/ai/BRDGenerationPage'
import { AIRequestManagementPage } from './pages/ai/AIRequestManagementPage'
import { LoginPage } from './pages/auth/LoginPage'
import { ProtectedRoute } from './components/auth/ProtectedRoute'
import './App.css'

function App() {
  return (
    <Provider store={store}>
      <ThemeProvider attribute="class" defaultTheme="system" enableSystem>
        <Router>
          <Routes>
            <Route path="/auth/login" element={<LoginPage />} />
            <Route path="/" element={
              <ProtectedRoute>
                <Layout>
                  <Dashboard />
                </Layout>
              </ProtectedRoute>
            } />
            <Route path="/dashboard" element={
              <ProtectedRoute>
                <Layout>
                  <Dashboard />
                </Layout>
              </ProtectedRoute>
            } />
            <Route path="/ai-requests" element={
              <ProtectedRoute>
                <Layout>
                  <AIRequestsPage />
                </Layout>
              </ProtectedRoute>
            } />
            <Route path="/workflows" element={
              <ProtectedRoute>
                <Layout>
                  <WorkflowsPage />
                </Layout>
              </ProtectedRoute>
            } />
            <Route path="/quality-assurance" element={
              <ProtectedRoute>
                <Layout>
                  <QualityAssurancePage />
                </Layout>
              </ProtectedRoute>
            } />
            <Route path="/ai/code-generation" element={
              <ProtectedRoute>
                <Layout>
                  <CodeGenerationPage />
                </Layout>
              </ProtectedRoute>
            } />
            <Route path="/ai/brd-generation" element={
              <ProtectedRoute>
                <Layout>
                  <BRDGenerationPage />
                </Layout>
              </ProtectedRoute>
            } />
            <Route path="/ai/requests" element={
              <ProtectedRoute>
                <Layout>
                  <AIRequestManagementPage />
                </Layout>
              </ProtectedRoute>
            } />
          </Routes>
          <Toaster />
        </Router>
      </ThemeProvider>
    </Provider>
  )
}

export default App
