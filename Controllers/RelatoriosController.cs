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
using System.Web.Http.Cors;
using System.Data.SqlClient;


namespace SevenMedicalApi.Controllers
{
    public class HISTORICO_ATENDIMENTO
    {
        public int ID_CLIENTE { get; set; }
        public string CLIENTE { get; set; }
        public string PROCEDIMENTO { get; set; }
        public Nullable<DateTime> DATA_INI { get; set; }
        public Nullable<DateTime> DATA_FIM { get; set; }
        public string REGIAO { get; set; }
        public string PROFISSIONAL { get; set; }
        public string SESSOES { get; set; }
    }

    public class HISTORICO_FINANCEIRO
    {
        public int ID_CLIENTE { get; set; }
        public string CLIENTE { get; set; }
        public string PROCEDIMENTO { get; set; }
        public string REGIAO { get; set; }
        public Nullable<DateTime> DATA { get; set; }                
        public string PROFISSIONAL { get; set; }
        public decimal VALOR { get; set; }
        public string FORMA_PGTO { get; set; }
        public string SITUACAO { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public Nullable<int> ID_APPOINTMENTS { get; set; }
        public Nullable<int> ID_ATENDIMENTO { get; set; }
        public Nullable<int> ID_ATENDIMENTO_PROCEDIMENTO { get; set; }
        public Nullable<int> NUM_SESSOES { get; set; }
    }

    public class INADIMPLENTES
    {
        public int ID_CLIENTE { get; set; }
        public string CLIENTE { get; set; }
        public decimal VALOR { get; set; }
        public decimal SALDO { get; set; }
        public DateTime DATA_VENCIMENTO { get; set; }
        public DateTime DATA_ATUALIZACAO { get; set; }
        public string NUM_DOCUMENTO { get; set; }
        public int ID_DOCUMENTO { get; set; }
        public string STATUS { get; set; }
        public Nullable<int> ID_CONTA { get; set; }
    }

    public class RelatoriosController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [Route("api/Relatorios/HistoricoAtendimento/{dtInicial}/{dtFinal}/{idProfissional}/{idCliente}")]
        public IEnumerable<HISTORICO_ATENDIMENTO> GetHISTORICO_ATENDIMENTO(DateTime dtInicial, DateTime dtFinal, int idProfissional, int idCliente)
        {
            return db.Database.SqlQuery<HISTORICO_ATENDIMENTO>(@"SELECT ID_CLIENTE, CLIENTE, DATA_INI, DATA_FIM, PROCEDIMENTO, REGIAO, PROFISSIONAL, SESSOES
                                                                  FROM V_HISTORICO_ATENDIMENTO
                                                                 WHERE CAST(DATA_INI AS DATE) >= CAST(@DATA_INI as date)
                                                                   AND CAST(DATA_INI AS DATE) <= CAST(@DATA_FIM as date)
                                                                   AND (ID_PROFISSIONAL = @ID_PROFISSIONAL OR @ID_PROFISSIONAL = -1)
                                                                   AND (ID_CLIENTE = @ID_CLIENTE OR @ID_CLIENTE = -1)
                                                                 ORDER BY DATA_INI, CLIENTE",
                                                            new SqlParameter("@DATA_INI", dtInicial.Date),
                                                            new SqlParameter("@DATA_FIM", dtFinal.Date),
                                                            new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                            new SqlParameter("@ID_CLIENTE", idCliente)).ToList();
        }

