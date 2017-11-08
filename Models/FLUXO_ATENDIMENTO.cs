using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SevenMedicalApi.Models
{
    [Table("FLUXO_ATENDIMENTO")]
    public class FLUXO_ATENDIMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_APPOINTMENT { get; set; }
        public int ID_CLIENTE { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public DateTime DATA_HORA_INI { get; set; }
        public Nullable<System.DateTime> DATA_HORA_FIM { get; set; }
        public string STATUS { get; set; }
    }
}