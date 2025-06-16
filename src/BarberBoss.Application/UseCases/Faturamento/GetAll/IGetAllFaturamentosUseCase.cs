using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Faturamento.GetAll;

public interface IGetAllFaturamentosUseCase
{
    Task<ResponseFaturamentosJson> Execute();
}
