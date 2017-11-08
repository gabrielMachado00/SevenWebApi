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
    public class PERFILDTO
    {
        public int ID_PERFIL { get; set; }
        public string DESCRICAO { get; set; }
        public int ORDEM_HIERARQUICA { get; set; }
    }
    public class PerfisController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [Route("api/perfil", Name = "GetPerfis")]
        public IEnumerable<PERFILDTO> GetPERFIL()
        {
            return (from p in db.PERFILs
                    select new PERFILDTO()
                    {
                        ID_PERFIL = p.ID_PERFIL,
                        DESCRICAO = p.DESCRICAO,
                        ORDEM_HIERARQUICA = p.ORDEM_HIERARQUICA
                    }).OrderBy(p => p.ORDEM_HIERARQUICA).ToList();
        }

        [Route("api/perfil/{idPerfil}", Name = "GetPerfilById")]
        public IEnumerable<PERFILDTO> GetPERFIL(int idPerfil)
        {
            return (from p in db.PERFILs
                    select new PERFILDTO()
                    {
                        ID_PERFIL = p.ID_PERFIL,
                        DESCRICAO = p.DESCRICAO,
                        ORDEM_HIERARQUICA = p.ORDEM_HIERARQUICA
                    }).Where(p => p.ID_PERFIL == idPerfil).OrderBy(p => p.ORDEM_HIERARQUICA).ToList();
        }

        [Route("api/perfil/hierarquia")]
        public IEnumerable<PERFILDTO> GetPERFIL_HIERARQUIA()
        {
            return (from p in db.PERFILs
                    select new PERFILDTO()
                    {
                        ID_PERFIL = p.ID_PERFIL,
                        DESCRICAO = p.DESCRICAO,
                        ORDEM_HIERARQUICA = p.ORDEM_HIERARQUICA
                    }).OrderBy(p => p.ORDEM_HIERARQUICA).ToList();
        }

        [Route("api/perfil/hierarquia/{OrdemHierarquica}")]
        public IEnumerable<PERFILDTO> GetPERFIL_HIERARQUIA(int OrdemHierarquica)
        {
            return (from p in db.PERFILs
                    select new PERFILDTO()
                    {
                        ID_PERFIL = p.ID_PERFIL,
                        DESCRICAO = p.DESCRICAO,
                        ORDEM_HIERARQUICA = p.ORDEM_HIERARQUICA
                    }).Where(p => p.ORDEM_HIERARQUICA > OrdemHierarquica).OrderBy(p => p.ORDEM_HIERARQUICA).ToList();
        }

        // PUT: api/Perfis/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPERFIL(int id, PERFIL pERFIL)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pERFIL.ID_PERFIL)
            {
                return BadRequest();
            }

            db.Entry(pERFIL).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PERFILExists(id))
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

        // POST: api/Perfis
        [ResponseType(typeof(PERFIL))]
        [Route("api/perfil", Name = "PostPerfil")]
        public IHttpActionResult PostPERFIL(PERFIL pERFIL)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_PERFIL;").First();

            pERFIL.ID_PERFIL = NextValue;

            db.PERFILs.Add(pERFIL);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PERFILExists(pERFIL.ID_PERFIL))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtRoute("GetPerfilById", new { idPerfil = pERFIL.ID_PERFIL }, pERFIL);
        }

        // DELETE: api/Perfis/5
        [ResponseType(typeof(PERFIL))]
        public IHttpActionResult DeletePERFIL(int id)
        {
            PERFIL pERFIL = db.PERFILs.Find(id);
            if (pERFIL == null)
            {
                return NotFound();
            }

            db.PERFILs.Remove(pERFIL);
            db.SaveChanges();

            return Ok(pERFIL);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PERFILExists(int id)
        {
            return db.PERFILs.Count(e => e.ID_PERFIL == id) > 0;
        }
    }
}