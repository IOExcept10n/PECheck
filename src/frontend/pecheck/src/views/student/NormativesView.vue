<template>
  <div class="normatives-container">
    <h2>Physical Education Normatives</h2>
    
    <div class="normatives-grid">
      <div
        v-for="normative in normatives"
        :key="normative.id"
        class="normative-card card"
      >
        <div class="normative-header">
          <h3>{{ normative.name }}</h3>
          <span class="normative-type">{{ normative.type }}</span>
        </div>
        
        <div class="normative-content">
          <p>{{ normative.description }}</p>
          
          <div class="normative-results">
            <h4>Your Results</h4>
            <div v-if="normative.results" class="result-item">
              <span class="result-label">{{ normative.unit }}</span>
              <span class="result-value">{{ normative.results }}</span>
            </div>
            <div v-else class="no-result">
              No results yet
            </div>
          </div>
          
          <div class="normative-actions">
            <button
              v-if="normative.canRegister"
              class="btn btn-success"
              @click="registerForNormative(normative.id)"
            >
              Register
            </button>
            <button
              v-else
              class="btn btn-secondary"
              disabled
            >
              Registration Closed
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

interface Normative {
  id: string
  name: string
  type: string
  description: string
  unit: string
  results?: string
  canRegister: boolean
}

const normatives = ref<Normative[]>([
  {
    id: '1',
    name: '1km Run',
    type: 'Endurance',
    description: 'Run 1 kilometer as fast as possible',
    unit: 'Time',
    results: '4:32',
    canRegister: false
  },
  {
    id: '2',
    name: 'Push-ups',
    type: 'Strength',
    description: 'Maximum number of push-ups in 1 minute',
    unit: 'Count',
    results: '25',
    canRegister: false
  },
  {
    id: '3',
    name: 'Long Jump',
    type: 'Power',
    description: 'Standing long jump from a stationary position',
    unit: 'Distance (cm)',
    canRegister: true
  },
  {
    id: '4',
    name: 'Sit-ups',
    type: 'Strength',
    description: 'Maximum number of sit-ups in 1 minute',
    unit: 'Count',
    canRegister: true
  }
])

const registerForNormative = (normativeId: string) => {
  // This would call an API to register the student
  console.log('Registering for normative:', normativeId)
}
</script>

<style scoped>
.normatives-container {
  max-width: 1200px;
  margin: 0 auto;
}

.normatives-container h2 {
  margin-bottom: 2rem;
}

.normatives-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 2rem;
}

.normative-card {
  transition: var(--transition);
}

.normative-card:hover {
  transform: translateY(-2px);
  box-shadow: var(--shadow-medium);
}

.normative-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.normative-header h3 {
  margin: 0;
  color: var(--text-primary);
}

.normative-type {
  background: var(--primary-color);
  color: white;
  padding: 0.25rem 0.75rem;
  border-radius: var(--border-radius-small);
  font-size: 0.875rem;
  font-weight: 500;
}

.normative-content {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.normative-content p {
  color: var(--text-secondary);
  line-height: 1.5;
  margin: 0;
}

.normative-results h4 {
  margin: 0 0 0.5rem 0;
  color: var(--text-primary);
  font-size: 1rem;
}

.result-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem;
  background: var(--secondary-color);
  border-radius: var(--border-radius-small);
}

.result-label {
  color: var(--text-secondary);
  font-weight: 500;
}

.result-value {
  color: var(--primary-color);
  font-weight: 600;
  font-size: 1.125rem;
}

.no-result {
  text-align: center;
  padding: 1rem;
  color: var(--text-light);
  background: var(--secondary-color);
  border-radius: var(--border-radius-small);
}

.normative-actions {
  margin-top: auto;
}

@media (max-width: 768px) {
  .normatives-grid {
    grid-template-columns: 1fr;
  }
}
</style> 