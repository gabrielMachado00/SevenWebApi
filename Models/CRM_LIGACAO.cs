using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("CRM_LIGACAO")]
    public class CRM_LIGACAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CRM_LIGACAO { get; set; }
        public int ID_APPOINTMENT { get; set; }
        public int ID_REGIAO_CORPO_ALERTA { get; set; }
        public DateTime DATA_LIGACAO { get; set; }
        public DateTime DATA_ATUALIZACAO { get; set; }
        public int STATUS_LIGACAO { get; set; }
        public string OBSERVACAO { get; set; }
        public string LOGIN { get; set; }
    }
}
