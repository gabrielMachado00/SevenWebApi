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
using System.Data.SqlClient;

namespace SisMedApi.Controllers
{
    public class CONTADTO
    {
        public int ID_CONTA { get; set; }
        public string DESCRICAO { get; set; }
        public Nullable<int> ID_UNIDADE { get; set; }
        public decimal SALDO_INICIAL { get; set; }
        public string UNIDADE { get; set; }
    }

    public class EXTRATO_CONTA
    {
        public int ID_EXTRATO { get; set; }
        public int ID_DOCUMENTO { get; set; }
        public Decimal VALOR { get; set; }
        public DateTime DATA { get; set; }
        public string LOGIN { get; set; }
        public Nullable<Decimal> DESCONTO { get; set; }
        public int ID_CONTA { get; set; }
        public string OPERACAO { get; set; }
        public string TIPO { get; set; }
        public string DOCUMENTO { get; set; }
    }

    public class ContasController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/contas
        [Route("api/contas")]
        public IEnumerable<CONTADTO> GetCONTAs()
        {
            return (from c in db.CONTAs
                    join u in db.UNIDADEs on c.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    select new CONTADTO()
                    {
                        ID_CONTA = c.ID_CONTA,
                        DESCRICAO = c.DESCRICAO,
                        ID_UNIDADE = c.ID_UNIDADE,
                        SALDO_INICIAL = c.SALDO_INICIAL,
                        UNIDADE = u.NOME_FANTASIA
                    }).ToList();
        }

        // GET api/conta/1
        [Route("api/conta/{idConta}")]
        public IEnumerable<CONTADTO> GetCONTA(int idConta)
        {
            return (from c in db.CONTAs
                    join u in db.UNIDADEs on c.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    select new CONTADTO()
                    {
                        ID_CONTA = c.ID_CONTA,
                        DESCRICAO = c.DESCRICAO,
                        ID_UNIDADE = c.ID_UNIDADE,
                        SALDO_INICIAL = c.SALDO_INICIAL,
                        UNIDADE = u.NOME_FANTASIA
                    }).Where(c => c.ID_CONTA == idConta).ToList();
        }

        // GET api/banco/1/contas
  /*      [Route("api/banco/{idBanco}/contas")]
        public IEnumerable<CONTADTO> GetBANCO_CONTAS(int idBanco)
        {
            return (from c in db.CONTAs
                    join u in db.UNIDADEs on c.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    select new CONTADTO()
                    {
                        ID_CONTA = c.ID_CONTA,
                        DESCRICAO = c.DESCRICAO,
                        ID_UNIDADE = c.ID_UNIDADE,
                        UNIDADE = u.NOME_FANTASIA
                    }).Where(c => c.ID_BANCO == idBanco).ToList();
        }

        // GET api/banco/1/conta/1
        [Route("api/banco/{idBanco}/conta/{idConta}")]
        public IEnumerable<CONTADTO> GetBANCO_CONTA(int idBanco, int idConta)
        {
            return (from c in db.CONTAs
                    join u in db.UNIDADEs on c.ID_UNIDADE equals u.ID_UNIDADE into _u
                    from u in _u.DefaultIfEmpty()
                    select new CONTADTO()
                    {
                        ID_CONTA = c.ID_CONTA,
                        DESCRICAO = c.DESCRICAO,
                        ID_UNIDADE = c.ID_UNIDADE,
                        UNIDADE = u.NOME_FANTASIA
                    }).Where(c => c.ID_BANCO == idBanco && c.ID_CONTA == idConta).ToList();
        } */

        // GET api/banco/conta/1/saldo/01-01-2015
        [Route("api/banco/conta/{idConta}/saldo/{dtSaldo}")]
        public decimal GetBANCO_CONTA(int idConta, DateTime dtSaldo)
        {
                Decimal Saldo = db.Database.SqlQuery<Decimal>("SELECT dbo.CONTA_SALDO_DIA(Convert(date, @DATA), @CONTA) AS SALDO",
                                                                            new SqlParameter("@DATA", dtSaldo), 
                                                                             new SqlParameter("@CONTA", idConta)).First();
                return Saldo;                
        }

