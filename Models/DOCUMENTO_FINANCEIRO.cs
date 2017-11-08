using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("DOCUMENTO_FINANCEIRO")]
    public class DOCUMENTO_FINANCEIRO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DOCUMENTO { get; set; }
        public string TIPO { get; set; }
        public DateTime DATA_ATUALIZACAO { get; set; }
        public string LOGIN { get; set; }
        public Decimal VALOR { get; set; }
        public DateTime DATA_VENCIMENTO { get; set; }
        public string NUM_DOCUMENTO { get; set; }
        public Nullable<int> ID_CENTRO_CUSTO { get; set; }
        public Nullable<int> ID_SUB_CATEGORIA { get; set; }
        public string STATUS { get; set; }
        public Nullable<int> ID_CLIENTE { get; set; }
        public Nullable<int> ID_ITEM_CAIXA { get; set; }
        public Nullable<int> ID_FORNECEDOR { get; set; }
        public Decimal SALDO { get; set; }
        public Nullable<int> ID_EQUIPAMENTO { get; set; }
        public Nullable<int> ID_CONTA { get; set; }
        public Nullable<int> ID_CATEGORIA { get; set; }
        public Nullable<int> TIPO_PGTO { get; set; }
        public Nullable<int> NUM_PARCELA { get; set; }
        public Nullable<DateTime> DATA_QUITACAO { get; set; }
        public string OBS { get; set; }
        public Nullable<int> ID_PROFISSIONAL { get; set; }
        public Nullable<int> ID_CARTAO { get; set; }
        public Nullable<int> CV_CARTAO { get; set; }
        public Nullable<int> ID_BANCO_CHEQUE { get; set; }
        public string CONTA_CHEQUE { get; set; }
        public Nullable<DateTime> DATA_COMPETENCIA { get; set; }
        public string NUM_CHEQUE { get; set; }
        public string AGENCIA { get; set; }
        public Nullable<DateTime> DATA_INCLUSAO { get; set; }
        public Nullable<int> ID_ENTRADA { get; set; }
        public Nullable<int> ID_CONVENIO { get; set; }
    }
}