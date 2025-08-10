import { createApp } from 'vue'
import { createPinia } from 'pinia'
<<<<<<< Updated upstream
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import 'vuetify/styles'
import '@mdi/font/css/materialdesignicons.css'
=======
import { Toast, type PluginOptions } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'
>>>>>>> Stashed changes

import App from './App.vue'
import router from './router'

<<<<<<< Updated upstream
// Create vuetify instance
const vuetify = createVuetify({
  components,
  directives,
  theme: {
    defaultTheme: 'light',
    themes: {
      light: {
        dark: false,
        colors: {
          primary: '#1976D2',
          secondary: '#424242',
          accent: '#82B1FF',
          error: '#FF5252',
          info: '#2196F3',
          success: '#4CAF50',
          warning: '#FFC107',
        }
      },
      dark: {
        dark: true,
        colors: {
          primary: '#2196F3',
          secondary: '#424242',
          accent: '#FF4081',
          error: '#FF5252',
          info: '#2196F3',
          success: '#4CAF50',
          warning: '#FFC107',
        }
      }
    }
  }
})
=======
import './assets/main.css'
>>>>>>> Stashed changes

const app = createApp(App)

app.use(createPinia())
app.use(router)
<<<<<<< Updated upstream
app.use(vuetify)

app.mount('#app')
=======
app.use(Toast, {
  autoClose: 3000,
  position: 'top-right'
} as PluginOptions)

app.mount('#app') 
>>>>>>> Stashed changes
