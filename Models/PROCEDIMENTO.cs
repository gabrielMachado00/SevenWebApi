using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("PROCEDIMENTO")]
    public class PROCEDIMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PROCEDIMENTO { get; set; }
        public string DESCRICAO { get; set; }
        public string COR { get; set; }
        public int ID_TIPO_SERVICO { get; set; }
        public bool MOVIMENTA_ESTOQUE { get; set; }
        public bool CRP { get; set; }
    }
}