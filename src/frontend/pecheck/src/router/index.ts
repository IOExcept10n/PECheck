import { createRouter, createWebHistory } from 'vue-router'
<<<<<<< Updated upstream
import { useAuthStore } from '../stores/auth'

// Routes
const routes = [
  {
    path: '/',
    name: 'home',
    component: () => import('../views/HomeView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('../views/LoginView.vue'),
    meta: { guest: true }
  },
  {
    path: '/calendar',
    name: 'calendar',
    component: () => import('../views/CalendarView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/profile',
    name: 'profile',
    component: () => import('../views/ProfileView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/news',
    name: 'news',
    component: () => import('../views/NewsView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/students',
    name: 'students',
    component: () => import('../views/StudentsView.vue'),
    meta: { requiresAuth: true, requiresAdmin: true }
  },
  {
    path: '/teachers',
    name: 'teachers',
    component: () => import('../views/TeachersView.vue'),
    meta: { requiresAuth: true, requiresAdmin: true }
  },
  {
    path: '/sections',
    name: 'sections',
    component: () => import('../views/SectionsView.vue'),
    meta: { requiresAuth: true, requiresAdminOrTeacher: true }
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

// Navigation guards
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth)
  const requiresAdmin = to.matched.some(record => record.meta.requiresAdmin)
  const requiresAdminOrTeacher = to.matched.some(record => record.meta.requiresAdminOrTeacher)
  const isAuthenticated = authStore.isAuthenticated || authStore.checkSavedAuth()
  
  // Redirect to login if authentication is required and user is not authenticated
  if (requiresAuth && !isAuthenticated) {
    next('/login')
    return
  }
  
  // Redirect to home if user is authenticated and trying to access login
  if (to.path === '/login' && isAuthenticated) {
    next('/')
    return
  }
  
  // Check admin permissions
  if (requiresAdmin && authStore.user?.role !== 'admin') {
    next('/')
    return
  }
  
  // Check admin or teacher permissions
  if (requiresAdminOrTeacher && authStore.user?.role !== 'admin' && authStore.user?.role !== 'teacher') {
    next('/')
    return
  }
  
  next()
})

export default router
=======
import { useAuthStore } from '@/stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'login',
      component: () => import('@/views/LoginView.vue'),
      meta: { requiresAuth: false }
    },
    {
      path: '/student',
      name: 'student',
      component: () => import('@/views/StudentLayout.vue'),
      meta: { requiresAuth: true, role: 'student' },
      children: [
        {
          path: '',
          name: 'student-profile',
          component: () => import('@/views/student/ProfileView.vue')
        },
        {
          path: 'sections',
          name: 'student-sections',
          component: () => import('@/views/student/SectionsView.vue')
        },
        {
          path: 'sections/:id',
          name: 'student-section-detail',
          component: () => import('@/views/student/SectionDetailView.vue')
        },
        {
          path: 'normatives',
          name: 'student-normatives',
          component: () => import('@/views/student/NormativesView.vue')
        }
      ]
    },
    {
      path: '/teacher',
      name: 'teacher',
      component: () => import('@/views/TeacherLayout.vue'),
      meta: { requiresAuth: true, role: 'teacher' },
      children: [
        {
          path: '',
          name: 'teacher-profile',
          component: () => import('@/views/teacher/ProfileView.vue')
        },
        {
          path: 'calendar',
          name: 'teacher-calendar',
          component: () => import('@/views/teacher/CalendarView.vue')
        },
        {
          path: 'sections/:id/edit',
          name: 'teacher-section-edit',
          component: () => import('@/views/teacher/SectionEditView.vue')
        }
      ]
    },
    {
      path: '/moderator',
      name: 'moderator',
      component: () => import('@/views/ModeratorLayout.vue'),
      meta: { requiresAuth: true, role: 'moderator' },
      children: [
        {
          path: '',
          name: 'moderator-profile',
          component: () => import('@/views/moderator/ProfileView.vue')
        },
        {
          path: 'students',
          name: 'moderator-students',
          component: () => import('@/views/moderator/StudentsView.vue')
        },
        {
          path: 'teachers',
          name: 'moderator-teachers',
          component: () => import('@/views/moderator/TeachersView.vue')
        },
        {
          path: 'sections',
          name: 'moderator-sections',
          component: () => import('@/views/moderator/SectionsView.vue')
        }
      ]
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'not-found',
      component: () => import('@/views/NotFoundView.vue')
    }
  ]
})

// Navigation guard
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next('/')
  } else if (to.meta.role && authStore.user?.role !== to.meta.role) {
    // Redirect to appropriate dashboard based on user role
    const roleRoutes = {
      student: '/student',
      teacher: '/teacher',
      moderator: '/moderator'
    }
    next(roleRoutes[authStore.user?.role as keyof typeof roleRoutes] || '/')
  } else {
    next()
  }
})

export default router 
>>>>>>> Stashed changes
