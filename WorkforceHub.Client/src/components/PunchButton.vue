<script setup lang="ts">
interface Props {
  isLoading?: boolean
  disabled?: boolean
}

withDefaults(defineProps<Props>(), {
  isLoading: false,
  disabled: false
})

defineEmits<{
  click: []
}>()
</script>

<template>
  <button
    class="punch-button"
    :disabled="isLoading || disabled"
    :class="{ 'is-loading': isLoading }"
    @click="$emit('click')"
  >
    <span v-if="!isLoading" class="button-text">
      <slot>Registrar Ponto</slot>
    </span>
    <span v-else class="button-text loading">
      <span class="spinner"></span>
      Processando...
    </span>
  </button>
</template>

<style scoped>
.punch-button {
  width: 100%;
  height: 120px;
  padding: 0;
  border: none;
  border-radius: 16px;
  background: linear-gradient(135deg, #0066cc 0%, #0052a3 100%);
  color: white;
  font-size: 24px;
  font-weight: 600;
  letter-spacing: 0.5px;
  cursor: pointer;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  box-shadow: 0 8px 24px rgba(0, 102, 204, 0.3);
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  overflow: hidden;
}

.punch-button:not(:disabled):hover {
  background: linear-gradient(135deg, #0052a3 0%, #003d7a 100%);
  box-shadow: 0 12px 32px rgba(0, 102, 204, 0.4);
  transform: translateY(-2px);
}

.punch-button:not(:disabled):active {
  transform: translateY(0);
  box-shadow: 0 4px 12px rgba(0, 102, 204, 0.3);
}

.punch-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.button-text {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
  transition: opacity 0.2s;
}

.button-text.loading {
  color: rgba(255, 255, 255, 0.9);
}

.spinner {
  width: 20px;
  height: 20px;
  border: 3px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

/* Efeito de ondulação ao clicar */
.punch-button::before {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  width: 0;
  height: 0;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.3);
  transform: translate(-50%, -50%);
  pointer-events: none;
}

.punch-button:not(:disabled):active::before {
  animation: ripple 0.6s ease-out;
}

@keyframes ripple {
  to {
    width: 300px;
    height: 300px;
    opacity: 0;
  }
}

@media (max-width: 768px) {
  .punch-button {
    height: 100px;
    font-size: 20px;
  }
}
</style>
