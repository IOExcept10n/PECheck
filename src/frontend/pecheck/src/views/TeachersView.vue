<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { useTeachersStore, type Teacher } from '../stores/teachers'
import { useSectionsStore } from '../stores/sections'

const auth = useAuthStore()
const teachersStore = useTeachersStore()
const sectionsStore = useSectionsStore()
const router = useRouter()

// Form dialogs
const showAddDialog = ref(false)
const showEditDialog = ref(false)
const showDeleteDialog = ref(false)

// Current teacher being edited/deleted
const currentTeacher = ref<Teacher | null>(null)

// New teacher form
const newTeacher = ref({
  id: '',
  name: '',
  sections: [],
  email: '',
  phone: ''
})

// Search
const searchQuery = ref('')

// Filtered teachers based on search
const filteredTeachers = computed(() => {
  if (!searchQuery.value) return teachersStore.teachers
  
  const query = searchQuery.value.toLowerCase()
  return teachersStore.teachers.filter(teacher => 
    teacher.id.toLowerCase().includes(query) ||
    teacher.name.toLowerCase().includes(query) ||
    teacher.email.toLowerCase().includes(query) ||
    teacher.phone.toLowerCase().includes(query) ||
    teacher.sections.some(section => section.toLowerCase().includes(query))
  )
})

// Available sections
const availableSections = computed(() => {
  return sectionsStore.sections.map(section => section.name)
})

// Check if user is authenticated and is admin, if not redirect to login
onMounted(() => {
  if (!auth.isAuthenticated && !auth.checkSavedAuth()) {
    router.push('/login')
  } else if (auth.user?.role !== 'admin') {
    router.push('/')
  }
})

// Add a new teacher
function addTeacher() {
  // Create a new teacher ID if not provided
  if (!newTeacher.value.id) {
    newTeacher.value.id = 'T' + Math.floor(10000 + Math.random() * 90000)
  }
  
  teachersStore.addTeacher({
    ...newTeacher.value,
    sections: [...newTeacher.value.sections]
  })
  
  // Reset form
  newTeacher.value = {
    id: '',
    name: '',
    sections: [],
    email: '',
    phone: ''
  }
  
  showAddDialog.value = false
}

// Edit teacher
function editTeacher() {
  if (currentTeacher.value) {
    teachersStore.updateTeacher(currentTeacher.value.id, currentTeacher.value)
    showEditDialog.value = false
  }
}

// Delete teacher
function deleteTeacher() {
  if (currentTeacher.value) {
    teachersStore.removeTeacher(currentTeacher.value.id)
    showDeleteDialog.value = false
  }
}
</script>

<template>
  <v-container v-if="auth.isAuthenticated && auth.user?.role === 'admin'">
    <v-card class="mb-4">
      <v-card-title class="d-flex align-center">
        <span class="text-h4">Teachers Management</span>
        <v-spacer></v-spacer>
        <v-btn color="primary" @click="showAddDialog = true">
          <v-icon left>mdi-plus</v-icon>
          Add Teacher
        </v-btn>
      </v-card-title>
      
      <v-card-text>
        <v-text-field
          v-model="searchQuery"
          label="Search Teachers"
          prepend-icon="mdi-magnify"
          clearable
        ></v-text-field>
        
        <v-data-table
          :headers="[
            { title: 'ID', key: 'id' },
            { title: 'Name', key: 'name' },
            { title: 'Email', key: 'email' },
            { title: 'Phone', key: 'phone' },
            { title: 'Sections', key: 'sections' },
            { title: 'Actions', key: 'actions', sortable: false }
          ]"
          :items="filteredTeachers"
        >
          <template v-slot:item.sections="{ item }">
            <v-chip
              v-for="section in item.sections"
              :key="section"
              color="primary"
              class="ma-1"
              size="small"
            >
              {{ section }}
            </v-chip>
          </template>
          
          <template v-slot:item.actions="{ item }">
            <v-btn icon small @click="currentTeacher = {...item}; showEditDialog = true">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon small @click="currentTeacher = item; showDeleteDialog = true">
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>
    
    <!-- Add Teacher Dialog -->
    <v-dialog v-model="showAddDialog" max-width="600px">
      <v-card>
        <v-card-title>
          Add New Teacher
          <v-spacer></v-spacer>
          <v-btn icon @click="showAddDialog = false">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        
        <v-card-text>
          <v-form @submit.prevent="addTeacher">
            <v-text-field
              v-model="newTeacher.id"
              label="Teacher ID (optional, will be generated if empty)"
              hint="Format: Txxxxx"
              persistent-hint
            ></v-text-field>
            
            <v-text-field
              v-model="newTeacher.name"
              label="Full Name"
              required
            ></v-text-field>
            
            <v-text-field
              v-model="newTeacher.email"
              label="Email"
              type="email"
              required
            ></v-text-field>
            
            <v-text-field
              v-model="newTeacher.phone"
              label="Phone Number"
              required
            ></v-text-field>
            
            <v-select
              v-model="newTeacher.sections"
              :items="availableSections"
              label="Sections"
              multiple
              chips
            ></v-select>
          </v-form>
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="secondary" @click="showAddDialog = false">
            Cancel
          </v-btn>
          <v-btn color="primary" @click="addTeacher">
            Add Teacher
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
    <!-- Edit Teacher Dialog -->
    <v-dialog v-model="showEditDialog" max-width="600px">
      <v-card v-if="currentTeacher">
        <v-card-title>
          Edit Teacher: {{ currentTeacher.name }}
          <v-spacer></v-spacer>
          <v-btn icon @click="showEditDialog = false">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        
        <v-card-text>
          <v-form @submit.prevent="editTeacher">
            <v-text-field
              v-model="currentTeacher.id"
              label="Teacher ID"
              disabled
            ></v-text-field>
            
            <v-text-field
              v-model="currentTeacher.name"
              label="Full Name"
              required
            ></v-text-field>
            
            <v-text-field
              v-model="currentTeacher.email"
              label="Email"
              type="email"
              required
            ></v-text-field>
            
            <v-text-field
              v-model="currentTeacher.phone"
              label="Phone Number"
              required
            ></v-text-field>
            
            <v-select
              v-model="currentTeacher.sections"
              :items="availableSections"
              label="Sections"
              multiple
              chips
            ></v-select>
          </v-form>
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="secondary" @click="showEditDialog = false">
            Cancel
          </v-btn>
          <v-btn color="primary" @click="editTeacher">
            Save Changes
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
    <!-- Delete Confirmation Dialog -->
    <v-dialog v-model="showDeleteDialog" max-width="400px">
      <v-card v-if="currentTeacher">
        <v-card-title class="text-h5">
          Confirm Deletion
        </v-card-title>
        
        <v-card-text>
          Are you sure you want to delete teacher <strong>{{ currentTeacher.name }}</strong> (ID: {{ currentTeacher.id }})?
          This action cannot be undone.
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="secondary" @click="showDeleteDialog = false">
            Cancel
          </v-btn>
          <v-btn color="error" @click="deleteTeacher">
            Delete
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>