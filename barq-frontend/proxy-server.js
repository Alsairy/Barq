import corsAnywhere from 'cors-anywhere';

const host = process.env.HOST || '0.0.0.0';
const port = process.env.PORT || 8080;

corsAnywhere.createServer({
  originWhitelist: ['https://barq-application-plu4nmbz.devinapps.com', 'http://localhost:5173'],
  requireHeader: ['origin', 'x-requested-with'],
  removeHeaders: [
    'cookie',
    'cookie2',
    'x-heroku-queue-wait-time',
    'x-heroku-queue-depth',
    'x-heroku-dynos-in-use',
    'x-request-start',
  ],
  redirectSameOrigin: true,
  httpProxyOptions: {
    secure: false
  },
}).listen(port, host, function() {
  console.log('CORS Anywhere proxy server running on ' + host + ':' + port);
});
