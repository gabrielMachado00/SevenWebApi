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
using System.Data.SqlClient;

namespace SevenMedicalApi.Controllers
{
    public class PROFISSIONAL_COMISSAODTO
    {
        public int ID_PROFISSIONAL { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public decimal PERCENTUAL { get; set; }
        public decimal VALOR { get; set; }
        public string NOME { get; set; }
        public string DESC_PROCEDIMENTO { get; set; }
        public string DESC_REGIAO_CORPO { get; set; }
        public int NUM_SESSOES { get; set; }
        public String UPD_OR_INSERT { get; set; }
        public Nullable<int> FORMA_COMISSAO { get; set; }
        public string DESC_FORMA_COMISSAO { get; set; }
    }

    public class ProfissionalComissoesController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/ProfissionalProcedimentoRegiaoComissao/1
        [Route("api/ProfissionalProcedimentoRegiaoComissao/{idProfissional}")]
        public IEnumerable<PROFISSIONAL_COMISSAODTO> GetPROF_PROCEDIMENTO_COMISSAO(int idProfissional)
        {
            return (from pc in db.PROFISSIONAL_COMISSAO
                    join pf in db.PROFISSIONALs on pc.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL
                    join r in db.REGIAO_CORPO on pc.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pc.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    join pr in db.PROCEDIMENTO_REGIAO_CORPO on new { pc.ID_PROCEDIMENTO, pc.ID_REGIAO_CORPO } equals new { pr.ID_PROCEDIMENTO, pr.ID_REGIAO_CORPO }
                    select new PROFISSIONAL_COMISSAODTO()
                    {
                        ID_PROFISSIONAL = pc.ID_PROFISSIONAL,
                        ID_PROCEDIMENTO = pc.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pc.ID_REGIAO_CORPO,
                        VALOR = pc.VALOR,
                        PERCENTUAL = pc.PERCENTUAL,
                        DESC_PROCEDIMENTO = p.DESCRICAO,
                        DESC_REGIAO_CORPO = r.REGIAO,
                        NOME = pf.NOME,
                        NUM_SESSOES = pr.NUM_SESSOES,
                        FORMA_COMISSAO = pc.FORMA_COMISSAO,
                        DESC_FORMA_COMISSAO = pc.FORMA_COMISSAO == 0 ? "Fluxo de caixa" :
                                              pc.FORMA_COMISSAO == 1 ? "Faturamento" :
                                              pc.FORMA_COMISSAO == 2 ? "Atendimento" :
                                              pc.FORMA_COMISSAO == 3 ? "Venda" : ""
                    }).Where(pc => pc.ID_PROFISSIONAL == idProfissional && pc.NUM_SESSOES == 1).ToList();
        }

        // GET api/ProfissionalProcedimentoRegiaoComissao/1/1
        [Route("api/ProfissionalProcedimentoRegiaoComissao/{idProfissional}/{idProcedimento}")]
        public IEnumerable<PROFISSIONAL_COMISSAODTO> GetPROF_PROCEDIMENTO_REGIAO_COMISSAO(int idProfissional, int idProcedimento)
        {
            return (from pc in db.PROFISSIONAL_COMISSAO
                    join pf in db.PROFISSIONALs on pc.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL
                    join r in db.REGIAO_CORPO on pc.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pc.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    join pr in db.PROCEDIMENTO_REGIAO_CORPO on new { pc.ID_PROCEDIMENTO, pc.ID_REGIAO_CORPO } equals new { pr.ID_PROCEDIMENTO, pr.ID_REGIAO_CORPO }
                    select new PROFISSIONAL_COMISSAODTO()
                    {
                        ID_PROFISSIONAL = pc.ID_PROFISSIONAL,
                        ID_PROCEDIMENTO = pc.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pc.ID_REGIAO_CORPO,
                        VALOR = pc.VALOR,
                        PERCENTUAL = pc.PERCENTUAL,
                        DESC_PROCEDIMENTO = p.DESCRICAO,
                        DESC_REGIAO_CORPO = r.REGIAO,
                        NOME = pf.NOME,
                        NUM_SESSOES = pr.NUM_SESSOES,
                        FORMA_COMISSAO = pc.FORMA_COMISSAO
                    }).Where(pc => pc.ID_PROFISSIONAL == idProfissional && pc.ID_PROCEDIMENTO == idProcedimento && pc.NUM_SESSOES == 1).ToList();
        }

