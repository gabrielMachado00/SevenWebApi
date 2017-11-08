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
using SevenMedicalApi.Models;
using SisMedApi.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace SevenMedicalApi.Controllers
{
    public class V_COMISSOESDTO
    {
        public string PROFISSIONAL { get; set; }
        public string DESCRICAO { get; set; }
        public string REGIAO { get; set; }
        public string CLIENTE { get; set; }
        public decimal VALOR_COMISSAO { get; set; }
        public Nullable<decimal> VALOR_BASE { get; set; }
        public Nullable<decimal> VALOR_FATOR { get; set; }
        public Nullable<DateTime> DATA_INCLUSAO { get; set; }
        public Nullable<DateTime> DATA_PAGAMENTO { get; set; }
        public string LOGIN { get; set; }
        public string OBSERVACAO { get; set; }
        public Nullable<DateTime> DATA_VENDA { get; set; }
        public Nullable<decimal> VALOR_TOTAL_FATURADO { get; set; }                
        public string STATUS { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public Nullable<int> ID_PROCEDIMENTO { get; set; }
        public Nullable<int> ID_REGIAO_CORPO { get; set; }
        public Nullable<int> ID_VENDA { get; set; }
        public string FATOR_COMISSAO { get; set; }
        public string DESC_FATOR_COMISSAO { get; set; }
        public Nullable<int> ID_ATENDIMENTO { get; set; }
        public int ID_COMISSAO { get; set; }
        public Nullable<int> FORMA_COMISSAO { get; set; }        
        public string DESC_FORMA_COMISSAO { get; set; }
        public Nullable<int> ID_DOCUMENTO { get; set; }
    }

    public class COMISSAO_NAO_GERADA
    {
        public int ID_PROFISSIONAL { get; set; }
        public string PROFISSIONAL { get; set; }
        public int ID_VENDA { get; set; }
        public Nullable<DateTime> DATA_FATURAMENTO { get; set; }
        public Nullable<decimal> VALOR_PAGO { get; set; }
        public string PROCEDIMENTO { get; set; }
        public string REGIAO { get; set; }
        public string FORMA_PG { get; set; }
        public string TIPO_COMISSAO { get; set; }
        public Nullable<decimal> VALOR_COMISSAO { get; set; }        
    }
        

    public class VComissoesController : ApiController
    {
        private SisMedContext db = new SisMedContext();
        private string sql_ComissoesNaoGeradas = @"SELECT VI.ID_PROFISSIONAL, PF.NOME PROFISSIONAL, V.ID_VENDA, V.DATA DATA_FATURAMENTO, 
                                                                       VI.VALOR_PAGO, P.DESCRICAO PROCEDIMENTO, RC.REGIAO, dbo.GET_FORMAS_PAGAMENTO(V.ID_VENDA) FORMA_PG,
	                                                                   CASE WHEN (PRC.PERCENTUAL IS NULL) THEN 
	                                                                      CASE WHEN (PRC.VALOR IS NULL) THEN '' ELSE 'VALOR' END
	                                                                   ELSE 'PERCENTUAL' END AS TIPO_COMISSAO, 
	                                                                   CASE WHEN (ISNULL(PRC.PERCENTUAL, 0) = 0 ) THEN 
	                                                                      CASE WHEN (ISNULL(PRC.VALOR, 0) = 0) THEN 0 ELSE PRC.VALOR END
	                                                                   ELSE PRC.PERCENTUAL END AS VALOR_COMISSAO
                                                                FROM VENDA_ITEM VI
	                                                                 LEFT JOIN PROFISSIONAL_COMISSAO PRC 
	                                                                  ON PRC.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO AND PRC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
                                                                          AND PRC.ID_PROFISSIONAL = VI.ID_PROFISSIONAL,
                                                                     VENDA V, REGIAO_CORPO RC, PROCEDIMENTO P, PROFISSIONAL PF		 
                                                                WHERE PF.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
                                                                  AND P.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO
                                                                  AND RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
                                                                  AND VI.ID_VENDA = V.ID_VENDA  
                                                                  AND V.STATUS = 'C'
                                                                  AND VI.VALOR_PAGO > 0
                                                                  AND NOT EXISTS(SELECT CP.ID_COMISSAO FROM COMISSAO_PAGAMENTO CP
				                                                                  WHERE CP.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
					                                                                AND CP.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO
					                                                                AND CP.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
					                                                                AND CP.ID_VENDA = VI.ID_VENDA)                                                                  
                                                                  AND (@ID_PROFISSIONAL = -1 OR VI.ID_PROFISSIONAL = @ID_PROFISSIONAL)
                                                                  AND (@ID_PROCEDIMENTO = -1 OR VI.ID_PROCEDIMENTO = @ID_PROCEDIMENTO) 
                                                                  AND (@ID_REGIAO_CORPO = -1 OR VI.ID_REGIAO_CORPO = @ID_REGIAO_CORPO) 
                                                                UNION ALL
                                                                SELECT VI.ID_PROFISSIONAL, PF.NOME PROFISSIONAL, V.ID_VENDA, V.DATA DATA_FATURAMENTO, 
                                                                        VI.VALOR_PAGO, P.DESCRICAO PROCEDIMENTO, RC.REGIAO, dbo.GET_FORMAS_PAGAMENTO(V.ID_VENDA) FORMA_PG,
	                                                                    CASE WHEN (PRC.PERCENTUAL IS NULL) THEN 
	                                                                        CASE WHEN (PRC.VALOR IS NULL) THEN '' ELSE 'VALOR' END
	                                                                    ELSE 'PERCENTUAL' END AS TIPO_COMISSAO, 
	                                                                    CASE WHEN (ISNULL(PRC.PERCENTUAL, 0) = 0 ) THEN 
	                                                                        CASE WHEN (ISNULL(PRC.VALOR, 0) = 0) THEN 0 ELSE PRC.VALOR END
	                                                                    ELSE PRC.PERCENTUAL END AS VALOR_COMISSAO
                                                                FROM VENDA_ITEM VI
	                                                                    LEFT JOIN PROFISSIONAL_COMISSAO PRC 
	                                                                    ON PRC.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO AND PRC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
                                                                            AND PRC.ID_PROFISSIONAL = VI.ID_PROFISSIONAL,
                                                                        VENDA V, REGIAO_CORPO RC, PROCEDIMENTO P, PROFISSIONAL PF, COMISSAO_PAGAMENTO CP		 
                                                                WHERE PF.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
                                                                    AND P.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO
                                                                    AND RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
                                                                    AND VI.ID_VENDA = V.ID_VENDA  
                                                                    AND V.STATUS = 'C'
                                                                    AND CP.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
	                                                                AND CP.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO
	                                                                AND CP.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
	                                                                AND CP.ID_VENDA = VI.ID_VENDA
	                                                                AND CP.VALOR_COMISSAO = 0
                                                                    AND CP.VALOR_BASE > 0 
                                                                    AND (@ID_PROFISSIONAL = -1 OR VI.ID_PROFISSIONAL = @ID_PROFISSIONAL)
                                                                    AND (@ID_PROCEDIMENTO = -1 OR VI.ID_PROCEDIMENTO = @ID_PROCEDIMENTO) 
                                                                    AND (@ID_REGIAO_CORPO = -1 OR VI.ID_REGIAO_CORPO = @ID_REGIAO_CORPO) ";

        [Route("api/comissoes/{dtInicial}/{dtFinal}/{idProfissional}/{idProcedimento}/{idRegiaCorpo}/{financeiro}/{periodo}")]
        public IEnumerable<V_COMISSOESDTO> GetCOMISSOES(DateTime? dtInicial, DateTime? dtFinal, int idProfissional,
                                                        int idProcedimento, int idRegiaCorpo, string financeiro, string periodo)
        {
            return (from c in db.V_COMISSOES
                    select new V_COMISSOESDTO()
                    {
                        PROFISSIONAL = c.PROFISSIONAL,
                        DESCRICAO = c.DESCRICAO,
                        REGIAO = c.REGIAO,
                        CLIENTE = c.CLIENTE,
                        VALOR_COMISSAO = c.VALOR_COMISSAO,
                        VALOR_BASE = c.VALOR_BASE,
                        VALOR_FATOR = c.VALOR_FATOR,
                        DATA_INCLUSAO = c.DATA_INCLUSAO,
                        DATA_PAGAMENTO = c.DATA_PAGAMENTO,
                        LOGIN = c.LOGIN,
                        OBSERVACAO = c.OBSERVACAO,
                        DATA_VENDA = c.DATA_VENDA,
                        VALOR_TOTAL_FATURADO = c.VALOR_TOTAL_FATURADO,                        
                        ID_PROFISSIONAL = c.ID_PROFISSIONAL,
                        ID_PROCEDIMENTO = c.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = c.ID_REGIAO_CORPO,
                        ID_VENDA = c.ID_VENDA,
                        FATOR_COMISSAO = c.FATOR_COMISSAO,
                        DESC_FATOR_COMISSAO = c.FATOR_COMISSAO == "P" ? "PERCENTUAL" : "VALOR",
                        ID_ATENDIMENTO = c.ID_ATENDIMENTO ,
                        ID_COMISSAO = c.ID_COMISSAO,
                        FORMA_COMISSAO = c.FORMA_COMISSAO,
                        DESC_FORMA_COMISSAO = c.FORMA_COMISSAO == null? "": 
                                              (c.FORMA_COMISSAO == 1 ? "Faturamento" : 
                                                c.FORMA_COMISSAO == 2 ? "Atendimento" : 
                                                    c.FORMA_COMISSAO == 3 ? "Venda" : "Fluxo de caixa"),
                        STATUS = c.STATUS
                    }).Where(c => (periodo == "A" ? (DbFunctions.TruncateTime(c.DATA_PAGAMENTO) >= DbFunctions.TruncateTime(dtInicial) &&
                                                     DbFunctions.TruncateTime(c.DATA_PAGAMENTO) <= DbFunctions.TruncateTime(dtFinal) &&
                                                     c.FORMA_COMISSAO == 2) :
                                      (periodo == "V" ? (DbFunctions.TruncateTime(c.DATA_PAGAMENTO) >= DbFunctions.TruncateTime(dtInicial) &&
                                                     DbFunctions.TruncateTime(c.DATA_PAGAMENTO) <= DbFunctions.TruncateTime(dtFinal)) :
                                                   (DbFunctions.TruncateTime(c.DATA_INCLUSAO) >= DbFunctions.TruncateTime(dtInicial) &&
                                                     DbFunctions.TruncateTime(c.DATA_INCLUSAO) <= DbFunctions.TruncateTime(dtFinal)))) &&
                                  c.ID_PROFISSIONAL == (idProfissional != 0 ? idProfissional : c.ID_PROFISSIONAL) &&
                                  c.ID_PROCEDIMENTO == (idProcedimento != 0 ? idProcedimento : c.ID_PROCEDIMENTO) &&
                                  c.ID_REGIAO_CORPO == (idRegiaCorpo != 0 ? idRegiaCorpo : c.ID_REGIAO_CORPO) &&
                                  (financeiro == "T"? c.STATUS == "P" : c.STATUS != "P") ).ToList();

        }

        [Route("api/comissoes/{idProfissional}")]
        public IEnumerable<COMISSAO_NAO_GERADA> GetCOMISSOES_NAOGERADAS(int idProfissional)
        {
            return db.Database.SqlQuery<COMISSAO_NAO_GERADA>(sql_ComissoesNaoGeradas,
                                                           new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                           new SqlParameter("@ID_PROCEDIMENTO", -1),
                                                           new SqlParameter("@ID_REGIAO_CORPO", -1)).ToList();
        }
        
        [Route("api/comissoes/gerarComissoes/{idProfissional}/{login}")]
        public IEnumerable<COMISSAO_NAO_GERADA> GetGERAR_COMISSOES(int idProfissional, string login)
        {
            return GerarComissao(idProfissional, login, -1, -1);
        }

        [Route("api/comissoes/gerarComissoesPorProcedimento/{idProfissional}/{idProcedimento}/{idRegiaoCorpo}/{login}")]
        public IEnumerable<COMISSAO_NAO_GERADA> GetGERAR_COMISSOES_PROCEDIMENTO(int idProfissional, int idProcedimento, int idRegiaoCorpo, string login)
        {
            return GerarComissao(idProfissional, login, idProcedimento, idRegiaoCorpo);
        }

        public IEnumerable<COMISSAO_NAO_GERADA> GerarComissao(int idProfissional, string login, int idProcedimento, int idRegiaoCorpo)
        {
            IEnumerable<COMISSAO_NAO_GERADA> lista = db.Database.SqlQuery<COMISSAO_NAO_GERADA>(sql_ComissoesNaoGeradas,
                                                           new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                           new SqlParameter("@ID_PROCEDIMENTO", idProcedimento),
                                                           new SqlParameter("@ID_REGIAO_CORPO", idRegiaoCorpo)).ToList();

            foreach (COMISSAO_NAO_GERADA reg in lista)
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SisMedContext"].ConnectionString))
                {
                    string msg;
                    try
                    {
                        SqlCommand command = new SqlCommand("[dbo].[GERA_COMISSOES_PAGAMENTO]", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PLOGIN", SqlDbType.VarChar)).Value = login;
                        command.Parameters.Add(new SqlParameter("@PID_VENDA", SqlDbType.Int)).Value = reg.ID_VENDA;
                        command.Parameters.Add(new SqlParameter("@PDATA_GERACAO", SqlDbType.DateTime)).Value = reg.DATA_FATURAMENTO;
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        msg = e.Message;
                    }
                }
            }
            return db.Database.SqlQuery<COMISSAO_NAO_GERADA>(sql_ComissoesNaoGeradas,
                                                           new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                           new SqlParameter("@ID_PROCEDIMENTO", idProcedimento),
                                                           new SqlParameter("@ID_REGIAO_CORPO", idRegiaoCorpo)).ToList();
        }

        // PUT: api/VComissoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutV_COMISSOES(int id, V_COMISSOES v_COMISSOES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != v_COMISSOES.ID_COMISSAO)
            {
                return BadRequest();
            }

            db.Entry(v_COMISSOES).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!V_COMISSOESExists(id))
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

        [ResponseType(typeof(void))]
        [Route("api/comissao/PutCOMISSAO_PAGAMENTO/{id}")]
        public IHttpActionResult PutCOMISSAO_PAGAMENTO(int id, V_COMISSOESDTO comissao)
        {
            if (!V_COMISSOESExists(id))
            {
                return NotFound();
            }
            else
            {
                try
                {
                    var sql = @"UPDATE COMISSAO_PAGAMENTO 
                               SET STATUS = {0}, 
                                   LOGIN = {1},
                                   DATA_ATUALIZACAO = GETDATE(),
                                   ID_DOCUMENTO = {2}
                             WHERE ID_COMISSAO = {3} ";

                    return Ok(db.Database.ExecuteSqlCommand(sql, comissao.STATUS, comissao.LOGIN, comissao.ID_DOCUMENTO, id.ToString()) > 0);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;                    
                }
            }            
        }

        // POST: api/VComissoes
        [ResponseType(typeof(V_COMISSOES))]
        public IHttpActionResult PostV_COMISSOES(V_COMISSOES v_COMISSOES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.V_COMISSOES.Add(v_COMISSOES);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (V_COMISSOESExists(v_COMISSOES.ID_COMISSAO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = v_COMISSOES.PROFISSIONAL }, v_COMISSOES);
        }

        // DELETE: api/VComissoes/5
        [ResponseType(typeof(V_COMISSOES))]
        public IHttpActionResult DeleteV_COMISSOES(string id)
        {
            V_COMISSOES v_COMISSOES = db.V_COMISSOES.Find(id);
            if (v_COMISSOES == null)
            {
                return NotFound();
            }

            db.V_COMISSOES.Remove(v_COMISSOES);
            db.SaveChanges();

            return Ok(v_COMISSOES);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool V_COMISSOESExists(int id)
        {
            return db.V_COMISSOES.Count(e => e.ID_COMISSAO == id) > 0;
        }
    }
}