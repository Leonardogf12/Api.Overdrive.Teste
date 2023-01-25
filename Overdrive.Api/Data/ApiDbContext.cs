using Microsoft.EntityFrameworkCore;
using Overdrive.Api.Models;

namespace Overdrive.Api.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<HistoricoAlteracao> HistoricoAlteracaos { get; set; }
    }
}
