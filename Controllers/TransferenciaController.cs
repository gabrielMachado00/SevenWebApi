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
    public class TRANSFERENCIADTO
    {
        public int ID_TRANSFERENCIA { get; set; }
        public int ID_CONTA_ORIGEM { get; set; }
        public int ID_CONTA_DESTINO { get; set; }
        public System.Decimal VALOR { get; set; }
        public System.DateTime DATA { get; set; }
        public string LOGIN { get; set; }
        public string CONTA_ORIGEM { get; set; }
        public string CONTA_DESTINO { get; set; }
    }

    public class TransferenciaController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/transferencias
        [Route("api/transferencias")]
        public IEnumerable<TRANSFERENCIADTO> GetTRANSFERENCIAs()
        {
            return (from t in db.TRANSFERENCIAs
                    join co in db.CONTAs on t.ID_CONTA_ORIGEM equals co.ID_CONTA
                    join cd in db.CONTAs on t.ID_CONTA_DESTINO equals cd.ID_CONTA
                    select new TRANSFERENCIADTO()
                    {
                        ID_TRANSFERENCIA = t.ID_TRANSFERENCIA,
                        ID_CONTA_ORIGEM = t.ID_CONTA_ORIGEM,
                        ID_CONTA_DESTINO = t.ID_CONTA_DESTINO,
                        VALOR = t.VALOR,
                        DATA = t.DATA,
                        LOGIN = t.LOGIN,
                        CONTA_ORIGEM = co.DESCRICAO,
                        CONTA_DESTINO = cd.DESCRICAO
                    }).ToList();
        }

        [Route("api/transferencias/periodo/{dtInicial}/{dtFinal}")]
        public IEnumerable<TRANSFERENCIADTO> GetTRANSFERENCIAS_PERIODO(DateTime dtInicial, DateTime dtFinal)
        {
            return (from t in db.TRANSFERENCIAs
                    join co in db.CONTAs on t.ID_CONTA_ORIGEM equals co.ID_CONTA
                    join cd in db.CONTAs on t.ID_CONTA_DESTINO equals cd.ID_CONTA
                    select new TRANSFERENCIADTO()
                    {
                        ID_TRANSFERENCIA = t.ID_TRANSFERENCIA,
                        ID_CONTA_ORIGEM = t.ID_CONTA_ORIGEM,
                        ID_CONTA_DESTINO = t.ID_CONTA_DESTINO,
                        VALOR = t.VALOR,
                        DATA = t.DATA,
                        LOGIN = t.LOGIN,
                        CONTA_ORIGEM = co.DESCRICAO,
                        CONTA_DESTINO = cd.DESCRICAO
                    }).Where(t => DbFunctions.TruncateTime(t.DATA) >= DbFunctions.TruncateTime(dtInicial) &&
                                  DbFunctions.TruncateTime(t.DATA) <= DbFunctions.TruncateTime(dtFinal)).ToList();
        }

        // PUT: api/Transferencia/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTRANSFERENCIA(int id, TRANSFERENCIA tRANSFERENCIA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tRANSFERENCIA.ID_TRANSFERENCIA)
            {
                return BadRequest();
            }

            db.Entry(tRANSFERENCIA).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TRANSFERENCIAExists(id))
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

        // POST: api/Transferencia
        [ResponseType(typeof(TRANSFERENCIA))]
        public IHttpActionResult PostTRANSFERENCIA(TRANSFERENCIA tRANSFERENCIA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_TRANSFERENCIA;").First();

            tRANSFERENCIA.ID_TRANSFERENCIA = NextValue;

            db.TRANSFERENCIAs.Add(tRANSFERENCIA);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TRANSFERENCIAExists(tRANSFERENCIA.ID_TRANSFERENCIA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tRANSFERENCIA.ID_TRANSFERENCIA }, tRANSFERENCIA);
        }

        // DELETE: api/Transferencia/5
        [ResponseType(typeof(TRANSFERENCIA))]
        public IHttpActionResult DeleteTRANSFERENCIA(int id)
        {
            TRANSFERENCIA tRANSFERENCIA = db.TRANSFERENCIAs.Find(id);
            if (tRANSFERENCIA == null)
            {
                return NotFound();
            }

            db.TRANSFERENCIAs.Remove(tRANSFERENCIA);
            db.SaveChanges();

            return Ok(tRANSFERENCIA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TRANSFERENCIAExists(int id)
        {
            return db.TRANSFERENCIAs.Count(e => e.ID_TRANSFERENCIA == id) > 0;
        }
    }
}