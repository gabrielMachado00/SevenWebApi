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
    public class TipoProdutoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/TipoProduto
        public IQueryable<TIPO_PRODUTO> GetTIPO_PRODUTO()
        {
            return db.TIPO_PRODUTO;
        }

        // GET: api/TipoProduto/5
        [ResponseType(typeof(TIPO_PRODUTO))]
        public IHttpActionResult GetTIPO_PRODUTO(int id)
        {
            TIPO_PRODUTO tIPO_PRODUTO = db.TIPO_PRODUTO.Find(id);
            if (tIPO_PRODUTO == null)
            {
                return NotFound();
            }

            return Ok(tIPO_PRODUTO);
        }

        // PUT: api/TipoProduto/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTIPO_PRODUTO(int id, TIPO_PRODUTO tIPO_PRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tIPO_PRODUTO.ID_TIPO_PRODUTO)
            {
                return BadRequest();
            }

            db.Entry(tIPO_PRODUTO).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TIPO_PRODUTOExists(id))
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

        // POST: api/TipoProduto
        [ResponseType(typeof(TIPO_PRODUTO))]
        public IHttpActionResult PostTIPO_PRODUTO(TIPO_PRODUTO tIPO_PRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TIPO_PRODUTO.Add(tIPO_PRODUTO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TIPO_PRODUTOExists(tIPO_PRODUTO.ID_TIPO_PRODUTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tIPO_PRODUTO.ID_TIPO_PRODUTO }, tIPO_PRODUTO);
        }

        // DELETE: api/TipoProduto/5
        [ResponseType(typeof(TIPO_PRODUTO))]
        public IHttpActionResult DeleteTIPO_PRODUTO(int id)
        {
            TIPO_PRODUTO tIPO_PRODUTO = db.TIPO_PRODUTO.Find(id);
            if (tIPO_PRODUTO == null)
            {
                return NotFound();
            }

            db.TIPO_PRODUTO.Remove(tIPO_PRODUTO);
            db.SaveChanges();

            return Ok(tIPO_PRODUTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TIPO_PRODUTOExists(int id)
        {
            return db.TIPO_PRODUTO.Count(e => e.ID_TIPO_PRODUTO == id) > 0;
        }
    }
}