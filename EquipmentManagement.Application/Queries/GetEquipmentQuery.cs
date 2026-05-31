using EquipmentManagement.Application.Dtos;
using EquipmentManagement.Shared.Results;
using MediatR;

namespace EquipmentManagement.Application.Queries
{
    public record GetEquipmentQuery(int PageIndex, int PageSize)
        : IRequest<Result<PagedResult<EquipmentDto>>>;

}
