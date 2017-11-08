using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("ATENDIMENTO_ANAMNESE")]
    public class ATENDIMENTO_ANAMNESE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ATENDIMENTO { get; set; }
        public int ID_ANAMNESE { get; set; }
        public string DESCRICAO { get; set; }
        public string LOGIN { get; set; }
    }
}