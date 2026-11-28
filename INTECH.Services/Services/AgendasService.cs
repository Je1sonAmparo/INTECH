using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using INTECH.Context;
using INTECH.Models;
using System.Linq.Expressions;

namespace INTECH.Services;

public class AgendasService(
    IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Agendas, int>
{

    public async Task<Result> Guardar(Agendas agenda)
    {
        if (!await Existe(agenda.AgendaId))
        {
            var inserto = await Insertar(agenda);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(agenda);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int agendaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Agendas
            .AnyAsync(a => a.AgendaId == agendaId);
    }

    private async Task<bool> EsAgendaValida(Contexto contexto, Agendas agenda)
    {
        if (string.IsNullOrWhiteSpace(agenda.TecnicoId))
        {
            agenda.TecnicoId = null;
        }

        if (agenda.Fecha.DayOfWeek == DayOfWeek.Sunday)
        {
            return false;
        }

        if (agenda.HoraInicio < new TimeOnly(8, 0) || agenda.HoraFin > new TimeOnly(18, 0) || agenda.HoraFin <= agenda.HoraInicio)
        {
            return false;
        }

        var solicitud = await contexto.Solicitudes
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SolicitudId == agenda.SolicitudId && !s.IsDeleted);
        if (solicitud == null || solicitud.EstadoSolicitudId == 9)
        {
            return false;
        }

        var solicitudYaAgendada = await contexto.Agendas
            .AnyAsync(a => !a.IsDeleted && a.SolicitudId == agenda.SolicitudId && a.AgendaId != agenda.AgendaId);
        if (solicitudYaAgendada)
        {
            return false;
        }

        var horarioOcupado = await contexto.Agendas
            .AnyAsync(a => !a.IsDeleted &&
                           a.AgendaId != agenda.AgendaId &&
                           a.Fecha.Date == agenda.Fecha.Date &&
                           a.TecnicoId == agenda.TecnicoId &&
                           agenda.HoraInicio < a.HoraFin &&
                           agenda.HoraFin > a.HoraInicio);
        if (horarioOcupado)
        {
            return false;
        }

        return true;
    }

    private async Task<bool> Insertar(Agendas agenda)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        if (!await EsAgendaValida(contexto, agenda))
        {
            return false;
        }

        contexto.Agendas.Add(agenda);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Agendas agenda)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var agendaExistente = await contexto.Agendas
            .SingleOrDefaultAsync(a => a.AgendaId == agenda.AgendaId && !a.IsDeleted);
        if (agendaExistente == null)
        {
            return false;
        }

        if (!await EsAgendaValida(contexto, agenda))
        {
            return false;
        }

        agendaExistente.SolicitudId = agenda.SolicitudId;
        agendaExistente.TecnicoId = agenda.TecnicoId;
        agendaExistente.Fecha = agenda.Fecha;
        agendaExistente.HoraInicio = agenda.HoraInicio;
        agendaExistente.HoraFin = agenda.HoraFin;

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Agendas?>> Buscar(int agendaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Agendas
            .Include(a => a.Solicitud)
                .ThenInclude(s => s.Cliente)
            .Include(a => a.Solicitud)
                .ThenInclude(s => s.EstadoSolicitud)
            .Include(a => a.Solicitud)
                .ThenInclude(s => s.SolicitudesDetalle)
                    .ThenInclude(detalle => detalle.Servicio)
            .FirstOrDefaultAsync(a => a.AgendaId == agendaId && !a.IsDeleted);
    }

    public async Task<Result<List<Agendas>>> GetList(Expression<Func<Agendas, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Agendas
            .Include(a => a.Solicitud)
                .ThenInclude(s => s.Cliente)
            .Include(a => a.Solicitud)
                .ThenInclude(s => s.EstadoSolicitud)
            .Include(a => a.Solicitud)
                .ThenInclude(s => s.SolicitudesDetalle)
                    .ThenInclude(detalle => detalle.Servicio)
            .Where(a => !a.IsDeleted)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result> Eliminar(int agendaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var agenda = await contexto.Agendas
            .Include(a => a.Solicitud)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AgendaId == agendaId && !a.IsDeleted);
        if (agenda == null || agenda.Solicitud.EstadoSolicitudId == 2 || agenda.Solicitud.EstadoSolicitudId == 3 || agenda.Solicitud.EstadoSolicitudId == 9)
        {
            return Result.Success(false);
        }

        var eliminado = await contexto.Agendas
            .Where(a => a.AgendaId == agendaId && !a.IsDeleted)
            .ExecuteUpdateAsync(a => a.SetProperty(agendaItem => agendaItem.IsDeleted, true)) > 0;
        return Result.Success(eliminado);
    }
}
