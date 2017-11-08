using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("TIPO_PRODUTO")]
    public class TIPO_PRODUTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPO_PRODUTO { get; set; }
        public string DESC_TIPO_PRODUTO { get; set; }
    }
}