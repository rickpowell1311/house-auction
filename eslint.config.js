import eslint from '@eslint/js';
import pluginVue from 'eslint-plugin-vue';
import tseslint from 'typescript-eslint';

export default tseslint.config(
  eslint.configs.recommended,
  ...tseslint.configs.recommended,
  ...pluginVue.configs['flat/recommended'],
  {
    plugins: {
      'typescript-eslint': tseslint.plugin,
    },
    languageOptions: {
      parserOptions: {
        parser: tseslint.parser,
        extraFileExtensions: ['.vue'],
        sourceType: 'module',
      },
    },
  },
  {
    rules: {
      /* Defaults are pretty insanely strict. Don't care enough about these  */
      'vue/multi-word-component-names': 'off',
      'vue/multiline-html-element-content-newline': 'off',
      'vue/singleline-html-element-content-newline': 'off',
      'vue/max-attributes-per-line': 'off',
      '@typescript-eslint/no-unused-vars': 'off',
      'vue/first-attribute-linebreak': 'off',
      'vue/html-quotes': 'off',
      'vue/html-closing-bracket-newline': 'off',
      'vue/html-indent': 'off',
      'vue/require-v-for-key': 'off',
      'no-unexpected-multiline': 'off',

      /* Protect against unused code */
      'vue/no-unused-vars': 'error'
    }
  }
)