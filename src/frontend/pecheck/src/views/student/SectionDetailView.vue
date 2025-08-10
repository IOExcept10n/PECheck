<template>
  <div class="section-detail-container">
    <div v-if="section" class="section-detail">
      <div class="section-header">
        <button @click="goBack" class="btn btn-secondary">
          ← Back to Sections
        </button>
        <h1>{{ section.name }}</h1>
      </div>

      <div class="section-content">
        <div class="section-image">
          <img
            v-if="section.coverImage"
            :src="section.coverImage"
            :alt="section.name"
          />
          <div v-else class="image-placeholder">
            📚
          </div>
        </div>

        <div class="section-info">
          <h2>Description</h2>
          <p>{{ section.description }}</p>

          <h2>Schedule</h2>
          <div class="schedule-list">
            <div
              v-for="lesson in section.schedule"
              :key="lesson.id"
              class="schedule-item"
            >
              <span class="day">{{ getDayName(lesson.dayOfWeek) }}</span>
              <span class="time">{{ lesson.startTime }} - {{ lesson.endTime }}</span>
              <span v-if="lesson.room" class="room">{{ lesson.room }}</span>
            </div>
          </div>

          <div class="section-meta">
            <div class="meta-item">
              <label>Teacher:</label>
              <span>{{ section.teacherName }}</span>
            </div>
            <div class="meta-item">
              <label>Price:</label>
              <span class="price">${{ section.price }}</span>
            </div>
            <div v-if="section.maxStudents" class="meta-item">
              <label>Capacity:</label>
              <span>{{ section.currentStudents || 0 }}/{{ section.maxStudents }}</span>
            </div>
          </div>

          <button class="btn btn-success w-100">
            Enroll in Section
          </button>
        </div>
      </div>
    </div>

    <div v-else class="loading">
      <p>Loading section details...</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useSectionsStore } from '@/stores/sections'

const route = useRoute()
const router = useRouter()
const sectionsStore = useSectionsStore()

const section = computed(() => sectionsStore.currentSection)

const getDayName = (dayOfWeek: number) => {
  const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']
  return days[dayOfWeek]
}

const goBack = () => {
  router.push('/student/sections')
}

onMounted(async () => {
  const sectionId = route.params.id as string
  await sectionsStore.fetchSectionById(sectionId)
})
</script>

<style scoped>
.section-detail-container {
  max-width: 1000px;
  margin: 0 auto;
}

.section-header {
  margin-bottom: 2rem;
}

.section-header h1 {
  margin: 1rem 0;
}

.section-content {
  display: grid;
  grid-template-columns: 1fr 2fr;
  gap: 2rem;
}

.section-image {
  height: 400px;
  overflow: hidden;
  border-radius: var(--border-radius);
  background: var(--secondary-color);
  display: flex;
  align-items: center;
  justify-content: center;
}

.section-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.image-placeholder {
  font-size: 6rem;
  color: var(--text-light);
}

.section-info h2 {
  margin: 1.5rem 0 0.5rem 0;
  color: var(--text-primary);
}

.section-info p {
  color: var(--text-secondary);
  line-height: 1.6;
  margin-bottom: 1.5rem;
}

.schedule-list {
  margin-bottom: 1.5rem;
}

.schedule-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem;
  background: var(--secondary-color);
  border-radius: var(--border-radius-small);
  margin-bottom: 0.5rem;
}

.day {
  font-weight: 600;
  color: var(--text-primary);
}

.time {
  color: var(--text-secondary);
}

.room {
  color: var(--primary-color);
  font-weight: 500;
}

.section-meta {
  margin-bottom: 2rem;
}

.meta-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.5rem 0;
  border-bottom: 1px solid var(--border-color);
}

.meta-item label {
  font-weight: 600;
  color: var(--text-secondary);
}

.meta-item span {
  color: var(--text-primary);
}

.price {
  font-weight: 600;
  color: var(--primary-color);
  font-size: 1.125rem;
}

.loading {
  text-align: center;
  padding: 2rem;
  color: var(--text-secondary);
}

@media (max-width: 768px) {
  .section-content {
    grid-template-columns: 1fr;
  }
  
  .schedule-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.25rem;
  }
}
</style> 