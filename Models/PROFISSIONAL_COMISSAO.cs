using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("PROFISSIONAL_COMISSAO")]
    public class PROFISSIONAL_COMISSAO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ID_PROFISSIONAL { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public decimal PERCENTUAL { get; set; }
        public decimal VALOR { get; set; }
        public Nullable<int> FORMA_COMISSAO { get; set; }
    }
}