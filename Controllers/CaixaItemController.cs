using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;

namespace SisMedApi.Controllers
{
    public class CAIXA_ITEMDTO
    {
        public int ID_ITEM_CAIXA { get; set; }
        public int ID_CAIXA { get; set; }
        public Nullable<int> ID_ITEM_PGTO_VENDA { get; set; }
        public string DEBITO_CREDITO { get; set; }
        public string DESCRICAO { get; set; }
        public string NUM_DOCUMENTO { get; set; }
        public Nullable<Decimal> VALOR { get; set; }
        public int TIPO_PGTO_CAIXA_ITEM { get; set; }
        public string TIPO { get; set; }
        public Nullable<int> NUM_PARCELAS { get; set; }
        public Nullable<DateTime> PRIMEIRO_VCTO { get; set; }
        public string DEBITOCREDITO { get; set; }
        public Nullable<int> ID_CENTRO_CUSTO { get; set; }
        public Nullable<int> ID_CATEGORIA { get; set; }
        public Nullable<int> ID_SUB_CATEGORIA { get; set; }
        public Nullable<int> ID_CONTA { get; set; }
        public string CENTRO_CUSTO { get; set; }
        public string CATEGORIA { get; set; }
        public string SUB_CATEGORIA { get; set; }
        public string CONTA { get; set; }
        public string BANDEIRA { get; set; }
        public string OPERACAO_CARTAO { get; set; }
        public string CLIENTE { get; set; }
        public string PROCEDIMENTO { get; set; }
        public string REGIAO { get; set; }
        public string PROFISSIONAL { get; set; }
        public string STATUS { get; set; }
        public string DESC_STATUS { get; set; }
        public Nullable<decimal> VALOR_PAGO { get; set; }
        public Nullable<int> CV_CARTAO { get; set; }
        public string NUM_CHEQUE { get; set; }
        public Nullable<DateTime> DATA_COMPETENCIA { get; set; }
        public Nullable<Decimal> VALOR_PAGO_PARCIAL { get; set; }
        public Nullable<DateTime> DATA_VENCIMENTO { get; set; }
        public Nullable<DateTime> DATA_ABERTURA { get; set; }
        public Nullable<DateTime> DATA_FECHAMENTO { get; set; }
        public string LOGIN { get; set; }
        public decimal VALOR_ABERTURA { get; set; }
        public decimal VALOR_FECHAMENTO { get; set; }
    }

