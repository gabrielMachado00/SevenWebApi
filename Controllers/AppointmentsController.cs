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
using System.Web.Http.Cors;
using System.Data.SqlClient;

namespace SisMedApi.Controllers
{
    public class AppointmentsDTO
    {
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
        public string CLIENTE { get; set; }
        public string PROFISSIONAL { get; set; }
        public string PROCEDIMENTO { get; set; }
        public string CONVENIO { get; set; }
        public string SALA { get; set; }
        public string EQUIPAMENTO { get; set; }
        public string OPCAO { get; set; }
        public string ALERTA_CLINICO { get; set; }
        public string RegioesFaturadas { get; set; }
        public string PROFISSAO { get; set; }
        public Nullable<DateTime> DATA_NASCIMENTO { get; set; }
        public string URL_FOTO { get; set; }
        public bool BASICO { get; set; }
        public string DESC_STATUS { get; set; }
        public string COR_SERVICO { get; set; }
        public bool PACOTE { get; set; }
        public Nullable<int> ID_EQUIPAMENTO { get; set; }
        public Nullable<int> ID_PROFISSIONAL { get; set; }
        public Nullable<int> ID_REGIAO_CORPO { get; set; }
        public string LOGIN { get; set; }
        public Nullable<DateTime> DATA_INCLUSAO { get; set; }
        public Nullable<DateTime> DATA_ATUALIZACAO { get; set; }

    }

    public class ResumoAgenda
    {
        public Nullable<int> TOTAL { get; set; }
        public Nullable<int> CONFIRMADOS { get; set; }
        public Nullable<int> CANCELADOS { get; set; }
        public Nullable<int> ATENDIDOS { get; set; }
        public Nullable<int> AGENDADOS { get; set; }
        public Nullable<int> FALTOU { get; set; }
        public Nullable<int> OUTROS { get; set; }
    }

    public class ProfissionalAgenda
    {
        public string TITULO { get; set; }
        public int AGENDADOS { get; set; }
        public int CONFIRMADOS { get; set; }
        public int ATENDIDOS { get; set; }
        public int CANCELADOS { get; set; }
        public int OUTROS { get; set; }
    }

    //public class AgendaDia
    //{
    //    public int DIA { get; set; }
    //    public int AGENDADOS { get; set; }
    //    public int CONFIRMADOS { get; set; }
    //    public int ATENDIDOS { get; set; }
    //    public int CANCELADOS { get; set; }
    //    public int FALTOU { get; set; }
    //    public int OUTROS { get; set; }
    //}

    public class TaxaOcupacaoDia
    {
        public Nullable<int> COD_DIA_SEMANA { get; set; }
        public Nullable<decimal> PERC { get; set; }
    }

    public class TaxaOcupacaoProfissional
    {
        public string TITULO { get; set; }
        public Nullable<decimal> TAXA_OCUPACAO { get; set; }
    }

    public class FaturamentoServico
    {
        public string SERVICO { get; set; }
        public decimal VALOR_FATURAMENTO { get; set; }
    }

    public class ClientePagamento
    {
        public Nullable<int> ID_CLIENTE { get; set; }
        public Nullable<decimal> VALOR_PAGO { get; set; }
    }

    public class ListaAgenda
    {
        public Int64 NUM_ROW { get; set; }
        public DateTime DATA { get; set; }
        public string HORA_INI { get; set; }
        public string HORA_FIN { get; set; }
        public string STATUS { get; set; }
        public string CLIENTE { get; set; }
        public string PROFISSIONAL { get; set; }
        public string PROCEDIMENTO { get; set; }
        public string AREA_CORPO { get; set; }
        public Nullable<int> ID_PROCEDIMENTO { get; set; }
        public string LOGIN { get; set; }
        public Nullable<DateTime> DATA_INCLUSAO { get; set; }
        public Nullable<DateTime> DATA_ATUALIZACAO { get; set; }
        public string OBSERVACAO { get; set; }
        public string CONVENIO { get; set; }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AppointmentsController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/Appointment/1
        [Route("api/Appointment/{idAppointment}")]
        public IEnumerable<AppointmentsDTO> GetAPPOINTMENT(int idAppointment)
        {
            return (from a in db.Appointments
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ResourceID equals p.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on a.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    join ar in db.APPOINTMENTS_REGIAO_CORPO on a.UniqueID equals ar.ID_APPOINTMENTS into _ar
                    from ar in _ar.DefaultIfEmpty()
                    select new AppointmentsDTO()
                    {
                        UniqueID = a.UniqueID,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        PROCEDIMENTO = pr.DESCRICAO,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.OPCAO_COBRANCA == 0 ? "PARTICULAR" :
                                a.OPCAO_COBRANCA == 1 ? "CONVÊNIO" : "",
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        ResourceID = a.ResourceID,
                        ID_CLIENTE = a.ID_CLIENTE,
                        RegioesCorpoIDs = a.RegioesCorpoIDs,
                        ID_PROCEDIMENTO = a.ID_PROCEDIMENTO,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        Status = a.Status,
                        ID_SALA = a.ID_SALA,
                        ID_CONVENIO = a.ID_CONVENIO,
                        RegioesFaturadas = a.RegioesFaturadas,
                        FATURADO = (ar.FATURADO == 0) ? false : true,
                        PROFISSAO = c.PROFISSAO,
                        DATA_NASCIMENTO = c.DATA_NASCIMENTO,
                        URL_FOTO = c.DIR_FOTO,
                        Description = a.Description,
                        DESC_STATUS = a.Status == 0 ? "AGENDADO" :
                                      a.Status == 1 ? "CONFIRMADO" :
                                      a.Status == 2 ? "AGUARDANDO" :
                                      a.Status == 3 ? "EM ATENDIMENTO" :
                                      a.Status == 4 ? "ATENDIDO" :
                                      a.Status == 5 ? "CANCELADO" :
                                      a.Status == 6 ? "FALTOU" : "",
                        COR_SERVICO = pr.COR,
                        PACOTE = a.PACOTE,
                        ID_EQUIPAMENTO = a.ID_EQUIPAMENTO,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ENCAIXE = a.ENCAIXE,
                        ID_REGIAO_CORPO = ar.ID_REGIAO_CORPO,
                        LOGIN = a.LOGIN,
                        DATA_INCLUSAO = a.DATA_INCLUSAO,
                        DATA_ATUALIZACAO = a.DATA_ATUALIZACAO
                    }).Where(a => a.UniqueID == idAppointment).ToList();
        }

        // GET api/Appointment/1
        [Route("api/Appointment/Cliente/Agenda/{idCliente}")]
        public IEnumerable<AppointmentsDTO> GetAppt(int idCliente)
        {
            string sql = "SELECT C.NOME as CLIENTE, (case when A.Status = 1 then 'CONFIRMADO' WHEN A.STATUS = 2 THEN 'AGUARDANDO' WHEN A.STATUS = 4 THEN 'ATENDIDO'  END) AS DESC_STATUS, "
                + " A.StartDate, A.EndDate, P.NOME as PROFISSIONAL, pc.DESCRICAO as PROCEDIMENTO "
                + " FROM Appointments A "
                + " inner join profissional p on a.ResourceId = p.id_profissional "
                + " inner join procedimento pc on pc.id_procedimento = a.id_procedimento "
                + " inner join cliente C on C.ID_CLIENTE = A.ID_CLIENTE "
                + " WHERE A.Status in (1, 2, 4) and A.ID_CLIENTE = 0" + idCliente
                + " AND NOT A.UniqueID IN "
                + " (SELECT ID_APPOINTMENT "
                + " FROM VENDA V "
                + " INNER JOIN VENDA_ITEM  VI ON VI.ID_VENDA = V.ID_VENDA "
                + " WHERE V.ID_CLIENTE = 0" + idCliente
                + " AND V.STATUS = 'C' AND not VI.ID_APPOINTMENT is null)";

            return db.Database.SqlQuery<AppointmentsDTO>(sql).ToList();
        }

