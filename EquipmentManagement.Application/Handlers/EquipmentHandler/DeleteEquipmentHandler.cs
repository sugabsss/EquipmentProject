using EquipmentManagement.Application.Commands;
using EquipmentManagement.Domain.Interfaces;
using EquipmentManagement.Shared.Results;
using MediatR;

namespace EquipmentManagement.Application.Handlers.EquipmentHandler
{
    public class DeleteEquipmentHandler : IRequestHandler<DeleteEquipmentCommand, Result<bool>>
    {
        private readonly IEquipmentRepository _repo;
        public DeleteEquipmentHandler(IEquipmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<bool>> Handle(DeleteEquipmentCommand request, CancellationToken token)
        {
            var equipment = await _repo.GetEquipmentByIdAsync(request.Id, token);

            if (!equipment.IsSuccess || equipment.Value is null)
                return Result.NotFound<bool>("Equipamento não encontrado.");

            await _repo.DeleteEquipmentAsync(request.Id, token);
            return Result.Ok(true);
        }
    }
}
