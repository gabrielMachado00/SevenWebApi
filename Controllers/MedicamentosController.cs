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
    public class MedicamentosController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/Medicamentos
        public IQueryable<MEDICAMENTO> GetMEDICAMENTOes()
        {
            return db.MEDICAMENTOes;
        }

        // GET: api/Medicamentos/5
        [ResponseType(typeof(MEDICAMENTO))]
        public IHttpActionResult GetMEDICAMENTO(int id)
        {
            MEDICAMENTO mEDICAMENTO = db.MEDICAMENTOes.Find(id);
            if (mEDICAMENTO == null)
            {
                return NotFound();
            }

            return Ok(mEDICAMENTO);
        }

        // PUT: api/Medicamentos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMEDICAMENTO(int id, MEDICAMENTO mEDICAMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mEDICAMENTO.ID_MEDICAMENTO)
            {
                return BadRequest();
            }

            db.Entry(mEDICAMENTO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MEDICAMENTOExists(id))
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

        // POST: api/Medicamentos
        [ResponseType(typeof(MEDICAMENTO))]
        public IHttpActionResult PostMEDICAMENTO(MEDICAMENTO mEDICAMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MEDICAMENTOes.Add(mEDICAMENTO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MEDICAMENTOExists(mEDICAMENTO.ID_MEDICAMENTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = mEDICAMENTO.ID_MEDICAMENTO }, mEDICAMENTO);
        }

        // DELETE: api/Medicamentos/5
        [ResponseType(typeof(MEDICAMENTO))]
        public IHttpActionResult DeleteMEDICAMENTO(int id)
        {
            MEDICAMENTO mEDICAMENTO = db.MEDICAMENTOes.Find(id);
            if (mEDICAMENTO == null)
            {
                return NotFound();
            }

            db.MEDICAMENTOes.Remove(mEDICAMENTO);
            db.SaveChanges();

            return Ok(mEDICAMENTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MEDICAMENTOExists(int id)
        {
            return db.MEDICAMENTOes.Count(e => e.ID_MEDICAMENTO == id) > 0;
        }
    }
}