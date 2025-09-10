import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from './store/auth';

import Login from './views/Login.vue';
import Dashboard from './views/Dashboard.vue';
import Students from './views/Students.vue';
import Courses from './views/Courses.vue';

const routes = [
  { path: '/login', name: 'login', component: Login, meta: { public: true } },
  { path: '/', name: 'dashboard', component: Dashboard, meta: { requiresAuth: true } },
  { path: '/students', name: 'students', component: Students, meta: { requiresAuth: true, roles: ['Admin','Teacher'] } },
  { path: '/courses', name: 'courses', component: Courses, meta: { requiresAuth: true } },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Basit guard
router.beforeEach((to) => {
  const auth = useAuthStore();
  if (to.meta.public) return true;

  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    return { name: 'login' };
  }
  if (to.meta.roles && to.meta.roles.length > 0) {
    const ok = to.meta.roles.some(r => auth.roles.includes(r));
    if (!ok) return { name: 'dashboard' };
  }
  return true;
});

export default router;
