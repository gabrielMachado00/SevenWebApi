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
    public class CARTAODTO
    {
        public int ID_CARTAO { get; set; }
        public string BANDEIRA { get; set; }
        public int NUM_DIAS_VENCIMENTO { get; set; }
        public string TIPO { get; set; }
        public string DESC_TIPO { get; set; }
    }
    public class CartoesController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/cartao
        [Route("api/cartao")]
        public IEnumerable<CARTAODTO> GetCartao()
        {
            return (from c in db.CARTAOs
                    select new CARTAODTO()
                    {
                        ID_CARTAO = c.ID_CARTAO,
                        BANDEIRA = c.BANDEIRA,
                        NUM_DIAS_VENCIMENTO = c.NUM_DIAS_VENCIMENTO,
                        TIPO = c.TIPO,
                        DESC_TIPO = c.TIPO == "C" ? "CRÉDITO" : "DÉBITO"
                    }).ToList();
        }

        // GET api/conta/1
        [Route("api/cartao/{idCartao}")]
        public IEnumerable<CARTAODTO> GetCartao(int idCartao)
        {
            return (from c in db.CARTAOs
                    select new CARTAODTO()
                    {
                        ID_CARTAO = c.ID_CARTAO,
                        BANDEIRA = c.BANDEIRA,
                        NUM_DIAS_VENCIMENTO = c.NUM_DIAS_VENCIMENTO,
                        TIPO = c.TIPO,
                        DESC_TIPO = c.TIPO == "C" ? "CRÉDITO" : "DÉBITO"
                    }).Where(c => c.ID_CARTAO == idCartao).ToList();
        }

        // PUT: api/Cartoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCARTAO(int id, CARTAO cARTAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cARTAO.ID_CARTAO)
            {
                return BadRequest();
            }

            db.Entry(cARTAO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CARTAOExists(id))
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

        // POST: api/Cartoes
        [ResponseType(typeof(CARTAO))]
        public IHttpActionResult PostCARTAO(CARTAO cARTAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_CARTAO;").First();

            cARTAO.ID_CARTAO = NextValue;

            db.CARTAOs.Add(cARTAO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CARTAOExists(cARTAO.ID_CARTAO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cARTAO.ID_CARTAO }, cARTAO);
        }

        // DELETE: api/Cartoes/5
        [ResponseType(typeof(CARTAO))]
        public IHttpActionResult DeleteCARTAO(int id)
        {
            CARTAO cARTAO = db.CARTAOs.Find(id);
            if (cARTAO == null)
            {
                return NotFound();
            }

            db.CARTAOs.Remove(cARTAO);
            db.SaveChanges();

            return Ok(cARTAO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CARTAOExists(int id)
        {
            return db.CARTAOs.Count(e => e.ID_CARTAO == id) > 0;
        }
    }
}