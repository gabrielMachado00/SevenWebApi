using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SevenMedicalApi.Models;
using SisMedApi.Models;
using System;
using System.Collections.Generic;

namespace SevenMedicalApi.Controllers
{
    public class UNIDADE_SOCIODTO
    {
        public int ID_SOCIO { get; set; }
        public int ID_UNIDADE { get; set; }
        public string NOME { get; set; }
        public Nullable<decimal> COTA { get; set; }
        public string CELULAR { get; set; }
        public string TELEFONE { get; set; }
        public string EMAIL { get; set; }
        public string UNIDADE { get; set; }
    }
    public class UnidadeSocioController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [Route("api/unidade/{idUnidade}/socio")]
        public IEnumerable<UNIDADE_SOCIODTO> GetSOCIOSs(int idUnidade)
        {
            return (from s in db.UNIDADE_SOCIO
                    join u in db.UNIDADEs on s.ID_UNIDADE equals u.ID_UNIDADE
                    select new UNIDADE_SOCIODTO()
                    {
                        ID_SOCIO = s.ID_SOCIO,
                        ID_UNIDADE = s.ID_UNIDADE,
                        NOME = s.NOME,
                        COTA = s.COTA,
                        CELULAR = s.CELULAR,
                        TELEFONE = s.TELEFONE,
                        EMAIL = s.EMAIL,
                        UNIDADE = u.NOME_FANTASIA
                    }).Where(s => s.ID_UNIDADE == idUnidade).ToList();
        }

        [Route("api/unidade/{idUnidade}/socio/{idSocio}")]
        public IEnumerable<UNIDADE_SOCIODTO> GetSOCIOSs(int idUnidade, int idSocio)
        {
            return (from s in db.UNIDADE_SOCIO
                    join u in db.UNIDADEs on s.ID_UNIDADE equals u.ID_UNIDADE
                    select new UNIDADE_SOCIODTO()
                    {
                        ID_SOCIO = s.ID_SOCIO,
                        ID_UNIDADE = s.ID_UNIDADE,
                        NOME = s.NOME,
                        COTA = s.COTA,
                        CELULAR = s.CELULAR,
                        TELEFONE = s.TELEFONE,
                        EMAIL = s.EMAIL,
                        UNIDADE = u.NOME_FANTASIA
                    }).Where(s => s.ID_UNIDADE == idUnidade && s.ID_SOCIO == idSocio).ToList();
        }

        // PUT: api/UnidadeSocio/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUNIDADE_SOCIO(int id, UNIDADE_SOCIO uNIDADE_SOCIO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uNIDADE_SOCIO.ID_SOCIO)
            {
                return BadRequest();
            }

            db.Entry(uNIDADE_SOCIO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UNIDADE_SOCIOExists(id))
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

        // POST: api/UnidadeSocio
        [ResponseType(typeof(UNIDADE_SOCIO))]
        public IHttpActionResult PostUNIDADE_SOCIO(UNIDADE_SOCIO uNIDADE_SOCIO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_SOCIO;").First();

            uNIDADE_SOCIO.ID_SOCIO = NextValue;

            db.UNIDADE_SOCIO.Add(uNIDADE_SOCIO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UNIDADE_SOCIOExists(uNIDADE_SOCIO.ID_SOCIO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = uNIDADE_SOCIO.ID_SOCIO }, uNIDADE_SOCIO);
        }

        // DELETE: api/UnidadeSocio/5
        [ResponseType(typeof(UNIDADE_SOCIO))]
        public IHttpActionResult DeleteUNIDADE_SOCIO(int id)
        {
            UNIDADE_SOCIO uNIDADE_SOCIO = db.UNIDADE_SOCIO.Find(id);
            if (uNIDADE_SOCIO == null)
            {
                return NotFound();
            }

            db.UNIDADE_SOCIO.Remove(uNIDADE_SOCIO);
            db.SaveChanges();

            return Ok(uNIDADE_SOCIO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UNIDADE_SOCIOExists(int id)
        {
            return db.UNIDADE_SOCIO.Count(e => e.ID_SOCIO == id) > 0;
        }
    }
}