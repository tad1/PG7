import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

function parseJwt (token:string) {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}


export const useAuthStore = defineStore('authInfo', () => {
  const token = ref<string>(localStorage.getItem('token') || "");
  const payload = computed(() => {
    if (!token.value) {
      return null
    }
    return parseJwt(token.value)
  });

  const username = computed(() => {
    if (!token.value) {
      return ""
    }
    const p = parseJwt(token.value)
    return p.username
  });

  const setToken = (value: string) => {
    token.value = value
  }

  return { token, payload, username, setToken }
})
