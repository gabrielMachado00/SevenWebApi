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

namespace SevenMedicalApi.Controllers
{
    public class CentroCustoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/CentroCusto
        public IQueryable<CENTRO_CUSTO> GetCENTRO_CUSTO()
        {
            return db.CENTRO_CUSTO;
        }

        // GET: api/CentroCusto/5
        [ResponseType(typeof(CENTRO_CUSTO))]
        public IHttpActionResult GetCENTRO_CUSTO(int id)
        {
            CENTRO_CUSTO cENTRO_CUSTO = db.CENTRO_CUSTO.Find(id);
            if (cENTRO_CUSTO == null)
            {
                return NotFound();
            }

            return Ok(cENTRO_CUSTO);
        }

        // PUT: api/CentroCusto/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCENTRO_CUSTO(int id, CENTRO_CUSTO cENTRO_CUSTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cENTRO_CUSTO.ID_CENTRO_CUSTO)
            {
                return BadRequest();
            }

            db.Entry(cENTRO_CUSTO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CENTRO_CUSTOExists(id))
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

        // POST: api/CentroCusto
        [ResponseType(typeof(CENTRO_CUSTO))]
        public IHttpActionResult PostCENTRO_CUSTO(CENTRO_CUSTO cENTRO_CUSTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_CENTRO_CUSTO;").First();

            cENTRO_CUSTO.ID_CENTRO_CUSTO = NextValue;

            db.CENTRO_CUSTO.Add(cENTRO_CUSTO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CENTRO_CUSTOExists(cENTRO_CUSTO.ID_CENTRO_CUSTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cENTRO_CUSTO.ID_CENTRO_CUSTO }, cENTRO_CUSTO);
        }

        // DELETE: api/CentroCusto/5
        [ResponseType(typeof(CENTRO_CUSTO))]
        public IHttpActionResult DeleteCENTRO_CUSTO(int id)
        {
            CENTRO_CUSTO cENTRO_CUSTO = db.CENTRO_CUSTO.Find(id);
            if (cENTRO_CUSTO == null)
            {
                return NotFound();
            }

            db.CENTRO_CUSTO.Remove(cENTRO_CUSTO);
            db.SaveChanges();

            return Ok(cENTRO_CUSTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CENTRO_CUSTOExists(int id)
        {
            return db.CENTRO_CUSTO.Count(e => e.ID_CENTRO_CUSTO == id) > 0;
        }
    }
}