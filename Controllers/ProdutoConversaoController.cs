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

namespace SevenMedicalApi.Controllers
{
    public class PRODUTO_CONVERSAODTO
    {
        public int ID_CONVERSAO { get; set; }
        public int ID_PRODUTO { get; set; }
        public string UNIDADE { get; set; }
        public decimal VALOR { get; set; }
        public string PRODUTO { get; set; }
        public Nullable<int> ID_UNIDADE { get; set; }
    }
    public class ProdutoConversaoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [Route("api/produto/{idProduto}/conversao")]
        public IEnumerable<PRODUTO_CONVERSAODTO> GetPRODUTO_CONVERSAO(int idProduto)
        {
            return (from pc in db.PRODUTO_CONVERSAO
                    join p in db.PRODUTOes on pc.ID_PRODUTO equals p.ID_PRODUTO
                    join u in db.UNIDADE_PRODUTO on pc.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    select new PRODUTO_CONVERSAODTO()
                    {
                        ID_CONVERSAO = pc.ID_CONVERSAO,
                        ID_PRODUTO = pc.ID_PRODUTO,
                        UNIDADE = u.DESCRICAO,
                        VALOR = pc.VALOR,
                        PRODUTO = p.DESC_PRODUTO,
                        ID_UNIDADE = pc.ID_UNIDADE
                    }).Where(pc => pc.ID_PRODUTO == idProduto).ToList();
        }

        [Route("api/produto/{idProduto}/conversao/{idConversao}")]
        public IEnumerable<PRODUTO_CONVERSAODTO> GetPRODUTO_CONVERSAO(int idProduto, int idConversao)
        {
            return (from pc in db.PRODUTO_CONVERSAO
                    join p in db.PRODUTOes on pc.ID_PRODUTO equals p.ID_PRODUTO
                    join u in db.UNIDADE_PRODUTO on pc.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    select new PRODUTO_CONVERSAODTO()
                    {
                        ID_CONVERSAO = pc.ID_CONVERSAO,
                        ID_PRODUTO = pc.ID_PRODUTO,
                        UNIDADE = u.DESCRICAO,
                        VALOR = pc.VALOR,
                        PRODUTO = p.DESC_PRODUTO,
                        ID_UNIDADE = p.ID_UNIDADE
                    }).Where(pc => pc.ID_PRODUTO == idProduto && pc.ID_CONVERSAO == idConversao).ToList();
        }

        // PUT: api/ProdutoConversao/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPRODUTO_CONVERSAO(int id, PRODUTO_CONVERSAO pRODUTO_CONVERSAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pRODUTO_CONVERSAO.ID_CONVERSAO)
            {
                return BadRequest();
            }

            db.Entry(pRODUTO_CONVERSAO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PRODUTO_CONVERSAOExists(id))
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

        // POST: api/ProdutoConversao
        [ResponseType(typeof(PRODUTO_CONVERSAO))]
        public IHttpActionResult PostPRODUTO_CONVERSAO(PRODUTO_CONVERSAO pRODUTO_CONVERSAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_CONVERSAO;").First();

            pRODUTO_CONVERSAO.ID_CONVERSAO = NextValue;

            db.PRODUTO_CONVERSAO.Add(pRODUTO_CONVERSAO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PRODUTO_CONVERSAOExists(pRODUTO_CONVERSAO.ID_CONVERSAO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pRODUTO_CONVERSAO.ID_CONVERSAO }, pRODUTO_CONVERSAO);
        }

        // DELETE: api/ProdutoConversao/5
        [ResponseType(typeof(PRODUTO_CONVERSAO))]
        public IHttpActionResult DeletePRODUTO_CONVERSAO(int id)
        {
            PRODUTO_CONVERSAO pRODUTO_CONVERSAO = db.PRODUTO_CONVERSAO.Find(id);
            if (pRODUTO_CONVERSAO == null)
            {
                return NotFound();
            }

            db.PRODUTO_CONVERSAO.Remove(pRODUTO_CONVERSAO);
            db.SaveChanges();

            return Ok(pRODUTO_CONVERSAO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PRODUTO_CONVERSAOExists(int id)
        {
            return db.PRODUTO_CONVERSAO.Count(e => e.ID_CONVERSAO == id) > 0;
        }
    }
}