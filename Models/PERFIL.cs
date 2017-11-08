using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("PERFIL")]
    public class PERFIL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PERFIL { get; set; }
        public string DESCRICAO { get; set; }
        public int ORDEM_HIERARQUICA { get; set; }
    }
}