import apiClient from './client'
import type { Section } from '@/stores/sections'

export const sectionsApi = {
  async getSections(): Promise<Section[]> {
    const response = await apiClient.get('/sections')
    return response.data
  },

  async getSectionById(id: string): Promise<Section> {
    const response = await apiClient.get(`/sections/${id}`)
    return response.data
  },

  async createSection(sectionData: Omit<Section, 'id'>): Promise<Section> {
    const response = await apiClient.post('/sections', sectionData)
    return response.data
  },

  async updateSection(id: string, sectionData: Partial<Section>): Promise<Section> {
    const response = await apiClient.put(`/sections/${id}`, sectionData)
    return response.data
  },

  async deleteSection(id: string): Promise<void> {
    await apiClient.delete(`/sections/${id}`)
  },

  async uploadCoverImage(id: string, file: File): Promise<{ coverImage: string }> {
    const formData = new FormData()
    formData.append('coverImage', file)
    
    const response = await apiClient.post(`/sections/${id}/cover`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    })
    return response.data
  }
} 