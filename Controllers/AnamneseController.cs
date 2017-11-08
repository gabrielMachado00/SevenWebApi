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
using System.Threading.Tasks;

namespace SisMedApi.Controllers
{
    public class ANAMNESEDTO
    {
        public int ID_ANAMNESE { get; set; }
        public string TITULO { get; set; }
        public string DESCRICAO { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public string NOME { get; set; }
    }

    public class AnamneseController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/profissional/1/anamnese
        [Route("api/anamneses")]
        public IEnumerable<ANAMNESEDTO> GetANAMNESEs()
        {
            return (from a in db.ANAMNESEs
                    join p in db.PROFISSIONALs on a.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new ANAMNESEDTO()
                    {
                        ID_ANAMNESE = a.ID_ANAMNESE,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        TITULO = a.TITULO,
                        DESCRICAO = a.DESCRICAO,
                        NOME = p.NOME
                    }).ToList();
        }

        // GET api/profissional/1/anamnese
        [Route("api/profissional/{idProfissional}/anamnese")]
        public IEnumerable<ANAMNESEDTO> GetPROFISSIONAL_ANAMNESE(int idProfissional)
        {
            return (from a in db.ANAMNESEs
                    join p in db.PROFISSIONALs on a.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new ANAMNESEDTO()
                    {
                        ID_ANAMNESE = a.ID_ANAMNESE,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        TITULO = a.TITULO,
                        DESCRICAO = a.DESCRICAO,
                        NOME = p.NOME
                    }).Where(pr => pr.ID_PROFISSIONAL == idProfissional).ToList();
        }


        // GET api/profissional/anamnese/5/2
        [Route("api/profissionalanamnese/{idProfissional}/{idAnamnese}")]
        public IEnumerable<ANAMNESEDTO> GetANAMNESE(int idProfissional, int idAnamnese)
        {
            return (from a in db.ANAMNESEs
                    join p in db.PROFISSIONALs on a.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new ANAMNESEDTO()
                    {
                        ID_ANAMNESE = a.ID_ANAMNESE,
                        ID_PROFISSIONAL = a.ID_PROFISSIONAL,
                        TITULO = a.TITULO,
                        DESCRICAO = a.DESCRICAO,
                        NOME = p.NOME
                    }).Where(a => a.ID_PROFISSIONAL == idProfissional && a.ID_ANAMNESE == idAnamnese).ToList();
        }

        // PUT: api/ProfissionalAnamnese/PutPROFISSIONAL_ANAMNESE/5/2
        [ResponseType(typeof(void))]
        [Route("api/ProfissionalAnamnese/PutPROFISSIONAL_ANAMNESE/{idProfissional}/{idAnamnese}")]
        public IHttpActionResult PutPROFISSIONAL_ANAMNESE(int idProfissional, int idAnamnese, ANAMNESE aNAMNESE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idProfissional != aNAMNESE.ID_PROFISSIONAL)
            {
                return BadRequest();
            }

            if (idAnamnese != aNAMNESE.ID_ANAMNESE)
            {
                return BadRequest();
            }

            db.Entry(aNAMNESE).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ANAMNESEExists(idProfissional, idAnamnese))
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

        // PUT: api/Anamnese/PutANAMNESE/5
        [ResponseType(typeof(void))]
        [Route("api/Anamnese/PutANAMNESE/{idAnamnese}")]
        public IHttpActionResult PutANAMNESE(int idAnamnese, ANAMNESE aNAMNESE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idAnamnese != aNAMNESE.ID_ANAMNESE)
            {
                return BadRequest();
            }

            db.Entry(aNAMNESE).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ANAMNESEExists(idAnamnese))
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

        // POST: api/Anamnese
        [ResponseType(typeof(ANAMNESE))]
        public IHttpActionResult PostANAMNESE(ANAMNESE aNAMNESE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ANAMNESE;").First();

            aNAMNESE.ID_ANAMNESE = NextValue;

            db.ANAMNESEs.Add(aNAMNESE);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ANAMNESEExists(aNAMNESE.ID_PROFISSIONAL, aNAMNESE.ID_ANAMNESE))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aNAMNESE.ID_ANAMNESE }, aNAMNESE);
        }

        // DELETE: api/Profissional/Anamnese/Delete/1/2
        [ResponseType(typeof(ANAMNESE))]
        [Route("api/Profissional/Anamnese/Delete/{idProfissional}/{idAnamnese}")]
        public async Task<IHttpActionResult> DeletePROFISSIONAL_ANAMNESE(int idProfissional, int idAnamnese)
        {
            ATENDIMENTO_ANAMNESE aTENDIMENTO_ANAMNESE = await db.ATENDIMENTO_ANAMNESE.Where(aa => aa.ID_ANAMNESE == idAnamnese).FirstOrDefaultAsync();
            if (aTENDIMENTO_ANAMNESE != null)
            {
                return Content(HttpStatusCode.NotModified, "Anamnese não pode ser excluída pois está vinculada a um atendimento");
            }

            ANAMNESE aNAMNESE = await db.ANAMNESEs.Where(a => a.ID_PROFISSIONAL == idProfissional && a.ID_ANAMNESE == idAnamnese).FirstOrDefaultAsync();
            if (aNAMNESE == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM ANAMNESE WHERE ID_PROFISSIONAL = {0} AND ID_ANAMNESE = {1}";

            if (db.Database.ExecuteSqlCommand(sql, idProfissional, idAnamnese) > 0)
                return Ok(aNAMNESE);
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

        private bool ANAMNESEExists(int idProfissional, int idAnamnese)
        {
            return db.ANAMNESEs.Count(e => e.ID_ANAMNESE == idAnamnese && e.ID_PROFISSIONAL == idProfissional) > 0;
        }

        private bool ANAMNESEExists(int idAnamnese)
        {
            return db.ANAMNESEs.Count(e => e.ID_ANAMNESE == idAnamnese) > 0;
        }
    }
}