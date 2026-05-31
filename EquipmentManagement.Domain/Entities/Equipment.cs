
using EquipmentManagement.Shared.Results;

namespace EquipmentManagement.Domain.Entities
{
    public class Equipment
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string NumeroSerie { get; private set; } = string.Empty;
        public string RefNumCertificado { get; private set; } = string.Empty;
        public int RefSetor { get; private set; }
        public DateTime DataAquisicao { get; private set; } = DateTime.MinValue;

        #region Propriedades de Auditoria
        /*
        public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;
        public string UsuarioCriacao { get; private set; } = string.Empty;
        public DateTime DataAlteracao { get; private set; } = DateTime.UtcNow;
        public string UsuarioAlteracao { get; private set; } = string.Empty;
        */
        #endregion

        private Equipment()
        {
            Id = Guid.NewGuid();
        }

        public Result<Equipment> Update(string nome, string numeroSerie, DateTime dataAquisicao, string refNumCertificado, int refSetor)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return Result.Fail<Equipment>("Nome is required.");
            if (string.IsNullOrWhiteSpace(numeroSerie))
                return Result.Fail<Equipment>("NumeroSerie is required.");
            if (dataAquisicao == DateTime.MinValue)
                return Result.Fail<Equipment>("DataAquisicao is required.");
            if (string.IsNullOrWhiteSpace(refNumCertificado))
                return Result.Fail<Equipment>("RefNumCertificado is required.");
            if (refSetor <= 0)
                return Result.Fail<Equipment>("RefSetor must be greater than zero.");

            Nome = nome;
            NumeroSerie = numeroSerie;
            DataAquisicao = dataAquisicao;
            RefNumCertificado = refNumCertificado;
            RefSetor = refSetor;

            return Result.Ok(this);
        }

        public static Result<Equipment> Create(string nome, string numeroSerie, DateTime dataAquisicao, string refNumCertificado, int refSetor)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return Result.Fail<Equipment>("Nome is required.");
            if (string.IsNullOrWhiteSpace(numeroSerie))
                return Result.Fail<Equipment>("NumeroSerie is required.");
            if (dataAquisicao == DateTime.MinValue)
                return Result.Fail<Equipment>("DataAquisicao is required.");
            if (string.IsNullOrWhiteSpace(refNumCertificado))
                return Result.Fail<Equipment>("RefNumCertificado is required.");
            if (refSetor <= 0)
                return Result.Fail<Equipment>("RefSetor must be greater than zero.");
            var equipment = new Equipment
            {
                Nome = nome,
                NumeroSerie = numeroSerie,
                DataAquisicao = dataAquisicao,
                RefNumCertificado = refNumCertificado,
                RefSetor = refSetor
            };
            return Result.Ok(equipment);
        }
    }
}
