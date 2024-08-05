/** @type {import('tailwindcss').Config} */
import forms from '@tailwindcss/forms'
import tailwindcssAnimated from 'tailwindcss-animated'
import plugin from 'tailwindcss/plugin'
export default {
  content: [
    // Example content paths...
    './public/**/*.html',
    './src/**/*.{js,jsx,ts,tsx,vue}',
  ],
  theme: {
    extend: {
      colors: {
        "primary": {
          DEFAULT: "#d99021",
          100: '#f7e9d3',
          200: '#f0d3a6',
          300: '#e8bc7a',
          400: '#e1a64d',
          500: '#d99021',
          600: '#825614',
          700: '#573a0d',
          800: '#2b1d07'
        },
        "secondary": "#292F4E"
      },
      textShadow: {
        sm: '0 1px 2px var(--tw-shadow-color)',
        DEFAULT: '0 2px 4px var(--tw-shadow-color)',
        lg: '0 8px 16px var(--tw-shadow-color)',
      },
    },
  },
  plugins: [
    plugin(function ({ matchUtilities, theme }) {
      matchUtilities(
        {
          'text-shadow': (value) => ({
            textShadow: value,
          }),
        },
        { values: theme('textShadow') }
      )
    }),
    tailwindcssAnimated,
    forms
  ],
}

