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
    public class FORNECEDOR_ENDERECODTO
    {
        public int ID_ENDERECO { get; set; }
        public int ID_FORNECEDOR { get; set; }
        public string ENDERECO { get; set; }
        public string BAIRRO { get; set; }
        public string CEP { get; set; }
        public int ID_CIDADE { get; set; }
        public string TIPO_ENDERECO { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
    }

    public class FornecedorEnderecoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/fornecedor/1/endereco
        [Route("api/fornecedor/{idFornecedor}/endereco")]
        public IEnumerable<FORNECEDOR_ENDERECODTO> GetFORNECEDOR_ENDERECO(int idFornecedor)
        {
            return (from ce in db.FORNECEDOR_ENDERECO
                    join ci in db.CIDADEs on ce.ID_CIDADE equals ci.ID_CIDADE
                    select new FORNECEDOR_ENDERECODTO()
                    {
                        ID_ENDERECO = ce.ID_ENDERECO,
                        ID_FORNECEDOR = ce.ID_FORNECEDOR,
                        BAIRRO = ce.BAIRRO,
                        CEP = ce.CEP,
                        ENDERECO = ce.ENDERECO,
                        ID_CIDADE = ce.ID_CIDADE,
                        TIPO_ENDERECO = ce.TIPO_ENDERECO,
                        CIDADE = ci.NOME,
                        UF = ci.UF
                    }).Where(ce => ce.ID_FORNECEDOR == idFornecedor).ToList();
        }

        // GET api/fornecedor/endereco/5
        [Route("api/fornecedor/endereco/{id}")]
        public FORNECEDOR_ENDERECODTO GetENDERECO(int id)
        {
            return (from ce in db.FORNECEDOR_ENDERECO
                    join ci in db.CIDADEs on ce.ID_CIDADE equals ci.ID_CIDADE
                    select new FORNECEDOR_ENDERECODTO()
                    {
                        ID_ENDERECO = ce.ID_ENDERECO,
                        ID_FORNECEDOR = ce.ID_FORNECEDOR,
                        BAIRRO = ce.BAIRRO,
                        CEP = ce.CEP,
                        ENDERECO = ce.ENDERECO,
                        ID_CIDADE = ce.ID_CIDADE,
                        TIPO_ENDERECO = ce.TIPO_ENDERECO,
                        CIDADE = ci.NOME,
                        UF = ci.UF
                    }).Where(ce => ce.ID_ENDERECO == id).First();

            //if (fornecedor_endereco == null)
            //{
            //    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            //}

            //return fornecedor_endereco;
        }

        // PUT: api/FornecedorEndereco/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFORNECEDOR_ENDERECO(int id, FORNECEDOR_ENDERECO fORNECEDOR_ENDERECO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fORNECEDOR_ENDERECO.ID_ENDERECO)
            {
                return BadRequest();
            }

            db.Entry(fORNECEDOR_ENDERECO).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FORNECEDOR_ENDERECOExists(id))
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

        // POST: api/FornecedorEndereco
        [ResponseType(typeof(FORNECEDOR_ENDERECO))]
        public IHttpActionResult PostFORNECEDOR_ENDERECO(FORNECEDOR_ENDERECO fORNECEDOR_ENDERECO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ENDERECO_FORNECEDOR;").First();

            fORNECEDOR_ENDERECO.ID_ENDERECO = NextValue;

            db.FORNECEDOR_ENDERECO.Add(fORNECEDOR_ENDERECO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FORNECEDOR_ENDERECOExists(fORNECEDOR_ENDERECO.ID_ENDERECO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = fORNECEDOR_ENDERECO.ID_ENDERECO }, fORNECEDOR_ENDERECO);
        }

        // DELETE: api/FornecedorEndereco/5
        [ResponseType(typeof(FORNECEDOR_ENDERECO))]
        public IHttpActionResult DeleteFORNECEDOR_ENDERECO(int id)
        {
            FORNECEDOR_ENDERECO fORNECEDOR_ENDERECO = db.FORNECEDOR_ENDERECO.Find(id);
            if (fORNECEDOR_ENDERECO == null)
            {
                return NotFound();
            }

            db.FORNECEDOR_ENDERECO.Remove(fORNECEDOR_ENDERECO);
            db.SaveChanges();

            return Ok(fORNECEDOR_ENDERECO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FORNECEDOR_ENDERECOExists(int id)
        {
            return db.FORNECEDOR_ENDERECO.Count(e => e.ID_ENDERECO == id) > 0;
        }
    }
}