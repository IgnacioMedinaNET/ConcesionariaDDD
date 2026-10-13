using Concesionaria.Domain.AsientosContables;
using Concesionaria.Domain.Contabilidad.AsientosContables.Enum;

namespace Concesionaria.Domain.Interfaces.IRepositories;

public interface IAsientoContableRepository
{
    //TAREA PARA AGREGAR
    Task AddAsync(AsientoContable asiento);

    //TAREA PARA ACTUALIZAR
    Task UpdateAsync(AsientoContable asiento);
    
    //TAREA PARA OBTENER POR ID CON DETALLE
    Task<AsientoContable?> ObtenerPorIdConDetalleAsync(Guid id, Guid empresaId);

    //TAREA PARA OBTENER ACTIVOS SEGUN FILTRO
    Task<List<AsientoContable>> ObtenerActivosAsync(
        Guid empresaId,
        DateTime? desde = null,
        DateTime? hasta = null,
        EstadoAsiento? estado = null,
        OrigenAsiento? origen = null);

    //TAREA PARA OBTENER INACTIVOS SEGUN FILTRO
    Task<List<AsientoContable>> ObtenerInactivosAsync(
        Guid empresaId,
        DateTime? desde = null,
        DateTime? hasta = null);

}