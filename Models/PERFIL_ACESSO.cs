using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("PERFIL_ACESSO")]
    public class PERFIL_ACESSO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PERFIL { get; set; }
        public int ID_ACESSO { get; set; }
    }
}