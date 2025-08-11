<template>
  <div class="statistics-page">
    <div class="statistics-card card">
      <h2>Статистика</h2>
      <div class="statistics-content">
        <p>Здесь будет отображаться статистика и аналитика</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.statistics-page {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.statistics-card {
  padding: 2rem;
}

.statistics-card h2 {
  margin-bottom: 1.5rem;
  color: var(--primary-color);
}

.statistics-content {
  padding: 1rem 0;
}
</style>