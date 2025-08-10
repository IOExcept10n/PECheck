import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface Student {
  id: string;
  name: string;
  group: string;
  section: string;
  normativeResults: {
    [key: string]: number;
  };
  attendances: {
    date: string;
    present: boolean;
  }[];
}

export const useStudentsStore = defineStore('students', () => {
  const students = ref<Student[]>([
    {
      id: 'S12345',
      name: 'John Doe',
      group: 'Group A',
      section: 'Swimming',
      normativeResults: {
        'Push-ups': 25,
        'Running (1km)': 240,
        'Pull-ups': 10
      },
      attendances: [
        { date: '2023-09-01', present: true },
        { date: '2023-09-08', present: false },
        { date: '2023-09-15', present: true }
      ]
    },
    {
      id: 'S67890',
      name: 'Jane Smith',
      group: 'Group B',
      section: 'Basketball',
      normativeResults: {
        'Push-ups': 20,
        'Running (1km)': 300,
        'Pull-ups': 5
      },
      attendances: [
        { date: '2023-09-02', present: true },
        { date: '2023-09-09', present: true },
        { date: '2023-09-16', present: true }
      ]
    }
  ])

  function addStudent(student: Student) {
    students.value.push(student)
  }

  function updateStudent(id: string, updates: Partial<Student>) {
    const index = students.value.findIndex(s => s.id === id)
    if (index !== -1) {
      students.value[index] = { ...students.value[index], ...updates }
    }
  }

  function removeStudent(id: string) {
    const index = students.value.findIndex(s => s.id === id)
    if (index !== -1) {
      students.value.splice(index, 1)
    }
  }

  function getStudentById(id: string): Student | undefined {
    return students.value.find(s => s.id === id)
  }

  return {
    students,
    addStudent,
    updateStudent,
    removeStudent,
    getStudentById
  }
})