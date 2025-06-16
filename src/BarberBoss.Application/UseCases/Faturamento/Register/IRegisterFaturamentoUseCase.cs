using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Faturamento.Register;

public interface IRegisterFaturamentoUseCase
{
    Task<ResponseRegisteredFaturamentoJson> Execute(RequestFaturamentoJson request);
}
