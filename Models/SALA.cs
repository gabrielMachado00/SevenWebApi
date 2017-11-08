using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("SALA")]
    public class SALA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_SALA { get; set; }
        public string DESCRICAO { get; set; }
        public string LOCALIZACAO { get; set; }
    }
}