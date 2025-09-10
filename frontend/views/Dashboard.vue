<template>
  <h2>Dashboard</h2>
  <p>Hoş geldin! Rol(lerin): {{ roles.join(', ') || '-' }}</p>

  <div v-if="isStudent" style="margin-top:16px;">
    <h3>Notlarım</h3>
    <button @click="loadGrades">Yenile</button>
    <ul>
      <li v-for="g in grades" :key="g.id">
        {{ g.course }} — {{ g.value }} ({{ new Date(g.createdAt).toLocaleString() }})
      </li>
    </ul>

    <h3>Devamsızlık</h3>
    <button @click="loadAttendance">Yenile</button>
    <ul>
      <li v-for="a in attendance" :key="a.id">
        {{ a.course }} — {{ a.date }} — {{ a.isPresent ? 'Var' : 'Yok' }}
      </li>
    </ul>
  </div>
</template>

<script setup>
import { computed, ref } from 'vue';
import { useAuthStore } from '../store/auth';
import api from '../api';

const auth = useAuthStore();
const roles = computed(() => auth.roles);
const isStudent = computed(() => auth.roles.includes('Student'));

const grades = ref([]);
const attendance = ref([]);

async function loadGrades() {
  const { data } = await api.get('/api/grades/me');
  grades.value = data;
}
async function loadAttendance() {
  const { data } = await api.get('/api/attendance/me');
  attendance.value = data;
}
</script>
