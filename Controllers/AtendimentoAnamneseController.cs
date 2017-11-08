using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;
using System.Data.SqlClient;
using System.Web.Http.Cors;

namespace SisMedApi.Controllers
{
    public class ATENDIMENTO_ANAMNESEDTO
    {
        public int ID_ATENDIMENTO { get; set; }
        public int ID_ANAMNESE { get; set; }
        public string LOGIN { get; set; }
        public string DESCRICAO { get; set; }
        public Nullable<DateTime> DATA_HORA_INI { get; set; }
        public Nullable<int> ID_PROFISSIONAL { get; set; }
        public string PROFISSIONAL { get; set; }
    }
    public class AtendimentoAnamneseController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET: api/AtendimentoAnamnese
        public IQueryable<ATENDIMENTO_ANAMNESE> GetATENDIMENTO_ANAMNESE()
        {
            return db.ATENDIMENTO_ANAMNESE;
        }
       
        // GET: api/AtendimentoAnamnese/5
        [ResponseType(typeof(ATENDIMENTO_ANAMNESE))]
        public IHttpActionResult GetATENDIMENTO_ANAMNESE(int id)
        {
            ATENDIMENTO_ANAMNESE aTENDIMENTO_ANAMNESE = db.ATENDIMENTO_ANAMNESE.Find(id);
            if (aTENDIMENTO_ANAMNESE == null)
            {
                return NotFound();
            }

            return Ok(aTENDIMENTO_ANAMNESE);
        }

        // GET api/atendimentoanamnese/cliente/1
        [Route("api/atendimentoanamnese/cliente/{idCliente}")]
        public IEnumerable<ATENDIMENTO_ANAMNESEDTO> GetAtendimentoAnamnese(int idCliente)
        {
            string sql = @"SELECT AN.ID_ANAMNESE, AN.ID_ATENDIMENTO, CAST(A.DATA_HORA_INI AS DATE) DATA_HORA_INI, DESCRICAO, 
                                  A.ID_PROFISSIONAL, P.NOME PROFISSIONAL
                            FROM ATENDIMENTO_ANAMNESE AN, ATENDIMENTO A,PROFISSIONAL P
                            WHERE P.ID_PROFISSIONAL = A.ID_PROFISSIONAL
                            AND AN.ID_ATENDIMENTO = A.ID_ATENDIMENTO
                            and A.ID_CLIENTE = @PID_CLIENTE
                          ORDER BY A.DATA_HORA_INI DESC";
            return db.Database.SqlQuery<ATENDIMENTO_ANAMNESEDTO>(sql, new SqlParameter("@PID_CLIENTE", idCliente)).ToList();
            
        }

        // PUT: api/AtendimentoAnamnese/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutATENDIMENTO_ANAMNESE(int id, ATENDIMENTO_ANAMNESE aTENDIMENTO_ANAMNESE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aTENDIMENTO_ANAMNESE.ID_ATENDIMENTO)
            {
                return BadRequest();
            }

            db.Entry(aTENDIMENTO_ANAMNESE).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ATENDIMENTO_ANAMNESEExists(id))
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

        // POST: api/AtendimentoAnamnese
        [ResponseType(typeof(ATENDIMENTO_ANAMNESE))]
        public async Task<IHttpActionResult> PostATENDIMENTO_ANAMNESE(ATENDIMENTO_ANAMNESE aTENDIMENTO_ANAMNESE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ATENDIMENTO_ANAMNESE.Add(aTENDIMENTO_ANAMNESE);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ATENDIMENTO_ANAMNESEExists(aTENDIMENTO_ANAMNESE.ID_ATENDIMENTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aTENDIMENTO_ANAMNESE.ID_ATENDIMENTO }, aTENDIMENTO_ANAMNESE);
        }

        // DELETE: api/AtendimentoAnamnese/5
        [ResponseType(typeof(ATENDIMENTO_ANAMNESE))]
        public async Task<IHttpActionResult> DeleteATENDIMENTO_ANAMNESE(int id)
        {
            ATENDIMENTO_ANAMNESE aTENDIMENTO_ANAMNESE = await db.ATENDIMENTO_ANAMNESE.FindAsync(id);
            if (aTENDIMENTO_ANAMNESE == null)
            {
                return NotFound();
            }

            db.ATENDIMENTO_ANAMNESE.Remove(aTENDIMENTO_ANAMNESE);
            await db.SaveChangesAsync();

            return Ok(aTENDIMENTO_ANAMNESE);
        }    
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ATENDIMENTO_ANAMNESEExists(int id)
        {
            return db.ATENDIMENTO_ANAMNESE.Count(e => e.ID_ATENDIMENTO == id) > 0;
        }
    }
}