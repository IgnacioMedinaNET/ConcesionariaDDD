using System.Collections.Generic;
using System.Threading.Tasks;
using Concesionaria.Domain.Common.Enums;

namespace Concesionaria.Domain.Interfaces.IRepositories
{
    public interface IVendedorRepository
    {
        // TAREA PARA AGREGAR
        Task AddAsync(Vendedor vendedor);

        // TAREA PARA ACTUALIZAR
        Task UpdateAsync();

        // TAREA PARA OBTENER ACTIVAS
        Task<List<Vendedor>> ObtenerActivasAsync(Guid empresaId, string filtro = null);

        // TAREA PARA OBTENER INACTIVAS
        Task<List<Vendedor>> ObtenerInactivasAsync(Guid empresaId, string filtro = null);

        // TAREA PARA EXTIENCIA AGREGAR
        Task<EstadoExistencia> ExistePorDniAsync(string dni, Guid empresaId);
        Task<EstadoExistencia> ExistePorEmailAsync(string email, Guid empresaId);

        // TAREA PARA EXISTENCIA ACTUALIZAR
        Task<EstadoExistencia> ExistePorDniExcluyendoIdAsync(
            string dni,
            Guid empresaId,
            Guid vendedorId
        );
    
        Task<EstadoExistencia> ExistePorEmailExcluyendoIdAsync(
            string email,
            Guid empresaId,
            Guid vendedorId
        );
    }
}
