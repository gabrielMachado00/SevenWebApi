using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SevenMedicalApi.Models;
using SisMedApi.Models;
using System;
using System.Data.SqlClient;

namespace SevenMedicalApi.Controllers
{
    public class FARMACIA_ESTOQUEDTO
    {
        public int ID_FARMACIA { get; set; }
        public int ID_PRODUTO { get; set; }
        public decimal QUANTIDADE { get; set; }
        public string FARMACIA { get; set; }
        public string PRODUTO { get; set; }
        public Nullable<int> ID_UNIDADE { get; set; }
        public string UNIDADE { get; set; }
    }
    public class FarmaciaEstoqueController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [Route("api/farmacia/estoque/unidade/{idUnidade}")]
        public IEnumerable<FARMACIA_ESTOQUEDTO> GetFARMACIAs_UNIDADE_ESTOQUE(int idUnidade)
        {
            return (from fe in db.FARMACIA_ESTOQUE
                    join f in db.FARMACIA_SATELITE on fe.ID_FARMACIA equals f.ID_FARMACIA
                    join p in db.PRODUTOes on fe.ID_PRODUTO equals p.ID_PRODUTO
                    join u in db.UNIDADEs on f.ID_UNIDADE equals u.ID_UNIDADE
                    select new FARMACIA_ESTOQUEDTO()
                    {
                        ID_FARMACIA = fe.ID_FARMACIA,
                        ID_PRODUTO = fe.ID_PRODUTO,
                        QUANTIDADE = fe.QUANTIDADE,
                        FARMACIA = f.DESCRICAO,
                        PRODUTO = p.DESC_PRODUTO,
                        ID_UNIDADE = f.ID_UNIDADE,
                        UNIDADE = u.NOME_FANTASIA
                    }).Where(fe => fe.ID_UNIDADE == idUnidade).ToList();
        }

        [Route("api/farmacia/estoque/unidade/{idUnidade}/produto/{idProduto}")]
        public IEnumerable<FARMACIA_ESTOQUEDTO> GetFARMACIAs_UNIDADE_ESTOQUE(int idUnidade, int idProduto)
        {
            return (from fe in db.FARMACIA_ESTOQUE
                    join f in db.FARMACIA_SATELITE on fe.ID_FARMACIA equals f.ID_FARMACIA
                    join p in db.PRODUTOes on fe.ID_PRODUTO equals p.ID_PRODUTO
                    join u in db.UNIDADEs on f.ID_UNIDADE equals u.ID_UNIDADE
                    select new FARMACIA_ESTOQUEDTO()
                    {
                        ID_FARMACIA = fe.ID_FARMACIA,
                        ID_PRODUTO = fe.ID_PRODUTO,
                        QUANTIDADE = fe.QUANTIDADE,
                        FARMACIA = f.DESCRICAO,
                        PRODUTO = p.DESC_PRODUTO,
                        ID_UNIDADE = f.ID_UNIDADE,
                        UNIDADE = u.NOME_FANTASIA
                    }).Where(fe => fe.ID_UNIDADE == idUnidade && fe.ID_PRODUTO == idProduto).ToList();
        }

        [Route("api/farmacia/{idFarmacia}/estoque")]
        public IEnumerable<FARMACIA_ESTOQUEDTO> GetFARMACIAs_ESTOQUE(int idFarmacia)
        {
            return (from fe in db.FARMACIA_ESTOQUE
                    join f in db.FARMACIA_SATELITE on fe.ID_FARMACIA equals f.ID_FARMACIA
                    join p in db.PRODUTOes on fe.ID_PRODUTO equals p.ID_PRODUTO
                    join u in db.UNIDADEs on f.ID_UNIDADE equals u.ID_UNIDADE
                    select new FARMACIA_ESTOQUEDTO()
                    {
                        ID_FARMACIA = fe.ID_FARMACIA,
                        ID_PRODUTO = fe.ID_PRODUTO,
                        QUANTIDADE = fe.QUANTIDADE,
                        FARMACIA = f.DESCRICAO,
                        PRODUTO = p.DESC_PRODUTO,
                        ID_UNIDADE = f.ID_UNIDADE,
                        UNIDADE = u.NOME_FANTASIA
                    }).Where(fe => fe.ID_FARMACIA == idFarmacia).ToList();
        }

