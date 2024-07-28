import { fileURLToPath, URL } from 'node:url'

import vue from '@vitejs/plugin-vue'
import VueRouter from 'unplugin-vue-router/vite'
import { defineConfig } from 'vite'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    VueRouter({
      routesFolder: [
        {
          src: 'src',
          path: '',
          // override globals
          exclude: (excluded) => excluded,
          filePatterns: (filePatterns) => filePatterns,
          extensions: (extensions) => extensions,
        },
      ],

      // what files should be considered as a pages
      extensions: ['.vue'],

      // what files to include
      filePatterns: ['**/*'],

      // files to exclude from the scan
      exclude: ['**/_shared', '**/App.vue'],
    }),
    vue(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  }
})
