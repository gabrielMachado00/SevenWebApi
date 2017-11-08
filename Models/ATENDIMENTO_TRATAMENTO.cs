using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("ATENDIMENTO_TRATAMENTO")]
    public class ATENDIMENTO_TRATAMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TRATAMENTO { get; set; }
        public int ID_ATENDIMENTO { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public string LOGIN { get; set; }
        public Nullable<int> ID_REGIAO_CORPO { get; set; }
        public Nullable<int> NUM_SESSOES { get; set; }
        public Nullable<decimal> VALOR_NEGOCIADO { get; set; }
    }
}