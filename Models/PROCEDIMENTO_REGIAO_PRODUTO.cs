using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("PROCEDIMENTO_REGIAO_PRODUTO")]
    public class PROCEDIMENTO_REGIAO_PRODUTO
    {
        [Key][Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PROCEDIMENTO { get; set; }
        [Key][Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_REGIAO_CORPO { get; set; }
        [Key][Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PRODUTO { get; set; }
        public Nullable<decimal> QUANTIDADE_PADRAO { get; set; }
        public Nullable<int> ID_UNIDADE { get; set; }
        public Nullable<int> ID_FARMACIA { get; set; }
    }
}