using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SevenMedicalApi.Models;
using SisMedApi.Models;
using System.Collections.Generic;

namespace SisMedApi.Controllers
{
    public class APPOINTMENTS_REGIAO_CORPODTO
    {
        public int ID_APPOINTMENTS { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public string AREA_TIPO { get; set; }
    }
    public class AppointmentRegiaoCorpoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/Appointment/1
        [Route("api/AppointmentRegioes/{idAppointment}")]
        public IEnumerable<APPOINTMENTS_REGIAO_CORPODTO> GetAPPOINTMENT_REGIOES(int idAppointment)
        {
            return (from ar in db.APPOINTMENTS_REGIAO_CORPO
                    join rc in db.REGIAO_CORPO on ar.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    select new APPOINTMENTS_REGIAO_CORPODTO()
                    {
                        ID_APPOINTMENTS = ar.ID_APPOINTMENTS,
                        ID_REGIAO_CORPO = ar.ID_REGIAO_CORPO,
                        AREA_TIPO = rc.REGIAO
                    }).Where(ar => ar.ID_APPOINTMENTS == idAppointment).ToList();
        }

        // PUT: api/AppointmentRegiaoCorpo/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAPPOINTMENTS_REGIAO_CORPO(int id, APPOINTMENTS_REGIAO_CORPO aPPOINTMENTS_REGIAO_CORPO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aPPOINTMENTS_REGIAO_CORPO.ID_APPOINTMENTS)
            {
                return BadRequest();
            }

            db.Entry(aPPOINTMENTS_REGIAO_CORPO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!APPOINTMENTS_REGIAO_CORPOExists(id))
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

        // POST: api/AppointmentRegiaoCorpo
        [ResponseType(typeof(APPOINTMENTS_REGIAO_CORPO))]
        public IHttpActionResult PostAPPOINTMENTS_REGIAO_CORPO(APPOINTMENTS_REGIAO_CORPO aPPOINTMENTS_REGIAO_CORPO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.APPOINTMENTS_REGIAO_CORPO.Add(aPPOINTMENTS_REGIAO_CORPO);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = aPPOINTMENTS_REGIAO_CORPO.ID_APPOINTMENTS }, aPPOINTMENTS_REGIAO_CORPO);
        }

        // DELETE: api/AppointmentRegiaoCorpo/5
        [ResponseType(typeof(APPOINTMENTS_REGIAO_CORPO))]
        public IHttpActionResult DeleteAPPOINTMENTS_REGIAO_CORPO(int id)
        {
            APPOINTMENTS_REGIAO_CORPO aPPOINTMENTS_REGIAO_CORPO = db.APPOINTMENTS_REGIAO_CORPO.Find(id);
            if (aPPOINTMENTS_REGIAO_CORPO == null)
            {
                return NotFound();
            }

            db.APPOINTMENTS_REGIAO_CORPO.Remove(aPPOINTMENTS_REGIAO_CORPO);
            db.SaveChanges();

            return Ok(aPPOINTMENTS_REGIAO_CORPO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool APPOINTMENTS_REGIAO_CORPOExists(int id)
        {
            return db.APPOINTMENTS_REGIAO_CORPO.Count(e => e.ID_APPOINTMENTS == id) > 0;
        }
    }
}