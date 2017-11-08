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
    public class V_LOCALIDADESDTO
    {
        public string CEP { get; set; }
        public string RUA { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
        public string BAIRRO { get; set; }
        public Nullable<int> ID_CIDADE { get; set; }
    }
    public class VLocalidadesController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/conta/1
        [Route("api/localidade/{cep}")]
        public IEnumerable<V_LOCALIDADESDTO> GetCONTA(string cep)
        {
            return (from l in db.V_LOCALIDADES
                    select new V_LOCALIDADESDTO()
                    {
                        CEP = l.CEP,
                        RUA = l.RUA,
                        CIDADE = l.CIDADE,
                        UF = l.UF,
                        BAIRRO = l.BAIRRO,
                        ID_CIDADE = l.ID_CIDADE
                    }).Where(l => l.CEP == cep).ToList();
        }

        // PUT: api/VLocalidades/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutV_LOCALIDADES(string id, V_LOCALIDADES v_LOCALIDADES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != v_LOCALIDADES.CEP)
            {
                return BadRequest();
            }

            db.Entry(v_LOCALIDADES).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!V_LOCALIDADESExists(id))
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

        // POST: api/VLocalidades
        [ResponseType(typeof(V_LOCALIDADES))]
        public IHttpActionResult PostV_LOCALIDADES(V_LOCALIDADES v_LOCALIDADES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.V_LOCALIDADES.Add(v_LOCALIDADES);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (V_LOCALIDADESExists(v_LOCALIDADES.CEP))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = v_LOCALIDADES.CEP }, v_LOCALIDADES);
        }

        // DELETE: api/VLocalidades/5
        [ResponseType(typeof(V_LOCALIDADES))]
        public IHttpActionResult DeleteV_LOCALIDADES(string id)
        {
            V_LOCALIDADES v_LOCALIDADES = db.V_LOCALIDADES.Find(id);
            if (v_LOCALIDADES == null)
            {
                return NotFound();
            }

            db.V_LOCALIDADES.Remove(v_LOCALIDADES);
            db.SaveChanges();

            return Ok(v_LOCALIDADES);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool V_LOCALIDADESExists(string id)
        {
            return db.V_LOCALIDADES.Count(e => e.CEP == id) > 0;
        }
    }
}