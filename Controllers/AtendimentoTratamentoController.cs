using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SevenMedicalApi.Models;
using SisMedApi.Models;

namespace SevenMedicalApi.Controllers
{
    public class ATENDIMENTO_TRATAMENTODTO
    {
        public int ID_TRATAMENTO { get; set; }
        public int ID_ATENDIMENTO { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public Nullable<int> ID_REGIAO_CORPO { get; set; }
        public Nullable<int> NUM_SESSOES { get; set; }
        public string PROFISSIONAL { get; set; }
        public string CLIENTE { get; set; }
        public string PROCEDIMENTO { get; set; }
        public string REGIAO { get; set; }
        public string LOGIN { get; set; }
        public Nullable<decimal> VALOR_NEGOCIADO { get; set; }
    }
    public class AtendimentoTratamentoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/atendimento/1/tratamentos
        [Route("api/atendimento/{idAtendimento}/tratamentos")]
        public IEnumerable<ATENDIMENTO_TRATAMENTODTO> GetATENDIMENTO_TRATAMENTOS(int idAtendimento)
        {
            return (from at in db.ATENDIMENTO_TRATAMENTO
                    join a in db.ATENDIMENTOes on at.ID_ATENDIMENTO equals a.ID_ATENDIMENTO
                    join p in db.PROFISSIONALs on a.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join pr in db.PROCEDIMENTOes on at.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join rc in db.REGIAO_CORPO on at.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    select new ATENDIMENTO_TRATAMENTODTO()
                    {
                        ID_TRATAMENTO = at.ID_TRATAMENTO,
                        ID_ATENDIMENTO = at.ID_ATENDIMENTO,
                        ID_PROCEDIMENTO = at.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = at.ID_REGIAO_CORPO,
                        NUM_SESSOES = at.NUM_SESSOES,
                        PROFISSIONAL = p.NOME,
                        CLIENTE = c.NOME,
                        PROCEDIMENTO = pr.DESCRICAO,
                        LOGIN = at.LOGIN,
                        REGIAO = rc.REGIAO,
                        VALOR_NEGOCIADO = at.VALOR_NEGOCIADO
                    }).Where(at => at.ID_ATENDIMENTO == idAtendimento).ToList();
        }

        // GET api/atendimento/1/tratamento/1
        [Route("api/atendimento/{idAtendimento}/tratamento/{idTratamento}")]
        public IEnumerable<ATENDIMENTO_TRATAMENTODTO> GetATENDIMENTO_RECEITA(int idAtendimento, int idTratamento)
        {
            return (from at in db.ATENDIMENTO_TRATAMENTO
                    join a in db.ATENDIMENTOes on at.ID_ATENDIMENTO equals a.ID_ATENDIMENTO
                    join p in db.PROFISSIONALs on a.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    join c in db.CLIENTES on a.ID_CLIENTE equals c.ID_CLIENTE
                    join pr in db.PROCEDIMENTOes on at.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join rc in db.REGIAO_CORPO on at.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    select new ATENDIMENTO_TRATAMENTODTO()
                    {
                        ID_TRATAMENTO = at.ID_TRATAMENTO,
                        ID_ATENDIMENTO = at.ID_ATENDIMENTO,
                        ID_PROCEDIMENTO = at.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = at.ID_REGIAO_CORPO,
                        NUM_SESSOES = at.NUM_SESSOES,
                        PROFISSIONAL = p.NOME,
                        CLIENTE = c.NOME,
                        LOGIN = at.LOGIN,
                        PROCEDIMENTO = pr.DESCRICAO,
                        REGIAO = rc.REGIAO,
                        VALOR_NEGOCIADO = at.VALOR_NEGOCIADO
                    }).Where(at => at.ID_ATENDIMENTO == idAtendimento && at.ID_TRATAMENTO == idTratamento).ToList();
        }

        // PUT: api/AtendimentoTratamento/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutATENDIMENTO_TRATAMENTO(int id, ATENDIMENTO_TRATAMENTO aTENDIMENTO_TRATAMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aTENDIMENTO_TRATAMENTO.ID_TRATAMENTO)
            {
                return BadRequest();
            }

            db.Entry(aTENDIMENTO_TRATAMENTO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ATENDIMENTO_TRATAMENTOExists(id))
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

        // POST: api/AtendimentoTratamento
        [ResponseType(typeof(ATENDIMENTO_TRATAMENTO))]
        public IHttpActionResult PostATENDIMENTO_TRATAMENTO(ATENDIMENTO_TRATAMENTO aTENDIMENTO_TRATAMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_TRATAMENTO;").First();

            aTENDIMENTO_TRATAMENTO.ID_TRATAMENTO = NextValue;

            db.ATENDIMENTO_TRATAMENTO.Add(aTENDIMENTO_TRATAMENTO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ATENDIMENTO_TRATAMENTOExists(aTENDIMENTO_TRATAMENTO.ID_TRATAMENTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aTENDIMENTO_TRATAMENTO.ID_TRATAMENTO }, aTENDIMENTO_TRATAMENTO);
        }

        // DELETE: api/AtendimentoTratamento/5
        [ResponseType(typeof(ATENDIMENTO_TRATAMENTO))]
        public IHttpActionResult DeleteATENDIMENTO_TRATAMENTO(int id)
        {
            ATENDIMENTO_TRATAMENTO aTENDIMENTO_TRATAMENTO = db.ATENDIMENTO_TRATAMENTO.Find(id);
            if (aTENDIMENTO_TRATAMENTO == null)
            {
                return NotFound();
            }

            db.ATENDIMENTO_TRATAMENTO.Remove(aTENDIMENTO_TRATAMENTO);
            db.SaveChanges();

            return Ok(aTENDIMENTO_TRATAMENTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ATENDIMENTO_TRATAMENTOExists(int id)
        {
            return db.ATENDIMENTO_TRATAMENTO.Count(e => e.ID_TRATAMENTO == id) > 0;
        }
    }
}