using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Faturamentos;

public interface IFaturamentoUpdateOnlyRepository
{
    void Update(Faturamento faturamento);
    Task<Faturamento?> GetById(long id);
}
