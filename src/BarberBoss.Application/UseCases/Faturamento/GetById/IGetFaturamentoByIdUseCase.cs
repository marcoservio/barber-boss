using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Faturamento.GetById;

public interface IGetFaturamentoByIdUseCase
{
    Task<ResponseFaturamentoJson> Execute(long id);
}
