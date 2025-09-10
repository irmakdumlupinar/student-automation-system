<template>
  <h2>Students</h2>
  <button @click="load">Yenile</button>
  <ul>
    <li v-for="s in list" :key="s.id">
      {{ s.number }} — {{ s.name }} {{ s.surname }}
    </li>
  </ul>
</template>

<script setup>
import { ref } from 'vue';
import api from '../api';

const list = ref([]);

async function load() {
  const { data } = await api.get('/api/students');
  list.value = data;
}
</script>
