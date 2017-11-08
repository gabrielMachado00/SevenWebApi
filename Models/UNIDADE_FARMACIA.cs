using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("UNIDADE_FARMACIA")]
    public class UNIDADE_FARMACIA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_FARMACIA { get; set; }
        public int ID_UNIDADE { get; set; }
    }
}