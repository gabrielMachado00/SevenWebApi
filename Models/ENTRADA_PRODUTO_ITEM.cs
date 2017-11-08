using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("ENTRADA_PRODUTO_ITEM")]
    public class ENTRADA_PRODUTO_ITEM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ITEM_ENTRADA { get; set; }
        public int ID_ENTRADA { get; set; }
        public int ID_PRODUTO { get; set; }
        public Nullable<int> ID_FARMACIA { get; set; }
        public decimal QUANTIDADE { get; set; }
        public decimal VALOR { get; set; }
        public string UNIDADE_ENTRADA { get; set; }
        public Nullable<int> ID_UNIDADE { get; set; }
    }
}