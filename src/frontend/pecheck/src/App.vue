<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

onMounted(() => {
  // Check if user is authenticated on app load
  const token = localStorage.getItem('token')
  
  if (!token && router.currentRoute.value.meta.requiresAuth) {
    router.push('/login')
  }
})
</script>

<template>
  <div id="app">
    <RouterView v-slot="{ Component }">
      <transition name="fade" mode="out-in">
        <component :is="Component" />
      </transition>
    </RouterView>
  </div>
</template>

<style>
@import '@/assets/main.css';
</style>