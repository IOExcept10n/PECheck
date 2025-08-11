<script setup lang="ts">
interface Schedule {
  dayOfWeek: string
  startTime: string
  endTime: string
  location: string
}

interface Section {
  id: number
  name: string
  description: string
  image: string
  price: number
  capacity: {
    current: number
    max: number
  }
  teacher: string
  schedule: Schedule[]
}

// Mock data
const sections: Section[] = [
  {
    id: 1,
    name: 'Баскетбол',
    description: 'Развитие навыков командной игры, техники броска и стратегического мышления. Подходит для всех уровней подготовки.',
    image: 'https://images.unsplash.com/photo-1546519638-68e109498ffc?w=800&h=400&q=80',
    price: 2000,
    capacity: { current: 15, max: 20 },
    teacher: 'Петров И.В.',
    schedule: [
      { dayOfWeek: 'Понедельник', startTime: '16:00', endTime: '17:30', location: 'Спортзал №1' },
      { dayOfWeek: 'Четверг', startTime: '16:00', endTime: '17:30', location: 'Спортзал №1' }
    ]
  },
  {
    id: 2,
    name: 'Волейбол',
    description: 'Тренировки направлены на развитие координации, командного взаимодействия и техники игры в волейбол.',
    image: 'https://images.unsplash.com/photo-1592656094267-764a45160876?w=800&h=400&q=80',
    price: 2000,
    capacity: { current: 12, max: 18 },
    teacher: 'Сидорова А.П.',
    schedule: [
      { dayOfWeek: 'Вторник', startTime: '18:00', endTime: '19:30', location: 'Спортзал №2' },
      { dayOfWeek: 'Пятница', startTime: '18:00', endTime: '19:30', location: 'Спортзал №2' }
    ]
  },
  {
    id: 3,
    name: 'Фитнес',
    description: 'Комплексные тренировки для поддержания формы, развития силы и выносливости.',
    image: 'https://images.unsplash.com/photo-1540497077202-7c8a3999166f?w=800&h=400&q=80',
    price: 2500,
    capacity: { current: 8, max: 15 },
    teacher: 'Иванова М.С.',
    schedule: [
      { dayOfWeek: 'Понедельник', startTime: '19:00', endTime: '20:30', location: 'Тренажерный зал' },
      { dayOfWeek: 'Среда', startTime: '19:00', endTime: '20:30', location: 'Тренажерный зал' }
    ]
  },
  {
    id: 4,
    name: 'Плавание',
    description: 'Обучение различным стилям плавания, тренировки на выносливость и технику.',
    image: 'https://images.unsplash.com/photo-1600965962361-9035dbfd1c50?w=800&h=400&q=80',
    price: 3000,
    capacity: { current: 10, max: 12 },
    teacher: 'Морозов Д.К.',
    schedule: [
      { dayOfWeek: 'Вторник', startTime: '17:00', endTime: '18:30', location: 'Бассейн' },
      { dayOfWeek: 'Четверг', startTime: '17:00', endTime: '18:30', location: 'Бассейн' }
    ]
  },
  {
    id: 5,
    name: 'Йога',
    description: 'Гармоничное развитие тела и духа через асаны, пранаяму и медитацию. Уменьшение стресса и улучшение гибкости.',
    image: 'https://images.unsplash.com/photo-1544367567-0f2fcb009e0b?w=800&h=400&q=80',
    price: 2800,
    capacity: { current: 14, max: 15 },
    teacher: 'Кузнецова Е.М.',
    schedule: [
      { dayOfWeek: 'Среда', startTime: '08:00', endTime: '09:30', location: 'Зал для йоги' },
      { dayOfWeek: 'Суббота', startTime: '10:00', endTime: '11:30', location: 'Зал для йоги' }
    ]
  },
  {
    id: 6,
    name: 'Бокс',
    description: 'Обучение технике бокса, развитие силы, скорости и выносливости. Спарринги и работа на снарядах.',
    image: 'https://images.unsplash.com/photo-1549719386-74dfcbf7dbed?w=800&h=400&q=80',
    price: 3500,
    capacity: { current: 8, max: 10 },
    teacher: 'Волков А.С.',
    schedule: [
      { dayOfWeek: 'Понедельник', startTime: '20:00', endTime: '21:30', location: 'Боксёрский зал' },
      { dayOfWeek: 'Четверг', startTime: '20:00', endTime: '21:30', location: 'Боксёрский зал' }
    ]
  },
  {
    id: 7,
    name: 'Настольный теннис',
    description: 'Тренировка реакции, координации и тактического мышления. Индивидуальные и парные игры.',
    image: 'https://images.unsplash.com/photo-1534158914592-062992fbe900?w=800&h=400&q=80',
    price: 1800,
    capacity: { current: 6, max: 8 },
    teacher: 'Соколов П.Р.',
    schedule: [
      { dayOfWeek: 'Вторник', startTime: '16:00', endTime: '17:30', location: 'Теннисный зал' },
      { dayOfWeek: 'Пятница', startTime: '16:00', endTime: '17:30', location: 'Теннисный зал' }
    ]
  },
  {
    id: 8,
    name: 'Скалолазание',
    description: 'Обучение технике скалолазания, развитие силы и гибкости. Тренировки на искусственном рельефе.',
    image: 'https://images.unsplash.com/photo-1522163182402-834f871fd851?w=800&h=400&q=80',
    price: 3000,
    capacity: { current: 5, max: 8 },
    teacher: 'Горбунов М.А.',
    schedule: [
      { dayOfWeek: 'Среда', startTime: '18:00', endTime: '19:30', location: 'Скалодром' },
      { dayOfWeek: 'Суббота', startTime: '12:00', endTime: '13:30', location: 'Скалодром' }
    ]
  },
  {
    id: 9,
    name: 'Кроссфит',
    description: 'Интенсивные функциональные тренировки для развития силы, выносливости и мощности.',
    image: 'https://images.unsplash.com/photo-1534367610401-9f5ed68180aa?w=800&h=400&q=80',
    price: 3500,
    capacity: { current: 10, max: 12 },
    teacher: 'Власов Д.И.',
    schedule: [
      { dayOfWeek: 'Понедельник', startTime: '17:00', endTime: '18:30', location: 'Кроссфит-зал' },
      { dayOfWeek: 'Среда', startTime: '17:00', endTime: '18:30', location: 'Кроссфит-зал' },
      { dayOfWeek: 'Пятница', startTime: '17:00', endTime: '18:30', location: 'Кроссфит-зал' }
    ]
  },
  {
    id: 10,
    name: 'Танцы',
    description: 'Современные танцевальные направления: хип-хоп, брейк-данс, джаз-фанк. Развитие пластики и чувства ритма.',
    image: 'https://images.unsplash.com/photo-1535525153412-5a42439a210d?w=800&h=400&q=80',
    price: 2500,
    capacity: { current: 15, max: 20 },
    teacher: 'Романова К.А.',
    schedule: [
      { dayOfWeek: 'Вторник', startTime: '19:00', endTime: '20:30', location: 'Танцевальный зал' },
      { dayOfWeek: 'Четверг', startTime: '19:00', endTime: '20:30', location: 'Танцевальный зал' }
    ]
  },
  {
    id: 11,
    name: 'Борьба',
    description: 'Обучение приёмам самбо и дзюдо. Развитие силы, ловкости и тактического мышления.',
    image: 'https://images.unsplash.com/photo-1555597673-b21d5c935865?w=800&h=400&q=80',
    price: 3000,
    capacity: { current: 12, max: 15 },
    teacher: 'Борисов Г.П.',
    schedule: [
      { dayOfWeek: 'Понедельник', startTime: '18:00', endTime: '19:30', location: 'Борцовский зал' },
      { dayOfWeek: 'Четверг', startTime: '18:00', endTime: '19:30', location: 'Борцовский зал' }
    ]
  },
  {
    id: 12,
    name: 'Бадминтон',
    description: 'Обучение технике игры в бадминтон. Тренировка реакции, координации и выносливости.',
    image: 'https://images.unsplash.com/photo-1613921304042-aa8f1da987a8?w=800&h=400&q=80',
    price: 1800,
    capacity: { current: 8, max: 12 },
    teacher: 'Федорова Н.В.',
    schedule: [
      { dayOfWeek: 'Среда', startTime: '16:00', endTime: '17:30', location: 'Спортзал №3' },
      { dayOfWeek: 'Суббота', startTime: '14:00', endTime: '15:30', location: 'Спортзал №3' }
    ]
  },
  {
    id: 13,
    name: 'Стретчинг',
    description: 'Комплекс упражнений на растяжку. Улучшение гибкости и подвижности суставов.',
    image: 'https://images.unsplash.com/photo-1601925260368-ae2f83cf9b3f?w=800&h=400&q=80',
    price: 2200,
    capacity: { current: 10, max: 15 },
    teacher: 'Андреева О.В.',
    schedule: [
      { dayOfWeek: 'Вторник', startTime: '09:00', endTime: '10:30', location: 'Зал для йоги' },
      { dayOfWeek: 'Пятница', startTime: '09:00', endTime: '10:30', location: 'Зал для йоги' }
    ]
  },
  {
    id: 14,
    name: 'Функциональный тренинг',
    description: 'Тренировки с собственным весом и оборудованием. Развитие всех групп мышц.',
    image: 'https://images.unsplash.com/photo-1517838277536-f5f99be501cd?w=800&h=400&q=80',
    price: 2800,
    capacity: { current: 12, max: 15 },
    teacher: 'Калинин С.М.',
    schedule: [
      { dayOfWeek: 'Понедельник', startTime: '15:00', endTime: '16:30', location: 'Тренажерный зал' },
      { dayOfWeek: 'Среда', startTime: '15:00', endTime: '16:30', location: 'Тренажерный зал' }
    ]
  },
  {
    id: 15,
    name: 'Пилатес',
    description: 'Система упражнений для укрепления мышц корпуса, улучшения осанки и координации.',
    image: 'https://images.unsplash.com/photo-1518611012118-696072aa579a?w=800&h=400&q=80',
    price: 2500,
    capacity: { current: 8, max: 12 },
    teacher: 'Макарова Ю.А.',
    schedule: [
      { dayOfWeek: 'Вторник', startTime: '11:00', endTime: '12:30', location: 'Зал для йоги' },
      { dayOfWeek: 'Четверг', startTime: '11:00', endTime: '12:30', location: 'Зал для йоги' }
    ]
  },
  {
    id: 16,
    name: 'Легкая атлетика',
    description: 'Тренировки по бегу, прыжкам и метаниям. Развитие скорости и выносливости.',
    image: 'https://images.unsplash.com/photo-1552674605-db6ffd4facb5?w=800&h=400&q=80',
    price: 2000,
    capacity: { current: 15, max: 20 },
    teacher: 'Попов А.И.',
    schedule: [
      { dayOfWeek: 'Понедельник', startTime: '16:30', endTime: '18:00', location: 'Стадион' },
      { dayOfWeek: 'Четверг', startTime: '16:30', endTime: '18:00', location: 'Стадион' }
    ]
  },
  {
    id: 17,
    name: 'Теннис',
    description: 'Обучение технике большого тенниса. Индивидуальные и групповые тренировки.',
    image: 'https://images.unsplash.com/photo-1595435934249-5df7ed86e1c0?w=800&h=400&q=80',
    price: 3500,
    capacity: { current: 4, max: 6 },
    teacher: 'Лебедев П.С.',
    schedule: [
      { dayOfWeek: 'Среда', startTime: '15:00', endTime: '16:30', location: 'Теннисный корт' },
      { dayOfWeek: 'Суббота', startTime: '15:00', endTime: '16:30', location: 'Теннисный корт' }
    ]
  },
  {
    id: 18,
    name: 'Гимнастика',
    description: 'Развитие гибкости, координации и силы. Элементы художественной и спортивной гимнастики.',
    image: 'https://images.unsplash.com/photo-1590937144826-a0805797660c?w=800&h=400&q=80',
    price: 2800,
    capacity: { current: 10, max: 12 },
    teacher: 'Смирнова Т.В.',
    schedule: [
      { dayOfWeek: 'Вторник', startTime: '17:00', endTime: '18:30', location: 'Гимнастический зал' },
      { dayOfWeek: 'Пятница', startTime: '17:00', endTime: '18:30', location: 'Гимнастический зал' }
    ]
  },
  {
    id: 19,
    name: 'Каратэ',
    description: 'Изучение техники каратэ, развитие силы духа и тела. Подготовка к соревнованиям.',
    image: 'https://images.unsplash.com/photo-1555597408-22bc30f27001?w=800&h=400&q=80',
    price: 3000,
    capacity: { current: 12, max: 15 },
    teacher: 'Васильев И.К.',
    schedule: [
      { dayOfWeek: 'Понедельник', startTime: '19:00', endTime: '20:30', location: 'Додзё' },
      { dayOfWeek: 'Четверг', startTime: '19:00', endTime: '20:30', location: 'Додзё' }
    ]
  },
  {
    id: 20,
    name: 'Шахматы',
    description: 'Обучение стратегии и тактике шахматной игры. Развитие логического мышления.',
    image: 'https://images.unsplash.com/photo-1529699211952-734e80c4d42b?w=800&h=400&q=80',
    price: 1500,
    capacity: { current: 8, max: 10 },
    teacher: 'Королев М.П.',
    schedule: [
      { dayOfWeek: 'Среда', startTime: '17:00', endTime: '18:30', location: 'Шахматный клуб' },
      { dayOfWeek: 'Суббота', startTime: '11:00', endTime: '12:30', location: 'Шахматный клуб' }
    ]
  }
]

