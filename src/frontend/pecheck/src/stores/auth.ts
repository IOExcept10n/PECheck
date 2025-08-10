import { defineStore } from 'pinia'
<<<<<<< Updated upstream
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
=======
import { ref, computed } from 'vue'
import { authApi } from '@/api/auth'

export interface User {
  id: string
  email: string
  fullName: string
  role: 'student' | 'teacher' | 'moderator'
  group?: string
  course?: number
  profilePicture?: string
}

export interface LoginCredentials {
  email: string
  password: string
}

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const token = ref<string | null>(localStorage.getItem('token'))
  const isLoading = ref(false)

  const isAuthenticated = computed(() => !!token.value && !!user.value)

  const login = async (credentials: LoginCredentials) => {
    try {
      isLoading.value = true
      const response = await authApi.login(credentials)
      
      token.value = response.token
      user.value = response.user
      
      localStorage.setItem('token', response.token)
      
      return response
    } catch (error) {
      throw error
    } finally {
      isLoading.value = false
    }
  }

  const logout = () => {
    user.value = null
    token.value = null
    localStorage.removeItem('token')
  }

  const loadUser = async () => {
    if (!token.value) return

    try {
      isLoading.value = true
      const userData = await authApi.getCurrentUser()
      user.value = userData
    } catch (error) {
      logout()
      throw error
    } finally {
      isLoading.value = false
    }
  }

  const updateProfile = async (profileData: Partial<User>) => {
    try {
      isLoading.value = true
      const updatedUser = await authApi.updateProfile(profileData)
      user.value = { ...user.value, ...updatedUser }
      return updatedUser
    } catch (error) {
      throw error
    } finally {
      isLoading.value = false
    }
  }

  return {
    user,
    token,
    isLoading,
    isAuthenticated,
    login,
    logout,
    loadUser,
    updateProfile
  }
}) 
>>>>>>> Stashed changes
