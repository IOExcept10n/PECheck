<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'

const sidebarOpen = ref(window.innerWidth >= 768)
const isMobile = ref(window.innerWidth < 768)
const router = useRouter()
const route = useRoute()
const isDarkMode = ref(localStorage.getItem('darkMode') === 'true')

// Get current page title based on route
const currentPageTitle = computed(() => {
  const routeName = route.name?.toString() || ''
  if (routeName.includes('home')) return 'Главная'
  if (routeName.includes('user')) return 'Пользователи'
  if (routeName.includes('section')) return 'Секции'
  if (routeName.includes('teacher')) return 'Преподаватели'
  if (routeName.includes('statistic')) return 'Статистика'
  if (routeName.includes('profile')) return 'Профиль'
  return routeName.replace(/-/g, ' ').replace(/^\w/, (c) => c.toUpperCase())
})

const toggleSidebar = () => {
  sidebarOpen.value = !sidebarOpen.value
}

const handleResize = () => {
  isMobile.value = window.innerWidth < 768
  if (!isMobile.value && !sidebarOpen.value) {
    sidebarOpen.value = true
  }
}

const toggleDarkMode = () => {
  isDarkMode.value = !isDarkMode.value
  localStorage.setItem('darkMode', isDarkMode.value.toString())
  document.documentElement.classList.toggle('dark-mode', isDarkMode.value)
}

const logout = async () => {
  localStorage.removeItem('token')
  localStorage.removeItem('userRole')
  router.push('/login')
}

onMounted(() => {
  window.addEventListener('resize', handleResize)
  document.documentElement.classList.toggle('dark-mode', isDarkMode.value)
})

onUnmounted(() => {
  window.removeEventListener('resize', handleResize)
})
</script>

<template>
  <div class="layout-container" :class="{ 'dark-mode': isDarkMode }">
    <!-- Sidebar -->
    <aside class="sidebar" :class="{ 'sidebar-collapsed': !sidebarOpen, 'sidebar-mobile': isMobile && sidebarOpen }">
      <div class="sidebar-header">
        <div class="logo">
          <span v-if="sidebarOpen">PE Check</span>
        </div>
        <button v-if="isMobile && sidebarOpen" class="sidebar-close" @click="toggleSidebar">
          <i class="material-icons">close</i>
        </button>
        <button v-if="!isMobile" class="sidebar-toggle" @click="toggleSidebar">
          <i class="material-icons">{{ sidebarOpen ? 'chevron_left' : 'chevron_right' }}</i>
        </button>
      </div>

      <div class="sidebar-content">
        <nav class="sidebar-nav">
          <RouterLink to="/moderator" class="nav-item" exact-active-class="nav-item-active">
            <i class="material-icons">home</i>
            <span v-if="sidebarOpen">Главная</span>
          </RouterLink>
          <RouterLink to="/moderator/users" class="nav-item" active-class="nav-item-active">
            <i class="material-icons">people</i>
            <span v-if="sidebarOpen">Пользователи</span>
          </RouterLink>
          <RouterLink to="/moderator/sections" class="nav-item" active-class="nav-item-active">
            <i class="material-icons">fitness_center</i>
            <span v-if="sidebarOpen">Секции</span>
          </RouterLink>
          <RouterLink to="/moderator/teachers" class="nav-item" active-class="nav-item-active">
            <i class="material-icons">school</i>
            <span v-if="sidebarOpen">Преподаватели</span>
          </RouterLink>
          <RouterLink to="/moderator/statistics" class="nav-item" active-class="nav-item-active">
            <i class="material-icons">bar_chart</i>
            <span v-if="sidebarOpen">Статистика</span>
          </RouterLink>
        </nav>
      </div>

      <div class="sidebar-footer">
        <button class="nav-item logout-btn" @click="logout">
          <i class="material-icons">exit_to_app</i>
          <span v-if="sidebarOpen">Выход</span>
        </button>
      </div>
    </aside>

    <!-- Main content -->
    <main class="main-content" :class="{ 'main-content-expanded': !sidebarOpen || (isMobile && !sidebarOpen) }">
      <header class="main-header">
        <div class="header-left">
          <button class="menu-toggle" @click="toggleSidebar" v-if="isMobile && !sidebarOpen">
            <i class="material-icons">menu</i>
          </button>
          <h1 class="page-title">{{ currentPageTitle }}</h1>
        </div>
        
        <div class="header-actions">
          <button class="action-btn theme-toggle" @click="toggleDarkMode" title="Переключить тему">
            <i class="material-icons">{{ isDarkMode ? 'light_mode' : 'dark_mode' }}</i>
          </button>
          <div class="profile-image" @click="router.push('/moderator/profile')" title="Профиль">
            <i class="material-icons">person</i>
          </div>
        </div>
      </header>

      <div class="content-wrapper">
        <RouterView />
      </div>

      <!-- Mobile bottom navigation -->
      <div class="mobile-nav" v-if="isMobile">
        <RouterLink to="/moderator" class="mobile-nav-item" exact-active-class="mobile-nav-active">
          <i class="material-icons">home</i>
          <span>Главная</span>
        </RouterLink>
        <RouterLink to="/moderator/users" class="mobile-nav-item" active-class="mobile-nav-active">
          <i class="material-icons">people</i>
          <span>Пользователи</span>
        </RouterLink>
        <RouterLink to="/moderator/sections" class="mobile-nav-item" active-class="mobile-nav-active">
          <i class="material-icons">fitness_center</i>
          <span>Секции</span>
        </RouterLink>
      </div>
    </main>

    <!-- Overlay for mobile sidebar -->
    <div class="sidebar-overlay" v-if="isMobile && sidebarOpen" @click="toggleSidebar"></div>
  </div>
