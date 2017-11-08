using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisMedApi.Models
{
    [Table("BANCO")]
    public class BANCO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_BANCO { get; set; }
        public string NOME { get; set; }
        public string NUMERO { get; set; }
    }
}
