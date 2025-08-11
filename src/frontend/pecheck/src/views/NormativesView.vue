<script setup lang="ts">
import { ref, computed } from 'vue'
import { useMessage, NDatePicker } from 'naive-ui'
import type { DateValue } from 'naive-ui'

const message = useMessage()
const selectedDate = ref<DateValue>(Date.now())

interface Normative {
  id: number
  name: string
  description: string
  targetValue: number
  unit: string
  currentValue?: number
  date?: string
}

// Mock data
const normatives: Normative[] = [
  {
    id: 1,
    name: 'Бег 100м',
    description: 'Проверка скоростных качеств',
    targetValue: 13.5,
    unit: 'сек',
    currentValue: 14.2,
    date: '2025-08-05'
  },
  {
    id: 2,
    name: 'Подтягивания',
    description: 'Проверка силы верхнего плечевого пояса',
    targetValue: 12,
    unit: 'раз',
    currentValue: 10,
    date: '2025-08-05'
  },
  {
    id: 3,
    name: 'Прыжок в длину',
    description: 'Проверка взрывной силы ног',
    targetValue: 230,
    unit: 'см',
    currentValue: 215,
    date: '2025-08-05'
  },
  {
    id: 4,
    name: 'Отжимания',
    description: 'Проверка силы верхнего плечевого пояса',
    targetValue: 40,
    unit: 'раз',
    currentValue: 35,
    date: '2025-08-05'
  }
]

// Обработчик выбора даты
function handleDateSelect(timestamp: DateValue) {
  if (timestamp) {
    const date = new Date(timestamp)
    message.info('Выбрана дата: ' + date.toLocaleDateString())
  }
}

// Вычисляем общий прогресс по всем нормативам
const totalProgress = computed(() => {
  const total = normatives.length
  let completed = 0

  normatives.forEach(norm => {
    if (norm.currentValue !== undefined) {
      const progress = (norm.currentValue / norm.targetValue) * 100
      if (progress >= 100) completed++
    }
  })

  return Math.round((completed / total) * 100)
})

// Функция для вычисления процента выполнения норматива
function calculateProgress(current: number | undefined, target: number): number {
  if (current === undefined) return 0
  const progress = (current / target) * 100
  return Math.min(progress, 100)
}
</script>

<template>
  <div class="normatives-page">
    <!-- Карточка с общим прогрессом -->
    <div class="progress-card card">
      <div class="progress-header">
        <h2>Общий прогресс</h2>
        <div class="progress-circle">
          <svg viewBox="0 0 36 36" class="circular-chart">
            <path d="M18 2.0845
              a 15.9155 15.9155 0 0 1 0 31.831
              a 15.9155 15.9155 0 0 1 0 -31.831"
              class="circle-bg"
            />
            <path d="M18 2.0845
              a 15.9155 15.9155 0 0 1 0 31.831
              a 15.9155 15.9155 0 0 1 0 -31.831"
              class="circle"
              :stroke-dasharray="`${totalProgress}, 100`"
            />
            <text x="18" y="20.35" class="percentage">{{ totalProgress }}%</text>
          </svg>
        </div>
      </div>
    </div>

    <!-- Календарь -->
    <div class="calendar-card card">
      <h2>Выбор даты сдачи</h2>
      <div class="calendar-wrapper">
        <n-date-picker
          v-model:value="selectedDate"
          @update:value="handleDateSelect"
          type="date"
          :clearable="false"
          style="width: 100%"
        />
      </div>
    </div>

    <!-- Список нормативов -->
    <div class="normatives-grid">
      <div v-for="normative in normatives" :key="normative.id" class="normative-card card">
        <h3>{{ normative.name }}</h3>
        <p class="description">{{ normative.description }}</p>

        <div class="progress-bar">
          <div
            class="progress-fill"
            :style="{ width: `${calculateProgress(normative.currentValue, normative.targetValue)}%` }"
          ></div>
        </div>

        <div class="stats">
          <div class="current">
            <span class="label">Текущий:</span>
            <span class="value">{{ normative.currentValue || '—' }} {{ normative.unit }}</span>
          </div>
          <div class="target">
            <span class="label">Цель:</span>
            <span class="value">{{ normative.targetValue }} {{ normative.unit }}</span>
          </div>
          <div class="date" v-if="normative.date">
            <span class="label">Дата:</span>
            <span class="value">{{ new Date(normative.date).toLocaleDateString() }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.normatives-page {
  padding: 2rem 1rem;
  max-width: 1200px;
  margin: 0 auto;
  display: grid;
  gap: 2rem;
  grid-template-columns: 2fr 1fr;
  grid-template-areas:
    "progress calendar"
    "normatives normatives";
}

.progress-card {
  grid-area: progress;
  padding: 2rem;
}

.calendar-card {
  grid-area: calendar;
  padding: 2rem;
}

.normatives-grid {
  grid-area: normatives;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
}

.progress-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.progress-header h2 {
  margin: 0;
  color: var(--text-color);
}

.progress-circle {
  width: 150px;
  height: 150px;
}

.circular-chart {
  display: block;
  margin: 0 auto;
  max-width: 100%;
}

.circle-bg {
  fill: none;
  stroke: var(--border-color);
  stroke-width: 3.8;
}

.circle {
  fill: none;
  stroke: var(--primary-color);
  stroke-width: 3.8;
  stroke-linecap: round;
  animation: progress 1s ease-out forwards;
}

.percentage {
  fill: var(--text-color);
  font-size: 0.5em;
  text-anchor: middle;
  font-weight: bold;
}

.calendar-wrapper {
  margin-top: 1rem;
}

.normative-card {
  padding: 1.5rem;
}

.normative-card h3 {
  margin: 0 0 0.5rem;
  color: var(--text-color);
}

.description {
  color: var(--text-muted);
  font-size: 0.875rem;
  margin: 0 0 1rem;
}

.progress-bar {
  height: 8px;
  background: var(--border-color);
  border-radius: 4px;
  margin-bottom: 1rem;
  overflow: hidden;
}

.progress-fill {
  height: 100%;
  background: var(--primary-color);
  border-radius: 4px;
  transition: width 0.6s ease;
}

.stats {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
  gap: 1rem;
}

.stats > div {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.label {
  color: var(--text-muted);
  font-size: 0.75rem;
}

.value {
  color: var(--text-color);
  font-weight: 500;
}

@keyframes progress {
  0% {
    stroke-dasharray: 0 100;
  }
}

/* Dark mode */
:global(.dark-mode) .circle-bg {
  stroke: var(--border-dark);
}

:global(.dark-mode) .percentage {
  fill: var(--text-dark);
}

:global(.dark-mode) .normative-card h3 {
  color: var(--text-dark);
}

:global(.dark-mode) .description {
  color: var(--text-muted-dark);
}

:global(.dark-mode) .progress-bar {
  background: var(--border-dark);
}

:global(.dark-mode) .label {
  color: var(--text-muted-dark);
}

:global(.dark-mode) .value {
  color: var(--text-dark);
}

/* Responsive */
@media (max-width: 768px) {
  .normatives-page {
    grid-template-columns: 1fr;
    grid-template-areas:
      "progress"
      "calendar"
      "normatives";
  }

  .progress-card,
  .calendar-card {
    padding: 1.5rem;
  }

  .progress-circle {
    width: 120px;
    height: 120px;
  }
}
</style>
