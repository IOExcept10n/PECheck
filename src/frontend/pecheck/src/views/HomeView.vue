<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import QRCode from 'qrcode.vue'

const auth = useAuthStore()
const router = useRouter()
const isOffline = ref(false)

// Check if user is authenticated, if not redirect to login
onMounted(() => {
  if (!auth.isAuthenticated && !auth.checkSavedAuth()) {
    router.push('/login')
  }
  
  // Check online status
  updateOnlineStatus()
  window.addEventListener('online', updateOnlineStatus)
  window.addEventListener('offline', updateOnlineStatus)
})

function updateOnlineStatus() {
  isOffline.value = !navigator.onLine
  auth.setOfflineStatus(isOffline.value)
}
</script>

<template>
  <v-container>
    <v-alert v-if="isOffline" type="warning" class="mb-4">
      You are currently offline. Some features may not be available. Your QR code is still accessible.
    </v-alert>
    
    <v-row v-if="auth.isAuthenticated">
      <v-col cols="12">
        <v-card class="mb-4">
          <v-card-title class="text-h5">
            Welcome, {{ auth.user?.name }}!
          </v-card-title>
          <v-card-text>
            <p v-if="auth.user?.role === 'student'">
              Your student ID: {{ auth.user?.id }}
            </p>
            <p v-else-if="auth.user?.role === 'teacher'">
              Your teacher ID: {{ auth.user?.id }}
            </p>
            <p v-else>
              Your admin ID: {{ auth.user?.id }}
            </p>
          </v-card-text>
        </v-card>
      </v-col>
      
      <v-col cols="12" md="6" v-if="auth.user?.role === 'student'">
        <v-card>
          <v-card-title>Your QR Code</v-card-title>
          <v-card-text class="d-flex justify-center">
            <QRCode :value="auth.user?.id || ''" :size="200" level="H" />
          </v-card-text>
          <v-card-actions>
            <v-btn color="primary" block>Download QR Code</v-btn>
          </v-card-actions>
          <v-card-text class="text-center">
            <p>Show this QR code to your teacher to mark attendance</p>
          </v-card-text>
        </v-card>
      </v-col>
      
      <v-col cols="12" md="6">
        <v-card>
          <v-card-title>Quick Links</v-card-title>
          <v-card-text>
            <v-list>
              <v-list-item to="/calendar">
                <template v-slot:prepend>
                  <v-icon>mdi-calendar</v-icon>
                </template>
                <v-list-item-title>Calendar</v-list-item-title>
              </v-list-item>
              
              <v-list-item to="/profile">
                <template v-slot:prepend>
                  <v-icon>mdi-account</v-icon>
                </template>
                <v-list-item-title>Profile</v-list-item-title>
              </v-list-item>
              
              <v-list-item to="/news">
                <template v-slot:prepend>
                  <v-icon>mdi-newspaper</v-icon>
                </template>
                <v-list-item-title>News</v-list-item-title>
              </v-list-item>
              
              <v-list-item v-if="auth.user?.role === 'admin'" to="/students">
                <template v-slot:prepend>
                  <v-icon>mdi-account-group</v-icon>
                </template>
                <v-list-item-title>Manage Students</v-list-item-title>
              </v-list-item>
              
              <v-list-item v-if="auth.user?.role === 'admin'" to="/teachers">
                <template v-slot:prepend>
                  <v-icon>mdi-teach</v-icon>
                </template>
                <v-list-item-title>Manage Teachers</v-list-item-title>
              </v-list-item>
              
              <v-list-item v-if="auth.user?.role === 'admin' || auth.user?.role === 'teacher'" to="/sections">
                <template v-slot:prepend>
                  <v-icon>mdi-clipboard-list</v-icon>
                </template>
                <v-list-item-title>Manage Sections</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>