using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;
using System.Threading.Tasks;
using System;

namespace SisMedApi.Controllers
{
    public class PROFISSIONAL_RECEITADTO
    {
        public int ID_RECEITA { get; set; }
        public Nullable<int> ID_PROFISSIONAL { get; set; }
        public string TITULO { get; set; }
        public string DESCRICAO { get; set; }
        public string NOME { get; set; }
    }

    public class ProfissionalReceitaController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/profissional/1/receita
        [Route("api/receitas")]
        public IEnumerable<PROFISSIONAL_RECEITADTO> GetRECEITAS()
        {
            return (from pr in db.PROFISSIONAL_RECEITA
                    join p in db.PROFISSIONALs on pr.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new PROFISSIONAL_RECEITADTO()
                    {
                        ID_RECEITA = pr.ID_RECEITA,
                        ID_PROFISSIONAL = pr.ID_PROFISSIONAL,
                        TITULO = pr.TITULO,
                        DESCRICAO = pr.DESCRICAO,
                        NOME = p.NOME
                    }).ToList();
        }

        // GET api/profissional/1/receita
        [Route("api/profissional/{idProfissional}/receita")]
        public IEnumerable<PROFISSIONAL_RECEITADTO> GetPROFISSIONAL_RECEITA(int idProfissional)
        {
            return (from pr in db.PROFISSIONAL_RECEITA
                    join p in db.PROFISSIONALs on pr.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new PROFISSIONAL_RECEITADTO()
                    {
                        ID_RECEITA = pr.ID_RECEITA,
                        ID_PROFISSIONAL = pr.ID_PROFISSIONAL,
                        TITULO = pr.TITULO,
                        DESCRICAO = pr.DESCRICAO,
                        NOME = p.NOME
                    }).Where(pr => pr.ID_PROFISSIONAL == idProfissional).ToList();
        }


        // GET api/profissional/receita/5/2
        [Route("api/profissionalreceita/{idProfissional}/{idReceita}")]
        public IEnumerable<PROFISSIONAL_RECEITADTO> GetRECEITA(int idProfissional, int idReceita)
        {
            return (from pr in db.PROFISSIONAL_RECEITA
                    join p in db.PROFISSIONALs on pr.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    select new PROFISSIONAL_RECEITADTO()
                    {
                        ID_RECEITA = pr.ID_RECEITA,
                        ID_PROFISSIONAL = pr.ID_PROFISSIONAL,
                        TITULO = pr.TITULO,
                        DESCRICAO = pr.DESCRICAO,
                        NOME = p.NOME
                    }).Where(pr => pr.ID_PROFISSIONAL == idProfissional && pr.ID_RECEITA == idReceita).ToList();
        }

        // PUT: api/ProfissionalReceita/PutPROFISSIONAL_RECEITA/5/2
        [ResponseType(typeof(void))]
        [Route("api/ProfissionalReceita/PutPROFISSIONAL_RECEITA/{idProfissional}/{idReceita}")]
        public IHttpActionResult PutPROFISSIONAL_RECEITA(int idProfissional, int idReceita, PROFISSIONAL_RECEITA pROFISSIONAL_RECEITA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idProfissional != pROFISSIONAL_RECEITA.ID_PROFISSIONAL)
            {
                return BadRequest();
            }

            if (idReceita != pROFISSIONAL_RECEITA.ID_RECEITA)
            {
                return BadRequest();
            }

            db.Entry(pROFISSIONAL_RECEITA).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                string err = e.Message;
                if (!PROFISSIONAL_RECEITAExists(idProfissional, idReceita))
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

        // POST: api/ProfissionalReceita
        [ResponseType(typeof(PROFISSIONAL_RECEITA))]
        public IHttpActionResult PostPROFISSIONAL_RECEITA(PROFISSIONAL_RECEITA pROFISSIONAL_RECEITA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_PROFISSIONAL_RECEITA;").First();

            pROFISSIONAL_RECEITA.ID_RECEITA = NextValue;

            db.PROFISSIONAL_RECEITA.Add(pROFISSIONAL_RECEITA);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PROFISSIONAL_RECEITAExists(pROFISSIONAL_RECEITA.ID_RECEITA, pROFISSIONAL_RECEITA.ID_PROFISSIONAL))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROFISSIONAL_RECEITA.ID_RECEITA }, pROFISSIONAL_RECEITA);
        }

        // DELETE: api/Profissional/Receita/Delete/1/2
        [ResponseType(typeof(PROFISSIONAL_RECEITA))]
        [Route("api/Profissional/Receita/Delete/{idProfissional}/{idReceita}")]
        public async Task<IHttpActionResult> DeletePROFISSIONAL_RECEITA(int idProfissional, int idReceita)
        {
            ATENDIMENTO_RECEITA aTENDIMENTO_RECEITA = await db.ATENDIMENTO_RECEITA.Where(ar => ar.ID_RECEITA == idReceita).FirstOrDefaultAsync();
            if (aTENDIMENTO_RECEITA != null)
            {
                return Content(HttpStatusCode.NotModified, "Receita não pode ser excluída pois está vinculada a um atendimento");
            }

            PROFISSIONAL_RECEITA pROFISSIONAL_RECEITA = await db.PROFISSIONAL_RECEITA.Where(pr => pr.ID_PROFISSIONAL == idProfissional && pr.ID_RECEITA == idReceita).FirstOrDefaultAsync();
            if (pROFISSIONAL_RECEITA == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM PROFISSIONAL_RECEITA WHERE ID_PROFISSIONAL = {0} AND ID_RECEITA = {1}";

            if (db.Database.ExecuteSqlCommand(sql, idProfissional, idReceita) > 0)
                return Ok(pROFISSIONAL_RECEITA);
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

        private bool PROFISSIONAL_RECEITAExists(int idProfissional, int idReceita)
        {
            return db.PROFISSIONAL_RECEITA.Count(e => e.ID_RECEITA == idReceita && e.ID_PROFISSIONAL == idProfissional) > 0;
        }
    }
}