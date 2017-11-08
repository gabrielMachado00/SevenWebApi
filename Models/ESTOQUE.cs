using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("ESTOQUE")]
    public class ESTOQUE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ESTOQUE { get; set; }
        public int ID_PRODUTO { get; set; }
        public decimal QUANTIDADE { get; set; }
        public int ID_UNIDADE { get; set; }
    }
}