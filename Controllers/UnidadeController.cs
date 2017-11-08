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
    public class UNIDADEDTO
    {
        public int ID_UNIDADE { get; set; }
        public string NOME_FANTASIA { get; set; }
        public string CNPJ { get; set; }
        public string RAZAO_SOCIAL { get; set; }
        public string TELEFONE { get; set; }
        public string EMAIL { get; set; }
        public string CONTATO { get; set; }
        public bool EXIBE_AGENDA { get; set; }
        public string CPF { get; set; }
    }

    public class UnidadeController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/Unidade
        public IQueryable<UNIDADE> GetUNIDADEs()
        {
            return db.UNIDADEs;
        }

        // GET: api/Unidade/5
        [ResponseType(typeof(UNIDADE))]
        public IHttpActionResult GetUNIDADE(int id)
        {
            UNIDADE uNIDADE = db.UNIDADEs.Find(id);
            if (uNIDADE == null)
            {
                return NotFound();
            }

            return Ok(uNIDADE);
        }

        // GET api/unidades
        [Route("api/unidade")]
        public IEnumerable<UNIDADEDTO> GetUNIDADE()
        {
            return (from u in db.UNIDADEs
                    select new UNIDADEDTO()
                    {
                        ID_UNIDADE = u.ID_UNIDADE,
                        NOME_FANTASIA = u.NOME_FANTASIA,
                        CNPJ = u.CNPJ,
                        RAZAO_SOCIAL = u.RAZAO_SOCIAL,
                        TELEFONE = u.TELEFONE,
                        EMAIL = u.EMAIL,
                        CONTATO = u.CONTATO,
                        EXIBE_AGENDA = u.EXIBE_AGENDA,
                        CPF = u.CPF
                    }).Where(u => u.EXIBE_AGENDA == true).ToList();
        }

        // PUT: api/Unidade/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUNIDADE(int id, UNIDADE uNIDADE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uNIDADE.ID_UNIDADE)
            {
                return BadRequest();
            }

            db.Entry(uNIDADE).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UNIDADEExists(id))
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

        // POST: api/Unidade
        [ResponseType(typeof(UNIDADE))]
        public IHttpActionResult PostUNIDADE(UNIDADE uNIDADE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_UNIDADE;").First();

            uNIDADE.ID_UNIDADE = NextValue;

            db.UNIDADEs.Add(uNIDADE);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UNIDADEExists(uNIDADE.ID_UNIDADE))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = uNIDADE.ID_UNIDADE }, uNIDADE);
        }

        // DELETE: api/Unidade/5
        [ResponseType(typeof(UNIDADE))]
        public IHttpActionResult DeleteUNIDADE(int id)
        {
            UNIDADE uNIDADE = db.UNIDADEs.Find(id);
            if (uNIDADE == null)
            {
                return NotFound();
            }

            db.UNIDADEs.Remove(uNIDADE);
            db.SaveChanges();

            return Ok(uNIDADE);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UNIDADEExists(int id)
        {
            return db.UNIDADEs.Count(e => e.ID_UNIDADE == id) > 0;
        }
    }
}