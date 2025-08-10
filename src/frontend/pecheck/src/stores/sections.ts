import { defineStore } from 'pinia'
<<<<<<< Updated upstream
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
=======
import { ref, computed } from 'vue'
import { sectionsApi } from '@/api/sections'

export interface Section {
  id: string
  name: string
  description: string
  coverImage?: string
  price: number
  schedule: LessonSchedule[]
  teacherId: string
  teacherName: string
  maxStudents?: number
  currentStudents?: number
}

export interface LessonSchedule {
  id: string
  dayOfWeek: number // 0-6 (Sunday-Saturday)
  startTime: string
  endTime: string
  room?: string
}

export interface SectionFilters {
  search?: string
  priceMin?: number
  priceMax?: number
  dayOfWeek?: number
  teacherId?: string
}

export const useSectionsStore = defineStore('sections', () => {
  const sections = ref<Section[]>([])
  const currentSection = ref<Section | null>(null)
  const isLoading = ref(false)
  const filters = ref<SectionFilters>({})

  const filteredSections = computed(() => {
    let filtered = sections.value

    if (filters.value.search) {
      const search = filters.value.search.toLowerCase()
      filtered = filtered.filter(section => 
        section.name.toLowerCase().includes(search) ||
        section.description.toLowerCase().includes(search) ||
        section.teacherName.toLowerCase().includes(search)
      )
    }

    if (filters.value.priceMin !== undefined) {
      filtered = filtered.filter(section => section.price >= filters.value.priceMin!)
    }

    if (filters.value.priceMax !== undefined) {
      filtered = filtered.filter(section => section.price <= filters.value.priceMax!)
    }

    if (filters.value.dayOfWeek !== undefined) {
      filtered = filtered.filter(section => 
        section.schedule.some(lesson => lesson.dayOfWeek === filters.value.dayOfWeek)
      )
    }

    if (filters.value.teacherId) {
      filtered = filtered.filter(section => section.teacherId === filters.value.teacherId)
    }

    return filtered
  })

  const fetchSections = async () => {
    try {
      isLoading.value = true
      const data = await sectionsApi.getSections()
      sections.value = data
    } catch (error) {
      throw error
    } finally {
      isLoading.value = false
    }
  }

  const fetchSectionById = async (id: string) => {
    try {
      isLoading.value = true
      const data = await sectionsApi.getSectionById(id)
      currentSection.value = data
      return data
    } catch (error) {
      throw error
    } finally {
      isLoading.value = false
    }
  }

  const createSection = async (sectionData: Omit<Section, 'id'>) => {
    try {
      isLoading.value = true
      const newSection = await sectionsApi.createSection(sectionData)
      sections.value.push(newSection)
      return newSection
    } catch (error) {
      throw error
    } finally {
      isLoading.value = false
    }
  }

  const updateSection = async (id: string, sectionData: Partial<Section>) => {
    try {
      isLoading.value = true
      const updatedSection = await sectionsApi.updateSection(id, sectionData)
      
      const index = sections.value.findIndex(s => s.id === id)
      if (index !== -1) {
        sections.value[index] = updatedSection
      }
      
      if (currentSection.value?.id === id) {
        currentSection.value = updatedSection
      }
      
      return updatedSection
    } catch (error) {
      throw error
    } finally {
      isLoading.value = false
    }
  }

  const deleteSection = async (id: string) => {
    try {
      isLoading.value = true
      await sectionsApi.deleteSection(id)
      
      sections.value = sections.value.filter(s => s.id !== id)
      
      if (currentSection.value?.id === id) {
        currentSection.value = null
      }
    } catch (error) {
      throw error
    } finally {
      isLoading.value = false
    }
  }

  const setFilters = (newFilters: SectionFilters) => {
    filters.value = { ...filters.value, ...newFilters }
  }

  const clearFilters = () => {
    filters.value = {}
  }

  return {
    sections,
    currentSection,
    isLoading,
    filters,
    filteredSections,
    fetchSections,
    fetchSectionById,
    createSection,
    updateSection,
    deleteSection,
    setFilters,
    clearFilters
  }
}) 
>>>>>>> Stashed changes
