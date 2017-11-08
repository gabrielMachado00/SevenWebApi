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
    public class ENTRADA_PRODUTODTO
    {
        public int ID_ENTRADA { get; set; }
        public int ID_FORNECEDOR { get; set; }
        public int ID_UNIDADE { get; set; }
        public DateTime DATA_ENTRADA { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public string FORNECEDOR { get; set; }
        public string UNIDADE { get; set; }
        public string STATUS { get; set; }
        public string DESC_STATUS { get; set; }
        public string LOGIN { get; set; }
    }
    public class EntradaProdutoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [Route("api/entrada/{status}/{dtInicial}/{dtFinal}")]
        public IEnumerable<ENTRADA_PRODUTODTO> GetENTREDAs(string status, DateTime? dtInicial, DateTime? dtFinal)
        {
            return (from ep in db.ENTRADA_PRODUTO
                    join f in db.FORNECEDORs on ep.ID_FORNECEDOR equals f.ID_FORNECEDOR
                    join u in db.UNIDADEs on ep.ID_UNIDADE equals u.ID_UNIDADE
                    select new ENTRADA_PRODUTODTO()
                    {
                        ID_ENTRADA = ep.ID_ENTRADA,
                        ID_FORNECEDOR = ep.ID_FORNECEDOR,
                        ID_UNIDADE = ep.ID_UNIDADE,
                        DATA_ENTRADA = ep.DATA_ENTRADA,
                        NUMERO_DOCUMENTO = ep.NUMERO_DOCUMENTO,
                        FORNECEDOR = f.NOME_FANTASIA,
                        UNIDADE = u.NOME_FANTASIA,
                        STATUS = ep.STATUS,
                        DESC_STATUS = ep.STATUS == "I" ? "INICIADO" :
                                      ep.STATUS == "L" ? "CONCLUÍDO" :
                                      ep.STATUS == "C" ? "CANCELADO" : "",
                        LOGIN = ep.LOGIN
                    }).Where(ep => ep.STATUS == status && DbFunctions.TruncateTime(ep.DATA_ENTRADA) >= DbFunctions.TruncateTime(dtInicial) &&
                                    DbFunctions.TruncateTime(ep.DATA_ENTRADA) <= DbFunctions.TruncateTime(dtFinal)).ToList();
        }

        [Route("api/entrada/login/{login}/{status}/{dtInicial}/{dtFinal}")]
        public IEnumerable<ENTRADA_PRODUTODTO> GetENTREDAs(string login, string status, DateTime? dtInicial, DateTime? dtFinal)
        {
            return (from ep in db.ENTRADA_PRODUTO
                    join f in db.FORNECEDORs on ep.ID_FORNECEDOR equals f.ID_FORNECEDOR
                    join u in db.UNIDADEs on ep.ID_UNIDADE equals u.ID_UNIDADE
                    select new ENTRADA_PRODUTODTO()
                    {
                        ID_ENTRADA = ep.ID_ENTRADA,
                        ID_FORNECEDOR = ep.ID_FORNECEDOR,
                        ID_UNIDADE = ep.ID_UNIDADE,
                        DATA_ENTRADA = ep.DATA_ENTRADA,
                        NUMERO_DOCUMENTO = ep.NUMERO_DOCUMENTO,
                        FORNECEDOR = f.NOME_FANTASIA,
                        UNIDADE = u.NOME_FANTASIA,
                        STATUS = ep.STATUS,
                        DESC_STATUS = ep.STATUS == "I" ? "INICIADO" :
                                      ep.STATUS == "L" ? "CONCLUÍDO" :
                                      ep.STATUS == "C" ? "CANCELADO" : "",
                        LOGIN = ep.LOGIN
                    }).Where(ep => ep.LOGIN.ToUpper() == login.ToUpper() && ep.STATUS == status && DbFunctions.TruncateTime(ep.DATA_ENTRADA) >= DbFunctions.TruncateTime(dtInicial) &&
                                    DbFunctions.TruncateTime(ep.DATA_ENTRADA) <= DbFunctions.TruncateTime(dtFinal)).ToList();
        }

        [Route("api/entrada/{idEntrada}")]
        public IEnumerable<ENTRADA_PRODUTODTO> GetENTREDAs(int idEntrada)
        {
            return (from ep in db.ENTRADA_PRODUTO
                    join f in db.FORNECEDORs on ep.ID_FORNECEDOR equals f.ID_FORNECEDOR
                    join u in db.UNIDADEs on ep.ID_UNIDADE equals u.ID_UNIDADE
                    select new ENTRADA_PRODUTODTO()
                    {
                        ID_ENTRADA = ep.ID_ENTRADA,
                        ID_FORNECEDOR = ep.ID_FORNECEDOR,
                        ID_UNIDADE = ep.ID_UNIDADE,
                        DATA_ENTRADA = ep.DATA_ENTRADA,
                        NUMERO_DOCUMENTO = ep.NUMERO_DOCUMENTO,
                        FORNECEDOR = f.NOME_FANTASIA,
                        UNIDADE = u.NOME_FANTASIA,
                        STATUS = ep.STATUS,
                        DESC_STATUS = ep.STATUS == "I" ? "INICIADO" :
                                      ep.STATUS == "L" ? "CONCLUÍDO" :
                                      ep.STATUS == "C" ? "CANCELADO" : "",
                        LOGIN = ep.LOGIN
                    }).Where(ep => ep.ID_ENTRADA == idEntrada).ToList();
        }

        // PUT: api/EntradaProduto/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutENTRADA_PRODUTO(int id, ENTRADA_PRODUTO eNTRADA_PRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eNTRADA_PRODUTO.ID_ENTRADA)
            {
                return BadRequest();
            }

            db.Entry(eNTRADA_PRODUTO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ENTRADA_PRODUTOExists(id))
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

        // POST: api/EntradaProduto
        [ResponseType(typeof(ENTRADA_PRODUTO))]
        public IHttpActionResult PostENTRADA_PRODUTO(ENTRADA_PRODUTO eNTRADA_PRODUTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ENTRADA;").First();

            eNTRADA_PRODUTO.ID_ENTRADA = NextValue;

            db.ENTRADA_PRODUTO.Add(eNTRADA_PRODUTO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ENTRADA_PRODUTOExists(eNTRADA_PRODUTO.ID_ENTRADA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = eNTRADA_PRODUTO.ID_ENTRADA }, eNTRADA_PRODUTO);
        }

        // DELETE: api/EntradaProduto/5
        [ResponseType(typeof(ENTRADA_PRODUTO))]
        public IHttpActionResult DeleteENTRADA_PRODUTO(int id)
        {
            ENTRADA_PRODUTO eNTRADA_PRODUTO = db.ENTRADA_PRODUTO.Find(id);
            if (eNTRADA_PRODUTO == null)
            {
                return NotFound();
            }

            db.ENTRADA_PRODUTO.Remove(eNTRADA_PRODUTO);
            db.SaveChanges();

            return Ok(eNTRADA_PRODUTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ENTRADA_PRODUTOExists(int id)
        {
            return db.ENTRADA_PRODUTO.Count(e => e.ID_ENTRADA == id) > 0;
        }
    }
}