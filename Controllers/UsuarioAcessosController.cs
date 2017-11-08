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
    public class USUARIO_ACESSODTO
    {
        public int ID_USUARIO { get; set; }
        public int ID_ACESSO { get; set; }
        public string USUARIO { get; set; }
        public string ACESSO { get; set; }
        public int COD_ACESSO { get; set; }
    }
    public class UsuarioAcessosController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [Route("api/usuarioacesso")]
        public IEnumerable<USUARIO_ACESSODTO> GetUSUARIO_ACESSO()
        {
            return (from ua in db.USUARIO_ACESSO
                    join u in db.USUARIOs on ua.ID_USUARIO equals u.ID_USUARIO
                    join a in db.ACESSOes on ua.ID_ACESSO equals a.ID_ACESSO
                    select new USUARIO_ACESSODTO()
                    {
                        ID_USUARIO = ua.ID_USUARIO,
                        ID_ACESSO = ua.ID_ACESSO,
                        USUARIO = u.LOGIN,
                        ACESSO = a.DESCRICAO,
                        COD_ACESSO = a.CODIGO_ACESSO
                    }).ToList();
        }

        [Route("api/usuarioacesso/{idUsuario}")]
        public IEnumerable<USUARIO_ACESSODTO> GetUSUARIO_ACESSO(int idUsuario)
        {
            return (from ua in db.USUARIO_ACESSO
                    join u in db.USUARIOs on ua.ID_USUARIO equals u.ID_USUARIO
                    join a in db.ACESSOes on ua.ID_ACESSO equals a.ID_ACESSO
                    select new USUARIO_ACESSODTO()
                    {
                        ID_USUARIO = ua.ID_USUARIO,
                        ID_ACESSO = ua.ID_ACESSO,
                        USUARIO = u.LOGIN,
                        ACESSO = a.DESCRICAO,
                        COD_ACESSO = a.CODIGO_ACESSO
                    }).Where(ua => ua.ID_USUARIO == idUsuario).ToList();
        }

        // PUT: api/UsuarioAcessos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUSUARIO_ACESSO(int id, USUARIO_ACESSO uSUARIO_ACESSO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uSUARIO_ACESSO.ID_USUARIO)
            {
                return BadRequest();
            }

            db.Entry(uSUARIO_ACESSO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!USUARIO_ACESSOExists(id))
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

        // POST: api/UsuarioAcessos
        [ResponseType(typeof(USUARIO_ACESSO))]
        public IHttpActionResult PostUSUARIO_ACESSO(USUARIO_ACESSO uSUARIO_ACESSO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.USUARIO_ACESSO.Add(uSUARIO_ACESSO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (USUARIO_ACESSOExists(uSUARIO_ACESSO.ID_USUARIO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = uSUARIO_ACESSO.ID_USUARIO }, uSUARIO_ACESSO);
        }

        // DELETE: api/UsuarioAcesso/5
        [ResponseType(typeof(USUARIO_ACESSO))]
        [Route("api/usuarioacesso/Delete/{idUsuario}/{idAcesso}")]
        public IHttpActionResult DeleteUSUARIO_ACESSO(int idUsuario, int idAcesso)
        {
            USUARIO_ACESSO uSUARIO_ACESSO = db.USUARIO_ACESSO.Where(ua => ua.ID_USUARIO == idUsuario && ua.ID_ACESSO == idAcesso).FirstOrDefault();
            if (uSUARIO_ACESSO == null)
            {
                return NotFound();
            }

            var sql = @" DELETE FROM USUARIO_ACESSO WHERE ID_USUARIO = {0} AND ID_ACESSO = {1} ";

            if (db.Database.ExecuteSqlCommand(sql, idUsuario, idAcesso) > 0)
                return Ok(uSUARIO_ACESSO);
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

        private bool USUARIO_ACESSOExists(int id)
        {
            return db.USUARIO_ACESSO.Count(e => e.ID_USUARIO == id) > 0;
        }
    }
}