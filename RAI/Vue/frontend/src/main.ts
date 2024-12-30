import './assets/main.css'
import './index.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import axios from 'axios'
import VueAxios from 'vue-axios'

import App from './App.vue'
import router from './router'

import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import authInfo from './plugins/authInfo'


const vuetify = createVuetify({
    components,
    directives,
})

const app = createApp(App)

app.use(vuetify)

const backnedAPI = axios.create({
    baseURL: import.meta.env.VITE_BACKEND_BASE_URL,
});

backnedAPI.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
}, (error) => {
    return Promise.reject(error);
});

app.use(createPinia())
app.use(router)
app.use(authInfo)

app.use(VueAxios, backnedAPI)
app.provide('axios', app.config.globalProperties.axios)

app.mount('#app')
