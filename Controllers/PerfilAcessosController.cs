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
    public class PERFIL_ACESSODTO
    {
        public int ID_PERFIL { get; set; }
        public int ID_ACESSO { get; set; }
        public string PERFIL { get; set; }
        public string ACESSO { get; set; }
        public int COD_ACESSO { get; set; }
    }
    public class PerfilAcessosController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [Route("api/perfilacesso")]
        public IEnumerable<PERFIL_ACESSODTO> GetPERFIL_ACESSO()
        {
            return (from pa in db.PERFIL_ACESSO
                    join p in db.PERFILs on pa.ID_PERFIL equals p.ID_PERFIL
                    join a in db.ACESSOes on pa.ID_ACESSO equals a.ID_ACESSO
                    select new PERFIL_ACESSODTO()
                    {
                        ID_PERFIL = pa.ID_PERFIL,
                        ID_ACESSO = pa.ID_ACESSO,
                        PERFIL = p.DESCRICAO,
                        ACESSO = a.DESCRICAO,
                        COD_ACESSO = a.CODIGO_ACESSO
                    }).ToList();
        }

        [Route("api/perfilacesso/{idPerfil}")]
        public IEnumerable<PERFIL_ACESSODTO> GetPERFIL_ACESSO(int idPerfil)
        {
            return (from pa in db.PERFIL_ACESSO
                    join p in db.PERFILs on pa.ID_PERFIL equals p.ID_PERFIL
                    join a in db.ACESSOes on pa.ID_ACESSO equals a.ID_ACESSO
                    select new PERFIL_ACESSODTO()
                    {
                        ID_PERFIL = pa.ID_PERFIL,
                        ID_ACESSO = pa.ID_ACESSO,
                        PERFIL = p.DESCRICAO,
                        ACESSO = a.DESCRICAO,
                        COD_ACESSO = a.CODIGO_ACESSO
                    }).Where(pa => pa.ID_PERFIL == idPerfil).ToList();
        }

        // PUT: api/PerfilAcessos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPERFIL_ACESSO(int id, PERFIL_ACESSO pERFIL_ACESSO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pERFIL_ACESSO.ID_PERFIL)
            {
                return BadRequest();
            }

            db.Entry(pERFIL_ACESSO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PERFIL_ACESSOExists(id))
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

        // POST: api/PerfilAcessos
        [ResponseType(typeof(PERFIL_ACESSO))]
        public IHttpActionResult PostPERFIL_ACESSO(PERFIL_ACESSO pERFIL_ACESSO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PERFIL_ACESSO.Add(pERFIL_ACESSO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PERFIL_ACESSOExists(pERFIL_ACESSO.ID_PERFIL))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pERFIL_ACESSO.ID_PERFIL }, pERFIL_ACESSO);
        }

        [ResponseType(typeof(PERFIL_ACESSO))]
        [Route("api/perfilacesso/Delete/{idPerfil}/{idAcesso}")]
        public IHttpActionResult DeletePERFIL_ACESSO(int idPerfil, int idAcesso)
        {
            PERFIL_ACESSO pERFIL_ACESSO = db.PERFIL_ACESSO.Where(pa => pa.ID_PERFIL == idPerfil && pa.ID_ACESSO == idAcesso).FirstOrDefault();
            if (pERFIL_ACESSO == null)
            {
                return NotFound();
            }

            var sql = @" DELETE FROM PERFIL_ACESSO WHERE ID_PERFIL = {0} AND ID_ACESSO = {1} ";

            if (db.Database.ExecuteSqlCommand(sql, idPerfil, idAcesso) > 0)
                return Ok(pERFIL_ACESSO);
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

        private bool PERFIL_ACESSOExists(int id)
        {
            return db.PERFIL_ACESSO.Count(e => e.ID_PERFIL == id) > 0;
        }
    }
}