<template>
  <div class="login-container">
    <div class="login-card">
      <div class="login-header">
        <h1>TimeTap</h1>
        <p>Sistema de Ponto Digital</p>
      </div>

      <form @submit.prevent="handleLogin" class="login-form">
        <div class="form-group">
          <label for="userId">ID do Usuário</label>
          <input
            id="userId"
            v-model="userId"
            type="text"
            placeholder="Digite seu ID"
            required
            @keyup.enter="handleLogin"
            :disabled="isLoading"
          />
        </div>

        <button type="submit" class="btn-login" :disabled="isLoading || !userId.trim()">
          <span v-if="!isLoading">Entrar</span>
          <span v-else>Conectando...</span>
        </button>
      </form>

      <div v-if="errorMessage" class="error-message">
        {{ errorMessage }}
      </div>

      <div class="login-footer">
        <p>Desenvolvedores: Pressione Enter para fazer login rápido</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuth } from '@/composables/useAuth'

const userId = ref('')
const isLoading = ref(false)
const errorMessage = ref('')

const { login } = useAuth()

const handleLogin = async () => {
  if (!userId.value.trim()) {
    errorMessage.value = 'Por favor, digite um ID de usuário'
    return
  }

  isLoading.value = true
  errorMessage.value = ''

  try {
    await login(userId.value.trim())
  } catch (error) {
    errorMessage.value = 'Erro ao fazer login. Tente novamente.'
    console.error('Login error:', error)
  } finally {
    isLoading.value = false
  }
}
</script>

<style scoped>
.login-container {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial,
    'Noto Sans', sans-serif;
}

.login-card {
  background: white;
  border-radius: 16px;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  padding: 48px;
  width: 100%;
  max-width: 400px;
  animation: slideUp 0.3s ease-out;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.login-header {
  text-align: center;
  margin-bottom: 32px;
}

.login-header h1 {
  font-size: 32px;
  font-weight: 700;
  color: #667eea;
  margin: 0 0 8px 0;
}

.login-header p {
  font-size: 14px;
  color: #6b7280;
  margin: 0;
}

.login-form {
  display: flex;
  flex-direction: column;
  gap: 20px;
  margin-bottom: 24px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-group label {
  font-size: 14px;
  font-weight: 500;
  color: #374151;
}

.form-group input {
  padding: 12px 16px;
  border: 2px solid #e5e7eb;
  border-radius: 8px;
  font-size: 14px;
  transition: all 0.3s ease;
  font-family: inherit;
}

.form-group input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.form-group input:disabled {
  background-color: #f3f4f6;
  cursor: not-allowed;
}

.btn-login {
  padding: 12px 24px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  font-family: inherit;
}

.btn-login:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 10px 20px rgba(102, 126, 234, 0.3);
}

.btn-login:active:not(:disabled) {
  transform: translateY(0);
}

.btn-login:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.error-message {
  padding: 12px 16px;
  background-color: #fee2e2;
  border: 1px solid #fecaca;
  border-radius: 8px;
  color: #991b1b;
  font-size: 14px;
  text-align: center;
  animation: slideUp 0.2s ease-out;
}

.login-footer {
  text-align: center;
  padding-top: 20px;
  border-top: 1px solid #e5e7eb;
}

.login-footer p {
  font-size: 12px;
  color: #9ca3af;
  margin: 0;
}

@media (max-width: 480px) {
  .login-card {
    padding: 32px 24px;
    margin: 16px;
  }

  .login-header h1 {
    font-size: 28px;
  }
}
</style>
