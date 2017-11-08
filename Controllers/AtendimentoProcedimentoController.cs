using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SevenMedicalApi.Models;
using SisMedApi.Models;

namespace SevenMedicalApi.Controllers
{

    public class ATENDIMENTO_PROCEDIMENTODTO
    {
        public int ID_ATENDIMENTO_PROCEDIMENTO { get; set; }
        public int ID_ATENDIMENTO { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public Nullable<int> ID_REGIAO_CORPO { get; set; }
        public Nullable<int> ID_EQUIPAMENTO { get; set; }
        public Nullable<int> ID_APPOINTMENT { get; set; }
        public string LOGIN { get; set; }
        public string STATUS { get; set; }
        public Nullable<decimal> VALOR { get; set; }
        public string PROCEDIMENTO { get; set; }
        public string REGIAO { get; set; }
        public string EQUIPAMENTO { get; set; }
        public string DESC_STATUS { get; set; }
        public string CONVENIO { get; set; }
        public Nullable<int> ID_CLIENTE { get; set; }
        public Nullable<int> ID_PROFISSIONAL { get; set; }
        public string PROFISSIONAL { get; set; }
        public string CLIENTE { get; set; }
        public Nullable<DateTime> DATA { get; set; }
        public int NUM_SESSOES { get; set; }
        public bool BASICO { get; set; }
    }

    public class AtendimentoProcedimentoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/atendimento/1/procedimentos
        [Route("api/atendimento/{idAtendimento}/procedimentos")]
        public IEnumerable<ATENDIMENTO_PROCEDIMENTODTO> GetATENDIMENTO_PROCEDIMENTOS(int idAtendimento)
        {
            return (from ap in db.ATENDIMENTO_PROCEDIMENTO
                    join a in db.ATENDIMENTOes on ap.ID_ATENDIMENTO equals a.ID_ATENDIMENTO
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join pf in db.PROFISSIONALs on a.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on ap.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join rc in db.REGIAO_CORPO on ap.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    join prc in db.PROCEDIMENTO_REGIAO_CORPO on ap.ID_REGIAO_CORPO equals prc.ID_REGIAO_CORPO into _prc
                    from prc in _prc.Where(prc => prc.ID_PROCEDIMENTO == ap.ID_PROCEDIMENTO).DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on ap.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    select new ATENDIMENTO_PROCEDIMENTODTO()
                    {
                        ID_ATENDIMENTO_PROCEDIMENTO = ap.ID_ATENDIMENTO_PROCEDIMENTO,
                        ID_ATENDIMENTO = ap.ID_ATENDIMENTO,
                        ID_PROCEDIMENTO = ap.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = ap.ID_REGIAO_CORPO,
                        ID_EQUIPAMENTO = ap.ID_EQUIPAMENTO,
                        LOGIN = ap.LOGIN,
                        STATUS = ap.STATUS,
                        PROCEDIMENTO = pr.DESCRICAO,
                        REGIAO = rc.REGIAO,
                        EQUIPAMENTO = e.DESCRICAO,
                        DESC_STATUS = ap.STATUS == "A" ? "ABERTO" :
                                      ap.STATUS == "F" ? "FATURADO" : "",
                        CONVENIO = cv.DESCRICAO,
                        ID_CLIENTE = a.ID_CLIENTE,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        PROFISSIONAL = pf.NOME,
                        VALOR = prc.VALOR,
                        CLIENTE = c.NOME,
                        DATA = a.DATA_HORA_FIM,
                        NUM_SESSOES = prc.NUM_SESSOES
                    }).Where(ap => ap.ID_ATENDIMENTO == idAtendimento && ap.NUM_SESSOES == 1).ToList();
        }

