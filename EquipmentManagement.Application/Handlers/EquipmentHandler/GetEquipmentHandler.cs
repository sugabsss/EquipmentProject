using AutoMapper;
using EquipmentManagement.Application.Dtos;
using EquipmentManagement.Application.Queries;
using EquipmentManagement.Domain.Interfaces;
using EquipmentManagement.Shared.Results;
using MediatR;

public class GetEquipmentHandler
    : IRequestHandler<GetEquipmentQuery, Result<PagedResult<EquipmentDto>>>
{
    private readonly IEquipmentRepository _repo;
    private readonly IMapper _mapper;

    public GetEquipmentHandler(IEquipmentRepository repository, IMapper mapper)
    {
        _repo = repository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<EquipmentDto>>> Handle(
        GetEquipmentQuery request,
        CancellationToken cancellationToken)
    {
        var pagedEquipments = await _repo.GetEquipmentAsync(
            request.PageIndex,
            request.PageSize,
            cancellationToken);

        if (!pagedEquipments.Data.Any())
            return Result.NotFound<PagedResult<EquipmentDto>>("Nenhum equipamento encontrado.");

        return Result.Ok(new PagedResult<EquipmentDto>
        {
            Data = _mapper.Map<IEnumerable<EquipmentDto>>(pagedEquipments.Data),
            PageIndex = pagedEquipments.PageIndex,
            PageSize = pagedEquipments.PageSize,
            TotalCount = pagedEquipments.TotalCount
        });
    }
}