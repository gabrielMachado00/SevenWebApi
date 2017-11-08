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
    public class AcessosController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/Acessos
        public IQueryable<ACESSO> GetACESSOes()
        {
            return db.ACESSOes;
        }

        // GET: api/Acessos/5
        [ResponseType(typeof(ACESSO))]
        public IHttpActionResult GetACESSO(int id)
        {
            ACESSO aCESSO = db.ACESSOes.Find(id);
            if (aCESSO == null)
            {
                return NotFound();
            }

            return Ok(aCESSO);
        }

        // PUT: api/Acessos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutACESSO(int id, ACESSO aCESSO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aCESSO.ID_ACESSO)
            {
                return BadRequest();
            }

            db.Entry(aCESSO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ACESSOExists(id))
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

        // POST: api/Acessos
        [ResponseType(typeof(ACESSO))]
        public IHttpActionResult PostACESSO(ACESSO aCESSO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ACESSO;").First();

            aCESSO.ID_ACESSO = NextValue;

            db.ACESSOes.Add(aCESSO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ACESSOExists(aCESSO.ID_ACESSO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aCESSO.ID_ACESSO }, aCESSO);
        }

        // DELETE: api/Acessos/5
        [ResponseType(typeof(ACESSO))]
        public IHttpActionResult DeleteACESSO(int id)
        {
            ACESSO aCESSO = db.ACESSOes.Find(id);
            if (aCESSO == null)
            {
                return NotFound();
            }

            db.ACESSOes.Remove(aCESSO);
            db.SaveChanges();

            return Ok(aCESSO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ACESSOExists(int id)
        {
            return db.ACESSOes.Count(e => e.ID_ACESSO == id) > 0;
        }
    }
}