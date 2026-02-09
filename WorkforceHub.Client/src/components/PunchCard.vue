<script setup lang="ts">
import type { PunchResponse } from '@/types/punch'
import { computed } from 'vue'

interface Props {
  punch: PunchResponse
  label: string
  isLast?: boolean
}

const props = defineProps<Props>()

const emit = defineEmits<{
  'location-click': []
}>()

const hasLocation = computed(() => {
  return props.punch.latitude !== null && props.punch.longitude !== null
})

function formatTime(isoString: string): string {
  const date = new Date(isoString)
  return date.toLocaleTimeString('pt-BR', {
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit'
  })
}
</script>

<template>
  <div class="punch-card" :class="{ 'is-last': isLast }">
    <div class="punch-header">
      <div class="punch-type-badge">
        <span class="badge-number">{{ punch.type }}</span>
      </div>
      <div class="punch-info">
        <h3 class="punch-label">{{ label }}</h3>
        <p class="punch-time">{{ formatTime(punch.timestamp) }}</p>
      </div>
    </div>

    <div v-if="hasLocation" class="punch-location">
      <svg class="location-icon" viewBox="0 0 24 24" fill="none">
        <path
          d="M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z"
          fill="currentColor"
        />
      </svg>
      <button class="location-button" @click="emit('location-click')">
        <span v-if="punch.accuracy" class="accuracy">
          ±{{ punch.accuracy.toFixed(1) }}m
        </span>
        <span class="location-text">Ver no mapa</span>
      </button>
    </div>

    <div class="punch-meta">
      <span v-if="punch.ipAddress" class="meta-item">
        <strong>IP:</strong> {{ punch.ipAddress }}
      </span>
    </div>
  </div>
</template>

<style scoped>
.punch-card {
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 16px;
  transition: all 0.2s ease;
  position: relative;
  overflow: hidden;
}

.punch-card:not(.is-last) {
  margin-bottom: 12px;
}

.punch-card:hover {
  border-color: #0066cc;
  box-shadow: 0 4px 12px rgba(0, 102, 204, 0.1);
}

.punch-header {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 12px;
}

.punch-type-badge {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: linear-gradient(135deg, #0066cc 0%, #0052a3 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.badge-number {
  color: white;
  font-weight: 700;
  font-size: 18px;
}

.punch-info {
  flex: 1;
}

.punch-label {
  margin: 0;
  font-size: 14px;
  font-weight: 600;
  color: #1f2937;
  text-transform: capitalize;
}

.punch-time {
  margin: 4px 0 0;
  font-size: 24px;
  font-weight: 700;
  color: #0066cc;
}

.punch-location {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 12px;
  padding-bottom: 12px;
  border-bottom: 1px solid #f3f4f6;
}

.location-icon {
  width: 18px;
  height: 18px;
  color: #0066cc;
  flex-shrink: 0;
}

.location-button {
  flex: 1;
  background: none;
  border: none;
  padding: 0;
  text-align: left;
  cursor: pointer;
  display: flex;
  flex-direction: column;
  gap: 2px;
  transition: color 0.2s;
}

.location-button:hover {
  color: #0052a3;
}

.accuracy {
  font-size: 12px;
  color: #6b7280;
  font-weight: 500;
}

.location-text {
  font-size: 13px;
  color: #0066cc;
  font-weight: 600;
  text-decoration: underline;
}

.punch-meta {
  font-size: 12px;
  color: #6b7280;
  display: flex;
  flex-wrap: wrap;
  gap: 12px;
}

.meta-item {
  display: flex;
  gap: 4px;
  align-items: center;
}

.meta-item strong {
  color: #374151;
}

@media (max-width: 480px) {
  .punch-card {
    padding: 12px;
  }

  .punch-time {
    font-size: 20px;
  }

  .punch-label {
    font-size: 13px;
  }
}
</style>
