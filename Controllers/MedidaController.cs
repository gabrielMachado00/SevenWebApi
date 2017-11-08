using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;

namespace SisMedApi.Controllers
{
    public class MedidaController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/Medida
        public IQueryable<MEDIDA> GetMEDIDAs()
        {
            return db.MEDIDAs;
        }

        // GET: api/Medida/5
        [ResponseType(typeof(MEDIDA))]
        public async Task<IHttpActionResult> GetMEDIDA(int id)
        {
            MEDIDA mEDIDA = await db.MEDIDAs.FindAsync(id);
            if (mEDIDA == null)
            {
                return NotFound();
            }

            return Ok(mEDIDA);
        }

        // PUT: api/Medida/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMEDIDA(int id, MEDIDA mEDIDA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mEDIDA.ID_MEDIDA)
            {
                return BadRequest();
            }

            db.Entry(mEDIDA).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MEDIDAExists(id))
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

        // POST: api/Medida
        [ResponseType(typeof(MEDIDA))]
        public async Task<IHttpActionResult> PostMEDIDA(MEDIDA mEDIDA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_MEDIDA;").First();

            mEDIDA.ID_MEDIDA = NextValue;

            db.MEDIDAs.Add(mEDIDA);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MEDIDAExists(mEDIDA.ID_MEDIDA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = mEDIDA.ID_MEDIDA }, mEDIDA);
        }

        // DELETE: api/Medida/5
        [ResponseType(typeof(MEDIDA))]
        public async Task<IHttpActionResult> DeleteMEDIDA(int id)
        {
            MEDIDA mEDIDA = await db.MEDIDAs.FindAsync(id);
            if (mEDIDA == null)
            {
                return NotFound();
            }

            db.MEDIDAs.Remove(mEDIDA);
            await db.SaveChangesAsync();

            return Ok(mEDIDA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MEDIDAExists(int id)
        {
            return db.MEDIDAs.Count(e => e.ID_MEDIDA == id) > 0;
        }
    }
}