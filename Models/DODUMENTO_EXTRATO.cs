using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("DOCUMENTO_EXTRATO")]
    public class DOCUMENTO_EXTRATO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_EXTRATO { get; set; }
        public int ID_DOCUMENTO { get; set; }
        public decimal VALOR { get; set; }
        public DateTime DATA { get; set; }
        public string LOGIN { get; set; }
        public Nullable<decimal> DESCONTO { get; set; }
        public int ID_CONTA { get; set; }

    }
}