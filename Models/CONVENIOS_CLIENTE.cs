using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("CONVENIOS_CLIENTE")]
    public class CONVENIOS_CLIENTE
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CONVENIO { get; set; }
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CLIENTE { get; set; }
        public string NUMERO_CONVENIO { get; set; }
        public string ABRANGENCIA { get; set; }
        public Nullable<DateTime> VIGENCIA { get; set; }
        public DateTime VALIDADE { get; set; }
        public string ACOMODACAO { get; set; }
        public string PRODUTO { get; set; }
        public string CONTRATANTE { get; set; }
     }
}