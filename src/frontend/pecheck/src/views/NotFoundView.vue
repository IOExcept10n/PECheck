<template>
  <div class="not-found">
    <div class="not-found-content">
      <h1>404</h1>
      <h2>Page Not Found</h2>
      <p>The page you're looking for doesn't exist.</p>
      <router-link to="/" class="btn">
        Go Home
      </router-link>
    </div>
  </div>
</template>

<style scoped>
.not-found {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
  padding: 2rem;
}

.not-found-content h1 {
  font-size: 6rem;
  color: var(--primary-color);
  margin: 0;
}

.not-found-content h2 {
  margin: 1rem 0;
  color: var(--text-primary);
}

.not-found-content p {
  color: var(--text-secondary);
  margin-bottom: 2rem;
}
</style> 