        // GET api/Appointment/Cliente/1
        [Route("api/Appointment/Cliente/{idCliente}")]
        public IEnumerable<AppointmentsDTO> GetAPPOINTMENTCLIENTE(int idCliente)
        {
            return (from a in db.Appointments
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ResourceID equals p.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on a.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    join ar in db.APPOINTMENTS_REGIAO_CORPO on a.UniqueID equals ar.ID_APPOINTMENTS into _ar
                    from ar in _ar.DefaultIfEmpty()
                    select new AppointmentsDTO()
                    {
                        UniqueID = a.UniqueID,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        PROCEDIMENTO = pr.DESCRICAO,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.OPCAO_COBRANCA == 0 ? "PARTICULAR" :
                                a.OPCAO_COBRANCA == 1 ? "CONVÊNIO" : "",
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        ResourceID = a.ResourceID,
                        ID_CLIENTE = a.ID_CLIENTE,
                        RegioesCorpoIDs = a.RegioesCorpoIDs,
                        ID_PROCEDIMENTO = a.ID_PROCEDIMENTO,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        FATURADO = (ar.FATURADO == 0) ? false : true,
                        ID_SALA = a.ID_SALA,
                        ID_CONVENIO = a.ID_CONVENIO,
                        RegioesFaturadas = a.RegioesFaturadas,
                        Status = a.Status,
                        DESC_STATUS = a.Status == 0 ? "AGENDADO" :
                                      a.Status == 1 ? "CONFIRMADO" :
                                      a.Status == 2 ? "AGUARDANDO" :
                                      a.Status == 3 ? "EM ATENDIMENTO" :
                                      a.Status == 4 ? "ATENDIDO" :
                                      a.Status == 5 ? "CANCELADO" :
                                      a.Status == 6 ? "FALTOU" : "",
                        COR_SERVICO = pr.COR,
                        PACOTE = a.PACOTE,
                        ID_EQUIPAMENTO = a.ID_EQUIPAMENTO,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ENCAIXE = a.ENCAIXE,
                        ID_REGIAO_CORPO = ar.ID_REGIAO_CORPO,
                        LOGIN = a.LOGIN,
                        DATA_INCLUSAO = a.DATA_INCLUSAO,
                        DATA_ATUALIZACAO = a.DATA_ATUALIZACAO
                    }).Where(a => a.ID_CLIENTE == idCliente).ToList();
        }

        [Route("api/Appointment/Cliente/Confirmados")]
        public IEnumerable<AppointmentsDTO> GetAPPOINTMENTCLIENTE_CONFIRMADOS()
        {
            return (from a in db.Appointments
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ResourceID equals p.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on a.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    join ar in db.APPOINTMENTS_REGIAO_CORPO on a.UniqueID equals ar.ID_APPOINTMENTS into _ar
                    from ar in _ar.DefaultIfEmpty()
                    select new AppointmentsDTO()
                    {
                        UniqueID = a.UniqueID,
                        Status = a.Status,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        PROCEDIMENTO = pr.DESCRICAO,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.OPCAO_COBRANCA == 0 ? "PARTICULAR" :
                                a.OPCAO_COBRANCA == 1 ? "CONVÊNIO" : "",
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        ResourceID = a.ResourceID,
                        ID_CLIENTE = a.ID_CLIENTE,
                        RegioesCorpoIDs = a.RegioesCorpoIDs,
                        ID_PROCEDIMENTO = a.ID_PROCEDIMENTO,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        FATURADO = (ar.FATURADO == 0) ? false : true,
                        ID_SALA = a.ID_SALA,
                        ID_CONVENIO = a.ID_CONVENIO,
                        Label = a.Label,
                        RegioesFaturadas = a.RegioesFaturadas,
                        DESC_STATUS = a.Status == 0 ? "AGENDADO" :
                                      a.Status == 1 ? "CONFIRMADO" :
                                      a.Status == 2 ? "AGUARDANDO" :
                                      a.Status == 3 ? "EM ATENDIMENTO" :
                                      a.Status == 4 ? "ATENDIDO" :
                                      a.Status == 5 ? "CANCELADO" :
                                      a.Status == 6 ? "FALTOU" : "",
                        COR_SERVICO = pr.COR,
                        PACOTE = a.PACOTE,
                        ID_EQUIPAMENTO = a.ID_EQUIPAMENTO,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ENCAIXE = a.ENCAIXE,
                        ID_REGIAO_CORPO = ar.ID_REGIAO_CORPO,
                        LOGIN = a.LOGIN,
                        DATA_INCLUSAO = a.DATA_INCLUSAO,
                        DATA_ATUALIZACAO = a.DATA_ATUALIZACAO
                    }).Where(a => a.Label == 0 && a.Status == 1 && DbFunctions.TruncateTime(a.StartDate) == DbFunctions.TruncateTime(DateTime.Now)).ToList();
        }

        // GET api/Appointment/Cliente/Confirmado/1
        [Route("api/Appointment/Cliente/Confirmado/{idCliente}")]
        public IEnumerable<AppointmentsDTO> GetAPPOINTMENTCLIENTE_CONFIRMADO(int idCliente)
        {
            return (from a in db.Appointments
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ResourceID equals p.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on a.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    join ar in db.APPOINTMENTS_REGIAO_CORPO on a.UniqueID equals ar.ID_APPOINTMENTS into _ar
                    from ar in _ar.DefaultIfEmpty()
                    select new AppointmentsDTO()
                    {
                        UniqueID = a.UniqueID,
                        Status = a.Status,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        PROCEDIMENTO = pr.DESCRICAO,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.OPCAO_COBRANCA == 0 ? "PARTICULAR" :
                                a.OPCAO_COBRANCA == 1 ? "CONVÊNIO" : "",
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        ResourceID = a.ResourceID,
                        ID_CLIENTE = a.ID_CLIENTE,
                        RegioesCorpoIDs = a.RegioesCorpoIDs,
                        ID_PROCEDIMENTO = a.ID_PROCEDIMENTO,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        FATURADO = (ar.FATURADO == 0) ? false : true,
                        ID_SALA = a.ID_SALA,
                        ID_CONVENIO = a.ID_CONVENIO,
                        RegioesFaturadas = a.RegioesFaturadas,
                        BASICO = c.BASICO,
                        DESC_STATUS = a.Status == 0 ? "AGENDADO" :
                                      a.Status == 1 ? "CONFIRMADO" :
                                      a.Status == 2 ? "AGUARDANDO" :
                                      a.Status == 3 ? "EM ATENDIMENTO" :
                                      a.Status == 4 ? "ATENDIDO" :
                                      a.Status == 5 ? "CANCELADO" :
                                      a.Status == 6 ? "FALTOU" : "",
                        COR_SERVICO = pr.COR,
                        PACOTE = a.PACOTE,
                        ID_EQUIPAMENTO = a.ID_EQUIPAMENTO,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ENCAIXE = a.ENCAIXE,
                        ID_REGIAO_CORPO = ar.ID_REGIAO_CORPO,
                        LOGIN = a.LOGIN,
                        DATA_INCLUSAO = a.DATA_INCLUSAO,
                        DATA_ATUALIZACAO = a.DATA_ATUALIZACAO
                    }).Where(a => (a.ID_CLIENTE == idCliente || idCliente == -1) && (a.Status == 1 || a.Status == 4 || a.Status == 2) && (a.FATURADO == false) && (a.BASICO == false)
                    && (a.ID_CONVENIO == null || a.CONVENIO == "PARTICULAR" || a.ID_CONVENIO == 9999)).ToList();

        }

        [Route("api/Appointment/Cliente/ConfirmadoPeriodo/{idCliente}/{dtInicial}/{dtFinal}")]
        public IEnumerable<AppointmentsDTO> GetAPPOINTMENTCLIENTE_CONFIRMADOPeriodo(int idCliente, DateTime dtInicial, DateTime dtFinal)
        {
            return GetAPPOINTMENTCLIENTE_CONFIRMADOAux(idCliente, dtInicial, dtFinal);
        }

