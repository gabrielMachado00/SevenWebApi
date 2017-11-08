using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;

namespace SevenMedicalApi.Controllers
{
    public class PROCEDIMENTO_REGIAO_CORPODTO
    {
        public int ID_PROCEDIMENTO { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public decimal VALOR { get; set; }
        public TimeSpan TEMPO { get; set; }
        public string REGIAO { get; set; }
        public int NUM_SESSOES { get; set; }
        public string OBS { get; set; }
        public string PROCEDIMENTO { get; set; }
        public bool MOVIMENTA_ESTOQUE { get; set; }
        public bool ATIVO { get; set; }
    }

    public class ProcedimentoRegioesCorpoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/ProcedimentoRegiaoCorpo/RegioesCorpo/
        [Route("api/ProcedimentoRegiaoCorpo/RegioesCorpo")]
        public IEnumerable<PROCEDIMENTO_REGIAO_CORPODTO> GetPROCEDIMENTO_REGIOES_CORPO()
        {
            return (from pr in db.PROCEDIMENTO_REGIAO_CORPO
                    join r in db.REGIAO_CORPO on pr.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pr.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    select new PROCEDIMENTO_REGIAO_CORPODTO()
                    {
                        ID_PROCEDIMENTO = pr.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pr.ID_REGIAO_CORPO,
                        VALOR = pr.VALOR,
                        TEMPO = pr.TEMPO,
                        REGIAO = r.REGIAO,
                        NUM_SESSOES = pr.NUM_SESSOES,
                        OBS = pr.OBS,
                        PROCEDIMENTO = p.DESCRICAO,
                        MOVIMENTA_ESTOQUE = pr.MOVIMENTA_ESTOQUE,
                        ATIVO =pr.ATIVO
                    }).Where(C=>C.ATIVO == true).ToList();
        }

        // GET api/ProcedimentoRegiaoCorpo/RegioesCorpo/
        [Route("api/ProcedimentoRegiaoCorpo/RegioesCorpo/unitario")]
        public IEnumerable<PROCEDIMENTO_REGIAO_CORPODTO> GetPROCEDIMENTO_REGIOES_CORPO_UNITARIO()
        {
            return (from pr in db.PROCEDIMENTO_REGIAO_CORPO
                    join r in db.REGIAO_CORPO on pr.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pr.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    select new PROCEDIMENTO_REGIAO_CORPODTO()
                    {
                        ID_PROCEDIMENTO = pr.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pr.ID_REGIAO_CORPO,
                        VALOR = pr.VALOR,
                        TEMPO = pr.TEMPO,
                        REGIAO = r.REGIAO,
                        NUM_SESSOES = pr.NUM_SESSOES,
                        OBS = pr.OBS,
                        PROCEDIMENTO = p.DESCRICAO,
                        MOVIMENTA_ESTOQUE = pr.MOVIMENTA_ESTOQUE,
                        ATIVO = pr.ATIVO
                    }).Where(pr => pr.NUM_SESSOES == 1 && pr.ATIVO == true).ToList();
        }

        // GET api/ProcedimentoRegiaoCorpo/RegiaoCorpo/1
        [Route("api/ProcedimentoRegiaoCorpo/RegiaoCorpo/{idProcedimento}")]
        public IEnumerable<PROCEDIMENTO_REGIAO_CORPODTO> GetPROCEDIMENTO_REGIAO_CORPO(int idProcedimento)
        {
            return (from pr in db.PROCEDIMENTO_REGIAO_CORPO
                    join r in db.REGIAO_CORPO on pr.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pr.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    select new PROCEDIMENTO_REGIAO_CORPODTO()
                    {
                        ID_PROCEDIMENTO = pr.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pr.ID_REGIAO_CORPO,
                        VALOR = pr.VALOR,
                        TEMPO = pr.TEMPO,
                        REGIAO = r.REGIAO,
                        NUM_SESSOES = pr.NUM_SESSOES,
                        OBS = pr.OBS,
                        PROCEDIMENTO = p.DESCRICAO,
                        MOVIMENTA_ESTOQUE = pr.MOVIMENTA_ESTOQUE,
                        ATIVO = pr.ATIVO
                    }).Where(pr => pr.ID_PROCEDIMENTO == idProcedimento && pr.ATIVO == true).ToList();
        }

