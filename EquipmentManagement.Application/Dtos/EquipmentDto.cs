
namespace EquipmentManagement.Application.Dtos
{
    public class EquipmentDto
    {
        public Guid id { get; set; }
        public string Nome { get; set; }

        public string NumeroSerie { get; set; }

        public DateTime DataAquisicao { get; set; }

        public string RefNumCertificado { get; set; }

        public int RefSetor { get; set; }
    }
}
