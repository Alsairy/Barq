import { useState, useEffect } from 'react';
import { Github, GitBranch, FileCode, Download, Star, GitFork, ExternalLink, Search, RefreshCw } from 'lucide-react';
import { Card, CardContent, CardHeader, CardTitle } from '../ui/card';
import { Button } from '../ui/button';
import { Input } from '../ui/input';
import { Badge } from '../ui/badge';
import { ScrollArea } from '../ui/scroll-area';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../ui/tabs';

interface Repository {
  id: string;
  name: string;
  fullName: string;
  description: string;
  language: string;
  stars: number;
  forks: number;
  lastUpdated: Date;
  url: string;
  defaultBranch: string;
  isPrivate: boolean;
  size: number;
}

interface Branch {
  name: string;
  commit: {
    sha: string;
    message: string;
    author: string;
    date: Date;
  };
  isProtected: boolean;
}

interface AnalysisResult {
  repositoryId: string;
  codeQuality: {
    score: number;
    issues: string[];
    suggestions: string[];
  };
  dependencies: {
    name: string;
    version: string;
    vulnerabilities: number;
  }[];
  metrics: {
    linesOfCode: number;
    testCoverage: number;
    complexity: number;
  };
  recommendations: string[];
}

export function RepositoryIntegration() {
  const [repositories, setRepositories] = useState<Repository[]>([]);
  const [selectedRepo, setSelectedRepo] = useState<Repository | null>(null);
  const [branches, setBranches] = useState<Branch[]>([]);
  const [repoUrl, setRepoUrl] = useState('');
  const [searchQuery, setSearchQuery] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [isAnalyzing, setIsAnalyzing] = useState(false);
  const [analysisResult, setAnalysisResult] = useState<AnalysisResult | null>(null);
  const [activeTab, setActiveTab] = useState('repositories');

  useEffect(() => {
    loadMockRepositories();
  }, []);

  const loadMockRepositories = () => {
    const mockRepos: Repository[] = [
      {
        id: '1',
        name: 'barq-enterprise',
        fullName: 'Alsairy/barq-enterprise',
        description: 'Enterprise workflow management system with AI integration',
        language: 'C#',
        stars: 45,
        forks: 12,
        lastUpdated: new Date('2024-01-15'),
        url: 'https://github.com/Alsairy/barq-enterprise',
        defaultBranch: 'main',
        isPrivate: false,
        size: 15420
      },
      {
        id: '2',
        name: 'react-workflow-designer',
        fullName: 'Alsairy/react-workflow-designer',
        description: 'Visual workflow designer component for React applications',
        language: 'TypeScript',
        stars: 128,
        forks: 34,
        lastUpdated: new Date('2024-01-10'),
        url: 'https://github.com/Alsairy/react-workflow-designer',
        defaultBranch: 'main',
        isPrivate: false,
        size: 8750
      },
      {
        id: '3',
        name: 'ai-code-analyzer',
        fullName: 'Alsairy/ai-code-analyzer',
        description: 'AI-powered code analysis and quality assessment tool',
        language: 'Python',
        stars: 89,
        forks: 23,
        lastUpdated: new Date('2024-01-08'),
        url: 'https://github.com/Alsairy/ai-code-analyzer',
        defaultBranch: 'develop',
        isPrivate: true,
        size: 12300
      }
    ];
    setRepositories(mockRepos);
  };

  const handleConnectRepository = async () => {
    if (!repoUrl.trim()) return;
    
    setIsLoading(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 2000));
      
      const newRepo: Repository = {
        id: Date.now().toString(),
        name: repoUrl.split('/').pop() || 'unknown',
        fullName: repoUrl.replace('https://github.com/', ''),
        description: 'Connected repository from URL',
        language: 'Unknown',
        stars: 0,
        forks: 0,
        lastUpdated: new Date(),
        url: repoUrl,
        defaultBranch: 'main',
        isPrivate: false,
        size: 0
      };
      
      setRepositories(prev => [newRepo, ...prev]);
      setRepoUrl('');
    } catch (error) {
      console.error('Failed to connect repository:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleAnalyzeRepository = async (repo: Repository) => {
    setIsAnalyzing(true);
    setSelectedRepo(repo);
    setActiveTab('analysis');
    
    try {
      await new Promise(resolve => setTimeout(resolve, 3000));
      
      const mockAnalysis: AnalysisResult = {
        repositoryId: repo.id,
        codeQuality: {
          score: Math.floor(Math.random() * 40) + 60, // 60-100
          issues: [
            'Unused variables detected in 3 files',
            'Missing error handling in API calls',
            'Inconsistent naming conventions'
          ],
          suggestions: [
            'Add comprehensive unit tests',
            'Implement proper logging',
            'Update dependencies to latest versions'
          ]
        },
        dependencies: [
          { name: 'react', version: '18.2.0', vulnerabilities: 0 },
          { name: 'lodash', version: '4.17.20', vulnerabilities: 2 },
          { name: 'axios', version: '0.27.2', vulnerabilities: 1 }
        ],
        metrics: {
          linesOfCode: Math.floor(Math.random() * 50000) + 10000,
          testCoverage: Math.floor(Math.random() * 40) + 60,
          complexity: Math.floor(Math.random() * 20) + 5
        },
        recommendations: [
          'Implement automated testing pipeline',
          'Add code documentation',
          'Set up continuous integration',
          'Review and update security policies'
        ]
      };
      
      setAnalysisResult(mockAnalysis);
      
      const mockBranches: Branch[] = [
        {
          name: repo.defaultBranch,
          commit: {
            sha: 'abc123def456',
            message: 'feat: add new workflow features',
            author: 'Alsairy',
            date: new Date()
          },
          isProtected: true
        },
        {
          name: 'feature/ai-integration',
          commit: {
            sha: 'def456ghi789',
            message: 'wip: implementing AI service integration',
            author: 'Alsairy',
            date: new Date(Date.now() - 86400000)
          },
          isProtected: false
        }
      ];
      setBranches(mockBranches);
      
    } catch (error) {
      console.error('Failed to analyze repository:', error);
    } finally {
      setIsAnalyzing(false);
    }
  };

  const handleBrowseRepository = (repo: Repository) => {
    setSelectedRepo(repo);
    setActiveTab('browse');
    
    const mockBranches: Branch[] = [
      {
        name: repo.defaultBranch,
        commit: {
          sha: 'abc123def456',
          message: 'feat: add new workflow features',
          author: 'Alsairy',
          date: new Date()
        },
        isProtected: true
      },
      {
        name: 'feature/ai-integration',
        commit: {
          sha: 'def456ghi789',
          message: 'wip: implementing AI service integration',
          author: 'Alsairy',
          date: new Date(Date.now() - 86400000)
        },
        isProtected: false
      }
    ];
    setBranches(mockBranches);
  };

  const filteredRepositories = repositories.filter(repo =>
    repo.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
    repo.description.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const formatFileSize = (bytes: number): string => {
    if (bytes === 0) return '0 KB';
    const k = 1024;
    const sizes = ['KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(1)) + ' ' + sizes[i];
  };

  const getQualityColor = (score: number): string => {
    if (score >= 80) return 'text-green-600';
    if (score >= 60) return 'text-yellow-600';
    return 'text-red-600';
  };

  return (
    <div className="space-y-6">
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Github className="h-5 w-5" />
            Repository Integration
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="flex gap-2 mb-4">
            <Input
              placeholder="https://github.com/username/repository"
              value={repoUrl}
              onChange={(e) => setRepoUrl(e.target.value)}
              className="flex-1"
            />
            <Button 
              onClick={handleConnectRepository}
              disabled={isLoading || !repoUrl.trim()}
            >
              {isLoading ? (
                <RefreshCw className="h-4 w-4 mr-2 animate-spin" />
              ) : (
                <Github className="h-4 w-4 mr-2" />
              )}
              Connect Repository
            </Button>
          </div>
          
          <div className="flex gap-2 mb-4">
            <div className="relative flex-1">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
              <Input
                placeholder="Search repositories..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className="pl-10"
              />
            </div>
          </div>
        </CardContent>
      </Card>

      <Tabs value={activeTab} onValueChange={setActiveTab}>
        <TabsList className="grid w-full grid-cols-3">
          <TabsTrigger value="repositories">Repositories</TabsTrigger>
          <TabsTrigger value="browse">Browse</TabsTrigger>
          <TabsTrigger value="analysis">Analysis</TabsTrigger>
        </TabsList>

        <TabsContent value="repositories" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Connected Repositories ({filteredRepositories.length})</CardTitle>
            </CardHeader>
            <CardContent>
              <ScrollArea className="h-96">
                <div className="space-y-4">
                  {filteredRepositories.map((repo) => (
                    <div key={repo.id} className="border rounded-lg p-4">
                      <div className="flex items-start justify-between">
                        <div className="flex-1">
                          <div className="flex items-center gap-2 mb-2">
                            <h3 className="font-semibold">{repo.name}</h3>
                            {repo.isPrivate && (
                              <Badge variant="secondary" className="text-xs">Private</Badge>
                            )}
                            <Badge variant="outline" className="text-xs">{repo.language}</Badge>
                          </div>
                          <p className="text-sm text-gray-600 mb-3">{repo.description}</p>
                          <div className="flex items-center gap-4 text-xs text-gray-500">
                            <div className="flex items-center gap-1">
                              <Star className="h-3 w-3" />
                              {repo.stars}
                            </div>
                            <div className="flex items-center gap-1">
                              <GitFork className="h-3 w-3" />
                              {repo.forks}
                            </div>
                            <span>{formatFileSize(repo.size)}</span>
                            <span>Updated {repo.lastUpdated.toLocaleDateString()}</span>
                          </div>
                        </div>
                        <div className="flex gap-2 ml-4">
                          <Button 
                            size="sm" 
                            variant="outline" 
                            onClick={() => handleBrowseRepository(repo)}
                          >
                            <GitBranch className="h-4 w-4 mr-1" />
                            Browse
                          </Button>
                          <Button 
                            size="sm" 
                            onClick={() => handleAnalyzeRepository(repo)}
                            disabled={isAnalyzing}
                          >
                            {isAnalyzing && selectedRepo?.id === repo.id ? (
                              <RefreshCw className="h-4 w-4 mr-1 animate-spin" />
                            ) : (
                              <FileCode className="h-4 w-4 mr-1" />
                            )}
                            Analyze
                          </Button>
                          <Button 
                            size="sm" 
                            variant="ghost"
                            onClick={() => window.open(repo.url, '_blank')}
                          >
                            <ExternalLink className="h-4 w-4" />
                          </Button>
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              </ScrollArea>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="browse" className="space-y-4">
          {selectedRepo ? (
            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2">
                  <GitBranch className="h-5 w-5" />
                  {selectedRepo.name} - Branches
                </CardTitle>
              </CardHeader>
              <CardContent>
                <div className="space-y-3">
                  {branches.map((branch) => (
                    <div key={branch.name} className="border rounded-lg p-3">
                      <div className="flex items-center justify-between">
                        <div>
                          <div className="flex items-center gap-2 mb-1">
                            <span className="font-medium">{branch.name}</span>
                            {branch.isProtected && (
                              <Badge variant="secondary" className="text-xs">Protected</Badge>
                            )}
                            {branch.name === selectedRepo.defaultBranch && (
                              <Badge variant="outline" className="text-xs">Default</Badge>
                            )}
                          </div>
                          <p className="text-sm text-gray-600 mb-1">{branch.commit.message}</p>
                          <div className="text-xs text-gray-500">
                            {branch.commit.author} • {branch.commit.date.toLocaleDateString()} • {branch.commit.sha.substring(0, 7)}
                          </div>
                        </div>
                        <Button size="sm" variant="outline">
                          <Download className="h-4 w-4 mr-1" />
                          Download
                        </Button>
                      </div>
                    </div>
                  ))}
                </div>
              </CardContent>
            </Card>
          ) : (
            <Card>
              <CardContent className="text-center py-8">
                <GitBranch className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                <p className="text-gray-600">Select a repository to browse its branches</p>
              </CardContent>
            </Card>
          )}
        </TabsContent>

        <TabsContent value="analysis" className="space-y-4">
          {analysisResult ? (
            <div className="grid gap-4">
              <Card>
                <CardHeader>
                  <CardTitle className="flex items-center gap-2">
                    <FileCode className="h-5 w-5" />
                    Code Quality Analysis
                  </CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="space-y-4">
                    <div className="flex items-center gap-4">
                      <div className="text-center">
                        <div className={`text-2xl font-bold ${getQualityColor(analysisResult.codeQuality.score)}`}>
                          {analysisResult.codeQuality.score}%
                        </div>
                        <div className="text-sm text-gray-600">Quality Score</div>
                      </div>
                      <div className="grid grid-cols-3 gap-4 flex-1">
                        <div className="text-center">
                          <div className="text-lg font-semibold">{analysisResult.metrics.linesOfCode.toLocaleString()}</div>
                          <div className="text-xs text-gray-600">Lines of Code</div>
                        </div>
                        <div className="text-center">
                          <div className="text-lg font-semibold">{analysisResult.metrics.testCoverage}%</div>
                          <div className="text-xs text-gray-600">Test Coverage</div>
                        </div>
                        <div className="text-center">
                          <div className="text-lg font-semibold">{analysisResult.metrics.complexity}</div>
                          <div className="text-xs text-gray-600">Complexity</div>
                        </div>
                      </div>
                    </div>
                    
                    <div className="grid md:grid-cols-2 gap-4">
                      <div>
                        <h4 className="font-medium mb-2">Issues Found</h4>
                        <ul className="text-sm space-y-1">
                          {analysisResult.codeQuality.issues.map((issue, index) => (
                            <li key={index} className="text-red-600">• {issue}</li>
                          ))}
                        </ul>
                      </div>
                      <div>
                        <h4 className="font-medium mb-2">Suggestions</h4>
                        <ul className="text-sm space-y-1">
                          {analysisResult.codeQuality.suggestions.map((suggestion, index) => (
                            <li key={index} className="text-blue-600">• {suggestion}</li>
                          ))}
                        </ul>
                      </div>
                    </div>
                  </div>
                </CardContent>
              </Card>

              <Card>
                <CardHeader>
                  <CardTitle>Dependencies & Security</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="space-y-3">
                    {analysisResult.dependencies.map((dep, index) => (
                      <div key={index} className="flex items-center justify-between p-2 border rounded">
                        <div>
                          <span className="font-medium">{dep.name}</span>
                          <span className="text-sm text-gray-600 ml-2">v{dep.version}</span>
                        </div>
                        {dep.vulnerabilities > 0 ? (
                          <Badge variant="destructive" className="text-xs">
                            {dep.vulnerabilities} vulnerabilities
                          </Badge>
                        ) : (
                          <Badge variant="secondary" className="text-xs">Secure</Badge>
                        )}
                      </div>
                    ))}
                  </div>
                </CardContent>
              </Card>

              <Card>
                <CardHeader>
                  <CardTitle>Recommendations</CardTitle>
                </CardHeader>
                <CardContent>
                  <ul className="space-y-2">
                    {analysisResult.recommendations.map((rec, index) => (
                      <li key={index} className="flex items-start gap-2">
                        <div className="w-2 h-2 bg-blue-500 rounded-full mt-2 flex-shrink-0"></div>
                        <span className="text-sm">{rec}</span>
                      </li>
                    ))}
                  </ul>
                </CardContent>
              </Card>
            </div>
          ) : (
            <Card>
              <CardContent className="text-center py-8">
                {isAnalyzing ? (
                  <div>
                    <RefreshCw className="h-12 w-12 text-blue-500 mx-auto mb-4 animate-spin" />
                    <p className="text-gray-600">Analyzing repository...</p>
                  </div>
                ) : (
                  <div>
                    <FileCode className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                    <p className="text-gray-600">Select a repository and click "Analyze" to view detailed analysis</p>
                  </div>
                )}
              </CardContent>
            </Card>
          )}
        </TabsContent>
      </Tabs>
    </div>
  );
}
