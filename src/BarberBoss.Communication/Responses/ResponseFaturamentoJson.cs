using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Responses;

public class ResponseFaturamentoJson
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public PaymentType PaymentType { get; set; }
}
