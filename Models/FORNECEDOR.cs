using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("FORNECEDOR")]
    public class FORNECEDOR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_FORNECEDOR { get; set; }
        public string RAZAO_SOCIAL { get; set; }
        public string CPF { get; set; }
        public string NOME_FANTASIA { get; set; }
        public string TELEFONE { get; set; }
        public string FAX { get; set; }
        public string CELULAR { get; set; }
        public string OBS { get; set; }
        public string EMAIL { get; set; }
        public string INSCRICAO_ESTADUAL { get; set; }
        public System.DateTime DATA_CADASTRO { get; set; }      
        public string CNPJ { get; set; }
    }
}