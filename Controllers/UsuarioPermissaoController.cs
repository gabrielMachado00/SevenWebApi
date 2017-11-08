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
    public class USUARIO_PERMISSAODTO
    {
        public int ID_PERMISSAO { get; set; }
        public int ID_USUARIO { get; set; }
        public string DESCRICAO { get; set; }
        public bool PERMITE { get; set; }
        public string LOGIN { get; set; }
    }
    public class UsuarioPermissaoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/Usuario/1/permissao
        [Route("api/usuario/{login}/permissao")]
        public IEnumerable<USUARIO_PERMISSAODTO> GetUSUARIO_PERMISSAO(string login)
        {
            return (from up in db.USUARIO_PERMISSAO
                    join u in db.USUARIOs on up.ID_USUARIO equals u.ID_USUARIO
                    select new USUARIO_PERMISSAODTO()
                    {
                        ID_PERMISSAO = up.ID_PERMISSAO,
                        ID_USUARIO = up.ID_USUARIO,
                        DESCRICAO = up.DESCRICAO,
                        PERMITE = up.PERMITE,
                        LOGIN = u.LOGIN
                    }).Where(u => u.LOGIN == login).ToList();
        }

        // GET api/Usuario/1/permissao
        [Route("api/usuario/{login}/permissao/{idPermissao}")]
        public IEnumerable<USUARIO_PERMISSAODTO> GetUSUARIO_PERMISSAO(string login, int idPermissao)
        {
            return (from up in db.USUARIO_PERMISSAO
                    join u in db.USUARIOs on up.ID_USUARIO equals u.ID_USUARIO
                    select new USUARIO_PERMISSAODTO()
                    {
                        ID_PERMISSAO = up.ID_PERMISSAO,
                        ID_USUARIO = up.ID_USUARIO,
                        DESCRICAO = up.DESCRICAO,
                        PERMITE = up.PERMITE,
                        LOGIN = u.LOGIN
                    }).Where(u => u.LOGIN == login && u.ID_PERMISSAO == idPermissao).ToList();
        }

        // PUT: api/UsuarioPermissao/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUSUARIO_PERMISSAO(int id, USUARIO_PERMISSAO uSUARIO_PERMISSAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uSUARIO_PERMISSAO.ID_PERMISSAO)
            {
                return BadRequest();
            }

            db.Entry(uSUARIO_PERMISSAO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!USUARIO_PERMISSAOExists(id))
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

        // POST: api/UsuarioPermissao
        [ResponseType(typeof(USUARIO_PERMISSAO))]
        public IHttpActionResult PostUSUARIO_PERMISSAO(USUARIO_PERMISSAO uSUARIO_PERMISSAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_PERMISSAO;").First();

            uSUARIO_PERMISSAO.ID_PERMISSAO = NextValue;

            db.USUARIO_PERMISSAO.Add(uSUARIO_PERMISSAO);

            try
            { 
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (USUARIO_PERMISSAOExists(uSUARIO_PERMISSAO.ID_PERMISSAO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = uSUARIO_PERMISSAO.ID_PERMISSAO }, uSUARIO_PERMISSAO);
        }

        // DELETE: api/UsuarioPermissao/5
        [ResponseType(typeof(USUARIO_PERMISSAO))]
        public IHttpActionResult DeleteUSUARIO_PERMISSAO(int id)
        {
            USUARIO_PERMISSAO uSUARIO_PERMISSAO = db.USUARIO_PERMISSAO.Find(id);
            if (uSUARIO_PERMISSAO == null)
            {
                return NotFound();
            }

            db.USUARIO_PERMISSAO.Remove(uSUARIO_PERMISSAO);
            db.SaveChanges();

            return Ok(uSUARIO_PERMISSAO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool USUARIO_PERMISSAOExists(int id)
        {
            return db.USUARIO_PERMISSAO.Count(e => e.ID_PERMISSAO == id) > 0;
        }
    }
}