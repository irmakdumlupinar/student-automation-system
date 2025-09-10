<template>
  <nav style="display:flex; gap:12px; padding:12px; border-bottom:1px solid #eee;">
    <router-link to="/">Dashboard</router-link>
    <router-link to="/courses">Courses</router-link>
    <router-link v-if="isAdminOrTeacher" to="/students">Students</router-link>

    <div style="margin-left:auto;">
      <span v-if="auth.isAuthenticated" style="margin-right:8px">{{ auth.email }}</span>
      <button v-if="auth.isAuthenticated" @click="onLogout">Logout</button>
      <router-link v-else to="/login">Login</router-link>
    </div>
  </nav>
</template>

<script setup>
import { computed } from 'vue';
import { useAuthStore } from '../store/auth';

const auth = useAuthStore();
const isAdminOrTeacher = computed(() => auth.roles.includes('Admin') || auth.roles.includes('Teacher'));

function onLogout() {
  auth.logout();
  window.location.href = '/login';
}
</script>
