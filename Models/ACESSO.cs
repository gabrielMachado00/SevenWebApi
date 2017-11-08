using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("ACESSO")]
    public class ACESSO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ACESSO { get; set; }
        public string DESCRICAO { get; set; }
        public int CODIGO_ACESSO { get; set; }
    }
}