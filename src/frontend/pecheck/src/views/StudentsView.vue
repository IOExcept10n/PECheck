<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { useStudentsStore, type Student } from '../stores/students'
import QRCode from 'qrcode.vue'

const auth = useAuthStore()
const studentsStore = useStudentsStore()
const router = useRouter()

// Form dialogs
const showAddDialog = ref(false)
const showEditDialog = ref(false)
const showDeleteDialog = ref(false)
const showQRDialog = ref(false)

// Current student being edited/deleted
const currentStudent = ref<Student | null>(null)

// New student form
const newStudent = ref({
  id: '',
  name: '',
  group: '',
  section: '',
  normativeResults: {},
  attendances: []
})

// Search
const searchQuery = ref('')

// Normative results form
const normativeKey = ref('')
const normativeValue = ref(0)

// Filtered students based on search
const filteredStudents = computed(() => {
  if (!searchQuery.value) return studentsStore.students
  
  const query = searchQuery.value.toLowerCase()
  return studentsStore.students.filter(student => 
    student.id.toLowerCase().includes(query) ||
    student.name.toLowerCase().includes(query) ||
    student.group.toLowerCase().includes(query) ||
    student.section.toLowerCase().includes(query)
  )
})

// Check if user is authenticated and is admin, if not redirect to login
onMounted(() => {
  if (!auth.isAuthenticated && !auth.checkSavedAuth()) {
    router.push('/login')
  } else if (auth.user?.role !== 'admin') {
    router.push('/')
  }
})

// Add a new student
function addStudent() {
  // Create a new student ID if not provided
  if (!newStudent.value.id) {
    newStudent.value.id = 'S' + Math.floor(10000 + Math.random() * 90000)
  }
  
  studentsStore.addStudent({
    ...newStudent.value,
    normativeResults: { ...newStudent.value.normativeResults },
    attendances: [...newStudent.value.attendances]
  })
  
  // Reset form
  newStudent.value = {
    id: '',
    name: '',
    group: '',
    section: '',
    normativeResults: {},
    attendances: []
  }
  
  showAddDialog.value = false
}

// Edit student
function editStudent() {
  if (currentStudent.value) {
    studentsStore.updateStudent(currentStudent.value.id, currentStudent.value)
    showEditDialog.value = false
  }
}

// Delete student
function deleteStudent() {
  if (currentStudent.value) {
    studentsStore.removeStudent(currentStudent.value.id)
    showDeleteDialog.value = false
  }
}

// Add normative result to new student
function addNormativeToNew() {
  if (normativeKey.value && normativeValue.value) {
    newStudent.value.normativeResults = {
      ...newStudent.value.normativeResults,
      [normativeKey.value]: normativeValue.value
    }
    
    normativeKey.value = ''
    normativeValue.value = 0
  }
}

// Add normative result to current student
function addNormativeToCurrent() {
  if (normativeKey.value && normativeValue.value && currentStudent.value) {
    currentStudent.value.normativeResults = {
      ...currentStudent.value.normativeResults,
      [normativeKey.value]: normativeValue.value
    }
    
    normativeKey.value = ''
    normativeValue.value = 0
  }
}

// Remove normative from new student
function removeNormativeFromNew(key: string) {
  const { [key]: _, ...rest } = newStudent.value.normativeResults
  newStudent.value.normativeResults = rest
}

// Remove normative from current student
function removeNormativeFromCurrent(key: string) {
  if (currentStudent.value) {
    const { [key]: _, ...rest } = currentStudent.value.normativeResults
    currentStudent.value.normativeResults = rest
  }
}

// Add attendance to new student
function addAttendanceToNew(present: boolean) {
  newStudent.value.attendances.push({
    date: new Date().toISOString().slice(0, 10),
    present
  })
}

// Add attendance to current student
function addAttendanceToCurrent(present: boolean) {
  if (currentStudent.value) {
    currentStudent.value.attendances.push({
      date: new Date().toISOString().slice(0, 10),
      present
    })
  }
}

// Show QR code for student
function showStudentQR(student: Student) {
  currentStudent.value = student
  showQRDialog.value = true
}
</script>

