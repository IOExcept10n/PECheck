<script setup lang="ts">
import { onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { useStudentsStore } from '../stores/students'
import { useTeachersStore } from '../stores/teachers'
import QRCode from 'qrcode.vue'

const auth = useAuthStore()
const studentsStore = useStudentsStore()
const teachersStore = useTeachersStore()
const router = useRouter()

// Check if user is authenticated, if not redirect to login
onMounted(() => {
  if (!auth.isAuthenticated && !auth.checkSavedAuth()) {
    router.push('/login')
  }
})

// Get student or teacher data based on the user role
const userData = computed(() => {
  if (auth.user?.role === 'student') {
    return studentsStore.getStudentById(auth.user.id)
  } else if (auth.user?.role === 'teacher') {
    return teachersStore.getTeacherById(auth.user.id)
  }
  return null
})
</script>

<template>
  <v-container v-if="auth.isAuthenticated">
    <v-row>
      <v-col cols="12" md="4">
        <v-card>
          <v-card-title>Profile</v-card-title>
          <v-card-text class="text-center">
            <v-avatar size="150" color="primary">
              <v-icon size="100" color="white">mdi-account</v-icon>
            </v-avatar>
            
            <h2 class="mt-4">{{ auth.user?.name }}</h2>
            <p class="text-subtitle-1">{{ auth.user?.role?.charAt(0).toUpperCase() + auth.user?.role?.slice(1) }}</p>
            <p>ID: {{ auth.user?.id }}</p>
          </v-card-text>
        </v-card>
      </v-col>
      
      <v-col cols="12" md="4">
        <v-card>
          <v-card-title>Your QR Code</v-card-title>
          <v-card-text class="d-flex justify-center">
            <QRCode :value="auth.user?.id || ''" :size="200" level="H" />
          </v-card-text>
          <v-card-text class="text-center">
            <p>Your QR code contains your ID: {{ auth.user?.id }}</p>
            <p v-if="auth.user?.role === 'student'">Show this QR code to your teacher to mark attendance</p>
          </v-card-text>
        </v-card>
      </v-col>
      
      <v-col cols="12" md="4">
        <v-card v-if="auth.user?.role === 'student' && userData">
          <v-card-title>Student Information</v-card-title>
          <v-card-text>
            <v-list>
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon>mdi-account-group</v-icon>
                </template>
                <v-list-item-title>Group: {{ userData.group }}</v-list-item-title>
              </v-list-item>
              
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon>mdi-clipboard-list</v-icon>
                </template>
                <v-list-item-title>Section: {{ userData.section }}</v-list-item-title>
              </v-list-item>
            </v-list>
            
            <v-divider class="my-4"></v-divider>
            
            <h3 class="text-h6 mb-2">Normative Results</h3>
            <v-list>
              <v-list-item v-for="(value, key) in userData.normativeResults" :key="key">
                <v-list-item-title>{{ key }}: {{ value }}</v-list-item-title>
              </v-list-item>
            </v-list>
            
            <v-divider class="my-4"></v-divider>
            
            <h3 class="text-h6 mb-2">Recent Attendance</h3>
            <v-list>
              <v-list-item v-for="(attendance, index) in userData.attendances.slice(-3)" :key="index">
                <v-list-item-title>
                  {{ new Date(attendance.date).toLocaleDateString() }}:
                  <v-chip
                    :color="attendance.present ? 'success' : 'error'"
                    size="small"
                    class="ml-2"
                  >
                    {{ attendance.present ? 'Present' : 'Absent' }}
                  </v-chip>
                </v-list-item-title>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
        
        <v-card v-else-if="auth.user?.role === 'teacher' && userData">
          <v-card-title>Teacher Information</v-card-title>
          <v-card-text>
            <v-list>
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon>mdi-email</v-icon>
                </template>
                <v-list-item-title>Email: {{ userData.email }}</v-list-item-title>
              </v-list-item>
              
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon>mdi-phone</v-icon>
                </template>
                <v-list-item-title>Phone: {{ userData.phone }}</v-list-item-title>
              </v-list-item>
            </v-list>
            
            <v-divider class="my-4"></v-divider>
            
            <h3 class="text-h6 mb-2">Sections</h3>
            <v-chip-group>
              <v-chip
                v-for="section in userData.sections"
                :key="section"
                color="primary"
                class="ma-1"
              >
                {{ section }}
              </v-chip>
            </v-chip-group>
          </v-card-text>
        </v-card>
        
        <v-card v-else-if="auth.user?.role === 'admin'">
          <v-card-title>Admin Dashboard</v-card-title>
          <v-card-text>
            <v-list>
              <v-list-item to="/students">
                <template v-slot:prepend>
                  <v-icon>mdi-account-group</v-icon>
                </template>
                <v-list-item-title>Manage Students</v-list-item-title>
              </v-list-item>
              
              <v-list-item to="/teachers">
                <template v-slot:prepend>
                  <v-icon>mdi-teach</v-icon>
                </template>
                <v-list-item-title>Manage Teachers</v-list-item-title>
              </v-list-item>
              
              <v-list-item to="/sections">
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
    
    <v-row v-if="auth.user?.role === 'student'">
      <v-col cols="12">
        <v-btn color="primary" block @click="router.push('/calendar')">
          View Your Calendar
          <v-icon right>mdi-calendar</v-icon>
        </v-btn>
      </v-col>
    </v-row>
  </v-container>
</template>