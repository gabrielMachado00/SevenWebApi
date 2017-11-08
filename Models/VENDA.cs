using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("VENDA")]
    public class VENDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_VENDA { get; set; }
        public int ID_CLIENTE { get; set; }
        public DateTime DATA { get; set; }
        public Decimal VALOR { get; set; }
        public string LOGIN { get; set; }
        public Nullable<DateTime> DATA_ATUALIZACAO { get; set; }
        public string STATUS { get; set; }
        public Nullable<bool> COMISSAO_GERADA { get; set; }
        public Nullable<DateTime> DATA_COMISSAO_GERADA { get; set; }
        public string LOGIN_COMISSAO_GERADA { get; set; }
        public Nullable<int> ID_USUARIO { get; set; }
    }
}