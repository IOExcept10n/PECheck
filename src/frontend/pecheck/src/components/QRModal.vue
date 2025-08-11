<script setup lang="ts">
import QrcodeVue from 'qrcode-vue'

defineProps<{
  show: boolean
}>()

defineEmits<{
  'update:show': [value: boolean]
}>()

const message = "Здесь должен был быть QR код, но у нас нет денег"
</script>

<template>
  <div v-if="show" class="modal-overlay" @click="$emit('update:show', false)">
    <div class="modal-content card" @click.stop>
      <div class="modal-header">
        <h3>QR-код посещаемости</h3>
        <button class="close-button" @click="$emit('update:show', false)">
          <i class="material-icons">close</i>
        </button>
      </div>
      <div class="qr-container">
        <div class="qr-wrapper">
          <qrcode-vue
            :value="message"
            :size="200"
            level="H"
          />
        </div>
      </div>
      <p class="qr-instructions">Покажите этот QR-код преподавателю для отметки посещаемости</p>
    </div>
  </div>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 50;
}

.modal-content {
  background-color: var(--surface-color);
  border-radius: var(--border-radius-lg);
  width: 90%;
  max-width: 400px;
  box-shadow: var(--shadow-lg);
  overflow: hidden;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  border-bottom: 1px solid var(--border-color);
}

.modal-header h3 {
  font-size: 1.25rem;
  font-weight: 600;
  margin: 0;
  color: var(--text-color);
}

.close-button {
  background: none;
  border: none;
  cursor: pointer;
  color: var(--text-muted);
  padding: 0.5rem;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: var(--border-radius);
  transition: var(--transition-base);
}

.close-button:hover {
  background-color: var(--hover-color);
  color: var(--text-color);
}

.qr-container {
  padding: 2rem;
  display: flex;
  justify-content: center;
  align-items: center;
}

.qr-wrapper {
  background-color: white;
  padding: 1rem;
  border-radius: var(--border-radius);
  box-shadow: var(--shadow-sm);
}

.qr-instructions {
  text-align: center;
  padding: 0 1.5rem 1.5rem;
  color: var(--text-muted);
  margin: 0;
  font-size: 0.875rem;
}

/* Dark mode */
:global(.dark-mode) .modal-content {
  background-color: var(--surface-dark);
}

:global(.dark-mode) .qr-wrapper {
  background-color: white;
}

:global(.dark-mode) .modal-header h3 {
  color: var(--text-dark);
}

:global(.dark-mode) .close-button {
  color: var(--text-muted-dark);
}

:global(.dark-mode) .close-button:hover {
  background-color: var(--hover-dark);
  color: var(--text-dark);
}

:global(.dark-mode) .qr-instructions {
  color: var(--text-muted-dark);
}
</style>
