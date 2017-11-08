using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("PROFISSIONAL_RECEITA")]
    public class PROFISSIONAL_RECEITA
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ID_RECEITA { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public string TITULO { get; set; }
        public string DESCRICAO { get; set; }
    }
}
