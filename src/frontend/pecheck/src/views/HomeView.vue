<script setup lang="ts">
// Mock news data
const news = [
  {
    id: 1,
    title: 'Открытие нового спортзала',
    image: 'https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=800&h=400&q=80'
  },
  {
    id: 2,
    title: 'Соревнования по баскетболу',
    image: 'https://images.unsplash.com/photo-1546519638-68e109498ffc?w=800&h=400&q=80'
  },
  {
    id: 3,
    title: 'Новые тренажеры',
    image: 'https://images.unsplash.com/photo-1540497077202-7c8a3999166f?w=800&h=400&q=80'
  },
  {
    id: 4,
    title: 'Набор в секцию волейбола',
    image: 'https://images.unsplash.com/photo-1592656094267-764a45160876?w=800&h=400&q=80'
  },
  {
    id: 5,
    title: 'Летние тренировки',
    image: 'https://images.unsplash.com/photo-1517438322307-e67111335449?w=800&h=400&q=80'
  }
]

function handleScroll(event: WheelEvent) {
  const container = event.currentTarget as HTMLElement
  container.scrollLeft += event.deltaY
}
</script>

<template>
  <div class="home-page">
    <div class="welcome-section">
      <div class="welcome-card card">
        <h1>Добро пожаловать в PE Check</h1>
        <p>Система учета посещаемости и контроля результатов по физической культуре</p>
      </div>
    </div>

    <div class="news-section">
      <h2>Новости и события</h2>

      <div class="news-carousel" @wheel.prevent="handleScroll">
        <div class="news-container">
          <div
            v-for="item in news"
            :key="item.id"
            class="news-card card"
          >
            <div class="news-image">
              <img :src="item.image" :alt="item.title">
            </div>
            <div class="news-content">
              <h3>{{ item.title }}</h3>
            </div>
          </div>
        </div>

        <div class="scroll-hint">
          <div class="scroll-icon">
            <i class="material-icons">swipe</i>
            Прокрутите для просмотра
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.home-page {
  display: flex;
  flex-direction: column;
  gap: 2rem;
  padding: 2rem 1rem;
}

.welcome-card {
  background: linear-gradient(135deg, var(--primary-color), var(--primary-dark));
  color: white;
  padding: 2rem;
  text-align: center;
}

.welcome-card h1 {
  margin: 0 0 1rem;
  font-size: 2rem;
}

.welcome-card p {
  margin: 0;
  opacity: 0.9;
  font-size: 1.125rem;
}

.news-section {
  position: relative;
}

.news-section h2 {
  margin: 0 0 1.5rem;
  color: var(--text-color);
  font-size: 1.5rem;
}

.news-carousel {
  position: relative;
  margin: 0 -1rem;
  padding: 0 1rem;
}

.news-container {
  display: flex;
  gap: 1rem;
  overflow-x: auto;
  padding: 1rem 0;
  scroll-snap-type: x mandatory;
  -webkit-overflow-scrolling: touch;
  scrollbar-width: none; /* Firefox */
}

.news-container::-webkit-scrollbar {
  display: none; /* Chrome, Safari, Opera */
}

.news-card {
  flex: 0 0 300px;
  scroll-snap-align: start;
  overflow: hidden;
  transition: transform 0.3s ease;
  cursor: pointer;
}

.news-card:hover {
  transform: translateY(-4px);
}

.news-image {
  height: 200px;
  overflow: hidden;
}

.news-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.news-card:hover .news-image img {
  transform: scale(1.05);
}

.news-content {
  padding: 1rem;
}

.news-content h3 {
  margin: 0;
  font-size: 1.125rem;
  color: var(--text-color);
}

.scroll-hint {
  position: absolute;
  bottom: -2rem;
  left: 0;
  right: 0;
  display: flex;
  justify-content: center;
  opacity: 0.7;
  animation: fadeInOut 2s infinite;
}

.scroll-icon {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: var(--text-muted);
  font-size: 0.875rem;
}

.scroll-icon i {
  font-size: 1.25rem;
}

@keyframes fadeInOut {
  0%, 100% { opacity: 0.7; }
  50% { opacity: 0.3; }
}

/* Dark mode */
:global(.dark-mode) .news-section h2 {
  color: var(--text-dark);
}

:global(.dark-mode) .news-content h3 {
  color: var(--text-dark);
}

:global(.dark-mode) .scroll-icon {
  color: var(--text-muted-dark);
}

/* Touch devices */
@media (hover: none) {
  .news-card:hover {
    transform: none;
  }

  .news-card:hover .news-image img {
    transform: none;
  }
}

/* Responsive */
@media (max-width: 768px) {
  .home-page {
    padding: 1rem;
  }

  .welcome-card {
    padding: 1.5rem;
  }

  .welcome-card h1 {
    font-size: 1.5rem;
  }

  .welcome-card p {
    font-size: 1rem;
  }

  .news-section h2 {
    font-size: 1.25rem;
  }

  .news-card {
    flex: 0 0 260px;
  }

  .news-image {
    height: 160px;
  }
}
</style>
