using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;
using System.Data.SqlClient;

namespace SisMedApi.Controllers
{
    public class CAIXADTO
    {
        public int ID_CAIXA { get; set; }
        public string LOGIN { get; set; }
        public Nullable<DateTime> DATA_ABERTURA { get; set; }
        public Nullable<Decimal> VALOR_ABERTURA { get; set; }
        public Nullable<DateTime> DATA_FECHAMENTO { get; set; }
        public Nullable<Decimal> VALOR_FECHAMENTO { get; set; }
        public Nullable<Decimal> VALOR_AJUSTE { get; set; }
        public bool CONFERIDO { get; set; }
        public Nullable<decimal> TOTAL_CARTAO { get; set; }
        public Nullable<decimal> TOTAL_CHEQUE { get; set; }
        public Nullable<decimal> TOTAL_DINHEIRO { get; set; }
        public Nullable<decimal> TOTAL_BOLETO { get; set; }
        public Nullable<decimal> TOTAL_PENDENTE { get; set; }
        public Nullable<decimal> VALOR_PAGO_PARCIAL { get; set; }
    }

    public class CaixasController : ApiController
    {
        string sql = "  select ID_CAIXA, LOGIN, DATA_ABERTURA, VALOR_ABERTURA, DATA_FECHAMENTO, VALOR_FECHAMENTO, CONFERIDO, "+
       " (cast(isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 2 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'C'), 0) - " +
"          isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 2 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'D'), 0) as decimal(15,2)) ) AS TOTAL_CARTAO, " +
" (cast(isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 1 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'C'), 0) - " +
"  isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 1 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'D'), 0) as decimal(15,2)) ) AS TOTAL_CHEQUE, " +
" (cast(isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 0 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'C'), 0) - " +
"     isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 0 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'D'), 0) as decimal(15,2)) ) AS TOTAL_DINHEIRO, " +
" (cast(isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 3 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'C'), 0) - " +
"     isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 3 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'D'), 0) as decimal(15,2)) ) AS TOTAL_BOLETO, " +
" cast(0 as decimal(15,2)) AS TOTAL_PENDENTE, " +
" cast(((isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.STATUS = 'A'  AND CAIXA_ITEM.DEBITO_CREDITO = 'C'), 0) - " +
"                  isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.STATUS = 'A'  AND CAIXA_ITEM.DEBITO_CREDITO = 'D'), 0)) -  " +
"                   VALOR_FECHAMENTO) as decimal(15,2)) AS VALOR_AJUSTE,  " +
" Cast(((isnull((SELECT SUM(CASE WHEN ISNULL(VALOR_PAGO_PARCIAL,0) <> 0 THEN VALOR_PAGO_PARCIAL ELSE VALOR END) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.STATUS = 'A'  AND CAIXA_ITEM.DEBITO_CREDITO = 'C'), 0) - "+
" isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.STATUS = 'A'  AND CAIXA_ITEM.DEBITO_CREDITO = 'D'), 0)) -  "+
" VALOR_FECHAMENTO) as decimal(15,2)) AS VALOR_PAGO_PARCIAL " +
                 " from CAIXA " +
                 " WHERE LOGIN = ";

        private SisMedContext db = new SisMedContext();

        // GET: api/Caixas
        public IQueryable<CAIXA> GetCAIXAs()
        {
            return db.CAIXAs;
        }

        // GET: api/Caixas/5
        [ResponseType(typeof(CAIXA))]
        public IHttpActionResult GetCAIXA(int id)
        {
            CAIXA cAIXA = db.CAIXAs.Find(id);
            if (cAIXA == null)
            {
                return NotFound();
            }

            return Ok(cAIXA);
        }

