import { describe, it, expect, vi, beforeEach } from "vitest"
import { createApp } from "vue"
import authInfo from "../authInfo"
import { useAuthStore } from "@/stores/authStore"
import { createPinia, setActivePinia } from "pinia"


const mockLocalStorage = (() => {
    let store: Record<string, string> = {}
    return {
        getItem: (key: string) => store[key] || null,
        setItem: (key: string, value: string) => { store[key] = value },
        removeItem: (key: string) => { delete store[key] },
        clear: () => { store = {} }
    }
})()

describe("authInfo plugin", () => {
    beforeEach(() => {
        vi.resetAllMocks()
        setActivePinia(createPinia())
        Object.defineProperty(window, "localStorage", {
            value: mockLocalStorage,
            writable: true
        })
        mockLocalStorage.clear()
    })

    it("installs plugin and provides $isLogged", () => {
        const app = createApp({})
        app.use(authInfo)
        expect(app.config.globalProperties.$isLogged).toBeDefined()
    })

    it("sets token and updates localStorage", () => {
        const app = createApp({})
        app.use(authInfo)
        const { setToken } = app._context.provides.authInfo
        setToken("abc123")
        expect(window.localStorage.getItem("token")).toBe("abc123")
    })

    it("removes token on logout", () => {
        const app = createApp({})
        app.use(authInfo)
        const { logout } = app._context.provides.authInfo
        window.localStorage.setItem("token", "abc123")
        logout()
        expect(window.localStorage.getItem("token")).toBeNull()
    })

    it("marks user as not logged if token expired", async () => {
        const store = useAuthStore()
        const token = btoa(JSON.stringify({ exp: 0 })) + "." + btoa(JSON.stringify({})) + "." + btoa(JSON.stringify({}));
        store.setToken(token)
        const app = createApp({})
        app.use(authInfo)
        expect(app.config.globalProperties.$isLogged.value).toBe(false)
    })
})