        // GET api/ProfissionalProcedimentoRegiaoComissao/1/1/1
        [Route("api/ProfissionalProcedimentoRegiaoComissao/{idProfissional}/{idProcedimento}/{idRegiaoCorpo}")]
        public IEnumerable<PROFISSIONAL_COMISSAODTO> GetPROF_PROCEDIMENTO_REGIAO_COMISSAO(int idProfissional, int idProcedimento, int idRegiaoCorpo)
        {
            return (from pc in db.PROFISSIONAL_COMISSAO
                    join pf in db.PROFISSIONALs on pc.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL
                    join r in db.REGIAO_CORPO on pc.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PROCEDIMENTOes on pc.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    join pr in db.PROCEDIMENTO_REGIAO_CORPO on new { pc.ID_PROCEDIMENTO, pc.ID_REGIAO_CORPO } equals new { pr.ID_PROCEDIMENTO, pr.ID_REGIAO_CORPO }
                    select new PROFISSIONAL_COMISSAODTO()
                    {
                        ID_PROFISSIONAL = pc.ID_PROFISSIONAL,
                        ID_PROCEDIMENTO = pc.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pc.ID_REGIAO_CORPO,
                        VALOR = pc.VALOR,
                        PERCENTUAL = pc.PERCENTUAL,
                        DESC_PROCEDIMENTO = p.DESCRICAO,
                        DESC_REGIAO_CORPO = r.REGIAO,
                        NOME = pf.NOME,
                        NUM_SESSOES = pr.NUM_SESSOES,
                        FORMA_COMISSAO = pc.FORMA_COMISSAO
                    }).Where(pc => pc.ID_PROFISSIONAL == idProfissional && pc.ID_PROCEDIMENTO == idProcedimento && pc.ID_REGIAO_CORPO == idRegiaoCorpo && pc.NUM_SESSOES == 1).ToList();
        }

        //Get: Serviços e Regioes sem comissão definida
        [Route("api/ProfissionalProcedimentoRegiaoComissao/GetListaComissoesNaoConfigurada/{idProfissional}")]
        public IEnumerable<PROFISSIONAL_COMISSAODTO> GetListaComissoesNaoConfigurada( int idProfissional)
        {
            return db.Database.SqlQuery<PROFISSIONAL_COMISSAODTO>(@"SELECT NOME, DESC_PROCEDIMENTO, DESC_REGIAO_CORPO, PERCENTUAL, VALOR,
                                                                            ID_PROFISSIONAL, ID_PROCEDIMENTO, ID_REGIAO_CORPO, UPD_OR_INSERT, FORMA_COMISSAO
                                                                       FROM
                                                                         (SELECT DISTINCT PF.NOME, P.DESCRICAO DESC_PROCEDIMENTO, RC.REGIAO AS DESC_REGIAO_CORPO,  
	                                                                           0 PERCENTUAL, 0 VALOR, 
	                                                                           VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO, VI.ID_REGIAO_CORPO,
	                                                                           'INS' UPD_OR_INSERT, NULL FORMA_COMISSAO
                                                                        FROM VENDA_ITEM VI,
                                                                             VENDA V, REGIAO_CORPO RC, PROCEDIMENTO P, PROFISSIONAL PF 		 
                                                                        WHERE PF.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
                                                                          AND P.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO
                                                                          AND RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
                                                                          AND VI.ID_VENDA = V.ID_VENDA  
                                                                          AND NOT EXISTS (SELECT * FROM PROFISSIONAL_COMISSAO PC
				                                                                          WHERE PC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
					                                                                        AND PC.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO
					                                                                        AND PC.ID_PROFISSIONAL = VI.ID_PROFISSIONAL)
                                                                          AND V.STATUS = 'C'
                                                                          AND (@ID_PROFISSIONAL = -1 OR PF.ID_PROFISSIONAL = @ID_PROFISSIONAL)
                                                                        UNION ALL
                                                                        SELECT PF.NOME, P.DESCRICAO DESC_PROCEDIMENTO, RC.REGIAO,  
	                                                                           ISNULL(PRC.PERCENTUAL, 0) PERCENTUAL, ISNULL(PRC.VALOR, 0) VALOR, 
	                                                                           PRC.ID_PROFISSIONAL, PRC.ID_PROCEDIMENTO, PRC.ID_REGIAO_CORPO,
	                                                                           'UPD' UPD_OR_INSERT, PRC.FORMA_COMISSAO
                                                                        FROM  PROFISSIONAL_COMISSAO PRC, REGIAO_CORPO RC, 
                                                                              PROCEDIMENTO P, PROFISSIONAL PF		 
                                                                        WHERE PRC.ID_PROCEDIMENTO = P.ID_PROCEDIMENTO
                                                                          AND PRC.ID_REGIAO_CORPO = RC.ID_REGIAO_CORPO
                                                                          AND PRC.ID_PROFISSIONAL = PF.ID_PROFISSIONAL
                                                                          AND PRC.PERCENTUAL = 0 
                                                                          AND PRC.VALOR = 0
                                                                          AND (@ID_PROFISSIONAL = -1 OR PF.ID_PROFISSIONAL = @ID_PROFISSIONAL)) REG
                                                                    ORDER BY ID_PROFISSIONAL",                                                           
                                                           new SqlParameter("@ID_PROFISSIONAL", idProfissional)).ToList();
        }

