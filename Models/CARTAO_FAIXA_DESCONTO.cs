using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SevenMedicalApi.Models
{
    [Table("CARTAO_FAIXA_DESCONTO")]
    public class CARTAO_FAIXA_DESCONTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ID_CARTAO_FAIXA_DESCONTO { get; set; }
        public int ID_CARTAO { get; set; }
        public int PARCELA_INICIAL { get; set; }
        public int PARCELA_FINAL { get; set; }
        public System.Decimal DESCONTO { get; set; }
    }
}