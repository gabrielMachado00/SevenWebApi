using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SevenMedicalApi.Models
{
    [Table("TRANSFERENCIA")]
    public class TRANSFERENCIA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TRANSFERENCIA { get; set; }
        public int ID_CONTA_ORIGEM { get; set; }
        public int ID_CONTA_DESTINO { get; set; }
        public System.Decimal VALOR { get; set; }
        public System.DateTime DATA { get; set; }
        public string LOGIN { get; set; }
    }
}