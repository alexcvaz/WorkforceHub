import { ref, computed } from 'vue'
import api from '@/services/api'
import type { PunchResponse } from '@/types/punch'

export const usePunchMonth = () => {
  const allPunches = ref<PunchResponse[]>([])
  const isLoading = ref(false)
  const error = ref<string | null>(null)
  const selectedMonth = ref(new Date())
  const refreshTrigger = ref(0) // Trigger para forçar refresh

  // Obtém o primeiro e último dia do mês
  const getMonthRange = (date: Date) => {
    const firstDay = new Date(date.getFullYear(), date.getMonth(), 1)
    const lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0)
    return { firstDay, lastDay }
  }

  // Formata data para ISO string sem timezone
  const formatDateForApi = (date: Date): string => {
    return date.toISOString().split('T')[0]
  }

  // Busca punches de um mês específico
  const loadMonthPunches = async (date: Date = new Date()) => {
    isLoading.value = true
    error.value = null

    try {
      const { firstDay, lastDay } = getMonthRange(date)
      
      const startDate = formatDateForApi(firstDay)
      const endDate = formatDateForApi(lastDay)
      
      // Busca todos os punches do mês via API
      const punches = await api.getPunchesByDateRange(startDate, endDate)
      
      allPunches.value = punches
    } catch (err) {
      error.value = 'Erro ao carregar punches do mês'
      console.error('Load month punches error:', err)
    } finally {
      isLoading.value = false
    }
  }

  // Obtém os punches do dia especificado
  const getPunchesForDay = (day: number): PunchResponse[] => {
    const targetDate = new Date(selectedMonth.value.getFullYear(), selectedMonth.value.getMonth(), day)
    const targetDateOnly = new Date(targetDate.getFullYear(), targetDate.getMonth(), targetDate.getDate())
    
    return allPunches.value.filter(punch => {
      const punchDate = new Date(punch.timestamp)
      const punchDateOnly = new Date(punchDate.getFullYear(), punchDate.getMonth(), punchDate.getDate())
      return punchDateOnly.getTime() === targetDateOnly.getTime()
    })
  }

  // Obtém os registros únicos para um dia (sem duplicatas de tipo)
  const getUniquePunchTypesForDay = (day: number): PunchResponse[] => {
    const dayPunches = getPunchesForDay(day)
    const seen = new Set<string>()
    return dayPunches.filter(punch => {
      if (seen.has(punch.type)) return false
      seen.add(punch.type)
      return true
    })
  }

  // Obtém o horário formatado para exibição
  const formatPunchTime = (timestamp: string): string => {
    const date = new Date(timestamp)
    return date.toLocaleTimeString('pt-BR', { 
      hour: '2-digit', 
      minute: '2-digit',
      hour12: false
    })
  }

  // Obtém label do tipo de punch
  const getPunchTypeLabel = (type: string | number): string => {
    const typeStr = String(type)
    const labels: Record<string, string> = {
      'ClockIn': 'Entrada',
      'ClockOut': 'Saída',
      'LunchStart': 'Inicio Almoço',
      'LunchEnd': 'Fim Almoço',
      'Extra': 'Extra',
      'CheckIn': 'Entrada',
      'CheckOut': 'Saída',
      'Break': 'Intervalo',
      'Return': 'Retorno',
      '1': 'Entrada',
      '2': 'Inicio Almoço',
      '3': 'Fim Almoço',
      '4': 'Saída',
      '5': 'Extra'
    }
    return labels[typeStr] || typeStr
  }

  // Nome do mês
  const monthName = computed(() => {
    const months = [
      'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
      'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'
    ]
    return months[selectedMonth.value.getMonth()]
  })

  const yearAndMonth = computed(() => {
    return `${monthName.value} de ${selectedMonth.value.getFullYear()}`
  })

  // Número de dias no mês
  const daysInMonth = computed(() => {
    return new Date(selectedMonth.value.getFullYear(), selectedMonth.value.getMonth() + 1, 0).getDate()
  })

  // Array com dias do mês
  const monthDays = computed(() => {
    return Array.from({ length: daysInMonth.value }, (_, i) => i + 1)
  })

  // Muda para mês anterior
  const previousMonth = () => {
    selectedMonth.value = new Date(selectedMonth.value.getFullYear(), selectedMonth.value.getMonth() - 1, 1)
    loadMonthPunches(selectedMonth.value)
  }

  // Muda para mês próximo
  const nextMonth = () => {
    selectedMonth.value = new Date(selectedMonth.value.getFullYear(), selectedMonth.value.getMonth() + 1, 1)
    loadMonthPunches(selectedMonth.value)
  }

  // Volta para mês atual
  const goToToday = () => {
    selectedMonth.value = new Date()
    loadMonthPunches(selectedMonth.value)
  }

  // Recarrega dados do mês atual (sem navegar)
  const reloadCurrentMonth = async () => {
    await loadMonthPunches(selectedMonth.value)
    // Incrementa trigger para forçar re-render
    refreshTrigger.value++
  }

  return {
    allPunches,
    isLoading,
    error,
    selectedMonth,
    monthName,
    yearAndMonth,
    daysInMonth,
    monthDays,
    refreshTrigger, // Exportar o trigger
    loadMonthPunches,
    getPunchesForDay,
    getUniquePunchTypesForDay,
    formatPunchTime,
    getPunchTypeLabel,
    previousMonth,
    nextMonth,
    goToToday,
    reloadCurrentMonth
  }
}
