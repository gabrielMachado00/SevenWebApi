using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("REGIAO_CORPO_ALERTA")]
    public class REGIAO_CORPO_ALERTA
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_REGIAO_CORPO_ALERTA { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public int STATUS_ATIVAR { get; set; }
        public int INTERVALO { get; set; }
        public string DESCRICAO { get; set; }
    }
}