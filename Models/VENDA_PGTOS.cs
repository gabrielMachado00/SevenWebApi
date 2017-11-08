using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("VENDA_PGTOS")]
    public class VENDA_PGTOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ITEM_PGTO_VENDA { get; set; }
        public int ID_VENDA { get; set; }
        public Nullable<int> TIPO_PGTO { get; set; }
        public string DESCRICAO { get; set; }
        public int NUM_PARCELAS { get; set; }
        public DateTime PRIMEIRO_VCTO { get; set; }
        public decimal VALOR { get; set; }
        public Nullable<decimal> VALOR_DESCONTO { get; set; }
        public Nullable<int> ID_CONVENIO { get; set; }
        public Nullable<decimal> VALOR_ACRESCIMO { get; set; }
        public Nullable<int> ID_CARTAO { get; set; }
        public string STATUS { get; set; }
        public Nullable<decimal> VALOR_PAGO_PARCIAL { get; set; }
    }
}