        // GET api/ProcedimentoRegiaoCorpo/RegiaoCorpo/1
        [Route("api/ProcedimentoRegiaoCorpo/RegiaoCorpo/Unitario/{idProcedimento}")]
        public IEnumerable<PROCEDIMENTO_REGIAO_CORPODTO> GetPROCEDIMENTO_REGIAO_CORPO_NUM_SESSAO(int idProcedimento)
        {
            return (from pr in db.PROCEDIMENTO_REGIAO_CORPO
                    join r in db.REGIAO_CORPO on pr.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pr.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    select new PROCEDIMENTO_REGIAO_CORPODTO()
                    {
                        ID_PROCEDIMENTO = pr.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pr.ID_REGIAO_CORPO,
                        VALOR = pr.VALOR,
                        TEMPO = pr.TEMPO,
                        REGIAO = r.REGIAO,
                        NUM_SESSOES = pr.NUM_SESSOES,
                        OBS = pr.OBS,
                        PROCEDIMENTO = p.DESCRICAO,
                        MOVIMENTA_ESTOQUE = pr.MOVIMENTA_ESTOQUE,
                        ATIVO = pr.ATIVO
                    }).Where(pr => pr.ID_PROCEDIMENTO == idProcedimento && pr.NUM_SESSOES == 1 && pr.ATIVO == true).ToList();
        }

        [Route("api/Agenda/RegiaoCorpo/{idProcedimento}")]
        public IEnumerable<PROCEDIMENTO_REGIAO_CORPODTO> GetAGENDA_REGIAO_CORPO(int idProcedimento)
        {
            return (from pr in db.PROCEDIMENTO_REGIAO_CORPO
                    join r in db.REGIAO_CORPO on pr.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pr.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    select new PROCEDIMENTO_REGIAO_CORPODTO()
                    {
                        ID_PROCEDIMENTO = pr.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pr.ID_REGIAO_CORPO,
                        VALOR = pr.VALOR,
                        TEMPO = pr.TEMPO,
                        REGIAO = r.REGIAO,
                        NUM_SESSOES = pr.NUM_SESSOES,
                        OBS = pr.OBS,
                        PROCEDIMENTO = p.DESCRICAO,
                        MOVIMENTA_ESTOQUE = pr.MOVIMENTA_ESTOQUE,
                        ATIVO = pr.ATIVO
                    }).Where(pr => pr.ID_PROCEDIMENTO == idProcedimento && pr.NUM_SESSOES <= 1 && pr.ATIVO == true).ToList();
        }

        // GET api/ProcedimentoRegiaoCorpo/1/1
        [Route("api/ProcedimentoRegiaoCorpo/{idProcedimento}/{idRegiaoCorpo}")]
        public IEnumerable<PROCEDIMENTO_REGIAO_CORPODTO> GetPROCEDIMENTO_PRO_REGICAO_CORPO(int idProcedimento, int idRegiaoCorpo)
        {
            return (from pr in db.PROCEDIMENTO_REGIAO_CORPO
                    join r in db.REGIAO_CORPO on pr.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pr.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    select new PROCEDIMENTO_REGIAO_CORPODTO()
                    {
                        ID_PROCEDIMENTO = pr.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pr.ID_REGIAO_CORPO,
                        VALOR = pr.VALOR,
                        TEMPO = pr.TEMPO,
                        REGIAO = r.REGIAO,
                        NUM_SESSOES = pr.NUM_SESSOES,
                        OBS = pr.OBS,
                        PROCEDIMENTO = p.DESCRICAO,
                        MOVIMENTA_ESTOQUE = pr.MOVIMENTA_ESTOQUE,
                        ATIVO = pr.ATIVO
                    }).Where(pr => pr.ID_PROCEDIMENTO == idProcedimento && pr.ID_REGIAO_CORPO == idRegiaoCorpo && pr.ATIVO == true).ToList();
        }

        // GET api/ProcedimentoRegiaoCorpo/1/1/1
        [Route("api/ProcedimentoRegiaoCorpo/{idProcedimento}/{idRegiaoCorpo}/{numSessao}")]
        public IEnumerable<PROCEDIMENTO_REGIAO_CORPODTO> GetPROCEDIMENTO_REGICAO_CORPO_SESSAO(int idProcedimento, int idRegiaoCorpo, int numSessao)
        {
            return (from pr in db.PROCEDIMENTO_REGIAO_CORPO
                    join r in db.REGIAO_CORPO on pr.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pr.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    select new PROCEDIMENTO_REGIAO_CORPODTO()
                    {
                        ID_PROCEDIMENTO = pr.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pr.ID_REGIAO_CORPO,
                        VALOR = pr.VALOR,
                        TEMPO = pr.TEMPO,
                        REGIAO = r.REGIAO,
                        NUM_SESSOES = pr.NUM_SESSOES,
                        OBS = pr.OBS,
                        PROCEDIMENTO = p.DESCRICAO,
                        MOVIMENTA_ESTOQUE = pr.MOVIMENTA_ESTOQUE,
                        ATIVO = pr.ATIVO
                    }).Where(pr => pr.ID_PROCEDIMENTO == idProcedimento && pr.ID_REGIAO_CORPO == idRegiaoCorpo && pr.NUM_SESSOES == numSessao && pr.ATIVO == true).ToList();
        }

