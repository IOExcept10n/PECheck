<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const email = ref('')
const password = ref('')
const loading = ref(false)
const errorMessage = ref('')
const router = useRouter()

// Login function
const login = async () => {
  if (!email.value || !password.value) {
    errorMessage.value = 'Пожалуйста, введите email и пароль'
    return
  }
  
  loading.value = true
  errorMessage.value = ''
  
  try {
    // Mock login for demonstration
    if (email.value.includes('student')) {
      localStorage.setItem('token', 'mock-student-token')
      localStorage.setItem('userRole', 'Student')
      router.push('/student')
    } else if (email.value.includes('teacher')) {
      localStorage.setItem('token', 'mock-teacher-token')
      localStorage.setItem('userRole', 'Teacher')
      router.push('/teacher')
    } else if (email.value.includes('admin')) {
      localStorage.setItem('token', 'mock-admin-token')
      localStorage.setItem('userRole', 'Moderator')
      router.push('/moderator')
    } else {
      errorMessage.value = 'Неверный email. Используйте демо-аккаунты, указанные ниже.'
    }
  } catch (error: any) {
    errorMessage.value = error.message || 'Произошла непредвиденная ошибка'
  } finally {
    loading.value = false
  }
}

// Demo login functions
const loginAsStudent = () => {
  email.value = 'student@example.com'
  password.value = 'password'
  login()
}

const loginAsTeacher = () => {
  email.value = 'teacher@example.com'
  password.value = 'password'
  login()
}

const loginAsAdmin = () => {
  email.value = 'admin@example.com'
  password.value = 'password'
  login()
}
</script>

<template>
  <div class="login-page">
    <div class="login-container glass">
      <div class="login-header">
        <div class="logo">
          <h1>Вход</h1>
        </div>
      </div>
      
      <div class="login-form-container">
        <div v-if="errorMessage" class="error-message">
          {{ errorMessage }}
        </div>
        
        <form @submit.prevent="login" class="login-form">
          <div class="form-group">
            <label for="email" class="form-label">Email</label>
            <input
              id="email"
              v-model="email"
              type="email"
              class="form-input"
              placeholder="Введите email"
              required
            />
          </div>
          
          <div class="form-group">
            <label for="password" class="form-label">Пароль</label>
            <input
              id="password"
              v-model="password"
              type="password"
              class="form-input"
              placeholder="Введите пароль"
              required
            />
          </div>
          
          <div class="form-actions">
            <div class="remember-me">
              <input type="checkbox" id="remember" />
              <label for="remember">Запомнить меня</label>
            </div>
            <a href="#" class="forgot-password">Забыли пароль?</a>
          </div>
          
          <button type="submit" class="btn btn-primary login-btn" :disabled="loading">
            <span v-if="loading">Вход...</span>
            <span v-else>Войти</span>
          </button>
        </form>
        
        <div class="demo-logins">
          <h3>Демо-аккаунты</h3>
          <div class="demo-buttons">
            <button class="btn btn-demo" @click="loginAsStudent">
              Войти как студент
            </button>
            <button class="btn btn-demo" @click="loginAsTeacher">
              Войти как преподаватель
            </button>
            <button class="btn btn-demo" @click="loginAsAdmin">
              Войти как администратор
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.login-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #a8c8e8 0%, #275886 100%);
  background-image: 
    url("data:image/svg+xml,%3Csvg width='100' height='100' viewBox='0 0 100 100' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath d='M11 18c3.866 0 7-3.134 7-7s-3.134-7-7-7-7 3.134-7 7 3.134 7 7 7zm48 25c3.866 0 7-3.134 7-7s-3.134-7-7-7-7 3.134-7 7 3.134 7 7 7zm-43-7c1.657 0 3-1.343 3-3s-1.343-3-3-3-3 1.343-3 3 1.343 3 3 3zm63 31c1.657 0 3-1.343 3-3s-1.343-3-3-3-3 1.343-3 3 1.343 3 3 3zM34 90c1.657 0 3-1.343 3-3s-1.343-3-3-3-3 1.343-3 3 1.343 3 3 3zm56-76c1.657 0 3-1.343 3-3s-1.343-3-3-3-3 1.343-3 3 1.343 3 3 3zM12 86c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm28-65c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm23-11c2.76 0 5-2.24 5-5s-2.24-5-5-5-5 2.24-5 5 2.24 5 5 5zm-6 60c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm29 22c2.76 0 5-2.24 5-5s-2.24-5-5-5-5 2.24-5 5 2.24 5 5 5zM32 63c2.76 0 5-2.24 5-5s-2.24-5-5-5-5 2.24-5 5 2.24 5 5 5zm57-13c2.76 0 5-2.24 5-5s-2.24-5-5-5-5 2.24-5 5 2.24 5 5 5zm-9-21c1.105 0 2-.895 2-2s-.895-2-2-2-2 .895-2 2 .895 2 2 2zM60 91c1.105 0 2-.895 2-2s-.895-2-2-2-2 .895-2 2 .895 2 2 2zM35 41c1.105 0 2-.895 2-2s-.895-2-2-2-2 .895-2 2 .895 2 2 2zM12 60c1.105 0 2-.895 2-2s-.895-2-2-2-2 .895-2 2 .895 2 2 2z' fill='rgba(255,255,255,.075)' fill-rule='evenodd'/%3E%3C/svg%3E"),
    linear-gradient(135deg, #a8c8e8 0%, #275886 100%);
  padding: 2rem;
}

.login-container {
  width: 100%;
  max-width: 400px;
  border-radius: var(--border-radius-lg);
  overflow: hidden;
}

.login-header {
  text-align: center;
  padding: 1.5rem 1.5rem 0.5rem;
}

.logo h1 {
  font-size: 1.75rem;
  font-weight: 600;
  color: var(--primary-color);
  text-align: center;
  margin: 0;
}

.login-form-container {
  padding: 0 2rem 2rem;
}

.login-form {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 0.875rem;
}

.remember-me {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.forgot-password {
  color: var(--primary-color);
  text-decoration: none;
}

.forgot-password:hover {
  text-decoration: underline;
}

.login-btn {
  width: 100%;
  padding: 0.75rem;
  font-weight: 600;
  margin-top: 0.5rem;
}

.error-message {
  background-color: rgba(239, 68, 68, 0.1);
  color: var(--error-color);
  padding: 0.75rem;
  border-radius: var(--border-radius);
  margin-bottom: 1rem;
}

.demo-logins {
  margin-top: 2rem;
  padding-top: 1rem;
  border-top: 1px solid var(--border-color);
}

.demo-logins h3 {
  text-align: center;
  margin-bottom: 1rem;
  color: #64748b;
  font-weight: 500;
}

.demo-buttons {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.btn-demo {
  padding: 0.75rem;
  background-color: rgba(39, 88, 134, 0.1);
  color: var(--primary-color);
  border: 1px solid var(--primary-color);
  border-radius: var(--border-radius);
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-demo:hover {
  background-color: rgba(39, 88, 134, 0.2);
}

/* Glass effect enhancement */
.glass {
  background: rgba(255, 255, 255, 0.8);
  backdrop-filter: blur(10px);
  -webkit-backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.2);
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
}

@media (max-width: 480px) {
  .login-container {
    border-radius: 0;
    height: 100vh;
    max-width: 100%;
  }
  
  .login-page {
    padding: 0;
  }
}
</style>