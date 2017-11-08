using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SevenMedicalApi.Models
{
    [Table("ATENDIMENTO_PROCEDIMENTO")]
    public class ATENDIMENTO_PROCEDIMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ATENDIMENTO_PROCEDIMENTO { get; set; }
        public int ID_ATENDIMENTO { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public Nullable<int> ID_REGIAO_CORPO { get; set; }
        public Nullable<int> ID_EQUIPAMENTO { get; set; }
        public Nullable<int> ID_APPOINTMENT { get; set; }
        public string LOGIN { get; set; }
        public string STATUS { get; set; }
    }
}