        // GET api/caixa/pablo
        [Route("api/caixa/{login}")]
        public IEnumerable<CAIXADTO> GetCAIXA_LOGIN(string login)
        {
            sql = sql + "'" + login + "'";
            return db.Database.SqlQuery<CAIXADTO>(sql).ToList();
            /* return (from c in db.CAIXAs
                     select new CAIXADTO()
                     {
                         ID_CAIXA = c.ID_CAIXA,
                         LOGIN = c.LOGIN,
                         DATA_ABERTURA = c.DATA_ABERTURA,
                         VALOR_ABERTURA = c.VALOR_ABERTURA,
                         DATA_FECHAMENTO = c.DATA_FECHAMENTO,
                         VALOR_FECHAMENTO = c.VALOR_FECHAMENTO,
                         CONFERIDO = c.CONFERIDO,
                         TOTAL_CARTAO = ( 
                                            ((decimal) ((from ci in db.CAIXA_ITEM
                                                    where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 2 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                    group ci by new
                                                    {
                                                        ci.ID_CAIXA
                                                    } into g
                                                    select new
                                                    {
                                                        Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                    }).FirstOrDefault().Expr1)) -
 ((decimal)((from ci in db.CAIXA_ITEM
             where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 2 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
             group ci by new
             {
                 ci.ID_CAIXA
             } into g
             select new
             {
                 Expr1 = (decimal)g.Sum(p => p.VALOR)
             }).FirstOrDefault().Expr1))
                                                    ),
                         TOTAL_CHEQUE = (
                                             ((decimal)((from ci in db.CAIXA_ITEM
                                                   where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 1 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                         group ci by new
                                                   {
                                                       ci.ID_CAIXA
                                                   } into g
                                                   select new
                                                   {
                                                       Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                   }).FirstOrDefault().Expr1)) -
                                                   ((decimal)((from ci in db.CAIXA_ITEM
                                                               where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 1 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                               group ci by new
                                                               {
                                                                   ci.ID_CAIXA
                                                               } into g
                                                               select new
                                                               {
                                                                   Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                               }).FirstOrDefault().Expr1))
                                                   ),
                         TOTAL_DINHEIRO = (
                                             ((decimal)((from ci in db.CAIXA_ITEM
                                                     where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 0 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                         group ci by new
                                                     {
                                                         ci.ID_CAIXA
                                                     } into g
                                                     select new
                                                     {
                                                         Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                     }).FirstOrDefault().Expr1) ) -
                                                     ((decimal)((from ci in db.CAIXA_ITEM
                                                                 where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 0 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                                 group ci by new
                                                                 {
                                                                     ci.ID_CAIXA
                                                                 } into g
                                                                 select new
                                                                 {
                                                                     Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                                 }).FirstOrDefault().Expr1))
                                                     ),
                         TOTAL_BOLETO = ( 
                                             ((decimal)((from ci in db.CAIXA_ITEM
                                                     where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 3 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                         group ci by new
                                                     {
                                                         ci.ID_CAIXA
                                                     } into g
                                                     select new
                                                     {
                                                         Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                     }).FirstOrDefault().Expr1)) -
                                                     ((decimal)((from ci in db.CAIXA_ITEM
                                                                 where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 3 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                                 group ci by new
                                                                 {
                                                                     ci.ID_CAIXA
                                                                 } into g
                                                                 select new
                                                                 {
                                                                     Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                                 }).FirstOrDefault().Expr1))
                                                     ),
                         TOTAL_PENDENTE = 0,
                         VALOR_AJUSTE = ( ((decimal)((from ci in db.CAIXA_ITEM
                                                   where ci.ID_CAIXA == c.ID_CAIXA && ci.STATUS == "A"
                                                   group ci by new
                                                   {
                                                       ci.ID_CAIXA
                                                   } into g
                                                   select new
                                                   {
                                                       Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                   }).FirstOrDefault().Expr1)) - c.VALOR_FECHAMENTO )
                     }).Where(c => c.LOGIN == login).ToList(); */
        }

