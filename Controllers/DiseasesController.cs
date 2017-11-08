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
    public class DiseasesController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/Diseases
        public IQueryable<DISEASES> GetDISEASES()
        {
            return db.DISEASES;
        }

        // GET: api/Diseases/5
        [ResponseType(typeof(DISEASES))]
        public IHttpActionResult GetDISEASES(int id)
        {
            DISEASES dISEASES = db.DISEASES.Find(id);
            if (dISEASES == null)
            {
                return NotFound();
            }

            return Ok(dISEASES);
        }

        // PUT: api/Diseases/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDISEASES(int id, DISEASES dISEASES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dISEASES.ID_DISEASES)
            {
                return BadRequest();
            }

            db.Entry(dISEASES).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DISEASESExists(id))
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

        // POST: api/Diseases
        [ResponseType(typeof(DISEASES))]
        public IHttpActionResult PostDISEASES(DISEASES dISEASES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DISEASES.Add(dISEASES);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DISEASESExists(dISEASES.ID_DISEASES))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = dISEASES.ID_DISEASES }, dISEASES);
        }

        // DELETE: api/Diseases/5
        [ResponseType(typeof(DISEASES))]
        public IHttpActionResult DeleteDISEASES(int id)
        {
            DISEASES dISEASES = db.DISEASES.Find(id);
            if (dISEASES == null)
            {
                return NotFound();
            }

            db.DISEASES.Remove(dISEASES);
            db.SaveChanges();

            return Ok(dISEASES);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DISEASESExists(int id)
        {
            return db.DISEASES.Count(e => e.ID_DISEASES == id) > 0;
        }
    }
}