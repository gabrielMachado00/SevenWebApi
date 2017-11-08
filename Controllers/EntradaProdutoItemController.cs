using System;
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

namespace SevenMedicalApi.Controllers
{
    public class ENTRADA_PRODUTO_ITEMDTO
    {
        public int ID_ITEM_ENTRADA { get; set; }
        public int ID_ENTRADA { get; set; }
        public int ID_PRODUTO { get; set; }
        public Nullable<int> ID_FARMACIA { get; set; }
        public decimal QUANTIDADE { get; set; }
        public decimal VALOR { get; set; }
        public string UNIDADE_ENTRADA { get; set; }
        public string PRODUTO { get; set; }
        public string FARMACIA { get; set; }
        public Nullable<int> ID_UNIDADE { get; set; }
    }

    public class ENTRADA_ESTOQUE_DOCUMENTO
    {
        public int ID_ENTRADA { get; set; }
        public int ID_FORNECEDOR { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public DateTime DATA_ENTRADA { get; set; }
        public decimal VALOR_TOTAL { get; set; }
        public string FORNECEDOR { get; set; }
    }

    public class EntradaProdutoItemController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [Route("api/entrada/{idEntrada}/item")]
        public IEnumerable<ENTRADA_PRODUTO_ITEMDTO> GetENTREDA_ITENs(int idEntrada)
        {
            return (from epi in db.ENTRADA_PRODUTO_ITEM
                    join p in db.PRODUTOes on epi.ID_PRODUTO equals p.ID_PRODUTO
                    join f in db.FARMACIA_SATELITE on epi.ID_FARMACIA equals f.ID_FARMACIA into _f
                    from f in _f.DefaultIfEmpty()
                    join u in db.UNIDADE_PRODUTO on epi.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    select new ENTRADA_PRODUTO_ITEMDTO()
                    {
                        ID_ITEM_ENTRADA = epi.ID_ITEM_ENTRADA,
                        ID_ENTRADA = epi.ID_ENTRADA,
                        ID_PRODUTO = epi.ID_PRODUTO,
                        ID_FARMACIA = epi.ID_FARMACIA,
                        QUANTIDADE = epi.QUANTIDADE,
                        VALOR = epi.VALOR,
                        UNIDADE_ENTRADA = u.DESCRICAO,
                        PRODUTO = p.DESC_PRODUTO,
                        FARMACIA = f.DESCRICAO,
                        ID_UNIDADE = epi.ID_UNIDADE
                    }).Where(epi => epi.ID_ENTRADA == idEntrada).ToList();
        }

        [Route("api/entrada/{idEntrada}/item/{idItem}")]
        public IEnumerable<ENTRADA_PRODUTO_ITEMDTO> GetENTREDA_ITENs(int idEntrada, int idItem)
        {
            return (from epi in db.ENTRADA_PRODUTO_ITEM
                    join p in db.PRODUTOes on epi.ID_PRODUTO equals p.ID_PRODUTO
                    join f in db.FARMACIA_SATELITE on epi.ID_FARMACIA equals f.ID_FARMACIA into _f
                    from f in _f.DefaultIfEmpty()
                    join u in db.UNIDADE_PRODUTO on epi.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    select new ENTRADA_PRODUTO_ITEMDTO()
                    {
                        ID_ITEM_ENTRADA = epi.ID_ITEM_ENTRADA,
                        ID_ENTRADA = epi.ID_ENTRADA,
                        ID_PRODUTO = epi.ID_PRODUTO,
                        ID_FARMACIA = epi.ID_FARMACIA,
                        QUANTIDADE = epi.QUANTIDADE,
                        VALOR = epi.VALOR,
                        UNIDADE_ENTRADA = u.DESCRICAO,
                        PRODUTO = p.DESC_PRODUTO,
                        FARMACIA = f.DESCRICAO,
                        ID_UNIDADE = epi.ID_UNIDADE
                    }).Where(epi => epi.ID_ENTRADA == idEntrada && epi.ID_ITEM_ENTRADA == idItem).ToList();
        }

        [Route("api/entradaProdutoItem/documento/{idEntrada}")]
        public IEnumerable<ENTRADA_ESTOQUE_DOCUMENTO> GetENTRADA_ESTOQUE_DOCUMENTO(int idEntrada)
        {
            string sql = string.Format(@"SELECT E.ID_ENTRADA, E.ID_FORNECEDOR, DATA_ENTRADA, NUMERO_DOCUMENTO, V.VALOR_TOTAL, F.NOME_FANTASIA FORNECEDOR
                                           FROM ENTRADA_PRODUTO E, (SELECT ID_ENTRADA, SUM(VALOR) VALOR_TOTAL 
							                                          FROM ENTRADA_PRODUTO_ITEM 
						                                             WHERE ID_ENTRADA = {0}
							                                         GROUP BY ID_ENTRADA) V, FORNECEDOR F
                                          WHERE F.ID_FORNECEDOR = E.ID_FORNECEDOR
                                            AND E.ID_ENTRADA = V.ID_ENTRADA
                                            AND E.ID_ENTRADA = {1}", idEntrada, idEntrada);
            return db.Database.SqlQuery<ENTRADA_ESTOQUE_DOCUMENTO>(sql).ToList();
        }

        // PUT: api/EntradaProdutoItem/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutENTRADA_PRODUTO_ITEM(int id, ENTRADA_PRODUTO_ITEM eNTRADA_PRODUTO_ITEM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eNTRADA_PRODUTO_ITEM.ID_ITEM_ENTRADA)
            {
                return BadRequest();
            }

            db.Entry(eNTRADA_PRODUTO_ITEM).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ENTRADA_PRODUTO_ITEMExists(id))
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

        // POST: api/EntradaProdutoItem
        [ResponseType(typeof(ENTRADA_PRODUTO_ITEM))]
        public IHttpActionResult PostENTRADA_PRODUTO_ITEM(ENTRADA_PRODUTO_ITEM eNTRADA_PRODUTO_ITEM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ENTRADA_ITEM;").First();

            eNTRADA_PRODUTO_ITEM.ID_ITEM_ENTRADA = NextValue;

            db.ENTRADA_PRODUTO_ITEM.Add(eNTRADA_PRODUTO_ITEM);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ENTRADA_PRODUTO_ITEMExists(eNTRADA_PRODUTO_ITEM.ID_ITEM_ENTRADA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = eNTRADA_PRODUTO_ITEM.ID_ITEM_ENTRADA }, eNTRADA_PRODUTO_ITEM);
        }

        // DELETE: api/EntradaProdutoItem/5
        [ResponseType(typeof(ENTRADA_PRODUTO_ITEM))]
        public IHttpActionResult DeleteENTRADA_PRODUTO_ITEM(int id)
        {
            ENTRADA_PRODUTO_ITEM eNTRADA_PRODUTO_ITEM = db.ENTRADA_PRODUTO_ITEM.Find(id);
            if (eNTRADA_PRODUTO_ITEM == null)
            {
                return NotFound();
            }

            db.ENTRADA_PRODUTO_ITEM.Remove(eNTRADA_PRODUTO_ITEM);
            db.SaveChanges();

            return Ok(eNTRADA_PRODUTO_ITEM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ENTRADA_PRODUTO_ITEMExists(int id)
        {
            return db.ENTRADA_PRODUTO_ITEM.Count(e => e.ID_ITEM_ENTRADA == id) > 0;
        }
    }
}