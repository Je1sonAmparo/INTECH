using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using INTECH.Context;
using INTECH.Models;
using System.Linq.Expressions;

namespace INTECH.Services;

public class OrdenesTrabajoService(
    IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<OrdenesTrabajo, int>
{

    public async Task<Result> Guardar(OrdenesTrabajo ordenTrabajo)
    {
        if (!await Existe(ordenTrabajo.OrdenTrabajoId))
        {
            var inserto = await Insertar(ordenTrabajo);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(ordenTrabajo);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int ordenTrabajoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.OrdenesTrabajo
            .AnyAsync(o => o.OrdenTrabajoId == ordenTrabajoId);
    }

    private async Task<bool> Insertar(OrdenesTrabajo ordenTrabajo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.OrdenesTrabajo.Add(ordenTrabajo);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(OrdenesTrabajo ordenTrabajo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var ordenExistente = await contexto.OrdenesTrabajo
            .Include(o => o.Solicitud)
            .SingleOrDefaultAsync(o => o.OrdenTrabajoId == ordenTrabajo.OrdenTrabajoId && !o.IsDeleted);
        if (ordenExistente == null || ordenExistente.Solicitud.EstadoSolicitudId == 9)
        {
            return false;
        }

        ordenExistente.SolicitudId = ordenTrabajo.SolicitudId;
        ordenExistente.Tareas = ordenTrabajo.Tareas;
        ordenExistente.Materiales = ordenTrabajo.Materiales;
        ordenExistente.InstruccionesCampo = ordenTrabajo.InstruccionesCampo;

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<OrdenesTrabajo?>> Buscar(int ordenTrabajoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.OrdenesTrabajo
            .Include(o => o.Solicitud)
                .ThenInclude(s => s.Cliente)
            .Include(o => o.Solicitud)
                .ThenInclude(s => s.EstadoSolicitud)
            .Include(o => o.Solicitud)
                .ThenInclude(s => s.SolicitudesDetalle)
                    .ThenInclude(detalle => detalle.Servicio)
            .FirstOrDefaultAsync(o => o.OrdenTrabajoId == ordenTrabajoId && !o.IsDeleted);
    }

    public async Task<Result<List<OrdenesTrabajo>>> GetList(Expression<Func<OrdenesTrabajo, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.OrdenesTrabajo
            .Include(o => o.Solicitud)
                .ThenInclude(s => s.Cliente)
            .Include(o => o.Solicitud)
                .ThenInclude(s => s.EstadoSolicitud)
            .Include(o => o.Solicitud)
                .ThenInclude(s => s.SolicitudesDetalle)
                    .ThenInclude(detalle => detalle.Servicio)
            .Where(o => !o.IsDeleted)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result> Eliminar(int ordenTrabajoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var orden = await contexto.OrdenesTrabajo
            .Include(o => o.Solicitud)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.OrdenTrabajoId == ordenTrabajoId && !o.IsDeleted);
        if (orden == null || orden.Solicitud.EstadoSolicitudId == 8 || orden.Solicitud.EstadoSolicitudId == 9)
        {
            return Result.Success(false);
        }

        var eliminado = await contexto.OrdenesTrabajo
            .Where(o => o.OrdenTrabajoId == ordenTrabajoId && !o.IsDeleted)
            .ExecuteUpdateAsync(o => o.SetProperty(ordenItem => ordenItem.IsDeleted, true)) > 0;
        return Result.Success(eliminado);
    }
}
