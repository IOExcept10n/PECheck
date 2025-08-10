import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface Teacher {
  id: string;
  name: string;
  sections: string[];
  email: string;
  phone: string;
}

export const useTeachersStore = defineStore('teachers', () => {
  const teachers = ref<Teacher[]>([
    {
      id: 'T12345',
      name: 'Jane Teacher',
      sections: ['Swimming', 'Running'],
      email: 'jane.teacher@example.com',
      phone: '+1234567890'
    },
    {
      id: 'T67890',
      name: 'Bob Coach',
      sections: ['Basketball', 'Football'],
      email: 'bob.coach@example.com',
      phone: '+0987654321'
    }
  ])

  function addTeacher(teacher: Teacher) {
    teachers.value.push(teacher)
  }

  function updateTeacher(id: string, updates: Partial<Teacher>) {
    const index = teachers.value.findIndex(t => t.id === id)
    if (index !== -1) {
      teachers.value[index] = { ...teachers.value[index], ...updates }
    }
  }

  function removeTeacher(id: string) {
    const index = teachers.value.findIndex(t => t.id === id)
    if (index !== -1) {
      teachers.value.splice(index, 1)
    }
  }

  function getTeacherById(id: string): Teacher | undefined {
    return teachers.value.find(t => t.id === id)
  }

  return {
    teachers,
    addTeacher,
    updateTeacher,
    removeTeacher,
    getTeacherById
  }
})