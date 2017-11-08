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
    public class ConvenioController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/Convenio
        public IQueryable<CONVENIO> GetCONVENIOs()
        {
            return db.CONVENIOs;
        }

        // GET: api/Convenio/5
        [ResponseType(typeof(CONVENIO))]
        public IHttpActionResult GetCONVENIO(int id)
        {
            CONVENIO cONVENIO = db.CONVENIOs.Find(id);
            if (cONVENIO == null)
            {
                return NotFound();
            }

            return Ok(cONVENIO);
        }

        // PUT: api/Convenio/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCONVENIO(int id, CONVENIO cONVENIO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cONVENIO.ID_CONVENIO)
            {
                return BadRequest();
            }

            db.Entry(cONVENIO).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CONVENIOExists(id))
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

        // POST: api/Convenio
        [ResponseType(typeof(CONVENIO))]
        public IHttpActionResult PostCONVENIO(CONVENIO cONVENIO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_CONVENIO;").First();

            cONVENIO.ID_CONVENIO = NextValue;

            db.CONVENIOs.Add(cONVENIO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CONVENIOExists(cONVENIO.ID_CONVENIO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cONVENIO.ID_CONVENIO }, cONVENIO);
        }

        // DELETE: api/Convenio/5
        [ResponseType(typeof(CONVENIO))]
        public IHttpActionResult DeleteCONVENIO(int id)
        {
            CONVENIO cONVENIO = db.CONVENIOs.Find(id);
            if (cONVENIO == null)
            {
                return NotFound();
            }

            db.CONVENIOs.Remove(cONVENIO);
            db.SaveChanges();

            return Ok(cONVENIO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CONVENIOExists(int id)
        {
            return db.CONVENIOs.Count(e => e.ID_CONVENIO == id) > 0;
        }
    }
}