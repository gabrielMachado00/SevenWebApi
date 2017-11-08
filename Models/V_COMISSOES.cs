using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("V_COMISSOES")]
    public class V_COMISSOES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string PROFISSIONAL { get; set; }
        public string DESCRICAO { get; set; }
        public string REGIAO { get; set; }
        public string CLIENTE { get; set; }
        public decimal VALOR_COMISSAO { get; set; }
        public Nullable<decimal> VALOR_BASE { get; set; }
        public Nullable<decimal> VALOR_FATOR { get; set; }
        public Nullable<DateTime> DATA_INCLUSAO { get; set; }
        public Nullable<DateTime> DATA_PAGAMENTO { get; set; }
        public string LOGIN { get; set; }
        public string OBSERVACAO { get; set; }
        public Nullable<DateTime> DATA_VENDA { get; set; }
        public Nullable<decimal> VALOR_TOTAL_FATURADO { get; set; }        
        public string STATUS { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public Nullable<int> ID_PROCEDIMENTO { get; set; }
        public Nullable<int> ID_REGIAO_CORPO { get; set; }
        public Nullable<int> ID_VENDA { get; set; }
        public string FATOR_COMISSAO { get; set; }
        public Nullable<int> ID_ATENDIMENTO { get; set; }
        public int ID_COMISSAO { get; set; }
        public Nullable<int> FORMA_COMISSAO { get; set; }
    }
}