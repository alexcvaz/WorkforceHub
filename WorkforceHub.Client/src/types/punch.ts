// Tipos relacionados ao registro de ponto

export enum PunchType {
  ClockIn = 1,
  LunchStart = 2,
  LunchEnd = 3,
  ClockOut = 4,
  Extra = 5
}

export interface Coordinates {
  latitude: number | null
  longitude: number | null
  accuracy: number | null
}

export interface CreatePunchRequest {
  latitude?: number | null
  longitude?: number | null
  accuracy?: number | null
}

export interface PunchResponse {
  id: string
  timestamp: string
  type: PunchType
  latitude?: number | null
  longitude?: number | null
  accuracy?: number | null
  ipAddress?: string | null
}

export interface PunchError {
  message: string
  code?: string
}
