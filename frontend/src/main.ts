import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { addStoryblok } from './storyblok'

const app = createApp(App)

app.use(router)
await addStoryblok(app);

app.mount('#app')
