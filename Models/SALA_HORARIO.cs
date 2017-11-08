using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("SALA_HORARIO")]
    public class SALA_HORARIO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ID_SALA_HORARIO { get; set; }
        public int COD_DIA_SEMANA { get; set; }
        public int ID_SALA { get; set; }
        public TimeSpan HORA_INICIAL_1 { get; set; }
        public TimeSpan HORA_FINAL_1 { get; set; }
        public TimeSpan HORA_INICIAL_2 { get; set; }
        public TimeSpan HORA_FINAL_2 { get; set; }
        public TimeSpan HORA_INICIAL_3 { get; set; }
        public TimeSpan HORA_FINAL_3 { get; set; }
    }
}