// Format schedule for display
function formatSchedule(schedule: Schedule[]): string {
  return schedule
    .map(s => `${s.dayOfWeek} ${s.startTime}-${s.endTime}`)
    .join(' • ')
}
</script>

<template>
  <div class="sections-page">
    <div class="sections-header">
      <h1>Доступные секции</h1>
      <p class="sections-description">Выберите подходящую секцию и запишитесь на занятия</p>
    </div>

    <div class="sections-grid">
      <div
        v-for="section in sections"
        :key="section.id"
        class="section-card card"
      >
        <div class="section-image">
          <img :src="section.image" :alt="section.name">
          <div class="section-price">{{ section.price }} ₽/мес</div>
        </div>

        <div class="section-content">
          <h2>{{ section.name }}</h2>
          <p class="section-description">{{ section.description }}</p>

          <div class="section-details">
            <div class="detail-item">
              <i class="material-icons">person</i>
              <span>{{ section.teacher }}</span>
            </div>
            <div class="detail-item">
              <i class="material-icons">groups</i>
              <span>{{ section.capacity.current }}/{{ section.capacity.max }} мест</span>
            </div>
            <div class="detail-item schedule">
              <i class="material-icons">schedule</i>
              <span>{{ formatSchedule(section.schedule) }}</span>
            </div>
          </div>

          <button
            class="enroll-button"
            :disabled="section.capacity.current >= section.capacity.max"
          >
            {{ section.capacity.current >= section.capacity.max ? 'Нет мест' : 'Записаться' }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.sections-page {
  padding: 2rem 1rem;
  max-width: 1400px;
  margin: 0 auto;
}

.sections-header {
  text-align: center;
  margin-bottom: 2rem;
}

.sections-header h1 {
  color: var(--text-color);
  font-size: 2rem;
  margin: 0 0 0.5rem;
}

.sections-description {
  color: var(--text-muted);
  font-size: 1.125rem;
  margin: 0;
}

.sections-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(min(100%, 400px), 1fr));
  gap: 2rem;
}

