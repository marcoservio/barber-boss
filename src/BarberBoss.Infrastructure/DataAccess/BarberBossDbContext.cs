using BarberBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess;

public class BarberBossDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Faturamento> Faturamentos { get; set; }
}
