﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("CONTATOS_FORNECEDORES")]
    public class FORNECEDOR_CONTATO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CONTATO { get; set; }
        public int ID_FORNECEDOR { get; set; }
        public string CONTATO { get; set; }
        public System.DateTime DATA_CADASTRO { get; set; }
        public string TELEFONE { get; set; }
        public string CELULAR { get; set; }
        public string EMAIL { get; set; }
        public string AREA { get; set; }
    }
}