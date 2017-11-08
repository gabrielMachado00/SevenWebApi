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
    public class DOCUMENTO_EXTRATODTO
    {
        public int ID_EXTRATO { get; set; }
        public int ID_DOCUMENTO { get; set; }
        public decimal VALOR { get; set; }
        public DateTime DATA { get; set; }
        public string LOGIN { get; set; }
        public Nullable<decimal> DESCONTO { get; set; }
        public int ID_CONTA { get; set; }
        public string CONTA { get; set; }
    }
    public class DocumentoExtratoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/documento/1/extrato
        [Route("api/documento/{idDocumento}/extrato")]
        public IEnumerable<DOCUMENTO_EXTRATODTO> GetDOCUMENTO_EXTRATO(int idDocumento)
        {
            return (from d in db.DOCUMENTO_EXTRATO
                    join c in db.CONTAs on d.ID_CONTA equals c.ID_CONTA
                    select new DOCUMENTO_EXTRATODTO()
                    {
                        ID_EXTRATO = d.ID_EXTRATO,
                        ID_DOCUMENTO = d.ID_DOCUMENTO,
                        VALOR = d.VALOR,
                        DATA = d.DATA,
                        LOGIN = d.LOGIN,
                        DESCONTO = d.DESCONTO,
                        ID_CONTA = d.ID_CONTA,
                        CONTA = c.DESCRICAO
                    }).Where(d => d.ID_DOCUMENTO == idDocumento).ToList();
        }

        // PUT: api/DocumentoExtrato/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDOCUMENTO_EXTRATO(int id, DOCUMENTO_EXTRATO dOCUMENTO_EXTRATO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dOCUMENTO_EXTRATO.ID_EXTRATO)
            {
                return BadRequest();
            }

            db.Entry(dOCUMENTO_EXTRATO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DOCUMENTO_EXTRATOExists(id))
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

        // POST: api/DocumentoExtrato
        [ResponseType(typeof(DOCUMENTO_EXTRATO))]
        public IHttpActionResult PostDOCUMENTO_EXTRATO(DOCUMENTO_EXTRATO dOCUMENTO_EXTRATO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_EXTRATO;").First();

            dOCUMENTO_EXTRATO.ID_EXTRATO = NextValue;

            db.DOCUMENTO_EXTRATO.Add(dOCUMENTO_EXTRATO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DOCUMENTO_EXTRATOExists(dOCUMENTO_EXTRATO.ID_EXTRATO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = dOCUMENTO_EXTRATO.ID_EXTRATO }, dOCUMENTO_EXTRATO);
        }

        // DELETE: api/DocumentoExtrato/5
        [ResponseType(typeof(DOCUMENTO_EXTRATO))]
        public IHttpActionResult DeleteDOCUMENTO_EXTRATO(int id)
        {
            DOCUMENTO_EXTRATO dOCUMENTO_EXTRATO = db.DOCUMENTO_EXTRATO.Find(id);
            if (dOCUMENTO_EXTRATO == null)
            {
                return NotFound();
            }

            db.DOCUMENTO_EXTRATO.Remove(dOCUMENTO_EXTRATO);
            db.SaveChanges();

            return Ok(dOCUMENTO_EXTRATO);
        }

        
        [ResponseType(typeof(DOCUMENTO_EXTRATO))]
        [Route("api/DocumentoExtrato/DeleteDOCUMENTO_EXTRATO_PORIDDOCUMENTO/{id}")]
        public IHttpActionResult DeleteDOCUMENTO_EXTRATO_PORIDDOCUMENTO(int id)
        {
            List<DOCUMENTO_EXTRATO> dOCUMENTO_EXTRATO = db.DOCUMENTO_EXTRATO.Where(c=>c.ID_DOCUMENTO == id).ToList();
            if (dOCUMENTO_EXTRATO == null)
            {
                return NotFound();
            }
            
            for(var i=0; i < dOCUMENTO_EXTRATO.Count; i++)
            {
                db.DOCUMENTO_EXTRATO.Remove(dOCUMENTO_EXTRATO[i]);
                db.SaveChanges();
            }

            return Ok(dOCUMENTO_EXTRATO);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DOCUMENTO_EXTRATOExists(int id)
        {
            return db.DOCUMENTO_EXTRATO.Count(e => e.ID_EXTRATO == id) > 0;
        }
    }
}