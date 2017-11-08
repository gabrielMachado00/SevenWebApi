using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("PROFISSIONAL")]
    public class PROFISSIONAL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PROFISSIONAL { get; set; }
        public string NOME { get; set; }
        public string TITULO { get; set; }
        public string ESPECIALIDADE { get; set; }
        public System.DateTime? DATA_CADASTRO { get; set; }
        public string TELEFONE { get; set; }
        public string CELULAR { get; set; }
        public string EMAIL { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public System.DateTime? DATA_NASCIMENTO { get; set; }
        public string SEXO { get; set; }
        public string ESTADO_CIVIL { get; set; }
        public string REGISTRO { get; set; }
        public Nullable<int> ID_SALA { get; set; }
        public Nullable<bool> STATUS { get; set; }
        public Nullable<bool> EXIBE_AGENDA { get; set; }
        public Nullable<int> TEMPO_PADRAO { get; set; }
    }
}