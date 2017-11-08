using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("Appointments")]
    public class Appointments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniqueID { get; set; }
        public int Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool AllDay { get; set; }
        public string Subject { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int Label { get; set; }
        public int ResourceID { get; set; }
        public string ResourceIDs { get; set; }
        public string ReminderInfo { get; set; }
        public string RecurrenceInfo { get; set; }
        public string CustomField1 { get; set; }
        public int ID_CLIENTE { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public Nullable<int> ID_SALA { get; set; }
        public string RegioesCorpoIDs { get; set; }
        public bool ENCAIXE { get; set; }
        public Nullable<int> ID_CONVENIO { get; set; }
        public int OPCAO_COBRANCA { get; set; }
        public bool FATURADO { get; set; }
        public string RegioesFaturadas { get; set; }
        public bool PACOTE { get; set; }
        public Nullable<int> ID_EQUIPAMENTO { get; set; }
        public Nullable<int> ID_PROFISSIONAL { get; set; }
        public string LOGIN { get; set; }
        public Nullable<DateTime> DATA_INCLUSAO { get; set; }
        public Nullable<DateTime> DATA_ATUALIZACAO { get; set; }
    }
}