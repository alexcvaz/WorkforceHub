<template>
  <div class="punch-table-container">
    <!-- Controles do Mês -->
    <div class="month-controls">
      <button @click="previousMonth" class="btn-nav" title="Mês anterior">
        <svg viewBox="0 0 24 24" fill="currentColor">
          <path d="M15.41 7.41L14 6l-6 6 6 6 1.41-1.41L10.83 12z"></path>
        </svg>
      </button>

      <div class="month-display">
        <h2 class="month-title">{{ yearAndMonth }}</h2>
      </div>

      <button @click="nextMonth" class="btn-nav" title="Próximo mês">
        <svg viewBox="0 0 24 24" fill="currentColor">
          <path d="M10 6L8.59 7.41 13.17 12l-4.58 4.59L10 18l6-6z"></path>
        </svg>
      </button>

      <button @click="goToToday" class="btn-today">
        Hoje
      </button>
    </div>

    <!-- Tabela de Punches -->
    <div class="table-wrapper">
      <table class="punch-table">
        <thead>
          <tr>
            <th class="day-header">Dia</th>
            <th class="punch-header" v-for="index in maxPunchesPerDay" :key="`header-${index}`">
              Registro {{ index }}
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="day in monthDays" :key="day" :class="getDayRowClass(day)">
            <td class="day-cell">
              <div class="day-number">{{ String(day).padStart(2, '0') }}</div>
              <div class="day-name">{{ getDayName(day) }}</div>
            </td>
            <td v-for="punchIndex in maxPunchesPerDay" :key="`punch-${day}-${punchIndex}`" class="punch-cell">
              <div v-if="getPunchAtIndex(day, punchIndex)" class="punch-info">
                <div class="punch-time">{{ getPunchAtIndex(day, punchIndex)?.formattedTime }}</div>
                <div class="punch-type">{{ getPunchAtIndex(day, punchIndex)?.label }}</div>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Info -->
    <div v-if="isLoading" class="loading-info">
      <p>Carregando dados...</p>
    </div>
    <div v-if="error" class="error-info">
      <p>{{ error }}</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, inject, watch, onMounted } from 'vue'
import type { PunchResponse } from '@/types/punch'

// Injetar a instância compartilhada do composable
const punchMonth = inject<any>('punchMonth')

if (!punchMonth) {
  throw new Error('punchMonth não foi fornecido pelo componente pai')
}

const {
  allPunches,
  isLoading,
  error,
  selectedMonth,
  yearAndMonth,
  monthDays,
  refreshTrigger,
  getPunchesForDay,
  formatPunchTime,
  getPunchTypeLabel,
  previousMonth,
  nextMonth,
  goToToday,
  loadMonthPunches
} = punchMonth

// Calcula o máximo de registros por dia para dimensionar a tabela
const maxPunchesPerDay = computed(() => {
  refreshTrigger.value // Força re-compute quando trigger mudar
  let max = 0
  for (const day of monthDays.value) {
    const count = getPunchesForDay(day).length
    if (count > max) max = count
  }
  return Math.max(max, 2) // Mínimo de 2 colunas
})

// Obtém o punch em um índice específico para um dia
const getPunchAtIndex = (day: number, index: number) => {
  const dayPunches = getPunchesForDay(day)
  const punch = dayPunches[index - 1]
  
  if (!punch) return null
  
  const typeStr = String(punch.type)
  return {
    formattedTime: formatPunchTime(punch.timestamp),
    label: getPunchTypeLabel(typeStr),
    type: punch.type
  }
}

// Obtém o nome do dia da semana
const getDayName = (day: number) => {
  const date = new Date(selectedMonth.value.getFullYear(), selectedMonth.value.getMonth(), day)
  const dayNames = ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab']
  return dayNames[date.getDay()]
}

// Determina classe CSS para linha (para destacar fim de semana)
const getDayRowClass = (day: number) => {
  const date = new Date(selectedMonth.value.getFullYear(), selectedMonth.value.getMonth(), day)
  const dayOfWeek = date.getDay()
  
  if (dayOfWeek === 0 || dayOfWeek === 6) {
    return 'weekend'
  }
  
  // Destaca hoje
  const today = new Date()
  if (
    date.getDate() === today.getDate() &&
    date.getMonth() === today.getMonth() &&
    date.getFullYear() === today.getFullYear()
  ) {
    return 'today'
  }
  
  return ''
}

// Já carregado por TimeTap.vue no onMounted, não precisa carregar aqui

</script>

<style scoped>
.punch-table-container {
  background: white;
  border-radius: 16px;
  padding: 24px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
  overflow: hidden;
}

