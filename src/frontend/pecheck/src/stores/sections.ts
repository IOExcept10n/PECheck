import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface Section {
  id: string;
  name: string;
  description: string;
  coverImage: string;
  schedule: {
    day: string;
    startTime: string;
    endTime: string;
    location: string;
  }[];
  reviews: {
    id: string;
    studentId: string;
    studentName: string;
    rating: number;
    comment: string;
    date: string;
  }[];
}

export const useSectionsStore = defineStore('sections', () => {
  const sections = ref<Section[]>([
    {
      id: 'SEC001',
      name: 'Swimming',
      description: 'Swimming lessons for beginners and advanced students',
      coverImage: 'https://picsum.photos/id/1083/800/400',
      schedule: [
        {
          day: 'Monday',
          startTime: '15:00',
          endTime: '16:30',
          location: 'Swimming Pool A'
        },
        {
          day: 'Wednesday',
          startTime: '15:00',
          endTime: '16:30',
          location: 'Swimming Pool A'
        }
      ],
      reviews: [
        {
          id: 'REV001',
          studentId: 'S12345',
          studentName: 'John Doe',
          rating: 4,
          comment: 'Great lessons, very helpful instructor!',
          date: '2023-08-15'
        }
      ]
    },
    {
      id: 'SEC002',
      name: 'Basketball',
      description: 'Basketball training for all skill levels',
      coverImage: 'https://picsum.photos/id/1058/800/400',
      schedule: [
        {
          day: 'Tuesday',
          startTime: '16:00',
          endTime: '17:30',
          location: 'Gym B'
        },
        {
          day: 'Thursday',
          startTime: '16:00',
          endTime: '17:30',
          location: 'Gym B'
        }
      ],
      reviews: [
        {
          id: 'REV002',
          studentId: 'S67890',
          studentName: 'Jane Smith',
          rating: 5,
          comment: 'Excellent coaching and fun atmosphere!',
          date: '2023-09-05'
        }
      ]
    }
  ])

  function addSection(section: Section) {
    sections.value.push(section)
  }

  function updateSection(id: string, updates: Partial<Section>) {
    const index = sections.value.findIndex(s => s.id === id)
    if (index !== -1) {
      sections.value[index] = { ...sections.value[index], ...updates }
    }
  }

  function removeSection(id: string) {
    const index = sections.value.findIndex(s => s.id === id)
    if (index !== -1) {
      sections.value.splice(index, 1)
    }
  }

  function getSectionById(id: string): Section | undefined {
    return sections.value.find(s => s.id === id)
  }

  function updateSectionSchedule(id: string, schedule: Section['schedule']) {
    const index = sections.value.findIndex(s => s.id === id)
    if (index !== -1) {
      sections.value[index].schedule = schedule
    }
  }

  function removeReview(sectionId: string, reviewId: string) {
    const sectionIndex = sections.value.findIndex(s => s.id === sectionId)
    if (sectionIndex !== -1) {
      const reviewIndex = sections.value[sectionIndex].reviews.findIndex(r => r.id === reviewId)
      if (reviewIndex !== -1) {
        sections.value[sectionIndex].reviews.splice(reviewIndex, 1)
      }
    }
  }

  return {
    sections,
    addSection,
    updateSection,
    removeSection,
    getSectionById,
    updateSectionSchedule,
    removeReview
  }
})