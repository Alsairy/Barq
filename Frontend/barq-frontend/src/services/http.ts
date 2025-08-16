import axios from 'axios';

function getCookie(name: string): string | null {
  if (typeof document === 'undefined') return null;
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop()!.split(';').shift() || null;
  return null;
}

axios.defaults.withCredentials = true;

axios.interceptors.request.use((config) => {
  const method = (config.method || 'get').toLowerCase();
  const unsafe = ['post', 'put', 'patch', 'delete'].includes(method);
  if (unsafe) {
    const xsrf = getCookie('XSRF-TOKEN');
    if (!config.headers) config.headers = {};
    if (xsrf) (config.headers as any)['X-XSRF-TOKEN'] = xsrf;
  }
  return config;
});

if (typeof window !== 'undefined' && typeof window.fetch === 'function') {
  const originalFetch = window.fetch.bind(window);
  window.fetch = (input: RequestInfo | URL, init?: RequestInit): Promise<Response> => {
    const newInit: RequestInit = { ...init };
    if (!newInit.credentials) newInit.credentials = 'include';
    const method = (newInit.method || 'GET').toUpperCase();
    const unsafe = ['POST', 'PUT', 'PATCH', 'DELETE'].includes(method);
    if (unsafe) {
      const xsrf = getCookie('XSRF-TOKEN');
      newInit.headers = newInit.headers instanceof Headers ? newInit.headers : new Headers(newInit.headers || {});
      if (xsrf) (newInit.headers as Headers).set('X-XSRF-TOKEN', xsrf);
      if (!(newInit.headers as Headers).has('Content-Type')) {
        (newInit.headers as Headers).set('Content-Type', 'application/json');
      }
    }
    return originalFetch(input, newInit);
  };
}
