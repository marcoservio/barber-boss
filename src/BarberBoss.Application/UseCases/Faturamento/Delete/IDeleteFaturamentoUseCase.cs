﻿namespace BarberBoss.Application.UseCases.Faturamento.Delete;

public interface IDeleteFaturamentoUseCase
{
    Task Execute(long id);
}
