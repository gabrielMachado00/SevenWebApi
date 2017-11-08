using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;

namespace SisMedApi.Controllers
{
    public class CONVENIO_DEDUCAODTO
    {
        public int ID_DEDUCAO { get; set; }
        public int ID_CONVENIO { get; set; }
        public string DESCRICAO { get; set; }
        public System.Decimal PERCENTUAL { get; set; }
        public string CONVENIO { get; set; }
    }

    public class ConvenioDeducaoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/ConvenioDeducoes/1
        [Route("api/ConvenioDeducoes/{idConvenio}")]
        public IEnumerable<CONVENIO_DEDUCAODTO> GetCONVENIO_DEDUCOES(int idConvenio)
        {
            return (from cd in db.CONVENIO_DEDUCAO
                    join c in db.CONVENIOs on cd.ID_CONVENIO equals c.ID_CONVENIO
                    select new CONVENIO_DEDUCAODTO()
                    {
                        ID_DEDUCAO = cd.ID_DEDUCAO,
                        ID_CONVENIO = cd.ID_CONVENIO,
                        DESCRICAO = cd.DESCRICAO,
                        PERCENTUAL = cd.PERCENTUAL,
                        CONVENIO = c.DESCRICAO
                    }).Where(cd => cd.ID_CONVENIO == idConvenio).ToList();
        }

        // GET api/Convenio/1/Deducao/1
        [Route("api/Convenio/{idConvenio}/Deducao/{idDeducao}")]
        public IEnumerable<CONVENIO_DEDUCAODTO> GetCONVENIO_DEDUCAO(int idConvenio, int idDeducao)
        {
            return (from cd in db.CONVENIO_DEDUCAO
                    join c in db.CONVENIOs on cd.ID_CONVENIO equals c.ID_CONVENIO
                    select new CONVENIO_DEDUCAODTO()
                    {
                        ID_DEDUCAO = cd.ID_DEDUCAO,
                        ID_CONVENIO = cd.ID_CONVENIO,
                        DESCRICAO = cd.DESCRICAO,
                        PERCENTUAL = cd.PERCENTUAL,
                        CONVENIO = c.DESCRICAO
                    }).Where(cd => cd.ID_CONVENIO == idConvenio && cd.ID_DEDUCAO == idDeducao).ToList();
        }

        //PUT api/ConvenioDeducoes/PutCONVENIO_DEDUCAO/1/1
        [ResponseType(typeof(void))]
        [Route("api/ConvenioDeducoes/PutCONVENIO_DEDUCAO/{idConvenio}/{idDeducao}")]
        public IHttpActionResult PutCONVENIO_DEDUCAO(int idConvenio, int idDeducao, CONVENIO_DEDUCAO cONVENIO_DEDUCAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idConvenio != cONVENIO_DEDUCAO.ID_CONVENIO)
            {
                return BadRequest();
            }

            if (idDeducao != cONVENIO_DEDUCAO.ID_DEDUCAO)
            {
                return BadRequest();
            }

            db.Entry(cONVENIO_DEDUCAO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CONVENIO_DEDUCAOExists(idConvenio, idDeducao))
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

        // POST: api/ConvenioDeducao
        [ResponseType(typeof(CONVENIO_DEDUCAO))]
        public async Task<IHttpActionResult> PostCONVENIO_DEDUCAO(CONVENIO_DEDUCAO cONVENIO_DEDUCAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_DEDUCAO;").First();

            cONVENIO_DEDUCAO.ID_DEDUCAO = NextValue;

            db.CONVENIO_DEDUCAO.Add(cONVENIO_DEDUCAO);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CONVENIO_DEDUCAOExists(cONVENIO_DEDUCAO.ID_CONVENIO, cONVENIO_DEDUCAO.ID_DEDUCAO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cONVENIO_DEDUCAO.ID_DEDUCAO }, cONVENIO_DEDUCAO);
        }

        // DELETE: api/ConvenioDeducao/5
        [ResponseType(typeof(CONVENIO_DEDUCAO))]
        public async Task<IHttpActionResult> DeleteCONVENIO_DEDUCAO(int id)
        {
            CONVENIO_DEDUCAO cONVENIO_DEDUCAO = await db.CONVENIO_DEDUCAO.FindAsync(id);
            if (cONVENIO_DEDUCAO == null)
            {
                return NotFound();
            }

            db.CONVENIO_DEDUCAO.Remove(cONVENIO_DEDUCAO);
            await db.SaveChangesAsync();

            return Ok(cONVENIO_DEDUCAO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CONVENIO_DEDUCAOExists(int idConvenio, int idDeducao)
        {
            return db.CONVENIO_DEDUCAO.Count(e => e.ID_DEDUCAO == idDeducao && e.ID_CONVENIO == idConvenio) > 0;
        }
    }
}