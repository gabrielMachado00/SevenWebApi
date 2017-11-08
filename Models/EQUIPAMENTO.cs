using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("EQUIPAMENTO")]
    public class EQUIPAMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_EQUIPAMENTO { get; set; }
        public string DESCRICAO { get; set; }
        public string TIPO { get; set; }
        public string NUM_SERIE { get; set; }
        public Nullable<decimal> VALOR_COMPRA { get; set; }
        public Nullable<int> ANO { get; set; }
        public Nullable<int> ANO_COMPRA { get; set; }
        public string ANVISA { get; set; }
        public string ACESSORIOS_COMPONENTES { get; set; }
        public Nullable<int> QUANTIDADE { get; set; }
        public Nullable<int> TEMPO_DEPRECIACAO { get; set; }

    }
}