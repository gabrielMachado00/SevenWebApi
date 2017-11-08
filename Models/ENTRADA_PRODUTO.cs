using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("ENTRADA_PRODUTO")]
    public class ENTRADA_PRODUTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ENTRADA { get; set; }
        public int ID_FORNECEDOR { get; set; }
        public int ID_UNIDADE { get; set; }
        public DateTime DATA_ENTRADA { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public string STATUS { get; set; }
        public string LOGIN { get; set; }
    }
}