        // GET api/atendimento/procedimentos/cliente/1
        [Route("api/atendimento/procedimentos/cliente/{idCliente}")]
        public IEnumerable<ATENDIMENTO_PROCEDIMENTODTO> GetATENDIMENTO_PROCEDIMENTOS_CLIENTE(int idCliente)
        {
            return (from ap in db.ATENDIMENTO_PROCEDIMENTO
                    join a in db.ATENDIMENTOes on ap.ID_ATENDIMENTO equals a.ID_ATENDIMENTO
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join pf in db.PROFISSIONALs on a.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on ap.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join rc in db.REGIAO_CORPO on ap.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    join prc in db.PROCEDIMENTO_REGIAO_CORPO on ap.ID_REGIAO_CORPO equals prc.ID_REGIAO_CORPO into _prc
                    from prc in _prc.Where(prc => prc.ID_PROCEDIMENTO == ap.ID_PROCEDIMENTO).DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on ap.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    select new ATENDIMENTO_PROCEDIMENTODTO()
                    {
                        ID_ATENDIMENTO_PROCEDIMENTO = ap.ID_ATENDIMENTO_PROCEDIMENTO,
                        ID_ATENDIMENTO = ap.ID_ATENDIMENTO,
                        ID_PROCEDIMENTO = ap.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = ap.ID_REGIAO_CORPO,
                        ID_EQUIPAMENTO = ap.ID_EQUIPAMENTO,
                        LOGIN = ap.LOGIN,
                        STATUS = ap.STATUS,
                        PROCEDIMENTO = pr.DESCRICAO,
                        REGIAO = rc.REGIAO,
                        EQUIPAMENTO = e.DESCRICAO,
                        DESC_STATUS = ap.STATUS == "A" ? "ABERTO" :
                                      ap.STATUS == "F" ? "FATURADO" : "",
                        CONVENIO = cv.DESCRICAO,
                        ID_CLIENTE = a.ID_CLIENTE,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        PROFISSIONAL = pf.NOME,
                        VALOR = prc.VALOR,
                        CLIENTE = c.NOME,
                        DATA = a.DATA_HORA_FIM,
                        NUM_SESSOES = prc.NUM_SESSOES
                    }).Where(at => at.ID_CLIENTE == idCliente && at.NUM_SESSOES == 1).ToList();
        }

        // GET api/atendimento/procedimentos/faturar/cliente/1
        [Route("api/atendimento/procedimentos/faturar/cliente/{idCliente}/{dtInicial}/{dtFinal}")]
        public IEnumerable<ATENDIMENTO_PROCEDIMENTODTO> GetPROCEDIMENTOS_FATURAR_CLIENTE(int idCliente, DateTime dtInicial, DateTime dtFinal)
        {
            return (from ap in db.ATENDIMENTO_PROCEDIMENTO
                    join a in db.ATENDIMENTOes on ap.ID_ATENDIMENTO equals a.ID_ATENDIMENTO
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join pf in db.PROFISSIONALs on a.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on ap.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join rc in db.REGIAO_CORPO on ap.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    join prc in db.PROCEDIMENTO_REGIAO_CORPO on ap.ID_REGIAO_CORPO equals prc.ID_REGIAO_CORPO into _prc
                    from prc in _prc.Where(prc => prc.ID_PROCEDIMENTO == ap.ID_PROCEDIMENTO).DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on ap.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    select new ATENDIMENTO_PROCEDIMENTODTO()
                    {
                        ID_ATENDIMENTO_PROCEDIMENTO = ap.ID_ATENDIMENTO_PROCEDIMENTO,
                        ID_ATENDIMENTO = ap.ID_ATENDIMENTO,
                        ID_PROCEDIMENTO = ap.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = ap.ID_REGIAO_CORPO,
                        ID_EQUIPAMENTO = ap.ID_EQUIPAMENTO,
                        LOGIN = ap.LOGIN,
                        STATUS = ap.STATUS,
                        PROCEDIMENTO = pr.DESCRICAO,
                        REGIAO = rc.REGIAO,
                        EQUIPAMENTO = e.DESCRICAO,
                        DESC_STATUS = ap.STATUS == "A" ? "ABERTO" :
                                      ap.STATUS == "F" ? "FATURADO" : "",
                        CONVENIO = cv.DESCRICAO,
                        ID_CLIENTE = a.ID_CLIENTE,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        PROFISSIONAL = pf.NOME,
                        VALOR = prc.VALOR,
                        CLIENTE = c.NOME,
                        DATA = a.DATA_HORA_FIM,
                        NUM_SESSOES = prc.NUM_SESSOES,
                        BASICO = c.BASICO
                    }).Where(at => (at.ID_CLIENTE == idCliente || idCliente == -1) && at.BASICO == false && at.STATUS == "A" && at.NUM_SESSOES == 1 && DbFunctions.TruncateTime(at.DATA) >= DbFunctions.TruncateTime(dtInicial) && DbFunctions.TruncateTime(at.DATA) <= DbFunctions.TruncateTime(dtFinal)).ToList();
        }

