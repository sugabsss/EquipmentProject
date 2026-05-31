using AutoMapper;
using EquipmentManagement.Application.Commands;
using EquipmentManagement.Application.Dtos;
using EquipmentManagement.Domain.Entities;
using EquipmentManagement.Domain.Interfaces;
using EquipmentManagement.Shared.Results;
using MediatR;

namespace EquipmentManagement.Application.Handlers.EquipmentHandler
{
    public class InsertEquipmentHandler : IRequestHandler<InsertEquipmentCommand, Result<EquipmentDto>>
    {
        private readonly IEquipmentRepository _repo;
        private readonly IMapper _mapper;

        public InsertEquipmentHandler(IEquipmentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<EquipmentDto>> Handle(InsertEquipmentCommand request, CancellationToken token)
        {
            EquipmentDto DtoRequest = new EquipmentDto
            {
                Nome = request.Nome,
                NumeroSerie = request.NumeroSerie,
                DataAquisicao = request.DataAquisicao,
                RefNumCertificado = request.RefNumCertificado,
                RefSetor = request.RefSetor
            };

            var validateEntity = Equipment.Create(DtoRequest.Nome, DtoRequest.NumeroSerie, DtoRequest.DataAquisicao, DtoRequest.RefNumCertificado, DtoRequest.RefSetor);

            if (!validateEntity.IsSuccess)
            {
                return Result.Fail<EquipmentDto>(validateEntity.Error);
            }

            var result = await _repo.InsertEquipmentAsync(validateEntity.Value, token);

            if (!result.IsSuccess)
            {
                return Result.Fail<EquipmentDto>("Failed to insert equipment.");
            }

            return Result.Ok(_mapper.Map<EquipmentDto>(result.Value));
        }
    }
}
