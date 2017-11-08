using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("CONVENIO")]
    public class CONVENIO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CONVENIO { get; set; }
        public string DESCRICAO { get; set; }
        public int DIA_FATURAMENTO { get; set; }
        public int PRAZO_FATURAMENTO { get; set; }
    }
}