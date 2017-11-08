using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SevenMedicalApi.Models;
using SisMedApi.Models;

namespace SevenMedicalApi.Controllers
{
    public class UnidadesProdutoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/UnidadesProduto
        public IQueryable<UNIDADE_PRODUTO> GetUNIDADE_PRODUTO()
        {
            return db.UNIDADE_PRODUTO;
        }

        // GET: api/UnidadesProduto/5
        [ResponseType(typeof(UNIDADE_PRODUTO))]
        public IHttpActionResult GetUNIDADE_PRODUTO(int id)
        {
            UNIDADE_PRODUTO uNIDADE_PRODUTO = db.UNIDADE_PRODUTO.Find(id);
            if (uNIDADE_PRODUTO == null)
            {
                return NotFound();
            }

            return Ok(uNIDADE_PRODUTO);
        }

        // PUT: api/UnidadesProduto/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUNIDADE_PRODUTO(int id, UNIDADE_PRODUTO uNIDADE_PRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uNIDADE_PRODUTO.ID_UNIDADE)
            {
                return BadRequest();
            }

            db.Entry(uNIDADE_PRODUTO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UNIDADE_PRODUTOExists(id))
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

        // POST: api/UnidadesProduto
        [ResponseType(typeof(UNIDADE_PRODUTO))]
        public IHttpActionResult PostUNIDADE_PRODUTO(UNIDADE_PRODUTO uNIDADE_PRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_UNIDADE_PRODUTO;").First();

            uNIDADE_PRODUTO.ID_UNIDADE = NextValue;

            db.UNIDADE_PRODUTO.Add(uNIDADE_PRODUTO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UNIDADE_PRODUTOExists(uNIDADE_PRODUTO.ID_UNIDADE))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = uNIDADE_PRODUTO.ID_UNIDADE }, uNIDADE_PRODUTO);
        }

        // DELETE: api/UnidadesProduto/5
        [ResponseType(typeof(UNIDADE_PRODUTO))]
        public IHttpActionResult DeleteUNIDADE_PRODUTO(int id)
        {
            UNIDADE_PRODUTO uNIDADE_PRODUTO = db.UNIDADE_PRODUTO.Find(id);
            if (uNIDADE_PRODUTO == null)
            {
                return NotFound();
            }

            db.UNIDADE_PRODUTO.Remove(uNIDADE_PRODUTO);
            db.SaveChanges();

            return Ok(uNIDADE_PRODUTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UNIDADE_PRODUTOExists(int id)
        {
            return db.UNIDADE_PRODUTO.Count(e => e.ID_UNIDADE == id) > 0;
        }
    }
}