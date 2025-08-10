<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { useSectionsStore, type Section } from '../stores/sections'

const auth = useAuthStore()
const sectionsStore = useSectionsStore()
const router = useRouter()

// Form dialogs
const showAddDialog = ref(false)
const showEditDialog = ref(false)
const showDeleteDialog = ref(false)
const showScheduleDialog = ref(false)

// Current section being edited/deleted
const currentSection = ref<Section | null>(null)

// New section form
const newSection = ref({
  id: '',
  name: '',
  description: '',
  coverImage: '',
  schedule: [],
  reviews: []
})

// Schedule form
const scheduleDay = ref('')
const scheduleStartTime = ref('')
const scheduleEndTime = ref('')
const scheduleLocation = ref('')

// Search
const searchQuery = ref('')

// Filtered sections based on search
const filteredSections = computed(() => {
  if (!searchQuery.value) return sectionsStore.sections
  
  const query = searchQuery.value.toLowerCase()
  return sectionsStore.sections.filter(section => 
    section.id.toLowerCase().includes(query) ||
    section.name.toLowerCase().includes(query) ||
    section.description.toLowerCase().includes(query)
  )
})

// Days of the week
const daysOfWeek = [
  'Monday',
  'Tuesday',
  'Wednesday',
  'Thursday',
  'Friday',
  'Saturday',
  'Sunday'
]

// Check if user is authenticated and has appropriate role
onMounted(() => {
  if (!auth.isAuthenticated && !auth.checkSavedAuth()) {
    router.push('/login')
  } else if (auth.user?.role !== 'admin' && auth.user?.role !== 'teacher') {
    router.push('/')
  }
})

// Add a new section
function addSection() {
  // Create a new section ID if not provided
  if (!newSection.value.id) {
    newSection.value.id = 'SEC' + Math.floor(100 + Math.random() * 900)
  }
  
  // Use a placeholder image if not provided
  if (!newSection.value.coverImage) {
    newSection.value.coverImage = `https://picsum.photos/id/${Math.floor(Math.random() * 1000)}/800/400`
  }
  
  sectionsStore.addSection({
    ...newSection.value,
    schedule: [...newSection.value.schedule],
    reviews: [...newSection.value.reviews]
  })
  
  // Reset form
  newSection.value = {
    id: '',
    name: '',
    description: '',
    coverImage: '',
    schedule: [],
    reviews: []
  }
  
  showAddDialog.value = false
}

// Edit section
function editSection() {
  if (currentSection.value) {
    sectionsStore.updateSection(currentSection.value.id, currentSection.value)
    showEditDialog.value = false
  }
}

// Delete section
function deleteSection() {
  if (currentSection.value) {
    sectionsStore.removeSection(currentSection.value.id)
    showDeleteDialog.value = false
  }
}

// Add schedule to new section
function addScheduleToNew() {
  if (scheduleDay && scheduleStartTime && scheduleEndTime && scheduleLocation) {
    newSection.value.schedule.push({
      day: scheduleDay.value,
      startTime: scheduleStartTime.value,
      endTime: scheduleEndTime.value,
      location: scheduleLocation.value
    })
    
    // Reset form
    scheduleDay.value = ''
    scheduleStartTime.value = ''
    scheduleEndTime.value = ''
    scheduleLocation.value = ''
  }
}

// Add schedule to current section
function addScheduleToCurrent() {
  if (currentSection.value && scheduleDay.value && scheduleStartTime.value && scheduleEndTime.value && scheduleLocation.value) {
    currentSection.value.schedule.push({
      day: scheduleDay.value,
      startTime: scheduleStartTime.value,
      endTime: scheduleEndTime.value,
      location: scheduleLocation.value
    })
    
    // Reset form
    scheduleDay.value = ''
    scheduleStartTime.value = ''
    scheduleEndTime.value = ''
    scheduleLocation.value = ''
  }
}

// Remove schedule from new section
function removeScheduleFromNew(index: number) {
  newSection.value.schedule.splice(index, 1)
}

// Remove schedule from current section
function removeScheduleFromCurrent(index: number) {
  if (currentSection.value) {
    currentSection.value.schedule.splice(index, 1)
  }
}

// Remove review
function removeReview(sectionId: string, reviewId: string) {
  sectionsStore.removeReview(sectionId, reviewId)
}
</script>