        // GET api/caixa/DataAbertura/01-01-2015/Login/PABLO
        [Route("api/caixa/DataAbertura/{dataAbertura}/Login/{login}")]
        public IEnumerable<CAIXADTO> GetCAIXA_DT_LOGIN(DateTime dataAbertura, string login)
        {
            sql = sql + "'" + login + "'" +
                  " AND CAST(DATA_ABERTURA AS DATE) = cast('" + dataAbertura.ToString("yyyy-MM-dd") + "' as date)" + 
                  " AND DATA_FECHAMENTO IS NULL ";
            return db.Database.SqlQuery<CAIXADTO>(sql).ToList();

            /*return (from c in db.CAIXAs
                    select new CAIXADTO()
                    {
                        ID_CAIXA = c.ID_CAIXA,
                        LOGIN = c.LOGIN,
                        DATA_ABERTURA = c.DATA_ABERTURA,
                        VALOR_ABERTURA = c.VALOR_ABERTURA,
                        DATA_FECHAMENTO = c.DATA_FECHAMENTO,
                        VALOR_FECHAMENTO = c.VALOR_FECHAMENTO,
                        CONFERIDO = c.CONFERIDO,
                        TOTAL_CARTAO = (
                                           ((decimal)((from ci in db.CAIXA_ITEM
                                                       where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 2 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                       group ci by new
                                                       {
                                                           ci.ID_CAIXA
                                                       } into g
                                                       select new
                                                       {
                                                           Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                       }).FirstOrDefault().Expr1)) -
((decimal)((from ci in db.CAIXA_ITEM
            where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 2 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
            group ci by new
            {
                ci.ID_CAIXA
            } into g
            select new
            {
                Expr1 = (decimal)g.Sum(p => p.VALOR)
            }).FirstOrDefault().Expr1))
                                                   ),
                        TOTAL_CHEQUE = (
                                            ((decimal)((from ci in db.CAIXA_ITEM
                                                        where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 1 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                        group ci by new
                                                        {
                                                            ci.ID_CAIXA
                                                        } into g
                                                        select new
                                                        {
                                                            Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                        }).FirstOrDefault().Expr1)) -
                                                  ((decimal)((from ci in db.CAIXA_ITEM
                                                              where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 1 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                              group ci by new
                                                              {
                                                                  ci.ID_CAIXA
                                                              } into g
                                                              select new
                                                              {
                                                                  Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                              }).FirstOrDefault().Expr1))
                                                  ),
                        TOTAL_DINHEIRO = (
                                            ((decimal)((from ci in db.CAIXA_ITEM
                                                        where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 0 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                        group ci by new
                                                        {
                                                            ci.ID_CAIXA
                                                        } into g
                                                        select new
                                                        {
                                                            Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                        }).FirstOrDefault().Expr1)) -
                                                    ((decimal)((from ci in db.CAIXA_ITEM
                                                                where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 0 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                                group ci by new
                                                                {
                                                                    ci.ID_CAIXA
                                                                } into g
                                                                select new
                                                                {
                                                                    Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                                }).FirstOrDefault().Expr1))
                                                    ),
                        TOTAL_BOLETO = (
                                            ((decimal)((from ci in db.CAIXA_ITEM
                                                        where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 3 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                        group ci by new
                                                        {
                                                            ci.ID_CAIXA
                                                        } into g
                                                        select new
                                                        {
                                                            Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                        }).FirstOrDefault().Expr1)) -
                                                    ((decimal)((from ci in db.CAIXA_ITEM
                                                                where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 3 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                                group ci by new
                                                                {
                                                                    ci.ID_CAIXA
                                                                } into g
                                                                select new
                                                                {
                                                                    Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                                }).FirstOrDefault().Expr1))
                                                    ),
                        TOTAL_PENDENTE = 0,
                        VALOR_AJUSTE = ( ((decimal)((from ci in db.CAIXA_ITEM
                                                   where ci.ID_CAIXA == c.ID_CAIXA && ci.STATUS == "A"
                                                   group ci by new
                                                   {
                                                       ci.ID_CAIXA
                                                   } into g
                                                   select new
                                                   {
                                                       Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                   }).FirstOrDefault().Expr1)) - c.VALOR_FECHAMENTO )
                    }).Where(c => DbFunctions.TruncateTime(c.DATA_ABERTURA) == DbFunctions.TruncateTime(dataAbertura) && c.DATA_FECHAMENTO == null && c.LOGIN == login).ToList(); */
        }

