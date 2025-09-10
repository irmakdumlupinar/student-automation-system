<template>
  <NavBar />
  <main class="container" style="max-width: 960px; margin: 1.5rem auto;">
    <router-view />
  </main>
</template>

<script setup>
import NavBar from './components/NavBar.vue'
</script>
