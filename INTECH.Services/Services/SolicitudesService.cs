using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using INTECH.Context;
using INTECH.Models;
using System.Linq.Expressions;

namespace INTECH.Services;

public class SolicitudesService(
    IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Solicitudes, int>
{

    public async Task<Result> Guardar(Solicitudes solicitud)
    {
        if (!await Existe(solicitud.SolicitudId))
        {
            var inserto = await Insertar(solicitud);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(solicitud);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int solicitudId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Solicitudes
            .AnyAsync(s => s.SolicitudId == solicitudId);
    }

    private async Task<bool> Insertar(Solicitudes solicitud)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Solicitudes.Add(solicitud);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Solicitudes solicitud)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var solicitudExistente = await contexto.Solicitudes
            .Include(s => s.SolicitudesDetalle)
            .SingleOrDefaultAsync(s => s.SolicitudId == solicitud.SolicitudId);
        if (solicitudExistente == null)
        {
            return false;
        }

        solicitudExistente.ClienteId = solicitud.ClienteId;
        solicitudExistente.Direccion = solicitud.Direccion;
        solicitudExistente.ObservacionesCliente = solicitud.ObservacionesCliente;
        solicitudExistente.FechaSolicitud = solicitud.FechaSolicitud;
        solicitudExistente.EstadoSolicitudId = solicitud.EstadoSolicitudId;

        contexto.RemoveRange(solicitudExistente.SolicitudesDetalle);
        foreach (var detalle in solicitud.SolicitudesDetalle)
        {
            solicitudExistente.SolicitudesDetalle.Add(new SolicitudesDetalle
            {
                SolicitudId = solicitudExistente.SolicitudId,
                ServicioId = detalle.ServicioId,
                Cantidad = detalle.Cantidad
            });
        }

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Solicitudes?>> Buscar(int solicitudId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Solicitudes
            .Include(s => s.SolicitudesDetalle)
                .ThenInclude(detalle => detalle.Servicio)
            .FirstOrDefaultAsync(s => s.SolicitudId == solicitudId && !s.IsDeleted);
    }

    public async Task<Result<List<Solicitudes>>> GetList(Expression<Func<Solicitudes, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Solicitudes
            .Include(s => s.Cliente)
            .ThenInclude(c => c.TelefonosClientes)
            .Include(s => s.EstadoSolicitud)
            .Include(s => s.SolicitudesDetalle)
                .ThenInclude(detalle => detalle.Servicio)
            .Where(s => !s.IsDeleted)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result> Eliminar(int solicitudId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var solicitud = await contexto.Solicitudes
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SolicitudId == solicitudId && !s.IsDeleted);
        if (solicitud == null || solicitud.EstadoSolicitudId == 2 || solicitud.EstadoSolicitudId == 3 || solicitud.EstadoSolicitudId == 9)
        {
            return Result.Success(false);
        }

        await contexto.Agendas
            .Where(a => !a.IsDeleted && a.SolicitudId == solicitudId)
            .ExecuteUpdateAsync(a => a.SetProperty(agenda => agenda.IsDeleted, true));

        var eliminado = await contexto.Solicitudes
            .Where(s => s.SolicitudId == solicitudId && !s.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(solicitud => solicitud.IsDeleted, true)) > 0;
        return Result.Success(eliminado);
    }
}
