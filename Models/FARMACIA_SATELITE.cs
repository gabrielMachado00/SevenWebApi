using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("FARMACIA_SATELITE")]
    public class FARMACIA_SATELITE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_FARMACIA { get; set; }
        public string DESCRICAO { get; set; }
        public string LOCALIZACAO { get; set; }
        public string RAMAL { get; set; }
        public string RESPONSAVEL { get; set; }
        public Nullable<int> ID_UNIDADE { get; set;}
    }
}