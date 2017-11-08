using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("ATENDIMENTO_RECEITA")]
    public class ATENDIMENTO_RECEITA
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ATENDIMENTO_RECEITA { get; set; }
        public Nullable<int> ID_ATENDIMENTO { get; set; }
        public Nullable<int> ID_RECEITA { get; set; }
        public string LOGIN { get; set; }
        public string DESCRICAO { get; set; }
        public DateTime DATA { get; set; }
        public Nullable<int> ID_CLIENTE { get; set; }
        public string TITULO { get; set; }
    }
}