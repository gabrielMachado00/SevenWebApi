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
    public class TipoServicoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/TipoServico
        public IQueryable<TIPO_SERVICO> GetTIPO_SERVICO()
        {
            return db.TIPO_SERVICO;
        }

        // GET: api/TipoServico/5
        [ResponseType(typeof(TIPO_SERVICO))]
        public IHttpActionResult GetTIPO_SERVICO(int id)
        {
            TIPO_SERVICO tIPO_SERVICO = db.TIPO_SERVICO.Find(id);
            if (tIPO_SERVICO == null)
            {
                return NotFound();
            }

            return Ok(tIPO_SERVICO);
        }

        // PUT: api/TipoServico/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTIPO_SERVICO(int id, TIPO_SERVICO tIPO_SERVICO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tIPO_SERVICO.ID_TIPO_SERVICO)
            {
                return BadRequest();
            }

            db.Entry(tIPO_SERVICO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TIPO_SERVICOExists(id))
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

        // POST: api/TipoServico
        [ResponseType(typeof(TIPO_SERVICO))]
        public IHttpActionResult PostTIPO_SERVICO(TIPO_SERVICO tIPO_SERVICO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TIPO_SERVICO.Add(tIPO_SERVICO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TIPO_SERVICOExists(tIPO_SERVICO.ID_TIPO_SERVICO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tIPO_SERVICO.ID_TIPO_SERVICO }, tIPO_SERVICO);
        }

        // DELETE: api/TipoServico/5
        [ResponseType(typeof(TIPO_SERVICO))]
        public IHttpActionResult DeleteTIPO_SERVICO(int id)
        {
            TIPO_SERVICO tIPO_SERVICO = db.TIPO_SERVICO.Find(id);
            if (tIPO_SERVICO == null)
            {
                return NotFound();
            }

            db.TIPO_SERVICO.Remove(tIPO_SERVICO);
            db.SaveChanges();

            return Ok(tIPO_SERVICO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TIPO_SERVICOExists(int id)
        {
            return db.TIPO_SERVICO.Count(e => e.ID_TIPO_SERVICO == id) > 0;
        }
    }
}