using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SisMedApi.Models
{
    [Table("ATENDIMENTO")]
    public class ATENDIMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ATENDIMENTO { get; set; }
        public Nullable<int> ID_APPOINTMENT { get; set; }
        public string LOGIN { get; set; }
        private string fpronturario = string.Empty;
        public string PRONTUARIO
        {
            get { return GetTextoFormatado(fpronturario); }
            set { fpronturario = GetTextoFormatado(value); }
        }
        public string OBS { get; set; }
        public string STATUS { get; set; }
        public Nullable<System.DateTime> DATA_HORA_INI { get; set; }
        public Nullable<System.DateTime> DATA_HORA_FIM { get; set; }
        public Nullable<System.DateTime> DATA_ULT_ATUALIZACAO { get; set; }
        public int ID_CLIENTE { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public Nullable<int> ID_CONVENIO { get; set; }
        public Nullable<int> ID_SALA { get; set; }
        public string CANAL_ATENDIMENTO { get; set; }

        public string GetTextoFormatado(string rtfText)
        {
            string resultado = rtfText;
            if ((resultado != null) && (resultado != string.Empty) && (resultado.Contains("rtf")))
            {
                //System.Windows.Forms.RichTextBox;
                System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();
                // Use the RichTextBox to convert the RTF code to plain text.
                rtBox.Rtf = rtfText;
                resultado = rtBox.Text;
                //rtBox.Dispose();
            }
            return resultado;
        }
    }
}