    public class CaixaItemController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        string sql = @"SELECT ci.ID_ITEM_CAIXA, ci.ID_CAIXA, ci.ID_ITEM_PGTO_VENDA, ci.DEBITO_CREDITO, CX.LOGIN,CX.DATA_ABERTURA, CX.DATA_FECHAMENTO,
       ci.DESCRICAO, ci.NUM_DOCUMENTO, ci.VALOR, ci.TIPO_PGTO_CAIXA_ITEM, CX.VALOR_ABERTURA, CX.VALOR_FECHAMENTO,
       CASE WHEN ci.TIPO_PGTO_CAIXA_ITEM = 0 THEN 'DINHEIRO' 
	        WHEN ci.TIPO_PGTO_CAIXA_ITEM = 1 THEN 'CHEQUE'
			WHEN ci.TIPO_PGTO_CAIXA_ITEM = 2 THEN 'CARTÃO' 
			WHEN ci.TIPO_PGTO_CAIXA_ITEM = 3 THEN 'BOLETO'
			WHEN ci.TIPO_PGTO_CAIXA_ITEM = 4 THEN 'CONVÊNIO' ELSE '' END AS TIPO,
       vp.NUM_PARCELAS,
	   CASE WHEN VP.PRIMEIRO_VCTO IS NULL THEN CI.DATA_VENCIMENTO ELSE VP.PRIMEIRO_VCTO END AS PRIMEIRO_VCTO,
       ct.BANDEIRA,
	   CASE WHEN CI.DEBITO_CREDITO = 'D' THEN 'DÉBITO' ELSE 'CRÉDITO' END AS DEBITOCREDITO,
       ci.ID_CENTRO_CUSTO, ci.ID_CATEGORIA, ci.ID_SUB_CATEGORIA, ci.ID_CONTA,
       cc.DESCRICAO AS CENTRO_CUSTO, c.DESCRICAO AS CATEGORIA, sc.DESCRICAO AS SUB_CATEGORIA,
       Co.DESCRICAO AS CONTA,
	   CASE WHEN CT.TIPO = 'D' THEN 'DÉBITO' ELSE 'CRÉDITO' END AS OPERACAO_CARTAO,
       Cli.NOME AS CLIENTE, p.DESCRICAO AS PROCEDIMENTO, r.REGIAO, pr.NOME AS PROFISSIONAL,
       ci.STATUS,
	   CASE WHEN CI.STATUS = 'A' THEN 'ATIVO' ELSE 'CANCELADO' END AS DESC_STATUS,
	   dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(V.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA) AS VALOR_PAGO,
       ci.CV_CARTAO, Ci.NUM_CHEQUE, Ci.DATA_COMPETENCIA, ci.VALOR_PAGO_PARCIAL,
       CASE WHEN CI.DATA_VENCIMENTO IS NULL THEN VP.PRIMEIRO_VCTO ELSE CI.DATA_VENCIMENTO END AS DATA_VENCIMENTO
FROM CAIXA_ITEM CI
INNER JOIN CAIXA CX ON CX.ID_CAIXA = CI.ID_CAIXA
LEFT JOIN VENDA_PGTOS VP on VP.ID_ITEM_PGTO_VENDA = CI.ID_ITEM_PGTO_VENDA
LEFT JOIN CENTRO_CUSTO CC on CC.ID_CENTRO_CUSTO = CI.ID_CENTRO_CUSTO
LEFT JOIN CATEGORIA C on C.ID_CATEGORIA = ci.ID_CATEGORIA
LEFT JOIN SUB_CATEGORIA SC on SC.ID_SUB_CATEGORIA = CI.ID_SUB_CATEGORIA
LEFT JOIN CONTA CO on CO.ID_CONTA = CI.ID_CONTA 
LEFT JOIN CARTAO CT on CT.ID_CARTAO = VP.ID_CARTAO 
LEFT JOIN VENDA_ITEM VI on VI.ID_VENDA = VP.ID_VENDA
LEFT JOIN VENDA V on V.ID_VENDA = VP.ID_VENDA
LEFT JOIN CLIENTE CLI on CLI.ID_CLIENTE = V.ID_CLIENTE
LEFT JOIN PROCEDIMENTO P on P.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO
LEFT JOIN REGIAO_CORPO R on R.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
LEFT JOIN PROFISSIONAL PR on PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL";

