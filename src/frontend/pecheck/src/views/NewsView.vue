<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const auth = useAuthStore()
const router = useRouter()

// Check if user is authenticated, if not redirect to login
onMounted(() => {
  if (!auth.isAuthenticated && !auth.checkSavedAuth()) {
    router.push('/login')
  }
})

// Mock news data
const news = ref([
  {
    id: 1,
    title: 'New Swimming Pool Schedule',
    date: '2023-09-15',
    content: 'The swimming pool schedule has been updated for the fall semester. Please check the calendar for new timings.',
    image: 'https://picsum.photos/id/1083/800/400',
    author: 'Admin'
  },
  {
    id: 2,
    title: 'Basketball Tournament Announcement',
    date: '2023-09-10',
    content: 'We are excited to announce the upcoming basketball tournament starting next month. Registration is now open for all students.',
    image: 'https://picsum.photos/id/1058/800/400',
    author: 'Coach Bob'
  },
  {
    id: 3,
    title: 'Fitness Challenge Results',
    date: '2023-09-05',
    content: 'Congratulations to all participants in the summer fitness challenge! The results are now available on the bulletin board.',
    image: 'https://picsum.photos/id/684/800/400',
    author: 'PE Department'
  },
  {
    id: 4,
    title: 'New Yoga Classes',
    date: '2023-09-01',
    content: 'We are introducing new yoga classes starting next week. Classes will be held every Tuesday and Thursday morning.',
    image: 'https://picsum.photos/id/669/800/400',
    author: 'Admin'
  }
])
</script>

<template>
  <v-container v-if="auth.isAuthenticated">
    <v-card class="mb-4">
      <v-card-title class="text-h4">
        News & Announcements
      </v-card-title>
    </v-card>
    
    <v-row>
      <v-col v-for="item in news" :key="item.id" cols="12" md="6">
        <v-card class="mb-4">
          <v-img
            :src="item.image"
            height="200"
            cover
          ></v-img>
          
          <v-card-title>
            {{ item.title }}
          </v-card-title>
          
          <v-card-subtitle>
            {{ new Date(item.date).toLocaleDateString() }} by {{ item.author }}
          </v-card-subtitle>
          
          <v-card-text>
            <p>{{ item.content }}</p>
          </v-card-text>
          
          <v-card-actions>
            <v-btn color="primary" variant="text">
              Read More
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
    
    <v-card class="text-center pa-4" v-if="auth.user?.role === 'admin'">
      <v-btn color="primary" size="large">
        <v-icon left>mdi-plus</v-icon>
        Add News
      </v-btn>
    </v-card>
  </v-container>
</template>