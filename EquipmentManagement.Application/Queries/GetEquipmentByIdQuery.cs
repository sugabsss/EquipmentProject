using EquipmentManagement.Application.Dtos;
using EquipmentManagement.Shared.Results;
using MediatR;


namespace EquipmentManagement.Application.Queries
{
    public record GetEquipmentByIdQuery(Guid Id)
        : IRequest<Result<EquipmentDto>>;
}