        public IEnumerable<AppointmentsDTO> GetAPPOINTMENTCLIENTE_CONFIRMADOAux(int idCliente, DateTime? DataInicial, DateTime? DataFinal)
        {
            return (from a in db.Appointments
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ResourceID equals p.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on a.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    join ar in db.APPOINTMENTS_REGIAO_CORPO on a.UniqueID equals ar.ID_APPOINTMENTS into _ar
                    from ar in _ar.DefaultIfEmpty()
                    select new AppointmentsDTO()
                    {
                        UniqueID = a.UniqueID,
                        Status = a.Status,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        PROCEDIMENTO = pr.DESCRICAO,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.OPCAO_COBRANCA == 0 ? "PARTICULAR" :
                                a.OPCAO_COBRANCA == 1 ? "CONVÊNIO" : "",
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        ResourceID = a.ResourceID,
                        ID_CLIENTE = a.ID_CLIENTE,
                        RegioesCorpoIDs = a.RegioesCorpoIDs,
                        ID_PROCEDIMENTO = a.ID_PROCEDIMENTO,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        FATURADO = (ar.FATURADO == 0) ? false : true,
                        ID_SALA = a.ID_SALA,
                        ID_CONVENIO = a.ID_CONVENIO,
                        RegioesFaturadas = a.RegioesFaturadas,
                        BASICO = c.BASICO,
                        DESC_STATUS = a.Status == 0 ? "AGENDADO" :
                                      a.Status == 1 ? "CONFIRMADO" :
                                      a.Status == 2 ? "AGUARDANDO" :
                                      a.Status == 3 ? "EM ATENDIMENTO" :
                                      a.Status == 4 ? "ATENDIDO" :
                                      a.Status == 5 ? "CANCELADO" :
                                      a.Status == 6 ? "FALTOU" : "",
                        COR_SERVICO = pr.COR,
                        PACOTE = a.PACOTE,
                        ID_EQUIPAMENTO = a.ID_EQUIPAMENTO,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ENCAIXE = a.ENCAIXE,
                        ID_REGIAO_CORPO = ar.ID_REGIAO_CORPO,
                        LOGIN = a.LOGIN,
                        DATA_INCLUSAO = a.DATA_INCLUSAO,
                        DATA_ATUALIZACAO = a.DATA_ATUALIZACAO
                    }).Where(a => (a.ID_CLIENTE == idCliente || idCliente == -1) && (a.Status == 1 || a.Status == 4 || a.Status == 2) && (a.FATURADO == false) && (a.BASICO == false)
                    && (DataInicial == null || DbFunctions.TruncateTime(a.StartDate) >= DbFunctions.TruncateTime(DataInicial)) && (DataFinal == null || DbFunctions.TruncateTime(a.StartDate) <= DbFunctions.TruncateTime(DataFinal))).ToList();
        }

        // GET api/Appointment/Cliente/Confirmado/1
        [Route("api/Appointment/Clientes/Atendido")]
        public IEnumerable<AppointmentsDTO> GetAPPOINTMENTCLIENTE_ATENDIDO()
        {
            return (from a in db.Appointments
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ResourceID equals p.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on a.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    join ar in db.APPOINTMENTS_REGIAO_CORPO on a.UniqueID equals ar.ID_APPOINTMENTS into _ar
                    from ar in _ar.DefaultIfEmpty()
                    select new AppointmentsDTO()
                    {
                        UniqueID = a.UniqueID,
                        Status = a.Status,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        PROCEDIMENTO = pr.DESCRICAO,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.OPCAO_COBRANCA == 0 ? "PARTICULAR" :
                                a.OPCAO_COBRANCA == 1 ? "CONVÊNIO" : "",
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        ResourceID = a.ResourceID,
                        ID_CLIENTE = a.ID_CLIENTE,
                        RegioesCorpoIDs = a.RegioesCorpoIDs,
                        ID_PROCEDIMENTO = a.ID_PROCEDIMENTO,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        FATURADO = (ar.FATURADO == 0) ? false : true,
                        ID_SALA = a.ID_SALA,
                        ID_CONVENIO = a.ID_CONVENIO,
                        RegioesFaturadas = a.RegioesFaturadas,
                        DESC_STATUS = a.Status == 0 ? "AGENDADO" :
                                      a.Status == 1 ? "CONFIRMADO" :
                                      a.Status == 2 ? "AGUARDANDO" :
                                      a.Status == 3 ? "EM ATENDIMENTO" :
                                      a.Status == 4 ? "ATENDIDO" :
                                      a.Status == 5 ? "CANCELADO" :
                                      a.Status == 6 ? "FALTOU" : "",
                        COR_SERVICO = pr.COR,
                        PACOTE = a.PACOTE,
                        ID_EQUIPAMENTO = a.ID_EQUIPAMENTO,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ENCAIXE = a.ENCAIXE,
                        ID_REGIAO_CORPO = ar.ID_REGIAO_CORPO,
                        LOGIN = a.LOGIN,
                        DATA_INCLUSAO = a.DATA_INCLUSAO,
                        DATA_ATUALIZACAO = a.DATA_ATUALIZACAO
                    }).Where(a => a.Status == 4 && a.FATURADO == false && DbFunctions.TruncateTime(a.StartDate) == DbFunctions.TruncateTime(DateTime.Now)).ToList();
        }

        // GET api/Appointment/Profissional/1
        [Route("api/Appointment/Profissional/{idProfissional}")]
        public IEnumerable<AppointmentsDTO> GetAPPOINTMENTPROFISSIONAL(int idProfissional)
        {
            return (from a in db.Appointments
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ResourceID equals p.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on a.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    select new AppointmentsDTO()
                    {
                        UniqueID = a.UniqueID,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        PROCEDIMENTO = pr.DESCRICAO,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.OPCAO_COBRANCA == 0 ? "PARTICULAR" :
                                a.OPCAO_COBRANCA == 1 ? "CONVÊNIO" : "",
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        ResourceID = a.ResourceID,
                        ID_CLIENTE = a.ID_CLIENTE,
                        RegioesCorpoIDs = a.RegioesCorpoIDs,
                        ID_PROCEDIMENTO = a.ID_PROCEDIMENTO,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        ID_SALA = a.ID_SALA,
                        ID_CONVENIO = a.ID_CONVENIO,
                        RegioesFaturadas = a.RegioesFaturadas,
                        DESC_STATUS = a.Status == 0 ? "AGENDADO" :
                                      a.Status == 1 ? "CONFIRMADO" :
                                      a.Status == 2 ? "AGUARDANDO" :
                                      a.Status == 3 ? "EM ATENDIMENTO" :
                                      a.Status == 4 ? "ATENDIDO" :
                                      a.Status == 5 ? "CANCELADO" :
                                      a.Status == 6 ? "FALTOU" : "",
                        COR_SERVICO = pr.COR,
                        PACOTE = a.PACOTE,
                        ID_EQUIPAMENTO = a.ID_EQUIPAMENTO,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ENCAIXE = a.ENCAIXE,
                        LOGIN = a.LOGIN,
                        DATA_INCLUSAO = a.DATA_INCLUSAO,
                        DATA_ATUALIZACAO = a.DATA_ATUALIZACAO
                    }).Where(a => a.ResourceID == idProfissional).ToList();
        }