        // GET api/atendimento/procedimentos/faturar/cliente/1
        [Route("api/atendimento/procedimentos/faturar/cliente/{idCliente}")]
        public IEnumerable<ATENDIMENTO_PROCEDIMENTODTO> GetPROCEDIMENTOS_FATURAR_CLIENTE(int idCliente)
        {
            return (from ap in db.ATENDIMENTO_PROCEDIMENTO
                    join a in db.ATENDIMENTOes on ap.ID_ATENDIMENTO equals a.ID_ATENDIMENTO
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join pf in db.PROFISSIONALs on a.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on ap.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join rc in db.REGIAO_CORPO on ap.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    join prc in db.PROCEDIMENTO_REGIAO_CORPO on ap.ID_REGIAO_CORPO equals prc.ID_REGIAO_CORPO into _prc
                    from prc in _prc.Where(prc => prc.ID_PROCEDIMENTO == ap.ID_PROCEDIMENTO).DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on ap.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    select new ATENDIMENTO_PROCEDIMENTODTO()
                    {
                        ID_ATENDIMENTO_PROCEDIMENTO = ap.ID_ATENDIMENTO_PROCEDIMENTO,
                        ID_ATENDIMENTO = ap.ID_ATENDIMENTO,
                        ID_PROCEDIMENTO = ap.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = ap.ID_REGIAO_CORPO,
                        ID_EQUIPAMENTO = ap.ID_EQUIPAMENTO,
                        LOGIN = ap.LOGIN,
                        STATUS = ap.STATUS,
                        PROCEDIMENTO = pr.DESCRICAO,
                        REGIAO = rc.REGIAO,
                        EQUIPAMENTO = e.DESCRICAO,
                        DESC_STATUS = ap.STATUS == "A" ? "ABERTO" :
                                      ap.STATUS == "F" ? "FATURADO" : "",
                        CONVENIO = cv.DESCRICAO,
                        ID_CLIENTE = a.ID_CLIENTE,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        PROFISSIONAL = pf.NOME,
                        VALOR = prc.VALOR,
                        CLIENTE = c.NOME,
                        DATA = a.DATA_HORA_FIM,
                        NUM_SESSOES = prc.NUM_SESSOES
                    }).Where(at => at.ID_CLIENTE == idCliente && at.STATUS == "A" && at.NUM_SESSOES == 1).ToList();
        }

        [Route("api/atendimento/procedimentos/faturar/aberto")]
        public IEnumerable<ATENDIMENTO_PROCEDIMENTODTO> GetPROCEDIMENTOS_FATURAR()
        {
            return (from ap in db.ATENDIMENTO_PROCEDIMENTO
                    join a in db.ATENDIMENTOes on ap.ID_ATENDIMENTO equals a.ID_ATENDIMENTO
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join pf in db.PROFISSIONALs on a.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on ap.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join rc in db.REGIAO_CORPO on ap.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    join prc in db.PROCEDIMENTO_REGIAO_CORPO on ap.ID_REGIAO_CORPO equals prc.ID_REGIAO_CORPO into _prc
                    from prc in _prc.Where(prc => prc.ID_PROCEDIMENTO == ap.ID_PROCEDIMENTO).DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on ap.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    select new ATENDIMENTO_PROCEDIMENTODTO()
                    {
                        ID_ATENDIMENTO_PROCEDIMENTO = ap.ID_ATENDIMENTO_PROCEDIMENTO,
                        ID_ATENDIMENTO = ap.ID_ATENDIMENTO,
                        ID_PROCEDIMENTO = ap.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = ap.ID_REGIAO_CORPO,
                        ID_EQUIPAMENTO = ap.ID_EQUIPAMENTO,
                        STATUS = ap.STATUS,
                        PROCEDIMENTO = pr.DESCRICAO,
                        REGIAO = rc.REGIAO,
                        EQUIPAMENTO = e.DESCRICAO,
                        DESC_STATUS = ap.STATUS == "A" ? "ABERTO" :
                                      ap.STATUS == "F" ? "FATURADO" : "",
                        CONVENIO = cv.DESCRICAO,
                        ID_CLIENTE = a.ID_CLIENTE,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        PROFISSIONAL = pf.NOME,
                        VALOR = prc.VALOR,
                        CLIENTE = c.NOME,
                        DATA = a.DATA_HORA_FIM,
                        NUM_SESSOES = prc.NUM_SESSOES
                    }).Where(at => at.STATUS == "A" && DbFunctions.TruncateTime(at.DATA) == DbFunctions.TruncateTime(DateTime.Now) && at.NUM_SESSOES == 1).ToList();
        }

