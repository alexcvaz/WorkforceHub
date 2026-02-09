using Microsoft.EntityFrameworkCore;

namespace WorkforceHub.Server.Infrastructure.Data
{
    /// <summary>
    /// Inicializa o banco de dados de forma desacoplada e independente do provider.
    /// Pode ser usado com SQLite, PostgreSQL, SQL Server, etc.
    /// </summary>
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Aplica migrações e cria o banco de dados se necessário
        /// </summary>
        public static async Task InitializeAsync(WorkforceHubDbContext context)
        {
            try
            {
                // Aplicar todas as migrações pendentes
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Erro ao inicializar o banco de dados. Verifique a connection string e permissões.",
                    ex);
            }
        }

        /// <summary>
        /// Recria o banco de dados (apenas para desenvolvimento)
        /// </summary>
        public static async Task ResetDatabaseAsync(WorkforceHubDbContext context)
        {
            try
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Erro ao resetar o banco de dados.",
                    ex);
            }
        }

        /// <summary>
        /// Verifica se o banco de dados existe
        /// </summary>
        public static async Task<bool> DatabaseExistsAsync(WorkforceHubDbContext context)
        {
            return await context.Database.CanConnectAsync();
        }
    }
}
