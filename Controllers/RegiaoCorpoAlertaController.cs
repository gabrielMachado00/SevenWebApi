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

namespace SevenMedicalApi
{
    public class RegiaoCorpoAlertaController : ApiController
    {
        public class REGIAO_CORPO_ALERTADTO
        {            
            public int ID_REGIAO_CORPO_ALERTA { get; set; }
            public int ID_REGIAO_CORPO { get; set; }
            public int ID_PROCEDIMENTO { get; set; }
            public int INTERVALO { get; set; }
            public int STATUS_ATIVAR { get; set; }
            public string DESCRICAO_STATUS { get; set; }            
            public string DESCRICAO { get; set; }
        }

        private SisMedContext db = new SisMedContext();

        // GET: api/RegiaoCorpoAlertaController
        public IQueryable<REGIAO_CORPO_ALERTA> GetREGIAO_CORPO_ALERTA()
        {
            return db.REGIAO_CORPO_ALERTA.OrderBy(c=>c.DESCRICAO);
        }

        // GET: api/RegiaoCorpoAlerta/5
        [ResponseType(typeof(REGIAO_CORPO_ALERTA))]
        public IHttpActionResult GetREGIAO_CORPO_ALERTA(int id)
        {
            REGIAO_CORPO_ALERTA REG = db.REGIAO_CORPO_ALERTA.Find(id);
            if (REG == null)
            {
                return NotFound();
            }

            return Ok(REG);
        }

        //// GET: api/RegiaoCorpoAlertaController
        //public IQueryable<REGIAO_CORPO_ALERTA> GetALERTAS_POR_REGIAO_CORPO(int idRegiaoCorpo)
        //{
        //    //return db.REGIAO_CORPO_ALERTA;
        //    return db.REGIAO_CORPO_ALERTA.f(e => e.ID_REGIAO_CORPO == id);
        //}

        // GET api/ListaAlertasPorRegiaoCorpo/1/1
        [Route("api/RegiaoCorpoAlerta/ListaAlertasPorRegiaoCorpo/{idRegiaoCorpo}/{idProcedimento}")]
        public IEnumerable<REGIAO_CORPO_ALERTADTO> GetALERTAS_POR_REGIAO_CORPO(int idRegiaoCorpo, int idProcedimento)
        {
            //return (from r in db.REGIAO_CORPO_ALERTA).Where(r => r.ID_REGIAO_CORPO == idRegiaoCorpo).ToList();
            return db.Database.SqlQuery<REGIAO_CORPO_ALERTADTO>(@"SELECT ID_REGIAO_CORPO_ALERTA, ID_REGIAO_CORPO, ID_PROCEDIMENTO,
                                                                      INTERVALO, STATUS_ATIVAR, 
                                                                    CASE STATUS_ATIVAR
                                                                         WHEN 0 THEN 'Agendado'
                                                                         WHEN 1 THEN 'Confirmado'
                                                                         WHEN 2 THEN 'Aguardando'
                                                                         WHEN 3 THEN 'Em Atendimento'
		                                                                 WHEN 4 THEN 'Atendido'
		                                                                 WHEN 5 THEN 'Faturado' 
		                                                                 WHEN 6 THEN 'Cancelado'
		                                                                 WHEN 7 THEN 'Faltou'
                                                                         ELSE 'Outro'
                                                                      END DESCRICAO_STATUS,
	                                                                  DESCRICAO
                                                                 FROM REGIAO_CORPO_ALERTA 
                                                                WHERE ID_REGIAO_CORPO = @ID_REGIAO_CORPO
                                                                  AND ID_PROCEDIMENTO = @ID_PROCEDIMENTO
                                                               ORDER BY INTERVALO",
                                                              new SqlParameter("@ID_REGIAO_CORPO", idRegiaoCorpo),
                                                              new SqlParameter("@ID_PROCEDIMENTO", idProcedimento)).ToList();
                        
        }

        // PUT: api/RegiaoCorpoAlerta/
        [ResponseType(typeof(void))]
        public IHttpActionResult PutREGIAO_CORPO_ALERTA(int id, REGIAO_CORPO_ALERTA RegCorpoAlerta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != RegCorpoAlerta.ID_REGIAO_CORPO_ALERTA)
            {
                return BadRequest();
            }

            db.Entry(RegCorpoAlerta).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!REGIAO_CORPO_ALERTAExistsUp(id))
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

        // POST: api/RegiaoCorpoAlerta/
        [ResponseType(typeof(REGIAO_CORPO_ALERTA))]
        public IHttpActionResult PostREGIAO_CORPO_ALERTA(REGIAO_CORPO_ALERTA rEGIAO_CORPO_ALERTA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_REGIAO_CORPO_ALERTA;").First();

            rEGIAO_CORPO_ALERTA.ID_REGIAO_CORPO_ALERTA = NextValue;

            db.REGIAO_CORPO_ALERTA.Add(rEGIAO_CORPO_ALERTA);

            try
            {
                if (REGIAO_CORPO_ALERTAExists(rEGIAO_CORPO_ALERTA.ID_REGIAO_CORPO,
                                              rEGIAO_CORPO_ALERTA.ID_PROCEDIMENTO,
                                              rEGIAO_CORPO_ALERTA.STATUS_ATIVAR,
                                              rEGIAO_CORPO_ALERTA.INTERVALO))
                {
                    return Conflict();
                }
                else
                    db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (REGIAO_CORPO_ALERTAExistsUp(NextValue))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = rEGIAO_CORPO_ALERTA.ID_REGIAO_CORPO_ALERTA }, rEGIAO_CORPO_ALERTA);
        }

        // DELETE: api/RegiaoCorpoAlerta
        [ResponseType(typeof(REGIAO_CORPO_ALERTA))]
        public IHttpActionResult DeleteREGIAO_CORPO_ALERTA(int id)
        {
            REGIAO_CORPO_ALERTA rEGIAO_CORPO_ALERTA = db.REGIAO_CORPO_ALERTA.Find(id);
            if (rEGIAO_CORPO_ALERTA == null)
            {
                return NotFound();
            }

            db.REGIAO_CORPO_ALERTA.Remove(rEGIAO_CORPO_ALERTA);
            db.SaveChanges();

            return Ok(rEGIAO_CORPO_ALERTA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool REGIAO_CORPO_ALERTAExists(int IdRegiaoCorpo, int IdProcedimento, int status, int numDias)
        {
            return db.REGIAO_CORPO_ALERTA.Count(e => e.ID_REGIAO_CORPO == IdRegiaoCorpo && e.ID_PROCEDIMENTO ==IdProcedimento && e.STATUS_ATIVAR == status && e.INTERVALO == numDias) > 0;
        }

        private bool REGIAO_CORPO_ALERTAExistsUp(int id)
        {
            return db.REGIAO_CORPO_ALERTA.Count(e => e.ID_REGIAO_CORPO_ALERTA == id) > 0;
        }
    }
}