using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("CATEGORIA")]
    public class CATEGORIA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CATEGORIA { get; set; }
        public string DESCRICAO { get; set; }
        public string TIPO { get; set; }
        public string SUB_TIPO { get; set; }
    }
}