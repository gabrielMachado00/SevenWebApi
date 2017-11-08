﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisMedApi.Models
{
    [Table("CONTATOS_CLIENTES")]
    public class CLIENTE_CONTATO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CONTATO { get; set; }
        public int ID_CLIENTE { get; set; }
        public string CONTATO { get; set; }
        public System.DateTime DATA_CADASTRO { get; set; }
        public string TELEFONE { get; set; }
        public string CELULAR { get; set; }
        public string EMAIL { get; set; }
        public Nullable<System.DateTime> DATA_NASCIMENTO { get; set; }
        public string PROFISSAO { get; set; }
        public string AREA { get; set; }
        public string VINCULO { get; set; }
        public string DADOS_BANCARIOS { get; set; }
    }
}