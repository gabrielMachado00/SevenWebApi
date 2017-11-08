using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("PROCEDIMENTO_REGIAO_CORPO")]
    public class PROCEDIMENTO_REGIAO_CORPO
    {
        [Key][Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PROCEDIMENTO { get; set; }
        [Key][Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_REGIAO_CORPO { get; set; }
        [Key][Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NUM_SESSOES { get; set; }
        public decimal VALOR { get; set; }
        public TimeSpan TEMPO { get; set; }
        public string OBS { get; set; }
        public bool MOVIMENTA_ESTOQUE { get; set; }
        public bool ATIVO { get; set; }
    }
}