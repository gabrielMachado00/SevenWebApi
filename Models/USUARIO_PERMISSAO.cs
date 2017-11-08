using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SevenMedicalApi.Models
{
    [Table("USUARIO_PERMISSAO")]
    public class USUARIO_PERMISSAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PERMISSAO { get; set; }
        public int ID_USUARIO { get; set; }
        public string DESCRICAO { get; set; }
        public bool PERMITE { get; set; }
    }
}