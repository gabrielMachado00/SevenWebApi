using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("USUARIO")]
    public class USUARIO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_USUARIO { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }
        public Nullable<int> ID_PROFISSIONAL { get; set; }
        public int TIPO_ACESSO { get; set; }
        public bool MASTER { get; set; }
        public Nullable<int> ID_PERFIL { get; set; }
        public Nullable<decimal> PERCENTUAL_COMISSAO { get; set; }
        public string EMAIL_SENHA { get; set; }
        public string STATUS { get; set; }
    }
}