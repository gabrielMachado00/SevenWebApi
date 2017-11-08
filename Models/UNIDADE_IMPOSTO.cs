using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SevenMedicalApi.Models
{
   
    [Table("UNIDADE_IMPOSTO")]
    public class UNIDADE_IMPOSTO
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_UNIDADE_IMPOSTO { get; set; }
        public int ID_UNIDADE { get; set; }
        public string DESCRICAO { get; set; }
        public decimal PERCENTUAL { get; set; }
    }
}