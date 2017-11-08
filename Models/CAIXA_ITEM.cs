using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("CAIXA_ITEM")]
    public class CAIXA_ITEM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ITEM_CAIXA { get; set; }
        public int ID_CAIXA { get; set; }
        public Nullable<int> ID_ITEM_PGTO_VENDA { get; set; }
        public string DEBITO_CREDITO { get; set; }
        public string DESCRICAO { get; set; }
        public string NUM_DOCUMENTO { get; set; }
        public Nullable<Decimal> VALOR { get; set; }
        public int TIPO_PGTO_CAIXA_ITEM { get; set; }
        public Nullable<int> ID_CENTRO_CUSTO { get; set; }
        public Nullable<int> ID_CATEGORIA { get; set; }
        public Nullable<int> ID_SUB_CATEGORIA { get; set; }
        public Nullable<int> ID_CONTA { get; set; }
        public string STATUS { get; set; }
        public Nullable<int> CV_CARTAO { get; set; }
        public string NUM_CHEQUE { get; set; }
        public Nullable<DateTime> DATA_COMPETENCIA { get; set; }
        public Nullable<Decimal> VALOR_PAGO_PARCIAL { get; set; }
        public Nullable<DateTime> DATA_VENCIMENTO { get; set; }

        public Nullable<DateTime> DATA_ABERTURA { get; set; }
        public Nullable<DateTime> DATA_FECHAMENTO { get; set; }
        public string LOGIN { get; set; }
        public decimal VALOR_ABERTURA { get; set; }
        public decimal VALOR_FECHAMENTO { get; set; }
    }
}