        [Route("api/Relatorios/HistoricoFinanceiro/{dtInicial}/{dtFinal}/{idProfissional}/{idCliente}/{situacao}")]
        public IEnumerable<HISTORICO_FINANCEIRO> GetHISTORICO_FINANCEIRO(DateTime dtInicial, DateTime dtFinal, int idProfissional, int idCliente, int situacao)
        {
            string sql_pgto = @"SELECT PG.ID_CLIENTE, PG.CLIENTE, PG.PROCEDIMENTO, PG.REGIAO, 
                                        PG.DATA_PAGAMENTO DATA,  PG.PROFISSIONAL, PG.VALOR_PAGO VALOR, 
                                        dbo.GET_FORMAS_PAGAMENTO(ID_VENDA) FORMA_PGTO, 'PAGO' SITUACAO,
                                        ID_PROFISSIONAL, ID_PROCEDIMENTO, ID_REGIAO_CORPO,
                                        ID_APPOINTMENT as ID_APPOINTMENTS, NULL ID_ATEDIMENTO, NULL ID_ATENDIMENTO_PROCEDIMENTO,
                                        NULL NUM_SESSOES        
                                 FROM dbo.V_PAGAMENTOS PG
                                 WHERE STATUS = 'C'
                                    AND CAST(PG.DATA_PAGAMENTO AS DATE) >= CAST(@DATA_INI as date)
                                    AND CAST(PG.DATA_PAGAMENTO AS DATE) <= CAST(@DATA_FIM as date)   
                                    AND (ID_PROFISSIONAL = @ID_PROFISSIONAL OR @ID_PROFISSIONAL = -1)                                                               
                                    AND (ID_CLIENTE = @ID_CLIENTE OR @ID_CLIENTE = -1)
                                ORDER BY PG.DATA_PAGAMENTO, CLIENTE";

            string sql_pendente = @"SELECT PP.ID_CLIENTE, PP.CLIENTE, PP.PROCEDIMENTO, PP.REGIAO, 
                                        DATA_ATENDIMENTO DATA, PP.PROFISSIONAL, PP.VALOR_PAGAR VALOR, 
                                        NULL FORMA_PGTO, 'PENDENTE' SITUACAO, 
                                        ID_PROFISSIONAL, ID_PROCEDIMENTO, ID_REGIAO_CORPO, ID_APPOINTMENT ID_APPOINTMENTS,
                                        PP.ID_ATENDIMENTO, PP.ID_ATENDIMENTO_PROCEDIMENTO, PP.NUM_SESSOES
                                    FROM dbo.V_PAGAMENTO_PENDENTE PP
                                    WHERE CAST(DATA_ATENDIMENTO AS DATE) >= CAST(@DATA_INI as date)
                                    AND CAST(DATA_ATENDIMENTO AS DATE) <= CAST(@DATA_FIM as date) 
                                    AND (ID_PROFISSIONAL = @ID_PROFISSIONAL OR @ID_PROFISSIONAL = -1)                                                                 
                                    AND (ID_CLIENTE = @ID_CLIENTE OR @ID_CLIENTE = -1)
                                ORDER BY DATA_ATENDIMENTO, CLIENTE";

            string sql_todos = @"SELECT ID_CLIENTE, CLIENTE, PROCEDIMENTO, REGIAO, 
                                        DATA, PROFISSIONAL, VALOR, FORMA_PGTO,  SITUACAO, 
                                        ID_PROFISSIONAL, ID_PROCEDIMENTO, ID_REGIAO_CORPO,
                                        ID_APPOINTMENTS, ID_ATEDIMENTO, ID_ATENDIMENTO_PROCEDIMENTO,
                                        NUM_SESSOES
                                    FROM (                                                                
                                SELECT PG.ID_CLIENTE, PG.CLIENTE, PG.PROCEDIMENTO, PG.REGIAO, 
                                        PG.DATA_PAGAMENTO DATA,  PG.PROFISSIONAL, PG.VALOR_PAGO VALOR, 
                                        dbo.GET_FORMAS_PAGAMENTO(ID_VENDA) FORMA_PGTO, 'PAGO' SITUACAO,
                                        ID_PROFISSIONAL, ID_PROCEDIMENTO, ID_REGIAO_CORPO,
                                        PG.ID_APPOINTMENT AS ID_APPOINTMENTS, NULL ID_ATEDIMENTO, NULL ID_ATENDIMENTO_PROCEDIMENTO,
                                        NULL NUM_SESSOES
                                    FROM dbo.V_PAGAMENTOS PG
                                WHERE STATUS = 'C'
                                    AND CAST(PG.DATA_PAGAMENTO AS DATE) >= CAST(@DATA_INI as date)
                                    AND CAST(PG.DATA_PAGAMENTO AS DATE) <= CAST(@DATA_FIM as date) 
                                    AND (ID_PROFISSIONAL = @ID_PROFISSIONAL OR @ID_PROFISSIONAL = -1)                                                                 
                                    AND (ID_CLIENTE = @ID_CLIENTE OR @ID_CLIENTE = -1) 
                                UNION ALL
                                SELECT PP.ID_CLIENTE, PP.CLIENTE, PP.PROCEDIMENTO, PP.REGIAO, 
                                        DATA_ATENDIMENTO DATA, PP.PROFISSIONAL, PP.VALOR_PAGAR VALOR, 
                                        NULL FORMA_PGTO, 'PENDENTE' SITUACAO,
                                        ID_PROFISSIONAL, ID_PROCEDIMENTO, ID_REGIAO_CORPO,
                                        PP.ID_APPOINTMENT, PP.ID_ATENDIMENTO, PP.ID_ATENDIMENTO_PROCEDIMENTO, PP.NUM_SESSOES
                                    FROM dbo.V_PAGAMENTO_PENDENTE PP
                                    WHERE CAST(DATA_ATENDIMENTO AS DATE) >= CAST(@DATA_INI as date)
                                    AND CAST(DATA_ATENDIMENTO AS DATE) <= CAST(@DATA_FIM as date)
                                    AND (ID_PROFISSIONAL = @ID_PROFISSIONAL OR @ID_PROFISSIONAL = -1)                                                                  
                                    AND (ID_CLIENTE = @ID_CLIENTE OR @ID_CLIENTE = -1)) HI
                                ORDER BY DATA, CLIENTE";

            string sql = string.Empty;            
            switch (situacao)
            {
                case 1:
                    sql = sql_pgto;
                    break;
                case 2:
                    sql = sql_pendente;
                    break;
                default:
                    sql = sql_todos;
                    break;
            }   
            return db.Database.SqlQuery<HISTORICO_FINANCEIRO>(sql,
                                                            new SqlParameter("@DATA_INI", dtInicial.Date),
                                                            new SqlParameter("@DATA_FIM", dtFinal.Date),
                                                            new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                            new SqlParameter("@ID_CLIENTE", idCliente)).ToList();
        }