        // GET api/caixa/1/lancamentos
        [Route("api/caixas/{idCaixa}/lancamentos")]
        public IEnumerable<CAIXA_ITEMDTO> GetCAIXA_LCTOS(int idCaixa)
        {
            sql = sql + " WHERE CI.ID_CAIXA = " + idCaixa;
            return db.Database.SqlQuery<CAIXA_ITEMDTO>(sql).ToList();

            /* return (from ci in db.CAIXA_ITEM
                     join vp in db.VENDA_PGTOS on ci.ID_ITEM_PGTO_VENDA equals vp.ID_ITEM_PGTO_VENDA into _vp
                     from vp in _vp.DefaultIfEmpty()
                     join cc in db.CENTRO_CUSTO on ci.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                     from cc in _cc.DefaultIfEmpty()
                     join c in db.CATEGORIAs on ci.ID_CATEGORIA equals c.ID_CATEGORIA into _c
                     from c in _c.DefaultIfEmpty()
                     join sc in db.SUB_CATEGORIA on ci.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                     from sc in _sc.DefaultIfEmpty()
                     join co in db.CONTAs on ci.ID_CONTA equals co.ID_CONTA into _co
                     from co in _co.DefaultIfEmpty()
                     join ct in db.CARTAOs on vp.ID_CARTAO equals ct.ID_CARTAO into _ct
                     from ct in _ct.DefaultIfEmpty()
                     join vi in db.VENDA_ITEM on vp.ID_VENDA equals vi.ID_VENDA into _vi
                     from vi in _vi.DefaultIfEmpty()
                     join v in db.VENDAs on vp.ID_VENDA equals v.ID_VENDA into _v
                     from v in _v.DefaultIfEmpty()
                     join cli in db.CLIENTES on v.ID_CLIENTE equals cli.ID_CLIENTE into _cli
                     from cli in _cli.DefaultIfEmpty()
                     join p in db.PROCEDIMENTOes on vi.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO into _p
                     from p in _p.DefaultIfEmpty()
                     join r in db.REGIAO_CORPO on vi.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO into _r
                     from r in _r.DefaultIfEmpty()
                     join pr in db.PROFISSIONALs on vi.ID_PROFISSIONAL equals pr.ID_PROFISSIONAL into _pr
                     from pr in _pr.DefaultIfEmpty()
                     select new CAIXA_ITEMDTO()
                     {
                         ID_ITEM_CAIXA = ci.ID_ITEM_CAIXA,
                         ID_CAIXA = ci.ID_CAIXA,
                         ID_ITEM_PGTO_VENDA = ci.ID_ITEM_PGTO_VENDA,
                         DEBITO_CREDITO = ci.DEBITO_CREDITO,
                         DESCRICAO = ci.DESCRICAO,
                         NUM_DOCUMENTO = ci.NUM_DOCUMENTO,
                         VALOR = ci.VALOR,
                         TIPO_PGTO_CAIXA_ITEM = ci.TIPO_PGTO_CAIXA_ITEM,
                         TIPO = ci.TIPO_PGTO_CAIXA_ITEM == 0 ? "DINHEIRO" :
                                ci.TIPO_PGTO_CAIXA_ITEM == 1 ? "CHEQUE" :
                                ci.TIPO_PGTO_CAIXA_ITEM == 2 ? "CARTÃO" :
                                ci.TIPO_PGTO_CAIXA_ITEM == 3 ? "BOLETO" :
                                ci.TIPO_PGTO_CAIXA_ITEM == 4 ? "CONVÊNIO" : "",
                         NUM_PARCELAS = vp.NUM_PARCELAS,
                         PRIMEIRO_VCTO = vp.PRIMEIRO_VCTO == null? ci.DATA_VENCIMENTO: vp.PRIMEIRO_VCTO,
                         BANDEIRA = ct.BANDEIRA,
                         DEBITOCREDITO = ci.DEBITO_CREDITO == "D" ? "DÉBITO" :
                                         ci.DEBITO_CREDITO == "C" ? "CRÉDITO" : "",
                         ID_CENTRO_CUSTO = ci.ID_CENTRO_CUSTO,
                         ID_CATEGORIA = ci.ID_CATEGORIA,
                         ID_SUB_CATEGORIA = ci.ID_SUB_CATEGORIA,
                         ID_CONTA = ci.ID_CONTA,
                         CENTRO_CUSTO = cc.DESCRICAO,
                         CATEGORIA = c.DESCRICAO,
                         SUB_CATEGORIA = sc.DESCRICAO,
                         CONTA = co.DESCRICAO,
                         OPERACAO_CARTAO = ct.TIPO == "D" ? "DÉBITO" :
                                           ct.TIPO == "C" ? "CRÉDITO" : "",
                         CLIENTE = cli.NOME,
                         PROCEDIMENTO = p.DESCRICAO,
                         REGIAO = r.REGIAO,
                         PROFISSIONAL = pr.NOME,
                         STATUS = ci.STATUS,
                         DESC_STATUS = ci.STATUS == "A" ? "ATIVO" : "CANCELADO",
                         VALOR_PAGO = db.Database.SqlQuery<decimal>("SELECT dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(" + vi.ID_VENDA + "," + vi.ID_ITEM_VENDA + "," + vp.ID_ITEM_PGTO_VENDA + ")").FirstOrDefault(), //vi.VALOR_PAGO,
                         CV_CARTAO = ci.CV_CARTAO,
                         NUM_CHEQUE = ci.NUM_CHEQUE,
                         DATA_COMPETENCIA = ci.DATA_COMPETENCIA,
                         VALOR_PAGO_PARCIAL = ci.VALOR_PAGO_PARCIAL,
                         DATA_VENCIMENTO = ci.DATA_VENCIMENTO == null ? vp.PRIMEIRO_VCTO : ci.DATA_VENCIMENTO                        
                     }).Where(ci => ci.ID_CAIXA == idCaixa).ToList(); */
        }

