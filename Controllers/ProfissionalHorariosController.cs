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

namespace SevenMedicalApi.Controllers
{
    public class PROFISSIONAL_HORARIODTO
    {
        public int ID_HORARIO_PROFISSIONAL { get; set; }
        public int COD_DIA_SEMANA { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public TimeSpan HORA_INICIAL_1 { get; set; }
        public TimeSpan HORA_FINAL_1 { get; set; }
        public TimeSpan HORA_INICIAL_2 { get; set; }
        public TimeSpan HORA_FINAL_2 { get; set; }
        public TimeSpan HORA_INICIAL_3 { get; set; }
        public TimeSpan HORA_FINAL_3 { get; set; }
        public string DIA_SEMANA { get; set; }
        public string NOME { get; set; }
    }

    public class ProfissionalHorariosController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/profissional/1/horario
        [Route("api/profissional/{idProfissional}/horario")]
        public IEnumerable<PROFISSIONAL_HORARIODTO> GetPROFISSIONAL_HORARIO(int idProfissional)
        {
            return (from ph in db.PROFISSIONAL_HORARIO
                    join p in db.PROFISSIONALs on ph.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new PROFISSIONAL_HORARIODTO()
                    {
                        ID_HORARIO_PROFISSIONAL = ph.ID_HORARIO_PROFISSIONAL,
                        ID_PROFISSIONAL = ph.ID_PROFISSIONAL,
                        COD_DIA_SEMANA = ph.COD_DIA_SEMANA,
                        HORA_INICIAL_1 = ph.HORA_INICIAL_1,
                        HORA_FINAL_1 = ph.HORA_FINAL_1,
                        HORA_INICIAL_2 = ph.HORA_INICIAL_2,
                        HORA_FINAL_2 = ph.HORA_FINAL_2,
                        HORA_INICIAL_3 = ph.HORA_INICIAL_3,
                        HORA_FINAL_3 = ph.HORA_FINAL_3,
                        DIA_SEMANA = ph.COD_DIA_SEMANA == 1 ? "DOMINGO" :
                                     ph.COD_DIA_SEMANA == 2 ? "SEGUNDA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 3 ? "TERÇA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 4 ? "QUARTA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 5 ? "QUINTA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 6 ? "SEXTA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 7 ? "SÁBADO" : "",
                        NOME = p.NOME
                    }).Where(ph => ph.ID_PROFISSIONAL == idProfissional).ToList();

            //    return db.CLIENTE_ENDERECO.Where(c => c.ID_CLIENTE == idCliente);
        }


        // GET api/profissional/horario/5
        [Route("api/profissional/horario/{idHorarioProfissional}")]
        public IEnumerable<PROFISSIONAL_HORARIODTO> GetHORARIO(int idHorarioProfissional)
        {
            return (from ph in db.PROFISSIONAL_HORARIO
                    join p in db.PROFISSIONALs on ph.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new PROFISSIONAL_HORARIODTO()
                    {
                        ID_HORARIO_PROFISSIONAL = ph.ID_HORARIO_PROFISSIONAL,
                        ID_PROFISSIONAL = ph.ID_PROFISSIONAL,
                        COD_DIA_SEMANA = ph.COD_DIA_SEMANA,
                        HORA_INICIAL_1 = ph.HORA_INICIAL_1,
                        HORA_FINAL_1 = ph.HORA_FINAL_1,
                        HORA_INICIAL_2 = ph.HORA_INICIAL_2,
                        HORA_FINAL_2 = ph.HORA_FINAL_2,
                        HORA_INICIAL_3 = ph.HORA_INICIAL_3,
                        HORA_FINAL_3 = ph.HORA_FINAL_3,
                        DIA_SEMANA = ph.COD_DIA_SEMANA == 1 ? "DOMINGO" :
                                     ph.COD_DIA_SEMANA == 2 ? "SEGUNDA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 3 ? "TERÇA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 4 ? "QUARTA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 5 ? "QUINTA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 6 ? "SEXTA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 7 ? "SÁBADO" : "",
                        NOME = p.NOME
                    }).Where(ph => ph.ID_HORARIO_PROFISSIONAL == idHorarioProfissional).ToList();
        }

