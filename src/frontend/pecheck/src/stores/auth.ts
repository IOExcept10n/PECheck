import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<{
    id: string;
    name: string;
    role: 'student' | 'teacher' | 'admin' | null;
  } | null>(null)
  
  const isAuthenticated = ref(false)
  const isOffline = ref(false)

  function login(username: string, password: string, rememberMe: boolean) {
    // Mocking authentication process
    if (username === 'student' && password === 'password') {
      user.value = {
        id: 'S12345',
        name: 'John Student',
        role: 'student'
      }
      isAuthenticated.value = true
      
      if (rememberMe) {
        localStorage.setItem('user', JSON.stringify(user.value))
      }
      
      return true
    } else if (username === 'teacher' && password === 'password') {
      user.value = {
        id: 'T12345',
        name: 'Jane Teacher',
        role: 'teacher'
      }
      isAuthenticated.value = true
      
      if (rememberMe) {
        localStorage.setItem('user', JSON.stringify(user.value))
      }
      
      return true
    } else if (username === 'admin' && password === 'password') {
      user.value = {
        id: 'A12345',
        name: 'Admin User',
        role: 'admin'
      }
      isAuthenticated.value = true
      
      if (rememberMe) {
        localStorage.setItem('user', JSON.stringify(user.value))
      }
      
      return true
    }
    
    return false
  }

  function logout() {
    user.value = null
    isAuthenticated.value = false
    localStorage.removeItem('user')
    
    // Redirect to login page
    window.location.href = '/login'
  }
  
  function setOfflineStatus(status: boolean) {
    isOffline.value = status
  }
  
  function checkSavedAuth() {
    const savedUser = localStorage.getItem('user')
    if (savedUser) {
      user.value = JSON.parse(savedUser)
      isAuthenticated.value = true
      return true
    }
    return false
  }

  return { 
    user, 
    isAuthenticated, 
    isOffline,
    login, 
    logout,
    setOfflineStatus,
    checkSavedAuth
  }
})