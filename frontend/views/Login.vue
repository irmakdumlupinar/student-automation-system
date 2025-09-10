<template>
  <h2>Login</h2>
  <form @submit.prevent="submit" style="display:grid; gap:8px; max-width:360px;">
    <input v-model="email" type="email" placeholder="email" required />
    <input v-model="password" type="password" placeholder="password" required />
    <button :disabled="loading">{{ loading ? 'Loading...' : 'Login' }}</button>
  </form>

  <details style="margin-top:12px;">
    <summary>Seed hesapları</summary>
    <ul>
      <li>Admin: admin@demo.com / Admin123*</li>
      <li>Teacher: teacher@demo.com / Teacher123*</li>
      <li>Student: student@demo.com / Student123*</li>
    </ul>
  </details>

  <p v-if="error" style="color:red">{{ error }}</p>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../store/auth';

const router = useRouter();
const auth = useAuthStore();

const email = ref('');
const password = ref('');
const loading = ref(false);
const error = ref('');

async function submit() {
  error.value = '';
  loading.value = true;
  try {
    await auth.login(email.value, password.value);
    router.push({ name: 'dashboard' });
  } catch (e) {
    error.value = 'Giriş başarısız.';
  } finally {
    loading.value = false;
  }
}
</script>
