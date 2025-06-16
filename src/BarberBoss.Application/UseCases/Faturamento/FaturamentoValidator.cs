using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Faturamento;

public class FaturamentoValidator : AbstractValidator<RequestFaturamentoJson>
{
    public FaturamentoValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.TITLE_REQUIRED);

        RuleFor(e => e.Amount)
            .GreaterThan(0)
            .WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);

        RuleFor(e => e.PaymentType)
            .IsInEnum()
            .WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
    }
}