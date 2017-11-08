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
    public class PROFISSIONAL_ENDERECODTO
    {
        public int ID_ENDERECO { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public string ENDERECO { get; set; }
        public string BAIRRO { get; set; }
        public string CEP { get; set; }
        public int ID_CIDADE { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
    }

    public class ProfissionalEnderecoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/profissional/1/endereco
        [Route("api/profissional/{idProfissional}/endereco")]
        public IEnumerable<PROFISSIONAL_ENDERECODTO> GetPROFISSIONAL_ENDERECO(int idProfissional)
        {
            return (from ce in db.PROFISSIONAL_ENDERECO
                    join ci in db.CIDADEs on ce.ID_CIDADE equals ci.ID_CIDADE
                    select new PROFISSIONAL_ENDERECODTO()
                    {
                        ID_ENDERECO = ce.ID_ENDERECO,
                        ID_PROFISSIONAL = ce.ID_PROFISSIONAL,
                        BAIRRO = ce.BAIRRO,
                        CEP = ce.CEP,
                        ENDERECO = ce.ENDERECO,
                        ID_CIDADE = ce.ID_CIDADE,
                        CIDADE = ci.NOME,
                        UF = ci.UF
                    }).Where(ce => ce.ID_PROFISSIONAL == idProfissional).ToList();

            //    return db.CLIENTE_ENDERECO.Where(c => c.ID_CLIENTE == idCliente);
        }

        // GET api/cliente/endereco/5
        [Route("api/profissional/endereco/{id}")]
        [ResponseType(typeof(PROFISSIONAL_ENDERECO))]
        public IHttpActionResult GetENDERECO(int id)
        {
            PROFISSIONAL_ENDERECO pROFISSIONAL_ENDERECO = db.PROFISSIONAL_ENDERECO.Find(id);
            if (pROFISSIONAL_ENDERECO == null)
            {
                return NotFound();
            }

            return Ok(pROFISSIONAL_ENDERECO);
        }


        // PUT: api/ProfissionalEndereco/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPROFISSIONAL_ENDERECO(int id, PROFISSIONAL_ENDERECO pROFISSIONAL_ENDERECO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pROFISSIONAL_ENDERECO.ID_ENDERECO)
            {
                return BadRequest();
            }

            db.Entry(pROFISSIONAL_ENDERECO).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PROFISSIONAL_ENDERECOExists(id))
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

        // POST: api/ProfissionalEndereco
        [ResponseType(typeof(PROFISSIONAL_ENDERECO))]
        public IHttpActionResult PostPROFISSIONAL_ENDERECO(PROFISSIONAL_ENDERECO pROFISSIONAL_ENDERECO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ENDERECO_PROF;").First();

            pROFISSIONAL_ENDERECO.ID_ENDERECO = NextValue;

            db.PROFISSIONAL_ENDERECO.Add(pROFISSIONAL_ENDERECO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PROFISSIONAL_ENDERECOExists(pROFISSIONAL_ENDERECO.ID_ENDERECO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROFISSIONAL_ENDERECO.ID_ENDERECO }, pROFISSIONAL_ENDERECO);
        }

        // DELETE: api/ProfissionalEndereco/5
        [ResponseType(typeof(PROFISSIONAL_ENDERECO))]
        public IHttpActionResult DeletePROFISSIONAL_ENDERECO(int id)
        {
            PROFISSIONAL_ENDERECO pROFISSIONAL_ENDERECO = db.PROFISSIONAL_ENDERECO.Find(id);
            if (pROFISSIONAL_ENDERECO == null)
            {
                return NotFound();
            }

            db.PROFISSIONAL_ENDERECO.Remove(pROFISSIONAL_ENDERECO);
            db.SaveChanges();

            return Ok(pROFISSIONAL_ENDERECO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PROFISSIONAL_ENDERECOExists(int id)
        {
            return db.PROFISSIONAL_ENDERECO.Count(e => e.ID_ENDERECO == id) > 0;
        }
    }
}