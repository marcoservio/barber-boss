using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Faturamentos;
using ClosedXML.Excel;

namespace BarberBoss.Application.UseCases.Faturamento.Reports.Excel;

public class GenerateFaturamentoReportExcelUseCase(IFaturamentoReadOnlyRepository readOnlyRepository) : IGenerateFaturamentoReportExcelUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private readonly IFaturamentoReadOnlyRepository _readOnlyRepository = readOnlyRepository;

    public async Task<byte[]> Execute(DateOnly month)
    {
        var faturamentos = await _readOnlyRepository.FilterByMonth(month);

        if (faturamentos.Count == 0)
            return [];

        using var workbook = new XLWorkbook
        {
            Author = "BarberBoss"
        };

        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Arial";

        var worksheet = workbook.Worksheets.Add(month.ToString("Y"));

        InserHeader(worksheet);

        var row = 2;
        foreach (var faturamento in faturamentos)
        {
            WriteExpenseRow(worksheet, row, faturamento);

            row++;
        }

        worksheet.Columns().AdjustToContents();

        var file = new MemoryStream();

        workbook.SaveAs(file);

        return file.ToArray();
    }

    private static void WriteExpenseRow(IXLWorksheet worksheet, int row, Domain.Entities.Faturamento expense)
    {
        worksheet.Cell($"A{row}").Value = expense.Title;
        worksheet.Cell($"B{row}").Value = expense.Date.ToString("d");
        worksheet.Cell($"C{row}").Value = expense.PaymentType.PaymentTypeToString();

        worksheet.Cell($"D{row}").Value = expense.Amount;
        worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"{CURRENCY_SYMBOL} #,##0.00";

        worksheet.Cell($"E{row}").Value = expense.Description;
    }

    private static void InserHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Font.FontColor = XLColor.FromHtml("#FFFFFF");
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#205858");

        worksheet.Range("A1:E1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Columns("B:E").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Column("D").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        worksheet.Cell("D1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
    }
}
