using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("CLIENTE")]
    public class CLIENTE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CLIENTE { get; set; }
        public string NOME { get; set; }
        public DateTime DATA_CADASTRO { get; set; }
        public string TELEFONE { get; set; }
        public string TELEFONE2 { get; set; }
        public string FAX { get; set; }
        public string CELULAR { get; set; }
        public string CONTATO { get; set; }
        public string OBS { get; set; }
        public string EMAIL { get; set; }
        public string TIPO_PESSOA { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string SEGMENTO { get; set; }
        public string RAMO { get; set; }
        public string RAZAO_SOCIAL { get; set; }
        public Nullable<DateTime> DESDE { get; set; }
        public string CNPJ { get; set; }
        public Nullable<DateTime> DATA_NASCIMENTO { get; set; }
        public string INDICADO { get; set; }
        public string SEXO { get; set; }
        public string COR_PELE { get; set; }
        public string ESTADO_CIVIL { get; set; }
        public string NATURALIDADE { get; set; }
        public string PROFISSAO { get; set; }
        public string NOME_MAE { get; set; }
        public string NOME_PAI { get; set; }
        public bool BASICO { get; set; }
        public string CLASSIFICACAO { get; set; }
        public Nullable<int> ID_INDICACAO { get; set; }
        public string ALERTA_CLINICO { get; set; }
        public string INDICACAO_OUTRO { get; set; }
        public bool MENOR { get; set; }
        public string RESPONSAVEL { get; set; }
        public string CPF_RESPONSAVEL { get; set; }
        public string DIR_FOTO { get; set; }
        public string NOME_PREFERENCIA { get; set; }
    }
}