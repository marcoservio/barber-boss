using BarberBoss.Application.Services.AutoMapper;
using BarberBoss.Application.UseCases.Faturamento.Delete;
using BarberBoss.Application.UseCases.Faturamento.GetAll;
using BarberBoss.Application.UseCases.Faturamento.GetById;
using BarberBoss.Application.UseCases.Faturamento.Register;
using BarberBoss.Application.UseCases.Faturamento.Reports.Excel;
using BarberBoss.Application.UseCases.Faturamento.Reports.Pdf;
using BarberBoss.Application.UseCases.Faturamento.Update;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBoss.Application;

public static class ApplicationDependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
        AddAutoMapper(services);
    }

    public static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterFaturamentoUseCase, RegisterFaturamentoUseCase>();
        services.AddScoped<IGetAllFaturamentosUseCase, GetAllFaturamentosUseCase>();
        services.AddScoped<IGetFaturamentoByIdUseCase, GetFaturamentoByIdUseCase>();
        services.AddScoped<IDeleteFaturamentoUseCase, DeleteFaturamentoUseCase>();
        services.AddScoped<IUpdateFaturamentoUseCase, UpdateFaturamentoUseCase>();

        services.AddScoped<IGenerateFaturamentoReportExcelUseCase, GenerateFaturamentoReportExcelUseCase>();
        services.AddScoped<IGenerateFaturamentoReportPdfUseCase, GenerateFaturamentoReportPdfUseCase>();
    }

    public static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }
}
