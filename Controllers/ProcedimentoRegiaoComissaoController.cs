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
using SisMedApi.Models;

namespace SisMedApi.Controllers
{

    public class PROCEDIMENTO_REGIAO_COMISSAODTO
    {
        public int ID_PROCEDIMENTO { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public decimal PERCENTUAL { get; set; }
        public decimal VALOR { get; set; }
        public string DESCRICAO_PROCEDIMENTO { get; set; }
        public string DESCRICAO_REGIAO { get; set; }
    }

    public class ProcedimentoRegiaoComissaoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/ProcedimentoRegiaoComissao/RegiaoCorpo/1
        [Route("api/ProcedimentoRegiaoComissao/RegiaoCorpo/{idProcedimento}")]
        public IEnumerable<PROCEDIMENTO_REGIAO_COMISSAODTO> GetPROCEDIMENTO_REGIAO_COMISSAO(int idProcedimento)
        {
            return (from pr in db.PROCEDIMENTO_REGIAO_COMISSAO
                    join r in db.REGIAO_CORPO on pr.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pr.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    select new PROCEDIMENTO_REGIAO_COMISSAODTO()
                    {
                        ID_PROCEDIMENTO = pr.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pr.ID_REGIAO_CORPO,
                        VALOR = pr.VALOR,
                        PERCENTUAL = pr.PERCENTUAL,
                        DESCRICAO_PROCEDIMENTO = p.DESCRICAO,
                        DESCRICAO_REGIAO = r.REGIAO
                    }).Where(pr => pr.ID_PROCEDIMENTO == idProcedimento).ToList();
        }

        // GET api/ProcedimentoRegiaoComissao/1/1
        [Route("api/ProcedimentoRegiaoComissao/{idProcedimento}/{idRegiaoCorpo}")]
        public IEnumerable<PROCEDIMENTO_REGIAO_COMISSAODTO> GetPROCEDIMENTO_PRO_REGICAO_CORPO_COMISSAO(int idProcedimento, int idRegiaoCorpo)
        {
            return (from pr in db.PROCEDIMENTO_REGIAO_COMISSAO
                    join r in db.REGIAO_CORPO on pr.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pr.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    select new PROCEDIMENTO_REGIAO_COMISSAODTO()
                    {
                        ID_PROCEDIMENTO = pr.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pr.ID_REGIAO_CORPO,
                        VALOR = pr.VALOR,
                        PERCENTUAL = pr.PERCENTUAL,
                        DESCRICAO_PROCEDIMENTO = p.DESCRICAO,
                        DESCRICAO_REGIAO = r.REGIAO
                    }).Where(pr => pr.ID_PROCEDIMENTO == idProcedimento && pr.ID_REGIAO_CORPO == idRegiaoCorpo).ToList();
        }

        // PUT: api/ProcedimentoRegiaoCorpo/5/1
        [ResponseType(typeof(void))]
        [Route("api/ProcedimentoRegiaoComissao/PutPROCEDIMENTO_REGIAO_CORPO_COMISSAO/{idProcedimento}/{idRegiaoCorpo}")]
        public IHttpActionResult PutPROCEDIMENTO_REGIAO_CORPO(int idProcedimento, int idRegiaoCorpo, PROCEDIMENTO_REGIAO_COMISSAO pROCEDIMENTO_REGIAO_COMISSAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idProcedimento != pROCEDIMENTO_REGIAO_COMISSAO.ID_PROCEDIMENTO)
            {
                return BadRequest();
            }

            if (idRegiaoCorpo != pROCEDIMENTO_REGIAO_COMISSAO.ID_REGIAO_CORPO)
            {
                return BadRequest();
            }

            db.Entry(pROCEDIMENTO_REGIAO_COMISSAO).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PROCEDIMENTO_REGIAO_COMISSAOExists(idProcedimento, idRegiaoCorpo))
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

        // POST: api/ProcedimentoRegiaoComissao
        [ResponseType(typeof(PROCEDIMENTO_REGIAO_COMISSAO))]
        public IHttpActionResult PostPROCEDIMENTO_REGIAO_COMISSAO(PROCEDIMENTO_REGIAO_COMISSAO pROCEDIMENTO_REGIAO_COMISSAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PROCEDIMENTO_REGIAO_COMISSAO.Add(pROCEDIMENTO_REGIAO_COMISSAO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PROCEDIMENTO_REGIAO_COMISSAOExists(pROCEDIMENTO_REGIAO_COMISSAO.ID_PROCEDIMENTO, pROCEDIMENTO_REGIAO_COMISSAO.ID_REGIAO_CORPO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROCEDIMENTO_REGIAO_COMISSAO.ID_PROCEDIMENTO }, pROCEDIMENTO_REGIAO_COMISSAO);
        }

        // DELETE: api/ProcedimentoRegiaoComissao/5
        [ResponseType(typeof(PROCEDIMENTO_REGIAO_COMISSAO))]
        public IHttpActionResult DeletePROCEDIMENTO_REGIAO_COMISSAO(int id)
        {
            PROCEDIMENTO_REGIAO_COMISSAO pROCEDIMENTO_REGIAO_COMISSAO = db.PROCEDIMENTO_REGIAO_COMISSAO.Find(id);
            if (pROCEDIMENTO_REGIAO_COMISSAO == null)
            {
                return NotFound();
            }

            db.PROCEDIMENTO_REGIAO_COMISSAO.Remove(pROCEDIMENTO_REGIAO_COMISSAO);
            db.SaveChanges();

            return Ok(pROCEDIMENTO_REGIAO_COMISSAO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PROCEDIMENTO_REGIAO_COMISSAOExists(int idProcedimento, int idRegiao)
        {
            return db.PROCEDIMENTO_REGIAO_COMISSAO.Count(e => e.ID_PROCEDIMENTO == idProcedimento && e.ID_REGIAO_CORPO == idRegiao) > 0;
        }
    }
}