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

namespace SisMedApi.Controllers
{
    public class ATENDIMENTO_PRODUTODTO
    {
        public int ID_ATENDIMENTO { get; set; }
        public int ID_PRODUTO { get; set; }
        public string LOGIN { get; set; }
        public System.Decimal QUANTIDADE { get; set; }
        public string UNIDADE { get; set; }
        public string OBS { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public string DESC_PRODUTO { get; set; }
        public string UNIDADE_PROD { get; set; }
        public string REGIAO { get; set; }
    }

    public class AtendimentoProdutoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/Atendimento/1/Produtos
        [Route("api/Atendimento/{idAtendimento}/Produtos")]
        public IEnumerable<ATENDIMENTO_PRODUTODTO> GetATENDIMENTO_PRODUTOS(int idAtendimento)
        {
            return (from ap in db.ATENDIMENTO_PRODUTO
                    join rc in db.REGIAO_CORPO on ap.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    join p in db.PRODUTOes on ap.ID_PRODUTO equals p.ID_PRODUTO                    
                    select new ATENDIMENTO_PRODUTODTO()
                    {
                        ID_ATENDIMENTO = ap.ID_ATENDIMENTO,
                        ID_PRODUTO = ap.ID_PRODUTO,
                        LOGIN = ap.LOGIN,
                        QUANTIDADE = ap.QUANTIDADE,
                        UNIDADE = ap.UNIDADE,
                        OBS = ap.OBS,
                        ID_REGIAO_CORPO = ap.ID_REGIAO_CORPO,
                        DESC_PRODUTO = p.DESC_PRODUTO,
                        UNIDADE_PROD = p.UNIDADE,
                        REGIAO = rc.REGIAO
                    }).Where(ap => ap.ID_ATENDIMENTO == idAtendimento).ToList();
        }

        // GET api/Atendimento/1/Produto/1/Regiao/1
        [Route("api/Atendimento/{idAtendimento}/Produto/{idProduto}/Regiao/{idRegiao}")]
        public IEnumerable<ATENDIMENTO_PRODUTODTO> GetATENDIMENTO_PRODUTO_REGIAO(int idAtendimento, int idProduto, int idRegiao)
        {
            return (from ap in db.ATENDIMENTO_PRODUTO
                    join rc in db.REGIAO_CORPO on ap.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO
                    join p in db.PRODUTOes on ap.ID_PRODUTO equals p.ID_PRODUTO
                    select new ATENDIMENTO_PRODUTODTO()
                    {
                        ID_ATENDIMENTO = ap.ID_ATENDIMENTO,
                        ID_PRODUTO = ap.ID_PRODUTO,
                        LOGIN = ap.LOGIN,
                        QUANTIDADE = ap.QUANTIDADE,
                        UNIDADE = ap.UNIDADE,
                        OBS = ap.OBS,
                        ID_REGIAO_CORPO = ap.ID_REGIAO_CORPO,
                        DESC_PRODUTO = p.DESC_PRODUTO,
                        UNIDADE_PROD = p.UNIDADE,
                        REGIAO = rc.REGIAO
                    }).Where(ap => ap.ID_ATENDIMENTO == idAtendimento && ap.ID_PRODUTO == idProduto && ap.ID_REGIAO_CORPO == idRegiao).ToList();
        }

        // PUT: api/AtendimentoProdutoRegiao/PutATENDIMENTO_PRODUTO_REGIAO/1/1/1
        [ResponseType(typeof(void))]
        [Route("api/AtendimentoProdutoRegiao/PutATENDIMENTO_PRODUTO_REGIAO/{idAtendimento}/{idProduto}/{idRegiao}")]
        public IHttpActionResult PutATENDIMENTO_PRODUTO(int idAtendimento, int idProduto, int idRegiao, ATENDIMENTO_PRODUTO aTENDIMENTO_PRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idAtendimento != aTENDIMENTO_PRODUTO.ID_ATENDIMENTO)
            {
                return BadRequest();
            }

            if (idProduto != aTENDIMENTO_PRODUTO.ID_PRODUTO)
            {
                return BadRequest();
            }

            if (idRegiao != aTENDIMENTO_PRODUTO.ID_REGIAO_CORPO)
            {
                return BadRequest();
            }

            db.Entry(aTENDIMENTO_PRODUTO).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ATENDIMENTO_PRODUTOExists(idAtendimento, idProduto, idRegiao))
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

        // POST: api/AtendimentoProduto
        [ResponseType(typeof(ATENDIMENTO_PRODUTO))]
        public async Task<IHttpActionResult> PostATENDIMENTO_PRODUTO(ATENDIMENTO_PRODUTO aTENDIMENTO_PRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ATENDIMENTO_PRODUTO.Add(aTENDIMENTO_PRODUTO);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ATENDIMENTO_PRODUTOExists(aTENDIMENTO_PRODUTO.ID_ATENDIMENTO, aTENDIMENTO_PRODUTO.ID_PRODUTO, aTENDIMENTO_PRODUTO.ID_REGIAO_CORPO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aTENDIMENTO_PRODUTO.ID_ATENDIMENTO }, aTENDIMENTO_PRODUTO);
        }

        // DELETE: api/AtendimentoMatMed/5
        [ResponseType(typeof(ATENDIMENTO_PRODUTO))]
        [Route("api/Atendimento/MatMed/Delete/{idAtendimento}/{idProduto}")]
        public async Task<IHttpActionResult> DeleteATENDIMENTO_MATMED(int idAtendimento, int idProduto)
        {
            ATENDIMENTO_PRODUTO aTENDIMENTO_PRODUTO = await db.ATENDIMENTO_PRODUTO.Where(ar => ar.ID_ATENDIMENTO == idAtendimento && ar.ID_PRODUTO == idProduto).FirstOrDefaultAsync();
            if (aTENDIMENTO_PRODUTO == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM ATENDIMENTO_PRODUTO WHERE ID_ATENDIMENTO = {0} AND ID_PRODUTO = {1}";

            if (db.Database.ExecuteSqlCommand(sql, idAtendimento, idProduto) > 0)
                return Ok(aTENDIMENTO_PRODUTO);
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

        private bool ATENDIMENTO_PRODUTOExists(int idAtendimento, int idProduto, int idRegiao)
        {
            return db.ATENDIMENTO_PRODUTO.Count(e => e.ID_ATENDIMENTO == idAtendimento && e.ID_PRODUTO == idProduto && e.ID_REGIAO_CORPO == idRegiao) > 0;
        }
    }
}