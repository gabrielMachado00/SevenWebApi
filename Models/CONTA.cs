using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisMedApi.Models
{
    [Table("CONTA")]
    public class CONTA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CONTA { get; set; }
        public string DESCRICAO { get; set; }
        public Nullable<int> ID_UNIDADE { get; set; }
        public decimal SALDO_INICIAL { get; set; }
    }
}
