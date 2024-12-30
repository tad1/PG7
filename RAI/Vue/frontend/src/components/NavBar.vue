<script setup lang="ts">
import { useAuthStore } from '@/stores/authStore';
import { inject } from 'vue';

const authInfo = inject('authInfo');
const store = useAuthStore();
const isLogged = authInfo.isLogged;
const logout = authInfo.logout;

</script>

<template>
    <nav>
        <div class='border-slate-600 border-l-2 inline-block'>
            <router-link id="home-nav" to="/">Home</router-link>
            <router-link id="about-nav" to="/about">About</router-link>
        </div>
        <div class='border-slate-600 border-l-2 inline-block ml-3'>
            <router-link id="login-nav" v-if="!isLogged" to="/login">Login</router-link>
            <router-link id="register-nav" v-if="!isLogged" to="/register">Register</router-link>
            <router-link id="saved-nav" v-if="isLogged" to="/saved">{{ store.username }}</router-link>
            <button v-if="isLogged" id="logout-btn" @click="logout">Logout</button>
        </div>
    </nav>
</template>

