﻿namespace BarberBoss.Application.UseCases.Faturamento.Reports.Pdf;

public interface IGenerateFaturamentoReportPdfUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
