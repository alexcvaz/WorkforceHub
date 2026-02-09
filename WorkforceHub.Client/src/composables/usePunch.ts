import { ref, computed } from 'vue'
import type { PunchResponse } from '@/types/punch'
import punchApi from '@/services/api'
import { useGeolocation } from './useGeolocation'

export function usePunch() {
  const { getCoordinates, coordinates, isLoading: geoLoading } = useGeolocation()

  const todayPunches = ref<PunchResponse[]>([])
  const isLoading = ref(false)
  const error = ref<string | null>(null)
  const lastPunch = ref<PunchResponse | null>(null)
  const isAuthenticated = ref(!!localStorage.getItem('authToken'))

  const isRegistering = computed(() => isLoading.value || geoLoading.value)

  const getPunchTypeLabel = (type: number): string => {
    const labels: Record<number, string> = {
      1: 'Entrada',
      2: 'Pausa para almoço',
      3: 'Retorno do almoço',
      4: 'Saída',
      5: 'Hora extra'
    }
    return labels[type] || 'Desconhecido'
  }

  const formatTime = (isoString: string): string => {
    const date = new Date(isoString)
    return date.toLocaleTimeString('pt-BR', {
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit'
    })
  }

  const formatDate = (isoString: string): string => {
    const date = new Date(isoString)
    return date.toLocaleDateString('pt-BR', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    })
  }

  const ensureAuthenticated = async (): Promise<boolean> => {
    // Se já tem token, ok
    if (localStorage.getItem('authToken')) {
      isAuthenticated.value = true
      return true
    }

    // Fazer login automático com userId padrão para desenvolvimento
    try {
      await punchApi.login('dev-user')
      isAuthenticated.value = true
      return true
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Erro ao autenticar'
      error.value = message
      console.error('Erro ao autenticar:', err)
      return false
    }
  }

  const registerPunch = async (): Promise<PunchResponse | null> => {
    try {
      isLoading.value = true
      error.value = null

      // Garantir que está autenticado
      const authenticated = await ensureAuthenticated()
      if (!authenticated) {
        return null
      }

      // Obter localização
      await getCoordinates()

      // Registrar ponto
      const punch = await punchApi.registerPunch({
        latitude: coordinates.value.latitude,
        longitude: coordinates.value.longitude,
        accuracy: coordinates.value.accuracy
      })

      lastPunch.value = punch
      await fetchTodayPunches()

      return punch
    } catch (err) {
      const message =
        err instanceof Error ? err.message : 'Erro ao registrar ponto'
      error.value = message
      console.error('Erro ao registrar ponto:', err)
      return null
    } finally {
      isLoading.value = false
    }
  }

  const fetchTodayPunches = async (): Promise<void> => {
    try {
      error.value = null

      // Garantir que está autenticado
      const authenticated = await ensureAuthenticated()
      if (!authenticated) {
        return
      }

      todayPunches.value = await punchApi.getTodayPunches()
    } catch (err) {
      const message =
        err instanceof Error ? err.message : 'Erro ao carregar registros'
      error.value = message
      console.error('Erro ao carregar punches:', err)
    }
  }

  const loadInitialData = async (): Promise<void> => {
    await ensureAuthenticated()
    await fetchTodayPunches()
  }

  const logout = (): void => {
    punchApi.logout()
    isAuthenticated.value = false
    todayPunches.value = []
    lastPunch.value = null
  }

  return {
    todayPunches,
    isLoading,
    error,
    lastPunch,
    isRegistering,
    isAuthenticated,
    registerPunch,
    fetchTodayPunches,
    loadInitialData,
    logout,
    getPunchTypeLabel,
    formatTime,
    formatDate
  }
}
