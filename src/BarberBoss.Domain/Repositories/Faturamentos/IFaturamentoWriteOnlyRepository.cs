using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Faturamentos;

public interface IFaturamentoWriteOnlyRepository
{
    Task Add(Faturamento faturamento);
    Task<bool> Delete(long id);
}
