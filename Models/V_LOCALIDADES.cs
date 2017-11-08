using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("V_LOCALIDADES")]
    public class V_LOCALIDADES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CEP { get; set; }
        public string RUA { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
        public string BAIRRO { get; set; }
        public Nullable<int> ID_CIDADE { get; set; }
    }
}