        // GET api/atendimento/procedimento/item/1
        [Route("api/atendimento/procedimento/item/{idAtendimentoProcedimento}")]
        public IEnumerable<ATENDIMENTO_PROCEDIMENTODTO> GetATENDIMENTO_PROCEDIMENTO_ITEM(int idAtendimentoProcedimento)
        {
            return (from ap in db.ATENDIMENTO_PROCEDIMENTO
                    join a in db.ATENDIMENTOes on ap.ID_ATENDIMENTO equals a.ID_ATENDIMENTO
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join pf in db.PROFISSIONALs on a.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL
                    join pr in db.PROCEDIMENTOes on ap.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join rc in db.REGIAO_CORPO on ap.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    join prc in db.PROCEDIMENTO_REGIAO_CORPO on ap.ID_REGIAO_CORPO equals prc.ID_REGIAO_CORPO into _prc
                    from prc in _prc.Where(prc => prc.ID_PROCEDIMENTO == ap.ID_PROCEDIMENTO).DefaultIfEmpty()
                    join e in db.EQUIPAMENTOes on ap.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO into _e
                    from e in _e.DefaultIfEmpty()
                    select new ATENDIMENTO_PROCEDIMENTODTO()
                    {
                        ID_ATENDIMENTO_PROCEDIMENTO = ap.ID_ATENDIMENTO_PROCEDIMENTO,
                        ID_ATENDIMENTO = ap.ID_ATENDIMENTO,
                        ID_PROCEDIMENTO = ap.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = ap.ID_REGIAO_CORPO,
                        ID_EQUIPAMENTO = ap.ID_EQUIPAMENTO,
                        LOGIN = ap.LOGIN,
                        STATUS = ap.STATUS,
                        PROCEDIMENTO = pr.DESCRICAO,
                        REGIAO = rc.REGIAO,
                        EQUIPAMENTO = e.DESCRICAO,
                        DESC_STATUS = ap.STATUS == "A" ? "ABERTO" :
                                      ap.STATUS == "F" ? "FATURADO" : "",
                        CONVENIO = cv.DESCRICAO,
                        ID_CLIENTE = a.ID_CLIENTE,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        PROFISSIONAL = pf.NOME,
                        VALOR = prc.VALOR,
                        CLIENTE = c.NOME,
                        DATA = a.DATA_HORA_FIM,
                        NUM_SESSOES = prc.NUM_SESSOES
                    }).Where(at => at.ID_ATENDIMENTO_PROCEDIMENTO == idAtendimentoProcedimento && at.NUM_SESSOES == 1).ToList();
        }

        // PUT: api/AtendimentoProcedimento/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutATENDIMENTO_PROCEDIMENTO(int id, ATENDIMENTO_PROCEDIMENTO aTENDIMENTO_PROCEDIMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aTENDIMENTO_PROCEDIMENTO.ID_ATENDIMENTO_PROCEDIMENTO)
            {
                return BadRequest();
            }

            db.Entry(aTENDIMENTO_PROCEDIMENTO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ATENDIMENTO_PROCEDIMENTOExists(id))
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

        // POST: api/AtendimentoProcedimento
        [ResponseType(typeof(ATENDIMENTO_PROCEDIMENTO))]
        public IHttpActionResult PostATENDIMENTO_PROCEDIMENTO(ATENDIMENTO_PROCEDIMENTO aTENDIMENTO_PROCEDIMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ATEND_PROC;").First();

            aTENDIMENTO_PROCEDIMENTO.ID_ATENDIMENTO_PROCEDIMENTO = NextValue;

            db.ATENDIMENTO_PROCEDIMENTO.Add(aTENDIMENTO_PROCEDIMENTO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ATENDIMENTO_PROCEDIMENTOExists(aTENDIMENTO_PROCEDIMENTO.ID_ATENDIMENTO_PROCEDIMENTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }    

            return CreatedAtRoute("DefaultApi", new { id = aTENDIMENTO_PROCEDIMENTO.ID_ATENDIMENTO_PROCEDIMENTO }, aTENDIMENTO_PROCEDIMENTO);
        }

        // DELETE: api/AtendimentoProcedimento/5
        [ResponseType(typeof(ATENDIMENTO_PROCEDIMENTO))]
        public IHttpActionResult DeleteATENDIMENTO_PROCEDIMENTO(int id)
        {
            ATENDIMENTO_PROCEDIMENTO aTENDIMENTO_PROCEDIMENTO = db.ATENDIMENTO_PROCEDIMENTO.Find(id);
            if (aTENDIMENTO_PROCEDIMENTO == null)
            {
                return NotFound();
            }

            db.ATENDIMENTO_PROCEDIMENTO.Remove(aTENDIMENTO_PROCEDIMENTO);
            db.SaveChanges();

            return Ok(aTENDIMENTO_PROCEDIMENTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ATENDIMENTO_PROCEDIMENTOExists(int id)
        {
            return db.ATENDIMENTO_PROCEDIMENTO.Count(e => e.ID_ATENDIMENTO_PROCEDIMENTO == id) > 0;
        }
    }
}