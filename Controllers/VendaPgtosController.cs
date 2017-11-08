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

    public class VENDA_PGTOSDTO
    {
        public int ID_ITEM_PGTO_VENDA { get; set; }
        public int ID_VENDA { get; set; }
        public Nullable<int> TIPO_PGTO { get; set; }
        public string DESCRICAO { get; set; }
        public int NUM_PARCELAS { get; set; }
        public DateTime PRIMEIRO_VCTO { get; set; }
        public decimal VALOR { get; set; }
        public Nullable<decimal> VALOR_DESCONTO { get; set; }
        public Nullable<int> ID_CONVENIO { get; set; }
        public string CONVENIO { get; set; }
        public string TIPO { get; set; }
        public string DEBITOCREDITO { get; set; }
        public Nullable<decimal> VALOR_LIQUIDO { get; set; }
        public Nullable<decimal> VALOR_ACRESCIMO { get; set; }
        public Nullable<int> ID_CARTAO { get; set; }
        public string BANDEIRA { get; set; }
        public string STATUS { get; set; }
        public string DESC_STATUS { get; set; }
        public Nullable<decimal> VALOR_PAGO_PARCIAL { get; set; }

    }

    public class VendaPgtosController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/Venda/1/Pagamentos
        [Route("api/Vendas/{idVenda}/Pagamentos")]
        public IEnumerable<VENDA_PGTOSDTO> GetVENDA_PGTOS(int idVenda)
        {
            return (from vp in db.VENDA_PGTOS
                    join v in db.VENDAs on vp.ID_VENDA equals v.ID_VENDA
                    join cv in db.CONVENIOs on vp.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join ct in db.CARTAOs on vp.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new VENDA_PGTOSDTO()
                    {
                        ID_ITEM_PGTO_VENDA = vp.ID_ITEM_PGTO_VENDA,
                        ID_VENDA = vp.ID_VENDA,
                        TIPO_PGTO = vp.TIPO_PGTO,
                        DESCRICAO = vp.DESCRICAO,
                        NUM_PARCELAS = vp.NUM_PARCELAS,
                        PRIMEIRO_VCTO = vp.PRIMEIRO_VCTO,
                        VALOR = vp.VALOR,
                        VALOR_DESCONTO = vp.VALOR_DESCONTO,
                        BANDEIRA = ct.BANDEIRA,
                        DEBITOCREDITO = ct.TIPO == "D" ? "DÉBITO" :
                                        ct.TIPO == "C" ? "CRÉDITO" : "",
                        ID_CONVENIO = vp.ID_CONVENIO,
                        CONVENIO = cv.DESCRICAO,
                        VALOR_LIQUIDO = (vp.VALOR_DESCONTO == null ? vp.VALOR : vp.VALOR - vp.VALOR_DESCONTO)
                                        + (vp.VALOR_ACRESCIMO == null ? 0 : vp.VALOR_ACRESCIMO),
                        TIPO = vp.TIPO_PGTO == 0 ? "DINHEIRO" :
                               vp.TIPO_PGTO == 1 ? "CHEQUE" :
                               vp.TIPO_PGTO == 2 ? "CARTÃO" :
                               vp.TIPO_PGTO == 3 ? "BOLETO" :
                               vp.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        VALOR_ACRESCIMO = vp.VALOR_ACRESCIMO,
                        STATUS = vp.STATUS,
                        DESC_STATUS = vp.STATUS == "A" ? "ATIVO" : "CANCELADO",
                        VALOR_PAGO_PARCIAL = vp.VALOR_PAGO_PARCIAL == null ? 0 : vp.VALOR_PAGO_PARCIAL
                    }).Where(vp => vp.ID_VENDA == idVenda).ToList();
        }

        // GET api/Venda/1/Pagamento/2
        [Route("api/Venda/{idVenda}/Pagamento/{idPagamento}")]
        public IEnumerable<VENDA_PGTOSDTO> GetVENDA_PGTO(int idVenda, int idPagamento)
        {
            return (from vp in db.VENDA_PGTOS
                    join v in db.VENDAs on vp.ID_VENDA equals v.ID_VENDA
                    join cv in db.CONVENIOs on vp.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join ct in db.CARTAOs on vp.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    select new VENDA_PGTOSDTO()
                    {
                        ID_ITEM_PGTO_VENDA = vp.ID_ITEM_PGTO_VENDA,
                        ID_VENDA = vp.ID_VENDA,
                        TIPO_PGTO = vp.TIPO_PGTO,
                        DESCRICAO = vp.DESCRICAO,
                        NUM_PARCELAS = vp.NUM_PARCELAS,
                        PRIMEIRO_VCTO = vp.PRIMEIRO_VCTO,
                        VALOR = vp.VALOR,
                        VALOR_DESCONTO = vp.VALOR_DESCONTO,
                        BANDEIRA = ct.BANDEIRA,
                        DEBITOCREDITO = ct.TIPO == "D" ? "DÉBITO" :
                                        ct.TIPO == "C" ? "CRÉDITO" : "",
                        ID_CONVENIO = vp.ID_CONVENIO,
                        CONVENIO = cv.DESCRICAO,
                        VALOR_LIQUIDO = (vp.VALOR_DESCONTO == null ? vp.VALOR : vp.VALOR - vp.VALOR_DESCONTO)
                                        + (vp.VALOR_ACRESCIMO == null ? 0 : vp.VALOR_ACRESCIMO),
                        TIPO = vp.TIPO_PGTO == 0 ? "DINHEIRO" :
                               vp.TIPO_PGTO == 1 ? "CHEQUE" :
                               vp.TIPO_PGTO == 2 ? "CARTÃO" :
                               vp.TIPO_PGTO == 3 ? "BOLETO" :
                               vp.TIPO_PGTO == 4 ? "CONVÊNIO" : "",
                        VALOR_ACRESCIMO = vp.VALOR_ACRESCIMO,
                        STATUS = vp.STATUS,
                        DESC_STATUS = vp.STATUS == "A" ? "ATIVO" : "CANCELADO",
                        VALOR_PAGO_PARCIAL = vp.VALOR_PAGO_PARCIAL == null ? 0 : vp.VALOR_PAGO_PARCIAL
                    }).Where(vp => vp.ID_VENDA == idVenda && vp.ID_ITEM_PGTO_VENDA == idPagamento).ToList();
        }

        // PUT: api/VendaPgtos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVENDA_PGTOS(int id, VENDA_PGTOS vENDA_PGTOS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vENDA_PGTOS.ID_ITEM_PGTO_VENDA)
            {
                return BadRequest();
            }

            db.Entry(vENDA_PGTOS).State = EntityState.Modified;

            try
            {
                //db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VENDA_PGTOSExists(id))
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

        // POST: api/VendaPgtos
        [ResponseType(typeof(VENDA_PGTOS))]
        public IHttpActionResult PostVENDA_PGTOS(VENDA_PGTOS vENDA_PGTOS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ITEM_PGTO_VENDA;").First();

            vENDA_PGTOS.ID_ITEM_PGTO_VENDA = NextValue;

            db.VENDA_PGTOS.Add(vENDA_PGTOS);

            try
            {
                //db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                if (VENDA_PGTOSExists(vENDA_PGTOS.ID_ITEM_PGTO_VENDA))
                {
                    return Conflict();
                }
                else
                {
                    throw new Exception(e.Message);
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = vENDA_PGTOS.ID_ITEM_PGTO_VENDA }, vENDA_PGTOS);
        }

        // DELETE: api/VendaPgtos/5
        [ResponseType(typeof(VENDA_PGTOS))]
        public IHttpActionResult DeleteVENDA_PGTOS(int id)
        {
            VENDA_PGTOS vENDA_PGTOS = db.VENDA_PGTOS.Find(id);
            if (vENDA_PGTOS == null)
            {
                return NotFound();
            }

            db.VENDA_PGTOS.Remove(vENDA_PGTOS);
            //db.BulkSaveChanges();
            db.SaveChanges();


            return Ok(vENDA_PGTOS);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VENDA_PGTOSExists(int id)
        {
            return db.VENDA_PGTOS.Count(e => e.ID_ITEM_PGTO_VENDA == id) > 0;
        }
    }
}