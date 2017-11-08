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
    public class ProtocoloController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/Protocolo
        public IQueryable<PROTOCOLO> GetPROTOCOLOes()
        {
            return db.PROTOCOLOes;
        }

        // GET: api/Protocolo/5
        [ResponseType(typeof(PROTOCOLO))]
        public async Task<IHttpActionResult> GetPROTOCOLO(int id)
        {
            PROTOCOLO pROTOCOLO = await db.PROTOCOLOes.FindAsync(id);
            if (pROTOCOLO == null)
            {
                return NotFound();
            }

            return Ok(pROTOCOLO);
        }

        // PUT: api/Protocolo/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPROTOCOLO(int id, PROTOCOLO pROTOCOLO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pROTOCOLO.ID_PROTOCOLO)
            {
                return BadRequest();
            }

            db.Entry(pROTOCOLO).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PROTOCOLOExists(id))
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

        // POST: api/Protocolo
        [ResponseType(typeof(PROTOCOLO))]
        public async Task<IHttpActionResult> PostPROTOCOLO(PROTOCOLO pROTOCOLO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_PROTOCOLO;").First();

            pROTOCOLO.ID_PROTOCOLO = NextValue;

            db.PROTOCOLOes.Add(pROTOCOLO);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PROTOCOLOExists(pROTOCOLO.ID_PROTOCOLO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROTOCOLO.ID_PROTOCOLO }, pROTOCOLO);
        }

        // DELETE: api/Protocolo/5
        [ResponseType(typeof(PROTOCOLO))]
        public async Task<IHttpActionResult> DeletePROTOCOLO(int id)
        {
            PROTOCOLO pROTOCOLO = await db.PROTOCOLOes.FindAsync(id);
            if (pROTOCOLO == null)
            {
                return NotFound();
            }

            db.PROTOCOLOes.Remove(pROTOCOLO);
            await db.SaveChangesAsync();

            return Ok(pROTOCOLO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PROTOCOLOExists(int id)
        {
            return db.PROTOCOLOes.Count(e => e.ID_PROTOCOLO == id) > 0;
        }
    }
}