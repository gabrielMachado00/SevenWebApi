using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("ENDERECOS_FORNECEDORES")]
    public class FORNECEDOR_ENDERECO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ENDERECO { get; set; }
        public int ID_FORNECEDOR { get; set; }
        public string ENDERECO { get; set; }
        public string BAIRRO { get; set; }
        public string CEP { get; set; }
        public int ID_CIDADE { get; set; }
        public string TIPO_ENDERECO { get; set; }
    }
}