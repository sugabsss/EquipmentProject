using EquipmentManagement.Application.Dtos;
using EquipmentManagement.Shared.Results;
using MediatR;

namespace EquipmentManagement.Application.Commands
{
    public record UpdateEquipmentCommand(Guid Id) : IRequest<Result<EquipmentDto>>
    {
        public string Nome { get; set; }
        public string NumeroSerie { get; set; }
        public DateTime DataAquisicao { get; set; }
        public string RefNumCertificado { get; set; }
        public int RefSetor { get; set; }
    };

}
