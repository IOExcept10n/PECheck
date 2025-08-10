<template>
  <div class="profile-container">
    <div class="profile-block card mb-4">
      <div class="profile-header">
        <h2>Profile</h2>
      </div>
      <div class="profile-content">
        <div class="profile-picture">
          <img
            v-if="authStore.user?.profilePicture"
            :src="authStore.user.profilePicture"
            :alt="authStore.user?.fullName"
            class="avatar"
          />
          <div v-else class="avatar-placeholder">
            {{ authStore.user?.fullName?.charAt(0) }}
          </div>
        </div>
        <div class="profile-info">
          <div class="info-item">
            <label>Full name:</label>
            <span>{{ authStore.user?.fullName }}</span>
          </div>
          <div class="info-item">
            <label>Email:</label>
            <span>{{ authStore.user?.email }}</span>
          </div>
          <div class="info-item">
            <label>Role:</label>
            <span>{{ authStore.user?.role }}</span>
          </div>
        </div>
      </div>
    </div>

    <div class="sections-block card mb-4">
      <div class="sections-header">
        <h2>My Sections</h2>
      </div>
      <div class="sections-content">
        <div
          v-for="section in teacherSections"
          :key="section.id"
          class="section-item"
        >
          <div class="section-info">
            <h3>{{ section.name }}</h3>
            <p>{{ section.description }}</p>
          </div>
          <div class="section-actions">
            <button class="btn btn-outline" @click="editSection(section.id)">
              Edit
            </button>
          </div>
        </div>
        <div v-if="teacherSections.length === 0" class="no-sections">
          <p>No sections assigned yet.</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()

// Mock data for teacher sections
const teacherSections = ref([
  {
    id: '1',
    name: 'Basketball Basics',
    description: 'Learn fundamental basketball skills and techniques'
  },
  {
    id: '2',
    name: 'Swimming for Beginners',
    description: 'Introduction to swimming techniques and water safety'
  }
])

const editSection = (sectionId: string) => {
  router.push(`/teacher/sections/${sectionId}/edit`)
}
</script>

<style scoped>
.profile-container {
  max-width: 800px;
  margin: 0 auto;
}

.profile-block,
.sections-block {
  background: white;
  border-radius: var(--border-radius);
  box-shadow: var(--shadow-light);
  overflow: hidden;
}

.profile-header,
.sections-header {
  background: var(--primary-color);
  color: white;
  padding: 1rem 1.5rem;
}

.profile-header h2,
.sections-header h2 {
  margin: 0;
  font-size: 1.25rem;
}

.profile-content {
  padding: 1.5rem;
  display: flex;
  align-items: center;
  gap: 2rem;
}

.profile-picture {
  flex-shrink: 0;
}

.avatar,
.avatar-placeholder {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  object-fit: cover;
}

.avatar-placeholder {
  background: var(--primary-color);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2rem;
  font-weight: 600;
}

.profile-info {
  flex: 1;
}

.info-item {
  margin-bottom: 1rem;
  display: flex;
  align-items: center;
}

.info-item label {
  font-weight: 600;
  color: var(--text-secondary);
  min-width: 100px;
  margin-right: 1rem;
}

.info-item span {
  color: var(--text-primary);
  font-weight: 500;
}

.sections-content {
  padding: 1.5rem;
}

.section-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  border: 1px solid var(--border-color);
  border-radius: var(--border-radius-small);
  margin-bottom: 1rem;
}

.section-info h3 {
  margin: 0 0 0.5rem 0;
  color: var(--text-primary);
}

.section-info p {
  margin: 0;
  color: var(--text-secondary);
}

.no-sections {
  text-align: center;
  padding: 2rem;
  color: var(--text-secondary);
}

@media (max-width: 768px) {
  .profile-content {
    flex-direction: column;
    text-align: center;
    gap: 1rem;
  }
  
  .section-item {
    flex-direction: column;
    gap: 1rem;
    text-align: center;
  }
}
</style> 