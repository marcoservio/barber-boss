using BarberBoss.Application.UseCases.Faturamento.Reports.Excel;
using BarberBoss.Application.UseCases.Faturamento.Reports.Pdf;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BarberBoss.Api.Controllers
{
    public class ReportController : BarberBossBaseController
    {
        [HttpGet("excel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetExcel(
        [FromServices] IGenerateFaturamentoReportExcelUseCase useCase,
        [FromHeader] DateOnly month)
        {
            byte[] file = await useCase.Execute(month);

            if (file.Length == 0)
                return NoContent();

            return File(file, MediaTypeNames.Application.Octet, "report.xlsx");
        }

        [HttpGet("pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPdf(
            [FromServices] IGenerateFaturamentoReportPdfUseCase useCase,
            [FromHeader] DateOnly month)
        {
            byte[] file = await useCase.Execute(month);

            if (file.Length == 0)
                return NoContent();

            return File(file, MediaTypeNames.Application.Pdf, "report.pdf");
        }
    }
}
