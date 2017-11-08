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
using System.Data.SqlClient;

namespace SisMedApi.Controllers
{
    public class ATENDIMENTO_RECEITADTO
    {
        public int ID_ATENDIMENTO_RECEITA { get; set; }
        public Nullable<int> ID_ATENDIMENTO { get; set; }
        public Nullable<int> ID_RECEITA { get; set; }
        public string LOGIN { get; set; }
        public string DESCRICAO { get; set; }
        public DateTime DATA { get; set; }
        public Nullable<int> ID_CLIENTE { get; set; }
        public string TITULO { get; set; }
    }

    public class AtendimentoReceitaController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/Atendimento/1/Receitas
        [Route("api/Atendimento/{idAtendimento}/Receitas")]
        public IEnumerable<ATENDIMENTO_RECEITADTO> GetATENDIMENTO_RECEITAS(int idAtendimento)
        {
            return (from ar in db.ATENDIMENTO_RECEITA
                    join pr in db.PROFISSIONAL_RECEITA on ar.ID_RECEITA equals pr.ID_RECEITA into _pr
                    from pr in _pr.DefaultIfEmpty()                    
                    select new ATENDIMENTO_RECEITADTO()
                    {
                        ID_ATENDIMENTO_RECEITA = ar.ID_ATENDIMENTO_RECEITA,
                        ID_ATENDIMENTO = ar.ID_ATENDIMENTO,
                        ID_RECEITA = ar.ID_RECEITA,
                        DESCRICAO = ar.DESCRICAO,
                        LOGIN = ar.LOGIN,
                        DATA = ar.DATA,
                        TITULO = string.IsNullOrEmpty(ar.TITULO) ? pr.TITULO : ar.TITULO,
                        ID_CLIENTE = ar.ID_CLIENTE
                    }).Where(ar => ar.ID_ATENDIMENTO == idAtendimento).ToList();
        }

        // GET api/Atendimento/1/Receita/1
        [Route("api/Atendimento/{idAtendimento}/Receita/{idReceita}")]
        public IEnumerable<ATENDIMENTO_RECEITADTO> GetATENDIMENTO_RECEITA(int idAtendimento, int idReceita)
        {
            return (from ar in db.ATENDIMENTO_RECEITA
                    join pr in db.PROFISSIONAL_RECEITA on ar.ID_RECEITA equals pr.ID_RECEITA
                    select new ATENDIMENTO_RECEITADTO()
                    {
                        ID_ATENDIMENTO = ar.ID_ATENDIMENTO,
                        ID_RECEITA = ar.ID_RECEITA,
                        LOGIN = ar.LOGIN,
                        DESCRICAO = ar.DESCRICAO,
                        DATA = ar.DATA,
                        TITULO = pr.TITULO
                    }).Where(ar => ar.ID_ATENDIMENTO == idAtendimento && ar.ID_RECEITA == idReceita).ToList();
        }

        // GET api/Atendimento/1/Receita/1
        [Route("api/Atendimento/Receita/{idAtendimentoReceita}")]
        public IEnumerable<ATENDIMENTO_RECEITADTO> GetATENDIMENTO_RECEITA(int idAtendimentoReceita)
        {
            return (from ar in db.ATENDIMENTO_RECEITA
                    join pr in db.PROFISSIONAL_RECEITA on ar.ID_RECEITA equals pr.ID_RECEITA into _pr
                    from pr in _pr.DefaultIfEmpty()
                    select new ATENDIMENTO_RECEITADTO()
                    {
                        ID_ATENDIMENTO_RECEITA = ar.ID_ATENDIMENTO_RECEITA,
                        ID_ATENDIMENTO = ar.ID_ATENDIMENTO,
                        ID_RECEITA = ar.ID_RECEITA,
                        LOGIN = ar.LOGIN,
                        DESCRICAO = ar.DESCRICAO,
                        DATA = ar.DATA,
                        TITULO = string.IsNullOrEmpty(ar.TITULO) ? pr.TITULO : ar.TITULO,
                        ID_CLIENTE = ar.ID_CLIENTE
                    }).Where(ar => ar.ID_ATENDIMENTO_RECEITA == idAtendimentoReceita).ToList();
        }

