using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Faturamentos;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Faturamento.Register;

public class RegisterFaturamentoUseCase(IFaturamentoWriteOnlyRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : IRegisterFaturamentoUseCase
{
    private readonly IFaturamentoWriteOnlyRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseRegisteredFaturamentoJson> Execute(RequestFaturamentoJson request)
    {
        Validate(request);

        var expense = _mapper.Map<Domain.Entities.Faturamento>(request);

        await _repository.Add(expense);

        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisteredFaturamentoJson>(expense);
    }

    private static void Validate(RequestFaturamentoJson request)
    {
        var result = new FaturamentoValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException([.. result.Errors.Select(f => f.ErrorMessage)]);
    }
}
