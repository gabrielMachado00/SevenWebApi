using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SevenMedicalApi.Models
{
    [Table("TIPO_SERVICO")]
    public class TIPO_SERVICO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPO_SERVICO { get; set; }
        public string DESCRICAO { get; set; }
    }
}