import { defineStore } from 'pinia';
import api from '../api';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || null,
    roles: JSON.parse(localStorage.getItem('roles') || '[]'),
    email: localStorage.getItem('email') || null,
  }),
  getters: {
    isAuthenticated: (s) => !!s.token,
    isAdmin: (s) => s.roles.includes('Admin'),
    isTeacher: (s) => s.roles.includes('Teacher'),
    isStudent: (s) => s.roles.includes('Student'),
  },
  actions: {
    async login(email, password) {
      const { data } = await api.post('/api/auth/login', { email, password });
      this.token = data.accessToken;
      // Token’dan rolleri decode etmeye uğraşmayalım: backend seed hesaplarına göre hızla “rolleri getir” yapacağız.
      // Küçük bir endpoint yoksa, pratik çözüm: rol bilgisini local’de saklamak için
      // login ekranında hangi kullanıcıyla giriş yapıldıysa ona göre set edebiliriz:
      // Ama daha temiz olanı token’ı decode etmek:
      const payload = JSON.parse(atob(data.accessToken.split('.')[1]));
      const roles = (payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || []);
      this.roles = Array.isArray(roles) ? roles : [roles];
      this.email = email;

      localStorage.setItem('token', this.token);
      localStorage.setItem('roles', JSON.stringify(this.roles));
      localStorage.setItem('email', this.email);
    },
    logout() {
      this.token = null;
      this.roles = [];
      this.email = null;
      localStorage.removeItem('token');
      localStorage.removeItem('roles');
      localStorage.removeItem('email');
    },
  },
});
