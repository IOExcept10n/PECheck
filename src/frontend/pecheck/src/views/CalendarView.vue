<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { useSectionsStore } from '../stores/sections'
import VueCal from 'vue-cal'
import 'vue-cal/dist/vuecal.css'

const auth = useAuthStore()
const sectionsStore = useSectionsStore()
const router = useRouter()
const selectedDate = ref(null)
const selectedEvent = ref(null)
const showEventDialog = ref(false)
const showLessonCreateDialog = ref(false)
const newLesson = ref({
  title: '',
  startTime: '',
  endTime: '',
  location: '',
  description: ''
})

// Check if user is authenticated, if not redirect to login
onMounted(() => {
  if (!auth.isAuthenticated && !auth.checkSavedAuth()) {
    router.push('/login')
  }
})

// Get the user's role
const userRole = computed(() => auth.user?.role || null)

// Get sections based on user role
const userSections = computed(() => {
  if (userRole.value === 'student') {
    // For students, we would typically filter by their assigned section
    return sectionsStore.sections
  } else {
    // For teachers and admins, show all sections
    return sectionsStore.sections
  }
})

// Transform section schedules into calendar events
const calendarEvents = computed(() => {
  const events = []
  
  userSections.value.forEach(section => {
    section.schedule.forEach(schedule => {
      // Get the numeric day of the week (0 = Sunday, 1 = Monday, etc.)
      const dayMap = {
        'Sunday': 0,
        'Monday': 1,
        'Tuesday': 2,
        'Wednesday': 3,
        'Thursday': 4,
        'Friday': 5,
        'Saturday': 6
      }
      
      const dayOfWeek = dayMap[schedule.day]
      
      // Get current date and adjust to the schedule day
      const currentDate = new Date()
      const currentDayOfWeek = currentDate.getDay()
      
      // Calculate the date difference to get to the scheduled day
      let dateDiff = dayOfWeek - currentDayOfWeek
      if (dateDiff < 0) dateDiff += 7
      
      // Create date for this schedule
      const eventDate = new Date(currentDate)
      eventDate.setDate(currentDate.getDate() + dateDiff)
      
      // Set start time
      const [startHour, startMinute] = schedule.startTime.split(':').map(Number)
      const startDate = new Date(eventDate)
      startDate.setHours(startHour, startMinute, 0)
      
      // Set end time
      const [endHour, endMinute] = schedule.endTime.split(':').map(Number)
      const endDate = new Date(eventDate)
      endDate.setHours(endHour, endMinute, 0)
      
      // Add the event
      events.push({
        start: startDate,
        end: endDate,
        title: section.name,
        content: `Location: ${schedule.location}`,
        class: ['section-event'],
        section: section
      })
      
      // Add event for next week too (for demo purposes)
      const nextWeekStartDate = new Date(startDate)
      nextWeekStartDate.setDate(nextWeekStartDate.getDate() + 7)
      
      const nextWeekEndDate = new Date(endDate)
      nextWeekEndDate.setDate(nextWeekEndDate.getDate() + 7)
      
      events.push({
        start: nextWeekStartDate,
        end: nextWeekEndDate,
        title: section.name,
        content: `Location: ${schedule.location}`,
        class: ['section-event'],
        section: section
      })
    })
  })
  
  return events
})

// Handle event click
function onEventClick(event, e) {
  selectedEvent.value = event
  showEventDialog.value = true
}

// Handle day click
function onDayClick(day, e) {
  selectedDate.value = day
  
  if (userRole.value === 'teacher' || userRole.value === 'admin') {
    showLessonCreateDialog.value = true
  }
}

// Add new lesson
function addLesson() {
  // In a real app, this would send data to the backend
  console.log('Adding new lesson:', newLesson.value, 'on date:', selectedDate.value.date)
  
  // Reset form and close dialog
  newLesson.value = {
    title: '',
    startTime: '',
    endTime: '',
    location: '',
    description: ''
  }
  showLessonCreateDialog.value = false
}
</script>

<template>
  <v-container v-if="auth.isAuthenticated">
    <v-card>
      <v-card-title class="d-flex align-center">
        <span class="text-h5">Calendar</span>
        <v-spacer></v-spacer>
        <span class="text-subtitle-1">{{ userRole === 'student' ? 'Click on an event to view details' : 'Click on a day to add a lesson' }}</span>
      </v-card-title>
      
      <v-card-text>
        <vue-cal
          :events="calendarEvents"
          :time-from="8 * 60"
          :time-to="20 * 60"
          :disable-views="['years', 'year']"
          default-view="month"
          :on-event-click="onEventClick"
          :on-cell-click="onDayClick"
          style="height: 600px"
          class="vuecal--green-theme"
        />
      </v-card-text>
    </v-card>
    
    <!-- Event Details Dialog -->
    <v-dialog v-model="showEventDialog" max-width="500">
      <v-card v-if="selectedEvent">
        <v-card-title>
          {{ selectedEvent.title }}
          <v-spacer></v-spacer>
          <v-btn icon @click="showEventDialog = false">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        
        <v-card-text>
          <p><strong>Date:</strong> {{ selectedEvent.start.toLocaleDateString() }}</p>
          <p><strong>Time:</strong> {{ selectedEvent.start.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) }} - {{ selectedEvent.end.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) }}</p>
          <p v-html="selectedEvent.content"></p>
          
          <v-divider class="my-3"></v-divider>
          
          <div v-if="selectedEvent.section">
            <h3 class="text-h6 mb-2">Section Details</h3>
            <p>{{ selectedEvent.section.description }}</p>
          </div>
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="primary" @click="showEventDialog = false">
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
    <!-- Add Lesson Dialog (Teachers and Admins only) -->
    <v-dialog v-model="showLessonCreateDialog" max-width="500">
      <v-card v-if="selectedDate">
        <v-card-title>
          Add Lesson for {{ selectedDate.date.toLocaleDateString() }}
          <v-spacer></v-spacer>
          <v-btn icon @click="showLessonCreateDialog = false">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        
        <v-card-text>
          <v-form @submit.prevent="addLesson">
            <v-text-field
              v-model="newLesson.title"
              label="Lesson Title"
              required
            ></v-text-field>
            
            <v-row>
              <v-col cols="6">
                <v-text-field
                  v-model="newLesson.startTime"
                  label="Start Time"
                  type="time"
                  required
                ></v-text-field>
              </v-col>
              
              <v-col cols="6">
                <v-text-field
                  v-model="newLesson.endTime"
                  label="End Time"
                  type="time"
                  required
                ></v-text-field>
              </v-col>
            </v-row>
            
            <v-text-field
              v-model="newLesson.location"
              label="Location"
              required
            ></v-text-field>
            
            <v-textarea
              v-model="newLesson.description"
              label="Description"
              rows="3"
            ></v-textarea>
          </v-form>
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="secondary" @click="showLessonCreateDialog = false">
            Cancel
          </v-btn>
          <v-btn color="primary" @click="addLesson">
            Add Lesson
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<style>
.vuecal--green-theme .vuecal__event {
  background-color: #4CAF50;
  color: white;
  border-radius: 4px;
}
</style>