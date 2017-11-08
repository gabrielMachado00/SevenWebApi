using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("PROTOCOLO")]
    public class PROTOCOLO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PROTOCOLO { get; set; }
        public string TITULO { get; set; }
        public string DESCRICAO { get; set; }
    }
}