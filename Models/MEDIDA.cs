using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("MEDIDA")]
    public class MEDIDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_MEDIDA { get; set; }
        public string DESCRICAO { get; set; }
        public string UNIDADE { get; set; }
    }
}