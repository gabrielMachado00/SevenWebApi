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
    public class AtendimentoProtocoloController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/AtendimentoProtocolo
        public IQueryable<ATENDIMENTO_PROTOCOLO> GetATENDIMENTO_PROTOCOLO()
        {
            return db.ATENDIMENTO_PROTOCOLO;
        }

        // GET: api/AtendimentoProtocolo/5
        [ResponseType(typeof(ATENDIMENTO_PROTOCOLO))]
        public async Task<IHttpActionResult> GetATENDIMENTO_PROTOCOLO(int id)
        {
            ATENDIMENTO_PROTOCOLO aTENDIMENTO_PROTOCOLO = await db.ATENDIMENTO_PROTOCOLO.FindAsync(id);
            if (aTENDIMENTO_PROTOCOLO == null)
            {
                return NotFound();
            }

            return Ok(aTENDIMENTO_PROTOCOLO);
        }

        // PUT: api/AtendimentoProtocolo/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutATENDIMENTO_PROTOCOLO(int id, ATENDIMENTO_PROTOCOLO aTENDIMENTO_PROTOCOLO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aTENDIMENTO_PROTOCOLO.ID_ATENDIMENTO)
            {
                return BadRequest();
            }

            db.Entry(aTENDIMENTO_PROTOCOLO).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ATENDIMENTO_PROTOCOLOExists(id))
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

        // POST: api/AtendimentoProtocolo
        [ResponseType(typeof(ATENDIMENTO_PROTOCOLO))]
        public async Task<IHttpActionResult> PostATENDIMENTO_PROTOCOLO(ATENDIMENTO_PROTOCOLO aTENDIMENTO_PROTOCOLO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ATENDIMENTO_PROTOCOLO.Add(aTENDIMENTO_PROTOCOLO);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ATENDIMENTO_PROTOCOLOExists(aTENDIMENTO_PROTOCOLO.ID_ATENDIMENTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aTENDIMENTO_PROTOCOLO.ID_ATENDIMENTO }, aTENDIMENTO_PROTOCOLO);
        }

        // DELETE: api/AtendimentoProtocolo/5
        [ResponseType(typeof(ATENDIMENTO_PROTOCOLO))]
        public async Task<IHttpActionResult> DeleteATENDIMENTO_PROTOCOLO(int id)
        {
            ATENDIMENTO_PROTOCOLO aTENDIMENTO_PROTOCOLO = await db.ATENDIMENTO_PROTOCOLO.FindAsync(id);
            if (aTENDIMENTO_PROTOCOLO == null)
            {
                return NotFound();
            }

            db.ATENDIMENTO_PROTOCOLO.Remove(aTENDIMENTO_PROTOCOLO);
            await db.SaveChangesAsync();

            return Ok(aTENDIMENTO_PROTOCOLO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ATENDIMENTO_PROTOCOLOExists(int id)
        {
            return db.ATENDIMENTO_PROTOCOLO.Count(e => e.ID_ATENDIMENTO == id) > 0;
        }
    }
}