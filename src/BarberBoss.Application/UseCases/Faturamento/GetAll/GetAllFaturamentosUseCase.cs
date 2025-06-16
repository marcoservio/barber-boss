using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Faturamentos;

namespace BarberBoss.Application.UseCases.Faturamento.GetAll;

public class GetAllFaturamentosUseCase(IFaturamentoReadOnlyRepository repository, IMapper mapper) : IGetAllFaturamentosUseCase
{
    private readonly IFaturamentoReadOnlyRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseFaturamentosJson> Execute()
    {
        var result = await _repository.GetAll();

        return new ResponseFaturamentosJson
        {
            Faturamentos = _mapper.Map<List<ResponseShortFaturamentoJson>>(result)
        };
    }
}
