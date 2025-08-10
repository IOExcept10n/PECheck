<template>
  <div class="sections-container">
    <div class="sections-header">
      <h2>Available Sections</h2>
      <div class="search-filters">
        <input
          v-model="searchQuery"
          type="text"
          placeholder="Search sections..."
          class="form-input"
        />
        <select v-model="selectedDay" class="form-input">
          <option value="">All Days</option>
          <option value="0">Sunday</option>
          <option value="1">Monday</option>
          <option value="2">Tuesday</option>
          <option value="3">Wednesday</option>
          <option value="4">Thursday</option>
          <option value="5">Friday</option>
          <option value="6">Saturday</option>
        </select>
      </div>
    </div>

    <div class="sections-grid">
      <div
        v-for="section in filteredSections"
        :key="section.id"
        class="section-card card"
        @click="viewSection(section.id)"
      >
        <div class="section-image">
          <img
            v-if="section.coverImage"
            :src="section.coverImage"
            :alt="section.name"
          />
          <div v-else class="section-placeholder">
            📚
          </div>
        </div>
        <div class="section-content">
          <h3>{{ section.name }}</h3>
          <p>{{ section.description }}</p>
          <div class="section-meta">
            <span class="teacher">{{ section.teacherName }}</span>
            <span class="price">${{ section.price }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useSectionsStore } from '@/stores/sections'

const router = useRouter()
const sectionsStore = useSectionsStore()

const searchQuery = ref('')
const selectedDay = ref('')

const filteredSections = computed(() => {
  let filtered = sectionsStore.sections

  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase()
    filtered = filtered.filter(section =>
      section.name.toLowerCase().includes(query) ||
      section.description.toLowerCase().includes(query) ||
      section.teacherName.toLowerCase().includes(query)
    )
  }

  if (selectedDay.value !== '') {
    const day = parseInt(selectedDay.value)
    filtered = filtered.filter(section =>
      section.schedule.some(lesson => lesson.dayOfWeek === day)
    )
  }

  return filtered
})

const viewSection = (sectionId: string) => {
  router.push(`/student/sections/${sectionId}`)
}

onMounted(() => {
  sectionsStore.fetchSections()
})
</script>

<style scoped>
.sections-container {
  max-width: 1200px;
  margin: 0 auto;
}

.sections-header {
  margin-bottom: 2rem;
}

.sections-header h2 {
  margin-bottom: 1rem;
}

.search-filters {
  display: flex;
  gap: 1rem;
  margin-bottom: 2rem;
}

.sections-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 2rem;
}

.section-card {
  cursor: pointer;
  transition: var(--transition);
  overflow: hidden;
}

.section-card:hover {
  transform: translateY(-4px);
  box-shadow: var(--shadow-medium);
}

.section-image {
  height: 200px;
  overflow: hidden;
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

.section-placeholder {
  font-size: 4rem;
  color: var(--text-light);
}

.section-content {
  padding: 1.5rem;
}

.section-content h3 {
  margin: 0 0 0.5rem 0;
  color: var(--text-primary);
}

.section-content p {
  margin: 0 0 1rem 0;
  color: var(--text-secondary);
  line-height: 1.5;
}

.section-meta {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.teacher {
  color: var(--text-secondary);
  font-size: 0.875rem;
}

.price {
  font-weight: 600;
  color: var(--primary-color);
  font-size: 1.125rem;
}

@media (max-width: 768px) {
  .search-filters {
    flex-direction: column;
  }
  
  .sections-grid {
    grid-template-columns: 1fr;
  }
}
</style> 