        // GET api/caixa/aberto/Login/PABLO
        [Route("api/caixa/aberto/Login/{login}")]
        public IEnumerable<CAIXADTO> GetCAIXA_ABERTO_LOGIN(string login)
        {
            sql = sql + "'" + login + "'" +
                  " AND DATA_FECHAMENTO IS NULL ";
            return db.Database.SqlQuery<CAIXADTO>(sql).ToList();

            /*return (from c in db.CAIXAs
                    select new CAIXADTO()
                    {
                        ID_CAIXA = c.ID_CAIXA,
                        LOGIN = c.LOGIN,
                        DATA_ABERTURA = c.DATA_ABERTURA,
                        VALOR_ABERTURA = c.VALOR_ABERTURA,
                        DATA_FECHAMENTO = c.DATA_FECHAMENTO,
                        VALOR_FECHAMENTO = c.VALOR_FECHAMENTO,
                        CONFERIDO = c.CONFERIDO,
                        TOTAL_CARTAO = (
                                           ((decimal)((from ci in db.CAIXA_ITEM
                                                       where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 2 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                       group ci by new
                                                       {
                                                           ci.ID_CAIXA
                                                       } into g
                                                       select new
                                                       {
                                                           Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                       }).FirstOrDefault().Expr1)) -
((decimal)((from ci in db.CAIXA_ITEM
            where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 2 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
            group ci by new
            {
                ci.ID_CAIXA
            } into g
            select new
            {
                Expr1 = (decimal)g.Sum(p => p.VALOR)
            }).FirstOrDefault().Expr1))
                                                   ),
                        TOTAL_CHEQUE = (
                                            ((decimal)((from ci in db.CAIXA_ITEM
                                                        where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 1 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                        group ci by new
                                                        {
                                                            ci.ID_CAIXA
                                                        } into g
                                                        select new
                                                        {
                                                            Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                        }).FirstOrDefault().Expr1)) -
                                                  ((decimal)((from ci in db.CAIXA_ITEM
                                                              where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 1 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                              group ci by new
                                                              {
                                                                  ci.ID_CAIXA
                                                              } into g
                                                              select new
                                                              {
                                                                  Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                              }).FirstOrDefault().Expr1))
                                                  ),
                        TOTAL_DINHEIRO = (
                                            ((decimal)((from ci in db.CAIXA_ITEM
                                                        where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 0 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                        group ci by new
                                                        {
                                                            ci.ID_CAIXA
                                                        } into g
                                                        select new
                                                        {
                                                            Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                        }).FirstOrDefault().Expr1)) -
                                                    ((decimal)((from ci in db.CAIXA_ITEM
                                                                where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 0 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                                group ci by new
                                                                {
                                                                    ci.ID_CAIXA
                                                                } into g
                                                                select new
                                                                {
                                                                    Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                                }).FirstOrDefault().Expr1))
                                                    ),
                        TOTAL_BOLETO = (
                                            ((decimal)((from ci in db.CAIXA_ITEM
                                                        where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 3 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                        group ci by new
                                                        {
                                                            ci.ID_CAIXA
                                                        } into g
                                                        select new
                                                        {
                                                            Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                        }).FirstOrDefault().Expr1)) -
                                                    ((decimal)((from ci in db.CAIXA_ITEM
                                                                where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 3 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                                group ci by new
                                                                {
                                                                    ci.ID_CAIXA
                                                                } into g
                                                                select new
                                                                {
                                                                    Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                                }).FirstOrDefault().Expr1))
                                                    ),
                        TOTAL_PENDENTE = 0,
                        VALOR_AJUSTE = (((decimal)((from ci in db.CAIXA_ITEM
                                                    where ci.ID_CAIXA == c.ID_CAIXA && ci.STATUS == "A"
                                                    group ci by new
                                                    {
                                                        ci.ID_CAIXA
                                                    } into g
                                                    select new
                                                    {
                                                        Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                    }).FirstOrDefault().Expr1)) - c.VALOR_FECHAMENTO)
                    }).Where(c => c.DATA_FECHAMENTO == null && c.LOGIN == login).ToList(); */
        }

