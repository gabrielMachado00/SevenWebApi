using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("FARMACIA_ESTOQUE")]
    public class FARMACIA_ESTOQUE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_FARMACIA { get; set; }
        public int ID_PRODUTO { get; set; }
        public decimal QUANTIDADE { get; set; }
        public int ID_UNIDADE { get; set; }
    }
}