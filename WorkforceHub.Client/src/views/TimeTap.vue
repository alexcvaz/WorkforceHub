<script setup lang="ts">
import { ref, onMounted, provide } from 'vue'
import DateTimeClock from '@/components/DateTimeClock.vue'
import PunchButton from '@/components/PunchButton.vue'
import PunchMonthTable from '@/components/PunchMonthTable.vue'
import Notification from '@/components/Notification.vue'
import { usePunch } from '@/composables/usePunch'
import { useAuth } from '@/composables/useAuth'
import { usePunchMonth } from '@/composables/usePunchMonth'

const {
  isRegistering,
  error,
  registerPunch,
  getPunchTypeLabel
} = usePunch()

const { currentUserId, logout } = useAuth()

// Criar uma única instância do composable
const punchMonthInstance = usePunchMonth()

// Fornecer essa instância para todos os componentes filhos
provide('punchMonth', punchMonthInstance)

const showNotification = ref(false)
const notificationType = ref<'success' | 'error' | 'warning' | 'info'>('info')
const notificationTitle = ref('')
const notificationMessage = ref('')

const handlePunchClick = async () => {
  const punch = await registerPunch()

  if (punch) {
    notificationType.value = 'success'
    notificationTitle.value = 'Ponto registrado com sucesso!'
    notificationMessage.value = `${getPunchTypeLabel(punch.type)} às ${new Date(punch.timestamp).toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' })}`
    
    // Recarrega a tabela usando a mesma instância
    await punchMonthInstance.reloadCurrentMonth()
  } else if (error.value) {
    notificationType.value = 'error'
    notificationTitle.value = 'Erro ao registrar ponto'
    notificationMessage.value = error.value
  }

  showNotification.value = true
  setTimeout(() => {
    showNotification.value = false
  }, 5000)
}

onMounted(() => {
  // Carrega dados iniciais
  punchMonthInstance.loadMonthPunches()
})
</script>

<template>
  <div class="timetap-container">
    <!-- Header -->
    <header class="timetap-header">
      <div class="header-content">
        <div class="header-top">
          <div class="header-info">
            <h1 class="header-title">
              <span class="logo-icon">⏱️</span>
              TimeTap
            </h1>
          </div>
          <div class="header-actions">
            <div class="user-info">
              <span class="user-id">{{ currentUserId }}</span>
            </div>
            <button @click="logout" class="btn-logout" title="Sair">
              <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"></path>
                <polyline points="16 17 21 12 16 7"></polyline>
                <line x1="21" y1="12" x2="9" y2="12"></line>
              </svg>
            </button>
          </div>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main class="timetap-main">
      <!-- Zona de Ação (Registrar Ponto) -->
      <section class="action-zone">
        <div class="action-header-compact">
          <!-- Relógio com Data e Hora (Esquerda) -->
          <DateTimeClock />

          <!-- Botão Registrar Ponto (Direita) -->
          <div class="button-wrapper">
            <PunchButton
              :is-loading="isRegistering"
              @click="handlePunchClick"
            >
              <span v-if="!isRegistering">Registrar Ponto</span>
            </PunchButton>
            <p class="button-hint">Clique no botão para registrar sua entrada, saída ou intervalo</p>
          </div>
        </div>

        <div v-if="error && !isRegistering" class="error-info">
          <svg class="error-icon" viewBox="0 0 24 24">
            <path
              d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"
              fill="currentColor"
            />
          </svg>
          <p class="error-text">{{ error }}</p>
        </div>
      </section>

      <!-- Tabela Mensal -->
      <PunchMonthTable />
    </main>

    <!-- Notificação -->
    <Notification
      :show="showNotification"
      :type="notificationType"
      :title="notificationTitle"
      :message="notificationMessage"
      @close="showNotification = false"
    />
  </div>
</template>

<style scoped>
.timetap-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #f8fafc 0%, #f0f4ff 100%);
  display: flex;
  flex-direction: column;
}

