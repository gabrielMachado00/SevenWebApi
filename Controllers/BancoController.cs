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
    public class BancoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/Banco
        public IQueryable<BANCO> GetBANCOes()
        {
            return db.BANCOes;
        }

        // GET: api/Banco/5
        [ResponseType(typeof(BANCO))]
        public IHttpActionResult GetBANCO(int id)
        {
            BANCO bANCO = db.BANCOes.Find(id);
            if (bANCO == null)
            {
                return NotFound();
            }

            return Ok(bANCO);
        }

        // PUT: api/Banco/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBANCO(int id, BANCO bANCO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bANCO.ID_BANCO)
            {
                return BadRequest();
            }

            db.Entry(bANCO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BANCOExists(id))
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

        // POST: api/Banco
        [ResponseType(typeof(BANCO))]
        public IHttpActionResult PostBANCO(BANCO bANCO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_BANCO;").First();

            bANCO.ID_BANCO = NextValue;

            db.BANCOes.Add(bANCO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BANCOExists(bANCO.ID_BANCO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = bANCO.ID_BANCO }, bANCO);
        }

        // DELETE: api/Banco/5
        [ResponseType(typeof(BANCO))]
        public IHttpActionResult DeleteBANCO(int id)
        {
            BANCO bANCO = db.BANCOes.Find(id);
            if (bANCO == null)
            {
                return NotFound();
            }

            db.BANCOes.Remove(bANCO);
            db.SaveChanges();

            return Ok(bANCO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BANCOExists(int id)
        {
            return db.BANCOes.Count(e => e.ID_BANCO == id) > 0;
        }
    }
}