using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("APPOINTMENTS_REGIAO_CORPO")]
    public class APPOINTMENTS_REGIAO_CORPO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_APPOINTMENTS { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public int FATURADO { get; set; }
    }
}