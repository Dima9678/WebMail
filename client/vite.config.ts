import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

export default defineConfig({
    plugins: [plugin()],
    server: {
        host: true,
        port: 49981,
        allowedHosts: true
    }
});