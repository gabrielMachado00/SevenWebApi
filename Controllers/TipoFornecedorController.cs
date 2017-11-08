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
    public class TipoFornecedorController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/TipoFornecedor
        public IQueryable<FORNECEDOR_TIPO> GetTIPO_FORNECEDOR()
        {
            return db.TIPO_FORNECEDOR;
        }

        // GET: api/TipoFornecedor/5
        [ResponseType(typeof(FORNECEDOR_TIPO))]
        public IHttpActionResult GetTIPO_FORNECEDOR(int id)
        {
            FORNECEDOR_TIPO tIPO_FORNECEDOR = db.TIPO_FORNECEDOR.Find(id);
            if (tIPO_FORNECEDOR == null)
            {
                return NotFound();
            }

            return Ok(tIPO_FORNECEDOR);
        }

        // PUT: api/TipoFornecedor/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTIPO_FORNECEDOR(int id, FORNECEDOR_TIPO tIPO_FORNECEDOR)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tIPO_FORNECEDOR.ID_TP_FORNECEDOR)
            {
                return BadRequest();
            }

            db.Entry(tIPO_FORNECEDOR).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TIPO_FORNECEDORExists(id))
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

        // POST: api/TipoFornecedor
        [ResponseType(typeof(FORNECEDOR_TIPO))]
        public IHttpActionResult PostTIPO_FORNECEDOR(FORNECEDOR_TIPO tIPO_FORNECEDOR)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TIPO_FORNECEDOR.Add(tIPO_FORNECEDOR);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TIPO_FORNECEDORExists(tIPO_FORNECEDOR.ID_TP_FORNECEDOR))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tIPO_FORNECEDOR.ID_TP_FORNECEDOR }, tIPO_FORNECEDOR);
        }

        // DELETE: api/TipoFornecedor/5
        [ResponseType(typeof(FORNECEDOR_TIPO))]
        public IHttpActionResult DeleteTIPO_FORNECEDOR(int id)
        {
            FORNECEDOR_TIPO tIPO_FORNECEDOR = db.TIPO_FORNECEDOR.Find(id);
            if (tIPO_FORNECEDOR == null)
            {
                return NotFound();
            }

            db.TIPO_FORNECEDOR.Remove(tIPO_FORNECEDOR);
            db.SaveChanges();

            return Ok(tIPO_FORNECEDOR);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TIPO_FORNECEDORExists(int id)
        {
            return db.TIPO_FORNECEDOR.Count(e => e.ID_TP_FORNECEDOR == id) > 0;
        }
    }
}