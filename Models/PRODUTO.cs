using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("PRODUTO")]
    public class PRODUTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PRODUTO { get; set; }
        public string DESC_PRODUTO { get; set; }
        public string MARCA { get; set; }
        public int ID_TIPO_PRODUTO { get; set; }
        public decimal PRECO_REAL { get; set; }
        public decimal PRECO_CUSTO { get; set; }
        public DateTime DATA_CADASTRO { get; set; }
        public string DESATIVADO { get; set; }
        public string OBS { get; set; }
        public string UNIDADE { get; set; }
        public decimal QUANTIDADE_MINIMA { get; set; }
        public Nullable<decimal> FATOR_CONVERSAO { get; set; }
        public Nullable<int> ID_UNIDADE { get; set; }
    }
}