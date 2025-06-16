using BarberBoss.Domain.Enums;

namespace BarberBoss.Domain.Entities;

public class Faturamento
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public PaymentType PaymentType { get; set; }
}
