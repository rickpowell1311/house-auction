/** @type {import('tailwindcss').Config} */
import tailwindcssAnimated from 'tailwindcss-animated'
export default {
  content: [
    // Example content paths...
    './public/**/*.html',
    './src/**/*.{js,jsx,ts,tsx,vue}',
  ],
  theme: {
    extend: {
      colors: {
        "primary": "#d99021",
        "secondary": "#292F4E"
      }
    },
  },
  plugins: [
    tailwindcssAnimated
  ],
}

