<script setup lang="ts">
import { RouterView } from 'vue-router'
import NavBar from './components/NavBar.vue'
</script>

<template>
  <header>
    <div class="wrapper">
      <NavBar />
    </div>
  </header>
  <br>
  <RouterView />
</template>

<style scoped>
header {
  line-height: 1.5;
  max-height: 10vh;
  place-items: flex-start;
  display: block;
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  z-index: 1000;
  background-color: #181818;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
}

.wrapper{
  margin: 0 auto;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

nav {
  font-size: 12px;
  text-align: center;
  padding: 0.5rem 0;
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}


@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    flex-direction: column;
    flex-wrap: wrap;
  }

  nav {
    text-align: left;
    margin-left: -1rem;
    font-size: 1rem;

    padding: 1rem 0;
  }
}

</style>
