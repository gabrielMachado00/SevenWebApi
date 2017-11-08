using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("PRODUTO_CONVERSAO")]
    public class PRODUTO_CONVERSAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CONVERSAO { get; set; }
        public int ID_PRODUTO { get; set; }
        public string UNIDADE { get; set; }
        public decimal VALOR { get; set; }
        public Nullable<int> ID_UNIDADE { get; set; }
    }
}