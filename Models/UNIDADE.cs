using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisMedApi.Models
{
    [Table("UNIDADE")]
    public class UNIDADE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_UNIDADE { get; set; }
        public string NOME_FANTASIA { get; set; }
        public string CNPJ { get; set; }
        public string RAZAO_SOCIAL { get; set; }
        public string TELEFONE { get; set; }
        public string EMAIL { get; set; }
        public string CONTATO { get; set; }
        public bool EXIBE_AGENDA { get; set; }
        public string CPF { get; set; }
    }
}