        // GET api/caixa/1/lancamento/2
        [Route("api/caixas/{idCaixa}/lancamento/{idLancamento}")]
        public IEnumerable<CAIXA_ITEMDTO> GetCAIXA_LCTO(int idCaixa, int idLancamento)
        {
            sql = sql + " WHERE ci.ID_ITEM_CAIXA = " + idLancamento;
            return db.Database.SqlQuery<CAIXA_ITEMDTO>(sql).ToList();
            /*return (from ci in db.CAIXA_ITEM
                    join vp in db.VENDA_PGTOS on ci.ID_ITEM_PGTO_VENDA equals vp.ID_ITEM_PGTO_VENDA into _vp
                    from vp in _vp.DefaultIfEmpty()
                    join cc in db.CENTRO_CUSTO on ci.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    join c in db.CATEGORIAs on ci.ID_CATEGORIA equals c.ID_CATEGORIA into _c
                    from c in _c.DefaultIfEmpty()
                    join sc in db.SUB_CATEGORIA on ci.ID_SUB_CATEGORIA equals sc.ID_SUB_CATEGORIA into _sc
                    from sc in _sc.DefaultIfEmpty()
                    join co in db.CONTAs on ci.ID_CONTA equals co.ID_CONTA into _co
                    from co in _co.DefaultIfEmpty()
                    join ct in db.CARTAOs on vp.ID_CARTAO equals ct.ID_CARTAO into _ct
                    from ct in _ct.DefaultIfEmpty()
                    join vi in db.VENDA_ITEM on vp.ID_VENDA equals vi.ID_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join v in db.VENDAs on vp.ID_VENDA equals v.ID_VENDA into _v
                    from v in _v.DefaultIfEmpty()
                    join cli in db.CLIENTES on v.ID_CLIENTE equals cli.ID_CLIENTE into _cli
                    from cli in _cli.DefaultIfEmpty()
                    join p in db.PROCEDIMENTOes on vi.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO into _p
                    from p in _p.DefaultIfEmpty()
                    join r in db.REGIAO_CORPO on vi.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO into _r
                    from r in _r.DefaultIfEmpty()
                    join pr in db.PROFISSIONALs on vi.ID_PROFISSIONAL equals pr.ID_PROFISSIONAL into _pr
                    from pr in _pr.DefaultIfEmpty()
                    select new CAIXA_ITEMDTO()
                    {
                        ID_ITEM_CAIXA = ci.ID_ITEM_CAIXA,
                        ID_CAIXA = ci.ID_CAIXA,
                        ID_ITEM_PGTO_VENDA = ci.ID_ITEM_PGTO_VENDA,
                        DEBITO_CREDITO = ci.DEBITO_CREDITO,
                        DESCRICAO = ci.DESCRICAO,
                        NUM_DOCUMENTO = ci.NUM_DOCUMENTO,
                        VALOR = ci.VALOR,
                        TIPO_PGTO_CAIXA_ITEM = ci.TIPO_PGTO_CAIXA_ITEM,
                        TIPO = ci.TIPO_PGTO_CAIXA_ITEM == 0 ? "DINHEIRO" :
                               ci.TIPO_PGTO_CAIXA_ITEM == 1 ? "CHEQUE" :
                               ci.TIPO_PGTO_CAIXA_ITEM == 2 ? "CARTÃO" :
                               ci.TIPO_PGTO_CAIXA_ITEM == 3 ? "BOLETO" :
                               ci.TIPO_PGTO_CAIXA_ITEM == 4 ? "CONVÊNIO" : "",
                        NUM_PARCELAS = vp.NUM_PARCELAS,
                        PRIMEIRO_VCTO = vp.PRIMEIRO_VCTO == null ? ci.DATA_VENCIMENTO : vp.PRIMEIRO_VCTO,
                        BANDEIRA = ct.BANDEIRA,
                        DEBITOCREDITO = ci.DEBITO_CREDITO == "D" ? "DÉBITO" :
                                        ci.DEBITO_CREDITO == "C" ? "CRÉDITO" : "",
                        ID_CENTRO_CUSTO = ci.ID_CENTRO_CUSTO,
                        ID_CATEGORIA = ci.ID_CATEGORIA,
                        ID_SUB_CATEGORIA = ci.ID_SUB_CATEGORIA,
                        ID_CONTA = ci.ID_CONTA,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        CATEGORIA = c.DESCRICAO,
                        SUB_CATEGORIA = sc.DESCRICAO,
                        CONTA = co.DESCRICAO,
                        OPERACAO_CARTAO = ct.TIPO == "D" ? "DÉBITO" :
                                          ct.TIPO == "C" ? "CRÉDITO" : "",
                        CLIENTE = cli.NOME,
                        PROCEDIMENTO = p.DESCRICAO,
                        REGIAO = r.REGIAO,
                        PROFISSIONAL = pr.NOME,
                        STATUS = ci.STATUS,
                        DESC_STATUS = ci.STATUS == "A" ? "ATIVO" : "CANCELADO",
                        VALOR_PAGO = vi.VALOR_PAGO,
                        CV_CARTAO = ci.CV_CARTAO,
                        NUM_CHEQUE = ci.NUM_CHEQUE,
                        DATA_COMPETENCIA = ci.DATA_COMPETENCIA,
                        VALOR_PAGO_PARCIAL = ci.VALOR_PAGO_PARCIAL,
                        DATA_VENCIMENTO = ci.DATA_VENCIMENTO == null ? vp.PRIMEIRO_VCTO : ci.DATA_VENCIMENTO
                    }).Where(ci => ci.ID_CAIXA == idCaixa && ci.ID_ITEM_CAIXA == idLancamento).ToList(); */
        }

