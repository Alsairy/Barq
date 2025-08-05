import express from 'express';
import { createProxyMiddleware } from 'http-proxy-middleware';
import path from 'path';
import { fileURLToPath } from 'url';

const app = express();
const PORT = process.env.PORT || 3000;

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

app.use(express.static(path.join(__dirname, 'dist')));

app.use('/api/proxy', createProxyMiddleware({
  target: 'https://technology-stack-app-tunnel-phn9uram.devinapps.com',
  changeOrigin: true,
  pathRewrite: {
    '^/api/proxy': '', // Remove /api/proxy from the path
  },
  onProxyReq: (proxyReq, req, res) => {
    proxyReq.setHeader('Origin', 'https://barq-application-plu4nmbz.devinapps.com');
  },
  onProxyRes: (proxyRes, req, res) => {
    proxyRes.headers['Access-Control-Allow-Origin'] = 'https://barq-application-plu4nmbz.devinapps.com';
    proxyRes.headers['Access-Control-Allow-Credentials'] = 'true';
    proxyRes.headers['Access-Control-Allow-Methods'] = 'GET, POST, PUT, DELETE, PATCH, OPTIONS';
    proxyRes.headers['Access-Control-Allow-Headers'] = 'Content-Type, Authorization, X-Requested-With, X-Tenant-ID, X-Correlation-ID';
  }
}));

app.get('*', (req, res) => {
  res.sendFile(path.join(__dirname, 'dist', 'index.html'));
});

app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});
