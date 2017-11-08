using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("ENDERECOS_CLIENTES")]
    public class CLIENTE_ENDERECO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ENDERECO { get; set; }
        public int ID_CLIENTE { get; set; }
        public string ENDERECO { get; set; }
        public string BAIRRO { get; set; }
        public string CEP { get; set; }
        public Nullable<int> ID_CIDADE { get; set; }
        public string TIPO_ENDERECO { get; set; }
    }
}