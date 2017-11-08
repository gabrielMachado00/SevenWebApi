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
    public class USUARIODTO
    {
        public int ID_USUARIO { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }
        public Nullable<int> ID_PROFISSIONAL { get; set; }
        public int TIPO_ACESSO { get; set; }
        public bool MASTER { get; set; }
        public string DESC_TIPO_ACESSO { get; set; }
        public Nullable<int> ID_PERFIL { get; set; }
        public string PERFIL { get; set; }
        public Nullable<int> ORDEM_HIERARQUICA { get; set; }
        public Nullable<decimal> PERCENTUAL_COMISSAO { get; set; }
        public string EMAIL_SENHA { get; set; }
        public string STATUS { get; set; }
    }

    public class PERFILACESSODTO
    {
        public int ID_PERFIL { get; set; }
        public int ID_ACESSO { get; set; }
        public string PERFIL { get; set; }
        public string ACESSO { get; set; }
        public int COD_ACESSO { get; set; }
    }

    public class USUARIOACESSODTO
    {
        public int ID_USUARIO { get; set; }
        public int ID_ACESSO { get; set; }
        public string USUARIO { get; set; }
        public string ACESSO { get; set; }
        public int COD_ACESSO { get; set; }
    }

    public class UsuarioController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/Usuario/Login/teste
        [Route("api/usuario", Name = "GetUsuarios")]
        public IEnumerable<USUARIODTO> GetUSUARIO_LOGIN()
        {
            return (from u in db.USUARIOs
                    join p in db.PERFILs on u.ID_PERFIL equals p.ID_PERFIL into _p
                    from p in _p.DefaultIfEmpty()
                    select new USUARIODTO()
                    {
                        ID_USUARIO = u.ID_USUARIO,
                        ID_PROFISSIONAL = u.ID_PROFISSIONAL,
                        LOGIN = u.LOGIN,
                        SENHA = u.SENHA,
                        TIPO_ACESSO = u.TIPO_ACESSO,
                        MASTER = u.MASTER,
                        DESC_TIPO_ACESSO = u.TIPO_ACESSO == 0 ? "GESTÃO" :
                                                u.TIPO_ACESSO == 1 ? "CLÍNICO" :
                                                u.TIPO_ACESSO == 2 ? "GESTÃO E CLÍNICO" : "",
                        ID_PERFIL = u.ID_PERFIL,
                        PERFIL = p.DESCRICAO,
                        ORDEM_HIERARQUICA = p.ORDEM_HIERARQUICA,
                        PERCENTUAL_COMISSAO = u.PERCENTUAL_COMISSAO,
                        EMAIL_SENHA = u.EMAIL_SENHA,
                        STATUS = u.STATUS
                    }).ToList();
        }

        // GET api/Usuario/Login/teste
        [Route("api/usuario/{idUsuario}", Name = "GetUsuarioById")]
        public IEnumerable<USUARIODTO> GetUSUARIO_LOGIN(int idUsuario)
        {
            return (from u in db.USUARIOs
                    join p in db.PERFILs on u.ID_PERFIL equals p.ID_PERFIL into _p
                    from p in _p.DefaultIfEmpty()
                    select new USUARIODTO()
                    {
                        ID_USUARIO = u.ID_USUARIO,
                        ID_PROFISSIONAL = u.ID_PROFISSIONAL,
                        LOGIN = u.LOGIN,
                        SENHA = u.SENHA,
                        TIPO_ACESSO = u.TIPO_ACESSO,
                        MASTER = u.MASTER,
                        DESC_TIPO_ACESSO = u.TIPO_ACESSO == 0 ? "GESTÃO" :
                                                u.TIPO_ACESSO == 1 ? "CLÍNICO" :
                                                u.TIPO_ACESSO == 2 ? "GESTÃO E CLÍNICO" : "",
                        ID_PERFIL = u.ID_PERFIL,
                        PERFIL = p.DESCRICAO,
                        ORDEM_HIERARQUICA = p.ORDEM_HIERARQUICA,
                        PERCENTUAL_COMISSAO = u.PERCENTUAL_COMISSAO,
                        EMAIL_SENHA = u.EMAIL_SENHA,
                        STATUS = u.STATUS
                    }).Where(u => u.ID_USUARIO == idUsuario).ToList();
        }

        // GET api/Usuario/Login/teste
        [Route("api/usuario/Login/{login}")]
        public IEnumerable<USUARIODTO> GetUSUARIO_LOGIN(string login)
        {
            return (from u in db.USUARIOs
                    join p in db.PERFILs on u.ID_PERFIL equals p.ID_PERFIL into _p
                    from p in _p.DefaultIfEmpty()
                    select new USUARIODTO()
                    {
                        ID_USUARIO = u.ID_USUARIO,
                        ID_PROFISSIONAL = u.ID_PROFISSIONAL,
                        LOGIN = u.LOGIN,
                        SENHA = u.SENHA,
                        TIPO_ACESSO = u.TIPO_ACESSO,
                        MASTER = u.MASTER,
                        DESC_TIPO_ACESSO = u.TIPO_ACESSO == 0 ? "GESTÃO" :
                                                u.TIPO_ACESSO == 1 ? "CLÍNICO" :
                                                u.TIPO_ACESSO == 2 ? "GESTÃO E CLÍNICO" : "",
                        ID_PERFIL = u.ID_PERFIL,
                        PERFIL = p.DESCRICAO,
                        ORDEM_HIERARQUICA = p.ORDEM_HIERARQUICA,
                        PERCENTUAL_COMISSAO = u.PERCENTUAL_COMISSAO,
                        EMAIL_SENHA = u.EMAIL_SENHA,
                        STATUS = u.STATUS
                    }).Where(u => u.LOGIN == login).ToList();
        }

        [Route("api/usuario/{login}/{senha}/{tipoAcesso}")]
        public IEnumerable<USUARIODTO> GetUSUARIOs(string login, string senha, int tipoAcesso)
        {
            List<USUARIODTO> model;
            if (tipoAcesso < 4)
            {
                model = (from u in db.USUARIOs
                         join p in db.PERFILs on u.ID_PERFIL equals p.ID_PERFIL into _p
                         from p in _p.DefaultIfEmpty()
                         select new USUARIODTO()
                         {
                             ID_USUARIO = u.ID_USUARIO,
                             LOGIN = u.LOGIN,
                             SENHA = u.SENHA,
                             TIPO_ACESSO = u.TIPO_ACESSO,
                             DESC_TIPO_ACESSO = u.TIPO_ACESSO == 0 ? "GESTÃO" :
                                                u.TIPO_ACESSO == 1 ? "CLÍNICO" :
                                                u.TIPO_ACESSO == 2 ? "GESTÃO E CLÍNICO" : "",
                             MASTER = u.MASTER,
                             ID_PROFISSIONAL = u.ID_PROFISSIONAL,
                             ID_PERFIL = u.ID_PERFIL,
                             PERFIL = p.DESCRICAO,
                             ORDEM_HIERARQUICA = p.ORDEM_HIERARQUICA,
                             PERCENTUAL_COMISSAO = u.PERCENTUAL_COMISSAO,
                             EMAIL_SENHA = u.EMAIL_SENHA,
                             STATUS =  u.STATUS
                         }).Where(u => u.LOGIN == login.ToUpper() && u.TIPO_ACESSO == tipoAcesso && u.STATUS == "A").ToList();
            }
            else
            {
                model = (from u in db.USUARIOs
                         join p in db.PERFILs on u.ID_PERFIL equals p.ID_PERFIL into _p
                         from p in _p.DefaultIfEmpty()
                         select new USUARIODTO()
                         {
                             ID_USUARIO = u.ID_USUARIO,
                             LOGIN = u.LOGIN,
                             SENHA = u.SENHA,
                             TIPO_ACESSO = u.TIPO_ACESSO,
                             DESC_TIPO_ACESSO = u.TIPO_ACESSO == 0 ? "GESTÃO" :
                                                u.TIPO_ACESSO == 1 ? "CLÍNICO" :
                                                u.TIPO_ACESSO == 2 ? "GESTÃO E CLÍNICO" : "",
                             MASTER = u.MASTER,
                             ID_PROFISSIONAL = u.ID_PROFISSIONAL,
                             ID_PERFIL = u.ID_PERFIL,
                             PERFIL = p.DESCRICAO,
                             ORDEM_HIERARQUICA = p.ORDEM_HIERARQUICA,
                             PERCENTUAL_COMISSAO = u.PERCENTUAL_COMISSAO,
                             EMAIL_SENHA = u.EMAIL_SENHA,
                             STATUS = u.STATUS
                         }).Where(u => u.LOGIN == login && u.STATUS == "A").ToList();
            }

            if (model.Count == 0)
            {
                return null;
            }
            else
            {

                string EncryptionKey = "MAKV2SPBNI99212";
                byte[] clearBytes = Encoding.Unicode.GetBytes(senha);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        senha = Convert.ToBase64String(ms.ToArray());
                    }
                }

                //Comparando as informações
                if (senha == model[0].SENHA)
                {
                    model[0].SENHA = null;
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        [Route("api/acesso/usuario/{idUsuario}/{codAcesso}")]
        public bool GetUSUARIOs(int idUsuario, int codAcesso)
        {
            List<USUARIODTO> model = (from u in db.USUARIOs
                                      select new USUARIODTO()
                                      {
                                          ID_USUARIO = u.ID_USUARIO,
                                          ID_PERFIL = u.ID_PERFIL
                                      }).Where(u => u.ID_USUARIO == idUsuario).ToList();

            if (model.Count > 0)
            {
                List<PERFILACESSODTO> modelPerfilAcesso = (from pa in db.PERFIL_ACESSO
                                                           join a in db.ACESSOes on pa.ID_ACESSO equals a.ID_ACESSO
                                                           select new PERFILACESSODTO()
                                                           {
                                                               ID_PERFIL = pa.ID_PERFIL,
                                                               ID_ACESSO = pa.ID_ACESSO,
                                                               COD_ACESSO = a.CODIGO_ACESSO
                                                           }).Where(pa => pa.ID_PERFIL == model[0].ID_PERFIL && pa.COD_ACESSO == codAcesso).ToList();
                if (modelPerfilAcesso.Count == 0)
                {
                    List<USUARIOACESSODTO> modelUsuarioAcesso = (from ua in db.USUARIO_ACESSO
                                                                 join a in db.ACESSOes on ua.ID_ACESSO equals a.ID_ACESSO
                                                                 select new USUARIOACESSODTO()
                                                                 {
                                                                     ID_USUARIO = ua.ID_USUARIO,
                                                                     ID_ACESSO = ua.ID_ACESSO,
                                                                     COD_ACESSO = a.CODIGO_ACESSO
                                                                 }).Where(ua => ua.ID_USUARIO == idUsuario && ua.COD_ACESSO == codAcesso).ToList();
                    return (modelUsuarioAcesso.Count > 0);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

        }

        [Route("api/usuarios/perfil/{OrdemHierarquica}")]
        public IEnumerable<USUARIODTO> GetUSUARIOs_PERFIL(int OrdemHierarquica)
        {
            return (from u in db.USUARIOs
                    join p in db.PERFILs on u.ID_PERFIL equals p.ID_PERFIL into _p
                    from p in _p.DefaultIfEmpty()
                    select new USUARIODTO()
                    {
                        ID_USUARIO = u.ID_USUARIO,
                        LOGIN = u.LOGIN,
                        SENHA = u.SENHA,
                        TIPO_ACESSO = u.TIPO_ACESSO,
                        DESC_TIPO_ACESSO = u.TIPO_ACESSO == 0 ? "GESTÃO" :
                                                u.TIPO_ACESSO == 1 ? "CLÍNICO" :
                                                u.TIPO_ACESSO == 2 ? "GESTÃO E CLÍNICO" : "",
                        MASTER = u.MASTER,
                        ID_PROFISSIONAL = u.ID_PROFISSIONAL,
                        ID_PERFIL = u.ID_PERFIL,
                        PERFIL = p.DESCRICAO,
                        ORDEM_HIERARQUICA = p.ORDEM_HIERARQUICA,
                        PERCENTUAL_COMISSAO = u.PERCENTUAL_COMISSAO,
                        EMAIL_SENHA = u.EMAIL_SENHA,
                        STATUS = u.STATUS
                    }).Where(u => (u.ORDEM_HIERARQUICA >= OrdemHierarquica && u.MASTER == true) || u.ORDEM_HIERARQUICA > OrdemHierarquica).ToList();
        }

        // PUT: api/Usuario/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUSUARIO(int id, USUARIO uSUARIO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uSUARIO.ID_USUARIO)
            {
                return BadRequest();
            }

            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(uSUARIO.SENHA);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    uSUARIO.SENHA = Convert.ToBase64String(ms.ToArray());
                }
            }

            db.Entry(uSUARIO).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!USUARIOExists(id))
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

        // POST: api/Usuario
        [ResponseType(typeof(USUARIO))]
        [Route("api/usuario", Name = "PostUsuario")]
        public IHttpActionResult PostUSUARIO(USUARIO uSUARIO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_USUARIO;").First();

            uSUARIO.ID_USUARIO = NextValue;

            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(uSUARIO.SENHA);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    uSUARIO.SENHA = Convert.ToBase64String(ms.ToArray());
                }
            }

            db.USUARIOs.Add(uSUARIO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (USUARIOExists(uSUARIO.ID_USUARIO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetUsuarioById", new { idUsuario = uSUARIO.ID_USUARIO }, uSUARIO);
        }

        // DELETE: api/Usuario/5
        [ResponseType(typeof(USUARIO))]
        public async Task<IHttpActionResult> DeleteUSUARIO(int id)
        {
            USUARIO uSUARIO = await db.USUARIOs.FindAsync(id);
            if (uSUARIO == null)
            {
                return NotFound();
            }

            db.USUARIOs.Remove(uSUARIO);
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

        private bool USUARIOExists(int id)
        {
            return db.USUARIOs.Count(e => e.ID_USUARIO == id) > 0;
        }
    }
}