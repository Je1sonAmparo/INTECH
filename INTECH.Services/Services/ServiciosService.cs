using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using INTECH.Context;
using INTECH.Models;
using System.Linq.Expressions;

namespace INTECH.Services;

public class ServiciosService(
    IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Servicios, int>
{

    public async Task<Result> Guardar(Servicios servicio)
    {
        if (!await Existe(servicio.ServicioId))
        {
            var inserto = await Insertar(servicio);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(servicio);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int servicioId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Servicios
            .AnyAsync(s => s.ServicioId == servicioId);
    }

    public async Task<bool> ExisteNombre(string nombre, int servicioId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Servicios
            .AnyAsync(s => s.Nombre == nombre && s.ServicioId != servicioId && !s.IsDeleted);
    }

    private async Task<bool> Insertar(Servicios servicio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Servicios.Add(servicio);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Servicios servicio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(servicio);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Servicios?>> Buscar(int servicioId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Servicios
            .FirstOrDefaultAsync(s => s.ServicioId == servicioId && !s.IsDeleted);
    }

    public async Task<Result<List<Servicios>>> GetList(Expression<Func<Servicios, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Servicios
            .Include(s => s.CategoriaServicio)
            .Where(s => !s.IsDeleted)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result> Eliminar(int servicioId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Servicios
            .Where(s => s.ServicioId == servicioId && !s.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(servicio => servicio.IsDeleted, true)) > 0;
        return Result.Success(eliminado);
    }
}