        [Route("api/farmacia/{idFarmacia}/estoque/{idProduto}", Name = "GetEstoqueProdutoFarmacia")]
        public IEnumerable<FARMACIA_ESTOQUEDTO> GetFARMACIAs_ESTOQUE(int idFarmacia, int idProduto)
        {
            return (from fe in db.FARMACIA_ESTOQUE
                    join f in db.FARMACIA_SATELITE on fe.ID_FARMACIA equals f.ID_FARMACIA
                    join p in db.PRODUTOes on fe.ID_PRODUTO equals p.ID_PRODUTO
                    join u in db.UNIDADEs on f.ID_UNIDADE equals u.ID_UNIDADE
                    select new FARMACIA_ESTOQUEDTO()
                    {
                        ID_FARMACIA = fe.ID_FARMACIA,
                        ID_PRODUTO = fe.ID_PRODUTO,
                        QUANTIDADE = fe.QUANTIDADE,
                        FARMACIA = f.DESCRICAO,
                        PRODUTO = p.DESC_PRODUTO,
                        ID_UNIDADE = f.ID_UNIDADE,
                        UNIDADE = u.NOME_FANTASIA
                    }).Where(fe => fe.ID_FARMACIA == idFarmacia && fe.ID_PRODUTO == idProduto).ToList();
        }

        [Route("api/farmacia/{idFarmacia}/estoque/{idProduto}/unidade/{idUnidade}")]
        public IEnumerable<FARMACIA_ESTOQUEDTO> GetFARMACIAs_ESTOQUE(int idFarmacia, int idProduto, int idUnidade)
        {
            string sql = string.Format(@"SELECT FE.ID_FARMACIA, FE.ID_PRODUTO, FE.QUANTIDADE, F.DESCRICAO FARMACIA, P.DESC_PRODUTO PRODUTO,
                                                FE.ID_UNIDADE, U.NOME_FANTASIA UNIDADE
                                           FROM FARMACIA_ESTOQUE FE, FARMACIA_SATELITE F, PRODUTO P, UNIDADE U
                                          WHERE P.ID_PRODUTO = FE.ID_PRODUTO
                                            and u.ID_UNIDADE = fe.ID_UNIDADE
                                            and fe.ID_FARMACIA = f.ID_FARMACIA
                                            AND fe.ID_UNIDADE = {0}
                                            and fe.ID_PRODUTO = {1}
                                            and FE.ID_FARMACIA = {2}", idUnidade, idProduto, idFarmacia);           
            return db.Database.SqlQuery<FARMACIA_ESTOQUEDTO>(sql).ToList();
        }


        // PUT: api/FarmaciaEstoque/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFARMACIA_ESTOQUE(int id, FARMACIA_ESTOQUE fARMACIA_ESTOQUE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fARMACIA_ESTOQUE.ID_FARMACIA)
            {
                return BadRequest();
            }

            db.Entry(fARMACIA_ESTOQUE).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FARMACIA_ESTOQUEExists(id))
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

        // POST: api/FarmaciaEstoque
        [ResponseType(typeof(FARMACIA_ESTOQUE))]
        public IHttpActionResult PostFARMACIA_ESTOQUE(FARMACIA_ESTOQUE fARMACIA_ESTOQUE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FARMACIA_ESTOQUE.Add(fARMACIA_ESTOQUE);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FARMACIA_ESTOQUEExists(fARMACIA_ESTOQUE.ID_FARMACIA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = fARMACIA_ESTOQUE.ID_FARMACIA }, fARMACIA_ESTOQUE);
        }

        // DELETE: api/FarmaciaEstoque/5
        [ResponseType(typeof(FARMACIA_ESTOQUE))]
        [Route("api/farmaciaestoque/Delete/{idFarmacia}/{idProduto}/{idUnidade}")]
        public IHttpActionResult DeleteFARMACIA_ESTOQUE(int idFarmacia, int idProduto, int idUnidade)
        {
            FARMACIA_ESTOQUE fARMACIA_ESTOQUE = db.FARMACIA_ESTOQUE.Where(fe => fe.ID_FARMACIA == idFarmacia && fe.ID_PRODUTO == idProduto && fe.ID_UNIDADE == idUnidade).FirstOrDefault();
            if (fARMACIA_ESTOQUE == null)
            {
                return NotFound();
            }

            var sql = @" DELETE FROM FARMACIA_ESTOQUE WHERE ID_FARMACIA = {0} AND ID_PRODUTO = {1} AND ID_UNIDADE = {2} ";

            if (db.Database.ExecuteSqlCommand(sql, idFarmacia, idProduto, idUnidade) > 0)
                return Ok(fARMACIA_ESTOQUE);
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

        private bool FARMACIA_ESTOQUEExists(int id)
        {
            return db.FARMACIA_ESTOQUE.Count(e => e.ID_FARMACIA == id) > 0;
        }
    }
}