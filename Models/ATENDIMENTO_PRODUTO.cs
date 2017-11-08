using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("ATENDIMENTO_PRODUTO")]
    public class ATENDIMENTO_PRODUTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ATENDIMENTO { get; set; }
        public int ID_PRODUTO { get; set; }
        public string LOGIN { get; set; }
        public System.Decimal QUANTIDADE { get; set; }
        public string UNIDADE { get; set; }
        public string OBS { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
    }
}