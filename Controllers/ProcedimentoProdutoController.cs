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

namespace SisMedApi.Controllers
{
    public class PROCEDIMENTO_PRODUTODTO
    {
        public int ID_PROCEDIMENTO { get; set; }
        public int ID_PRODUTO { get; set; }
        public string DESC_PRODUTO { get; set; }
        public string DESCRICAO { get; set; }
        public Nullable<decimal> QUANTIDADE_PADRAO { get; set; }
        public Nullable<int> ID_UNIDADE { get; set; }
        public string UNIDADE { get; set; }
        public Nullable<int> ID_FARMACIA { get; set; }
        public string FARMACIA { get; set; }
    }

    public class ProcedimentoProdutoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/ProcedimentoProduto/Produtos/1
        [Route("api/ProcedimentoProduto/Produtos/{idProcedimento}")]
        public IEnumerable<PROCEDIMENTO_PRODUTODTO> GetPROCEDIMENTO_PRODUTOS(int idProcedimento)
        {
            return (from pp in db.PROCEDIMENTO_PRODUTO
                    join p in db.PRODUTOes on pp.ID_PRODUTO equals p.ID_PRODUTO
                    join pr in db.PROCEDIMENTOes on pp.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join u in db.UNIDADEs on pp.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join f in db.FARMACIA_SATELITE on pp.ID_FARMACIA equals f.ID_FARMACIA into _f
                    from f in _f.DefaultIfEmpty()
                    select new PROCEDIMENTO_PRODUTODTO()
                    {
                        ID_PROCEDIMENTO = pp.ID_PROCEDIMENTO,
                        ID_PRODUTO = pp.ID_PRODUTO,
                        DESCRICAO = pr.DESCRICAO,
                        DESC_PRODUTO = p.DESC_PRODUTO,
                        QUANTIDADE_PADRAO = pp.QUANTIDADE_PADRAO,
                        ID_UNIDADE = pp.ID_UNIDADE,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_FARMACIA = pp.ID_FARMACIA,
                        FARMACIA = f.DESCRICAO
                    }).Where(pp => pp.ID_PROCEDIMENTO == idProcedimento).ToList();
        }

        // GET api/ProcedimentoProduto/1/1
        [Route("api/ProcedimentoProduto/{idProcedimento}/{idProduto}")]
        public IEnumerable<PROCEDIMENTO_PRODUTODTO> GetPROCEDIMENTO_PRO_PRODUTO(int idProcedimento, int idProduto)
        {
            return (from pp in db.PROCEDIMENTO_PRODUTO
                    join p in db.PRODUTOes on pp.ID_PRODUTO equals p.ID_PRODUTO
                    join pr in db.PROCEDIMENTOes on pp.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join u in db.UNIDADEs on pp.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join f in db.FARMACIA_SATELITE on pp.ID_FARMACIA equals f.ID_FARMACIA into _f
                    from f in _f.DefaultIfEmpty()
                    select new PROCEDIMENTO_PRODUTODTO()
                    {
                        ID_PROCEDIMENTO = pp.ID_PROCEDIMENTO,
                        ID_PRODUTO = pp.ID_PRODUTO,
                        DESCRICAO = pr.DESCRICAO,
                        DESC_PRODUTO = p.DESC_PRODUTO,
                        QUANTIDADE_PADRAO = pp.QUANTIDADE_PADRAO,
                        ID_UNIDADE = pp.ID_UNIDADE,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_FARMACIA = pp.ID_FARMACIA,
                        FARMACIA = f.DESCRICAO
                    }).Where(pp => pp.ID_PROCEDIMENTO == idProcedimento && pp.ID_PRODUTO == idProduto).ToList();
        }

        // PUT: api/ProcedimentoEquipamento/5/1
        [ResponseType(typeof(void))]
        [Route("api/ProcedimentoProduto/PutPROCEDIMENTO_PRODUTOS/{idProcedimento}/{idProduto}")]
        public IHttpActionResult PutPROCEDIMENTO_PRODUTO(int idProcedimento, int idProduto, PROCEDIMENTO_PRODUTO pROCEDIMENTO_PRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idProcedimento != pROCEDIMENTO_PRODUTO.ID_PROCEDIMENTO)
            {
                return BadRequest();
            }

            if (idProduto != pROCEDIMENTO_PRODUTO.ID_PRODUTO)
            {
                return BadRequest();
            }

            db.Entry(pROCEDIMENTO_PRODUTO).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PROCEDIMENTO_PRODUTOExists(idProcedimento, idProduto))
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

        // POST: api/ProcedimentoProduto
        [ResponseType(typeof(PROCEDIMENTO_PRODUTO))]
        public async Task<IHttpActionResult> PostPROCEDIMENTO_PRODUTO(PROCEDIMENTO_PRODUTO pROCEDIMENTO_PRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PROCEDIMENTO_PRODUTO.Add(pROCEDIMENTO_PRODUTO);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PROCEDIMENTO_PRODUTOExists(pROCEDIMENTO_PRODUTO.ID_PROCEDIMENTO, pROCEDIMENTO_PRODUTO.ID_PRODUTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROCEDIMENTO_PRODUTO.ID_PROCEDIMENTO }, pROCEDIMENTO_PRODUTO);
        }

        // DELETE: api/ProcedimentoProduto/5
        [ResponseType(typeof(PROCEDIMENTO_PRODUTO))]
        [Route("api/ProcedimentoMatMed/DeletePROCEDIMENTO_MATMED/{idProcedimento}/{idMatMed}")]
        public async Task<IHttpActionResult> DeletePROCEDIMENTO_MATMED(int idProcedimento, int idMatMed)
        {
            PROCEDIMENTO_PRODUTO pROCEDIMENTO_PRODUTO = await db.PROCEDIMENTO_PRODUTO.Where(pp => pp.ID_PROCEDIMENTO == idProcedimento && pp.ID_PRODUTO == idMatMed).FirstOrDefaultAsync();
            if (pROCEDIMENTO_PRODUTO == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM PROCEDIMENTO_PRODUTO WHERE ID_PROCEDIMENTO = {0} AND ID_PRODUTO = {1}";

            if (db.Database.ExecuteSqlCommand(sql, idProcedimento, idMatMed) > 0)
                return Ok(pROCEDIMENTO_PRODUTO);
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

        private bool PROCEDIMENTO_PRODUTOExists(int idProcedimento, int idProduto)
        {
            return db.PROCEDIMENTO_PRODUTO.Count(e => e.ID_PROCEDIMENTO == idProcedimento && e.ID_PRODUTO == idProduto) > 0;
        }
    }
}