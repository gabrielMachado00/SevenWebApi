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
    public class IndicacaoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/Indicacao
        public IQueryable<INDICACAO> GetINDICACAOs()
        {
            return db.INDICACAOs;
        }

        // GET: api/Indicacao/5
        [ResponseType(typeof(INDICACAO))]
        public IHttpActionResult GetINDICACAO(int id)
        {
            INDICACAO iNDICACAO = db.INDICACAOs.Find(id);
            if (iNDICACAO == null)
            {
                return NotFound();
            }

            return Ok(iNDICACAO);
        }

        // PUT: api/Indicacao/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutINDICACAO(int id, INDICACAO iNDICACAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != iNDICACAO.ID_INDICACAO)
            {
                return BadRequest();
            }

            db.Entry(iNDICACAO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!INDICACAOExists(id))
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

        // POST: api/Indicacao
        [ResponseType(typeof(INDICACAO))]
        public IHttpActionResult PostINDICACAO(INDICACAO iNDICACAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_INDICACAO;").First();

            iNDICACAO.ID_INDICACAO = NextValue;

            db.INDICACAOs.Add(iNDICACAO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (INDICACAOExists(iNDICACAO.ID_INDICACAO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = iNDICACAO.ID_INDICACAO }, iNDICACAO);
        }

        // DELETE: api/Indicacao/5
        [ResponseType(typeof(INDICACAO))]
        public IHttpActionResult DeleteINDICACAO(int id)
        {
            INDICACAO iNDICACAO = db.INDICACAOs.Find(id);
            if (iNDICACAO == null)
            {
                return NotFound();
            }

            db.INDICACAOs.Remove(iNDICACAO);
            db.SaveChanges();

            return Ok(iNDICACAO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool INDICACAOExists(int id)
        {
            return db.INDICACAOs.Count(e => e.ID_INDICACAO == id) > 0;
        }
    }
}