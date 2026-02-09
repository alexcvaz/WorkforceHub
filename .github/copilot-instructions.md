# Copilot Instructions

## Sobre Este Projeto

**WorkforceHub** é uma aplicação full-stack de registro de ponto (punch) com:
- **Frontend**: Vue 3 + TypeScript + Vite
- **Backend**: .NET 10 + Entity Framework Core
- **Autenticação**: JWT
- **Banco de Dados**: SQLite

## Princípios de Trabalho

### 1. Sem Documentação Desnecessária
- ? **NÃO** criar arquivos `.md` ou `.ts` de documentação
- ? **NÃO** criar arquivos de "resumo" ou "guia"
- ? Apenas se **explicitamente solicitado** pelo usuário
- ? Priorizar código limpo com comentários quando necessário

### 2. Processo de Implementação
- ? Sempre usar `git diff` ou `git status` para verificar mudanças antes
- ? **NUNCA** fazer commit automático
- ? Todas as alterações devem ser **revisadas pelo usuário** primeiro
- ? Commit **apenas quando o usuário solicitar explicitamente**
- ? Descrever as mudanças propostas de forma clara
- ? Aguardar aprovação antes de qualquer ação

### 3. Padrões de Código

#### Frontend (Vue 3 + TypeScript)
```typescript
// ? Usar setup script
<script setup lang="ts">
  // Preferir const/let
  // Type annotations explícitas
  // Composables para lógica reutilizável
</script>

// ? Nomenclatura
- Componentes: PascalCase (PunchButton.vue)
- Variáveis: camelCase
- Constantes: UPPER_SNAKE_CASE
- Funções: camelCase (getPunchesForDay)

// ? Estilos
- Usar <style scoped>
- BEM ou utilitário
- Cores: usar variáveis CSS
```

#### Backend (C# .NET 10)
```csharp
// ? Padrões
- Controllers: async/await
- Services: Interface + Implementation
- DTOs para transferência de dados
- Logging estruturado com ILogger<T>

// ? Nomenclatura
- Classes: PascalCase
- Métodos: PascalCase
- Variáveis locais: camelCase
- Constantes: UPPER_SNAKE_CASE
- Interfaces: IPrefixPascalCase
```

### 4. Revisão de Código

Antes de qualquer mudança, verificar:
- ? Não quebra funcionalidades existentes
- ? Segue padrão do projeto
- ? Tratamento de erros apropriado
- ? Sem console.log/Debug.WriteLine em produção
- ? TypeScript/C# sem erros de tipo

### 5. Commits

**Formato de mensagem:**
```
[TIPO] Descrição breve (50 chars max)

- O quê foi feito
- Por quê foi feito
- Como impacta o projeto

Tipos:
- feat: Nova funcionalidade
- fix: Correção de bug
- refactor: Mudança de código sem alterar funcionalidade
- style: Formatação, sem mudança de lógica
- chore: Tarefas de build, deps, etc
```

**Exemplo:**
```
feat: Adicionar tabela mensal na tela de punch

- Implementar PunchMonthTable.vue
- Adicionar endpoints de busca por período
- Exibir registros em formato tabular (dias x registros)
- Adicionar navegação entre meses

Impacto: Melhora visualização do histórico de pontos
```

### 6. Estrutura de Pastas

```
WorkforceHub/
??? WorkforceHub.Server/
?   ??? Controllers/        (Endpoints HTTP)
?   ??? Application/
?   ?   ??? Services/       (Lógica de negócio)
?   ?   ??? Interfaces/     (Contratos)
?   ?   ??? DTOs/          (Transfer objects)
?   ??? Domain/
?   ?   ??? Entities/      (Modelos de domínio)
?   ?   ??? Enums/
?   ??? Infrastructure/
?       ??? Data/          (DbContext, Migrations)
?       ??? Repositories/  (Data access)
?
??? WorkforceHub.Client/
?   ??? src/
?       ??? components/    (Componentes Vue)
?       ??? composables/   (Vue Composables)
?       ??? views/         (Pages/Rotas)
?       ??? services/      (API calls)
?       ??? types/         (TypeScript types)
?       ??? router/        (Vue Router config)
?       ??? assets/        (CSS global, imagens)
?
??? .github/
    ??? copilot-instructions.md (Este arquivo)
```

### 7. Ferramentas e Tecnologias

**Frontend:**
- Vue 3 (Composition API + <script setup>)
- TypeScript
- Vite (build tool)
- Vue Router
- Fetch API / axios para requisições

**Backend:**
- .NET 10
- Entity Framework Core (ORM)
- JWT para autenticação
- SQLite (desenvolvimento)
- Async/Await pattern

**Qualidade:**
- ESLint (frontend)
- TypeScript strict mode
- C# static analysis

### 8. Testes

- ? Testar mudanças antes de pedir revisão
- ? Verificar console do navegador (F12)
- ? Testar em diferentes resoluções (responsive)
- ? Testar casos de erro/edge cases

### 9. Performance

- ? Lazy load de dados quando possível
- ? Evitar re-renders desnecessários (usePunchMonth com provide/inject)
- ? Cache de dados em composables
- ? Compressão de assets em produção

### 10. Segurança

- ? JWT tokens armazenados em localStorage
- ? CORS configurado no backend
- ? Validação no servidor (não confiar apenas no cliente)
- ? SQL injection prevention via ORM
- ? Senhas com hash (bcrypt quando implementado)

### 11. Comunicação

- Sempre descrever mudanças propostas
- Explicar trade-offs de decisões
- Mencionar impactos em outras partes do código
- Sugerir alternativas quando houver dúvidas

### 12. Problemas Conhecidos / Limitações

- SQLite para desenvolvimento (migrar para SQL Server em produção)
- Sem refresh automático (usuário precisa fazer F5)
- Sem autenticação com 2FA
- Sem backup automático de dados

## Fluxo de Trabalho Esperado

```
1. Usuário descreve o que quer fazer
   ?
2. Copilot analisa o código existente
   ?
3. Propõe solução com explicação
   ?
4. Implementa as mudanças
   ?
5. Descreve o que foi feito
   ?
6. Aguarda revisão do usuário
   ?
7. Commit apenas se aprovado
```

## Contato / Dúvidas

Se houver dúvidas sobre implementação:
- Sempre perguntar ao usuário antes de fazer mudanças grandes
- Explicar opções e trade-offs
- Respeitar preferências e padrões estabelecidos

---

**Última atualização**: 2026-02-09
**Versão**: 1.0.0