.section-card {
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.section-card:hover {
  transform: translateY(-4px);
  box-shadow: var(--shadow-lg);
}

.section-image {
  position: relative;
  height: 200px;
  overflow: hidden;
}

.section-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.section-card:hover .section-image img {
  transform: scale(1.05);
}

.section-price {
  position: absolute;
  right: 1rem;
  top: 1rem;
  background: var(--primary-color);
  color: white;
  padding: 0.5rem 1rem;
  border-radius: var(--border-radius);
  font-weight: 600;
}

.section-content {
  padding: 1.5rem;
}

.section-content h2 {
  margin: 0 0 0.5rem;
  color: var(--text-color);
  font-size: 1.5rem;
}

.section-description {
  color: var(--text-muted);
  margin: 0 0 1.5rem;
  line-height: 1.5;
}

.section-details {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  margin-bottom: 1.5rem;
}

.detail-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: var(--text-muted);
  font-size: 0.875rem;
}

.detail-item i {
  color: var(--primary-color);
  font-size: 1.25rem;
}

.detail-item.schedule {
  flex-wrap: wrap;
  gap: 0.25rem 0.5rem;
}

.enroll-button {
  width: 100%;
  padding: 0.75rem;
  border: none;
  border-radius: var(--border-radius);
  background: var(--primary-color);
  color: white;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.enroll-button:hover:not(:disabled) {
  background: var(--primary-dark);
}

.enroll-button:disabled {
  background: var(--border-color);
  cursor: not-allowed;
}

/* Dark mode */
:global(.dark-mode) .sections-header h1 {
  color: var(--text-dark);
}

:global(.dark-mode) .sections-description {
  color: var(--text-muted-dark);
}

:global(.dark-mode) .section-content h2 {
  color: var(--text-dark);
}

:global(.dark-mode) .section-description,
:global(.dark-mode) .detail-item {
  color: var(--text-muted-dark);
}

:global(.dark-mode) .enroll-button:disabled {
  background: var(--border-dark);
}

/* Responsive */
@media (max-width: 768px) {
  .sections-page {
    padding: 1rem;
  }

  .sections-header h1 {
    font-size: 1.5rem;
  }

  .sections-description {
    font-size: 1rem;
  }

  .sections-grid {
    gap: 1rem;
  }

  .section-image {
    height: 160px;
  }

  .section-content {
    padding: 1rem;
  }

  .section-content h2 {
    font-size: 1.25rem;
  }
}
</style>
