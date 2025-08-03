import React, { useState, useEffect } from 'react';
import { Document, Page, pdfjs } from 'react-pdf';
import mammoth from 'mammoth';
import * as XLSX from 'xlsx';

pdfjs.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjs.version}/pdf.worker.min.js`;

interface DocumentViewerProps {
  fileUrl: string;
  fileName: string;
  fileType: string;
  onDownload?: () => void;
}

export const DocumentViewer: React.FC<DocumentViewerProps> = ({
  fileUrl,
  fileName,
  fileType,
  onDownload
}) => {
  const [numPages, setNumPages] = useState<number>(0);
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [documentContent, setDocumentContent] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    loadDocument();
  }, [fileUrl, fileType]);

  const loadDocument = async () => {
    setLoading(true);
    setError('');
    
    try {
      if (fileType === 'application/pdf') {
        setLoading(false);
      } else if (fileType === 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' || 
                 fileType === 'application/msword') {
        const response = await fetch(fileUrl);
        const arrayBuffer = await response.arrayBuffer();
        const result = await mammoth.convertToHtml({ arrayBuffer });
        setDocumentContent(result.value);
        setLoading(false);
      } else if (fileType === 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' ||
                 fileType === 'application/vnd.ms-excel') {
        const response = await fetch(fileUrl);
        const arrayBuffer = await response.arrayBuffer();
        const workbook = XLSX.read(arrayBuffer, { type: 'array' });
        const sheetName = workbook.SheetNames[0];
        const worksheet = workbook.Sheets[sheetName];
        const htmlString = XLSX.utils.sheet_to_html(worksheet);
        setDocumentContent(htmlString);
        setLoading(false);
      } else if (fileType.startsWith('text/') || fileType === 'application/json') {
        const response = await fetch(fileUrl);
        const text = await response.text();
        setDocumentContent(`<pre>${text}</pre>`);
        setLoading(false);
      } else if (fileType.startsWith('image/')) {
        setDocumentContent(`<img src="${fileUrl}" alt="${fileName}" style="max-width: 100%; height: auto;" />`);
        setLoading(false);
      } else {
        setError('Unsupported file type for preview');
        setLoading(false);
      }
    } catch (err) {
      setError('Failed to load document');
      setLoading(false);
    }
  };

  const onDocumentLoadSuccess = ({ numPages }: { numPages: number }) => {
    setNumPages(numPages);
    setLoading(false);
  };

  const onDocumentLoadError = (_error: Error) => {
    setError('Failed to load PDF document');
    setLoading(false);
  };

  const goToPrevPage = () => {
    setPageNumber(prev => Math.max(prev - 1, 1));
  };

  const goToNextPage = () => {
    setPageNumber(prev => Math.min(prev + 1, numPages));
  };

  const handleDownload = () => {
    if (onDownload) {
      onDownload();
    } else {
      const link = document.createElement('a');
      link.href = fileUrl;
      link.download = fileName;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        <span className="ml-2">Loading document...</span>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex flex-col items-center justify-center h-64 text-red-600">
        <div className="text-lg font-semibold mb-2">Error</div>
        <div className="text-sm mb-4">{error}</div>
        <button
          onClick={handleDownload}
          className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
        >
          Download File
        </button>
      </div>
    );
  }

  return (
    <div className="document-viewer bg-white rounded-lg shadow-lg">
      {/* Header with controls */}
      <div className="flex items-center justify-between p-4 border-b">
        <div className="flex items-center space-x-2">
          <h3 className="text-lg font-semibold truncate">{fileName}</h3>
          <span className="text-sm text-gray-500">({fileType})</span>
        </div>
        <div className="flex items-center space-x-2">
          {fileType === 'application/pdf' && numPages > 0 && (
            <div className="flex items-center space-x-2">
              <button
                onClick={goToPrevPage}
                disabled={pageNumber <= 1}
                className="px-3 py-1 bg-gray-200 text-gray-700 rounded disabled:opacity-50"
              >
                Previous
              </button>
              <span className="text-sm">
                Page {pageNumber} of {numPages}
              </span>
              <button
                onClick={goToNextPage}
                disabled={pageNumber >= numPages}
                className="px-3 py-1 bg-gray-200 text-gray-700 rounded disabled:opacity-50"
              >
                Next
              </button>
            </div>
          )}
          <button
            onClick={handleDownload}
            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            Download
          </button>
        </div>
      </div>

      {/* Document content */}
      <div className="p-4 max-h-96 overflow-auto">
        {fileType === 'application/pdf' ? (
          <Document
            file={fileUrl}
            onLoadSuccess={onDocumentLoadSuccess}
            onLoadError={onDocumentLoadError}
            className="flex justify-center"
          >
            <Page pageNumber={pageNumber} />
          </Document>
        ) : (
          <div 
            className="document-content"
            dangerouslySetInnerHTML={{ __html: documentContent }}
          />
        )}
      </div>
    </div>
  );
};

export default DocumentViewer;
