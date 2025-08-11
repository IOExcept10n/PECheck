<template>
  <div class="students-page">
    <div class="students-card card">
      <h2>Студенты</h2>
      <div class="students-content">
        <p>Здесь будет отображаться список студентов</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.students-page {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.students-card {
  padding: 2rem;
}

.students-card h2 {
  margin-bottom: 1.5rem;
  color: var(--primary-color);
}

.students-content {
  padding: 1rem 0;
}
</style>