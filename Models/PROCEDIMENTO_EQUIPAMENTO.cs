using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("PROCEDIMENTO_EQUIPAMENTO")]
    public class PROCEDIMENTO_EQUIPAMENTO
    {
        [Key][Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PROCEDIMENTO { get; set; }
        [Key][Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_EQUIPAMENTO { get; set; }
        public Nullable<System.Decimal> VALOR { get; set; }
    }
}