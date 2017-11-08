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
    public class FARMACIA_SATELITEDTO
    {
        public int ID_FARMACIA { get; set; }
        public string DESCRICAO { get; set; }
        public string LOCALIZACAO { get; set; }
        public string RAMAL { get; set; }
        public string RESPONSAVEL { get; set; }
        public Nullable<int> ID_UNIDADE { get; set; }
        public string UNIDADE { get; set; }
    }

    public class FarmaciaSateliteController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [Route("api/farmacia")]
        public IEnumerable<FARMACIA_SATELITEDTO> GetFARMACIAs()
        {
            return (from f in db.FARMACIA_SATELITE
                    join u in db.UNIDADEs on f.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    select new FARMACIA_SATELITEDTO()
                    {
                        ID_FARMACIA = f.ID_FARMACIA,
                        DESCRICAO = f.DESCRICAO,
                        LOCALIZACAO = f.LOCALIZACAO,
                        RAMAL = f.RAMAL,
                        RESPONSAVEL = f.RESPONSAVEL,
                        ID_UNIDADE = f.ID_UNIDADE,
                        UNIDADE = u.NOME_FANTASIA
                    }).ToList();
        }

        [Route("api/farmacia/{idFarmacia}")]
        public IEnumerable<FARMACIA_SATELITEDTO> GetFARMACIAs(int idFarmacia)
        {
            return (from f in db.FARMACIA_SATELITE
                    join u in db.UNIDADEs on f.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    select new FARMACIA_SATELITEDTO()
                    {
                        ID_FARMACIA = f.ID_FARMACIA,
                        DESCRICAO = f.DESCRICAO,
                        LOCALIZACAO = f.LOCALIZACAO,
                        RAMAL = f.RAMAL,
                        RESPONSAVEL = f.RESPONSAVEL,
                        ID_UNIDADE = f.ID_UNIDADE,
                        UNIDADE = u.NOME_FANTASIA
                    }).Where(f => f.ID_FARMACIA == idFarmacia).ToList();
        }

        [Route("api/unidade/{idUnidade}/farmacias")]
        public IEnumerable<FARMACIA_SATELITEDTO> GetUNIDADE_FARMACIAs(int idUnidade)
        {
            return (from f in db.FARMACIA_SATELITE
                    join u in db.UNIDADEs on f.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    select new FARMACIA_SATELITEDTO()
                    {
                        ID_FARMACIA = f.ID_FARMACIA,
                        DESCRICAO = f.DESCRICAO,
                        LOCALIZACAO = f.LOCALIZACAO,
                        RAMAL = f.RAMAL,
                        RESPONSAVEL = f.RESPONSAVEL,
                        ID_UNIDADE = f.ID_UNIDADE,
                        UNIDADE = u.NOME_FANTASIA
                    }).Where(f => f.ID_UNIDADE == idUnidade).ToList();
        }

        // PUT: api/FarmaciaSatelite/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFARMACIA_SATELITE(int id, FARMACIA_SATELITE fARMACIA_SATELITE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fARMACIA_SATELITE.ID_FARMACIA)
            {
                return BadRequest();
            }

            db.Entry(fARMACIA_SATELITE).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FARMACIA_SATELITEExists(id))
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

        // POST: api/FarmaciaSatelite
        [ResponseType(typeof(FARMACIA_SATELITE))]
        public IHttpActionResult PostFARMACIA_SATELITE(FARMACIA_SATELITE fARMACIA_SATELITE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_FARMACIA;").First();

            fARMACIA_SATELITE.ID_FARMACIA = NextValue;

            db.FARMACIA_SATELITE.Add(fARMACIA_SATELITE);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FARMACIA_SATELITEExists(fARMACIA_SATELITE.ID_FARMACIA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = fARMACIA_SATELITE.ID_FARMACIA }, fARMACIA_SATELITE);
        }

        // DELETE: api/FarmaciaSatelite/5
        [ResponseType(typeof(FARMACIA_SATELITE))]
        public IHttpActionResult DeleteFARMACIA_SATELITE(int id)
        {
            FARMACIA_SATELITE fARMACIA_SATELITE = db.FARMACIA_SATELITE.Find(id);
            if (fARMACIA_SATELITE == null)
            {
                return NotFound();
            }

            db.FARMACIA_SATELITE.Remove(fARMACIA_SATELITE);
            db.SaveChanges();

            return Ok(fARMACIA_SATELITE);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FARMACIA_SATELITEExists(int id)
        {
            return db.FARMACIA_SATELITE.Count(e => e.ID_FARMACIA == id) > 0;
        }
    }
}