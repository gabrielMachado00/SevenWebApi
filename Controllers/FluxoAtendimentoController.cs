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
    public class FLUXO_ATENDIMENTODTO
    {
        public int ID_CLIENTE { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public int ID_APPOINTMENT { get; set; }
        public DateTime DATA_HORA_INI { get; set; }
        public Nullable<System.DateTime> DATA_HORA_FIM { get; set; }
        public string STATUS { get; set; }
        public string CLIENTE { get; set; }
        public string PROFISSIONAL { get; set; }
        public string DESC_STATUS { get; set; }
        public Nullable<TimeSpan> TEMPO_ESPERA { get; set; }
    }

    public class FluxoAtendimentoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/atendimento/fila
        [Route("api/atendimento/fila")]
        public IEnumerable<FLUXO_ATENDIMENTODTO> GetATENDIMENTO_FILA()
        {
            return (from f in db.FLUXO_ATENDIMENTO
                    join c in db.CLIENTES on f.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on f.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new FLUXO_ATENDIMENTODTO()
                    {
                        ID_CLIENTE = f.ID_CLIENTE,
                        ID_PROFISSIONAL = f.ID_PROFISSIONAL,
                        ID_APPOINTMENT = f.ID_APPOINTMENT,
                        DATA_HORA_INI = f.DATA_HORA_INI,
                        DATA_HORA_FIM = f.DATA_HORA_FIM,
                        STATUS = f.STATUS,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        DESC_STATUS = f.STATUS == "A" ? "AGUARDANDO" :
                                      f.STATUS == "I" ? "INICIADO" :
                                      f.STATUS == "C" ? "CONCLUÍDO" : "",
                        TEMPO_ESPERA = new TimeSpan(0, 0, 0)
                    }).Where(f => f.STATUS == "A").ToList();
        }

        // GET api/atendimento/fila/profissional/1
        [Route("api/atendimento/fila/profissional/{idProfissional}")]
        public IEnumerable<FLUXO_ATENDIMENTODTO> GetATENDIMENTO_FILA_PROFISSIONAL(int idProfissional)
        {
            return (from f in db.FLUXO_ATENDIMENTO
                    join c in db.CLIENTES on f.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on f.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new FLUXO_ATENDIMENTODTO()
                    {
                        ID_CLIENTE = f.ID_CLIENTE,
                        ID_PROFISSIONAL = f.ID_PROFISSIONAL,
                        ID_APPOINTMENT = f.ID_APPOINTMENT,
                        DATA_HORA_INI = f.DATA_HORA_INI,
                        DATA_HORA_FIM = f.DATA_HORA_FIM,
                        STATUS = f.STATUS,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        DESC_STATUS = f.STATUS == "A" ? "AGUARDANDO" :
                                      f.STATUS == "I" ? "INICIADO" :
                                      f.STATUS == "C" ? "CONCLUÍDO" : "",
                        TEMPO_ESPERA = new TimeSpan(0, 0, 0)
                    }).Where(f => f.STATUS == "A" && f.ID_PROFISSIONAL == idProfissional).ToList();
        }

        [Route("api/atendimento/fila/profissional/appointment/{idAppointment}")]
        public IEnumerable<FLUXO_ATENDIMENTODTO> GetATENDIMENTO_FILA_PROF_APT(int idAppointment)
        {
            return (from f in db.FLUXO_ATENDIMENTO
                    join c in db.CLIENTES on f.ID_CLIENTE equals c.ID_CLIENTE
                    join p in db.PROFISSIONALs on f.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new FLUXO_ATENDIMENTODTO()
                    {
                        ID_CLIENTE = f.ID_CLIENTE,
                        ID_PROFISSIONAL = f.ID_PROFISSIONAL,
                        ID_APPOINTMENT = f.ID_APPOINTMENT,
                        DATA_HORA_INI = f.DATA_HORA_INI,
                        DATA_HORA_FIM = f.DATA_HORA_FIM,
                        STATUS = f.STATUS,
                        CLIENTE = c.NOME,
                        PROFISSIONAL = p.NOME,
                        DESC_STATUS = f.STATUS == "A" ? "AGUARDANDO" :
                                      f.STATUS == "I" ? "INICIADO" :
                                      f.STATUS == "C" ? "CONCLUÍDO" : "",
                        TEMPO_ESPERA = new TimeSpan(0, 0, 0)
                    }).Where(f => f.STATUS == "A" && f.ID_APPOINTMENT == idAppointment).ToList();
        }
        // PUT: api/FluxoAtendimento/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFLUXO_ATENDIMENTO(int id, FLUXO_ATENDIMENTO fLUXO_ATENDIMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fLUXO_ATENDIMENTO.ID_APPOINTMENT)
            {
                return BadRequest();
            }

            db.Entry(fLUXO_ATENDIMENTO).State = EntityState.Modified;

            try
            {
                //db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FLUXO_ATENDIMENTOExists(id))
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

        // POST: api/FluxoAtendimento
        [ResponseType(typeof(FLUXO_ATENDIMENTO))]
        public IHttpActionResult PostFLUXO_ATENDIMENTO(FLUXO_ATENDIMENTO fLUXO_ATENDIMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FLUXO_ATENDIMENTO.Add(fLUXO_ATENDIMENTO);

            try
            {
                //db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FLUXO_ATENDIMENTOExists(fLUXO_ATENDIMENTO.ID_APPOINTMENT))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = fLUXO_ATENDIMENTO.ID_APPOINTMENT }, fLUXO_ATENDIMENTO);
        }

        // DELETE: api/FluxoAtendimento/5
        [ResponseType(typeof(FLUXO_ATENDIMENTO))]
        public IHttpActionResult DeleteFLUXO_ATENDIMENTO(int id)
        {
            FLUXO_ATENDIMENTO fLUXO_ATENDIMENTO = db.FLUXO_ATENDIMENTO.Find(id);
            if (fLUXO_ATENDIMENTO == null)
            {
                return NotFound();
            }

            db.FLUXO_ATENDIMENTO.Remove(fLUXO_ATENDIMENTO);
            //db.BulkSaveChanges();
            db.SaveChanges();

            return Ok(fLUXO_ATENDIMENTO);
        }

        // Limpar sala espera
        [ResponseType(typeof(FLUXO_ATENDIMENTO))]
        public IHttpActionResult LimparSalaEspera()
        {
            FLUXO_ATENDIMENTO fLUXO_ATENDIMENTO = db.FLUXO_ATENDIMENTO.Find(0);

            int resultado = db.Database.ExecuteSqlCommand(@"UPDATE FLUXO_ATENDIMENTO
                                                      SET STATUS = 'C',
                                                          DATA_HORA_FIM = ISNULL(DATA_HORA_FIM, GETDATE()-1)
                                                    WHERE STATUS = 'A'
                                                      AND cast (DATA_HORA_INI as date) <= cast(GETDATE()-1 as date)");

            if (resultado <= 0)
            {
                return NotFound();
            }           

            return Ok(fLUXO_ATENDIMENTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FLUXO_ATENDIMENTOExists(int id)
        {
            return db.FLUXO_ATENDIMENTO.Count(e => e.ID_APPOINTMENT == id) > 0;
        }
    }
}