using BarberBoss.Communication.Requests;

namespace BarberBoss.Application.UseCases.Faturamento.Update;

public interface IUpdateFaturamentoUseCase
{
    Task Execute(long id, RequestFaturamentoJson request);
}