/* Controles do Mês */
.month-controls {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 16px;
  margin-bottom: 24px;
  flex-wrap: wrap;
}

.btn-nav {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
  padding: 0;
  background: #f3f4f6;
  border: none;
  border-radius: 8px;
  color: #374151;
  cursor: pointer;
  transition: all 0.3s ease;
  flex-shrink: 0;
}

.btn-nav:hover {
  background: #e5e7eb;
  color: #111827;
}

.btn-nav svg {
  width: 20px;
  height: 20px;
}

.month-display {
  flex: 1;
  min-width: 200px;
  text-align: center;
}

.month-title {
  margin: 0;
  font-size: 24px;
  font-weight: 700;
  color: #0f172a;
}

.btn-today {
  padding: 10px 20px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  flex-shrink: 0;
}

.btn-today:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3);
}

/* Tabela */
.table-wrapper {
  overflow-x: auto;
  margin-bottom: 20px;
}

.punch-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 13px;
}

.punch-table thead {
  background: #f9fafb;
  border-bottom: 2px solid #e5e7eb;
}

.punch-table th {
  padding: 12px 8px;
  text-align: left;
  font-weight: 600;
  color: #374151;
  white-space: nowrap;
}

.day-header {
  min-width: 80px;
  background: #f3f4f6;
  font-weight: 700;
  color: #1f2937;
}

.punch-header {
  min-width: 100px;
  text-align: center;
  font-size: 12px;
}

.punch-table td {
  padding: 12px 8px;
  border-bottom: 1px solid #e5e7eb;
}

.day-cell {
  background: #fafbfc;
  font-weight: 600;
  min-width: 80px;
  position: sticky;
  left: 0;
  z-index: 10;
}

.day-number {
  font-size: 16px;
  color: #0f172a;
  margin-bottom: 2px;
}

.day-name {
  font-size: 11px;
  color: #6b7280;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.punch-cell {
  text-align: center;
  background: white;
  transition: background-color 0.2s ease;
}

.punch-cell:hover {
  background: #f9fafb;
}

.punch-info {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.punch-time {
  font-weight: 700;
  color: #667eea;
  font-family: 'Courier New', monospace;
  font-size: 12px;
}

.punch-type {
  font-size: 11px;
  color: #6b7280;
}

/* Destaque para fim de semana */
.punch-table tbody tr.weekend .day-cell {
  background: #f0f4ff;
}

.punch-table tbody tr.weekend .punch-cell {
  background: #fafbfc;
}

/* Destaque para hoje */
.punch-table tbody tr.today .day-cell {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.punch-table tbody tr.today .day-number {
  color: white;
}

.punch-table tbody tr.today .day-name {
  color: rgba(255, 255, 255, 0.9);
}

.punch-table tbody tr.today .punch-cell {
  background: #f3f0ff;
}

/* Info */
.loading-info,
.error-info {
  text-align: center;
  padding: 20px;
  border-radius: 8px;
  font-size: 14px;
  margin-top: 16px;
}

.loading-info {
  background: #eff6ff;
  color: #1e40af;
}

.error-info {
  background: #fef2f2;
  color: #991b1b;
  border: 1px solid #fee2e2;
}

/* Responsivo */
@media (max-width: 768px) {
  .punch-table-container {
    padding: 16px;
  }

  .month-controls {
    gap: 12px;
  }

  .month-title {
    font-size: 20px;
  }

  .punch-table {
    font-size: 12px;
  }

  .punch-table th,
  .punch-table td {
    padding: 10px 6px;
  }

  .punch-header {
    min-width: 90px;
    font-size: 11px;
  }

  .punch-time {
    font-size: 11px;
  }

  .punch-type {
    font-size: 10px;
  }
}

@media (max-width: 480px) {
  .punch-table-container {
    padding: 12px;
  }

  .month-controls {
    gap: 8px;
  }

  .month-display {
    min-width: auto;
    flex: 0;
  }

  .month-title {
    font-size: 18px;
  }

  .punch-table {
    font-size: 11px;
  }

  .punch-table th,
  .punch-table td {
    padding: 8px 4px;
  }

  .day-header {
    min-width: 70px;
  }

  .day-cell {
    min-width: 70px;
  }

  .day-number {
    font-size: 14px;
  }

  .day-name {
    font-size: 10px;
  }

  .punch-header {
    min-width: 80px;
    font-size: 10px;
  }

  .punch-time {
    font-size: 10px;
  }

  .punch-type {
    font-size: 9px;
  }
}
</style>
