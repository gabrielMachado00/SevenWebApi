using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;
using SevenMedicalApi.Models;

namespace SisMedApi.Controllers
{

    public class ATENDIMENTODTO
    {
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
        public string CLIENTE { get; set; }
        public string PROFISSIONAL { get; set; }
        public string CONVENIO { get; set; }
        public string SALA { get; set; }
        public string OPCAO { get; set; }
        public Nullable<int> ID_PROFISSIONAL { get; set; }
        public int ID_CLIENTE { get; set; }
        public string STATUS { get; set; }
        public DateTime? DATA_HORA_INI { get; set; }
        public DateTime? DATA_HORA_FIM { get; set; }
        public DateTime? DATA_ULT_ATUALIZACAO { get; set; }
        public string ALERTA_CLINICO { get; set; }
        public string DESC_STATUS { get; set; }
        public Nullable<int> ID_CONVENIO { get; set; }
        public Nullable<int> ID_SALA { get; set; }
        public string CANAL_ATENDIMENTO { get; set; }
        public string DESC_CANAL_ATENDIMENTO { get; set; }
        public Nullable<int> ID_USUARIO_ATENDIMENTO { get; set; }

        public string TELEFONE { get; set; }
        public string CELULAR { get; set; }
        public string EMAIL { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string SEXO { get; set; }
        public Nullable<DateTime> DATA_NASCIMENTO { get; set; }
        public string ESTADO_CIVIL { get; set; }
        public string NATURALIDADE { get; set; }
        public string PROFISSAO { get; set; }
        public string NOME_MAE { get; set; }
        public string NOME_PAI { get; set; }
        
        public string OBS_CLIENTE { get; set; }

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

    public class ORCAMENTO
    {
        public bool gerado { get; set; }
    }

    public class AtendimentoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/Atendimento/Profissional/1
        [Route("api/Atendimento/Profissional/{idProfissional}")]
        public IEnumerable<ATENDIMENTODTO> GetATENDIMENTO_PROFISSIONAL(int idProfissional)
        {
            return (from a in db.ATENDIMENTOes
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    select new ATENDIMENTODTO()
                    {
                        ID_ATENDIMENTO = a.ID_ATENDIMENTO,
                        ID_APPOINTMENT = a.ID_APPOINTMENT,
                        LOGIN = a.LOGIN,
                        PRONTUARIO = a.PRONTUARIO,
                        OBS = a.OBS,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.ID_CONVENIO == null ? "PARTICULAR" : "CONVÊNIO",
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ID_CLIENTE = a.ID_CLIENTE,
                        STATUS = a.STATUS,
                        DATA_HORA_INI = a.DATA_HORA_INI,
                        DATA_HORA_FIM = a.DATA_HORA_FIM,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        DATA_ULT_ATUALIZACAO = a.DATA_ULT_ATUALIZACAO,
                        DESC_STATUS = a.STATUS == "A" ? "AGUARDANDO" :
                                      a.STATUS == "I" ? "INICIADO" :
                                      a.STATUS == "C" ? "CONCLUÍDO" : "",
                        CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO,
                        DESC_CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO == "TE" ? "TELEFONE" :
                                                 a.CANAL_ATENDIMENTO == "EM" ? "E-MAIL" :
                                                 a.CANAL_ATENDIMENTO == "ME" ? "MENSAGEM" :
                                                 a.CANAL_ATENDIMENTO == "RE" ? "RECADO" :
                                                 a.CANAL_ATENDIMENTO == "OT" ? "OUTRO" : "AGENDA",
                        TELEFONE = c.TELEFONE,
                        CELULAR = c.CELULAR,
                        EMAIL = c.EMAIL,
                        CPF = c.CPF,
                        RG = c.RG,
                        DATA_NASCIMENTO = c.DATA_NASCIMENTO,
                        SEXO = c.SEXO == "M" ? "MASCULINO" :
                                    c.SEXO == "F" ? "FEMININO" : "",
                        NOME_PAI = c.NOME_PAI,
                        NOME_MAE = c.NOME_MAE,
                        PROFISSAO = c.PROFISSAO,
                        ESTADO_CIVIL = c.ESTADO_CIVIL,
                        NATURALIDADE = c.NATURALIDADE
                    }).Where(a => a.ID_PROFISSIONAL == idProfissional && a.ID_APPOINTMENT != null).ToList().OrderBy(c=>c.ID_CLIENTE).OrderBy(c=>c.DATA_HORA_INI);
        }

        // GET api/Atendimento/Profissional/1
        [Route("api/Atendimento/appointment/{idAppointment}")]
        public IEnumerable<ATENDIMENTODTO> GetATENDIMENTO_APPOINTMENT(int idAppointment)
        {
            return (from a in db.ATENDIMENTOes
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    select new ATENDIMENTODTO()
                    {
                        ID_ATENDIMENTO = a.ID_ATENDIMENTO,
                        ID_APPOINTMENT = a.ID_APPOINTMENT,
                        PRONTUARIO = a.PRONTUARIO,
                        OBS = a.OBS,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.ID_CONVENIO == null ? "PARTICULAR" : "CONVÊNIO",
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ID_CLIENTE = a.ID_CLIENTE,
                        STATUS = a.STATUS,
                        DATA_HORA_INI = a.DATA_HORA_INI,
                        DATA_HORA_FIM = a.DATA_HORA_FIM,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        DATA_ULT_ATUALIZACAO = a.DATA_ULT_ATUALIZACAO,
                        DESC_STATUS = a.STATUS == "A" ? "AGUARDANDO" :
                                      a.STATUS == "I" ? "INICIADO" :
                                      a.STATUS == "C" ? "CONCLUÍDO" : "",
                        CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO,
                        DESC_CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO == "TE" ? "TELEFONE" :
                                                 a.CANAL_ATENDIMENTO == "EM" ? "E-MAIL" :
                                                 a.CANAL_ATENDIMENTO == "ME" ? "MENSAGEM" :
                                                 a.CANAL_ATENDIMENTO == "RE" ? "RECADO" :
                                                 a.CANAL_ATENDIMENTO == "OT" ? "OUTRO" : "AGENDA"
                    }).Where(a => a.ID_APPOINTMENT == idAppointment).ToList();
        }

        // GET api/Atendimento/Profissional/1/Atendimento/2
        [Route("api/Atendimento/Profissional/{idProfissional}/Atendimento/{idAtendimento}")]
        public IEnumerable<ATENDIMENTODTO> GetATENDIMENTO_PROFISSIONAL(int idProfissional, int idAtendimento)
        {
            return (from a in db.ATENDIMENTOes
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    select new ATENDIMENTODTO()
                    {
                        ID_ATENDIMENTO = a.ID_ATENDIMENTO,
                        ID_APPOINTMENT = a.ID_APPOINTMENT,
                        LOGIN = a.LOGIN,
                        PRONTUARIO = a.PRONTUARIO,
                        OBS = a.OBS,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.ID_CONVENIO == null ? "PARTICULAR" : "CONVÊNIO",
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ID_CLIENTE = a.ID_CLIENTE,
                        STATUS = a.STATUS,
                        DATA_HORA_INI = a.DATA_HORA_INI,
                        DATA_HORA_FIM = a.DATA_HORA_FIM,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        DATA_ULT_ATUALIZACAO = a.DATA_ULT_ATUALIZACAO,
                        DESC_STATUS = a.STATUS == "A" ? "AGUARDANDO" :
                                      a.STATUS == "I" ? "INICIADO" :
                                      a.STATUS == "C" ? "CONCLUÍDO" : "",
                        CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO,
                        DESC_CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO == "TE" ? "TELEFONE" :
                                                 a.CANAL_ATENDIMENTO == "EM" ? "E-MAIL" :
                                                 a.CANAL_ATENDIMENTO == "ME" ? "MENSAGEM" :
                                                 a.CANAL_ATENDIMENTO == "RE" ? "RECADO" :
                                                 a.CANAL_ATENDIMENTO == "OT" ? "OUTRO" : "AGENDA"
                    }).Where(a => a.ID_PROFISSIONAL == idProfissional && a.ID_ATENDIMENTO == idAtendimento).ToList();
        }

        // GET api/Atendimento/Profissional/1/Cliente/2
        [Route("api/Atendimento/Profissional/{idProfissional}/Cliente/{idCliente}")]
        public IEnumerable<ATENDIMENTODTO> GetATENDIMENTO_PROFISSIONAL_CLI(int idProfissional, int idCliente)
        {
            return (from a in db.ATENDIMENTOes
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    select new ATENDIMENTODTO()
                    {
                        ID_ATENDIMENTO = a.ID_ATENDIMENTO,
                        ID_APPOINTMENT = a.ID_APPOINTMENT,
                        LOGIN = a.LOGIN,
                        PRONTUARIO = a.PRONTUARIO,
                        OBS = a.OBS,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.ID_CONVENIO == null ? "PARTICULAR" : "CONVÊNIO",
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ID_CLIENTE = a.ID_CLIENTE,
                        STATUS = a.STATUS,
                        DATA_HORA_INI = a.DATA_HORA_INI,
                        DATA_HORA_FIM = a.DATA_HORA_FIM,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        DATA_ULT_ATUALIZACAO = a.DATA_ULT_ATUALIZACAO,
                        DESC_STATUS = a.STATUS == "A" ? "AGUARDANDO" :
                                      a.STATUS == "I" ? "INICIADO" :
                                      a.STATUS == "C" ? "CONCLUÍDO" : "",
                        CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO,
                        DESC_CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO == "TE" ? "TELEFONE" :
                                                 a.CANAL_ATENDIMENTO == "EM" ? "E-MAIL" :
                                                 a.CANAL_ATENDIMENTO == "ME" ? "MENSAGEM" :
                                                 a.CANAL_ATENDIMENTO == "RE" ? "RECADO" :
                                                 a.CANAL_ATENDIMENTO == "OT" ? "OUTRO" : "AGENDA"
                    }).Where(a => a.ID_PROFISSIONAL == idProfissional && a.ID_CLIENTE == idCliente).ToList();
        }

        // GET api/Atendimento/Profissional/1/Cliente/2
        [Route("api/Atendimento/Cliente/{idCliente}")]
        public IEnumerable<ATENDIMENTODTO> GetATENDIMENTO_CLIENTE(int idCliente)
        {
            return (from a in db.ATENDIMENTOes
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on a.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()

                    select new ATENDIMENTODTO()
                    {
                        ID_ATENDIMENTO = a.ID_ATENDIMENTO,
                        ID_APPOINTMENT = a.ID_APPOINTMENT,
                        LOGIN = a.LOGIN,
                        PRONTUARIO = a.PRONTUARIO,
                        OBS = a.OBS,
                        CLIENTE = c.NOME,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.ID_CONVENIO == null ? "PARTICULAR" : "CONVÊNIO",
                        ID_CLIENTE = a.ID_CLIENTE,
                        STATUS = a.STATUS,
                        DATA_HORA_INI = a.DATA_HORA_INI,
                        DATA_HORA_FIM = a.DATA_HORA_FIM,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        DATA_ULT_ATUALIZACAO = a.DATA_ULT_ATUALIZACAO,
                        DESC_STATUS = a.STATUS == "A" ? "AGUARDANDO" :
                                      a.STATUS == "I" ? "INICIADO" :
                                      a.STATUS == "C" ? "CONCLUÍDO" : "",
                        PROFISSIONAL = p.NOME,
                        CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO,
                        DESC_CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO == "TE" ? "TELEFONE" :
                                                 a.CANAL_ATENDIMENTO == "EM" ? "E-MAIL" :
                                                 a.CANAL_ATENDIMENTO == "ME" ? "MENSAGEM" :
                                                 a.CANAL_ATENDIMENTO == "RE" ? "RECADO" :
                                                 a.CANAL_ATENDIMENTO == "OT" ? "OUTRO" : "AGENDA"
                    }).Where(a => a.ID_CLIENTE == idCliente && a.STATUS == "C").OrderByDescending(a => a.DATA_HORA_INI).ToList();
        }

        // GET api/Atendimento/Profissional/1/Cliente/2
        [Route("api/Atendimento/Cliente/{idCliente}/{login}")]
        public IEnumerable<ATENDIMENTODTO> GetATENDIMENTO_CLIENTELOGIN(int idCliente, string login)
        {
            var query = "SELECT  at.* "
                + " FROM ATENDIMENTO at "
                + " INNER JOIN Appointments ag ON ag.UniqueID = at.ID_APPOINTMENT and at.ID_CLIENTE = @idCliente "
                + " INNER JOIN PROCEDIMENTO pr ON pr.ID_PROCEDIMENTO = ag.ID_PROCEDIMENTO "
                + " WHERE (pr.CRP is null OR pr.CRP = 0) "
                + " or(pr.CRP = 1 and at.LOGIN = @login)"
                + " UNION ALL "
                + " SELECT at.* "
                + " from ATENDIMENTO at where at.ID_CLIENTE = @idCliente "
                + " and at.ID_APPOINTMENT is null ";

            List<ATENDIMENTODTO> resultado = new List<ATENDIMENTODTO>();

            resultado = db.Database.SqlQuery<ATENDIMENTODTO>(query,
                new System.Data.SqlClient.SqlParameter("@idCliente", idCliente),
                new System.Data.SqlClient.SqlParameter("@Login", login)).ToList();

            var id = new int[resultado.Count];
            for (var i = 0; i < resultado.Count; i++)
            {
                id[i] = resultado[i].ID_ATENDIMENTO;
            }

            return (from a in db.ATENDIMENTOes
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    join p in db.PROFISSIONALs on a.ID_PROFISSIONAL equals p.ID_PROFISSIONAL into _p
                    from p in _p.DefaultIfEmpty()
                    select new ATENDIMENTODTO()
                    {
                        ID_ATENDIMENTO = a.ID_ATENDIMENTO,
                        ID_APPOINTMENT = a.ID_APPOINTMENT,
                        LOGIN = a.LOGIN,
                        PRONTUARIO = a.PRONTUARIO,
                        OBS = a.OBS,
                        CLIENTE = c.NOME,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.ID_CONVENIO == null ? "PARTICULAR" : "CONVÊNIO",
                        ID_CLIENTE = a.ID_CLIENTE,
                        STATUS = a.STATUS,
                        DATA_HORA_INI = a.DATA_HORA_INI,
                        DATA_HORA_FIM = a.DATA_HORA_FIM,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        DATA_ULT_ATUALIZACAO = a.DATA_ULT_ATUALIZACAO,
                        DESC_STATUS = a.STATUS == "A" ? "AGUARDANDO" :
                                      a.STATUS == "I" ? "INICIADO" :
                                      a.STATUS == "C" ? "CONCLUÍDO" : "",
                        PROFISSIONAL = p.NOME,
                        CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO,
                        DESC_CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO == "TE" ? "TELEFONE" :
                                                 a.CANAL_ATENDIMENTO == "EM" ? "E-MAIL" :
                                                 a.CANAL_ATENDIMENTO == "ME" ? "MENSAGEM" :
                                                 a.CANAL_ATENDIMENTO == "RE" ? "RECADO" :
                                                 a.CANAL_ATENDIMENTO == "OT" ? "OUTRO" : "AGENDA"
                    }).Where(a => a.ID_CLIENTE == idCliente && a.STATUS == "C" && id.Contains(a.ID_ATENDIMENTO)).OrderByDescending(a => a.DATA_HORA_INI).ToList();
        }

        // GET api/Atendimento/Profissional/1
        [Route("api/Atendimento/Profissional/Usuario/{idUsuario}")]
        public IEnumerable<ATENDIMENTODTO> GetATENDIMENTO_PROFISSIONAL_USUARIO(int idUsuario)
        {
            return (from a in db.ATENDIMENTOes
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    join up in db.USUARIO_PROFISSIONAL on a.ID_PROFISSIONAL equals up.ID_PROFISSIONAL
                    select new ATENDIMENTODTO()
                    {
                        ID_ATENDIMENTO = a.ID_ATENDIMENTO,
                        ID_APPOINTMENT = a.ID_APPOINTMENT,
                        LOGIN = a.LOGIN,
                        PRONTUARIO = a.PRONTUARIO,
                        OBS = a.OBS,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.ID_CONVENIO == null ? "PARTICULAR" : "CONVÊNIO",
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ID_CLIENTE = a.ID_CLIENTE,
                        STATUS = a.STATUS,
                        DATA_HORA_INI = a.DATA_HORA_INI,
                        DATA_HORA_FIM = a.DATA_HORA_FIM,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        DATA_ULT_ATUALIZACAO = a.DATA_ULT_ATUALIZACAO,
                        DESC_STATUS = a.STATUS == "A" ? "AGUARDANDO" :
                                      a.STATUS == "I" ? "INICIADO" :
                                      a.STATUS == "C" ? "CONCLUÍDO" : "",
                        CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO,
                        DESC_CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO == "TE" ? "TELEFONE" :
                                                 a.CANAL_ATENDIMENTO == "EM" ? "E-MAIL" :
                                                 a.CANAL_ATENDIMENTO == "ME" ? "MENSAGEM" :
                                                 a.CANAL_ATENDIMENTO == "RE" ? "RECADO" :
                                                 a.CANAL_ATENDIMENTO == "OT" ? "OUTRO" : "AGENDA",
                        ID_USUARIO_ATENDIMENTO = up.ID_USUARIO
                    }).Where(a => a.ID_USUARIO_ATENDIMENTO == idUsuario && a.ID_APPOINTMENT != null).ToList();
        }

        // GET api/Atendimento/1                
        [Route("api/atendimento/id/{idAtendimento}", Name = "GetAtendimentoById")]
        public IEnumerable<ATENDIMENTODTO> GetAtendimentoById(int idAtendimento)
        {
            return (from a in db.ATENDIMENTOes
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    select new ATENDIMENTODTO()
                    {
                        ID_ATENDIMENTO = a.ID_ATENDIMENTO,
                        ID_APPOINTMENT = a.ID_APPOINTMENT,
                        LOGIN = a.LOGIN,
                        PRONTUARIO = a.PRONTUARIO,
                        OBS = a.OBS,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.ID_CONVENIO == null ? "PARTICULAR" : "CONVÊNIO",
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ID_CLIENTE = a.ID_CLIENTE,
                        STATUS = a.STATUS,
                        DATA_HORA_INI = a.DATA_HORA_INI,
                        DATA_HORA_FIM = a.DATA_HORA_FIM,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        DATA_ULT_ATUALIZACAO = a.DATA_ULT_ATUALIZACAO,
                        DESC_STATUS = a.STATUS == "A" ? "AGUARDANDO" :
                                      a.STATUS == "I" ? "INICIADO" :
                                      a.STATUS == "C" ? "CONCLUÍDO" : "",
                        CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO,
                        DESC_CANAL_ATENDIMENTO = a.CANAL_ATENDIMENTO == "TE" ? "TELEFONE" :
                                                 a.CANAL_ATENDIMENTO == "EM" ? "E-MAIL" :
                                                 a.CANAL_ATENDIMENTO == "ME" ? "MENSAGEM" :
                                                 a.CANAL_ATENDIMENTO == "RE" ? "RECADO" :
                                                 a.CANAL_ATENDIMENTO == "OT" ? "OUTRO" : "AGENDA"
                    }).Where(a => a.ID_ATENDIMENTO == idAtendimento && a.ID_APPOINTMENT != null).ToList();
        }

        // GET api/Atendimento/Profissional/1/Cliente/2
        [Route("api/Atendimento/GeraOrcamento/{idAtendimento}/{login}")]
        public IEnumerable<ORCAMENTO> GetGeraOrcamento(int idAtendimento, string login)
        {
            List<ORCAMENTO> lorc = new List<ORCAMENTO>();
            ORCAMENTO orc = new ORCAMENTO();
            List<ATENDIMENTO_TRATAMENTO> modelAtendTratamento = null;

            modelAtendTratamento = db.Database.SqlQuery<ATENDIMENTO_TRATAMENTO>(" SELECT ID_TRATAMENTO, ID_ATENDIMENTO, ID_PROCEDIMENTO," +
                                                                                " LOGIN, ID_REGIAO_CORPO, NUM_SESSOES, VALOR_NEGOCIADO" +
                                                                                " FROM ATENDIMENTO_TRATAMENTO" +
                                                                                " WHERE ID_ATENDIMENTO = " + idAtendimento).ToList();
            List<ATENDIMENTO> modelAtendimento = null;
            modelAtendimento = db.Database.SqlQuery<ATENDIMENTO>(" SELECT * FROM ATENDIMENTO" +
                                                                 " WHERE ID_ATENDIMENTO = " + idAtendimento).ToList();
            int idCliente = modelAtendimento[0].ID_CLIENTE;
            int idProfissional = modelAtendimento[0].ID_PROFISSIONAL;
            if (modelAtendTratamento.Count > 0)
            {
                var venda = new VENDA()
                {
                    ID_VENDA = 0,
                    ID_CLIENTE = idCliente,
                    DATA = DateTime.Now,
                    VALOR = 0,
                    LOGIN = login,
                    DATA_ATUALIZACAO = DateTime.Now,
                    STATUS = "O",
                    COMISSAO_GERADA = false
                };

                int idVenda = PostVENDA(venda);

                if (idVenda > 0)
                {
                    decimal? valorOrcamento = 0;
                    decimal? valorTotalVenda = 0;

                    for (int i = 0; i < modelAtendTratamento.Count; i++)
                    {
                        List<PROCEDIMENTO_REGIAO_CORPO> modelRegiaoCorpo = null;
                        modelRegiaoCorpo = db.Database.SqlQuery<PROCEDIMENTO_REGIAO_CORPO>(" SELECT ID_PROCEDIMENTO, ID_REGIAO_CORPO, NUM_SESSOES," +
                                                                                             " VALOR, TEMPO, OBS, MOVIMENTA_ESTOQUE" +
                                                                                             " FROM PROCEDIMENTO_REGIAO_CORPO" +
                                                                                             " WHERE ID_PROCEDIMENTO = " + modelAtendTratamento[i].ID_PROCEDIMENTO +
                                                                                             "   AND ID_REGIAO_CORPO = " + modelAtendTratamento[i].ID_REGIAO_CORPO).ToList();
                        valorOrcamento = 0;
                        for (int j = 0; j < modelRegiaoCorpo.Count; j++)
                        {
                            if (modelRegiaoCorpo[j].NUM_SESSOES == modelAtendTratamento[i].NUM_SESSOES)
                            {
                                valorOrcamento = modelRegiaoCorpo[0].VALOR;
                            }

                            if ((valorOrcamento == 0) && (modelRegiaoCorpo[j].NUM_SESSOES == 1))
                            {
                                valorOrcamento = modelRegiaoCorpo[0].VALOR * modelAtendTratamento[i].NUM_SESSOES;
                            }
                        }

                        var vendaItem = new VENDA_ITEM
                        {
                            ID_ITEM_VENDA = 0,
                            ID_ATENDIMENTO = Convert.ToInt32(idAtendimento),
                            ID_PROCEDIMENTO = modelAtendTratamento[i].ID_PROCEDIMENTO,
                            ID_REGIAO_CORPO = modelAtendTratamento[i].ID_REGIAO_CORPO,
                            ID_VENDA = idVenda,
                            VALOR = Convert.ToDecimal(valorOrcamento),
                            VALOR_PAGO = modelAtendTratamento[i].VALOR_NEGOCIADO != null ? modelAtendTratamento[i].VALOR_NEGOCIADO : valorOrcamento,
                            DATA_ATUALIZACAO = DateTime.Now,
                            NUM_SESSOES = modelAtendTratamento[i].NUM_SESSOES,
                            ID_PROFISSIONAL = idProfissional,
                            NUM_PACOTES = 1,
                            PACOTE = true,
                        };
                        valorTotalVenda = valorTotalVenda + (modelAtendTratamento[i].VALOR_NEGOCIADO != null ? modelAtendTratamento[i].VALOR_NEGOCIADO : valorOrcamento);
                        PostVENDA_ITEM(vendaItem);
                    }
                    if (valorTotalVenda > 0)
                    {
                        var sql = @" UPDATE VENDA 
                                     SET VALOR = VALOR + {0},
                                     DATA_ATUALIZACAO = GETDATE() 
                                     WHERE ID_VENDA = {1} ";
                        try
                        {
                            db.Database.ExecuteSqlCommand(sql, valorTotalVenda, idVenda);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                    orc.gerado = true;
                }
                else
                    orc.gerado = false;
            }
            else
                orc.gerado = false;
            lorc.Add(orc);
            return lorc;
        }

        protected int PostVENDA(VENDA vENDA)
        {
            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_VENDA;").First();

            vENDA.ID_VENDA = NextValue;

            db.VENDAs.Add(vENDA);

            try
            {
               // db.BulkSaveChanges();
                db.SaveChanges();
                return NextValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PostVENDA_ITEM(VENDA_ITEM vENDA_ITEM)
        {
            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ITEM_VENDA;").First();

            vENDA_ITEM.ID_ITEM_VENDA = NextValue;

            db.VENDA_ITEM.Add(vENDA_ITEM);

            try
            {
                //db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT: api/ProfissionalHorario/PutPROFISSIONAL_HORARIO/5/2
        [ResponseType(typeof(void))]
        [Route("api/Atendimento/PutATENDIMENTO/{idAtendimento}")]
        public IHttpActionResult PutATENDIMENTO(int idAtendimento, ATENDIMENTO aTENDIMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idAtendimento != aTENDIMENTO.ID_ATENDIMENTO)
            {
                return BadRequest();
            }

            aTENDIMENTO.DATA_ULT_ATUALIZACAO = DateTime.Now;
            if ((aTENDIMENTO.STATUS == "C") && (aTENDIMENTO.DATA_HORA_FIM == null))
                aTENDIMENTO.DATA_HORA_FIM = DateTime.Now;


            db.Entry(aTENDIMENTO).State = EntityState.Modified;

            try
            {
               // db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ATENDIMENTOExists(idAtendimento))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Atendimento
        [ResponseType(typeof(ATENDIMENTO))]
        public IHttpActionResult PostATENDIMENTO(ATENDIMENTO aTENDIMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ATENDIMENTO;").First();

            aTENDIMENTO.ID_ATENDIMENTO = NextValue;

            aTENDIMENTO.DATA_HORA_INI = DateTime.Now;
            aTENDIMENTO.DATA_ULT_ATUALIZACAO = DateTime.Now;

            db.ATENDIMENTOes.Add(aTENDIMENTO);

            try
            {
                //db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ATENDIMENTOExists(aTENDIMENTO.ID_ATENDIMENTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aTENDIMENTO.ID_ATENDIMENTO }, aTENDIMENTO);
        }

        // DELETE: api/Atendimento/5
        [ResponseType(typeof(ATENDIMENTO))]
        public IHttpActionResult DeleteATENDIMENTO(int id)
        {
            ATENDIMENTO aTENDIMENTO = db.ATENDIMENTOes.Find(id);
            if (aTENDIMENTO == null)
            {
                return NotFound();
            }

            db.ATENDIMENTOes.Remove(aTENDIMENTO);
            //db.BulkSaveChanges();
            db.SaveChanges();
        
            return Ok(aTENDIMENTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ATENDIMENTOExists(int id)
        {
            return db.ATENDIMENTOes.Count(e => e.ID_ATENDIMENTO == id) > 0;
        }
    }
}