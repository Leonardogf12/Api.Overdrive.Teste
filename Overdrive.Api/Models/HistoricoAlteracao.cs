using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Overdrive.Api.Models
{
    public class HistoricoAlteracao
    {
        [Key]
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public string Status { get; set; }
        public DateTime Data { get; set; }
        public string Usuario { get; set; }
       
        [ForeignKey("IdPessoa")]
        public virtual Pessoa Pessoa { get; set; }     
    }
}
