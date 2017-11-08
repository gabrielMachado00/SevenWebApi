using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisMedApi.Models
{
    [Table("CENTRO_CUSTO")]
    public class CENTRO_CUSTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CENTRO_CUSTO { get; set; }
        public string DESCRICAO { get; set; }
    }
}
