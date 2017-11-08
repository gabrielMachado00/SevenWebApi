using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("PROFISSIONAL_HORARIO")]
    public class PROFISSIONAL_HORARIO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ID_HORARIO_PROFISSIONAL { get; set; }
        public int COD_DIA_SEMANA { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public TimeSpan HORA_INICIAL_1 { get; set; }
        public TimeSpan HORA_FINAL_1 { get; set; }
        public TimeSpan HORA_INICIAL_2 { get; set; }
        public TimeSpan HORA_FINAL_2 { get; set; }
        public TimeSpan HORA_INICIAL_3 { get; set; }
        public TimeSpan HORA_FINAL_3 { get; set; }        
    }
}