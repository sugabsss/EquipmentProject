using AutoMapper;
using EquipmentManagement.Application.Commands;
using EquipmentManagement.Application.Dtos;
using EquipmentManagement.Domain.Interfaces;
using EquipmentManagement.Shared.Results;
using MediatR;

namespace EquipmentManagement.Application.Handlers.EquipmentHandler
{
    public class UpdateEquipmentHandler : IRequestHandler<UpdateEquipmentCommand, Result<EquipmentDto>>
    {
        private readonly IEquipmentRepository _repo;
        private readonly IMapper _mapper;

        public UpdateEquipmentHandler(IEquipmentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<EquipmentDto>> Handle(UpdateEquipmentCommand request, CancellationToken token)
        {
            var result = await _repo.UpdateEquipmentAsync(
                request.Id, request.Nome, request.NumeroSerie, request.DataAquisicao, request.RefNumCertificado, request.RefSetor, token);

            if (!result.IsSuccess || result.Value is null)
                return Result.Fail<EquipmentDto>(result.Error);

            return Result.Ok(_mapper.Map<EquipmentDto>(result.Value));
        }
    }
}
