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
    public class RegiaoCorpoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/RegiaoCorpo
        public IQueryable<REGIAO_CORPO> GetREGIAO_CORPO()
        {
            return db.REGIAO_CORPO.OrderBy(c=>c.REGIAO);
        }

        // GET: api/RegiaoCorpo/5
        [ResponseType(typeof(REGIAO_CORPO))]
        public IHttpActionResult GetREGIAO_CORPO(int id)
        {
            REGIAO_CORPO rEGIAO_CORPO = db.REGIAO_CORPO.Find(id);
            if (rEGIAO_CORPO == null)
            {
                return NotFound();
            }

            return Ok(rEGIAO_CORPO);
        }

        // PUT: api/RegiaoCorpo/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutREGIAO_CORPO(int id, REGIAO_CORPO rEGIAO_CORPO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rEGIAO_CORPO.ID_REGIAO_CORPO)
            {
                return BadRequest();
            }

            db.Entry(rEGIAO_CORPO).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!REGIAO_CORPOExists(id))
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

        // POST: api/RegiaoCorpo
        [ResponseType(typeof(REGIAO_CORPO))]
        public IHttpActionResult PostREGIAO_CORPO(REGIAO_CORPO rEGIAO_CORPO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_REGIAO_CORPO;").First();

            rEGIAO_CORPO.ID_REGIAO_CORPO = NextValue;

            db.REGIAO_CORPO.Add(rEGIAO_CORPO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (REGIAO_CORPOExists(rEGIAO_CORPO.ID_REGIAO_CORPO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = rEGIAO_CORPO.ID_REGIAO_CORPO }, rEGIAO_CORPO);
        }

        // DELETE: api/RegiaoCorpo/5
        [ResponseType(typeof(REGIAO_CORPO))]
        public IHttpActionResult DeleteREGIAO_CORPO(int id)
        {
            REGIAO_CORPO rEGIAO_CORPO = db.REGIAO_CORPO.Find(id);
            if (rEGIAO_CORPO == null)
            {
                return NotFound();
            }

            db.REGIAO_CORPO.Remove(rEGIAO_CORPO);
            db.SaveChanges();

            return Ok(rEGIAO_CORPO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool REGIAO_CORPOExists(int id)
        {
            return db.REGIAO_CORPO.Count(e => e.ID_REGIAO_CORPO == id) > 0;
        }
    }
}