        [Route("api/caixa/{login}/{dtInicial}/{dtFinal}")]
        public IEnumerable<CAIXADTO> GetCAIXA(string login, DateTime? dtInicial, DateTime? dtFinal)
        {
            sql = sql + "'" + login + "'" +
                 " AND CAST(DATA_ABERTURA AS DATE) >= cast('" + Convert.ToDateTime(dtInicial).ToString("yyyy-MM-dd") + "' as date)" +
                 " AND CAST(DATA_ABERTURA AS DATE) <= cast('" + Convert.ToDateTime(dtFinal).ToString("yyyy-MM-dd") + "' as date)";
            return db.Database.SqlQuery<CAIXADTO>(sql).ToList();
            /*return (from c in db.CAIXAs
              select new CAIXADTO()
              {
                  ID_CAIXA = c.ID_CAIXA,
                  LOGIN = c.LOGIN,
                  DATA_ABERTURA = c.DATA_ABERTURA,
                  VALOR_ABERTURA = c.VALOR_ABERTURA,
                  DATA_FECHAMENTO = c.DATA_FECHAMENTO,
                  VALOR_FECHAMENTO = c.VALOR_FECHAMENTO,
                  CONFERIDO = c.CONFERIDO,
                  TOTAL_CARTAO = (
                                            ((decimal)((from ci in db.CAIXA_ITEM
                                                        where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 2 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                        group ci by new
                                                        {
                                                            ci.ID_CAIXA
                                                        } into g
                                                        select new
                                                        {
                                                            Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                        }).FirstOrDefault().Expr1)) -
 ((decimal)((from ci in db.CAIXA_ITEM
             where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 2 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
             group ci by new
             {
                 ci.ID_CAIXA
             } into g
             select new
             {
                 Expr1 = (decimal)g.Sum(p => p.VALOR)
             }).FirstOrDefault().Expr1))
                                                    ),
                  TOTAL_CHEQUE = (
                                             ((decimal)((from ci in db.CAIXA_ITEM
                                                         where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 1 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                         group ci by new
                                                         {
                                                             ci.ID_CAIXA
                                                         } into g
                                                         select new
                                                         {
                                                             Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                         }).FirstOrDefault().Expr1)) -
                                                   ((decimal)((from ci in db.CAIXA_ITEM
                                                               where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 1 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                               group ci by new
                                                               {
                                                                   ci.ID_CAIXA
                                                               } into g
                                                               select new
                                                               {
                                                                   Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                               }).FirstOrDefault().Expr1))
                                                   ),
                  TOTAL_DINHEIRO = (
                                             ((decimal)((from ci in db.CAIXA_ITEM
                                                         where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 0 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                         group ci by new
                                                         {
                                                             ci.ID_CAIXA
                                                         } into g
                                                         select new
                                                         {
                                                             Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                         }).FirstOrDefault().Expr1)) -
                                                     ((decimal)((from ci in db.CAIXA_ITEM
                                                                 where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 0 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                                 group ci by new
                                                                 {
                                                                     ci.ID_CAIXA
                                                                 } into g
                                                                 select new
                                                                 {
                                                                     Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                                 }).FirstOrDefault().Expr1))
                                                     ),
                  TOTAL_BOLETO = (
                                             ((decimal)((from ci in db.CAIXA_ITEM
                                                         where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 3 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                         group ci by new
                                                         {
                                                             ci.ID_CAIXA
                                                         } into g
                                                         select new
                                                         {
                                                             Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                         }).FirstOrDefault().Expr1)) -
                                                     ((decimal)((from ci in db.CAIXA_ITEM
                                                                 where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 3 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                                 group ci by new
                                                                 {
                                                                     ci.ID_CAIXA
                                                                 } into g
                                                                 select new
                                                                 {
                                                                     Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                                 }).FirstOrDefault().Expr1))
                                                     ),
                  TOTAL_PENDENTE = 0,
                  VALOR_AJUSTE = (((decimal)((from ci in db.CAIXA_ITEM
                                              where ci.ID_CAIXA == c.ID_CAIXA && ci.STATUS == "A"
                                              group ci by new
                                              {
                                                  ci.ID_CAIXA
                                              } into g
                                              select new
                                              {
                                                  Expr1 = (decimal)g.Sum(p => p.VALOR)
                                              }).FirstOrDefault().Expr1)) - c.VALOR_FECHAMENTO)
              }).Where(c => c.LOGIN == login && DbFunctions.TruncateTime(c.DATA_ABERTURA) >= DbFunctions.TruncateTime(dtInicial) &&
                             DbFunctions.TruncateTime(c.DATA_ABERTURA) <= DbFunctions.TruncateTime(dtFinal)).ToList(); */
        }

