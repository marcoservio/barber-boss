namespace BarberBoss.Application.UseCases.Faturamento.Reports.Excel;

public interface IGenerateFaturamentoReportExcelUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