        [Route("api/conta/{idConta}/extrato/{dtInicial}/{dtFinal}")]
        public IEnumerable<EXTRATO_CONTA> GetEXTRATO_CONTA(int idConta, DateTime dtInicial, DateTime dtFinal)
        {
            return (from de in db.DOCUMENTO_EXTRATO
                    join df in db.DOCUMENTO_FINANCEIRO on de.ID_DOCUMENTO equals df.ID_DOCUMENTO
                    select new EXTRATO_CONTA()
                    {
                        ID_CONTA = de.ID_CONTA,
                        DATA = de.DATA,
                        VALOR = df.TIPO == "D" ? de.VALOR * -1 :
                                df.TIPO == "C" ? de.VALOR : 0,
                        LOGIN = de.LOGIN,
                        DESCONTO = de.DESCONTO,
                        TIPO = df.TIPO,
                        OPERACAO = df.TIPO == "D" ? "DÉBITO" :
                                   df.TIPO == "C" ? "CRÉDITO" : "",
                        DOCUMENTO = df.NUM_DOCUMENTO
                    }).Where(de => de.ID_CONTA == idConta && DbFunctions.TruncateTime(de.DATA) >= DbFunctions.TruncateTime(dtInicial) &&
                                                             DbFunctions.TruncateTime(de.DATA) <= DbFunctions.TruncateTime(dtFinal))
                     .Union(from t in db.TRANSFERENCIAs
                            select new EXTRATO_CONTA()
                            {
                                ID_CONTA = t.ID_CONTA_ORIGEM,
                                DATA = t.DATA,
                                VALOR = t.VALOR * -1,
                                LOGIN = t.LOGIN,
                                DESCONTO = 0,
                                TIPO = "D",
                                OPERACAO = "DÉBITO",
                                DOCUMENTO = "Transferência entre contas"
                            }).Where(to => to.ID_CONTA == idConta && DbFunctions.TruncateTime(to.DATA) >= DbFunctions.TruncateTime(dtInicial) &&
                                                             DbFunctions.TruncateTime(to.DATA) <= DbFunctions.TruncateTime(dtFinal))
                     .Union(from t in db.TRANSFERENCIAs
                            select new EXTRATO_CONTA()
                            {
                                ID_CONTA = t.ID_CONTA_DESTINO,
                                DATA = t.DATA,
                                VALOR = t.VALOR,
                                LOGIN = t.LOGIN,
                                DESCONTO = 0,
                                TIPO = "C",
                                OPERACAO = "CRÉDITO",
                                DOCUMENTO = "Transferência entre contas"
                            }).Where(to => to.ID_CONTA == idConta && DbFunctions.TruncateTime(to.DATA) >= DbFunctions.TruncateTime(dtInicial) &&
                                                             DbFunctions.TruncateTime(to.DATA) <= DbFunctions.TruncateTime(dtFinal)).ToList();
        }


        // PUT: api/Contas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCONTA(int id, CONTA cONTA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cONTA.ID_CONTA)
            {
                return BadRequest();
            }

            db.Entry(cONTA).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CONTAExists(id))
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

        // POST: api/Contas
        [ResponseType(typeof(CONTA))]
        public IHttpActionResult PostCONTA(CONTA cONTA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_CONTA;").First();

            cONTA.ID_CONTA = NextValue;

            db.CONTAs.Add(cONTA);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CONTAExists(cONTA.ID_CONTA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cONTA.ID_CONTA }, cONTA);
        }

        // DELETE: api/Contas/5
        [ResponseType(typeof(CONTA))]
        public IHttpActionResult DeleteCONTA(int id)
        {
            CONTA cONTA = db.CONTAs.Find(id);
            if (cONTA == null)
            {
                return NotFound();
            }

            db.CONTAs.Remove(cONTA);
            db.SaveChanges();

            return Ok(cONTA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CONTAExists(int id)
        {
            return db.CONTAs.Count(e => e.ID_CONTA == id) > 0;
        }
    }
}