        [Route("api/caixa/{dtInicial}/{dtFinal}")]
        public IEnumerable<CAIXADTO> GetCAIXA(DateTime? dtInicial, DateTime? dtFinal)
        {
            string sql1 = "  select ID_CAIXA, LOGIN, DATA_ABERTURA, VALOR_ABERTURA, DATA_FECHAMENTO, VALOR_FECHAMENTO, CONFERIDO, " +
       " (cast(isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 2 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'C'), 0) - " +
"          isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 2 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'D'), 0) as decimal(15,2)) ) AS TOTAL_CARTAO, " +
" (cast(isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 1 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'C'), 0) - " +
"  isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 1 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'D'), 0) as decimal(15,2)) ) AS TOTAL_CHEQUE, " +
" (cast(isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 0 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'C'), 0) - " +
"     isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 0 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'D'), 0) as decimal(15,2)) ) AS TOTAL_DINHEIRO, " +
" (cast(isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 3 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'C'), 0) - " +
"     isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.TIPO_PGTO_CAIXA_ITEM = 3 " +
"                                            AND CAIXA_ITEM.STATUS = 'A' AND CAIXA_ITEM.DEBITO_CREDITO = 'D'), 0) as decimal(15,2)) ) AS TOTAL_BOLETO, " +
" cast(0 as decimal(15,2)) AS TOTAL_PENDENTE, " +
" cast(((isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.STATUS = 'A'  AND CAIXA_ITEM.DEBITO_CREDITO = 'C'), 0) - " +
"                  isnull((SELECT SUM(VALOR) FROM CAIXA_ITEM WHERE CAIXA_ITEM.ID_CAIXA = CAIXA.ID_CAIXA AND CAIXA_ITEM.STATUS = 'A'  AND CAIXA_ITEM.DEBITO_CREDITO = 'D'), 0)) -  " +
"                   VALOR_FECHAMENTO) as decimal(15,2)) AS VALOR_AJUSTE " +
                 " from CAIXA " +
                 " WHERE CAST(DATA_ABERTURA AS DATE) >= cast('" + Convert.ToDateTime(dtInicial).ToString("yyyy-MM-dd") + "' as date)" +
                 " AND CAST(DATA_ABERTURA AS DATE) <= cast('" + Convert.ToDateTime(dtFinal).ToString("yyyy-MM-dd") + "' as date)";
            return db.Database.SqlQuery<CAIXADTO>(sql1).ToList();
            /*  return (from c in db.CAIXAs
                     select new CAIXADTO()
                     {
                         ID_CAIXA = c.ID_CAIXA,
                         LOGIN = c.LOGIN,
                         DATA_ABERTURA = c.DATA_ABERTURA,
                         VALOR_ABERTURA = c.VALOR_ABERTURA,
                         DATA_FECHAMENTO = c.DATA_FECHAMENTO,
                         VALOR_FECHAMENTO = c.VALOR_FECHAMENTO,
                         CONFERIDO = c.CONFERIDO,
                         TOTAL_CARTAO = (
                                            ((decimal)((from ci in db.CAIXA_ITEM
                                                        where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 2 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                        group ci by new
                                                        {
                                                            ci.ID_CAIXA
                                                        } into g
                                                        select new
                                                        {
                                                            Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                        }).FirstOrDefault().Expr1)) -
 ((decimal)((from ci in db.CAIXA_ITEM
             where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 2 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
             group ci by new
             {
                 ci.ID_CAIXA
             } into g
             select new
             {
                 Expr1 = (decimal)g.Sum(p => p.VALOR)
             }).FirstOrDefault().Expr1))
                                                    ),
                         TOTAL_CHEQUE = (
                                             ((decimal)((from ci in db.CAIXA_ITEM
                                                         where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 1 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                         group ci by new
                                                         {
                                                             ci.ID_CAIXA
                                                         } into g
                                                         select new
                                                         {
                                                             Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                         }).FirstOrDefault().Expr1)) -
                                                   ((decimal)((from ci in db.CAIXA_ITEM
                                                               where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 1 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                               group ci by new
                                                               {
                                                                   ci.ID_CAIXA
                                                               } into g
                                                               select new
                                                               {
                                                                   Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                               }).FirstOrDefault().Expr1))
                                                   ),
                         TOTAL_DINHEIRO = (
                                             ((decimal)((from ci in db.CAIXA_ITEM
                                                         where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 0 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                         group ci by new
                                                         {
                                                             ci.ID_CAIXA
                                                         } into g
                                                         select new
                                                         {
                                                             Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                         }).FirstOrDefault().Expr1)) -
                                                     ((decimal)((from ci in db.CAIXA_ITEM
                                                                 where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 0 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                                 group ci by new
                                                                 {
                                                                     ci.ID_CAIXA
                                                                 } into g
                                                                 select new
                                                                 {
                                                                     Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                                 }).FirstOrDefault().Expr1))
                                                     ),
                         TOTAL_BOLETO = (
                                             ((decimal)((from ci in db.CAIXA_ITEM
                                                         where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 3 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "C"
                                                         group ci by new
                                                         {
                                                             ci.ID_CAIXA
                                                         } into g
                                                         select new
                                                         {
                                                             Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                         }).FirstOrDefault().Expr1)) -
                                                     ((decimal)((from ci in db.CAIXA_ITEM
                                                                 where ci.ID_CAIXA == c.ID_CAIXA && ci.TIPO_PGTO_CAIXA_ITEM == 3 && ci.STATUS == "A" && ci.DEBITO_CREDITO == "D"
                                                                 group ci by new
                                                                 {
                                                                     ci.ID_CAIXA
                                                                 } into g
                                                                 select new
                                                                 {
                                                                     Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                                 }).FirstOrDefault().Expr1))
                                                     ),
                         TOTAL_PENDENTE = 0,
                         VALOR_AJUSTE = (((decimal)((from ci in db.CAIXA_ITEM
                                                     where ci.ID_CAIXA == c.ID_CAIXA && ci.STATUS == "A"
                                                     group ci by new
                                                     {
                                                         ci.ID_CAIXA
                                                     } into g
                                                     select new
                                                     {
                                                         Expr1 = (decimal)g.Sum(p => p.VALOR)
                                                     }).FirstOrDefault().Expr1)) - c.VALOR_FECHAMENTO)
                     }).Where(c => DbFunctions.TruncateTime(c.DATA_ABERTURA) >= DbFunctions.TruncateTime(dtInicial) &&
                                   DbFunctions.TruncateTime(c.DATA_ABERTURA) <= DbFunctions.TruncateTime(dtFinal)).ToList(); */
        }

