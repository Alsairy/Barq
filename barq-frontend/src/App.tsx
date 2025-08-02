import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import { Toaster } from 'sonner'
import { ThemeProvider } from 'next-themes'
import Layout from './components/Layout'
import Dashboard from './components/Dashboard'
import AIRequestsPage from './components/AIRequests/AIRequestsPage'
import WorkflowsPage from './components/Workflows/WorkflowsPage'
import QualityAssurancePage from './components/QualityAssurance/QualityAssurancePage'
import './App.css'

function App() {
  return (
    <ThemeProvider attribute="class" defaultTheme="system" enableSystem>
      <Router>
        <Layout>
          <Routes>
            <Route path="/" element={<Dashboard />} />
            <Route path="/ai-requests" element={<AIRequestsPage />} />
            <Route path="/workflows" element={<WorkflowsPage />} />
            <Route path="/quality-assurance" element={<QualityAssurancePage />} />
          </Routes>
        </Layout>
        <Toaster />
      </Router>
    </ThemeProvider>
  )
}

export default App
