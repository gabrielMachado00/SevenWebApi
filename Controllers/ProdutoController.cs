using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;

namespace SisMedApi.Controllers
{
    public class ProdutoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        public class PRODUTODTO
        {
            public int ID_PRODUTO { get; set; }
            public string DESC_PRODUTO { get; set; }
            public string MARCA { get; set; }
            public int ID_TIPO_PRODUTO { get; set; }
            public decimal PRECO_REAL { get; set; }
            public decimal PRECO_CUSTO { get; set; }
            public DateTime DATA_CADASTRO { get; set; }
            public string DESATIVADO { get; set; }
            public string DESC_DESATIVADO { get; set; }
            public string OBS { get; set; }
            public string UNIDADE { get; set; }
            public decimal QUANTIDADE_MINIMA { get; set; }
            public string DESC_TIPO_PRODUTO { get; set; }
            public Nullable<decimal> FATOR_CONVERSAO { get; set; }
            public Nullable<int> ID_UNIDADE { get; set; }
        }

        // GET: api/Produto
        public IEnumerable<PRODUTODTO> GetPRODUTOes()
        {
            return (from p in db.PRODUTOes
                    join tp in db.TIPO_PRODUTO on p.ID_TIPO_PRODUTO equals tp.ID_TIPO_PRODUTO
                    join u in db.UNIDADE_PRODUTO on p.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    select new PRODUTODTO()
                    {
                        ID_PRODUTO = p.ID_PRODUTO,
                        DESC_PRODUTO = p.DESC_PRODUTO,
                        MARCA = p.MARCA,
                        ID_TIPO_PRODUTO = p.ID_TIPO_PRODUTO,
                        PRECO_REAL = p.PRECO_REAL,
                        PRECO_CUSTO = p.PRECO_CUSTO,
                        DATA_CADASTRO = p.DATA_CADASTRO,
                        DESATIVADO = p.DESATIVADO,
                        DESC_DESATIVADO = p.DESATIVADO == "0" ? "ATIVO" :
                                          p.DESATIVADO == "1" ? "DESATIVADO":"",
                        OBS = p.OBS,
                        UNIDADE = u.DESCRICAO,
                        QUANTIDADE_MINIMA = p.QUANTIDADE_MINIMA,
                        DESC_TIPO_PRODUTO = tp.DESC_TIPO_PRODUTO,
                        FATOR_CONVERSAO = p.FATOR_CONVERSAO,
                        ID_UNIDADE = p.ID_UNIDADE
                    });
         //   return db.PRODUTOes;
        }

        // GET: api/Produto/5
        [ResponseType(typeof(PRODUTO))]
        public IHttpActionResult GetPRODUTO(int id)
        {
            PRODUTO pRODUTO = db.PRODUTOes.Find(id);
            if (pRODUTO == null)
            {
                return NotFound();
            }

            return Ok(pRODUTO);
        }

        // PUT: api/Produto/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPRODUTO(int id, PRODUTO pRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pRODUTO.ID_PRODUTO)
            {
                return BadRequest();
            }

            db.Entry(pRODUTO).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PRODUTOExists(id))
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

        // POST: api/Produto
        [ResponseType(typeof(PRODUTO))]
        public IHttpActionResult PostPRODUTO(PRODUTO pRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_PRODUTO;").First();

            pRODUTO.ID_PRODUTO = NextValue;

            db.PRODUTOes.Add(pRODUTO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PRODUTOExists(pRODUTO.ID_PRODUTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pRODUTO.ID_PRODUTO }, pRODUTO);
        }

        // DELETE: api/Produto/5
        [ResponseType(typeof(PRODUTO))]
        public IHttpActionResult DeletePRODUTO(int id)
        {
            PRODUTO pRODUTO = db.PRODUTOes.Find(id);
            if (pRODUTO == null)
            {
                return NotFound();
            }

            db.PRODUTOes.Remove(pRODUTO);
            db.SaveChanges();

            return Ok(pRODUTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PRODUTOExists(int id)
        {
            return db.PRODUTOes.Count(e => e.ID_PRODUTO == id) > 0;
        }
    }
}