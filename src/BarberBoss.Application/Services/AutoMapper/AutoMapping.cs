using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestFaturamentoJson, Faturamento>();
    }

    private void DomainToResponse()
    {
        CreateMap<Faturamento, ResponseRegisteredFaturamentoJson>();
        CreateMap<Faturamento, ResponseShortFaturamentoJson>();
        CreateMap<Faturamento, ResponseFaturamentoJson>();
    }
}
