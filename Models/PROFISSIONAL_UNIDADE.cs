using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("PROFISSIONAL_UNIDADE")]
    public class PROFISSIONAL_UNIDADE
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PROFISSIONAL { get; set; }
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_UNIDADE { get; set; }
    }
}