        [Route("api/ProcedimentoRegiaoCorpo/unitario/{idProcedimento}/{idRegiaoCorpo}")]
        public IEnumerable<PROCEDIMENTO_REGIAO_CORPODTO> GetPROCEDIMENTO_PRO_REGICAO_CORPO_UNITARIO(int idProcedimento, int idRegiaoCorpo)
        {
            return (from pr in db.PROCEDIMENTO_REGIAO_CORPO
                    join r in db.REGIAO_CORPO on pr.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pr.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    select new PROCEDIMENTO_REGIAO_CORPODTO()
                    {
                        ID_PROCEDIMENTO = pr.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pr.ID_REGIAO_CORPO,
                        VALOR = pr.VALOR,
                        TEMPO = pr.TEMPO,
                        REGIAO = r.REGIAO,
                        NUM_SESSOES = pr.NUM_SESSOES,
                        OBS = pr.OBS,
                        PROCEDIMENTO = p.DESCRICAO,
                        MOVIMENTA_ESTOQUE = pr.MOVIMENTA_ESTOQUE,
                        ATIVO = pr.ATIVO
                    }).Where(pr => pr.ID_PROCEDIMENTO == idProcedimento && pr.ID_REGIAO_CORPO == idRegiaoCorpo && pr.NUM_SESSOES <= 1 && pr.ATIVO == true).ToList();
        }

        [Route("api/ProcedimentoRegiaoCorpo/pacotes")]
        public IEnumerable<PROCEDIMENTO_REGIAO_CORPODTO> GetPACOTE_PROCEDIMENTO_PRO_REGICAO_CORPO()
        {
            return (from pr in db.PROCEDIMENTO_REGIAO_CORPO
                    join r in db.REGIAO_CORPO on pr.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pr.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    select new PROCEDIMENTO_REGIAO_CORPODTO()
                    {
                        ID_PROCEDIMENTO = pr.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pr.ID_REGIAO_CORPO,
                        VALOR = pr.VALOR,
                        TEMPO = pr.TEMPO,
                        REGIAO = r.REGIAO,
                        NUM_SESSOES = pr.NUM_SESSOES,
                        OBS = pr.OBS,
                        PROCEDIMENTO = p.DESCRICAO,
                        MOVIMENTA_ESTOQUE = pr.MOVIMENTA_ESTOQUE,
                        ATIVO = pr.ATIVO
                    }).Where(pr => pr.NUM_SESSOES  > 0 && pr.ATIVO == true).ToList();
        }

        [ResponseType(typeof(void))]
        [Route("api/ProcedimentoRegiaoCorpo/PutPROCEDIMENTO_REGIAO_CORPO/{idProcedimento}/{idRegiaoCorpo}/{numSessoes}")]
        public IHttpActionResult PutPROCEDIMENTO_REGIAO_CORPO(int idProcedimento, int idRegiaoCorpo, int numSessoes, PROCEDIMENTO_REGIAO_CORPO pROCEDIMENTO_REGIAO_CORPO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idProcedimento != pROCEDIMENTO_REGIAO_CORPO.ID_PROCEDIMENTO)
            {
                return BadRequest();
            }

            if (idRegiaoCorpo != pROCEDIMENTO_REGIAO_CORPO.ID_REGIAO_CORPO)
            {
                return BadRequest();
            }

