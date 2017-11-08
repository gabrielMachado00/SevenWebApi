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
    public class PROCEDIMENTO_REGIAO_PRODUTODTO
    {
        public int ID_PROCEDIMENTO { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public int ID_PRODUTO { get; set; }
        public string REGIAO { get; set; }
        public string DESC_PRODUTO { get; set; }
        public string DESCRICAO { get; set; }
        public Nullable<decimal> QUANTIDADE_PADRAO { get; set; }
        public Nullable<int> ID_UNIDADE { get; set; }
        public string UNIDADE { get; set; }
        public Nullable<int> ID_FARMACIA { get; set; }
        public string FARMACIA { get; set; }
    }

    public class ProcedimentoRegiaoProdutoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/ProcedimentoProduto/1/Regiao/2/Produtos
        [Route("api/ProcedimentoProduto/{idProcedimento}/Regiao/{idRegiao}/Produtos")]
        public IEnumerable<PROCEDIMENTO_REGIAO_PRODUTODTO> GetPROCEDIMENTO_REGIAO_PRODUTOS(int idProcedimento, int idRegiao)
        {
            return (from pp in db.PROCEDIMENTO_REGIAO_PRODUTO
                    join r in db.REGIAO_CORPO on pp.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PRODUTOes on pp.ID_PRODUTO equals p.ID_PRODUTO
                    join pr in db.PROCEDIMENTOes on pp.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join u in db.UNIDADEs on pp.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join f in db.FARMACIA_SATELITE on pp.ID_FARMACIA equals f.ID_FARMACIA into _f
                    from f in _f.DefaultIfEmpty()
                    select new PROCEDIMENTO_REGIAO_PRODUTODTO()
                    {
                        ID_PROCEDIMENTO = pp.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pp.ID_REGIAO_CORPO,
                        ID_PRODUTO = pp.ID_PRODUTO,
                        REGIAO = r.REGIAO,
                        DESCRICAO = pr.DESCRICAO,
                        DESC_PRODUTO = p.DESC_PRODUTO,
                        QUANTIDADE_PADRAO = pp.QUANTIDADE_PADRAO,
                        ID_UNIDADE = pp.ID_UNIDADE,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_FARMACIA = pp.ID_FARMACIA,
                        FARMACIA = f.DESCRICAO
                    }).Where(pp => pp.ID_PROCEDIMENTO == idProcedimento && pp.ID_REGIAO_CORPO == idRegiao).ToList();
        }

        // GET api/ProcedimentoRegiaoProduto/1/1/2
        [Route("api/ProcedimentoRegiaoProduto/{idProcedimento}/{idRegiao}/{idProduto}")]
        public IEnumerable<PROCEDIMENTO_REGIAO_PRODUTODTO> GetPROCEDIMENTO_PRO_PRODUTO(int idProcedimento, int idRegiao, int idProduto)
        {
            return (from pp in db.PROCEDIMENTO_REGIAO_PRODUTO
                    join r in db.REGIAO_CORPO on pp.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    join p in db.PRODUTOes on pp.ID_PRODUTO equals p.ID_PRODUTO
                    join pr in db.PROCEDIMENTOes on pp.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO
                    join u in db.UNIDADEs on pp.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    join f in db.FARMACIA_SATELITE on pp.ID_FARMACIA equals f.ID_FARMACIA into _f
                    from f in _f.DefaultIfEmpty()
                    select new PROCEDIMENTO_REGIAO_PRODUTODTO()
                    {
                        ID_PROCEDIMENTO = pp.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = pp.ID_REGIAO_CORPO,
                        ID_PRODUTO = pp.ID_PRODUTO,
                        REGIAO = r.REGIAO,
                        DESCRICAO = pr.DESCRICAO,
                        DESC_PRODUTO = p.DESC_PRODUTO,
                        QUANTIDADE_PADRAO = pp.QUANTIDADE_PADRAO,
                        ID_UNIDADE = pp.ID_UNIDADE,
                        UNIDADE = u.NOME_FANTASIA,
                        ID_FARMACIA = pp.ID_FARMACIA,
                        FARMACIA = f.DESCRICAO
                    }).Where(pp => pp.ID_PROCEDIMENTO == idProcedimento && pp.ID_REGIAO_CORPO == idRegiao && pp.ID_PRODUTO == idProduto).ToList();
        }

        // PUT: api/ProcedimentoProduto/PutPROCEDIMENTO_PRODUTOS/5/1
        [ResponseType(typeof(void))]
        [Route("api/ProcedimentoProduto/PutPROCEDIMENTO_PRODUTOS/{idProcedimento}/{idRegiao}/{idProduto}")]
        public IHttpActionResult PutPROCEDIMENTO_REGIAO_PRODUTO(int idProcedimento, int idRegiao, int idProduto, PROCEDIMENTO_REGIAO_PRODUTO pROCEDIMENTO_REGIAO_PRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idProcedimento != pROCEDIMENTO_REGIAO_PRODUTO.ID_PROCEDIMENTO)
            {
                return BadRequest();
            }

            if (idRegiao != pROCEDIMENTO_REGIAO_PRODUTO.ID_REGIAO_CORPO)
            {
                return BadRequest();
            }

            if (idProduto != pROCEDIMENTO_REGIAO_PRODUTO.ID_PRODUTO)
            {
                return BadRequest();
            }

            db.Entry(pROCEDIMENTO_REGIAO_PRODUTO).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PROCEDIMENTO_REGIAO_PRODUTOExists(idProcedimento, idRegiao, idProduto))
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

        // POST: api/ProcedimentoRegiaoProduto
        [ResponseType(typeof(PROCEDIMENTO_REGIAO_PRODUTO))]
        public async Task<IHttpActionResult> PostPROCEDIMENTO_REGIAO_PRODUTO(PROCEDIMENTO_REGIAO_PRODUTO pROCEDIMENTO_REGIAO_PRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PROCEDIMENTO_REGIAO_PRODUTO.Add(pROCEDIMENTO_REGIAO_PRODUTO);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PROCEDIMENTO_REGIAO_PRODUTOExists(pROCEDIMENTO_REGIAO_PRODUTO.ID_PROCEDIMENTO, pROCEDIMENTO_REGIAO_PRODUTO.ID_REGIAO_CORPO, pROCEDIMENTO_REGIAO_PRODUTO.ID_PRODUTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROCEDIMENTO_REGIAO_PRODUTO.ID_PROCEDIMENTO }, pROCEDIMENTO_REGIAO_PRODUTO);
        }

        // DELETE: api/ProcedimentoRegiaoProduto/5
        [ResponseType(typeof(PROCEDIMENTO_REGIAO_PRODUTO))]
        [Route("api/ProcedimentoRegiaoMatMed/DeletePROCEDIMENTO_REGIAO_MATMED/{idProcedimento}/{idRegiao}/{idMatMed}")]
        public async Task<IHttpActionResult> DeletePROCEDIMENTO_REGIAO_PRODUTO(int idProcedimento, int idRegiao, int idMatMed)
        {
            PROCEDIMENTO_REGIAO_PRODUTO pROCEDIMENTO_REGIAO_PRODUTO = await db.PROCEDIMENTO_REGIAO_PRODUTO.Where(pp => pp.ID_PROCEDIMENTO == idProcedimento && pp.ID_REGIAO_CORPO == idRegiao && pp.ID_PRODUTO == idMatMed).FirstOrDefaultAsync();
            if (pROCEDIMENTO_REGIAO_PRODUTO == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM PROCEDIMENTO_REGIAO_PRODUTO WHERE ID_PROCEDIMENTO = {0} AND ID_REGIAO_CORPO = {1} AND ID_PRODUTO = {2}";

            if (db.Database.ExecuteSqlCommand(sql, idProcedimento, idRegiao, idMatMed) > 0)
                return Ok(pROCEDIMENTO_REGIAO_PRODUTO);
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

        private bool PROCEDIMENTO_REGIAO_PRODUTOExists(int idProcedimento, int idRegiao, int idProduto)
        {
            return db.PROCEDIMENTO_REGIAO_PRODUTO.Count(e => e.ID_PROCEDIMENTO == idProcedimento && e.ID_REGIAO_CORPO == idRegiao && e.ID_PRODUTO == idProduto) > 0;
        }
    }
}