/* Header */
.timetap-header {
  background: white;
  border-bottom: 1px solid #e5e7eb;
  padding: 24px 20px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  position: sticky;
  top: 0;
  z-index: 100;
}

.header-content {
  max-width: 1000px;
  margin: 0 auto;
}

.header-top {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 16px;
}

.header-info {
  flex: 1;
}

.header-title {
  margin: 0;
  font-size: 32px;
  font-weight: 700;
  color: #0f172a;
  display: flex;
  align-items: center;
  gap: 12px;
}

.logo-icon {
  display: inline-block;
  font-size: 36px;
  animation: pulse 2s ease-in-out infinite;
}

@keyframes pulse {
  0%,
  100% {
    opacity: 1;
  }
  50% {
    opacity: 0.6;
  }
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-shrink: 0;
}

.user-info {
  padding: 8px 12px;
  background: #f3f4f6;
  border-radius: 8px;
  font-size: 13px;
  color: #374151;
  font-weight: 500;
  white-space: nowrap;
}

.user-id {
  display: block;
}

.btn-logout {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
  padding: 0;
  background: #fee2e2;
  border: none;
  border-radius: 8px;
  color: #991b1b;
  cursor: pointer;
  transition: all 0.3s ease;
  flex-shrink: 0;
}

.btn-logout:hover {
  background: #fecaca;
  transform: scale(1.05);
}

.btn-logout:active {
  transform: scale(0.95);
}

.btn-logout svg {
  width: 20px;
  height: 20px;
}

/* Main Content */
.timetap-main {
  flex: 1;
  max-width: 1000px;
  width: 100%;
  margin: 0 auto;
  padding: 24px 20px;
  display: flex;
  flex-direction: column;
  gap: 24px;
}

/* Action Zone */
.action-zone {
  background: white;
  border-radius: 16px;
  padding: 24px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
}

.action-header-compact {
  display: flex;
  justify-content: space-between;
  align-items: stretch;
  gap: 24px;
}

.button-wrapper {
  display: flex;
  flex-direction: column;
  align-items: stretch;
  gap: 6px;
  flex-shrink: 0;
  width: 250px;
}

.button-wrapper :deep(button) {
  height: 70px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.button-hint {
  margin: 0;
  font-size: 10px;
  color: #6b7280;
  text-align: center;
  line-height: 1.3;
}

.error-info {
  display: flex;
  align-items: flex-start;
  gap: 12px;
  padding: 12px;
  background: #fef2f2;
  border: 1px solid #fee2e2;
  border-radius: 8px;
  color: #991b1b;
}

.error-icon {
  width: 20px;
  height: 20px;
  flex-shrink: 0;
  margin-top: 2px;
}

.error-text {
  margin: 0;
  font-size: 13px;
  line-height: 1.5;
}

/* Responsivo */
@media (max-width: 768px) {
  .header-top {
    flex-direction: column;
    gap: 12px;
  }

  .header-actions {
    width: 100%;
  }

  .timetap-main {
    padding: 16px 16px;
    gap: 16px;
  }

  .action-zone {
    padding: 20px;
  }

  .action-header-compact {
    flex-direction: column;
    align-items: stretch;
    gap: 16px;
  }

  .button-wrapper {
    align-items: stretch;
    width: 100%;
  }

  .button-hint {
    text-align: center;
    max-width: 100%;
    font-size: 9px;
  }

  .header-title {
    font-size: 28px;
  }
}

  @media (max-width: 480px) {
    .timetap-header {
      padding: 16px;
    }

    .header-title {
      font-size: 24px;
    }

    .action-zone {
      padding: 16px;
      border-radius: 12px;
    }

    .action-header-compact {
      flex-direction: column;
      align-items: stretch;
      gap: 12px;
    }

    .button-wrapper {
      align-items: stretch;
      width: 100%;
    }

    .button-hint {
      text-align: center;
      max-width: 100%;
      font-size: 9px;
    }

    .timetap-main {
      padding: 12px 12px;
    }
  }
</style>
