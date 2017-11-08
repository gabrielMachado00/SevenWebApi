using SevenMedicalApi.Models;
using SisMedApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace SevenMedicalApi.Controllers
{
    public class CARTAO_FAIXA_DESCONTODTO
    {
        public int ID_CARTAO_FAIXA_DESCONTO { get; set; }
        public int ID_CARTAO { get; set; }
        public int PARCELA_INICIAL { get; set; }
        public int PARCELA_FINAL { get; set; }
        public System.Decimal DESCONTO { get; set; }
    }

    public class CartaoFaixaDescontoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        //GET api/CartaoFaixaDesconto/Cartao/1
        [Route("api/CartaoFaixaDesconto/Cartao/{idCartao}")]
        public IEnumerable<CARTAO_FAIXA_DESCONTODTO>GetCARTAO_FAIXA_DESCONTO(int idCartao)
        {
            return (from cd in db.CARTAO_FAIXA_DESCONTOes
                    join c in db.CARTAOs on cd.ID_CARTAO equals c.ID_CARTAO
                    select new CARTAO_FAIXA_DESCONTODTO()
                    {
                        ID_CARTAO_FAIXA_DESCONTO = cd.ID_CARTAO_FAIXA_DESCONTO,
                        ID_CARTAO = cd.ID_CARTAO,
                        PARCELA_INICIAL = cd.PARCELA_INICIAL,
                        PARCELA_FINAL = cd.PARCELA_FINAL,
                        DESCONTO = cd.DESCONTO
                    }).Where(cd => cd.ID_CARTAO == idCartao).ToList();
        }

        // GET api/Cartao/CartaoFaixaDesconto/5
        [Route("api/Cartao/CartaoFaixaDesconto/{id}")]
        public CARTAO_FAIXA_DESCONTO GetDESCONTO(int id)
        {
            CARTAO_FAIXA_DESCONTO faixa_desconto = db.CARTAO_FAIXA_DESCONTOes.Find(id);
            if (faixa_desconto == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return faixa_desconto;
        }

        // PUT: api/CartaoFaixaDesconto/5
        [ResponseType(typeof(void))]
        [Route("api/CartaoFaixaDesconto/PutCARTAO_FAIXA_DESCONTO/{idCartaoDesconto}")]
        public IHttpActionResult PutCARTAO_FAIXA_DESCONTO(int idCartaoDesconto, CARTAO_FAIXA_DESCONTO pCARTAO_FAIXA_DESCONTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (idCartaoDesconto != pCARTAO_FAIXA_DESCONTO.ID_CARTAO_FAIXA_DESCONTO)
            {
                return BadRequest();
            }

            db.Entry(pCARTAO_FAIXA_DESCONTO).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (! CARTAO_FAIXA_DESCONTOExists(idCartaoDesconto))
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

        // POST: api/CartaoFaixaDesconto
        [ResponseType(typeof(CARTAO_FAIXA_DESCONTO))]
        public IHttpActionResult PostCARTAO_FAIXA_DESCONTO(CARTAO_FAIXA_DESCONTO pCARTAO_FAIXA_DESCONTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_CARTAO_FAIXA_DESCONTO;").First();

            pCARTAO_FAIXA_DESCONTO.ID_CARTAO_FAIXA_DESCONTO = NextValue;

            db.CARTAO_FAIXA_DESCONTOes.Add(pCARTAO_FAIXA_DESCONTO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CARTAO_FAIXA_DESCONTOExists(pCARTAO_FAIXA_DESCONTO.ID_CARTAO_FAIXA_DESCONTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { idCartao = pCARTAO_FAIXA_DESCONTO.ID_CARTAO}, pCARTAO_FAIXA_DESCONTO);
        }

        [ResponseType(typeof(CARTAO_FAIXA_DESCONTO))]
        public IHttpActionResult DeleteCARTAO_FAIXA_DESCONTO(int id)
        {
            CARTAO_FAIXA_DESCONTO pCARTAO_FAIXA_DESCONTO = db.CARTAO_FAIXA_DESCONTOes.Where(cd => cd.ID_CARTAO_FAIXA_DESCONTO == id).FirstOrDefault();
            if (pCARTAO_FAIXA_DESCONTO == null)
            {
                return NotFound();
            }

            db.CARTAO_FAIXA_DESCONTOes.Remove(pCARTAO_FAIXA_DESCONTO);
            db.SaveChanges();

            return Ok(pCARTAO_FAIXA_DESCONTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CARTAO_FAIXA_DESCONTOExists(int idCartaoDesconto)
        {
            return db.CARTAO_FAIXA_DESCONTOes.Count(c => c.ID_CARTAO_FAIXA_DESCONTO == idCartaoDesconto) > 0;
        }
    }
}