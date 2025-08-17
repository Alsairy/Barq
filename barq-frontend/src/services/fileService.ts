import axios from 'axios';
export interface FileUploadItem {
  id: string;
  file: File;
  preview?: string;
  progress: number;
  status: 'pending' | 'uploading' | 'completed' | 'error';
  error?: string;
}

const API_BASE_URL = (import.meta as any).env.VITE_API_BASE_URL || 'https://barq-backend-tunnel-api.devinapps.com';

const fileApi = axios.create({
  baseURL: `${API_BASE_URL}/api/files`,
  headers: {
    'Content-Type': 'multipart/form-data',
  },
});

fileApi.interceptors.request.use((config) => {
  const token = localStorage.getItem('accessToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export interface FileUploadResponse {
  success: boolean;
  message: string;
  data: {
    fileId: string;
    fileName: string;
    fileSize: number;
    fileType: string;
    uploadUrl: string;
    downloadUrl: string;
    previewUrl?: string;
  };
}

export interface FileMetadata {
  id: string;
  fileName: string;
  fileSize: number;
  fileType: string;
  uploadedAt: string;
  uploadedBy: string;
  downloadUrl: string;
  previewUrl?: string;
  tags?: string[];
  description?: string;
}

export interface FileListResponse {
  success: boolean;
  message: string;
  data: {
    files: FileMetadata[];
    totalCount: number;
    pageSize: number;
    currentPage: number;
  };
}

export const fileService = {
  async uploadFile(file: File, onProgress?: (progress: number) => void): Promise<FileUploadResponse> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('fileName', file.name);
    formData.append('fileSize', file.size.toString());
    formData.append('fileType', file.type);

    const response = await fileApi.post('/upload', formData, {
      onUploadProgress: (progressEvent) => {
        if (progressEvent.total && onProgress) {
          const progress = Math.round((progressEvent.loaded * 100) / progressEvent.total);
          onProgress(progress);
        }
      },
    });

    return response.data;
  },

  async uploadMultipleFiles(
    files: FileUploadItem[],
    onProgress?: (fileId: string, progress: number) => void
  ): Promise<FileUploadResponse[]> {
    const uploadPromises = files.map(async (fileItem) => {
      try {
        const response = await this.uploadFile(fileItem.file, (progress) => {
          onProgress?.(fileItem.id, progress);
        });
        return response;
      } catch (error) {
        throw new Error(`Failed to upload ${fileItem.file.name}: ${error instanceof Error ? error.message : 'Unknown error'}`);
      }
    });

    return Promise.all(uploadPromises);
  },

  async getFiles(
    page: number = 1,
    pageSize: number = 20,
    fileType?: string,
    searchQuery?: string
  ): Promise<FileListResponse> {
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: pageSize.toString(),
    });

    if (fileType) {
      params.append('fileType', fileType);
    }

    if (searchQuery) {
      params.append('search', searchQuery);
    }

    const response = await fileApi.get(`/?${params.toString()}`);
    return response.data;
  },

  async getFileById(fileId: string): Promise<FileMetadata> {
    const response = await fileApi.get(`/${fileId}`);
    return response.data.data;
  },

  async deleteFile(fileId: string): Promise<{ success: boolean; message: string }> {
    const response = await fileApi.delete(`/${fileId}`);
    return response.data;
  },

  async downloadFile(fileId: string): Promise<Blob> {
    const response = await fileApi.get(`/${fileId}/download`, {
      responseType: 'blob',
    });
    return response.data;
  },

  async getFilePreview(fileId: string): Promise<string> {
    const response = await fileApi.get(`/${fileId}/preview`);
    return response.data.data.previewUrl;
  },

  async updateFileMetadata(
    fileId: string,
    metadata: { fileName?: string; description?: string; tags?: string[] }
  ): Promise<{ success: boolean; message: string }> {
    const response = await fileApi.put(`/${fileId}/metadata`, metadata);
    return response.data;
  },

  getFileTypeIcon(fileName: string): string {
    const extension = fileName.split('.').pop()?.toLowerCase();
    
    const iconMap: Record<string, string> = {
      jpg: '🖼️', jpeg: '🖼️', png: '🖼️', gif: '🖼️', svg: '🖼️', webp: '🖼️',
      js: '📄', jsx: '📄', ts: '📄', tsx: '📄', py: '🐍', java: '☕', 
      cpp: '⚡', c: '⚡', cs: '🔷', php: '🐘', rb: '💎', go: '🐹', rs: '🦀',
      html: '🌐', css: '🎨', scss: '🎨', json: '📋', xml: '📋', yaml: '📋', yml: '📋',
      pdf: '📕', doc: '📘', docx: '📘', txt: '📄', md: '📝',
      xls: '📊', xlsx: '📊', ppt: '📊', pptx: '📊',
      zip: '📦', tar: '📦', gz: '📦',
    };
    
    return iconMap[extension || ''] || '📄';
  },

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  },

  isImageFile(fileName: string): boolean {
    const extension = fileName.split('.').pop()?.toLowerCase();
    return ['jpg', 'jpeg', 'png', 'gif', 'svg', 'webp'].includes(extension || '');
  },

  isCodeFile(fileName: string): boolean {
    const extension = fileName.split('.').pop()?.toLowerCase();
    return ['js', 'jsx', 'ts', 'tsx', 'py', 'java', 'cpp', 'c', 'cs', 'php', 'rb', 'go', 'rs', 'html', 'css', 'scss', 'json', 'xml', 'yaml', 'yml'].includes(extension || '');
  },

  isDocumentFile(fileName: string): boolean {
    const extension = fileName.split('.').pop()?.toLowerCase();
    return ['pdf', 'doc', 'docx', 'txt', 'md', 'xls', 'xlsx', 'ppt', 'pptx'].includes(extension || '');
  }
};
