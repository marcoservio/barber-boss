using BarberBoss.Application.UseCases.Faturamento.Reports.Pdf.Colors;
using BarberBoss.Application.UseCases.Faturamento.Reports.Pdf.Fonts;
using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Faturamentos;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace BarberBoss.Application.UseCases.Faturamento.Reports.Pdf;

public class GenerateFaturamentoReportPdfUseCase : IGenerateFaturamentoReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private const int HEIGHT_ROW_FATURAMENTO_TABLE = 25;

    private readonly IFaturamentoReadOnlyRepository _readOnlyRepository;

    public GenerateFaturamentoReportPdfUseCase(IFaturamentoReadOnlyRepository readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;

        GlobalFontSettings.FontResolver = new FaturamentoReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var faturamentos = await _readOnlyRepository.FilterByMonth(month);
        if (faturamentos.Count == 0)
            return [];

        var document = CreateDocument(month);
        var page = CreatePage(document);

        CreateHeaderWithProfilePhotoAndName(page);

        CreateTotalSpentSection(page, month, faturamentos.Sum(e => e.Amount));

        foreach (var faturamento in faturamentos)
        {
            var table = CreateFaturamentoTable(page);

            AddFaturamentoDetailsToTable(faturamento, table);
        }

        return RenderDocument(document);
    }

    private static void AddFaturamentoDetailsToTable(Domain.Entities.Faturamento faturamento, Table table)
    {
        var row = AddNewBlankRow(table);

        AddFaturamentoTitle(row.Cells[0], faturamento.Title);

        AddHeaderForAmount(row.Cells[3]);

        row = AddNewBlankRow(table);

        row.Cells[0].AddParagraph(faturamento.Date.ToString("D"));
        SetStyleBaseForFaturamentoInformation(row.Cells[0]);
        row.Cells[0].Format.LeftIndent = 20;

        row.Cells[1].AddParagraph(faturamento.Date.ToString("t"));
        SetStyleBaseForFaturamentoInformation(row.Cells[1]);

        row.Cells[2].AddParagraph(faturamento.PaymentType.PaymentTypeToString());
        SetStyleBaseForFaturamentoInformation(row.Cells[2]);

        AddAmountForFaturamento(row.Cells[3], faturamento.Amount);

        AddDescriptionForFaturamento(faturamento.Description, table, row);

        AddWhiteSpace(table);
    }

    private static Row AddNewBlankRow(Table table)
    {
        Row row = table.AddRow();
        row.Height = HEIGHT_ROW_FATURAMENTO_TABLE;
        return row;
    }

    private static void AddDescriptionForFaturamento(string? description, Table table, Row row)
    {
        if (!string.IsNullOrWhiteSpace(description))
        {
            var descriptionRow = table.AddRow();
            descriptionRow.Height = HEIGHT_ROW_FATURAMENTO_TABLE;

            descriptionRow.Cells[0].AddParagraph(description);
            descriptionRow.Cells[0].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 10, Color = ColorsHelper.GRAY_MID };
            descriptionRow.Cells[0].Shading.Color = ColorsHelper.GRAY_LIGHT;
            descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            descriptionRow.Cells[0].MergeRight = 2;
            descriptionRow.Cells[0].Format.LeftIndent = 20;

            row.Cells[3].MergeDown = 1;
        }
    }

    private static void AddFaturamentoTitle(Cell cell, string faturamentoTitle)
    {
        cell.AddParagraph(faturamentoTitle.ToUpper());
        cell.Format.Font = new Font { Name = FontHelper.RELEWAY_REGULAR, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private static void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT.ToUpper());
        cell.Format.Font = new Font { Name = FontHelper.RELEWAY_REGULAR, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.GREEN_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.RightIndent = 10;
    }

    private static Document CreateDocument(DateOnly month)
    {
        var document = new Document
        {
            Info = new DocumentInfo
            {
                Title = $"{ResourceReportGenerationMessages.FATURAMENTO_FOR} - {month:Y}",
                Author = "BarberBoss Application"
            }
        };

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.DEFAULT_FONT;

        return document;
    }

    private static Section CreatePage(Document document)
    {
        var section = document.AddSection();

        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;

        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private static void CreateHeaderWithProfilePhotoAndName(Section page)
    {
        var table = page.AddTable();
        table.AddColumn("100");
        table.AddColumn("300");

        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "logo.png");

        row.Cells[0].AddImage(pathFile);

        row.Cells[1].AddParagraph("Barbearia do João".ToUpper());
        row.Cells[1].Format.Font = new Font { Name = FontHelper.RELEWAY_BLACK, Size = 16 };
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }

    private static void CreateTotalSpentSection(Section page, DateOnly month, decimal totalFaturamentos)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = 40;
        paragraph.Format.SpaceAfter = 40;

        var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));
        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RELEWAY_REGULAR, Size = 15 });

        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalFaturamentos:N2}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });

        paragraph.AddLineBreak();
    }

    private static Table CreateFaturamentoTable(Section page)
    {
        var table = page.AddTable();

        var column = table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    private static void SetStyleBaseForFaturamentoInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GRAY_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private static void AddAmountForFaturamento(Cell cell, decimal amount)
    {
        cell.AddParagraph($"{CURRENCY_SYMBOL} {amount:F2}");
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private static void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    private static byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };

        renderer.RenderDocument();

        using var stream = new MemoryStream();

        renderer.PdfDocument.Save(stream);

        return stream.ToArray();
    }
}
