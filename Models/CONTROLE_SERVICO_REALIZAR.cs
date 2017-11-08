using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SevenMedicalApi.Models
{
    [Table("CONTROLE_SERVICO_REALIZAR")]
    public class CONTROLE_SERVICO_REALIZAR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CLIENTE { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public int NUM_SESSOES { get; set; }
        public int SESSOES_REALIZADAS { get; set; }
        public int ID_VENDA { get; set; }
    }
}