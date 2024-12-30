import { describe, it, expect, beforeEach, vi } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import { useAuthStore } from '../authStore'

describe('authStore', () => {
    const exampleToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6IlRlc3RVc2VyIn0.zgmcdWQ5c_CHFDL_Akyjrks0j1ztBz855qgk33cf4hw'
    beforeEach(() => {
        setActivePinia(createPinia())
        vi.spyOn(Storage.prototype, 'getItem').mockReturnValue(exampleToken)
    })

    it('initializes token from localStorage', () => {
        const store = useAuthStore()
        
        expect(store.token).toBe(exampleToken)
    })

    it('sets and retrieves the token', () => {
        const store = useAuthStore()
        
        store.setToken('test-token')
        
        expect(store.token).toBe('test-token')
    })

    it('returns null payload if token is empty', () => {
        const store = useAuthStore()

        store.setToken('')
        
        expect(store.payload).toBeNull()
    })

    it('returns correct username from token', () => {
        const store = useAuthStore()
        const mockToken = btoa(JSON.stringify({})) + '.' + btoa(JSON.stringify({ username: 'myuser' })) + '.' + btoa(JSON.stringify({}))
        
        store.setToken(mockToken)
        
        expect(store.username).toBe('myuser')
    })
})