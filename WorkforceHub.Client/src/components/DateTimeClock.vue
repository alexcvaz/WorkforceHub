<template>
  <div class="datetime-clock">
    <div class="datetime-display">
      <span class="date">{{ formattedDate }}</span>
      <span class="separator"> - </span>
      <span class="time">{{ formattedTime }}</span>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'

const formattedDate = ref('')
const formattedTime = ref('00:00:00')

let intervalId: ReturnType<typeof setInterval> | null = null

const updateDateTime = () => {
  const now = new Date()
  
  // Formata data: "09 de fevereiro de 2026"
  formattedDate.value = now.toLocaleDateString('pt-BR', {
    day: '2-digit',
    month: 'long',
    year: 'numeric'
  })
  
  // Formata hora: "11:33:09"
  formattedTime.value = now.toLocaleTimeString('pt-BR', {
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
    hour12: false
  })
}

onMounted(() => {
  updateDateTime()
  intervalId = setInterval(updateDateTime, 1000)
})

onUnmounted(() => {
  if (intervalId) {
    clearInterval(intervalId)
  }
})
</script>

<style scoped>
.datetime-clock {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 16px;
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  margin-bottom: 0;
  height: 70px;
}

.datetime-display {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 16px;
  color: #374151;
  font-weight: 500;
}

.date {
  color: #6b7280;
  text-transform: capitalize;
}

.separator {
  color: #d1d5db;
  font-weight: 300;
}

.time {
  font-family: 'Courier New', monospace;
  font-weight: 700;
  color: #667eea;
  letter-spacing: 0.5px;
}

@media (max-width: 768px) {
  .datetime-clock {
    padding: 12px;
    margin-bottom: 16px;
  }

  .datetime-display {
    font-size: 14px;
    gap: 6px;
  }

  .date {
    font-size: 13px;
  }

  .time {
    font-size: 15px;
  }
}

@media (max-width: 480px) {
  .datetime-clock {
    padding: 10px;
    margin-bottom: 12px;
  }

  .datetime-display {
    font-size: 12px;
    gap: 4px;
    flex-wrap: wrap;
    justify-content: center;
  }

  .date {
    font-size: 12px;
    flex-basis: 100%;
  }

  .separator {
    display: none;
  }

  .time {
    font-size: 14px;
  }
}
</style>
