using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("UNIDADE_PRODUTO")]
    public class UNIDADE_PRODUTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_UNIDADE { get; set; }
        public string DESCRICAO { get; set; }
    }
}