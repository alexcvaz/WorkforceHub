# WorkforceHub Client

Frontend da aplicação WorkforceHub - Sistema de registro de ponto.

**Stack**: Vue 3 + TypeScript + Vite

## Setup Rápido

```bash
# Instalar dependências
npm install

# Desenvolvimento (hot-reload)
npm run dev

# Build para produção
npm run build

# Lint
npm run lint
```

**Frontend**: `http://localhost:5173`

## Estrutura

```
src/
??? components/    Componentes reutilizáveis (PunchButton, DateTimeClock, etc)
??? composables/   Lógica reativa (usePunch, useAuth, usePunchMonth)
??? views/         Páginas (TimeTap, Login)
??? services/      API Client
??? types/         TypeScript types
??? router/        Configuração de rotas
??? assets/        CSS e imagens
```

## Funcionalidades

- Autenticação JWT
- Registro de ponto (entrada/saída/intervalo)
- Visualização mensal com tabela interativa
- Relógio em tempo real
- Responsivo (desktop/tablet/mobile)
- Sincronização de estado com Provide/Inject

## Componentes Principais

- **TimeTap.vue** - Página principal de registro
- **Login.vue** - Tela de autenticação
- **PunchMonthTable.vue** - Tabela de histórico mensal
- **DateTimeClock.vue** - Relógio com data/hora
- **PunchButton.vue** - Botão de registro

## IDE Setup

- VS Code + [Vue (Official)](https://marketplace.visualstudio.com/items?itemName=Vue.volar)
- [Vue.js DevTools](https://chromewebstore.google.com/detail/vuejs-devtools/nhdogjmejiglipccpnnnanhbledajbpd)

## Variáveis de Ambiente

Crie `.env.local`:

```env
VITE_API_URL=http://localhost:5000/api
```

## Para Mais Informações

Veja o [README principal](../README.md) na raiz do projeto.

