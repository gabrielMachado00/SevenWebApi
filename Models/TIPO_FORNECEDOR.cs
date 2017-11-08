using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("TIPO_FORNECEDOR")]
    public class FORNECEDOR_TIPO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TP_FORNECEDOR { get; set; }
        public string TIPO_FORNECEDOR { get; set; }
    }
}