using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using INTECH.Context;
using INTECH.Models;
using System.Linq.Expressions;

namespace INTECH.Services;

public class CategoriasServiciosService(
    IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<CategoriasServicios, int>
{

    public async Task<Result> Guardar(CategoriasServicios categoriaServicio)
    {
        if (!await Existe(categoriaServicio.CategoriaServicioId))
        {
            var inserto = await Insertar(categoriaServicio);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(categoriaServicio);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int categoriaServicioId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.CategoriasServicios
            .AnyAsync(p => p.CategoriaServicioId == categoriaServicioId);
    }

    private async Task<bool> Insertar(CategoriasServicios categoriaServicio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.CategoriasServicios.Add(categoriaServicio);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(CategoriasServicios categoriaServicio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(categoriaServicio);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<CategoriasServicios?>> Buscar(int categoriaServicioId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.CategoriasServicios
            .FirstOrDefaultAsync(p => p.CategoriaServicioId == categoriaServicioId);
    }

    public async Task<Result<List<CategoriasServicios>>> GetList(Expression<Func<CategoriasServicios, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.CategoriasServicios
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result> Eliminar(int categoriaServicioId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var tieneServicios = await contexto.Servicios
            .AnyAsync(s => s.CategoriaServicioId == categoriaServicioId && !s.IsDeleted);
        if (tieneServicios)
        {
            return Result.Success(false);
        }

        var eliminado = await contexto.CategoriasServicios
            .Where(p => p.CategoriaServicioId == categoriaServicioId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }
}
