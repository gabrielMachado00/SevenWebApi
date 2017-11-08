using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SevenMedicalApi.Models
{
    [Table("DISEASES")]
    public class DISEASES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DISEASES { get; set; }
        public string CODIGO { get; set; }
        public string NOME { get; set; }
    }
}