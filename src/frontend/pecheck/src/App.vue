<<<<<<< Updated upstream
<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, watch, computed } from 'vue'
import { useTheme } from 'vuetify'
import { useDisplay } from 'vuetify'
import { useAuthStore } from './stores/auth'
import { useRoute } from 'vue-router'

const drawer = ref(true)
const theme = useTheme()
const { mobile } = useDisplay()
const auth = useAuthStore()
const route = useRoute()

// Computed property to check if user is on login page
const isLoginPage = computed(() => {
  return route.path === '/login'
})

// Handle theme switching
const toggleTheme = () => {
  theme.global.name.value = theme.global.current.value.dark ? 'light' : 'dark'
  localStorage.setItem('theme', theme.global.name.value)
}

// Handle swipe gesture for mobile sidebar
let touchStartX = 0
let touchEndX = 0

const handleTouchStart = (e: TouchEvent) => {
  touchStartX = e.changedTouches[0].screenX
}

const handleTouchEnd = (e: TouchEvent) => {
  touchEndX = e.changedTouches[0].screenX
  handleSwipeGesture()
}

const handleSwipeGesture = () => {
  const swipeThreshold = 50
  if (touchEndX - touchStartX > swipeThreshold) {
    // Swipe right - open drawer
    drawer.value = true
  } else if (touchStartX - touchEndX > swipeThreshold) {
    // Swipe left - close drawer
    drawer.value = false
  }
}

onMounted(() => {
  // Set theme from localStorage or default to light
  const savedTheme = localStorage.getItem('theme') || 'light'
  theme.global.name.value = savedTheme
  
  // Set drawer state based on device
  drawer.value = !mobile.value
  
  // Add touch event listeners for swipe gesture
  document.addEventListener('touchstart', handleTouchStart)
  document.addEventListener('touchend', handleTouchEnd)
  
  // Check if user is already authenticated from localStorage
  auth.checkSavedAuth()
})

onBeforeUnmount(() => {
  document.removeEventListener('touchstart', handleTouchStart)
  document.removeEventListener('touchend', handleTouchEnd)
})

// Watch for mobile changes and update drawer state
watch(mobile, (isMobile) => {
  drawer.value = !isMobile
})
</script>

<template>
  <!-- Login page has no app layout -->
  <template v-if="isLoginPage">
    <router-view />
  </template>
  
  <!-- App layout for authenticated users -->
  <v-app v-else>
    <v-navigation-drawer
      v-model="drawer"
      :permanent="!mobile"
      :temporary="mobile"
      app
      v-if="auth.isAuthenticated"
    >
      <v-list-item
        title="PE Check"
        subtitle="Student Management System"
      ></v-list-item>

      <v-divider></v-divider>

      <v-list density="compact" nav>
        <v-list-item prepend-icon="mdi-home" title="Home" to="/"></v-list-item>
        <v-list-item prepend-icon="mdi-calendar" title="Calendar" to="/calendar"></v-list-item>
        <v-list-item prepend-icon="mdi-card-account-details" title="Profile" to="/profile"></v-list-item>
        <v-list-item prepend-icon="mdi-newspaper" title="News" to="/news"></v-list-item>
        
        <!-- Admin-only options -->
        <template v-if="auth.user?.role === 'admin'">
          <v-list-item prepend-icon="mdi-account-group" title="Students" to="/students"></v-list-item>
          <v-list-item prepend-icon="mdi-teach" title="Teachers" to="/teachers"></v-list-item>
          <v-list-item prepend-icon="mdi-clipboard-list" title="Sections" to="/sections"></v-list-item>
        </template>
        
        <!-- Teacher-only options -->
        <template v-if="auth.user?.role === 'teacher'">
          <v-list-item prepend-icon="mdi-clipboard-list" title="Sections" to="/sections"></v-list-item>
        </template>
      </v-list>
      
      <template v-slot:append>
        <v-divider></v-divider>
        <v-list>
          <v-list-item @click="auth.logout()" prepend-icon="mdi-logout" title="Logout"></v-list-item>
        </v-list>
      </template>
    </v-navigation-drawer>

    <v-app-bar app>
      <v-app-bar-nav-icon @click="drawer = !drawer" v-if="mobile && auth.isAuthenticated"></v-app-bar-nav-icon>
      <v-toolbar-title>PE Check</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn icon @click="toggleTheme">
        <v-icon>{{ theme.global.current.value.dark ? 'mdi-white-balance-sunny' : 'mdi-moon-waxing-crescent' }}</v-icon>
      </v-btn>
    </v-app-bar>

    <v-main>
      <!-- Redirect to login if not authenticated -->
      <router-view v-if="auth.isAuthenticated || isLoginPage" />
      <v-container v-else class="fill-height" fluid>
        <v-row align="center" justify="center">
          <v-col cols="12" sm="8" md="6" lg="4">
            <v-card class="elevation-12">
              <v-card-text class="text-center">
                <p>You need to log in to access this page.</p>
                <v-btn color="primary" to="/login" class="mt-4">Go to Login</v-btn>
              </v-card-text>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </v-main>

    <v-footer app v-if="auth.isAuthenticated">
      <div>&copy; {{ new Date().getFullYear() }} PE Check</div>
    </v-footer>
  </v-app>
</template>

<style>
/* Additional global styles */
.v-main {
  min-height: calc(100vh - 64px - 48px); /* Viewport height minus header and footer */
}
</style>
=======
<template>
  <div id="app">
    <router-view />
  </div>
</template>

<script setup lang="ts">
// Main app component
</script>

<style>
#app {
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  height: 100vh;
  margin: 0;
  padding: 0;
}

* {
  box-sizing: border-box;
}

body {
  margin: 0;
  padding: 0;
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
  min-height: 100vh;
}
</style> 
>>>>>>> Stashed changes
