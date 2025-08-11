<template>
  <div class="teachers-page">
    <div class="teachers-card card">
      <h2>Преподаватели</h2>
      <div class="teachers-content">
        <p>Здесь будет отображаться список преподавателей</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.teachers-page {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.teachers-card {
  padding: 2rem;
}

.teachers-card h2 {
  margin-bottom: 1.5rem;
  color: var(--primary-color);
}

.teachers-content {
  padding: 1rem 0;
}
</style>