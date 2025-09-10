<template>
  <h2>Courses</h2>
  <button @click="load">Yenile</button>
  <ul>
    <li v-for="c in list" :key="c.id">
      {{ c.name }} — {{ c.status }}
    </li>
  </ul>
</template>

<script setup>
import { ref } from 'vue';
import api from '../api';

const list = ref([]);

async function load() {
  const { data } = await api.get('/api/courses');
  list.value = data;
}
</script>
