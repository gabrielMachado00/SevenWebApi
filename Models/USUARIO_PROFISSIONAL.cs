using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SisMedApi.Models
{
    [Table("USUARIO_PROFISSIONAL")]
    public class USUARIO_PROFISSIONAL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_USUARIO_PROFISSIONAL { get; set; }
        public int ID_USUARIO { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public DateTime DATA_INCLUSAO { get; set; }        
    }
}