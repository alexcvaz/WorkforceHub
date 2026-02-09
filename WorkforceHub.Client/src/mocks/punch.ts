import type { PunchResponse } from '@/types/punch'

// Dados mock para testes sem conexão com API
export const mockPunches: PunchResponse[] = [
  {
    id: '550e8400-e29b-41d4-a716-446655440001',
    timestamp: new Date(Date.now() - 8 * 60 * 60 * 1000).toISOString(), // 8 horas atrás
    type: 1, // ClockIn
    latitude: -23.5505,
    longitude: -46.6333,
    accuracy: 8.5,
    ipAddress: '192.168.1.100'
  },
  {
    id: '550e8400-e29b-41d4-a716-446655440002',
    timestamp: new Date(Date.now() - 4 * 60 * 60 * 1000).toISOString(), // 4 horas atrás
    type: 2, // LunchStart
    latitude: -23.5505,
    longitude: -46.6333,
    accuracy: 9.2,
    ipAddress: '192.168.1.100'
  },
  {
    id: '550e8400-e29b-41d4-a716-446655440003',
    timestamp: new Date(Date.now() - 2 * 60 * 60 * 1000).toISOString(), // 2 horas atrás
    type: 3, // LunchEnd
    latitude: -23.5505,
    longitude: -46.6333,
    accuracy: 7.8,
    ipAddress: '192.168.1.100'
  }
]

// Para ativar modo mock, descomente em api.ts:
// export default new PunchApi('mock')
//
// E descomente a seção no PunchApi que retorna mock data
