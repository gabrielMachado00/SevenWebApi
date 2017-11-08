using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SevenMedicalApi.Models
{
    [Table("MEDICAMENTO")]
    public class MEDICAMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_MEDICAMENTO { get; set; }
        public string NOME { get; set; }
        public string QUANTIDADE { get; set; }
        public string USO { get; set; }
        public string APRESENTACAO { get; set; }
        public string PRESCRICAO { get; set; }
        public string OBS { get; set; }
    }
}