        // GET api/Appointment/Profissional/1/Cliente/1
        [Route("api/Appointment/Profissional/{idProfissional}/Cliente/{idCliente}")]
        public IEnumerable<AppointmentsDTO> GetAPPOINTMENTPROFCLI(int idProfissional, int idCliente)
        {
            return (from a in db.Appointments
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ResourceID equals p.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on a.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    select new AppointmentsDTO()
                    {
                        UniqueID = a.UniqueID,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        PROCEDIMENTO = pr.DESCRICAO,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.OPCAO_COBRANCA == 0 ? "PARTICULAR" :
                                a.OPCAO_COBRANCA == 1 ? "CONVÊNIO" : "",
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        ResourceID = a.ResourceID,
                        ID_CLIENTE = a.ID_CLIENTE,
                        RegioesCorpoIDs = a.RegioesCorpoIDs,
                        ID_PROCEDIMENTO = a.ID_PROCEDIMENTO,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        ID_SALA = a.ID_SALA,
                        ID_CONVENIO = a.ID_CONVENIO,
                        RegioesFaturadas = a.RegioesFaturadas,
                        DESC_STATUS = a.Status == 0 ? "AGENDADO" :
                                      a.Status == 1 ? "CONFIRMADO" :
                                      a.Status == 2 ? "AGUARDANDO" :
                                      a.Status == 3 ? "EM ATENDIMENTO" :
                                      a.Status == 4 ? "ATENDIDO" :
                                      a.Status == 5 ? "CANCELADO" :
                                      a.Status == 6 ? "FALTOU" : "",
                        COR_SERVICO = pr.COR,
                        PACOTE = a.PACOTE,
                        ID_EQUIPAMENTO = a.ID_EQUIPAMENTO,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ENCAIXE = a.ENCAIXE,
                        LOGIN = a.LOGIN,
                        DATA_INCLUSAO = a.DATA_INCLUSAO,
                        DATA_ATUALIZACAO = a.DATA_ATUALIZACAO
                    }).Where(a => a.ResourceID == idProfissional && a.ID_CLIENTE == idCliente).ToList();
        }

        // GET api/Appointment/Profissional/1/Cliente/1/Appointment/123
        [Route("api/Appointment/Profissional/{idProfissional}/Cliente/{idCliente}/Appointment/{idAppointment}")]
        public IEnumerable<AppointmentsDTO> GetAPPOINTMENTPROFCLIAPT(int idProfissional, int idCliente, int idAppointment)
        {
            return (from a in db.Appointments
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ResourceID equals p.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on a.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    select new AppointmentsDTO()
                    {
                        UniqueID = a.UniqueID,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        PROCEDIMENTO = pr.DESCRICAO,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.OPCAO_COBRANCA == 0 ? "PARTICULAR" :
                                a.OPCAO_COBRANCA == 1 ? "CONVÊNIO" : "",
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        ResourceID = a.ResourceID,
                        ID_CLIENTE = a.ID_CLIENTE,
                        RegioesCorpoIDs = a.RegioesCorpoIDs,
                        ID_PROCEDIMENTO = a.ID_PROCEDIMENTO,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        ID_SALA = a.ID_SALA,
                        ID_CONVENIO = a.ID_CONVENIO,
                        RegioesFaturadas = a.RegioesFaturadas,
                        DESC_STATUS = a.Status == 0 ? "AGENDADO" :
                                      a.Status == 1 ? "CONFIRMADO" :
                                      a.Status == 2 ? "AGUARDANDO" :
                                      a.Status == 3 ? "EM ATENDIMENTO" :
                                      a.Status == 4 ? "ATENDIDO" :
                                      a.Status == 5 ? "CANCELADO" :
                                      a.Status == 6 ? "FALTOU" : "",
                        COR_SERVICO = pr.COR,
                        PACOTE = a.PACOTE,
                        ID_EQUIPAMENTO = a.ID_EQUIPAMENTO,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ENCAIXE = a.ENCAIXE,
                        LOGIN = a.LOGIN,
                        DATA_INCLUSAO = a.DATA_INCLUSAO,
                        DATA_ATUALIZACAO = a.DATA_ATUALIZACAO
                    }).Where(a => a.ResourceID == idProfissional && a.ID_CLIENTE == idCliente && a.UniqueID == idAppointment).ToList();
        }

        // GET api/Appointment/Profissional/1/Cliente/1/Appointment/123
        [Route("api/Appointment/periodo/{idAppointment}")]
        public IEnumerable<AppointmentsDTO> GetAPPOINTMENT(int idAppointment, string dtInicial, string dtFinal)
        {
            var dataInicial = Convert.ToDateTime(dtInicial);
            var dataFinal = Convert.ToDateTime(dtFinal);
            return (from a in db.Appointments
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on a.ResourceID equals p.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on a.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join s in db.SALAs on a.ID_SALA equals s.ID_SALA into _s
                    from s in _s.DefaultIfEmpty()
                    select new AppointmentsDTO()
                    {
                        UniqueID = a.UniqueID,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        PROCEDIMENTO = pr.DESCRICAO,
                        CONVENIO = cv.DESCRICAO,
                        SALA = s.DESCRICAO,
                        OPCAO = a.OPCAO_COBRANCA == 0 ? "PARTICULAR" :
                                a.OPCAO_COBRANCA == 1 ? "CONVÊNIO" : "",
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        ResourceID = a.ResourceID,
                        ID_CLIENTE = a.ID_CLIENTE,
                        RegioesCorpoIDs = a.RegioesCorpoIDs,
                        ID_PROCEDIMENTO = a.ID_PROCEDIMENTO,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        ID_SALA = a.ID_SALA,
                        ID_CONVENIO = a.ID_CONVENIO,
                        RegioesFaturadas = a.RegioesFaturadas,
                        DESC_STATUS = a.Status == 0 ? "AGENDADO" :
                                      a.Status == 1 ? "CONFIRMADO" :
                                      a.Status == 2 ? "AGUARDANDO" :
                                      a.Status == 3 ? "EM ATENDIMENTO" :
                                      a.Status == 4 ? "ATENDIDO" :
                                      a.Status == 5 ? "CANCELADO" :
                                      a.Status == 6 ? "FALTOU" : "",
                        COR_SERVICO = pr.COR,
                        PACOTE = a.PACOTE,
                        ID_EQUIPAMENTO = a.ID_EQUIPAMENTO,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        ENCAIXE = a.ENCAIXE,
                        LOGIN = a.LOGIN,
                        DATA_INCLUSAO = a.DATA_INCLUSAO,
                        DATA_ATUALIZACAO = a.DATA_ATUALIZACAO
                    }).Where(a => ((a.StartDate >= dataInicial && a.StartDate <= dataFinal)
                                    ||
                                  (a.EndDate >= dataInicial && a.EndDate <= dataFinal)) && a.UniqueID != idAppointment).ToList();
        }

        [Route("api/appointments/atualizaDelete/{idAppointment}/{login}")]
        public bool GetAtualizaLoginDelete(int idAppointment, string login)
        {

            return true;
        }

        // GET api/Appointment/Profissional/1/Cliente/1/Appointment/123
        [Route("api/resumoagenda/{dtInicial}/{dtFinal}")]
        public IEnumerable<ResumoAgenda> GetRESUMO_AGENDA(DateTime dtInicial, DateTime dtFinal)
        {
            ResumoAgenda itemResumo = new ResumoAgenda();
            List<ResumoAgenda> resumo = new List<ResumoAgenda>();

            itemResumo.TOTAL = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                        new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            itemResumo.CONFIRMADOS = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE status = 1 and cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                         new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();

            itemResumo.CANCELADOS = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE status = 6 and cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                         new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();

            itemResumo.ATENDIDOS = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE status = 4 and cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                         new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            itemResumo.AGENDADOS = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE status = 0 and cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                         new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            itemResumo.OUTROS = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE status not in (0, 1, 4, 6) and cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                         new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            itemResumo.FALTOU = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE status = 7 and cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                         new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();


            resumo.Add(itemResumo);

            return resumo;
        }

        // GET api/Appointment/Profissional/1/Cliente/1/Appointment/123
        [Route("api/resumoagenda/profissional/{idProfissional}")]
        public IEnumerable<ResumoAgenda> GetRESUMO_AGENDA_PROFISSIONAL(int idProfissional)
        {
            DateTime dtIni;
            DateTime dtFim;
            //     mes++;
            dtIni = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + "01");
            //  if (mes == DateTime.Now.Month)
            dtFim = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString());
            //  else
            //      dtFim = Convert.ToDateTime((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.DaysInMonth(Convert.ToInt32(DateTime.Now.Year), Convert.ToInt32(DateTime.Now.Month))));

