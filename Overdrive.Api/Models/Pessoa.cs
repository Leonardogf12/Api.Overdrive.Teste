using System.ComponentModel.DataAnnotations;

namespace Overdrive.Api.Models
{
    public class Pessoa
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string Telefone { get; set; }
        public string Usuario { get; set; }
        public string Status { get; set; }

        public enum StatusPessoa
        {
            Pendente,
            Ativo,
            Inativo
        }

        
    }
}
