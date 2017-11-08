using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("PRODUTO_FORNECEDORES")]
    public class PRODUTO_FORNECEDORES
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key][Column(Order = 0)]
        public int ID_FORNECEDOR { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key][Column(Order = 1)]
        public int ID_PRODUTO { get; set; }
        public Decimal PRECO_MEDIO { get; set; }
        public string OBS { get; set; }
    }
}