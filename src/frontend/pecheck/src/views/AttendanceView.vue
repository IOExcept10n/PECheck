<template>
  <div class="attendance-page">
    <div class="attendance-card card">
      <h2>Учет посещаемости</h2>
      <div class="attendance-content">
        <p>Здесь будет отображаться интерфейс управления посещаемостью</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.attendance-page {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.attendance-card {
  padding: 2rem;
}

.attendance-card h2 {
  margin-bottom: 1.5rem;
  color: var(--primary-color);
}

.attendance-content {
  padding: 1rem 0;
}
</style>