        [Route("api/Relatorios/Inadimplemente/{dtInicial}/{dtFinal}/{idCliente}")]
        public IEnumerable<INADIMPLENTES> GetINADIMPLENTE(DateTime dtInicial, DateTime dtFinal, int idCliente)
        {
            return db.Database.SqlQuery<INADIMPLENTES>(@"SELECT DC.ID_CLIENTE, C.NOME CLIENTE, isnull(VALOR, 0.0) VALOR, SALDO, DC.DATA_VENCIMENTO, 
                                                                DC.DATA_ATUALIZACAO,  NUM_DOCUMENTO, ID_DOCUMENTO, STATUS, ID_CONTA
                                                           FROM DOCUMENTO_FINANCEIRO DC, CLIENTE C
                                                          WHERE C.ID_CLIENTE = DC.ID_CLIENTE 
                                                            AND CAST(DC.DATA_VENCIMENTO AS DATE) >= CAST(@DATA_INI as date)
                                                            AND CAST(DC.DATA_VENCIMENTO AS DATE) <= CAST(@DATA_FIM as date)
                                                            AND CAST(DC.DATA_VENCIMENTO AS DATE) < CAST(GETDATE() AS DATE)
                                                            AND SALDO > 0
                                                            AND TIPO = 'C'",
                                                            new SqlParameter("@DATA_INI", dtInicial.Date),
                                                            new SqlParameter("@DATA_FIM", dtFinal.Date),
                                                            new SqlParameter("@ID_CLIENTE", idCliente)).ToList();
        }

    }
}
