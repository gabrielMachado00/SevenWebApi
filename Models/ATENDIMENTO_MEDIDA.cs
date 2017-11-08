using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("ATENDIMENTO_MEDIDA")]
    public class ATENDIMENTO_MEDIDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_MEDIDA { get; set; }
        public int ID_ATENDIMENTO { get; set; }
        public string LOGIN { get; set; }
        public string VALOR { get; set; }
    }
}