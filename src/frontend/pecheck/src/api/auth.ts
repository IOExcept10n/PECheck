import apiClient from './client'
import type { User, LoginCredentials } from '@/stores/auth'

export interface LoginResponse {
  token: string
  user: User
}

export const authApi = {
  async login(credentials: LoginCredentials): Promise<LoginResponse> {
    const response = await apiClient.post('/auth/login', credentials)
    return response.data
  },

  async getCurrentUser(): Promise<User> {
    const response = await apiClient.get('/auth/me')
    return response.data
  },

  async updateProfile(profileData: Partial<User>): Promise<User> {
    const response = await apiClient.put('/auth/profile', profileData)
    return response.data
  },

  async register(userData: {
    email: string
    password: string
    fullName: string
    role: 'student' | 'teacher' | 'moderator'
    group?: string
    course?: number
  }): Promise<LoginResponse> {
    const response = await apiClient.post('/auth/register', userData)
    return response.data
  }
} 