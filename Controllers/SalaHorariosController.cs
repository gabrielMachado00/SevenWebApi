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
    public class SALA_HORARIODTO
    {        
        public int ID_SALA_HORARIO { get; set; }
        public int COD_DIA_SEMANA { get; set; }
        public int ID_SALA { get; set; }
        public TimeSpan HORA_INICIAL_1 { get; set; }
        public TimeSpan HORA_FINAL_1 { get; set; }
        public TimeSpan HORA_INICIAL_2 { get; set; }
        public TimeSpan HORA_FINAL_2 { get; set; }
        public TimeSpan HORA_INICIAL_3 { get; set; }
        public TimeSpan HORA_FINAL_3 { get; set; }
        public string DIA_SEMANA { get; set; }
        public string NOME { get; set; }
    }

    public class SalaHorariosController: ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/sala/1/horario
        [Route("api/sala/{idSala}/horario")]
        public IEnumerable<SALA_HORARIODTO> GetSALA_HORARIO(int idSala)
        {
            return (from sh in db.SALA_HORARIO
                    join s in db.SALAs on sh.ID_SALA equals s.ID_SALA
                    orderby sh.COD_DIA_SEMANA, sh.HORA_FINAL_1, sh.HORA_INICIAL_2, sh.HORA_INICIAL_3
                    select new SALA_HORARIODTO()
                    {
                        ID_SALA_HORARIO = sh.ID_SALA_HORARIO,
                        ID_SALA = sh.ID_SALA,
                        COD_DIA_SEMANA = sh.COD_DIA_SEMANA,
                        HORA_INICIAL_1 = sh.HORA_INICIAL_1,
                        HORA_FINAL_1 = sh.HORA_FINAL_1,
                        HORA_INICIAL_2 = sh.HORA_INICIAL_2,
                        HORA_FINAL_2 = sh.HORA_FINAL_2,
                        HORA_INICIAL_3 = sh.HORA_INICIAL_3,
                        HORA_FINAL_3 = sh.HORA_FINAL_3,
                        DIA_SEMANA = sh.COD_DIA_SEMANA == 1 ? "DOMINGO" :
                                     sh.COD_DIA_SEMANA == 2 ? "SEGUNDA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 3 ? "TERÇA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 4 ? "QUARTA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 5 ? "QUINTA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 6 ? "SEXTA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 7 ? "SÁBADO" : "",
                        NOME = s.DESCRICAO
                    }).Where(sh => sh.ID_SALA == idSala).ToList();                        
        }

        // GET api/sala/horario/5
        [Route("api/sala/horario/{idSalaHorario}")]
        public IEnumerable<SALA_HORARIODTO> GetHORARIO(int idSalaHorario)
        {
            return (from sh in db.SALA_HORARIO
                    join s in db.SALAs on sh.ID_SALA equals s.ID_SALA
                    orderby sh.HORA_FINAL_1, sh.HORA_INICIAL_2, sh.HORA_INICIAL_3
                    select new SALA_HORARIODTO()
                    {
                        ID_SALA_HORARIO = sh.ID_SALA_HORARIO,
                        ID_SALA = sh.ID_SALA,
                        COD_DIA_SEMANA = sh.COD_DIA_SEMANA,
                        HORA_INICIAL_1 = sh.HORA_INICIAL_1,
                        HORA_FINAL_1 = sh.HORA_FINAL_1,
                        HORA_INICIAL_2 = sh.HORA_INICIAL_2,
                        HORA_FINAL_2 = sh.HORA_FINAL_2,
                        HORA_INICIAL_3 = sh.HORA_INICIAL_3,
                        HORA_FINAL_3 = sh.HORA_FINAL_3,
                        DIA_SEMANA = sh.COD_DIA_SEMANA == 1 ? "DOMINGO" :
                                     sh.COD_DIA_SEMANA == 2 ? "SEGUNDA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 3 ? "TERÇA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 4 ? "QUARTA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 5 ? "QUINTA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 6 ? "SEXTA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 7 ? "SÁBADO" : "",
                        NOME = s.DESCRICAO
                    }).Where(ph => ph.ID_SALA_HORARIO == idSalaHorario).ToList();
        }

        // GET api/sala/horario/5/3
        [Route("api/sala/horarios/{idSala}/{CodDiaSemana}")]
        public IEnumerable<SALA_HORARIODTO> GetHORARIO_SEMANA(int idSala, int CodDiaSemana)
        {
            return (from sh in db.SALA_HORARIO
                    join s in db.SALAs on sh.ID_SALA equals s.ID_SALA
                    orderby sh.HORA_FINAL_1, sh.HORA_INICIAL_2, sh.HORA_INICIAL_3
                    select new SALA_HORARIODTO()
                    {
                        ID_SALA_HORARIO = sh.ID_SALA_HORARIO,
                        ID_SALA = sh.ID_SALA,
                        COD_DIA_SEMANA = sh.COD_DIA_SEMANA,
                        HORA_INICIAL_1 = sh.HORA_INICIAL_1,
                        HORA_FINAL_1 = sh.HORA_FINAL_1,
                        HORA_INICIAL_2 = sh.HORA_INICIAL_2,
                        HORA_FINAL_2 = sh.HORA_FINAL_2,
                        HORA_INICIAL_3 = sh.HORA_INICIAL_3,
                        HORA_FINAL_3 = sh.HORA_FINAL_3,
                        DIA_SEMANA = sh.COD_DIA_SEMANA == 1 ? "DOMINGO" :
                                     sh.COD_DIA_SEMANA == 2 ? "SEGUNDA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 3 ? "TERÇA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 4 ? "QUARTA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 5 ? "QUINTA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 6 ? "SEXTA-FEIRA" :
                                     sh.COD_DIA_SEMANA == 7 ? "SÁBADO" : "",
                        NOME = s.DESCRICAO
                    }).Where(ph => ph.ID_SALA == idSala && ph.COD_DIA_SEMANA == CodDiaSemana).ToList();
        }

        // PUT: api/SalaHorarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSALA_HORARIO(int id, SALA_HORARIO pSALA_HORARIO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pSALA_HORARIO.ID_SALA_HORARIO)
            {
                return BadRequest();
            }

            db.Entry(pSALA_HORARIO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SALA_HORARIOExists(id))
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
        [ResponseType(typeof(SALA_HORARIO))]
        public IHttpActionResult PostSALA_HORARIO(SALA_HORARIO pSALA_HORARIO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_SALA_HORARIO;").First();

            pSALA_HORARIO.ID_SALA_HORARIO = NextValue;

            db.SALA_HORARIO.Add(pSALA_HORARIO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SALA_HORARIOExists(pSALA_HORARIO.ID_SALA_HORARIO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pSALA_HORARIO.ID_SALA_HORARIO }, pSALA_HORARIO);
        }

        // DELETE: api/SalaHorarios/5
        [ResponseType(typeof(SALA_HORARIO))]
        public IHttpActionResult DeleteSALA_HORARIO(int id)
        {
            SALA_HORARIO pSALA_HORARIO = db.SALA_HORARIO.Find(id);
            if (pSALA_HORARIO == null)
            {
                return NotFound();
            }

            db.SALA_HORARIO.Remove(pSALA_HORARIO);
            db.SaveChanges();

            return Ok(pSALA_HORARIO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SALA_HORARIOExists(int id)
        {
            return db.SALA_HORARIO.Count(e => e.ID_SALA_HORARIO == id) > 0;
        }
    }
}