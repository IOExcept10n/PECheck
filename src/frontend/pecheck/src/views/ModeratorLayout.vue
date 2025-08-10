<template>
  <div class="layout">
    <header class="header">
      <h1>PE Check - Moderator Dashboard</h1>
      <div class="user-info">
        <span>{{ authStore.user?.fullName }}</span>
        <button @click="logout" class="btn btn-outline">Logout</button>
      </div>
    </header>
    
    <nav class="nav">
      <router-link to="/moderator" class="nav-link">Profile</router-link>
      <router-link to="/moderator/students" class="nav-link">Students</router-link>
      <router-link to="/moderator/teachers" class="nav-link">Teachers</router-link>
      <router-link to="/moderator/sections" class="nav-link">Sections</router-link>
    </nav>
    
    <main class="main">
      <router-view />
    </main>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()

const logout = () => {
  authStore.logout()
  router.push('/')
}
</script>

<style scoped>
.layout {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.header {
  background: var(--primary-color);
  color: white;
  padding: 1rem 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.nav {
  background: white;
  padding: 1rem 2rem;
  border-bottom: 1px solid var(--border-color);
  display: flex;
  gap: 2rem;
}

.nav-link {
  color: var(--text-secondary);
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: var(--border-radius-small);
  transition: var(--transition);
}

.nav-link:hover,
.nav-link.router-link-active {
  background: var(--primary-color);
  color: white;
}

.main {
  flex: 1;
  padding: 2rem;
}

@media (max-width: 768px) {
  .header {
    flex-direction: column;
    gap: 1rem;
    text-align: center;
  }
  
  .nav {
    flex-direction: column;
    gap: 0.5rem;
  }
}
</style> 