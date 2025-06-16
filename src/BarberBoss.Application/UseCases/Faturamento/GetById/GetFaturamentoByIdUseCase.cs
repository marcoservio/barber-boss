using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Faturamentos;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Faturamento.GetById;

public class GetFaturamentoByIdUseCase(IFaturamentoReadOnlyRepository repository, IMapper mapper) : IGetFaturamentoByIdUseCase
{
    private readonly IFaturamentoReadOnlyRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseFaturamentoJson> Execute(long id)
    {
        var result = await _repository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.FATURAMENTO_NOT_FOUND);

        return _mapper.Map<ResponseFaturamentoJson>(result);
    }
}