        [Route("api/caixa/valorfechamento/{login}")]
        public decimal GetCAIXA(string login)
        {
            Decimal valorFechamento = db.Database.SqlQuery<Decimal>(" SELECT cast(ISNULL((SELECT SUM(CI.VALOR) TOTAL FROM CAIXA_ITEM CI WHERE CI.DEBITO_CREDITO='C' AND CI.ID_CAIXA = (SELECT ID_CAIXA FROM CAIXA WHERE LOGIN = @LOGIN AND DATA_FECHAMENTO IS NULL)), 0) - "+
                                                                    "             ISNULL((SELECT SUM(CI.VALOR) TOTAL FROM CAIXA_ITEM CI WHERE CI.DEBITO_CREDITO = 'D' AND CI.ID_CAIXA = (SELECT ID_CAIXA FROM CAIXA WHERE LOGIN = @LOGIN AND DATA_FECHAMENTO IS NULL)), 0) as decimal(15,2)) AS VALOR_FECHAMENTO",
                                                                        new SqlParameter("@LOGIN", login)).First();
            return valorFechamento;
        }


        // PUT: api/Caixas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCAIXA(int id, CAIXA cAIXA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cAIXA.ID_CAIXA)
            {
                return BadRequest();
            }

            db.Entry(cAIXA).State = EntityState.Modified;

            try
            {
               // db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CAIXAExists(id))
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

        // POST: api/Caixas
        [ResponseType(typeof(CAIXA))]
        public IHttpActionResult PostCAIXA(CAIXA cAIXA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_CAIXA;").First();

            cAIXA.ID_CAIXA = NextValue;

            db.CAIXAs.Add(cAIXA);

            try
            {
               // db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CAIXAExists(cAIXA.ID_CAIXA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cAIXA.ID_CAIXA }, cAIXA);
        }

        // DELETE: api/Caixas/5
        [ResponseType(typeof(CAIXA))]
        public IHttpActionResult DeleteCAIXA(int id)
        {
            CAIXA cAIXA = db.CAIXAs.Find(id);
            if (cAIXA == null)
            {
                return NotFound();
            }

            db.CAIXAs.Remove(cAIXA);
            //db.BulkSaveChanges();
            db.SaveChanges();

            return Ok(cAIXA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CAIXAExists(int id)
        {
            return db.CAIXAs.Count(e => e.ID_CAIXA == id) > 0;
        }
    }
}