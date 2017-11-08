using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;

namespace SisMedApi.Controllers
{
    public class FornecedorController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/Fornecedor
        public IQueryable<FORNECEDOR> GetFORNECEDORs()
        {
            return db.FORNECEDORs.OrderBy(c=>c.RAZAO_SOCIAL);
        }

        // GET: api/Fornecedor/5
        [ResponseType(typeof(FORNECEDOR))]
        public IHttpActionResult GetFORNECEDOR(int id)
        {
            FORNECEDOR fORNECEDOR = db.FORNECEDORs.Find(id);
            if (fORNECEDOR == null)
            {
                return NotFound();
            }

            return Ok(fORNECEDOR);
        }

        // PUT: api/Fornecedor/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFORNECEDOR(int id, FORNECEDOR fORNECEDOR)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fORNECEDOR.ID_FORNECEDOR)
            {
                return BadRequest();
            }

            db.Entry(fORNECEDOR).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FORNECEDORExists(id))
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

        // POST: api/Fornecedor
        [ResponseType(typeof(FORNECEDOR))]
        public IHttpActionResult PostFORNECEDOR(FORNECEDOR fORNECEDOR)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_FORNECEDOR;").First();

            fORNECEDOR.ID_FORNECEDOR = NextValue;

            db.FORNECEDORs.Add(fORNECEDOR);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FORNECEDORExists(fORNECEDOR.ID_FORNECEDOR))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = fORNECEDOR.ID_FORNECEDOR }, fORNECEDOR);
        }

        // DELETE: api/Fornecedor/5
        [ResponseType(typeof(FORNECEDOR))]
        public IHttpActionResult DeleteFORNECEDOR(int id)
        {
            FORNECEDOR fORNECEDOR = db.FORNECEDORs.Find(id);
            if (fORNECEDOR == null)
            {
                return NotFound();
            }

            db.FORNECEDORs.Remove(fORNECEDOR);
            db.SaveChanges();

            return Ok(fORNECEDOR);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FORNECEDORExists(int id)
        {
            return db.FORNECEDORs.Count(e => e.ID_FORNECEDOR == id) > 0;
        }
    }
}