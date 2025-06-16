using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestUtilities.Requests;

public class RequestFaturamentoJsonBuilder
{
    public static RequestFaturamentoJson Build()
    {
        return new Faker<RequestFaturamentoJson>()
            .RuleFor(r => r.Title, f => f.Commerce.ProductName())
            .RuleFor(r => r.Description, f => f.Commerce.ProductDescription())
            .RuleFor(r => r.Date, f => f.Date.Past())
            .RuleFor(r => r.PaymentType, f => f.PickRandom<PaymentType>())
            .RuleFor(r => r.Amount, f => f.Random.Decimal(min: 1, max: 1000));
    }
}
