using EquipmentManagement.Shared.Results;
using MediatR;

namespace EquipmentManagement.Application.Commands
{
    public record DeleteEquipmentCommand(Guid Id) : IRequest<Result<bool>>
    {
    }
}
