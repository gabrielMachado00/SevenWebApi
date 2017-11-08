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
    public class ContatoFornecedorController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/fornecedor/1/contato
        [Route("api/fornecedor/{idFornecedor}/contato")]
        public IEnumerable<FORNECEDOR_CONTATO> GetFORNECEDOR_CONTATO(int idFornecedor)
        {
            return db.FORNECEDOR_CONTATO.Where(c => c.ID_FORNECEDOR == idFornecedor);
        }

        // GET api/fornecedor/contato/5
        [Route("api/fornecedor/contato/{id}")]
        public FORNECEDOR_CONTATO GetCONTATO(int id)
        {
            FORNECEDOR_CONTATO fornecedor_contato = db.FORNECEDOR_CONTATO.Find(id);
            if (fornecedor_contato == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return fornecedor_contato;
        }

        // PUT: api/ContatoFornecedor/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFORNECEDOR_CONTATO(int id, FORNECEDOR_CONTATO fORNECEDOR_CONTATO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fORNECEDOR_CONTATO.ID_CONTATO)
            {
                return BadRequest();
            }

            db.Entry(fORNECEDOR_CONTATO).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FORNECEDOR_CONTATOExists(id))
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

        // POST: api/ContatoFornecedor
        [ResponseType(typeof(FORNECEDOR_CONTATO))]
        public IHttpActionResult PostFORNECEDOR_CONTATO(FORNECEDOR_CONTATO fORNECEDOR_CONTATO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_CONTATO_FORNECEDOR;").First();

            fORNECEDOR_CONTATO.ID_CONTATO = NextValue;

            db.FORNECEDOR_CONTATO.Add(fORNECEDOR_CONTATO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FORNECEDOR_CONTATOExists(fORNECEDOR_CONTATO.ID_CONTATO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = fORNECEDOR_CONTATO.ID_CONTATO }, fORNECEDOR_CONTATO);
        }

        // DELETE: api/ContatoFornecedor/5
        [ResponseType(typeof(FORNECEDOR_CONTATO))]
        public IHttpActionResult DeleteFORNECEDOR_CONTATO(int id)
        {
            FORNECEDOR_CONTATO fORNECEDOR_CONTATO = db.FORNECEDOR_CONTATO.Find(id);
            if (fORNECEDOR_CONTATO == null)
            {
                return NotFound();
            }

            db.FORNECEDOR_CONTATO.Remove(fORNECEDOR_CONTATO);
            db.SaveChanges();

            return Ok(fORNECEDOR_CONTATO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FORNECEDOR_CONTATOExists(int id)
        {
            return db.FORNECEDOR_CONTATO.Count(e => e.ID_CONTATO == id) > 0;
        }
    }
}