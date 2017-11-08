using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("CAIXA")]
    public class CAIXA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CAIXA { get; set; }
        public string LOGIN { get; set; }
        public Nullable<DateTime> DATA_ABERTURA { get; set; }
        public Nullable<Decimal> VALOR_ABERTURA { get; set; }
        public Nullable<DateTime> DATA_FECHAMENTO { get; set; }
        public Nullable<Decimal> VALOR_FECHAMENTO { get; set; }
        public Nullable<Decimal> VALOR_PAGO_PARCIAL { get; set; }
        public bool CONFERIDO { get; set; }
    }
}