            ResumoAgenda itemResumo = new ResumoAgenda();
            List<ResumoAgenda> resumo = new List<ResumoAgenda>();

            itemResumo.TOTAL = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE ResourceId = @ID_PROFISSIONAL AND cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                        new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                                        new SqlParameter("@DATA_INI", dtIni.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFim.Date)).First();
            itemResumo.CONFIRMADOS = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE ResourceId = @ID_PROFISSIONAL AND status = 1 and cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                        new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                                        new SqlParameter("@DATA_INI", dtIni.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFim.Date)).First();

            itemResumo.CANCELADOS = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE ResourceId = @ID_PROFISSIONAL AND status = 6 and cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                        new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                                        new SqlParameter("@DATA_INI", dtIni.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFim.Date)).First();

            itemResumo.ATENDIDOS = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE ResourceId = @ID_PROFISSIONAL AND status = 4 and cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                        new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                                        new SqlParameter("@DATA_INI", dtIni.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFim.Date)).First();
            itemResumo.AGENDADOS = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE ResourceId = @ID_PROFISSIONAL AND status = 0 and cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                         new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                                         new SqlParameter("@DATA_INI", dtIni.Date),
                                                                         new SqlParameter("@DATA_FIM", dtFim.Date)).First();
            itemResumo.OUTROS = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE ResourceId = @ID_PROFISSIONAL AND status not in (0, 1, 4, 6) and cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                        new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                                        new SqlParameter("@DATA_INI", dtIni.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFim.Date)).First();
            itemResumo.FALTOU = db.Database.SqlQuery<int>("SELECT count(*) from dbo.Appointments " +
                                                         " WHERE ResourceId = @ID_PROFISSIONAL AND status = 7 and cast(startdate as date) >= @DATA_INI AND cast(startdate as date) <= @DATA_FIM",
                                                                         new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                                         new SqlParameter("@DATA_INI", dtIni.Date),
                                                                         new SqlParameter("@DATA_FIM", dtFim.Date)).First();


            resumo.Add(itemResumo);

            return resumo;
        }


        [Route("api/profissionalagenda/{dtInicial}/{dtFinal}")]
        public IEnumerable<ProfissionalAgenda> GetPROFISSIONAL_AGENDA(DateTime dtInicial, DateTime dtFinal)
        {
            return db.Database.SqlQuery<ProfissionalAgenda>(" select p.TITULO, (select count(*) from dbo.Appointments a " +
                                                            "                  left join dbo.PROFISSIONAL p on p.ID_PROFISSIONAL = a.ResourceID " +
                                                            "                   where a.ResourceID is not null and a.Status = 0 and a.ResourceID = a1.ResourceID AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS AGENDADOS, " +
                                                            "  (select count(*) from dbo.Appointments a " +
                                                            "  left join dbo.PROFISSIONAL p on p.ID_PROFISSIONAL = a.ResourceID " +
                                                            "  where a.ResourceID is not null and a.Status = 1 and a.ResourceID = a1.ResourceID AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS CONFIRMADOS, " +
                                                            "  (select count(*) from dbo.Appointments a " +
                                                            "  left join dbo.PROFISSIONAL p on p.ID_PROFISSIONAL = a.ResourceID " +
                                                            "  where a.ResourceID is not null and a.Status = 4 and a.ResourceID = a1.ResourceID AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS ATENDIDOS, " +
                                                            "  (select count(*) from dbo.Appointments a " +
                                                            "  left join dbo.PROFISSIONAL p on p.ID_PROFISSIONAL = a.ResourceID " +
                                                            "  where a.ResourceID is not null and a.Status = 6 and a.ResourceID = a1.ResourceID AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS CANCELADOS, " +
                                                            "  (select count(*) from dbo.Appointments a " +
                                                            "  left join dbo.PROFISSIONAL p on p.ID_PROFISSIONAL = a.ResourceID " +
                                                            "  where a.ResourceID is not null and a.Status not in (0, 1, 4, 6) and a.ResourceID = a1.ResourceID AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS OUTROS " +
                                                            "  FROM dbo.Appointments A1 left join PROFISSIONAL p on p.ID_PROFISSIONAL = a1.ResourceID " +
                                                            "  where a1.ResourceID is not null AND cast(A1.startdate as date) >= @DATA_INI AND cast(A1.startdate as date) <= @DATA_FIM " +
                                                            "  group by p.TITULO, a1.ResourceID" +
                                                            " ORDER BY P.TITULO ",
                                                            new SqlParameter("@DATA_INI", dtInicial.Date),
                                                            new SqlParameter("@DATA_FIM", dtFinal.Date)).ToList();
        }

        [Route("api/agendadia/{dtInicial}/{dtFinal}")]
        public IEnumerable<AgendaDia> GetAGENDA_DIA(DateTime dtInicial, DateTime dtFinal)
        {
            return db.Database.SqlQuery<AgendaDia>("  select day(A1.StartDate) AS DIA, (select count(*) from dbo.Appointments a " +
                                                   "   where a.ResourceID is not null and a.Status = 0 and day(a.StartDate) = day(A1.StartDate) AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS AGENDADOS, " +
                                                   " (select count(*) from dbo.Appointments a " +
                                                   " where a.ResourceID is not null and a.Status = 1 and day(a.StartDate) = day(A1.StartDate) AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS CONFIRMADOS, " +
                                                   " (select count(*) from dbo.Appointments a " +
                                                   " where a.ResourceID is not null and a.Status = 4 and day(a.StartDate) = day(A1.StartDate) AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS ATENDIDOS, " +
                                                   " (select count(*) from dbo.Appointments a " +
                                                   " where a.ResourceID is not null and a.Status = 6 and day(a.StartDate) = day(A1.StartDate) AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS CANCELADOS, " +
                                                   " (select count(*) from dbo.Appointments a " +
                                                   " where a.ResourceID is not null and a.Status = 7 and day(a.StartDate) = day(A1.StartDate) AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS FALTOU, " +
                                                   " (select count(*) from dbo.Appointments a " +
                                                   " where a.ResourceID is not null and a.Status not in (0, 1, 4, 6) and day(a.StartDate) = day(A1.StartDate) AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS OUTROS " +
                                                   " FROM Appointments A1 " +
                                                   " where a1.ResourceID is not null AND cast(A1.startdate as date) >= @DATA_INI AND cast(A1.startdate as date) <= @DATA_FIM " +
                                                   " group by day(A1.StartDate) " +
                                                   " ORDER BY day(A1.StartDate)",
                                                            new SqlParameter("@DATA_INI", dtInicial.Date),
                                                            new SqlParameter("@DATA_FIM", dtFinal.Date)).ToList();
        }

        [Route("api/agendadia/profissional/{idProfissional}")]
        public IEnumerable<AgendaDia> GetAGENDA_DIA_PROFISSIONAL(int idProfissional)
        {
            DateTime dtIni;
            DateTime dtFim;
            dtIni = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + "01");
            dtFim = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString());

            return db.Database.SqlQuery<AgendaDia>("  select day(A1.StartDate) AS DIA, (select count(*) from dbo.Appointments a " +
                                                   "   where a.ResourceID = @ID_PROFISSIONAL and a.Status = 0 and day(a.StartDate) = day(A1.StartDate) AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS AGENDADOS, " +
                                                   " (select count(*) from dbo.Appointments a " +
                                                   " where a.ResourceID = @ID_PROFISSIONAL and a.Status = 1 and day(a.StartDate) = day(A1.StartDate) AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS CONFIRMADOS, " +
                                                   " (select count(*) from dbo.Appointments a " +
                                                   " where a.ResourceID = @ID_PROFISSIONAL and a.Status = 4 and day(a.StartDate) = day(A1.StartDate) AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS ATENDIDOS, " +
                                                   " (select count(*) from dbo.Appointments a " +
                                                   " where a.ResourceID = @ID_PROFISSIONAL and a.Status = 6 and day(a.StartDate) = day(A1.StartDate) AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS CANCELADOS, " +
                                                   " (select count(*) from dbo.Appointments a " +
                                                   " where a.ResourceID = @ID_PROFISSIONAL and a.Status = 7 and day(a.StartDate) = day(A1.StartDate) AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS FALTOU, " +
                                                   " (select count(*) from dbo.Appointments a " +
                                                   " where a.ResourceID = @ID_PROFISSIONAL and a.Status not in (0, 1, 4, 6) and day(a.StartDate) = day(A1.StartDate) AND cast(A.startdate as date) >= @DATA_INI AND cast(A.startdate as date) <= @DATA_FIM) AS OUTROS " +
                                                   " FROM Appointments A1 " +
                                                   " where a1.ResourceID = @ID_PROFISSIONAL AND cast(A1.startdate as date) >= @DATA_INI AND cast(A1.startdate as date) <= @DATA_FIM " +
                                                   " group by day(A1.StartDate) " +
                                                   " ORDER BY day(A1.StartDate)",
                                                            new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                            new SqlParameter("@DATA_INI", dtIni.Date),
                                                            new SqlParameter("@DATA_FIM", dtFim.Date)).ToList();
        }

        // GET api/Appointment/Profissional/1/Cliente/1/Appointment/123
        //[Route("api/gestaovista/{dtInicial}/{dtFinal}")]
        //public IEnumerable<GestaoVista> GetGESTAO_VISTA(DateTime dtInicial, DateTime dtFinal)
        //{
        //    GestaoVista gestaoVista = new GestaoVista();
        //    List<GestaoVista> gv = new List<GestaoVista>();

        //    gestaoVista.ARECEBER = db.Database.SqlQuery<decimal?>(@"SELECT ISNULL(SUM(SALDO),0) ARECEBER
        //                                                              FROM DOCUMENTO_FINANCEIRO 
        //                                                             WHERE STATUS <> 'Q'  
        //                                                               AND TIPO = 'D'
        //                                                               AND CAST(DATA_VENCIMENTO AS DATE) >= @DATA_INI 
        //                                                               AND CAST(DATA_VENCIMENTO AS DATE) <= @DATA_FIM", 
        //                                                               new SqlParameter("@DATA_INI", dtInicial.Date),
        //                                                               new SqlParameter("@DATA_FIM", dtFinal.Date)).First();

        //    gestaoVista.APAGAR = db.Database.SqlQuery<decimal?>(@"SELECT ISNULL(SUM(SALDO),0) APAGAR
        //                                                              FROM DOCUMENTO_FINANCEIRO 
        //                                                             WHERE STATUS <> 'Q'  
        //                                                               AND TIPO = 'C'
        //                                                               AND CAST(DATA_VENCIMENTO AS DATE) >= @DATA_INI 
        //                                                               AND CAST(DATA_VENCIMENTO AS DATE) <= @DATA_FIM",
        //                                                               new SqlParameter("@DATA_INI", dtInicial.Date),
        //                                                               new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
        //    gestaoVista.RECEBIDO = db.Database.SqlQuery<decimal?>(@"SELECT ISNULL(SUM(VALOR),0) RECEBIDO 
        //                                                              FROM DOCUMENTO_FINANCEIRO 
        //                                                             WHERE STATUS = 'Q'  
        //                                                               AND TIPO = 'D'
        //                                                               AND CAST(DATA_VENCIMENTO AS DATE) >= @DATA_INI 
        //                                                               AND CAST(DATA_VENCIMENTO AS DATE) <= @DATA_FIM",
        //                                                               new SqlParameter("@DATA_INI", dtInicial.Date),
        //                                                               new SqlParameter("@DATA_FIM", dtFinal.Date)).First();

        //    gestaoVista.PAGOS = db.Database.SqlQuery<decimal?>(@"SELECT ISNULL(SUM(VALOR),0) PAGOS 
        //                                                              FROM DOCUMENTO_FINANCEIRO 
        //                                                             WHERE STATUS = 'Q'  
        //                                                               AND TIPO = 'C'
        //                                                               AND CAST(DATA_VENCIMENTO AS DATE) >= @DATA_INI 
        //                                                               AND CAST(DATA_VENCIMENTO AS DATE) <= @DATA_FIM",
        //                                                               new SqlParameter("@DATA_INI", dtInicial.Date),
        //                                                               new SqlParameter("@DATA_FIM", dtFinal.Date)).First();

        //    gestaoVista.NOVOS_PACIENTES = db.Database.SqlQuery<int?>(" select count(*) AS TOTAL from dbo.Appointments a " +
        //                                                            " where cast(a.StartDate as date) >= @DATA_INI and cast(a.StartDate as date) <= @DATA_FIM and a.Status = 4 "+
        //                                                            "   and a.ID_CLIENTE not in (select id_cliente from Appointments where cast(StartDate as date) < @DATA_INI and a.Status = 4)",
        //                                                                new SqlParameter("@DATA_INI", dtInicial.Date),
        //                                                                new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
        //    gestaoVista.TICKET_MEDIO = db.Database.SqlQuery<decimal?>("select cast( (sum(valor)/count(*)) as decimal(15,2)) AS TICKET_MEDIO from VENDA where status = 'C' and cast(data as date) >= @DATA_INI and cast(data as date) <= @DATA_FIM",
        //                                                                 new SqlParameter("@DATA_INI", dtInicial.Date),
        //                                                                new SqlParameter("@DATA_FIM", dtFinal.Date)).First();

        //    gestaoVista.BASKET = db.Database.SqlQuery<decimal?>(" select cast( ( " +
        //                                                   " select count(vi.ID_ITEM_VENDA) from dbo.VENDA_ITEM vi " +
        //                                                   " left join dbo.venda v on v.ID_VENDA = vi.ID_VENDA " +
        //                                                   " where cast(data as date) >= @DATA_INI and cast(data as date) <= @DATA_FIM and v.STATUS = 'C') / " +
        //                                                   " nullif( (select count(*) from dbo.venda where cast(data as date) >= @DATA_INI and cast(data as date) <= @DATA_FIM and STATUS = 'C'), 0) as decimal(15,2)) AS BASKET", 
        //                                                                 new SqlParameter("@DATA_INI", dtInicial.Date),
        //                                                                new SqlParameter("@DATA_FIM", dtFinal.Date)).First();

        //    IEnumerable<TaxaOcupacaoDia> txOcupDia;
        //    txOcupDia = db.Database.SqlQuery<TaxaOcupacaoDia>(" select P.COD_DIA_SEMANA, " +
        //                                                          " cast( ((select SUm(DATEDIFF(MINUTE, StartDate, EndDate)) / nullif( (datediff(week, dateadd(month, datediff(month, 0, getdate()), 0), getdate()) + 1),0) "+
        //                                                          "    from dbo.Appointments a where cast(a.StartDate as date) >= @DATA_INI and cast(a.StartDate as date) <= @DATA_FIM " +
        //                                                          "     and datepart(DW, a.StartDate) = p.COD_DIA_SEMANA "+
        //                                                          "     group by  datepart(DW, StartDate)) * 100) / "+
        //                                                          "     ((select  SUM(DATEDIFF(MINUTE, HORA_INICIAL_1, HORA_FINAL_1) + "+
        //                                                          "      DATEDIFF(MINUTE, HORA_INICIAL_2, HORA_FINAL_2) + "+
        //                                                          "      DATEDIFF(MINUTE, HORA_INICIAL_3, HORA_FINAL_3)) "+
        //                                                          "       from dbo.PROFISSIONAL_HORARIO P1 " +
        //                                                          "       WHERE P1.COD_DIA_SEMANA = P.COD_DIA_SEMANA) ) as decimal(15,2)) AS PERC " +
        //                                                          " from dbo.PROFISSIONAL_HORARIO P " +
        //                                                          " group by P.COD_DIA_SEMANA "+
        //                                                          " order by p.COD_DIA_SEMANA   ",
        //                                                          new SqlParameter("@DATA_INI", dtInicial.Date),
        //                                                          new SqlParameter("@DATA_FIM", dtFinal.Date)).ToList();
        //    decimal? percs = 0;
        //    if (txOcupDia.Count() > 0)
        //    {                
        //        foreach (TaxaOcupacaoDia tx in txOcupDia)
        //        {
        //            percs = percs + tx.PERC;
        //        }
        //        percs = percs / txOcupDia.Count();
        //    }

        //    gestaoVista.TAXA_OCUPACAO = percs;
        //    gestaoVista.REATIVADOS = db.Database.SqlQuery<int?>(@"SELECT COUNT(V.ID_CLIENTE) FROM VENDA V
        //                                                           WHERE cast(V.DATA as date) BETWEEN @DATA_INI and @DATA_FIM 
        //                                                            AND V.STATUS = 'C'
        //                                                            AND NOT EXISTS(SELECT * FROM VENDA VI
        //                                                                            WHERE cast(VI.DATA as date) BETWEEN DATEADD(mm, -24, @DATA_INI) AND DATEADD(dd, -1, @DATA_INI)
        //                                                                and VI.ID_CLIENTE = V.ID_CLIENTE )",
        //                                                                new SqlParameter("@DATA_INI", dtInicial.Date),
        //                                                                new SqlParameter("@DATA_FIM", dtFinal.Date)).First();


        //    gestaoVista.POSITIVACAO = db.Database.SqlQuery<int?>(@"SELECT (QTD_CLI_MES * 100)/QTD_TOTAL_CLI  POSITIVACAO
        //                                                               FROM
        //                                                             (
        //                                                             SELECT COUNT(V.ID_CLIENTE) QTD_TOTAL_CLI FROM VENDA V
        //                                                             WHERE cast(V.DATA as date) BETWEEN DATEADD(mm, -24, @DATA_INI) and DATEADD(dd, -1, @DATA_FIM) 
        //                                                             AND V.STATUS = 'C') CLI_ULT_MESES, 
        //                                                             (
        //                                                             SELECT COUNT(V.ID_CLIENTE) QTD_CLI_MES FROM VENDA V
        //                                                             WHERE cast(V.DATA as date) BETWEEN @DATA_INI and @DATA_FIM 
        //                                                             AND V.STATUS = 'C') CLI_MES",
        //                                                                new SqlParameter("@DATA_INI", dtInicial.Date),
        //                                                                new SqlParameter("@DATA_FIM", dtFinal.Date)).First();

        //    gestaoVista.HORIZONTALIZACAO = db.Database.SqlQuery<int?>(@"SELECT COUNT(VI.ID_PROCEDIMENTO) FROM VENDA_ITEM VI, VENDA V
        //                                                                WHERE VI.ID_VENDA = V.ID_VENDA
        //                                                                 AND V.STATUS = 'C'   
        //                                                                 AND CAST(V.DATA AS DATE) BETWEEN @DATA_INI and @DATA_FIM ",
        //                                                                new SqlParameter("@DATA_INI", dtInicial.Date),
        //                                                                new SqlParameter("@DATA_FIM", dtFinal.Date)).First();

        //    decimal? percent_faturamento = db.Database.SqlQuery<decimal?>(@"SELECT (SUM(VI.VALOR_PAGO) * 80) / 100                                                                      
        //                                                                     FROM VENDA V, VENDA_ITEM VI
        //                                                                    WHERE VI.ID_VENDA = V.ID_VENDA
        //                                                                        AND V.STATUS = 'C' 
        //                                                                        AND CAST(V.DATA AS DATE) BETWEEN @DATA_INI and @DATA_FIM ",
        //                                                                new SqlParameter("@DATA_INI", dtInicial.Date),
        //                                                                new SqlParameter("@DATA_FIM", dtFinal.Date)).First();

        //    IEnumerable<ClientePagamento> cliPagto;
        //    cliPagto = db.Database.SqlQuery<ClientePagamento>(@"SELECT V.ID_CLIENTE,  SUM(VI.VALOR_PAGO) VALOR_PAGO                                                                     
        //                                                          FROM VENDA V, VENDA_ITEM VI
        //                                                        WHERE VI.ID_VENDA = V.ID_VENDA
        //                                                         AND V.STATUS = 'C' 
        //                                                         AND CAST(V.DATA AS DATE) BETWEEN @DATA_INI and @DATA_FIM 
        //                                                        GROUP BY V.ID_CLIENTE 
        //                                                        order by 2 desc",
        //                                                          new SqlParameter("@DATA_INI", dtInicial.Date),
        //                                                          new SqlParameter("@DATA_FIM", dtFinal.Date)).ToList();
        //    int qtd_Cli = 0;
        //    decimal? valor_acumulado = 0;
        //    if (cliPagto.Count() > 0)
        //    {
        //        foreach (ClientePagamento tx in cliPagto)
        //        {
        //            qtd_Cli++;
        //            valor_acumulado =+ tx.VALOR_PAGO;
        //            if (valor_acumulado >= percent_faturamento)
        //                break;                         
        //        }                
        //    }
        //    gestaoVista.PARETTO = qtd_Cli;

        //    gv.Add(gestaoVista);

        //    return gv;
        //}

        // GET api/banco/conta/1/saldo/01-01-2015
        [Route("api/appointment/{idAppointment}/equipamento/{idEquipamento}/data/{quantidade}")]
        public String GetEQUIPAMENTO_APPOINTMENT(int idAppointment, int idEquipamento, string dtApt, int quantidade)
        {
            var data = Convert.ToDateTime(dtApt);
            int totalAlocado = db.Database.SqlQuery<int>(" select count(pe.ID_EQUIPAMENTO) as TOTAL from dbo.Appointments a " +
                                                         " join dbo.PROCEDIMENTO_EQUIPAMENTO pe on pe.ID_PROCEDIMENTO = a.ID_PROCEDIMENTO " +
                                                         " where @DATA between a.StartDate and a.EndDate " +
                                                         "   and pe.ID_EQUIPAMENTO = @ID_EQUIPAMENTO" +
                                                         "   and a.UniqueID <> @UniqueID",
                                                         new SqlParameter("@DATA", data),
                                                         new SqlParameter("@ID_EQUIPAMENTO", idEquipamento),
                                                         new SqlParameter("@UniqueID", idAppointment)).First();
            return (totalAlocado >= quantidade ? "T" : "F");
        }

        // PUT: api/ProfissionalHorario/PutPROFISSIONAL_HORARIO/5/2
        [ResponseType(typeof(void))]
        [Route("api/Appointment/PutAPPOINTMENT/{idAppointment}")]
        public IHttpActionResult PutAppointments(int idAppointment, Appointments appointments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idAppointment != appointments.UniqueID)
            {
                return BadRequest();
            }

            db.Entry(appointments).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentsExists(idAppointment))
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

        [Route("api/taxaocupacaoprofissional/{dtInicial}/{dtFinal}")]
        public IEnumerable<TaxaOcupacaoProfissional> GetTaxaOcupacao_Profissional(DateTime dtInicial, DateTime dtFinal)
        {
            return db.Database.SqlQuery<TaxaOcupacaoProfissional>(@"select pp.NOME as TITULO,
                                                                           isnull(cast(avg(pp1.perc) as decimal(15,2)),0) as TAXA_OCUPACAO                                                                    
                                                                    from profissional pp
                                                                    left join (
                                                                         select p.id_profissional,
                                                                                cast( ((select SUm(DATEDIFF(MINUTE, StartDate, EndDate)) / nullif( (datediff(week, dateadd(month, datediff(month, 0, getdate()), 0), getdate()) + 1),0) 
                                                                                         from dbo.Appointments a 
			   	                                                                        where cast(a.StartDate as date) >= cast(@DATA_INI as date) and cast(a.StartDate as date) <= cast(@DATA_FIM as date)
                                                                                         and datepart(DW, a.StartDate) = p.COD_DIA_SEMANA
                                                                                         and a.ResourceID = p.ID_PROFISSIONAL 
                                                                                        group by  datepart(DW, StartDate)) * 100) / 
                                                                                ((select  SUM(DATEDIFF(MINUTE, HORA_INICIAL_1, HORA_FINAL_1) + 
                                                                                          DATEDIFF(MINUTE, HORA_INICIAL_2, HORA_FINAL_2) + 
                                                                                          DATEDIFF(MINUTE, HORA_INICIAL_3, HORA_FINAL_3))
                                                                                    from dbo.PROFISSIONAL_HORARIO P1 
                                                                                   WHERE P1.COD_DIA_SEMANA = P.COD_DIA_SEMANA
                                                                                     and P1.ID_PROFISSIONAL = p.ID_PROFISSIONAL    ) ) as decimal(15,2)) AS PERC 
                                                                          from dbo.PROFISSIONAL_HORARIO P 
                                                                         group by P.ID_PROFISSIONAL, P.COD_DIA_SEMANA
                                                                            ) as pp1 on pp1.ID_PROFISSIONAL = pp.ID_PROFISSIONAL
                                                                    group by pp.ID_PROFISSIONAL, pp.NOME 
                                                                    order by isnull(avg(pp1.perc),0) desc ",
                                                            new SqlParameter("@DATA_INI", dtInicial.Date),
                                                            new SqlParameter("@DATA_FIM", dtFinal.Date)).ToList();
        }

        [Route("api/faturamentoservico/{dtInicial}/{dtFinal}")]
        public IEnumerable<FaturamentoServico> Getfaturamentoservico(DateTime dtInicial, DateTime dtFinal)
        {
            return db.Database.SqlQuery<FaturamentoServico>(@"SELECT P.DESCRICAO SERVICO, SUM(VI.VALOR_PAGO) VALOR_FATURAMENTO
                                                              FROM VENDA V, VENDA_ITEM VI, PROCEDIMENTO P
                                                             WHERE P.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO
                                                               AND V.ID_VENDA = VI.ID_VENDA 
                                                               AND VI.VALOR_PAGO > 0
                                                               AND STATUS = 'C'
                                                               --AND CAST(V.DATA AS DATE) BETWEEN CAST('01/01/2016' AS DATE) AND CAST('31/01/2016' AS DATE)
                                                               AND CAST(V.DATA AS DATE) BETWEEN @DATA_INI AND @DATA_FIM
                                                              GROUP BY P.DESCRICAO 
                                                              order by SUM(VI.VALOR_PAGO) desc ",
                                                            new SqlParameter("@DATA_INI", dtInicial.Date),
                                                            new SqlParameter("@DATA_FIM", dtFinal.Date)
                                                            ).ToList();
        }

        [Route("api/agenda/relatorioagenda/{dtInicial}/{dtFinal}/{idProfissional}/{status}")]
        public IEnumerable<ListaAgenda> GetListaAgenda(DateTime dtInicial, DateTime dtFinal, int idProfissional, string status)//, int id_profissional, int status)
        {
            string vStatus = string.Empty;
            System.Text.StringBuilder sbSql = new System.Text.StringBuilder();

            sbSql.Append(@"SELECT * FROM (
                            SELECT ROW_NUMBER() OVER(ORDER BY startdate) AS NUM_ROW,
                                                             CAST(startdate AS DATE) DATA, 
                                                             convert(nvarchar(5), A.StartDate,108) HORA_INI, 
                                                             convert(nvarchar(5), A.EndDate,108) HORA_FIN, 
                                                              CASE A.STATUS
	                                                             WHEN 0 THEN 'Agendado' 
	                                                             WHEN 1 THEN 'Confirmado'
	                                                             WHEN 2 THEN 'Aguardando'
		                                                         WHEN 3 THEN 'Em Atendimento' 
		                                                         WHEN 4 THEN 'Atendido'		                                                         
		                                                         WHEN 5 THEN 'Cancelado'
		                                                         WHEN 6 THEN 'Faltou'
	                                                           END STATUS , 
                                                               C.NOME CLIENTE, P.NOME PROFISSIONAL, PR.DESCRICAO PROCEDIMENTO,
                                                               A.RegioesCorpoIDs AREA_CORPO, A.ID_PROCEDIMENTO, A.LOGIN,
                                                               A.DATA_INCLUSAO, A.DATA_ATUALIZACAO, DESCRIPTION OBSERVACAO,
															   CV.DESCRICAO AS CONVENIO     
                                                         FROM Appointments A
														 LEFT JOIN CLIENTE C ON C.ID_CLIENTE = A.ID_CLIENTE
														 LEFT JOIN PROFISSIONAL P ON P.ID_PROFISSIONAL = A.ResourceID
														 LEFT JOIN PROCEDIMENTO PR ON PR.ID_PROCEDIMENTO = A.ID_PROCEDIMENTO
														 LEFT JOIN CONVENIO CV ON cv.ID_CONVENIO = A.ID_CONVENIO
                                                        WHERE (A.ResourceID = @ID_PROFISSIONAL OR @ID_PROFISSIONAL = -1)                                                                                                               
                                                          AND cast(startdate as date) >= @DATA_INI
                                                          AND cast(startdate as date) <= @DATA_FIM ");

            if (status != string.Empty && status != "-1")
            {
                sbSql.Append(" AND A.STATUS IN (");
                sbSql.Append(status);
                sbSql.Append(")");
            }
            if (status.Contains("10") || status == "-1")
            {
                sbSql.Append(" UNION ALL ");
                sbSql.Append(@" SELECT ROW_NUMBER() OVER(ORDER BY startdate) AS NUM_ROW,
                                                             CAST(startdate AS DATE) DATA, 
                                                             convert(nvarchar(5), A.StartDate,108) HORA_INI, 
                                                             convert(nvarchar(5), A.EndDate,108) HORA_FIN, 
                                                             'Excluído' as STATUS , 
                                                               C.NOME CLIENTE, P.NOME PROFISSIONAL, PR.DESCRICAO PROCEDIMENTO,
                                                               A.RegioesCorpoIDs AREA_CORPO, A.ID_PROCEDIMENTO, A.LOGIN,
                                                               A.DATA_INCLUSAO, A.DATA_ATUALIZACAO, DESCRIPTION OBSERVACAO,
															   CV.DESCRICAO AS CONVENIO          
                                                         FROM APPOINTMENT_DELETE A
                                                         LEFT JOIN CLIENTE C ON C.ID_CLIENTE = A.ID_CLIENTE
														 LEFT JOIN PROFISSIONAL P ON P.ID_PROFISSIONAL = A.ResourceID
														 LEFT JOIN PROCEDIMENTO PR ON PR.ID_PROCEDIMENTO = A.ID_PROCEDIMENTO
														 LEFT JOIN CONVENIO CV ON cv.ID_CONVENIO = A.ID_CONVENIO
                                                        WHERE (A.ResourceID = @ID_PROFISSIONAL OR @ID_PROFISSIONAL = -1)                                                                                                               
                                                          AND cast(startdate as date) >= @DATA_INI
                                                          AND cast(startdate as date) <= @DATA_FIM ");
            }
            sbSql.Append(@") A  ORDER BY DATA ");

            return db.Database.SqlQuery<ListaAgenda>(sbSql.ToString(),
                                                         new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                         new SqlParameter("@DATA_INI", dtInicial.Date),
                                                         new SqlParameter("@DATA_FIM", dtFinal.Date)).ToList();
        }

        // POST: api/Appointments
        [ResponseType(typeof(Appointments))]
        public IHttpActionResult PostAppointments(Appointments appointments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointments);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AppointmentsExists(appointments.UniqueID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = appointments.UniqueID }, appointments);
        }

        // DELETE: api/Appointments/5
        [ResponseType(typeof(Appointments))]
        public IHttpActionResult DeleteAppointments(int id)
        {
            Appointments appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointments);
            db.SaveChanges();

            return Ok(appointments);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentsExists(int id)
        {
            return db.Appointments.Count(e => e.UniqueID == id) > 0;
        }
    }
}