        // GET api/profissional/horario/5/3
        [Route("api/profissionais/horarios/{idProfissional}/{CodDiaSemana}")]
        public IEnumerable<PROFISSIONAL_HORARIODTO> GetHORARIO_SEMANA(int idProfissional, int CodDiaSemana)
        {
            return (from ph in db.PROFISSIONAL_HORARIO
                    join p in db.PROFISSIONALs on ph.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new PROFISSIONAL_HORARIODTO()
                    {
                        ID_HORARIO_PROFISSIONAL = ph.ID_HORARIO_PROFISSIONAL,
                        ID_PROFISSIONAL = ph.ID_PROFISSIONAL,
                        COD_DIA_SEMANA = ph.COD_DIA_SEMANA,
                        HORA_INICIAL_1 = ph.HORA_INICIAL_1,
                        HORA_FINAL_1 = ph.HORA_FINAL_1,
                        HORA_INICIAL_2 = ph.HORA_INICIAL_2,
                        HORA_FINAL_2 = ph.HORA_FINAL_2,
                        HORA_INICIAL_3 = ph.HORA_INICIAL_3,
                        HORA_FINAL_3 = ph.HORA_FINAL_3,
                        DIA_SEMANA = ph.COD_DIA_SEMANA == 1 ? "DOMINGO" :
                                     ph.COD_DIA_SEMANA == 2 ? "SEGUNDA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 3 ? "TERÇA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 4 ? "QUARTA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 5 ? "QUINTA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 6 ? "SEXTA-FEIRA" :
                                     ph.COD_DIA_SEMANA == 7 ? "SÁBADO" : "",
                        NOME = p.NOME
                    }).Where(ph => ph.ID_PROFISSIONAL == idProfissional && ph.COD_DIA_SEMANA == CodDiaSemana).ToList();
        }

        // PUT: api/ProfissionalHorarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPROFISSIONAL_HORARIO(int id, PROFISSIONAL_HORARIO pROFISSIONAL_HORARIO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pROFISSIONAL_HORARIO.ID_HORARIO_PROFISSIONAL)
            {
                return BadRequest();
            }

            db.Entry(pROFISSIONAL_HORARIO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PROFISSIONAL_HORARIOExists(id))
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

        // POST: api/ProfissionalHorarios
        [ResponseType(typeof(PROFISSIONAL_HORARIO))]
        public IHttpActionResult PostPROFISSIONAL_HORARIO(PROFISSIONAL_HORARIO pROFISSIONAL_HORARIO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_HORARIO_PROFISSIONAL;").First();

            pROFISSIONAL_HORARIO.ID_HORARIO_PROFISSIONAL = NextValue;

            db.PROFISSIONAL_HORARIO.Add(pROFISSIONAL_HORARIO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PROFISSIONAL_HORARIOExists(pROFISSIONAL_HORARIO.ID_HORARIO_PROFISSIONAL))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROFISSIONAL_HORARIO.ID_HORARIO_PROFISSIONAL }, pROFISSIONAL_HORARIO);
        }

        // DELETE: api/ProfissionalHorarios/5
        [ResponseType(typeof(PROFISSIONAL_HORARIO))]
        public IHttpActionResult DeletePROFISSIONAL_HORARIO(int id)
        {
            PROFISSIONAL_HORARIO pROFISSIONAL_HORARIO = db.PROFISSIONAL_HORARIO.Find(id);
            if (pROFISSIONAL_HORARIO == null)
            {
                return NotFound();
            }

            db.PROFISSIONAL_HORARIO.Remove(pROFISSIONAL_HORARIO);
            db.SaveChanges();

            return Ok(pROFISSIONAL_HORARIO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PROFISSIONAL_HORARIOExists(int id)
        {
            return db.PROFISSIONAL_HORARIO.Count(e => e.ID_HORARIO_PROFISSIONAL == id) > 0;
        }
    }
}