<template>
  <v-container v-if="auth.isAuthenticated && (auth.user?.role === 'admin' || auth.user?.role === 'teacher')">
    <v-card class="mb-4">
      <v-card-title class="d-flex align-center">
        <span class="text-h4">Sections Management</span>
        <v-spacer></v-spacer>
        <v-btn color="primary" @click="showAddDialog = true">
          <v-icon left>mdi-plus</v-icon>
          Add Section
        </v-btn>
      </v-card-title>
      
      <v-card-text>
        <v-text-field
          v-model="searchQuery"
          label="Search Sections"
          prepend-icon="mdi-magnify"
          clearable
        ></v-text-field>
      </v-card-text>
    </v-card>
    
    <v-row>
      <v-col v-for="section in filteredSections" :key="section.id" cols="12" md="6">
        <v-card class="mb-4">
          <v-img
            :src="section.coverImage"
            height="200"
            cover
          ></v-img>
          
          <v-card-title>
            {{ section.name }}
            <v-spacer></v-spacer>
            <v-btn icon @click="currentSection = {...section}; showEditDialog = true">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon @click="currentSection = section; showDeleteDialog = true">
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </v-card-title>
          
          <v-card-subtitle>
            ID: {{ section.id }}
          </v-card-subtitle>
          
          <v-card-text>
            <p>{{ section.description }}</p>
            
            <v-divider class="my-3"></v-divider>
            
            <h3 class="text-h6 mb-2">Schedule</h3>
            <v-list v-if="section.schedule.length > 0">
              <v-list-item v-for="(schedule, index) in section.schedule" :key="index">
                <v-list-item-title>
                  {{ schedule.day }}: {{ schedule.startTime }} - {{ schedule.endTime }}
                </v-list-item-title>
                <v-list-item-subtitle>
                  Location: {{ schedule.location }}
                </v-list-item-subtitle>
              </v-list-item>
            </v-list>
            <p v-else>No schedule available</p>
            
            <v-btn color="primary" class="mt-2" @click="currentSection = {...section}; showScheduleDialog = true">
              Edit Schedule
            </v-btn>
            
            <v-divider class="my-3"></v-divider>
            
            <h3 class="text-h6 mb-2">Reviews ({{ section.reviews.length }})</h3>
            <v-list v-if="section.reviews.length > 0">
              <v-list-item v-for="review in section.reviews" :key="review.id">
                <v-list-item-title>
                  {{ review.studentName }}
                  <v-rating
                    :model-value="review.rating"
                    color="amber"
                    density="compact"
                    readonly
                    size="small"
                  ></v-rating>
                </v-list-item-title>
                <v-list-item-subtitle>
                  {{ new Date(review.date).toLocaleDateString() }}
                </v-list-item-subtitle>
                <v-list-item-text>
                  {{ review.comment }}
                </v-list-item-text>
                <v-list-item-action>
                  <v-btn icon small @click="removeReview(section.id, review.id)">
                    <v-icon>mdi-delete</v-icon>
                  </v-btn>
                </v-list-item-action>
              </v-list-item>
            </v-list>
            <p v-else>No reviews yet</p>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
    
    <!-- Add Section Dialog -->
    <v-dialog v-model="showAddDialog" max-width="600px">
      <v-card>
        <v-card-title>
          Add New Section
          <v-spacer></v-spacer>
          <v-btn icon @click="showAddDialog = false">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        
        <v-card-text>
          <v-form @submit.prevent="addSection">
            <v-text-field
              v-model="newSection.id"
              label="Section ID (optional, will be generated if empty)"
              hint="Format: SECxxx"
              persistent-hint
            ></v-text-field>
            
            <v-text-field
              v-model="newSection.name"
              label="Section Name"
              required
            ></v-text-field>
            
            <v-textarea
              v-model="newSection.description"
              label="Description"
              rows="3"
              required
            ></v-textarea>
            
            <v-text-field
              v-model="newSection.coverImage"
              label="Cover Image URL (optional)"
              hint="Will use a placeholder if empty"
              persistent-hint
            ></v-text-field>
            
            <v-divider class="my-4"></v-divider>
            
            <h3 class="text-h6 mb-2">Schedule</h3>
            
            <v-row>
              <v-col cols="6">
                <v-select
                  v-model="scheduleDay"
                  :items="daysOfWeek"
                  label="Day"
                ></v-select>
              </v-col>
              
              <v-col cols="3">
                <v-text-field
                  v-model="scheduleStartTime"
                  label="Start Time"
                  type="time"
                ></v-text-field>
              </v-col>
              
              <v-col cols="3">
                <v-text-field
                  v-model="scheduleEndTime"
                  label="End Time"
                  type="time"
                ></v-text-field>
              </v-col>
            </v-row>
            
            <v-row>
              <v-col cols="9">
                <v-text-field
                  v-model="scheduleLocation"
                  label="Location"
                ></v-text-field>
              </v-col>
              
              <v-col cols="3">
                <v-btn icon @click="addScheduleToNew" :disabled="!scheduleDay || !scheduleStartTime || !scheduleEndTime || !scheduleLocation" class="mt-2">
                  <v-icon>mdi-plus</v-icon>
                </v-btn>
              </v-col>
            </v-row>
            
            <v-list v-if="newSection.schedule.length > 0">
              <v-list-item v-for="(schedule, index) in newSection.schedule" :key="index">
                <v-list-item-title>
                  {{ schedule.day }}: {{ schedule.startTime }} - {{ schedule.endTime }}
                </v-list-item-title>
                <v-list-item-subtitle>
                  Location: {{ schedule.location }}
                </v-list-item-subtitle>
                <v-list-item-action>
                  <v-btn icon small @click="removeScheduleFromNew(index)">
                    <v-icon>mdi-delete</v-icon>
                  </v-btn>
                </v-list-item-action>
              </v-list-item>
            </v-list>
          </v-form>
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="secondary" @click="showAddDialog = false">
            Cancel
          </v-btn>
          <v-btn color="primary" @click="addSection">
            Add Section
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
    <!-- Edit Section Dialog -->
    <v-dialog v-model="showEditDialog" max-width="600px">
      <v-card v-if="currentSection">
        <v-card-title>
          Edit Section: {{ currentSection.name }}
          <v-spacer></v-spacer>
          <v-btn icon @click="showEditDialog = false">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        
        <v-card-text>
          <v-form @submit.prevent="editSection">
            <v-text-field
              v-model="currentSection.id"
              label="Section ID"
              disabled
            ></v-text-field>
            
            <v-text-field
              v-model="currentSection.name"
              label="Section Name"
              required
            ></v-text-field>
            
            <v-textarea
              v-model="currentSection.description"
              label="Description"
              rows="3"
              required
            ></v-textarea>
            
            <v-text-field
              v-model="currentSection.coverImage"
              label="Cover Image URL"
              required
            ></v-text-field>
          </v-form>
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="secondary" @click="showEditDialog = false">
            Cancel
          </v-btn>
          <v-btn color="primary" @click="editSection">
            Save Changes
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
    <!-- Edit Schedule Dialog -->
    <v-dialog v-model="showScheduleDialog" max-width="600px">
      <v-card v-if="currentSection">
        <v-card-title>
          Edit Schedule: {{ currentSection.name }}
          <v-spacer></v-spacer>
          <v-btn icon @click="showScheduleDialog = false">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        
        <v-card-text>
          <h3 class="text-h6 mb-2">Current Schedule</h3>
          <v-list v-if="currentSection.schedule.length > 0">
            <v-list-item v-for="(schedule, index) in currentSection.schedule" :key="index">
              <v-list-item-title>
                {{ schedule.day }}: {{ schedule.startTime }} - {{ schedule.endTime }}
              </v-list-item-title>
              <v-list-item-subtitle>
                Location: {{ schedule.location }}
              </v-list-item-subtitle>
              <v-list-item-action>
                <v-btn icon small @click="removeScheduleFromCurrent(index)">
                  <v-icon>mdi-delete</v-icon>
                </v-btn>
              </v-list-item-action>
            </v-list-item>
          </v-list>
          <p v-else>No schedule available</p>
          
          <v-divider class="my-4"></v-divider>
          
          <h3 class="text-h6 mb-2">Add Schedule Item</h3>
          <v-row>
            <v-col cols="6">
              <v-select
                v-model="scheduleDay"
                :items="daysOfWeek"
                label="Day"
              ></v-select>
            </v-col>
            
            <v-col cols="3">
              <v-text-field
                v-model="scheduleStartTime"
                label="Start Time"
                type="time"
              ></v-text-field>
            </v-col>
            
            <v-col cols="3">
              <v-text-field
                v-model="scheduleEndTime"
                label="End Time"
                type="time"
              ></v-text-field>
            </v-col>
          </v-row>
          
          <v-row>
            <v-col cols="9">
              <v-text-field
                v-model="scheduleLocation"
                label="Location"
              ></v-text-field>
            </v-col>
            
            <v-col cols="3">
              <v-btn icon @click="addScheduleToCurrent" :disabled="!scheduleDay || !scheduleStartTime || !scheduleEndTime || !scheduleLocation" class="mt-2">
                <v-icon>mdi-plus</v-icon>
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="secondary" @click="showScheduleDialog = false">
            Cancel
          </v-btn>
          <v-btn color="primary" @click="showScheduleDialog = false">
            Save Schedule
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
    <!-- Delete Confirmation Dialog -->
    <v-dialog v-model="showDeleteDialog" max-width="400px">
      <v-card v-if="currentSection">
        <v-card-title class="text-h5">
          Confirm Deletion
        </v-card-title>
        
        <v-card-text>
          Are you sure you want to delete the section <strong>{{ currentSection.name }}</strong> (ID: {{ currentSection.id }})?
          This action cannot be undone.
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="secondary" @click="showDeleteDialog = false">
            Cancel
          </v-btn>
          <v-btn color="error" @click="deleteSection">
            Delete
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>