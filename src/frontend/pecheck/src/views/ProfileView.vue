<script setup lang="ts">

// Mock data
const user = {
  avatar: 'https://i.pravatar.cc/150?img=32',
  firstName: 'Иван',
  lastName: 'Иванов',
  middleName: 'Иванович',
  group: 'ПИ-201',
  faculty: 'Институт информационных технологий'
}

const section = {
  name: 'Баскетбол',
  description: 'Тренировки по баскетболу для всех уровней подготовки',
  price: 2000,
  attendance: 85
}
</script>

<template>
  <div class="profile-page">
    <!-- User Info Card -->
    <div class="profile-card card">
      <div class="profile-header">
        <div class="avatar">
          <img :src="user.avatar" :alt="user.firstName">
        </div>
        <div class="user-info">
          <h2>{{ user.lastName }} {{ user.firstName }} {{ user.middleName }}</h2>
          <div class="user-details">
            <p><i class="material-icons">school</i> {{ user.group }}</p>
            <p><i class="material-icons">account_balance</i> {{ user.faculty }}</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Section Info Card -->
    <div class="section-card card">
      <h3><i class="material-icons">sports_basketball</i> Моя секция</h3>
      <div class="section-info">
        <div class="section-header">
          <div>
            <h4>{{ section.name }}</h4>
            <p class="section-description">{{ section.description }}</p>
          </div>
          <div class="price">{{ section.price }} ₽/мес</div>
        </div>
        <div class="attendance">
          <div class="attendance-header">
            <span>Посещаемость</span>
            <span class="attendance-percent">{{ section.attendance }}%</span>
          </div>
          <div class="progress-bar">
            <div
              class="progress-fill"
              :style="{ width: `${section.attendance}%` }"
              :class="{
                'high': section.attendance >= 80,
                'medium': section.attendance >= 60 && section.attendance < 80,
                'low': section.attendance < 60
              }"
            ></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.profile-page {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
  max-width: 800px;
  margin: 0 auto;
  padding: 1rem;
}

.card {
  background: var(--surface-color);
  border-radius: var(--border-radius);
  box-shadow: var(--shadow-sm);
  transition: var(--transition-base);
}

.card:hover {
  box-shadow: var(--shadow-md);
}

/* Profile Card */
.profile-card {
  padding: 2rem;
}

.profile-header {
  display: flex;
  gap: 2rem;
  align-items: center;
}

.avatar {
  width: 120px;
  height: 120px;
  border-radius: 50%;
  overflow: hidden;
  box-shadow: var(--shadow-md);
  flex-shrink: 0;
}

.avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.user-info h2 {
  margin: 0 0 1rem;
  color: var(--text-color);
  font-size: 1.5rem;
}

.user-details {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.user-details p {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin: 0;
  color: var(--text-muted);
}

.user-details i {
  color: var(--primary-color);
  font-size: 1.25rem;
}

/* Section Card */
.section-card {
  padding: 1.5rem;
}

.section-card h3 {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin: 0 0 1.5rem;
  color: var(--text-color);
  font-size: 1.25rem;
}

.section-card h3 i {
  color: var(--primary-color);
}

.section-info {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 1rem;
}

.section-header h4 {
  margin: 0 0 0.5rem;
  color: var(--text-color);
  font-size: 1.125rem;
}

.section-description {
  margin: 0;
  color: var(--text-muted);
  font-size: 0.875rem;
}

.price {
  font-weight: 600;
  color: var(--primary-color);
  font-size: 1.25rem;
  white-space: nowrap;
}

.attendance {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.attendance-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  color: var(--text-muted);
  font-size: 0.875rem;
}

.attendance-percent {
  font-weight: 600;
}

.progress-bar {
  height: 8px;
  background-color: var(--border-color);
  border-radius: 4px;
  overflow: hidden;
}

.progress-fill {
  height: 100%;
  transition: width 0.3s ease;
}

.progress-fill.high {
  background-color: var(--success-color);
}

.progress-fill.medium {
  background-color: var(--warning-color);
}

.progress-fill.low {
  background-color: var(--error-color);
}

/* Dark mode */
:global(.dark-mode) .card {
  background-color: var(--surface-dark);
}

:global(.dark-mode) .user-info h2,
:global(.dark-mode) .section-card h3,
:global(.dark-mode) .section-header h4 {
  color: var(--text-dark);
}

:global(.dark-mode) .user-details p,
:global(.dark-mode) .section-description,
:global(.dark-mode) .attendance-header {
  color: var(--text-muted-dark);
}

:global(.dark-mode) .progress-bar {
  background-color: var(--border-dark);
}

/* Responsive */
@media (max-width: 600px) {
  .profile-header {
    flex-direction: column;
    text-align: center;
    gap: 1rem;
  }

  .avatar {
    width: 100px;
    height: 100px;
  }

  .user-details {
    align-items: center;
  }

  .section-header {
    flex-direction: column;
    text-align: center;
  }

  .price {
    margin-top: 0.5rem;
  }
}
</style>
