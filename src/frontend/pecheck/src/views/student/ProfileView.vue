<template>
  <div class="profile-container">
    <!-- Profile Block -->
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
            <label>Group:</label>
            <span>{{ authStore.user?.group || 'Not specified' }}</span>
          </div>
          <div class="info-item">
            <label>Course:</label>
            <span>{{ authStore.user?.course || 'Not specified' }}</span>
          </div>
        </div>
      </div>
    </div>

    <!-- Section Block -->
    <div class="section-block card mb-4">
      <div class="section-header">
        <h2>Section</h2>
      </div>
      <div class="section-content">
        <div class="section-info">
          <h3>{{ currentSection?.name || 'No section selected' }}</h3>
          <p>{{ currentSection?.description || 'Select a section to view details' }}</p>
        </div>
        <div class="section-price">
          <div class="price-box">
            <span class="price">${{ currentSection?.price || 0 }}</span>
            <button class="btn btn-success" @click="goToSections">
              Buy
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Calendar Block -->
    <div class="calendar-block card mb-4">
      <div class="calendar-header">
        <h2>Attendance Calendar</h2>
        <button class="btn btn-outline" @click="showQRCode = true">
          Show QR Code
        </button>
      </div>
      <div class="calendar-content">
        <div class="calendar-grid">
          <div
            v-for="day in attendanceCalendar"
            :key="day.date"
            class="calendar-day"
            :class="getDayClass(day.status)"
            :title="`${day.date} - ${getStatusText(day.status)}`"
          >
            {{ new Date(day.date).getDate() }}
          </div>
        </div>
        <div class="calendar-legend">
          <div class="legend-item">
            <div class="legend-color visited"></div>
            <span>Visited</span>
          </div>
          <div class="legend-item">
            <div class="legend-color skipped"></div>
            <span>Skipped</span>
          </div>
          <div class="legend-item">
            <div class="legend-color no-lesson"></div>
            <span>No Lesson</span>
          </div>
        </div>
      </div>
    </div>

    <!-- QR Code Modal -->
    <div v-if="showQRCode" class="modal-overlay" @click="showQRCode = false">
      <div class="modal-content glass-card" @click.stop>
        <div class="modal-header">
          <h3>Student QR Code</h3>
          <button class="modal-close" @click="showQRCode = false">&times;</button>
        </div>
        <div class="qr-content">
          <div class="qr-code" ref="qrCodeRef"></div>
          <p class="student-id">Student ID: {{ authStore.user?.id }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useSectionsStore } from '@/stores/sections'
import QRCode from 'qrcode'

const router = useRouter()
const authStore = useAuthStore()
const sectionsStore = useSectionsStore()

const showQRCode = ref(false)
const qrCodeRef = ref<HTMLElement>()

// Mock attendance data for the last 30 days
const attendanceCalendar = ref([
  { date: '2024-01-01', status: 'visited' },
  { date: '2024-01-02', status: 'skipped' },
  { date: '2024-01-03', status: 'no-lesson' },
  { date: '2024-01-04', status: 'visited' },
  { date: '2024-01-05', status: 'visited' },
  // ... more days would be populated from API
])

const currentSection = computed(() => {
  // This would come from the student's enrolled section
  return sectionsStore.sections[0] || null
})

const getDayClass = (status: string) => {
  switch (status) {
    case 'visited':
      return 'day-visited'
    case 'skipped':
      return 'day-skipped'
    default:
      return 'day-no-lesson'
  }
}

const getStatusText = (status: string) => {
  switch (status) {
    case 'visited':
      return 'Visited'
    case 'skipped':
      return 'Skipped'
    default:
      return 'No Lesson'
  }
}

const goToSections = () => {
  router.push('/student/sections')
}

const generateQRCode = async () => {
  if (qrCodeRef.value && authStore.user?.id) {
    try {
      await QRCode.toCanvas(qrCodeRef.value, authStore.user.id, {
        width: 200,
        margin: 2,
        color: {
          dark: '#275886',
          light: '#ffffff'
        }
      })
    } catch (error) {
      console.error('Error generating QR code:', error)
    }
  }
}

onMounted(() => {
  // Load sections data
  sectionsStore.fetchSections()
})

// Watch for QR modal to show and generate QR code
watch(showQRCode, (newValue) => {
  if (newValue) {
    nextTick(() => {
      generateQRCode()
    })
  }
})
</script>

<style scoped>
.profile-container {
  max-width: 800px;
  margin: 0 auto;
}

.profile-block,
.section-block,
.calendar-block {
  background: white;
  border-radius: var(--border-radius);
  box-shadow: var(--shadow-light);
  overflow: hidden;
}

.profile-header,
.section-header,
.calendar-header {
  background: var(--primary-color);
  color: white;
  padding: 1rem 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.profile-header h2,
.section-header h2,
.calendar-header h2 {
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

.section-content {
  padding: 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.section-info {
  flex: 1;
}

.section-info h3 {
  margin: 0 0 0.5rem 0;
  color: var(--text-primary);
}

.section-info p {
  margin: 0;
  color: var(--text-secondary);
}

.section-price {
  flex-shrink: 0;
}

.price-box {
  background: var(--primary-color);
  color: white;
  padding: 1rem 1.5rem;
  border-radius: var(--border-radius);
  text-align: center;
  min-width: 120px;
}

.price {
  display: block;
  font-size: 1.5rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
}

.calendar-content {
  padding: 1.5rem;
}

.calendar-grid {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  gap: 0.5rem;
  margin-bottom: 1.5rem;
}

.calendar-day {
  aspect-ratio: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: var(--border-radius-small);
  font-weight: 500;
  cursor: pointer;
  transition: var(--transition);
}

.day-visited {
  background: var(--accent-color);
  color: white;
}

.day-skipped {
  background: var(--danger-color);
  color: white;
}

.day-no-lesson {
  background: #e0e0e0;
  color: var(--text-secondary);
}

.calendar-legend {
  display: flex;
  gap: 2rem;
  justify-content: center;
}

.legend-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.legend-color {
  width: 20px;
  height: 20px;
  border-radius: 4px;
}

.legend-color.visited {
  background: var(--accent-color);
}

.legend-color.skipped {
  background: var(--danger-color);
}

.legend-color.no-lesson {
  background: #e0e0e0;
}

/* Modal Styles */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 1rem;
}

.modal-content {
  background: white;
  border-radius: var(--border-radius);
  padding: 2rem;
  max-width: 400px;
  width: 100%;
  text-align: center;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.modal-header h3 {
  margin: 0;
}

.modal-close {
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
  color: var(--text-secondary);
  padding: 0;
  width: 30px;
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  transition: var(--transition);
}

.modal-close:hover {
  background: rgba(0, 0, 0, 0.1);
  color: var(--text-primary);
}

.qr-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
}

.qr-code {
  border: 1px solid var(--border-color);
  border-radius: var(--border-radius-small);
  padding: 1rem;
  background: white;
}

.student-id {
  font-weight: 500;
  color: var(--text-secondary);
  margin: 0;
}

@media (max-width: 768px) {
  .profile-content {
    flex-direction: column;
    text-align: center;
    gap: 1rem;
  }

  .section-content {
    flex-direction: column;
    gap: 1rem;
    text-align: center;
  }

  .calendar-grid {
    grid-template-columns: repeat(7, 1fr);
    gap: 0.25rem;
  }

  .calendar-legend {
    flex-direction: column;
    gap: 0.5rem;
  }
}
</style> 