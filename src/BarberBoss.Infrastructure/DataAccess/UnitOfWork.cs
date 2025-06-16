using BarberBoss.Domain.Repositories;

namespace BarberBoss.Infrastructure.DataAccess;

public class UnitOfWork(BarberBossDbContext context) : IUnitOfWork
{
    private readonly BarberBossDbContext _context = context;

    public async Task Commit() => await _context.SaveChangesAsync();
}
