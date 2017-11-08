using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("ANAMNESE")]
    public class ANAMNESE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ANAMNESE { get; set; }
        public string TITULO { get; set; }
        public string DESCRICAO { get; set; }
        public int ID_PROFISSIONAL { get; set; }
    }
}