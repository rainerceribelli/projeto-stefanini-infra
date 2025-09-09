const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function(app) {
  app.use(
    '/api',
    createProxyMiddleware({
      target: 'https://localhost:7205',
      changeOrigin: true,
      secure: false, // Para aceitar certificados auto-assinados
    })
  );
};