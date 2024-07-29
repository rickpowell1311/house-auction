import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { addStoryblok } from './storyblok'

const app = createApp(App)

app.use(router)

// FYI no await here: Can't enable top level awaits in Vite (breaks prod build)
addStoryblok(app);

app.mount('#app')
