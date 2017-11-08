using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("SUB_CATEGORIA")]
    public class SUB_CATEGORIA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_SUB_CATEGORIA { get; set; }
        public int ID_CATEGORIA { get; set; }
        public string DESCRICAO { get; set; }
        public Nullable<int> ID_CENTRO_CUSTO { get; set; }
        public Nullable<bool> COMPOR_TAXA_SALA { get; set; }
        public Nullable<bool> COMPOR_TAXA_EQUIPAMENTO { get; set; }
    }
}