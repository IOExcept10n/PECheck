import { createRouter, createWebHistory } from 'vue-router'

// Layouts
import StudentLayout from '@/layouts/StudentLayout.vue'
import TeacherLayout from '@/layouts/TeacherLayout.vue'
import ModeratorLayout from '@/layouts/ModeratorLayout.vue'

// Pages
import LoginView from '@/views/LoginView.vue'
import HomeView from '@/views/HomeView.vue'
import ProfileView from '@/views/ProfileView.vue'
import SectionsView from '@/views/SectionsView.vue'
import NormativesView from '@/views/NormativesView.vue'
import AttendanceView from '@/views/AttendanceView.vue'
import StudentsView from '@/views/StudentsView.vue'
import TeachersView from '@/views/TeachersView.vue'
import StatisticsView from '@/views/StatisticsView.vue'

// Function to check if user has appropriate role
const hasRole = (requiredRole: string): boolean => {
  const token = localStorage.getItem('token')
  if (!token) return false
  
  const userRole = localStorage.getItem('userRole') || 'Student'
  return userRole === requiredRole
}

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/login'
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    
    // Student routes
    {
      path: '/student',
      component: StudentLayout,
      meta: { requiresAuth: true, role: 'Student' },
      beforeEnter: (to, from, next) => {
        if (hasRole('Student')) next()
        else next('/login')
      },
      children: [
        {
          path: '',
          name: 'student-home',
          component: HomeView
        },
        {
          path: 'profile',
          name: 'student-profile',
          component: ProfileView
        },
        {
          path: 'sections',
          name: 'student-sections',
          component: SectionsView
        },
        {
          path: 'normatives',
          name: 'student-normatives',
          component: NormativesView
        }
      ]
    },
    
    // Teacher routes
    {
      path: '/teacher',
      component: TeacherLayout,
      meta: { requiresAuth: true, role: 'Teacher' },
      beforeEnter: (to, from, next) => {
        if (hasRole('Teacher')) next()
        else next('/login')
      },
      children: [
        {
          path: '',
          name: 'teacher-home',
          component: HomeView
        },
        {
          path: 'profile',
          name: 'teacher-profile',
          component: ProfileView
        },
        {
          path: 'sections',
          name: 'teacher-sections',
          component: SectionsView
        },
        {
          path: 'attendance',
          name: 'teacher-attendance',
          component: AttendanceView
        },
        {
          path: 'students',
          name: 'teacher-students',
          component: StudentsView
        }
      ]
    },
    
    // Moderator routes
    {
      path: '/moderator',
      component: ModeratorLayout,
      meta: { requiresAuth: true, role: 'Moderator' },
      beforeEnter: (to, from, next) => {
        if (hasRole('Moderator')) next()
        else next('/login')
      },
      children: [
        {
          path: '',
          name: 'moderator-home',
          component: HomeView
        },
        {
          path: 'profile',
          name: 'moderator-profile',
          component: ProfileView
        },
        {
          path: 'sections',
          name: 'moderator-sections',
          component: SectionsView
        },
        {
          path: 'users',
          name: 'moderator-users',
          component: StudentsView
        },
        {
          path: 'teachers',
          name: 'moderator-teachers',
          component: TeachersView
        },
        {
          path: 'statistics',
          name: 'moderator-statistics',
          component: StatisticsView
        }
      ]
    },
    
    // Catch-all route for 404
    {
      path: '/:pathMatch(.*)*',
      redirect: '/login'
    }
  ]
})

// Global navigation guard
router.beforeEach((to, from, next) => {
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth)
  const token = localStorage.getItem('token')
  
  if (requiresAuth && !token) {
    next('/login')
  } else {
    next()
  }
})

export default router