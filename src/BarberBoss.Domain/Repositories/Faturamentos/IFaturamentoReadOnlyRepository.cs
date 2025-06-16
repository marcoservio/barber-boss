using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Faturamentos;

public interface IFaturamentoReadOnlyRepository
{
    Task<List<Faturamento>> GetAll();
    Task<Faturamento?> GetById(long id);
    Task<List<Faturamento>> FilterByMonth(DateOnly date);
}