        // PUT: api/CaixaLancamento/PutCAIXA_LANCAMENTO/5/1
        [ResponseType(typeof(void))]
        [Route("api/CaixaLancamento/PutCAIXA_LANCAMENTO/{idCaixa}/{idLancamento}")]
        public IHttpActionResult PutCAIXA_LANCAMENTO(int idCaixa, int idLancamento, CAIXA_ITEM cAIXA_ITEM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idCaixa != cAIXA_ITEM.ID_CAIXA)
            {
                return BadRequest();
            }

            if (idLancamento != cAIXA_ITEM.ID_ITEM_CAIXA)
            {
                return BadRequest();
            }

            db.Entry(cAIXA_ITEM).State = EntityState.Modified;

            try
            {
                //db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CAIXA_ITEMExists(idCaixa, idLancamento))
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

        // POST: api/CaixaItem
        [ResponseType(typeof(CAIXA_ITEM))]
        public IHttpActionResult PostCAIXA_ITEM(CAIXA_ITEM cAIXA_ITEM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ITEM_CAIXA;").First();

            cAIXA_ITEM.ID_ITEM_CAIXA = NextValue;

            db.CAIXA_ITEM.Add(cAIXA_ITEM);

            try
            {
               // db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CAIXA_ITEMExists(cAIXA_ITEM.ID_CAIXA, cAIXA_ITEM.ID_ITEM_CAIXA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cAIXA_ITEM.ID_ITEM_CAIXA }, cAIXA_ITEM);
        }

        // DELETE: api/CaixaItem/5
        [ResponseType(typeof(CAIXA_ITEM))]
        public IHttpActionResult DeleteCAIXA_ITEM(int id)
        {
            CAIXA_ITEM cAIXA_ITEM = db.CAIXA_ITEM.Find(id);
            if (cAIXA_ITEM == null)
            {
                return NotFound();
            }

            db.CAIXA_ITEM.Remove(cAIXA_ITEM);
            //db.BulkSaveChanges();
            db.SaveChanges();

            return Ok(cAIXA_ITEM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CAIXA_ITEMExists(int idCaixa, int idLancamento)
        {
            return db.CAIXA_ITEM.Count(e => e.ID_ITEM_CAIXA == idLancamento && e.ID_CAIXA == idCaixa) > 0;
        }
    }
}