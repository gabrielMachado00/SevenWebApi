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
using SevenMedicalApi.Models;
using System.Data.SqlClient;

namespace SevenMedicalApi.Controllers
{
    public class UnidadeImpostoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/UnidadeImposto
        public IQueryable<UNIDADE_IMPOSTO> GetUNIDADE_IMPOSTO()
        {
            return db.UNIDADE_IMPOSTO;
        }

        // GET: api/UnidadeImposto/5
        [ResponseType(typeof(UNIDADE_IMPOSTO))]
        public IHttpActionResult GetUNIDADE_IMPOSTO(int id)
        {
            UNIDADE_IMPOSTO REG = db.UNIDADE_IMPOSTO.Find(id);
            if (REG == null)
            {
                return NotFound();
            }

            return Ok(REG);
        }

        // GET api/UnidadeImposto/ListaImpostosPorUnidade/1
        [Route("api/UnidadeImposto/ListaImpostosPorUnidade/{idUnidade}")]
        public IEnumerable<UNIDADE_IMPOSTO> GetIMPOSTOS_POR_UNIDADE(int idUnidade)
        {
            //return (from r in db.UNIDADE_IMPOSTO).Where(r => r.ID_UNIDADE == idUnidade).ToList();
            return db.Database.SqlQuery<UNIDADE_IMPOSTO>(@"SELECT ID_UNIDADE_IMPOSTO, ID_UNIDADE, DESCRICAO, PERCENTUAL
                                                          FROM UNIDADE_IMPOSTO 
                                                         WHERE ID_UNIDADE = @ID_UNIDADE
                                                         ORDER BY ID_UNIDADE_IMPOSTO",
                                                    new SqlParameter("@ID_UNIDADE", idUnidade)).ToList();

        }

        // PUT: api/UnidadeImposto/
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUNIDADE_IMPOSTO(int id, UNIDADE_IMPOSTO RegUnidadeImposto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != RegUnidadeImposto.ID_UNIDADE_IMPOSTO)
            {
                return BadRequest();
            }

            db.Entry(RegUnidadeImposto).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UNIDADE_IMPOSTOExists(id))
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

        // POST: api/UnidadeImposto/
        [ResponseType(typeof(UNIDADE_IMPOSTO))]
        public IHttpActionResult PostUNIDADE_IMPOSTO(UNIDADE_IMPOSTO uNIDADE_IMPOSTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_UNIDADE_IMPOSTO;").First();

            uNIDADE_IMPOSTO.ID_UNIDADE_IMPOSTO = NextValue;

            db.UNIDADE_IMPOSTO.Add(uNIDADE_IMPOSTO);

            try
            {
                if (UNIDADE_IMPOSTOExists(uNIDADE_IMPOSTO.ID_UNIDADE_IMPOSTO))
                {
                    return Conflict();
                }
                else
                    db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UNIDADE_IMPOSTOExists(uNIDADE_IMPOSTO.ID_UNIDADE_IMPOSTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = uNIDADE_IMPOSTO.ID_UNIDADE_IMPOSTO }, uNIDADE_IMPOSTO);
        }

        // DELETE: api/UnidadeImposto
        [ResponseType(typeof(UNIDADE_IMPOSTO))]
        public IHttpActionResult DeleteUNIDADE_IMPOSTO(int id)
        {
            UNIDADE_IMPOSTO uNIDADE_IMPOSTO = db.UNIDADE_IMPOSTO.Find(id);
            if (uNIDADE_IMPOSTO == null)
            {
                return NotFound();
            }

            db.UNIDADE_IMPOSTO.Remove(uNIDADE_IMPOSTO);
            db.SaveChanges();

            return Ok(uNIDADE_IMPOSTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UNIDADE_IMPOSTOExists(int id)
        {
            return db.UNIDADE_IMPOSTO.Count(e => e.ID_UNIDADE_IMPOSTO == id) > 0;
        }
    }
}