<template>
  <v-container v-if="auth.isAuthenticated && auth.user?.role === 'admin'">
    <v-card class="mb-4">
      <v-card-title class="d-flex align-center">
        <span class="text-h4">Students Management</span>
        <v-spacer></v-spacer>
        <v-btn color="primary" @click="showAddDialog = true">
          <v-icon left>mdi-plus</v-icon>
          Add Student
        </v-btn>
      </v-card-title>
      
      <v-card-text>
        <v-text-field
          v-model="searchQuery"
          label="Search Students"
          prepend-icon="mdi-magnify"
          clearable
        ></v-text-field>
        
        <v-data-table
          :headers="[
            { title: 'ID', key: 'id' },
            { title: 'Name', key: 'name' },
            { title: 'Group', key: 'group' },
            { title: 'Section', key: 'section' },
            { title: 'Actions', key: 'actions', sortable: false }
          ]"
          :items="filteredStudents"
        >
          <template v-slot:item.actions="{ item }">
            <v-btn icon small @click="showStudentQR(item)">
              <v-icon>mdi-qrcode</v-icon>
            </v-btn>
            <v-btn icon small @click="currentStudent = {...item}; showEditDialog = true">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn icon small @click="currentStudent = item; showDeleteDialog = true">
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>
    
    <!-- Add Student Dialog -->
    <v-dialog v-model="showAddDialog" max-width="600px">
      <v-card>
        <v-card-title>
          Add New Student
          <v-spacer></v-spacer>
          <v-btn icon @click="showAddDialog = false">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        
        <v-card-text>
          <v-form @submit.prevent="addStudent">
            <v-text-field
              v-model="newStudent.id"
              label="Student ID (optional, will be generated if empty)"
              hint="Format: Sxxxxx"
              persistent-hint
            ></v-text-field>
            
            <v-text-field
              v-model="newStudent.name"
              label="Full Name"
              required
            ></v-text-field>
            
            <v-text-field
              v-model="newStudent.group"
              label="Group"
              required
            ></v-text-field>
            
            <v-text-field
              v-model="newStudent.section"
              label="Section"
              required
            ></v-text-field>
            
            <v-divider class="my-4"></v-divider>
            
            <h3 class="text-h6 mb-2">Normative Results</h3>
            
            <v-row>
              <v-col cols="6">
                <v-text-field
                  v-model="normativeKey"
                  label="Test Name"
                ></v-text-field>
              </v-col>
              
              <v-col cols="4">
                <v-text-field
                  v-model.number="normativeValue"
                  label="Result"
                  type="number"
                ></v-text-field>
              </v-col>
              
              <v-col cols="2">
                <v-btn icon @click="addNormativeToNew" :disabled="!normativeKey || !normativeValue">
                  <v-icon>mdi-plus</v-icon>
                </v-btn>
              </v-col>
            </v-row>
            
            <v-list v-if="Object.keys(newStudent.normativeResults).length > 0">
              <v-list-item v-for="(value, key) in newStudent.normativeResults" :key="key">
                <v-list-item-title>{{ key }}: {{ value }}</v-list-item-title>
                <v-list-item-action>
                  <v-btn icon small @click="removeNormativeFromNew(key)">
                    <v-icon>mdi-delete</v-icon>
                  </v-btn>
                </v-list-item-action>
              </v-list-item>
            </v-list>
            
            <v-divider class="my-4"></v-divider>
            
            <h3 class="text-h6 mb-2">Attendance</h3>
            
            <v-row class="mb-2">
              <v-col cols="12" class="d-flex justify-end">
                <v-btn color="success" class="mr-2" @click="addAttendanceToNew(true)">
                  <v-icon left>mdi-check</v-icon>
                  Add Present
                </v-btn>
                
                <v-btn color="error" @click="addAttendanceToNew(false)">
                  <v-icon left>mdi-close</v-icon>
                  Add Absent
                </v-btn>
              </v-col>
            </v-row>
            
            <v-list v-if="newStudent.attendances.length > 0">
              <v-list-item v-for="(attendance, index) in newStudent.attendances" :key="index">
                <v-list-item-title>
                  {{ new Date(attendance.date).toLocaleDateString() }}:
                  <v-chip
                    :color="attendance.present ? 'success' : 'error'"
                    size="small"
                    class="ml-2"
                  >
                    {{ attendance.present ? 'Present' : 'Absent' }}
                  </v-chip>
                </v-list-item-title>
              </v-list-item>
            </v-list>
          </v-form>
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="secondary" @click="showAddDialog = false">
            Cancel
          </v-btn>
          <v-btn color="primary" @click="addStudent">
            Add Student
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
    <!-- Edit Student Dialog -->
    <v-dialog v-model="showEditDialog" max-width="600px">
      <v-card v-if="currentStudent">
        <v-card-title>
          Edit Student: {{ currentStudent.name }}
          <v-spacer></v-spacer>
          <v-btn icon @click="showEditDialog = false">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        
        <v-card-text>
          <v-form @submit.prevent="editStudent">
            <v-text-field
              v-model="currentStudent.id"
              label="Student ID"
              disabled
            ></v-text-field>
            
            <v-text-field
              v-model="currentStudent.name"
              label="Full Name"
              required
            ></v-text-field>
            
            <v-text-field
              v-model="currentStudent.group"
              label="Group"
              required
            ></v-text-field>
            
            <v-text-field
              v-model="currentStudent.section"
              label="Section"
              required
            ></v-text-field>
            
            <v-divider class="my-4"></v-divider>
            
            <h3 class="text-h6 mb-2">Normative Results</h3>
            
            <v-row>
              <v-col cols="6">
                <v-text-field
                  v-model="normativeKey"
                  label="Test Name"
                ></v-text-field>
              </v-col>
              
              <v-col cols="4">
                <v-text-field
                  v-model.number="normativeValue"
                  label="Result"
                  type="number"
                ></v-text-field>
              </v-col>
              
              <v-col cols="2">
                <v-btn icon @click="addNormativeToCurrent" :disabled="!normativeKey || !normativeValue">
                  <v-icon>mdi-plus</v-icon>
                </v-btn>
              </v-col>
            </v-row>
            
            <v-list v-if="Object.keys(currentStudent.normativeResults).length > 0">
              <v-list-item v-for="(value, key) in currentStudent.normativeResults" :key="key">
                <v-list-item-title>{{ key }}: {{ value }}</v-list-item-title>
                <v-list-item-action>
                  <v-btn icon small @click="removeNormativeFromCurrent(key)">
                    <v-icon>mdi-delete</v-icon>
                  </v-btn>
                </v-list-item-action>
              </v-list-item>
            </v-list>
            
            <v-divider class="my-4"></v-divider>
            
            <h3 class="text-h6 mb-2">Attendance</h3>
            
            <v-row class="mb-2">
              <v-col cols="12" class="d-flex justify-end">
                <v-btn color="success" class="mr-2" @click="addAttendanceToCurrent(true)">
                  <v-icon left>mdi-check</v-icon>
                  Add Present
                </v-btn>
                
                <v-btn color="error" @click="addAttendanceToCurrent(false)">
                  <v-icon left>mdi-close</v-icon>
                  Add Absent
                </v-btn>
              </v-col>
            </v-row>
            
            <v-list v-if="currentStudent.attendances.length > 0">
              <v-list-item v-for="(attendance, index) in currentStudent.attendances" :key="index">
                <v-list-item-title>
                  {{ new Date(attendance.date).toLocaleDateString() }}:
                  <v-chip
                    :color="attendance.present ? 'success' : 'error'"
                    size="small"
                    class="ml-2"
                  >
                    {{ attendance.present ? 'Present' : 'Absent' }}
                  </v-chip>
                </v-list-item-title>
              </v-list-item>
            </v-list>
          </v-form>
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="secondary" @click="showEditDialog = false">
            Cancel
          </v-btn>
          <v-btn color="primary" @click="editStudent">
            Save Changes
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
    <!-- Delete Confirmation Dialog -->
    <v-dialog v-model="showDeleteDialog" max-width="400px">
      <v-card v-if="currentStudent">
        <v-card-title class="text-h5">
          Confirm Deletion
        </v-card-title>
        
        <v-card-text>
          Are you sure you want to delete student <strong>{{ currentStudent.name }}</strong> (ID: {{ currentStudent.id }})?
          This action cannot be undone.
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="secondary" @click="showDeleteDialog = false">
            Cancel
          </v-btn>
          <v-btn color="error" @click="deleteStudent">
            Delete
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
    <!-- QR Code Dialog -->
    <v-dialog v-model="showQRDialog" max-width="400px">
      <v-card v-if="currentStudent">
        <v-card-title class="text-h5">
          Student QR Code
        </v-card-title>
        
        <v-card-text class="text-center">
          <p>{{ currentStudent.name }} (ID: {{ currentStudent.id }})</p>
          <div class="d-flex justify-center my-4">
            <QRCode :value="currentStudent.id" :size="200" level="H" />
          </div>
          <p class="text-caption">Scan this code to identify the student</p>
        </v-card-text>
        
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="primary" @click="showQRDialog = false">
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>