</template>

<style scoped>
.layout-container {
  display: flex;
  min-height: 100vh;
  position: relative;
}

/* Sidebar Styles */
.sidebar {
  width: 280px;
  background-color: var(--primary-color);
  color: var(--light-text);
  display: flex;
  flex-direction: column;
  transition: all 0.3s ease;
  height: 100vh;
  position: sticky;
  top: 0;
  z-index: 20;
}

.sidebar-collapsed {
  width: 70px;
}

.sidebar-mobile {
  position: fixed;
  left: 0;
  top: 0;
  height: 100%;
  z-index: 30;
}

.sidebar-header {
  padding: 1.5rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.logo {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  min-width: 32px;
}

.logo span {
  font-size: 1.25rem;
  font-weight: 600;
  white-space: nowrap;
  overflow: hidden;
}

.sidebar-close, .sidebar-toggle {
  background: none;
  border: none;
  color: white;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
}

.sidebar-toggle {
  position: absolute;
  right: -12px;
  top: 20px;
  background-color: var(--primary-color);
  border-radius: 50%;
  width: 24px;
  height: 24px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

.sidebar-content {
  flex: 1;
  overflow-y: auto;
  padding: 1rem 0;
}

.sidebar-nav {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.nav-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 0.75rem 1.5rem;
  color: rgba(255, 255, 255, 0.8);
  text-decoration: none;
  transition: all 0.2s ease;
  border-left: 3px solid transparent;
  min-width: 24px;
}

.nav-item:hover, .nav-item-active {
  background-color: rgba(255, 255, 255, 0.1);
  color: white;
  border-left: 3px solid white;
}

.sidebar-collapsed .nav-item {
  justify-content: center;
  padding: 0.75rem;
}

.sidebar-footer {
  padding: 1rem 0;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
}

.logout-btn {
  width: 100%;
  justify-content: flex-start;
  cursor: pointer;
  background: none;
  border: none;
  color: rgba(255, 255, 255, 0.8);
}

.sidebar-collapsed .logout-btn {
  justify-content: center;
}

.logout-btn:hover {
  color: white;
  background-color: rgba(255, 255, 255, 0.1);
}

/* Main Content Styles */
.main-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  transition: all 0.3s ease;
  position: relative;
  padding-bottom: 60px; /* Space for mobile nav */
}

.main-content-expanded {
  margin-left: 0;
}

.main-header {
  padding: 1rem 1.5rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background-color: white;
  box-shadow: var(--shadow-sm);
  position: sticky;
  top: 0;
  z-index: 10;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.menu-toggle {
  background: none;
  border: none;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  color: var(--text-color);
  padding: 0.5rem;
  border-radius: var(--border-radius);
}

.menu-toggle:hover {
  background-color: rgba(0, 0, 0, 0.05);
}

.page-title {
  font-size: 1.25rem;
  font-weight: 600;
  text-transform: capitalize;
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.action-btn {
  background: none;
  border: none;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  color: var(--text-color);
  padding: 0.5rem;
  border-radius: var(--border-radius);
}

.action-btn:hover {
  background-color: rgba(0, 0, 0, 0.05);
}

.profile-image {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background-color: var(--primary-light);
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
  color: white;
  cursor: pointer;
}

.content-wrapper {
  flex: 1;
  padding: 1.5rem;
  background-color: var(--background-color);
}

/* Mobile Navigation */
.mobile-nav {
  display: none;
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  background-color: white;
  box-shadow: 0 -2px 10px rgba(0, 0, 0, 0.1);
  z-index: 20;
}

.mobile-nav {
  display: flex;
  justify-content: space-around;
  padding: 0.5rem 0;
}

.mobile-nav-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 0.5rem;
  color: var(--text-color);
  text-decoration: none;
  font-size: 0.75rem;
}

.mobile-nav-item i {
  font-size: 1.5rem;
  margin-bottom: 0.25rem;
}

.mobile-nav-active {
  color: var(--primary-color);
}

/* Overlay */
.sidebar-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  z-index: 25;
}

/* Dark Mode */
.dark-mode .main-header {
  background-color: #1e293b;
  color: white;
}

.dark-mode .content-wrapper {
  background-color: #0f172a;
  color: white;
}

.dark-mode .mobile-nav {
  background-color: #1e293b;
}

.dark-mode .mobile-nav-item {
  color: #e2e8f0;
}

.dark-mode .action-btn {
  color: white;
}

.dark-mode .action-btn:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

@media (min-width: 768px) {
  .mobile-nav {
    display: none;
  }
  
  .main-content {
    padding-bottom: 0;
  }
}
</style>