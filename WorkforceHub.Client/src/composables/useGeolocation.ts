import { ref, computed } from 'vue'
import type { Coordinates } from '@/types/punch'

export function useGeolocation() {
  const coordinates = ref<Coordinates>({
    latitude: null,
    longitude: null,
    accuracy: null
  })

  const isLoading = ref(false)
  const error = ref<string | null>(null)
  const isSupported = ref(
    typeof navigator !== 'undefined' && 'geolocation' in navigator
  )

  const hasCoordinates = computed(
    () => coordinates.value.latitude !== null && coordinates.value.longitude !== null
  )

  const getCoordinates = (): Promise<Coordinates> => {
    return new Promise((resolve, reject) => {
      if (!isSupported.value) {
        const msg = 'Geolocalização não é suportada neste navegador'
        error.value = msg
        reject(new Error(msg))
        return
      }

      isLoading.value = true
      error.value = null

      navigator.geolocation.getCurrentPosition(
        (position) => {
          coordinates.value = {
            latitude: position.coords.latitude,
            longitude: position.coords.longitude,
            accuracy: position.coords.accuracy
          }
          isLoading.value = false
          resolve(coordinates.value)
        },
        (err) => {
          let message = 'Erro ao obter localização'
          if (err.code === 1) message = 'Permissão de localização negada'
          else if (err.code === 2) message = 'Localização indisponível'
          else if (err.code === 3) message = 'Requisição de localização expirou'

          error.value = message
          isLoading.value = false
          reject(new Error(message))
        },
        {
          enableHighAccuracy: true,
          timeout: 10000,
          maximumAge: 0
        }
      )
    })
  }

  const clearCoordinates = () => {
    coordinates.value = {
      latitude: null,
      longitude: null,
      accuracy: null
    }
    error.value = null
  }

  return {
    coordinates,
    isLoading,
    error,
    isSupported,
    hasCoordinates,
    getCoordinates,
    clearCoordinates
  }
}
