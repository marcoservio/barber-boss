using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Faturamentos;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Faturamento.Update;

public class UpdateFaturamentoUseCase(IFaturamentoUpdateOnlyRepository updateOnlyRepository, IMapper mapper, IUnitOfWork unitOfWork) : IUpdateFaturamentoUseCase
{
    private readonly IFaturamentoUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(long id, RequestFaturamentoJson request)
    {
        Validate(request);

        var expense = await _updateOnlyRepository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.FATURAMENTO_NOT_FOUND);

        _mapper.Map(request, expense);

        _updateOnlyRepository.Update(expense);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestFaturamentoJson request)
    {
        var result = new FaturamentoValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException([.. result.Errors.Select(f => f.ErrorMessage)]);
    }
}
