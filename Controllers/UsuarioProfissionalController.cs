using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;
using System.Text;
using System.Security.Cryptography;
using System.IO;


namespace SisMedApi.Controllers
{
    public class USUARIO_PROFISSIONALDTO
    {
        public int ID_USUARIO_PROFISSIONAL { get; set; }
        public int ID_USUARIO { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public DateTime DATA_INCLUSAO { get; set; }
        public string PROFISSIONAL { get; set; }
        public string LOGIN_USUARIO { get; set; }
    }

    public class UsuarioProfissionalController : ApiController
    {
        private SisMedContext db = new SisMedContext();


        // GET api/Usuario/Login/teste
        [Route("api/UsuarioProfissional", Name = "GetUsuarioProfissional")]
        public IEnumerable<USUARIO_PROFISSIONALDTO> GetUSUARIO_PROFISSIONAL()
        {
            return (from up in db.USUARIO_PROFISSIONAL
                    join u in db.USUARIOs on up.ID_USUARIO equals u.ID_USUARIO
                    join p in db.PROFISSIONALs on up.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new USUARIO_PROFISSIONALDTO()
                    {
                        ID_USUARIO_PROFISSIONAL = up.ID_USUARIO_PROFISSIONAL,
                        ID_USUARIO = u.ID_USUARIO,
                        ID_PROFISSIONAL = p.ID_PROFISSIONAL,
                        DATA_INCLUSAO = up.DATA_INCLUSAO,
                        LOGIN_USUARIO = u.LOGIN,
                        PROFISSIONAL = p.NOME
                    }).ToList();
        }

        [Route("api/UsuarioProfissional/{idUsuarioProfissional}", Name = "GetUsuarioProfissionalById")]
        public IEnumerable<USUARIO_PROFISSIONALDTO> GetUSUARIO_PROFISSIONAL(int idUsuarioProfissional)
        {
            return (from up in db.USUARIO_PROFISSIONAL
                    join u in db.USUARIOs on up.ID_USUARIO equals u.ID_USUARIO
                    join p in db.PROFISSIONALs on up.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new USUARIO_PROFISSIONALDTO()
                    {
                        ID_USUARIO_PROFISSIONAL = up.ID_USUARIO_PROFISSIONAL,
                        ID_USUARIO = u.ID_USUARIO,
                        ID_PROFISSIONAL = p.ID_PROFISSIONAL,
                        DATA_INCLUSAO = up.DATA_INCLUSAO,
                        LOGIN_USUARIO = u.LOGIN,
                        PROFISSIONAL = p.NOME
                    }).Where(u => u.ID_USUARIO_PROFISSIONAL == idUsuarioProfissional).ToList();
        }

        // POST: api/Usuario
        [ResponseType(typeof(USUARIO_PROFISSIONAL))]
        [Route("api/usuarioprofissional", Name = "PostUsuarioProfissional")]
        public IHttpActionResult PostUSUARIO_PROFISSIONAL(USUARIO_PROFISSIONAL uSUARIO_PROFISSIONAL)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_USUARIO_PROFISSIONAL;").First();

            uSUARIO_PROFISSIONAL.ID_USUARIO_PROFISSIONAL = NextValue;                        

            db.USUARIO_PROFISSIONAL.Add(uSUARIO_PROFISSIONAL);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (USUARIOProfissionalExists(uSUARIO_PROFISSIONAL.ID_USUARIO, uSUARIO_PROFISSIONAL.ID_PROFISSIONAL))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetUsuarioProfissionalById", new { idUsuarioProfissional = uSUARIO_PROFISSIONAL.ID_USUARIO_PROFISSIONAL }, uSUARIO_PROFISSIONAL);
        }

        // DELETE: api/Usuario/5
        [ResponseType(typeof(USUARIO_PROFISSIONAL))]
        public async Task<IHttpActionResult> DeleteUSUARIO_PROFISSIONAL(int id)
        {
            USUARIO_PROFISSIONAL uSUARIO = await db.USUARIO_PROFISSIONAL.FindAsync(id);
            if (uSUARIO == null)
            {
                return NotFound();
            }

            db.USUARIO_PROFISSIONAL.Remove(uSUARIO);
            await db.SaveChangesAsync();

            return Ok(uSUARIO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool USUARIOProfissionalExists(int idUsuario, int idProfissional)
        {
            return db.USUARIO_PROFISSIONAL.Count(e => e.ID_USUARIO == idUsuario && e.ID_PROFISSIONAL == idProfissional) > 0;
        }

    }
}
