import axios, { AxiosInstance } from 'axios'
import type { CreatePunchRequest, PunchResponse } from '@/types/punch'

interface LoginResponse {
  token: string
}

class PunchApi {
  private client: AxiosInstance

  constructor(baseURL?: string) {
    this.client = axios.create({
      baseURL: baseURL || 'https://localhost:7041/api',
      headers: {
        'Content-Type': 'application/json'
      }
    })

    // Interceptor para adicionar token JWT
    this.client.interceptors.request.use((config) => {
      const token = localStorage.getItem('authToken')
      if (token) {
        config.headers.Authorization = `Bearer ${token}`
      }
      return config
    })
  }

  async login(userId: string): Promise<string> {
    const response = await this.client.post<LoginResponse>('/auth/login', {
      userId
    })
    const token = response.data.token
    localStorage.setItem('authToken', token)
    return token
  }

  async registerPunch(request: CreatePunchRequest): Promise<PunchResponse> {
    const response = await this.client.post<PunchResponse>('/punch', request)
    return response.data
  }

  async getTodayPunches(): Promise<PunchResponse[]> {
    const response = await this.client.get<PunchResponse[]>('/punch/today')
    return response.data
  }

  async getPunchesByDate(date: string): Promise<PunchResponse[]> {
    const response = await this.client.get<PunchResponse[]>(`/punch/date/${date}`)
    return response.data
  }

  async getPunchesByDateRange(startDate: string, endDate: string): Promise<PunchResponse[]> {
    const response = await this.client.get<PunchResponse[]>(`/punch/range?startDate=${startDate}&endDate=${endDate}`)
    return response.data
  }

  logout(): void {
    localStorage.removeItem('authToken')
  }
}

export default new PunchApi()
