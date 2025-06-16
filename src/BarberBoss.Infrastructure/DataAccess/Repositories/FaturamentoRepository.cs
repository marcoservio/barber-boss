using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Faturamentos;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

public class FaturamentoRepository(BarberBossDbContext context) : IFaturamentoReadOnlyRepository, IFaturamentoWriteOnlyRepository, IFaturamentoUpdateOnlyRepository
{
    private readonly BarberBossDbContext _context = context;

    public async Task Add(Faturamento faturamento) => await _context.Faturamentos.AddAsync(faturamento);

    public async Task<bool> Delete(long id)
    {
        var result = await _context.Faturamentos.FirstOrDefaultAsync(e => e.Id == id);

        if (result is null)
            return false;

        _context.Faturamentos.Remove(result);

        return true;
    }

    public async Task<List<Faturamento>> GetAll() => await _context.Faturamentos.AsNoTracking().ToListAsync();

    async Task<Faturamento?> IFaturamentoReadOnlyRepository.GetById(long id) =>
             await _context.Faturamentos
             .AsNoTracking()
             .FirstOrDefaultAsync(e => e.Id == id);

    async Task<Faturamento?> IFaturamentoUpdateOnlyRepository.GetById(long id) =>
            await _context.Faturamentos
            .FirstOrDefaultAsync(e => e.Id == id);

    public void Update(Faturamento faturamento) => _context.Faturamentos.Update(faturamento);

    public async Task<List<Faturamento>> FilterByMonth(DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _context
            .Faturamentos
            .AsNoTracking()
            .Where(e => e.Date >= startDate && e.Date <= endDate)
            .OrderBy(e => e.Date)
            .ThenBy(e => e.Title)
            .ToListAsync();
    }
}
