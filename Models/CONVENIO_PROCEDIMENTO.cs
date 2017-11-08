using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("CONVENIO_PROCEDIMENTO")]
    public class CONVENIO_PROCEDIMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CONVENIO { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public System.Decimal VALOR { get; set; }
        public string CODIGO_CONVENIO { get; set; }
    }
}