using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("CONVENIO_DEDUCAO")]
    public class CONVENIO_DEDUCAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DEDUCAO { get; set; }
        public int ID_CONVENIO { get; set; }
        public string DESCRICAO { get; set; }
        public System.Decimal PERCENTUAL { get; set; }
    }
}