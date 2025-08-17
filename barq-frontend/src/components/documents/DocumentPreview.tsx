import React from 'react';
import DocumentViewer from './DocumentViewer';

interface DocumentPreviewProps {
  file: File | null;
  fileUrl?: string;
  onClose: () => void;
  onDownload?: () => void;
}

export const DocumentPreview: React.FC<DocumentPreviewProps> = ({
  file,
  fileUrl,
  onClose,
  onDownload
}) => {
  if (!file && !fileUrl) {
    return null;
  }

  const displayUrl = fileUrl || (file ? URL.createObjectURL(file) : '');
  const fileName = file?.name || 'Document';
  const fileType = file?.type || 'application/octet-stream';

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg shadow-xl max-w-4xl max-h-[90vh] w-full mx-4 overflow-hidden">
        <div className="flex items-center justify-between p-4 border-b">
          <h2 className="text-xl font-semibold">Document Preview</h2>
          <button
            onClick={onClose}
            className="text-gray-500 hover:text-gray-700 text-2xl"
          >
            ×
          </button>
        </div>
        
        <div className="overflow-auto max-h-[calc(90vh-80px)]">
          <DocumentViewer
            fileUrl={displayUrl}
            fileName={fileName}
            fileType={fileType}
            onDownload={onDownload}
          />
        </div>
      </div>
    </div>
  );
};

export default DocumentPreview;
