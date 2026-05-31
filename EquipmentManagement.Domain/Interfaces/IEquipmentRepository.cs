using EquipmentManagement.Domain.Entities;
using EquipmentManagement.Shared.Results;

namespace EquipmentManagement.Domain.Interfaces
{
    public interface IEquipmentRepository
    {
        Task<PagedResult<Equipment>> GetEquipmentAsync(int pagesize, int pageindex, CancellationToken cancellationToken);
        Task<Result<Equipment?>> GetEquipmentByIdAsync(Guid Id, CancellationToken cancellationToken);
        Task<Result<Equipment>> InsertEquipmentAsync(Equipment equipment, CancellationToken cancellationToken);
        Task<Result<Equipment?>> UpdateEquipmentAsync(Guid id, string nome, string numeroSerie, DateTime dataAquisicao, string refNumCertificado, int refSetor, CancellationToken cancellationToken);

        Task<bool> DeleteEquipmentAsync(Guid Id, CancellationToken cancellationToken);
    }
}
