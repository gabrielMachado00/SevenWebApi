using System.Collections.Generic;
using System.Data;
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
    public class UNIDADE_FARMACIADTO
    {
        public int ID_FARMACIA { get; set; }
        public int ID_UNIDADE { get; set; }
        public string FARMACIA { get; set; }
        public string UNIDADE { get; set; }
    }
    public class UnidadeFarmaciaController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [Route("api/unidade/{idUnidade}/farmacia")]
        public IEnumerable<UNIDADE_FARMACIADTO> GetFARMACIAs_UNIDADE(int idUnidade)
        {
            return (from uf in db.UNIDADE_FARMACIA
                    join u in db.UNIDADEs on uf.ID_UNIDADE equals u.ID_UNIDADE
                    join f in db.FARMACIA_SATELITE on uf.ID_FARMACIA equals f.ID_FARMACIA
                    select new UNIDADE_FARMACIADTO()
                    {
                        ID_FARMACIA = uf.ID_FARMACIA,
                        ID_UNIDADE = uf.ID_UNIDADE,
                        FARMACIA = f.DESCRICAO,
                        UNIDADE = u.NOME_FANTASIA
                    }).Where(s => s.ID_UNIDADE == idUnidade).ToList();
        }

        [Route("api/unidade/{idUnidade}/farmacia/{idFarmacia}")]
        public IEnumerable<UNIDADE_FARMACIADTO> GetFARMACIAs_UNIDADE(int idUnidade, int idFarmacia)
        {
            return (from uf in db.UNIDADE_FARMACIA
                    join u in db.UNIDADEs on uf.ID_UNIDADE equals u.ID_UNIDADE
                    join f in db.FARMACIA_SATELITE on uf.ID_FARMACIA equals f.ID_FARMACIA
                    select new UNIDADE_FARMACIADTO()
                    {
                        ID_FARMACIA = uf.ID_FARMACIA,
                        ID_UNIDADE = uf.ID_UNIDADE,
                        FARMACIA = f.DESCRICAO,
                        UNIDADE = u.NOME_FANTASIA
                    }).Where(s => s.ID_UNIDADE == idUnidade && s.ID_FARMACIA == idFarmacia).ToList();
        }

        // PUT: api/UnidadeFarmacia/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUNIDADE_FARMACIA(int id, UNIDADE_FARMACIA uNIDADE_FARMACIA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uNIDADE_FARMACIA.ID_FARMACIA)
            {
                return BadRequest();
            }

            db.Entry(uNIDADE_FARMACIA).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UNIDADE_FARMACIAExists(id))
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

        // POST: api/UnidadeFarmacia
        [ResponseType(typeof(UNIDADE_FARMACIA))]
        public IHttpActionResult PostUNIDADE_FARMACIA(UNIDADE_FARMACIA uNIDADE_FARMACIA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UNIDADE_FARMACIA.Add(uNIDADE_FARMACIA);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UNIDADE_FARMACIAExists(uNIDADE_FARMACIA.ID_FARMACIA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = uNIDADE_FARMACIA.ID_FARMACIA }, uNIDADE_FARMACIA);
        }

        // DELETE: api/UnidadeFarmacia/5
        [ResponseType(typeof(UNIDADE_FARMACIA))]
        [Route("api/unidadefarmacia/Delete/{idUnidade}/{idFarmacia}")]
        public IHttpActionResult DeleteUNIDADE_FARMACIA(int idUnidade, int idFarmacia)
        {
            UNIDADE_FARMACIA uNIDADE_FARMACIA = db.UNIDADE_FARMACIA.Where(uf => uf.ID_UNIDADE == idUnidade && uf.ID_FARMACIA == idFarmacia).FirstOrDefault();
            if (uNIDADE_FARMACIA == null)
            {
                return NotFound();
            }

            var sql = @" DELETE FROM UNIDADE_FARMACIA WHERE ID_UNIDADE = {0} AND ID_FARMACIA = {1} ";

            if (db.Database.ExecuteSqlCommand(sql, idUnidade, idFarmacia) > 0)
                return Ok(uNIDADE_FARMACIA);
            else
                return NotFound();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UNIDADE_FARMACIAExists(int id)
        {
            return db.UNIDADE_FARMACIA.Count(e => e.ID_FARMACIA == id) > 0;
        }
    }
}