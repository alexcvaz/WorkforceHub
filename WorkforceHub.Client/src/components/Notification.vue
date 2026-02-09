<script setup lang="ts">
import { computed, defineEmits } from 'vue'

interface Props {
  type?: 'success' | 'error' | 'warning' | 'info'
  title?: string
  message?: string
  show?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  type: 'info',
  show: false
})

const emit = defineEmits<{
  close: []
}>()
</script>

<template>
  <Transition name="notification">
    <div v-if="show" class="notification" :class="`notification-${type}`">
      <div class="notification-content">
        <svg class="notification-icon" viewBox="0 0 24 24" fill="none">
          <path :d="iconPath" fill="currentColor" />
        </svg>
        <div class="notification-text">
          <h4 v-if="title" class="notification-title">{{ title }}</h4>
          <p v-if="message" class="notification-message">{{ message }}</p>
        </div>
      </div>
      <button class="notification-close" @click="emit('close')" aria-label="Fechar">
        <svg viewBox="0 0 24 24" fill="none">
          <path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z" fill="currentColor" />
        </svg>
      </button>
    </div>
  </Transition>
</template>

<style scoped>
.notification {
  position: fixed;
  top: 20px;
  right: 20px;
  left: 20px;
  max-width: 500px;
  margin: 0 auto;
  border-radius: 12px;
  padding: 16px 20px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  box-shadow: 0 10px 32px rgba(0, 0, 0, 0.15);
  z-index: 1000;
  animation: slideIn 0.3s ease-out;
}

@keyframes slideIn {
  from {
    transform: translateY(-100%);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.notification-content {
  display: flex;
  align-items: flex-start;
  gap: 12px;
  flex: 1;
}

.notification-icon {
  width: 24px;
  height: 24px;
  flex-shrink: 0;
  margin-top: 2px;
}

.notification-text {
  flex: 1;
}

.notification-title {
  margin: 0;
  font-size: 14px;
  font-weight: 600;
  text-transform: capitalize;
}

.notification-message {
  margin: 4px 0 0;
  font-size: 13px;
  opacity: 0.9;
}

.notification-close {
  background: none;
  border: none;
  padding: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  opacity: 0.7;
  transition: opacity 0.2s;
  margin-top: -2px;
}

.notification-close:hover {
  opacity: 1;
}

.notification-close svg {
  width: 20px;
  height: 20px;
}

/* Variantes de tipo */
.notification-success {
  background: #ecfdf5;
  color: #047857;
  border-left: 4px solid #10b981;
}

.notification-error {
  background: #fef2f2;
  color: #991b1b;
  border-left: 4px solid #ef4444;
}

.notification-warning {
  background: #fffbeb;
  color: #92400e;
  border-left: 4px solid #f59e0b;
}

.notification-info {
  background: #eff6ff;
  color: #1e40af;
  border-left: 4px solid #0066cc;
}

/* Transição do Vue */
.notification-enter-active,
.notification-leave-active {
  transition: all 0.3s ease;
}

.notification-leave-to {
  opacity: 0;
  transform: translateY(-100%);
}

@media (max-width: 480px) {
  .notification {
    top: 10px;
    right: 10px;
    left: 10px;
    padding: 12px 16px;
  }

  .notification-icon {
    width: 20px;
    height: 20px;
  }

  .notification-title {
    font-size: 13px;
  }

  .notification-message {
    font-size: 12px;
  }
}
</style>
