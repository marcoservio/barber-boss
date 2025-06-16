using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Faturamentos;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Faturamento.Delete;

public class DeleteFaturamentoUseCase(IFaturamentoWriteOnlyRepository expensesWriteOnlyRepository, IUnitOfWork unitOfWork) : IDeleteFaturamentoUseCase
{
    private readonly IFaturamentoWriteOnlyRepository _expensesWriteOnlyRepository = expensesWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(long id)
    {
        var result =  await _expensesWriteOnlyRepository.Delete(id);

        if (!result)
            throw new NotFoundException(ResourceErrorMessages.FATURAMENTO_NOT_FOUND);

        await _unitOfWork.Commit();
    }
}
