using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("CIDADE")]
    public class CIDADE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CIDADE { get; set; }
        public string NOME { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
    }
}