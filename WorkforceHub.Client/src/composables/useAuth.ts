import { ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '@/services/api'

const isAuthenticated = ref(false)
const currentUserId = ref<string | null>(null)

export const useAuth = () => {
  const router = useRouter()

  // Verificar se há token salvo ao iniciar
  const checkAuth = () => {
    const token = localStorage.getItem('authToken')
    const userId = localStorage.getItem('userId')
    isAuthenticated.value = !!token
    currentUserId.value = userId || null
  }

  const login = async (userId: string) => {
    try {
      const token = await api.login(userId)
      localStorage.setItem('userId', userId)
      currentUserId.value = userId
      isAuthenticated.value = true
      router.push('/punch')
      return token
    } catch (error) {
      isAuthenticated.value = false
      currentUserId.value = null
      throw error
    }
  }

  const logout = () => {
    api.logout()
    localStorage.removeItem('userId')
    localStorage.removeItem('authToken')
    isAuthenticated.value = false
    currentUserId.value = null
    router.push('/login')
  }

  // Verificar autenticação ao carregar o app
  checkAuth()

  return {
    isAuthenticated,
    currentUserId,
    login,
    logout,
    checkAuth
  }
}
