using EquipmentManagement.Application.Dtos;
using EquipmentManagement.Application.Queries;
using MediatR;
using EquipmentManagement.Shared.Results;
using EquipmentManagement.Domain.Interfaces;
using AutoMapper;

namespace EquipmentManagement.Application.Handlers.EquipmentHandler
{
    public class GetEquipmentByIdHandler :
            IRequestHandler<GetEquipmentByIdQuery, Result<EquipmentDto>>
    {
        public readonly IEquipmentRepository _repo;
        public readonly IMapper _mapper;

        public GetEquipmentByIdHandler(IEquipmentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<EquipmentDto>> Handle(
            GetEquipmentByIdQuery request,
            CancellationToken cancellationToken)
        {
            var equipamento = await _repo.GetEquipmentByIdAsync(request.Id, cancellationToken);

            if (!equipamento.IsSuccess || equipamento.Value is null)
                return Result.NotFound<EquipmentDto>($"Nenhum Equipamento encontrado para {request.Id}");

            return Result.Ok(_mapper.Map<EquipmentDto>(equipamento.Value));
        }
    }
}
