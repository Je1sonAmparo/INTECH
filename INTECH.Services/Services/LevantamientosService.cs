using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using INTECH.Context;
using INTECH.Models;
using System.Linq.Expressions;

namespace INTECH.Services;

public class LevantamientosService(
    IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Levantamientos, int>
{

    public async Task<Result> Guardar(Levantamientos levantamiento)
    {
        if (!await Existe(levantamiento.LevantamientoId))
        {
            var inserto = await Insertar(levantamiento);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(levantamiento);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int levantamientoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Levantamientos
            .AnyAsync(l => l.LevantamientoId == levantamientoId);
    }

    private async Task<bool> EsLevantamientoValido(Contexto contexto, Levantamientos levantamiento)
    {
        if (string.IsNullOrWhiteSpace(levantamiento.TecnicoId))
        {
            levantamiento.TecnicoId = null;
        }

        if (levantamiento.FechaProgramada.DayOfWeek == DayOfWeek.Sunday)
        {
            return false;
        }

        if (levantamiento.HoraInicio < new TimeOnly(8, 0) || levantamiento.HoraFin > new TimeOnly(18, 0) || levantamiento.HoraFin <= levantamiento.HoraInicio)
        {
            return false;
        }

        var solicitud = await contexto.Solicitudes
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SolicitudId == levantamiento.SolicitudId && !s.IsDeleted);
        if (solicitud == null || solicitud.EstadoSolicitudId == 9 || solicitud.EstadoSolicitudId == 10)
        {
            return false;
        }

        var levantamientoDuplicado = await contexto.Levantamientos
            .AnyAsync(l => !l.IsDeleted && l.SolicitudId == levantamiento.SolicitudId && l.LevantamientoId != levantamiento.LevantamientoId);
        if (levantamientoDuplicado)
        {
            return false;
        }

        return true;
    }

    private async Task<bool> Insertar(Levantamientos levantamiento)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        if (!await EsLevantamientoValido(contexto, levantamiento))
        {
            return false;
        }

        contexto.Levantamientos.Add(levantamiento);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Levantamientos levantamiento)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var levantamientoExistente = await contexto.Levantamientos
            .Include(l => l.LevantamientosDetalle)
                .ThenInclude(d => d.FotosLevantamientos)
            .SingleOrDefaultAsync(l => l.LevantamientoId == levantamiento.LevantamientoId && !l.IsDeleted);
        if (levantamientoExistente == null)
        {
            return false;
        }

        if (!await EsLevantamientoValido(contexto, levantamiento))
        {
            return false;
        }

        levantamientoExistente.SolicitudId = levantamiento.SolicitudId;
        levantamientoExistente.TecnicoId = levantamiento.TecnicoId;
        levantamientoExistente.FechaProgramada = levantamiento.FechaProgramada;
        levantamientoExistente.HoraInicio = levantamiento.HoraInicio;
        levantamientoExistente.HoraFin = levantamiento.HoraFin;
        levantamientoExistente.Observaciones = levantamiento.Observaciones;
        levantamientoExistente.EsBorrador = levantamiento.EsBorrador;

        contexto.RemoveRange(levantamientoExistente.FotosLevantamientos);
        foreach (var foto in levantamiento.FotosLevantamientos)
        {
            levantamientoExistente.FotosLevantamientos.Add(new FotosLevantamientos
            {
                LevantamientoId = levantamientoExistente.LevantamientoId,
                NombreArchivo = foto.NombreArchivo,
                TipoContenido = foto.TipoContenido,
                Contenido = foto.Contenido
            });
        }

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Levantamientos?>> Buscar(int levantamientoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Levantamientos
            .Include(l => l.Solicitud)
                .ThenInclude(s => s.Cliente)
            .Include(l => l.Solicitud)
                .ThenInclude(s => s.EstadoSolicitud)
            .Include(l => l.Solicitud)
                .ThenInclude(s => s.SolicitudesDetalle)
                    .ThenInclude(detalle => detalle.Servicio)
            .Include(l => l.FotosLevantamientos)
            .FirstOrDefaultAsync(l => l.LevantamientoId == levantamientoId && !l.IsDeleted);
    }

    public async Task<Result<List<Levantamientos>>> GetList(Expression<Func<Levantamientos, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Levantamientos
            .Include(l => l.Solicitud)
                .ThenInclude(s => s.Cliente)
            .Include(l => l.Solicitud)
                .ThenInclude(s => s.EstadoSolicitud)
            .Include(l => l.Solicitud)
                .ThenInclude(s => s.SolicitudesDetalle)
                    .ThenInclude(detalle => detalle.Servicio)
            .Include(l => l.FotosLevantamientos)
            .Where(l => !l.IsDeleted)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result> Eliminar(int levantamientoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var levantamiento = await contexto.Levantamientos
            .Include(l => l.Solicitud)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.LevantamientoId == levantamientoId && !l.IsDeleted);
        if (levantamiento == null || levantamiento.Solicitud.EstadoSolicitudId == 2 || levantamiento.Solicitud.EstadoSolicitudId == 3 || levantamiento.Solicitud.EstadoSolicitudId == 9)
        {
            return Result.Success(false);
        }

        var eliminado = await contexto.Levantamientos
            .Where(l => l.LevantamientoId == levantamientoId && !l.IsDeleted)
            .ExecuteUpdateAsync(l => l.SetProperty(levantamientoItem => levantamientoItem.IsDeleted, true)) > 0;
        return Result.Success(eliminado);
    }
}
