using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("VENDA_ITEM")]
    public class VENDA_ITEM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ITEM_VENDA { get; set; }
        public int ID_VENDA { get; set; }
        public Nullable<int> ID_APPOINTMENT { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public Nullable<int> ID_REGIAO_CORPO { get; set; }
        public Nullable<int> NUM_SESSOES { get; set; }
        public decimal VALOR { get; set; }
        public Nullable<DateTime> DATA_ATUALIZACAO { get; set; }
        public Nullable<int> ID_ATENDIMENTO { get; set; }     
        public Nullable<int> ID_ATENDIMENTO_PROCEDIMENTO { get; set; }
        public Nullable<int> ID_PROFISSIONAL { get; set; }
        public bool PACOTE { get; set; }
        public Nullable<decimal> VALOR_PAGO { get; set; }
        public Nullable<int> NUM_PACOTES { get; set; }
        public Nullable<int> NUM_SESSOES_REALIZADAS { get; set; }
    }
}