            try
            {
                var sql = @"UPDATE PROCEDIMENTO_REGIAO_CORPO SET VALOR = {0}, TEMPO = {1}, NUM_SESSOES = {2}, OBS = {3}, MOVIMENTA_ESTOQUE = {4}, ATIVO = {5} " +
                             " WHERE ID_PROCEDIMENTO = {6} AND ID_REGIAO_CORPO = {7} AND NUM_SESSOES = {8}";

                if (db.Database.ExecuteSqlCommand(sql, pROCEDIMENTO_REGIAO_CORPO.VALOR, pROCEDIMENTO_REGIAO_CORPO.TEMPO,
                                                       pROCEDIMENTO_REGIAO_CORPO.NUM_SESSOES, pROCEDIMENTO_REGIAO_CORPO.OBS,
                                                       pROCEDIMENTO_REGIAO_CORPO.MOVIMENTA_ESTOQUE, pROCEDIMENTO_REGIAO_CORPO.ATIVO, idProcedimento, idRegiaoCorpo, numSessoes) > 0)
                    return Ok(pROCEDIMENTO_REGIAO_CORPO);
                else
                    return NotFound();
            }
            catch (Exception e)
            {
                string teste = e.Message;

                if (!PROCEDIMENTO_REGIAO_CORPOExists(idProcedimento, idRegiaoCorpo, numSessoes))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

          //  return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(void))]
        [Route("api/ProcedimentoRegiaoCorpo/PutPROCEDIMENTO_REGIAO_CORPO/ATIVO/{idProcedimento}/{idRegiaoCorpo}/{ativo}")]
        public IHttpActionResult PutPROCEDIMENTO_REGIAO_CORPO_ATIVO(int idProcedimento, int idRegiaoCorpo,int ativo)
        {
            try
            {
                var sql = @"UPDATE PROCEDIMENTO_REGIAO_CORPO SET ATIVO = " + ativo + " WHERE ID_PROCEDIMENTO = " + idProcedimento + " AND ID_REGIAO_CORPO = " + idRegiaoCorpo;

                if (db.Database.ExecuteSqlCommand(sql) > 0)
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception e)
            {
                throw;
            }

            //  return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/ProcedimentoRegioesCorpo
        [ResponseType(typeof(PROCEDIMENTO_REGIAO_CORPO))]
        public IHttpActionResult PostPROCEDIMENTO_REGIAO_CORPO(PROCEDIMENTO_REGIAO_CORPO pROCEDIMENTO_REGIAO_CORPO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PROCEDIMENTO_REGIAO_CORPO.Add(pROCEDIMENTO_REGIAO_CORPO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PROCEDIMENTO_REGIAO_CORPOExists(pROCEDIMENTO_REGIAO_CORPO.ID_PROCEDIMENTO, pROCEDIMENTO_REGIAO_CORPO.ID_REGIAO_CORPO, pROCEDIMENTO_REGIAO_CORPO.NUM_SESSOES))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROCEDIMENTO_REGIAO_CORPO.ID_PROCEDIMENTO }, pROCEDIMENTO_REGIAO_CORPO);
        }

        // DELETE: api/ProcedimentoRegiaoCorpo/5
        [ResponseType(typeof(PROCEDIMENTO_REGIAO_CORPO))]
        [Route("api/ProcedimentoRegiaoCorpo/DeletePROCEDIMENTO_REGIOES/procedimento/{idProcedimento}/regiao/{idRegiao}/sessoes/{numSessoes}")]
        public IHttpActionResult DeletePROCEDIMENTO_REGIAO_CORPO(int idProcedimento, int idRegiao, int numSessoes)
        {
            PROCEDIMENTO_REGIAO_CORPO pROCEDIMENTO_REGIAO_CORPO = db.PROCEDIMENTO_REGIAO_CORPO.Where(pr => pr.ID_PROCEDIMENTO == idProcedimento && pr.ID_REGIAO_CORPO == idRegiao && pr.NUM_SESSOES == numSessoes).FirstOrDefault();
            if (pROCEDIMENTO_REGIAO_CORPO == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM PROCEDIMENTO_REGIAO_CORPO WHERE ID_PROCEDIMENTO = {0} AND ID_REGIAO_CORPO = {1} AND NUM_SESSOES = {2}";

            if (db.Database.ExecuteSqlCommand(sql, idProcedimento, idRegiao, numSessoes) > 0)
                return Ok(pROCEDIMENTO_REGIAO_CORPO);
            else
                return NotFound();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PROCEDIMENTO_REGIAO_CORPOExists(int idProcedimento, int idRegiaoCorpo, int numSessoes)
        {
            return db.PROCEDIMENTO_REGIAO_CORPO.Count(e => e.ID_PROCEDIMENTO == idProcedimento && e.ID_REGIAO_CORPO == idRegiaoCorpo && e.NUM_SESSOES == numSessoes) > 0;
        }
    }
}