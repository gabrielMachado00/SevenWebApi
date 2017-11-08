using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("CARTAO")]
    public class CARTAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CARTAO { get; set; }
        public string BANDEIRA { get; set; }
        public int NUM_DIAS_VENCIMENTO { get; set; }
        public string TIPO { get; set; }
    }
}