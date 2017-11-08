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

namespace SisMedApi.Controllers
{
    public class PRODUTO_FORNECEDORESDTO
    {
        public int ID_FORNECEDOR { get; set; }
        public int ID_PRODUTO { get; set; }
        public System.Decimal PRECO_MEDIO { get; set; }
        public string OBS { get; set; }
        public string DESC_PRODUTO { get; set; }
        public string NOME_FANTASIA { get; set; }
    }

    public class ProdutoFornecedoresController : ApiController
    {
        private SisMedContext db = new SisMedContext();


        // GET api/ProdutoFornecedores/Fornecedores/1
        [Route("api/ProdutoFornecedores/Fornecedores/{idProduto}")]
        public IEnumerable<PRODUTO_FORNECEDORESDTO> GetPRODUTO_FORNECEDORES(int idProduto)
        {
            return (from pf in db.PRODUTO_FORNECEDORES
                    join p in db.PRODUTOes on pf.ID_PRODUTO equals p.ID_PRODUTO
                    join f in db.FORNECEDORs on pf.ID_FORNECEDOR equals f.ID_FORNECEDOR
                    select new PRODUTO_FORNECEDORESDTO()
                    {
                        ID_FORNECEDOR = pf.ID_FORNECEDOR,
                        ID_PRODUTO = pf.ID_PRODUTO,
                        PRECO_MEDIO = pf.PRECO_MEDIO,
                        OBS = pf.OBS,
                        DESC_PRODUTO = p.DESC_PRODUTO,
                        NOME_FANTASIA = f.NOME_FANTASIA
                    }).Where(pf => pf.ID_PRODUTO == idProduto).ToList();
        }

        // GET api/ProdutoFornecedores/Produtos/1
        [Route("api/ProdutoFornecedores/Produtos/{idFornecedor}")]
        public IEnumerable<PRODUTO_FORNECEDORESDTO> GetFORNECEDOR_PRODUTOS(int idFornecedor)
        {
            return (from pf in db.PRODUTO_FORNECEDORES
                    join p in db.PRODUTOes on pf.ID_PRODUTO equals p.ID_PRODUTO
                    join f in db.FORNECEDORs on pf.ID_FORNECEDOR equals f.ID_FORNECEDOR
                    select new PRODUTO_FORNECEDORESDTO()
                    {
                        ID_FORNECEDOR = pf.ID_FORNECEDOR,
                        ID_PRODUTO = pf.ID_PRODUTO,
                        PRECO_MEDIO = pf.PRECO_MEDIO,
                        OBS = pf.OBS,
                        DESC_PRODUTO = p.DESC_PRODUTO,
                        NOME_FANTASIA = f.NOME_FANTASIA
                    }).Where(pf => pf.ID_FORNECEDOR == idFornecedor).ToList();
        }

        // GET api/ProdutoFornecedores/1/1
        [Route("api/ProdutoFornecedores/{idProduto}/{idFornecedor}")]
        public IEnumerable<PRODUTO_FORNECEDORESDTO> GetFORNECEDOR_FOR_PRODUTOS(int IdProduto, int idFornecedor)
        {
            return (from pf in db.PRODUTO_FORNECEDORES
                    join p in db.PRODUTOes on pf.ID_PRODUTO equals p.ID_PRODUTO
                    join f in db.FORNECEDORs on pf.ID_FORNECEDOR equals f.ID_FORNECEDOR
                    select new PRODUTO_FORNECEDORESDTO()
                    {
                        ID_FORNECEDOR = pf.ID_FORNECEDOR,
                        ID_PRODUTO = pf.ID_PRODUTO,
                        PRECO_MEDIO = pf.PRECO_MEDIO,
                        OBS = pf.OBS,
                        DESC_PRODUTO = p.DESC_PRODUTO,
                        NOME_FANTASIA = f.NOME_FANTASIA
                    }).Where(pf => pf.ID_FORNECEDOR == idFornecedor && pf.ID_PRODUTO == IdProduto).ToList();
        }

        // PUT: api/ProdutoFornecedores/5
        [ResponseType(typeof(void))]
        [Route("api/ProdutoFornecedores/PutPRODUTO_FORNECEDORES/{idProduto}/{idFornecedor}")]
        public IHttpActionResult PutPRODUTO_FORNECEDORES(int idProduto, int idFornecedor, PRODUTO_FORNECEDORES pRODUTO_FORNECEDORES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idProduto != pRODUTO_FORNECEDORES.ID_PRODUTO)
            {
                return BadRequest();
            }

            if (idFornecedor != pRODUTO_FORNECEDORES.ID_FORNECEDOR)
            {
                return BadRequest();
            }

            db.Entry(pRODUTO_FORNECEDORES).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PRODUTO_FORNECEDORESExists(idProduto, idFornecedor))
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

        // POST: api/ProdutoFornecedores
        [ResponseType(typeof(PRODUTO_FORNECEDORES))]
        public IHttpActionResult PostPRODUTO_FORNECEDORES(PRODUTO_FORNECEDORES pRODUTO_FORNECEDORES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PRODUTO_FORNECEDORES.Add(pRODUTO_FORNECEDORES);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PRODUTO_FORNECEDORESExists(pRODUTO_FORNECEDORES.ID_PRODUTO, pRODUTO_FORNECEDORES.ID_FORNECEDOR))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { idProduto = pRODUTO_FORNECEDORES.ID_PRODUTO, idFornecedor = pRODUTO_FORNECEDORES.ID_FORNECEDOR }, pRODUTO_FORNECEDORES);
        }

        [ResponseType(typeof(PRODUTO_FORNECEDORES))]
        [Route("api/ProdutoFornecedor/DeletePRODUTO_FORNECEDOR/produto/{idProduto}/fornecedor/{idFornecedor}")]
        public IHttpActionResult DeletePRODUTO_FORNECEDOR(int idProduto, int idFornecedor)
        {
            PRODUTO_FORNECEDORES pRODUTO_FORNECEDORES = db.PRODUTO_FORNECEDORES.Where(pf => pf.ID_PRODUTO == idProduto && pf.ID_FORNECEDOR == idFornecedor).FirstOrDefault();
            if (pRODUTO_FORNECEDORES == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM PRODUTO_FORNECEDORES WHERE ID_PRODUTO = {0} AND ID_FORNECEDOR = {1}";

            if (db.Database.ExecuteSqlCommand(sql, idProduto, idFornecedor) > 0)
                return Ok(pRODUTO_FORNECEDORES);
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

        private bool PRODUTO_FORNECEDORESExists(int idProduto, int idFornecedor)
        {
            return db.PRODUTO_FORNECEDORES.Count(e => e.ID_PRODUTO == idProduto && e.ID_FORNECEDOR == idFornecedor) > 0;
        }
    }
}