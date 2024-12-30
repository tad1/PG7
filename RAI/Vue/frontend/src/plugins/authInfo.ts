import { useAuthStore } from "@/stores/authStore";
import { computed, ref } from "vue";


export default {
    install: (app: any) => {
        const store = useAuthStore()
        const time = ref(new Date())
        
        const isLogged = computed(() => {
            if (!store.token) {
                return false
            }
            const payload = store.payload
            const expired = payload.exp > time.value.getTime() / 1000
            if (!expired) {
                localStorage.removeItem('token')
                store.setToken(null)
                return false
            }
            return true
        })

        const setToken = (value: string|null) => {
            if(!value) {
                localStorage.removeItem('token')
                store.setToken("")
            } else {
                localStorage.setItem('token', value)
                store.setToken(value)
            }
        }

        const logout = () => {
            setToken(null)
        }

        setInterval(() => {
            time.value = new Date()
        }, 1000)

        app.config.globalProperties.$isLogged = isLogged
        app.provide('authInfo', { isLogged, setToken, logout })
    }
}