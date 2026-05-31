using EquipmentManagement.Domain.Entities;
using EquipmentManagement.Domain.Interfaces;
using EquipmentManagement.Infra.Context;
using EquipmentManagement.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace EquipmentManagement.Infra.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly AppDbContext _appDbContext;

        public EquipmentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<PagedResult<Equipment>> GetEquipmentAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var query = _appDbContext.Equipaments.AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var data = await query
                .OrderBy(e => e.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Equipment>
            {
                Data = data,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<Result<Equipment?>> GetEquipmentByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _appDbContext.Equipaments
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<Result<Equipment>> InsertEquipmentAsync(Equipment equipment, CancellationToken cancellationToken)
        {
            await _appDbContext.Equipaments.AddAsync(equipment, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return equipment;
        }

        public async Task<Result<Equipment?>> UpdateEquipmentAsync(Guid id, string nome, string numeroSerie, DateTime dataAquisicao, string refNumCertificado, int refSetor, CancellationToken cancellationToken)
        {
            var equipment = await _appDbContext.Equipaments
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (equipment is null)
                return Result.Fail<Equipment>("Equipment not found.");

            var updateResult = equipment.Update(nome, numeroSerie, dataAquisicao, refNumCertificado, refSetor);

            if (!updateResult.IsSuccess)
                return Result.Fail<Equipment>(updateResult.Error);

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return equipment;
        }

        public async Task<bool> DeleteEquipmentAsync(Guid id, CancellationToken cancellationToken)
        {
            var equipment = await _appDbContext.Equipaments
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (equipment is null)
                return false;

            _appDbContext.Equipaments.Remove(equipment);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}