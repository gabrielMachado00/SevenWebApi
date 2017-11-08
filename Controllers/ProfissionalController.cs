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
using System.Web.Http.Cors;

namespace SisMedApi.Controllers
{
    public class PROFISSIONALDTO
    {
        public int ID_PROFISSIONAL { get; set; }
        public string NOME { get; set; }
        public string TITULO { get; set; }
        public string ESPECIALIDADE { get; set; }
        public DateTime? DATA_CADASTRO { get; set; }
        public string TELEFONE { get; set; }
        public string CELULAR { get; set; }
        public string EMAIL { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public DateTime? DATA_NASCIMENTO { get; set; }
        public string SEXO { get; set; }
        public string ESTADO_CIVIL { get; set; }
        public string REGISTRO { get; set; }
        public Nullable<int> ID_SALA { get; set; }
        public Nullable<bool> STATUS { get; set; }
        public Nullable<bool> EXIBE_AGENDA { get; set; }
        public Nullable<int> TEMPO_PADRAO { get; set; }
    }
    public class ProfissionalController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/Profissional
        public IQueryable<PROFISSIONAL> GetPROFISSIONALs()
        {
            return db.PROFISSIONALs.OrderBy(a=>a.NOME);
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]

        // GET: api/Profissional/5
        [ResponseType(typeof(PROFISSIONAL))]
        public IHttpActionResult GetPROFISSIONAL(int id)
        {
            PROFISSIONAL pROFISSIONAL = db.PROFISSIONALs.Find(id);
            if (pROFISSIONAL == null)
            {
                return NotFound();
            }

            return Ok(pROFISSIONAL);
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]

        // GET api/profissional/horario/5
        [Route("api/sala/{idSala}/profissionais")]
        public IEnumerable<PROFISSIONALDTO> GetSALA_PROFISSIONAIS(int idSala)
        {
            return (from p in db.PROFISSIONALs
                    select new PROFISSIONALDTO()
                    {
                        ID_PROFISSIONAL = p.ID_PROFISSIONAL,
                        NOME = p.NOME,
                        TITULO = p.TITULO,
                        ESPECIALIDADE = p.ESPECIALIDADE,
                        DATA_CADASTRO = p.DATA_CADASTRO,
                        TELEFONE = p.TELEFONE,
                        CELULAR = p.CELULAR,
                        EMAIL = p.EMAIL,
                        CPF = p.CPF,
                        RG = p.RG,
                        DATA_NASCIMENTO = p.DATA_NASCIMENTO,
                        SEXO = p.SEXO,
                        ESTADO_CIVIL = p.ESTADO_CIVIL,
                        REGISTRO = p.REGISTRO,
                        ID_SALA = p.ID_SALA,
                        STATUS = p.STATUS,
                        EXIBE_AGENDA = p.EXIBE_AGENDA,
                        TEMPO_PADRAO = p.TEMPO_PADRAO
                    }).Where(p => p.ID_SALA == idSala).ToList().OrderBy(a=>a.NOME);
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]

        // GET api/profissional/horario/5
        [Route("api/profissionais")]
        public IEnumerable<PROFISSIONALDTO> GetPROFISSIONAIS()
        {
            return (from p in db.PROFISSIONALs
                    select new PROFISSIONALDTO()
                    {
                        ID_PROFISSIONAL = p.ID_PROFISSIONAL,
                        NOME = p.NOME,
                        TITULO = p.TITULO,
                        ESPECIALIDADE = p.ESPECIALIDADE,
                        DATA_CADASTRO = p.DATA_CADASTRO,
                        TELEFONE = p.TELEFONE,
                        CELULAR = p.CELULAR,
                        EMAIL = p.EMAIL,
                        CPF = p.CPF,
                        RG = p.RG,
                        DATA_NASCIMENTO = p.DATA_NASCIMENTO,
                        SEXO = p.SEXO,
                        ESTADO_CIVIL = p.ESTADO_CIVIL,
                        REGISTRO = p.REGISTRO,
                        ID_SALA = p.ID_SALA,
                        STATUS = p.STATUS,
                        EXIBE_AGENDA = p.EXIBE_AGENDA,
                        TEMPO_PADRAO = p.TEMPO_PADRAO
                    }).Where(p => p.STATUS == true && p.EXIBE_AGENDA == true).ToList().OrderBy(a=>a.NOME);
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]

        // PUT: api/Profissional/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPROFISSIONAL(int id, PROFISSIONAL pROFISSIONAL)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pROFISSIONAL.ID_PROFISSIONAL)
            {
                return BadRequest();
            }

            db.Entry(pROFISSIONAL).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PROFISSIONALExists(id))
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
        [EnableCors(origins: "*", headers: "*", methods: "*")]

        // POST: api/Profissional
        [ResponseType(typeof(PROFISSIONAL))]
        public IHttpActionResult PostPROFISSIONAL(PROFISSIONAL pROFISSIONAL)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_PROFISSIONAL;").First();

            pROFISSIONAL.ID_PROFISSIONAL = NextValue;

            db.PROFISSIONALs.Add(pROFISSIONAL);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PROFISSIONALExists(pROFISSIONAL.ID_PROFISSIONAL))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROFISSIONAL.ID_PROFISSIONAL }, pROFISSIONAL);
        }

        // DELETE: api/Profissional/5
        [ResponseType(typeof(PROFISSIONAL))]
        public IHttpActionResult DeletePROFISSIONAL(int id)
        {
            PROFISSIONAL pROFISSIONAL = db.PROFISSIONALs.Find(id);
            if (pROFISSIONAL == null)
            {
                return NotFound();
            }

            db.PROFISSIONALs.Remove(pROFISSIONAL);
            db.SaveChanges();

            return Ok(pROFISSIONAL);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PROFISSIONALExists(int id)
        {
            return db.PROFISSIONALs.Count(e => e.ID_PROFISSIONAL == id) > 0;
        }
    }
}