        // GET api/Atendimento/1/Receita/1
        [Route("api/Atendimento/Receitas/Cliente/{idCliente}")]
        public IEnumerable<ATENDIMENTO_RECEITADTO> GetAtendimentoByCliente(int idCliente)
        {
            string sql = @"SELECT ID_ATENDIMENTO_RECEITA, AR.ID_ATENDIMENTO, AR.ID_RECEITA, AR.LOGIN,
                               AR.DESCRICAO, AR.DATA, ISNULL(PR.TITULO, AR.TITULO) TITULO,
	                           ISNULL(A.ID_CLIENTE, AR.ID_CLIENTE) ID_CLIENTE
                          FROM ATENDIMENTO_RECEITA AR LEFT JOIN PROFISSIONAL_RECEITA PR ON PR.ID_RECEITA = AR.ID_RECEITA
                               LEFT JOIN ATENDIMENTO A ON A.ID_ATENDIMENTO = AR.ID_ATENDIMENTO
                           WHERE ISNULL(A.ID_CLIENTE, AR.ID_CLIENTE) =  @PID_CLIENTE
                            order by ID_ATENDIMENTO_RECEITA DESC";
            return db.Database.SqlQuery<ATENDIMENTO_RECEITADTO>(sql, new SqlParameter("@PID_CLIENTE", idCliente)).ToList();
        }


        // PUT: api/AtendimentoReceita/PutATENDIMENTO_RECEITA/1/1
        [ResponseType(typeof(void))]
        [Route("api/AtendimentoReceita/PutATENDIMENTO_RECEITA/{idAtendimentoReceita}")]
        public IHttpActionResult PutATENDIMENTO_RECEITA(int idAtendimentoReceita, ATENDIMENTO_RECEITA aTENDIMENTO_RECEITA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idAtendimentoReceita != aTENDIMENTO_RECEITA.ID_ATENDIMENTO_RECEITA)
            {
                return BadRequest();
            }

            //if (idReceita != aTENDIMENTO_RECEITA.ID_RECEITA)
            //{
            //    return BadRequest();
            //}

            db.Entry(aTENDIMENTO_RECEITA).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ATENDIMENTO_RECEITAExists(aTENDIMENTO_RECEITA.ID_ATENDIMENTO, aTENDIMENTO_RECEITA.ID_RECEITA))
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

        // POST: api/AtendimentoReceita
        [ResponseType(typeof(ATENDIMENTO_RECEITA))]
        public async Task<IHttpActionResult> PostATENDIMENTO_RECEITA(ATENDIMENTO_RECEITA aTENDIMENTO_RECEITA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ATENDIMENTO_RECEITA;").First();
            aTENDIMENTO_RECEITA.ID_ATENDIMENTO_RECEITA = NextValue;

            db.ATENDIMENTO_RECEITA.Add(aTENDIMENTO_RECEITA);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ATENDIMENTO_RECEITAExists(aTENDIMENTO_RECEITA.ID_ATENDIMENTO, aTENDIMENTO_RECEITA.ID_RECEITA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aTENDIMENTO_RECEITA.ID_ATENDIMENTO }, aTENDIMENTO_RECEITA);
        }

        // DELETE: api/AtendimentoReceita/5
        [ResponseType(typeof(ATENDIMENTO_RECEITA))]
        [Route("api/Atendimento/Receita/Delete/{idAtendimentoReceita}")]
        public IHttpActionResult DeleteATENDIMENTO_RECEITA(int idAtendimentoReceita)
        {
            ATENDIMENTO_RECEITA aTENDIMENTO_RECEITA = db.ATENDIMENTO_RECEITA.Where(ar => ar.ID_ATENDIMENTO_RECEITA == idAtendimentoReceita).FirstOrDefault();
            if (aTENDIMENTO_RECEITA == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM ATENDIMENTO_RECEITA WHERE ID_ATENDIMENTO_RECEITA = {0}";

            if (db.Database.ExecuteSqlCommand(sql, idAtendimentoReceita) > 0)
                return Ok(aTENDIMENTO_RECEITA);
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

        private bool ATENDIMENTO_RECEITAExists(int? idAtendimento, int? idReceita)
        {
            if (idAtendimento.Value > 0 && idReceita.Value > 0)
                return true;
            else
                return db.ATENDIMENTO_RECEITA.Count(e => e.ID_ATENDIMENTO == idAtendimento && e.ID_RECEITA == idReceita) > 0;
        }
    }
}