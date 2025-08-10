<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { useTheme } from 'vuetify'

const auth = useAuthStore()
const router = useRouter()
const theme = useTheme()

const username = ref('')
const password = ref('')
const rememberMe = ref(true)
const error = ref('')
const loading = ref(false)

// Simple regex validation
const usernameRegex = /^[a-zA-Z0-9_]{3,20}$/
const passwordRegex = /^.{6,}$/

// If already logged in, redirect to home
onMounted(() => {
  if (auth.isAuthenticated || auth.checkSavedAuth()) {
    router.push('/')
  }
  
  // Set theme from localStorage or default to light
  const savedTheme = localStorage.getItem('theme') || 'light'
  theme.global.name.value = savedTheme
})

// Toggle theme
function toggleTheme() {
  theme.global.name.value = theme.global.current.value.dark ? 'light' : 'dark'
  localStorage.setItem('theme', theme.global.name.value)
}

function isFormValid() {
  if (!usernameRegex.test(username.value)) {
    error.value = 'Username must be 3-20 characters and contain only letters, numbers, and underscores'
    return false
  }
  
  if (!passwordRegex.test(password.value)) {
    error.value = 'Password must be at least 6 characters'
    return false
  }
  
  return true
}

function handleLogin() {
  error.value = ''
  
  if (!isFormValid()) {
    return
  }
  
  loading.value = true
  
  // Simulate network delay
  setTimeout(() => {
    const success = auth.login(username.value, password.value, rememberMe.value)
    
    if (success) {
      router.push('/')
    } else {
      error.value = 'Invalid username or password'
    }
    
    loading.value = false
  }, 1000)
}
</script>

<template>
  <v-app>
    <v-app-bar color="primary" dark>
      <v-toolbar-title>PE Check</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn icon @click="toggleTheme">
        <v-icon>{{ theme.global.current.value.dark ? 'mdi-white-balance-sunny' : 'mdi-moon-waxing-crescent' }}</v-icon>
      </v-btn>
    </v-app-bar>
    
    <v-main>
      <v-container class="fill-height" fluid>
        <v-row align="center" justify="center">
          <v-col cols="12" sm="8" md="6" lg="4">
            <v-card class="elevation-12">
              <v-card-title class="text-center text-h4 pt-6">
                Login to PE Check
              </v-card-title>
              
              <v-card-text>
                <v-alert v-if="error" type="error" dense class="mb-4">{{ error }}</v-alert>
                <v-form @submit.prevent="handleLogin">
                  <v-text-field
                    v-model="username"
                    label="Username"
                    name="username"
                    prepend-icon="mdi-account"
                    type="text"
                    :rules="[v => !!v || 'Username is required', v => usernameRegex.test(v) || 'Invalid username format']"
                    variant="outlined"
                    class="mb-4"
                  ></v-text-field>

                  <v-text-field
                    v-model="password"
                    label="Password"
                    name="password"
                    prepend-icon="mdi-lock"
                    type="password"
                    :rules="[v => !!v || 'Password is required', v => passwordRegex.test(v) || 'Password must be at least 6 characters']"
                    variant="outlined"
                    class="mb-4"
                  ></v-text-field>

                  <v-checkbox
                    v-model="rememberMe"
                    label="Remember me"
                    required
                    class="mb-4"
                  ></v-checkbox>
                </v-form>
                
                <v-btn 
                  color="primary" 
                  size="large" 
                  block 
                  @click="handleLogin"
                  :loading="loading"
                  class="mb-4"
                >
                  Login
                </v-btn>
                
                <v-divider class="my-4"></v-divider>
                
                <div class="text-subtitle-1 mb-2">
                  <p>Note: Registration is handled by moderators only.</p>
                </div>
                
                <v-expansion-panels variant="accordion">
                  <v-expansion-panel>
                    <v-expansion-panel-title>Demo Credentials</v-expansion-panel-title>
                    <v-expansion-panel-text>
                      <v-list density="compact">
                        <v-list-item>
                          <v-list-item-title>
                            <strong>Student:</strong> username: "student", password: "password"
                          </v-list-item-title>
                        </v-list-item>
                        <v-list-item>
                          <v-list-item-title>
                            <strong>Teacher:</strong> username: "teacher", password: "password"
                          </v-list-item-title>
                        </v-list-item>
                        <v-list-item>
                          <v-list-item-title>
                            <strong>Admin:</strong> username: "admin", password: "password"
                          </v-list-item-title>
                        </v-list-item>
                      </v-list>
                    </v-expansion-panel-text>
                  </v-expansion-panel>
                </v-expansion-panels>
              </v-card-text>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
    
    <v-footer app>
      <div>&copy; {{ new Date().getFullYear() }} PE Check - Student Management System</div>
    </v-footer>
  </v-app>
</template>

<style scoped>
.v-card {
  border-radius: 12px;
}

.v-main {
  background: linear-gradient(135deg, rgba(25, 118, 210, 0.1) 0%, rgba(25, 118, 210, 0.2) 100%);
}
</style>