        // PUT: api/ProfissionalProcedimentoRegiaoComissao/5/1/2
        [ResponseType(typeof(void))]
        [Route("api/ProfissionalProcedimentoRegiaoComissao/PutPROF_PROC_REGIAO_COMISSAO/{idProfissional}/{idProcedimento}/{idRegiaoCorpo}")]
        public IHttpActionResult PutPROCEDIMENTO_REGIAO_CORPO(int idProfissional, int idProcedimento, int idRegiaoCorpo, PROFISSIONAL_COMISSAO pPROFISSIONAL_COMISSAO)
        {
          /*  if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idProfissional != pPROFISSIONAL_COMISSAO.ID_PROFISSIONAL)
            {
                return BadRequest();
            }

            if (idProcedimento != pPROFISSIONAL_COMISSAO.ID_PROCEDIMENTO)
            {
                return BadRequest();
            }

            if (idRegiaoCorpo != pPROFISSIONAL_COMISSAO.ID_REGIAO_CORPO)
            {
                return BadRequest();
            }

            db.Entry(pPROFISSIONAL_COMISSAO).State = EntityState.Modified;*/

            try
            {
                //  db.SaveChanges();
                var sql = @"UPDATE PROFISSIONAL_COMISSAO SET VALOR = {0}, PERCENTUAL = {1}, FORMA_COMISSAO = {2} " +
                             " WHERE ID_PROFISSIONAL = {3} AND ID_PROCEDIMENTO = {4} AND ID_REGIAO_CORPO = {5}";

                if (db.Database.ExecuteSqlCommand(sql, pPROFISSIONAL_COMISSAO.VALOR, pPROFISSIONAL_COMISSAO.PERCENTUAL, pPROFISSIONAL_COMISSAO.FORMA_COMISSAO,
                                                       idProfissional, idProcedimento, idRegiaoCorpo) > 0)
                    return Ok(pPROFISSIONAL_COMISSAO);
                else
                    return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PROFISSIONAL_COMISSAOExists(idProfissional, idProcedimento, idRegiaoCorpo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

           // return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProfissionalComissoes
        [ResponseType(typeof(PROFISSIONAL_COMISSAO))]
        public IHttpActionResult PostPROFISSIONAL_COMISSAO(PROFISSIONAL_COMISSAO pROFISSIONAL_COMISSAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PROFISSIONAL_COMISSAO.Add(pROFISSIONAL_COMISSAO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PROFISSIONAL_COMISSAOExists(pROFISSIONAL_COMISSAO.ID_PROFISSIONAL, pROFISSIONAL_COMISSAO.ID_PROCEDIMENTO, pROFISSIONAL_COMISSAO.ID_REGIAO_CORPO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROFISSIONAL_COMISSAO.ID_PROFISSIONAL }, pROFISSIONAL_COMISSAO);
        }

        // DELETE: api/ProfissionalComissao/5
        [ResponseType(typeof(PROFISSIONAL_COMISSAO))]
        [Route("api/profissional/{idProfissional}/{idProcedimento}/{idRegiaoCorpo}")]
        public IHttpActionResult DeletePROFISSIONAL_COMISSAO(int idProfissional, int idProcedimento, int idRegiaoCorpo)
        {
            PROFISSIONAL_COMISSAO pROFISSIONAL_COMISSAO = db.PROFISSIONAL_COMISSAO.Where(pc => pc.ID_PROFISSIONAL == idProfissional && pc.ID_PROCEDIMENTO == idProcedimento && pc.ID_REGIAO_CORPO == idRegiaoCorpo).FirstOrDefault();
            if (pROFISSIONAL_COMISSAO == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM PROFISSIONAL_COMISSAO WHERE ID_PROFISSIONAL = {0} AND ID_PROCEDIMENTO = {1} AND ID_REGIAO_CORPO = {2}";

            if (db.Database.ExecuteSqlCommand(sql, idProfissional, idProcedimento, idRegiaoCorpo) > 0)
                return Ok(pROFISSIONAL_COMISSAO);
            else
                return NotFound();
        }

        // DELETE: api/ProfissionalComissao/5
        [ResponseType(typeof(PROFISSIONAL_COMISSAO))]
        [Route("api/profissionalcomissao/{idProfissional}")]
        public IHttpActionResult DeletePROFISSIONAL_COMISSAO(int idProfissional)
        {
            PROFISSIONAL_COMISSAO pROFISSIONAL_COMISSAO = db.PROFISSIONAL_COMISSAO.Where(pc => pc.ID_PROFISSIONAL == idProfissional).FirstOrDefault();
            if (pROFISSIONAL_COMISSAO == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM PROFISSIONAL_COMISSAO WHERE ID_PROFISSIONAL = {0}";

            if (db.Database.ExecuteSqlCommand(sql, idProfissional) > 0)
                return Ok(pROFISSIONAL_COMISSAO);
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

        private bool PROFISSIONAL_COMISSAOExists(int idProfissional, int idProcedimento, int idRegiaoCorpo)
        {
            return db.PROFISSIONAL_COMISSAO.Count(e => e.ID_PROFISSIONAL == idProfissional && e.ID_PROCEDIMENTO == idProcedimento && e.ID_REGIAO_CORPO == idRegiaoCorpo) > 0;
        }
    }
}