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
    public class SalaController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/Sala
        public IQueryable<SALA> GetSALAs()
        {
            return db.SALAs;
        }

        // GET: api/Sala/5
        [ResponseType(typeof(SALA))]
        public IHttpActionResult GetSALA(int id)
        {
            SALA sALA = db.SALAs.Find(id);
            if (sALA == null)
            {
                return NotFound();
            }

            return Ok(sALA);
        }

        // PUT: api/Sala/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSALA(int id, SALA sALA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sALA.ID_SALA)
            {
                return BadRequest();
            }

            db.Entry(sALA).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SALAExists(id))
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

        // POST: api/Sala
        [ResponseType(typeof(SALA))]
        public IHttpActionResult PostSALA(SALA sALA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_SALA;").First();

            sALA.ID_SALA = NextValue;

            db.SALAs.Add(sALA);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SALAExists(sALA.ID_SALA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = sALA.ID_SALA }, sALA);
        }

        // DELETE: api/Sala/5
        [ResponseType(typeof(SALA))]
        public IHttpActionResult DeleteSALA(int id)
        {
            SALA sALA = db.SALAs.Find(id);
            if (sALA == null)
            {
                return NotFound();
            }

            db.SALAs.Remove(sALA);
            db.SaveChanges();

            return Ok(sALA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SALAExists(int id)
        {
            return db.SALAs.Count(e => e.ID_SALA == id) > 0;
        }
    }
}