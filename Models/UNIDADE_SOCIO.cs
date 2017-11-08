using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("UNIDADE_SOCIO")]
    public class UNIDADE_SOCIO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_SOCIO { get; set; }
        public int ID_UNIDADE { get; set; }
        public string NOME { get; set; }
        public Nullable<decimal> COTA { get; set; }
        public string CELULAR { get; set; }
        public string TELEFONE { get; set; }
        public string EMAIL { get; set; }
    }
}