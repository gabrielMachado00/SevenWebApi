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

namespace SevenMedicalApi.Controllers
{
    public class TOTAIS_VENDA
    {
        public decimal? TOTAL_ITEM { get; set; }
        public decimal? TOTAL_PAGO { get; set; }
    }

    public class NUM_DOCTODTO
    {
        public int? ID { get; set; }
        public string DESCRICAO { get; set; }

    }
    public class LOG_FATURAMENTO_FINANCEIRODTO
    {
        public int ID_LOG_FATURAMENTO_FINANCEIRO { get; set; }
        public int? ID_PARENT { get; set; }
        public int? ID_PARENT_ITEM { get; set; }
        public DateTime DATA_HORA { get; set; }
        public string ROTINA { get; set; }
        public string STATUS { get; set; }
        public string DESCRICAO { get; set; }
        public string LOGIN { get; set; }
    }
    public class VENDADTO
    {
        public int ID_VENDA { get; set; }
        public int ID_CLIENTE { get; set; }
        public DateTime DATA { get; set; }
        public decimal VALOR { get; set; }
        public string LOGIN { get; set; }
        public Nullable<DateTime> DATA_ATUALIZACAO { get; set; }
        public string STATUS { get; set; }
        public string CLIENTE { get; set; }
        public string DESC_STATUS { get; set; }
        public string PROCEDIMENTO { get; set; }
        public string REGIAO { get; set; }
        public string PROFISSIONAL { get; set; }
        public Nullable<bool> PACOTE { get; set; }
        public Nullable<decimal> VALOR_PAGO { get; set; }
        public Nullable<int> ID_PROFISSIONAL { get; set; }
        public Nullable<int> ID_PROCEDIMENTO { get; set; }
        public Nullable<bool> COMISSAO_GERADA { get; set; }
        public Nullable<DateTime> DATA_COMISSAO_GERADA { get; set; }
        public string LOGIN_COMISSAO_GERADA { get; set; }
        public Nullable<int> ID_USUARIO { get; set; }
        public Nullable<decimal> VALOR_PAGO_CARTAO { get; set; }
        public string TIPO_PGTO { get; set; }
        public string TIPO_CARTAO { get; set; }
    }
    public class VendaController : ApiController
    {
        private SisMedContext db = new SisMedContext();


        [Route("api/venda/docto")]
        public IEnumerable<NUM_DOCTODTO> GetVENDASDOCTO()
        {
            string sql = @"SELECT ID_PARENT AS ID, CASE ROTINA WHEN 'F' THEN 'FATURAMENTO' WHEN 'D' THEN 'FINANCEIRO' END AS DESCRICAO
                             FROM LOG_FATURAMENTO_FINANCEIRO WHERE 1=1";            

            return db.Database.SqlQuery<NUM_DOCTODTO>(sql).ToList();
        }
        [Route("api/venda/docto/{dtInicial}/{dtFinal}/{rotina}/{status}/{numDocto}")]
        public IEnumerable<LOG_FATURAMENTO_FINANCEIRODTO> GetVENDASDOCTO(string dtInicial, string dtFinal, string rotina, string status, string numDocto)
        {
            string sql = @"SELECT ID_LOG_FATURAMENTO_FINANCEIRO, ID_PARENT, ID_PARENT_ITEM, DATA_HORA, CASE ROTINA WHEN 'F' THEN 'FATURAMENTO' WHEN 'D' THEN 'FINANCEIRO' END AS ROTINA, CASE STATUS WHEN 'A' THEN 'ALTERAÇÃO' WHEN 'E' THEN 'EXCLUSÃO' END AS STATUS, DESCRICAO, LOGIN
                             FROM LOG_FATURAMENTO_FINANCEIRO WHERE 1=1";

            if (dtInicial != "-" && dtFinal != "-")
                sql += "AND CAST(DATA_HORA as DATE) BETWEEN CAST('" + dtInicial + "' AS DATE) AND CAST('" + dtFinal + "' AS DATE)";

            if (rotina != "-")
                sql += " AND ROTINA = '" + rotina + "'";
            if (status != "-")
                sql += " AND STATUS = '" + status + "'";
            if (numDocto != "-")
                sql += " AND ID_PARENT = " + numDocto + "";

            return db.Database.SqlQuery<LOG_FATURAMENTO_FINANCEIRODTO>(sql).ToList();
        }


        // GET api/Vendas
        [Route("api/Vendas/{status}")]
        public IEnumerable<VENDADTO> GetVENDAS(string status)
        {
            string sql = @"SELECT V.ID_VENDA, V.ID_CLIENTE, V.DATA, V.VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME CLIENTE,
                                   CASE V.STATUS WHEN 'I' THEN 'INICIADO' WHEN 'C' THEN 'CONCLUIDO' WHEN 'E' THEN 'CANCELADO' WHEN 'O' THEN 'ORCAMENTO' END DESC_STATUS,
	                               P.DESCRICAO PROCEDIMENTO, RC.REGIAO, PR.NOME PROFISSIONAL, VI.PACOTE, 
                                   dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA) AS VALOR_PAGO, 
                                   VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                               V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,
	                               dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) VALOR_PAGO_CARTAO,
								   (CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END) AS TIPO_PGTO,
                                   (CASE WHEN CT.TIPO = 'D' THEN 'DÉBITO'
                                    WHEN CT.TIPO = 'C' THEN 'CRÉDITO'
                                    WHEN CT.TIPO = NULL THEN ''
                                    WHEN CT.TIPO = '' THEN '' END) AS TIPO_CARTAO
                             FROM VENDA V
							 INNER JOIN  VENDA_ITEM VI ON  VI.ID_VENDA = V.ID_VENDA
							 INNER JOIN  CLIENTE C  ON  C.ID_CLIENTE = V.ID_CLIENTE
							 INNER JOIN PROCEDIMENTO P ON VI.ID_PROCEDIMENTO = P.ID_PROCEDIMENTO
							 INNER JOIN REGIAO_CORPO RC ON RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
							 INNER JOIN PROFISSIONAL PR ON PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
							 LEFT JOIN VENDA_PGTOS VP ON  VP.ID_VENDA = V.ID_VENDA
                             LEFT JOIN CARTAO CT ON CT.ID_CARTAO = VP.ID_CARTAO
							 WHERE  V.STATUS = UPPER(@STATUS)
                            ORDER BY CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END";
            return db.Database.SqlQuery<VENDADTO>(sql, new SqlParameter("@STATUS", status)).ToList();
        }

        // GET api/Venda/1
        [Route("api/Venda/{idVenda}", Name = "GetVendaByID")]
        public IEnumerable<VENDADTO> GetVENDA(int idVenda)
        {
            string sql = @"SELECT V.ID_VENDA, V.ID_CLIENTE, V.DATA, V.VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME CLIENTE,
                                   CASE V.STATUS WHEN 'I' THEN 'INICIADO' WHEN 'C' THEN 'CONCLUIDO' WHEN 'E' THEN 'CANCELADO' WHEN 'O' THEN 'ORCAMENTO' END DESC_STATUS,
	                               P.DESCRICAO PROCEDIMENTO, RC.REGIAO, PR.NOME PROFISSIONAL, VI.PACOTE, 
                                   dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA) AS VALOR_PAGO, 
                                   VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                               V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,
	                               dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) VALOR_PAGO_CARTAO,
								   (CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END) AS TIPO_PGTO,
                                   (CASE WHEN CT.TIPO = 'D' THEN 'DÉBITO'
                                    WHEN CT.TIPO = 'C' THEN 'CRÉDITO'
                                    WHEN CT.TIPO = NULL THEN ''
                                    WHEN CT.TIPO = '' THEN '' END) AS TIPO_CARTAO
                             FROM VENDA V
							 INNER JOIN  VENDA_ITEM VI ON  VI.ID_VENDA = V.ID_VENDA
							 INNER JOIN  CLIENTE C  ON  C.ID_CLIENTE = V.ID_CLIENTE
							 INNER JOIN PROCEDIMENTO P ON VI.ID_PROCEDIMENTO = P.ID_PROCEDIMENTO
							 INNER JOIN REGIAO_CORPO RC ON RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
							 INNER JOIN PROFISSIONAL PR ON PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
							 LEFT JOIN VENDA_PGTOS VP ON  VP.ID_VENDA = V.ID_VENDA
                             LEFT JOIN CARTAO CT ON CT.ID_CARTAO = VP.ID_CARTAO
							 WHERE V.ID_VENDA = @ID_VENDA
                            ORDER BY CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END";
            return db.Database.SqlQuery<VENDADTO>(sql,
                                                  new SqlParameter("@ID_VENDA", idVenda)).ToList();
        }

        // GET api/Vendas/Login/PABLO
        [Route("api/Vendas/{status}/Login/{login}")]
        public IEnumerable<VENDADTO> GetVENDAS_LOGIN(string status, string login)
        {
            string sql = @"SELECT V.ID_VENDA, V.ID_CLIENTE, V.DATA, V.VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME CLIENTE,
                                   CASE V.STATUS WHEN 'I' THEN 'INICIADO' WHEN 'C' THEN 'CONCLUIDO' WHEN 'E' THEN 'CANCELADO' WHEN 'O' THEN 'ORCAMENTO' END DESC_STATUS,
	                               P.DESCRICAO PROCEDIMENTO, RC.REGIAO, PR.NOME PROFISSIONAL, VI.PACOTE, 
                                   dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA) AS VALOR_PAGO, 
                                   VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                               V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,
	                               dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) VALOR_PAGO_CARTAO,
								   (CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END) AS TIPO_PGTO,
                                    (CASE WHEN CT.TIPO = 'D' THEN 'DÉBITO'
                                    WHEN CT.TIPO = 'C' THEN 'CRÉDITO'
                                    WHEN CT.TIPO = NULL THEN ''
                                    WHEN CT.TIPO = '' THEN '' END) AS TIPO_CARTAO
                             FROM VENDA V
							 INNER JOIN  VENDA_ITEM VI ON  VI.ID_VENDA = V.ID_VENDA
							 INNER JOIN  CLIENTE C  ON  C.ID_CLIENTE = V.ID_CLIENTE
							 INNER JOIN PROCEDIMENTO P ON VI.ID_PROCEDIMENTO = P.ID_PROCEDIMENTO
							 INNER JOIN REGIAO_CORPO RC ON RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
							 INNER JOIN PROFISSIONAL PR ON PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
							 LEFT JOIN VENDA_PGTOS VP ON  VP.ID_VENDA = V.ID_VENDA
                             LEFT JOIN CARTAO CT ON CT.ID_CARTAO = VP.ID_CARTAO
							 WHERE  UPPER(V.LOGIN) = UPPER(@LOGIN)   
                               AND V.STATUS = UPPER(@STATUS)
                            ORDER BY CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END";
            return db.Database.SqlQuery<VENDADTO>(sql,
                                                  new SqlParameter("@LOGIN", login),
                                                  new SqlParameter("@STATUS", status)).ToList();
        }

        [Route("api/venda/{status}/{dtInicial}/{dtFinal}")]
        public IEnumerable<VENDADTO> GetVENDAS(string status, DateTime? dtInicial, DateTime? dtFinal)
        {
            string sql = @"SELECT (CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END) AS TIPO_PGTO, 
 
                                   (CASE WHEN CT.TIPO = 'D' THEN 'DÉBITO'
                                    WHEN CT.TIPO = 'C' THEN 'CRÉDITO'
                                    WHEN CT.TIPO = NULL THEN ''
                                    WHEN CT.TIPO = '' THEN '' END) AS TIPO_CARTAO,

                                   V.ID_VENDA, V.ID_CLIENTE, V.DATA, ISNULL(VP.VALOR,0) AS VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME CLIENTE,
                                   CASE V.STATUS WHEN 'I' THEN 'INICIADO' WHEN 'C' THEN 'CONCLUIDO' WHEN 'E' THEN 'CANCELADO' WHEN 'O' THEN 'ORCAMENTO' END DESC_STATUS,
	                               P.DESCRICAO PROCEDIMENTO, RC.REGIAO, PR.NOME PROFISSIONAL, VI.PACOTE, 
                                   dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA) AS VALOR_PAGO, 
                                   VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                               V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,
	                               dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) VALOR_PAGO_CARTAO
                             FROM VENDA V
							 INNER JOIN  VENDA_ITEM VI ON  VI.ID_VENDA = V.ID_VENDA
							 INNER JOIN  CLIENTE C  ON  C.ID_CLIENTE = V.ID_CLIENTE
							 INNER JOIN PROCEDIMENTO P ON VI.ID_PROCEDIMENTO = P.ID_PROCEDIMENTO
							 INNER JOIN REGIAO_CORPO RC ON RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
							 INNER JOIN PROFISSIONAL PR ON PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
							 LEFT JOIN VENDA_PGTOS VP ON  VP.ID_VENDA = V.ID_VENDA
                             LEFT JOIN CARTAO CT ON CT.ID_CARTAO = VP.ID_CARTAO
                             WHERE 1=1
                               AND V.STATUS <> 'O'                               
                               AND V.STATUS = UPPER(@STATUS)                                                           
                               AND CAST(V.DATA as DATE) BETWEEN CAST(@DATA_INI AS DATE) AND CAST(@DATA_FIM AS DATE)
                             GROUP BY VP.TIPO_PGTO,CT.TIPO, VP.ID_ITEM_PGTO_VENDA, VP.ID_ITEM_PGTO_VENDA, V.ID_VENDA, V.ID_CLIENTE, V.DATA, VP.VALOR, LOGIN, V.DATA_ATUALIZACAO, 
                               V.STATUS, dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA), C.NOME,  PACOTE, VI.ID_PROFISSIONAL, PR.NOME, VI.ID_PROCEDIMENTO,
                               P.DESCRICAO,RC.REGIAO, COMISSAO_GERADA, DATA_COMISSAO_GERADA, LOGIN_COMISSAO_GERADA, ID_USUARIO,VI.ID_VENDA, VI.ID_ITEM_VENDA
                            ORDER BY CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END";
            return db.Database.SqlQuery<VENDADTO>(sql,
                                                  new SqlParameter("@STATUS", status),
                                                  new SqlParameter("@DATA_INI", dtInicial.Value.Date),
                                                  new SqlParameter("@DATA_FIM", dtFinal.Value.Date)).ToList();
        }

        [Route("api/venda/login/{login}/{status}/{dtInicial}/{dtFinal}")]
        public IEnumerable<VENDADTO> GetVENDAS(string login, string status, DateTime? dtInicial, DateTime? dtFinal)
        {
            string sql = @"SELECT V.ID_VENDA, V.ID_CLIENTE, V.DATA, V.VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME CLIENTE,
                                   CASE V.STATUS WHEN 'I' THEN 'INICIADO' WHEN 'C' THEN 'CONCLUIDO' WHEN 'E' THEN 'CANCELADO' WHEN 'O' THEN 'ORCAMENTO' END DESC_STATUS,
	                               P.DESCRICAO PROCEDIMENTO, RC.REGIAO, PR.NOME PROFISSIONAL, VI.PACOTE, 
                                   dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA) AS VALOR_PAGO, 
                                   VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                               V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,
	                               dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) VALOR_PAGO_CARTAO,
								   (CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END) AS TIPO_PGTO,
                                    (CASE WHEN CT.TIPO = 'D' THEN 'DÉBITO'
                                    WHEN CT.TIPO = 'C' THEN 'CRÉDITO'
                                    WHEN CT.TIPO = NULL THEN ''
                                    WHEN CT.TIPO = '' THEN '' END) AS TIPO_CARTAO

                             FROM VENDA V
							 INNER JOIN  VENDA_ITEM VI ON  VI.ID_VENDA = V.ID_VENDA
							 INNER JOIN  CLIENTE C  ON  C.ID_CLIENTE = V.ID_CLIENTE
							 INNER JOIN PROCEDIMENTO P ON VI.ID_PROCEDIMENTO = P.ID_PROCEDIMENTO
							 INNER JOIN REGIAO_CORPO RC ON RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
							 INNER JOIN PROFISSIONAL PR ON PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
							 LEFT JOIN VENDA_PGTOS VP ON  VP.ID_VENDA = V.ID_VENDA
                             LEFT JOIN CARTAO CT ON CT.ID_CARTAO = VP.ID_CARTAO
                             WHERE 1=1
                               AND V.STATUS <> 'O'
                               AND UPPER(V.LOGIN) = UPPER(@LOGIN)   
                               AND V.STATUS = UPPER(@STATUS)
                               AND CAST(V.DATA AS DATE) BETWEEN CAST(@DATA_INI AS DATE) AND CAST(@DATA_FIM AS DATE)
                             GROUP BY V.ID_VENDA, V.ID_CLIENTE, V.DATA, V.VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME ,
                               V.STATUS,P.DESCRICAO, RC.REGIAO, PR.NOME, VI.PACOTE, 
                               dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA), VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                           V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.TIPO_PGTO, CT.TIPO
                            ORDER BY CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END";
            return db.Database.SqlQuery<VENDADTO>(sql,
                                                  new SqlParameter("@LOGIN", login),
                                                  new SqlParameter("@STATUS", status),
                                                  new SqlParameter("@DATA_INI", dtInicial.Value.Date),
                                                  new SqlParameter("@DATA_FIM", dtFinal.Value.Date)).ToList();
        }

        [Route("api/venda/{status}/{dtInicial}/{dtFinal}/profissional/{idProfissional}")]
        public IEnumerable<VENDADTO> GetVENDAS_PROFISSIONAL(string status, DateTime? dtInicial, DateTime? dtFinal, int idProfissional)
        {
            string sql = @"SELECT (CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END) AS TIPO_PGTO, 

                                    (CASE WHEN CT.TIPO = 'D' THEN 'DÉBITO'
                                    WHEN CT.TIPO = 'C' THEN 'CRÉDITO'
                                    WHEN CT.TIPO = NULL THEN ''
                                    WHEN CT.TIPO = '' THEN '' END) AS TIPO_CARTAO,

                              V.ID_VENDA, V.ID_CLIENTE, V.DATA, ISNULL(VP.VALOR,0) AS VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME CLIENTE,
                              CASE V.STATUS WHEN 'I' THEN 'INICIADO' WHEN 'C' THEN 'CONCLUIDO' WHEN 'E' THEN 'CANCELADO' WHEN 'O' THEN 'ORCAMENTO' END DESC_STATUS,
	                          P.DESCRICAO PROCEDIMENTO, RC.REGIAO, PR.NOME PROFISSIONAL, VI.PACOTE, 
                              dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA) AS VALOR_PAGO, 
                              VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                          V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,
	                          dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) VALOR_PAGO_CARTAO
                             FROM VENDA V
							 INNER JOIN  VENDA_ITEM VI ON  VI.ID_VENDA = V.ID_VENDA
							 INNER JOIN  CLIENTE C  ON  C.ID_CLIENTE = V.ID_CLIENTE
							 INNER JOIN PROCEDIMENTO P ON VI.ID_PROCEDIMENTO = P.ID_PROCEDIMENTO
							 INNER JOIN REGIAO_CORPO RC ON RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
							 INNER JOIN PROFISSIONAL PR ON PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
							 LEFT JOIN VENDA_PGTOS VP ON  VP.ID_VENDA = V.ID_VENDA
                             LEFT JOIN CARTAO CT ON CT.ID_CARTAO = VP.ID_CARTAO
                             WHERE 1=1
                               AND V.STATUS <> 'O'                                 
                               AND V.STATUS = UPPER(@STATUS)                               
                               AND VI.ID_PROFISSIONAL = @ID_PROFISSIONAL
                               AND CAST(V.DATA AS DATE) BETWEEN CAST(@DATA_INI AS DATE) AND CAST(@DATA_FIM AS DATE)
                             GROUP BY VP.TIPO_PGTO,CT.TIPO, VP.ID_ITEM_PGTO_VENDA, VP.ID_ITEM_PGTO_VENDA, V.ID_VENDA, V.ID_CLIENTE, V.DATA, VP.VALOR, LOGIN, V.DATA_ATUALIZACAO, 
                               V.STATUS, dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA), C.NOME,  PACOTE, VI.ID_PROFISSIONAL, PR.NOME, VI.ID_PROCEDIMENTO,
                               P.DESCRICAO,RC.REGIAO, COMISSAO_GERADA, DATA_COMISSAO_GERADA, LOGIN_COMISSAO_GERADA, ID_USUARIO,VI.ID_VENDA, VI.ID_ITEM_VENDA
                            ORDER BY CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END";
            return db.Database.SqlQuery<VENDADTO>(sql,
                                                  new SqlParameter("@STATUS", status),
                                                  new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                  new SqlParameter("@DATA_INI", dtInicial.Value.Date),
                                                  new SqlParameter("@DATA_FIM", dtFinal.Value.Date)).ToList();
        }

        [Route("api/venda/login/{login}/{status}/{dtInicial}/{dtFinal}/profissional/{idProfissional}")]
        public IEnumerable<VENDADTO> GetVENDAS_PROFISSIONAL(string login, string status, DateTime? dtInicial, DateTime? dtFinal, int idProfissional)
        {
            string sql = @"SELECT V.ID_VENDA, V.ID_CLIENTE, V.DATA, V.VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME CLIENTE,
                                   CASE V.STATUS WHEN 'I' THEN 'INICIADO' WHEN 'C' THEN 'CONCLUIDO' WHEN 'E' THEN 'CANCELADO' WHEN 'O' THEN 'ORCAMENTO' END DESC_STATUS,
	                               P.DESCRICAO PROCEDIMENTO, RC.REGIAO, PR.NOME PROFISSIONAL, VI.PACOTE, 
                                   dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA) AS VALOR_PAGO, 
                                   VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                               V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,
	                               dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) VALOR_PAGO_CARTAO,
								   (CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END) AS TIPO_PGTO,
                                   (CASE WHEN CT.TIPO = 'D' THEN 'DÉBITO'
                                    WHEN CT.TIPO = 'C' THEN 'CRÉDITO'
                                    WHEN CT.TIPO = NULL THEN ''
                                    WHEN CT.TIPO = '' THEN '' END) AS TIPO_CARTAO
                             FROM VENDA V
							 INNER JOIN  VENDA_ITEM VI ON  VI.ID_VENDA = V.ID_VENDA
							 INNER JOIN  CLIENTE C  ON  C.ID_CLIENTE = V.ID_CLIENTE
							 INNER JOIN PROCEDIMENTO P ON VI.ID_PROCEDIMENTO = P.ID_PROCEDIMENTO
							 INNER JOIN REGIAO_CORPO RC ON RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
							 INNER JOIN PROFISSIONAL PR ON PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
							 LEFT JOIN VENDA_PGTOS VP ON  VP.ID_VENDA = V.ID_VENDA
                             LEFT JOIN CARTAO CT ON CT.ID_CARTAO = VP.ID_CARTAO
                             WHERE 1=1
                               AND V.STATUS <> 'O'
                               AND UPPER(V.LOGIN) = UPPER(@LOGIN)   
                               AND V.STATUS = UPPER(@STATUS)                               
                               AND VI.ID_PROFISSIONAL = @ID_PROFISSIONAL
                               AND CAST(V.DATA AS DATE) BETWEEN CAST(@DATA_INI AS DATE) AND CAST(@DATA_FIM AS DATE)
                            GROUP BY V.ID_VENDA, V.ID_CLIENTE, V.DATA, V.VALOR, LOGIN, V.DATA_ATUALIZACAO, 
                               V.STATUS, dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA), C.NOME,  PACOTE, VI.ID_PROFISSIONAL, PR.NOME, VI.ID_PROCEDIMENTO,
                               P.DESCRICAO,RC.REGIAO, COMISSAO_GERADA, DATA_COMISSAO_GERADA, LOGIN_COMISSAO_GERADA, ID_USUARIO,VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.TIPO_PGTO, CT.TIPO
                            ORDER BY CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END";
            return db.Database.SqlQuery<VENDADTO>(sql,
                                                  new SqlParameter("@LOGIN", login),
                                                  new SqlParameter("@STATUS", status),
                                                  new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                  new SqlParameter("@DATA_INI", dtInicial.Value.Date),
                                                  new SqlParameter("@DATA_FIM", dtFinal.Value.Date)).ToList();
        }


        [Route("api/venda/{status}/{dtInicial}/{dtFinal}/procedimento/{idProcedimento}")]
        public IEnumerable<VENDADTO> GetVENDAS_PROCEDIMENTO(string status, DateTime? dtInicial, DateTime? dtFinal, int idProcedimento)
        {
            string sql = @"SELECT V.ID_VENDA, V.ID_CLIENTE, V.DATA, V.VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME CLIENTE,
                                   CASE V.STATUS WHEN 'I' THEN 'INICIADO' WHEN 'C' THEN 'CONCLUIDO' WHEN 'E' THEN 'CANCELADO' WHEN 'O' THEN 'ORCAMENTO' END DESC_STATUS,
	                               P.DESCRICAO PROCEDIMENTO, RC.REGIAO, PR.NOME PROFISSIONAL, VI.PACOTE, 
                                   dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA) AS VALOR_PAGO, 
                                   VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                               V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,
	                               dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) VALOR_PAGO_CARTAO,
								   (CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END) AS TIPO_PGTO,
                                   (CASE WHEN CT.TIPO = 'D' THEN 'DÉBITO'
                                    WHEN CT.TIPO = 'C' THEN 'CRÉDITO'
                                    WHEN CT.TIPO = NULL THEN ''
                                    WHEN CT.TIPO = '' THEN '' END) AS TIPO_CARTAO
                             FROM VENDA V
							 INNER JOIN  VENDA_ITEM VI ON  VI.ID_VENDA = V.ID_VENDA
							 INNER JOIN  CLIENTE C  ON  C.ID_CLIENTE = V.ID_CLIENTE
							 INNER JOIN PROCEDIMENTO P ON VI.ID_PROCEDIMENTO = P.ID_PROCEDIMENTO
							 INNER JOIN REGIAO_CORPO RC ON RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
							 INNER JOIN PROFISSIONAL PR ON PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
							 LEFT JOIN VENDA_PGTOS VP ON  VP.ID_VENDA = V.ID_VENDA
                             LEFT JOIN CARTAO CT ON CT.ID_CARTAO = VP.ID_CARTAO
                             WHERE 1=1
                               AND V.STATUS <> 'O'                                
                               AND V.STATUS = UPPER(@STATUS)
                               AND VI.ID_PROCEDIMENTO = @ID_PROCEDIMENTO
                               AND CAST(V.DATA AS DATE) BETWEEN CAST(@DATA_INI AS DATE) AND CAST(@DATA_FIM AS DATE)
                            GROUP BY V.ID_VENDA, V.ID_CLIENTE, V.DATA, V.VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME ,
                               V.STATUS,P.DESCRICAO, RC.REGIAO, PR.NOME, VI.PACOTE, dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA), VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                           V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.TIPO_PGTO, CT.TIPO
                            ORDER BY CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END";
            return db.Database.SqlQuery<VENDADTO>(sql,
                                                  new SqlParameter("@STATUS", status),
                                                  new SqlParameter("@ID_PROCEDIMENTO", idProcedimento),                                                  
                                                  new SqlParameter("@DATA_INI", dtInicial.Value.Date),
                                                  new SqlParameter("@DATA_FIM", dtFinal.Value.Date)).ToList();
        }

        [Route("api/venda/login/{login}/{status}/{dtInicial}/{dtFinal}/procedimento/{idProcedimento}")]
        public IEnumerable<VENDADTO> GetVENDAS_PROCEDIMENTO(string login, string status, DateTime? dtInicial, DateTime? dtFinal, int idProcedimento)
        {
            string sql = @"SELECT (CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END) AS TIPO_PGTO, 
                                    (CASE WHEN CT.TIPO = 'D' THEN 'DÉBITO'
                                    WHEN CT.TIPO = 'C' THEN 'CRÉDITO'
                                    WHEN CT.TIPO = NULL THEN ''
                                    WHEN CT.TIPO = '' THEN '' END) AS TIPO_CARTAO,
                                   V.ID_VENDA, V.ID_CLIENTE, V.DATA, ISNULL(VP.VALOR,0) AS VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME CLIENTE,
                                   CASE V.STATUS WHEN 'I' THEN 'INICIADO' WHEN 'C' THEN 'CONCLUIDO' WHEN 'E' THEN 'CANCELADO' WHEN 'O' THEN 'ORCAMENTO' END DESC_STATUS,
	                               P.DESCRICAO PROCEDIMENTO, RC.REGIAO, PR.NOME PROFISSIONAL, VI.PACOTE, 
                                   dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA) AS VALOR_PAGO, 
                                   VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                               V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,
	                               dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) VALOR_PAGO_CARTAO
                             FROM VENDA V
							 INNER JOIN  VENDA_ITEM VI ON  VI.ID_VENDA = V.ID_VENDA
							 INNER JOIN  CLIENTE C  ON  C.ID_CLIENTE = V.ID_CLIENTE
							 INNER JOIN PROCEDIMENTO P ON VI.ID_PROCEDIMENTO = P.ID_PROCEDIMENTO
							 INNER JOIN REGIAO_CORPO RC ON RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
							 INNER JOIN PROFISSIONAL PR ON PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
							 LEFT JOIN VENDA_PGTOS VP ON  VP.ID_VENDA = V.ID_VENDA
                             LEFT JOIN CARTAO CT ON CT.ID_CARTAO = VP.ID_CARTAO
                             WHERE 1=1
                               AND V.STATUS <> 'O'
                               AND UPPER(V.LOGIN) = UPPER(@LOGIN)   
                               AND V.STATUS = UPPER(@STATUS)
                               AND VI.ID_PROCEDIMENTO = @ID_PROCEDIMENTO                               
                               AND CAST(V.DATA AS DATE) BETWEEN CAST(@DATA_INI AS DATE) AND CAST(@DATA_FIM AS DATE)
                            GROUP BY VP.TIPO_PGTO,CT.TIPO, VP.ID_ITEM_PGTO_VENDA, V.ID_VENDA, V.ID_CLIENTE, V.DATA, VP.VALOR, LOGIN, V.DATA_ATUALIZACAO, 
                               V.STATUS, dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA), C.NOME,  PACOTE, VI.ID_PROFISSIONAL, PR.NOME, VI.ID_PROCEDIMENTO,
                               P.DESCRICAO,RC.REGIAO, COMISSAO_GERADA, DATA_COMISSAO_GERADA, LOGIN_COMISSAO_GERADA, ID_USUARIO,VI.ID_VENDA, VI.ID_ITEM_VENDA
                            ORDER BY CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END";
            return db.Database.SqlQuery<VENDADTO>(sql,
                                                  new SqlParameter("@LOGIN", login),
                                                  new SqlParameter("@STATUS", status),
                                                  new SqlParameter("@ID_PROCEDIMENTO", idProcedimento),                                                  
                                                  new SqlParameter("@DATA_INI", dtInicial.Value.Date),
                                                  new SqlParameter("@DATA_FIM", dtFinal.Value.Date)).ToList();
        }

        [Route("api/venda/{status}/{dtInicial}/{dtFinal}/profissional/{idProfissional}/procedimento/{idProcedimento}")]
        public IEnumerable<VENDADTO> GetVENDAS_PROCEDIMENTO(string status, DateTime? dtInicial, DateTime? dtFinal, int idProfissional, int idProcedimento)
        {
            string sql = @"SELECT (CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END) AS TIPO_PGTO, 
                                   (CASE WHEN CT.TIPO = 'D' THEN 'DÉBITO'
                                    WHEN CT.TIPO = 'C' THEN 'CRÉDITO'
                                    WHEN CT.TIPO = NULL THEN ''
                                    WHEN CT.TIPO = '' THEN '' END) AS TIPO_CARTAO,
                                    V.ID_VENDA, V.ID_CLIENTE, V.DATA, ISNULL(VP.VALOR,0) AS VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME CLIENTE,
                                   CASE V.STATUS WHEN 'I' THEN 'INICIADO' WHEN 'C' THEN 'CONCLUIDO' WHEN 'E' THEN 'CANCELADO' WHEN 'O' THEN 'ORCAMENTO' END DESC_STATUS,
	                               P.DESCRICAO PROCEDIMENTO, RC.REGIAO, PR.NOME PROFISSIONAL, VI.PACOTE,  
                                   dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA) AS VALOR_PAGO, 
                                   VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                               V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,
	                               dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) VALOR_PAGO_CARTAO
                             FROM VENDA V
							 INNER JOIN  VENDA_ITEM VI ON  VI.ID_VENDA = V.ID_VENDA
							 INNER JOIN  CLIENTE C  ON  C.ID_CLIENTE = V.ID_CLIENTE
							 INNER JOIN PROCEDIMENTO P ON VI.ID_PROCEDIMENTO = P.ID_PROCEDIMENTO
							 INNER JOIN REGIAO_CORPO RC ON RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
							 INNER JOIN PROFISSIONAL PR ON PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
							 LEFT JOIN VENDA_PGTOS VP ON  VP.ID_VENDA = V.ID_VENDA
                             LEFT JOIN CARTAO CT ON CT.ID_CARTAO = VP.ID_CARTAO
                             WHERE 1=1
                               AND V.STATUS <> 'O'
                               AND V.STATUS = UPPER(@STATUS)
                               AND VI.ID_PROCEDIMENTO = @ID_PROCEDIMENTO
                               AND VI.ID_PROFISSIONAL = @ID_PROFISSIONAL
                               AND CAST(V.DATA AS DATE) BETWEEN CAST(@DATA_INI AS DATE) AND CAST(@DATA_FIM AS DATE)
                            GROUP BY VP.TIPO_PGTO,CT.TIPO, VP.ID_ITEM_PGTO_VENDA, V.ID_VENDA, V.ID_CLIENTE, V.DATA, VP.VALOR, LOGIN, V.DATA_ATUALIZACAO, 
                               V.STATUS, dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA), C.NOME,  PACOTE, VI.ID_PROFISSIONAL, PR.NOME, VI.ID_PROCEDIMENTO,
                               P.DESCRICAO,RC.REGIAO, COMISSAO_GERADA, DATA_COMISSAO_GERADA, LOGIN_COMISSAO_GERADA, ID_USUARIO,VI.ID_VENDA, VI.ID_ITEM_VENDA
                            ORDER BY CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END";
            return db.Database.SqlQuery<VENDADTO>(sql,
                                                  new SqlParameter("@STATUS", status),
                                                  new SqlParameter("@ID_PROCEDIMENTO", idProcedimento),
                                                  new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                  new SqlParameter("@DATA_INI", dtInicial.Value.Date),
                                                  new SqlParameter("@DATA_FIM", dtFinal.Value.Date)).ToList();
        }

        [Route("api/venda/login/{login}/{status}/{dtInicial}/{dtFinal}/profissional/{idProfissional}/procedimento/{idProcedimento}")]
        public IEnumerable<VENDADTO> GetVENDAS_PROCEDIMENTO(string login, string status, DateTime? dtInicial, DateTime? dtFinal, int idProfissional, int idProcedimento)
        {
            string sql = @"SELECT V.ID_VENDA, V.ID_CLIENTE, V.DATA, V.VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME CLIENTE,
                                   CASE V.STATUS WHEN 'I' THEN 'INICIADO' WHEN 'C' THEN 'CONCLUIDO' WHEN 'E' THEN 'CANCELADO' WHEN 'O' THEN 'ORCAMENTO' END DESC_STATUS,
	                               P.DESCRICAO PROCEDIMENTO, RC.REGIAO, PR.NOME PROFISSIONAL, VI.PACOTE, 
                                   dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA) AS VALOR_PAGO, 
                                   VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                               V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,
	                               dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) VALOR_PAGO_CARTAO,
								   (CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END) AS TIPO_PGTO,
                                    (CASE WHEN CT.TIPO = 'D' THEN 'DÉBITO'
                                    WHEN CT.TIPO = 'C' THEN 'CRÉDITO'
                                    WHEN CT.TIPO = NULL THEN ''
                                    WHEN CT.TIPO = '' THEN '' END) AS TIPO_CARTAO
                              FROM VENDA V
							 INNER JOIN  VENDA_ITEM VI ON  VI.ID_VENDA = V.ID_VENDA
							 INNER JOIN  CLIENTE C  ON  C.ID_CLIENTE = V.ID_CLIENTE
							 INNER JOIN PROCEDIMENTO P ON VI.ID_PROCEDIMENTO = P.ID_PROCEDIMENTO
							 INNER JOIN REGIAO_CORPO RC ON RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
							 INNER JOIN PROFISSIONAL PR ON PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
							 LEFT JOIN VENDA_PGTOS VP ON  VP.ID_VENDA = V.ID_VENDA
                             LEFT JOIN CARTAO CT ON CT.ID_CARTAO = VP.ID_CARTAO
                             WHERE 1=1
                               AND V.STATUS <> 'O'
                               AND UPPER(V.LOGIN) = UPPER(@LOGIN)   
                               AND V.STATUS = UPPER(@STATUS)
                               AND VI.ID_PROCEDIMENTO = @ID_PROCEDIMENTO
                               AND VI.ID_PROFISSIONAL = @ID_PROFISSIONAL
                               AND CAST(V.DATA AS DATE) BETWEEN CAST(@DATA_INI AS DATE) AND CAST(@DATA_FIM AS DATE)
                            GROUP BY V.ID_VENDA, V.ID_CLIENTE, V.DATA, V.VALOR, V.LOGIN, V.DATA_ATUALIZACAO, V.STATUS, C.NOME ,
                               V.STATUS,P.DESCRICAO, RC.REGIAO, PR.NOME, VI.PACOTE, dbo.GET_VALOR_POR_ITEM_VENDA_FORMA_PGTO(VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.ID_ITEM_PGTO_VENDA),
                               VI.ID_PROFISSIONAL, VI.ID_PROCEDIMENTO,
	                           V.COMISSAO_GERADA, V.DATA_COMISSAO_GERADA, V.LOGIN_COMISSAO_GERADA, V.ID_USUARIO,VI.ID_VENDA, VI.ID_ITEM_VENDA, VP.TIPO_PGTO, CT.TIPO
                            ORDER BY CASE WHEN VP.TIPO_PGTO = 0 THEN 'DINHEIRO'
                                    WHEN VP.TIPO_PGTO = 1 THEN 'CHEQUE' 
                                    WHEN VP.TIPO_PGTO = 2 THEN 'CARTÃO' 
                                    WHEN VP.TIPO_PGTO = 3 THEN 'BOLETO' 
                                    WHEN VP.TIPO_PGTO = 4 THEN 'CONVÊNIO' END";
            return db.Database.SqlQuery<VENDADTO>(sql,
                                                  new SqlParameter("@LOGIN", login),
                                                  new SqlParameter("@STATUS", status),
                                                  new SqlParameter("@ID_PROCEDIMENTO", idProcedimento),
                                                  new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                  new SqlParameter("@DATA_INI", dtInicial.Value.Date),
                                                  new SqlParameter("@DATA_FIM", dtFinal.Value.Date)).ToList();
        }

        // GET api/Orcamento
        [Route("api/Orcamento")]
        public IEnumerable<VENDADTO> GetOrcamento()
        {
            return (from v in db.VENDAs
                    join c in db.CLIENTES on v.ID_CLIENTE equals c.ID_CLIENTE
                    //join vi in db.VENDA_ITEM on v.ID_VENDA equals vi.ID_VENDA into _vi
                    //from vi in _vi.DefaultIfEmpty()
                    //join p in db.PROCEDIMENTOes on vi.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO into _p
                    //from p in _p.DefaultIfEmpty()
                    //join rc in db.REGIAO_CORPO on vi.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    //from rc in _rc.DefaultIfEmpty()
                    //join pr in db.PROFISSIONALs on vi.ID_PROFISSIONAL equals pr.ID_PROFISSIONAL into _pr
                    //from pr in _pr.DefaultIfEmpty()
                    select new VENDADTO()
                    {
                        ID_VENDA = v.ID_VENDA,
                        ID_CLIENTE = v.ID_CLIENTE,
                        DATA = v.DATA,
                        VALOR = v.VALOR,
                        LOGIN = v.LOGIN,
                        DATA_ATUALIZACAO = v.DATA_ATUALIZACAO,
                        STATUS = v.STATUS,
                        CLIENTE = c.NOME,
                        DESC_STATUS = v.STATUS == "I" ? "INICIADO" :
                                      v.STATUS == "C" ? "CONCLUIDO" :
                                      v.STATUS == "E" ? "CANCELADO" :
                                      v.STATUS == "O" ? "ORÇAMENTO" : "",
                        //PROCEDIMENTO = p.DESCRICAO,
                        //REGIAO = rc.REGIAO,
                        //PROFISSIONAL = pr.NOME,
                        //PACOTE = vi.PACOTE,
                        //VALOR_PAGO = vi.VALOR_PAGO,
                        //ID_PROFISSIONAL = vi.ID_PROFISSIONAL,
                        //ID_PROCEDIMENTO = vi.ID_PROCEDIMENTO,
                        COMISSAO_GERADA = v.COMISSAO_GERADA,
                        DATA_COMISSAO_GERADA = v.DATA_COMISSAO_GERADA,
                        LOGIN_COMISSAO_GERADA = v.LOGIN_COMISSAO_GERADA,
                        ID_USUARIO = v.ID_USUARIO
                    }).Where(v => v.STATUS == "O").ToList();
        }

        // GET api/Orcamento/1
        [Route("api/Orcamento/{idVenda}")]
        public IEnumerable<VENDADTO> GetORCAMENTO_ITEM(int idVenda)
        {
            return (from v in db.VENDAs
                    join c in db.CLIENTES on v.ID_CLIENTE equals c.ID_CLIENTE
                    join vi in db.VENDA_ITEM on v.ID_VENDA equals vi.ID_VENDA into _vi
                    from vi in _vi.DefaultIfEmpty()
                    join p in db.PROCEDIMENTOes on vi.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO into _p
                    from p in _p.DefaultIfEmpty()
                    join rc in db.REGIAO_CORPO on vi.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    join pr in db.PROFISSIONALs on vi.ID_PROFISSIONAL equals pr.ID_PROFISSIONAL into _pr
                    from pr in _pr.DefaultIfEmpty()
                    select new VENDADTO()
                    {
                        ID_VENDA = v.ID_VENDA,
                        ID_CLIENTE = v.ID_CLIENTE,
                        DATA = v.DATA,
                        VALOR = v.VALOR,
                        LOGIN = v.LOGIN,
                        DATA_ATUALIZACAO = v.DATA_ATUALIZACAO,
                        STATUS = v.STATUS,
                        CLIENTE = c.NOME,
                        DESC_STATUS = v.STATUS == "I" ? "INICIADO" :
                                      v.STATUS == "C" ? "CONCLUIDO" :
                                      v.STATUS == "E" ? "CANCELADO" :
                                      v.STATUS == "O" ? "ORÇAMENTO" : "",
                        PROCEDIMENTO = p.DESCRICAO,
                        REGIAO = rc.REGIAO,
                        PROFISSIONAL = pr.NOME,
                        PACOTE = vi.PACOTE,
                        VALOR_PAGO = vi.VALOR_PAGO,
                        ID_PROFISSIONAL = vi.ID_PROFISSIONAL,
                        ID_PROCEDIMENTO = vi.ID_PROCEDIMENTO,
                        COMISSAO_GERADA = v.COMISSAO_GERADA,
                        DATA_COMISSAO_GERADA = v.DATA_COMISSAO_GERADA,
                        LOGIN_COMISSAO_GERADA = v.LOGIN_COMISSAO_GERADA,
                        ID_USUARIO = v.ID_USUARIO
                    }).Where(v => v.ID_VENDA == idVenda && v.STATUS == "O").ToList();
        }

        [Route("api/venda/totais/{status}/profissional/{idProfissional}/procedimento/{idProcedimento}/{dtInicial}/{dtFinal}")]
        public IEnumerable<TOTAIS_VENDA> GetTotaisVenda(string status, int idProfissional, int idProcedimento, DateTime dtInicial, DateTime dtFinal)
        {
            string whereProfissional = string.Empty;
            string whereProcedimento = string.Empty;
            if (idProfissional > 0 )
                whereProfissional = @" AND EXISTS (SELECT VI.ID_PROFISSIONAL 
                                                    FROM VENDA_ITEM VI 
                                                   WHERE VI.ID_VENDA = V.ID_VENDA
                                                     AND VI.ID_PROFISSIONAL = "+idProfissional+" )";
            if (idProcedimento > 0)
                whereProcedimento = @" AND EXISTS (SELECT VI.ID_PROCEDIMENTO 
                                                    FROM VENDA_ITEM VI 
                                                   WHERE VI.ID_VENDA = V.ID_VENDA
                                                     AND VI.ID_PROCEDIMENTO = " + idProcedimento + " )";
            string sql = @"SELECT TOTAL_ITEM, TOTAL_PAGO
                            FROM
                         (SELECT SUM(V.VALOR) TOTAL_PAGO
                             FROM VENDA V
                             WHERE CAST(V.DATA AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM
                             AND V.STATUS = @STATUS " + whereProfissional + whereProcedimento +
                           @")V,
                         (SELECT SUM(VI.VALOR_PAGO) TOTAL_ITEM 
                                FROM VENDA_ITEM VI, VENDA V
                             WHERE VI.ID_VENDA = V.ID_VENDA
                              AND CAST(V.DATA AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM
                             AND V.STATUS = @STATUS
                                AND (-1 = " + idProfissional + " OR VI.ID_PROFISSIONAL = " + idProfissional + ")" +
                              " AND (-1 = " + idProcedimento + " OR VI.ID_PROCEDIMENTO = " + idProcedimento + ") ) VI";

            return db.Database.SqlQuery<TOTAIS_VENDA>(sql,
                                                  new SqlParameter("@STATUS", status),
                                                  new SqlParameter("@PDATA_INI", dtInicial.Date),
                                                  new SqlParameter("@PDATA_FIM", dtFinal.Date)).ToList();
        }
        // PUT: api/Venda/5
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/venda/PutVENDA/{idVenda}")]
        public IHttpActionResult PutVENDA(int idVenda, VENDA vENDA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idVenda != vENDA.ID_VENDA)
            {
                return BadRequest();
            }

            db.Entry(vENDA).State = EntityState.Modified;

            try
            {
                //db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VENDAExists(idVenda))
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

        // POST: api/Venda
        [ResponseType(typeof(VENDA))]
        [Route("api/venda", Name = "PostVenda")]
        public IHttpActionResult PostVENDA(VENDA vENDA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_VENDA;").First();

            vENDA.ID_VENDA = NextValue;

            db.VENDAs.Add(vENDA);

            try
            {
                //db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (VENDAExists(vENDA.ID_VENDA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetVendaByID", new { idVenda = vENDA.ID_VENDA }, vENDA);
        }

        // DELETE: api/Vendas/5
        [ResponseType(typeof(VENDA))]
        public IHttpActionResult DeleteVENDA(int id)
        {
            VENDA vENDA = db.VENDAs.Find(id);
            if (vENDA == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM VENDA_PGTOS WHERE ID_VENDA = {0}";

            if (db.Database.ExecuteSqlCommand(sql, id) > 0)
            {
                sql = @"DELETE FROM VENDA_ITEM WHERE ID_VENDA = {0}";
                if (db.Database.ExecuteSqlCommand(sql, id) > 0)
                {
                    db.VENDAs.Remove(vENDA);
                    //db.BulkSaveChanges();
                    db.SaveChanges();
                    return Ok(vENDA);
                }
                else
                {
                    db.VENDAs.Remove(vENDA);
                    //db.BulkSaveChanges();
                    db.SaveChanges();
                    return Ok(vENDA);
                }
            }
            else
            {
                sql = @"DELETE FROM VENDA_ITEM WHERE ID_VENDA = {0}";
                if (db.Database.ExecuteSqlCommand(sql, id) > 0)
                {
                    db.VENDAs.Remove(vENDA);
                    //db.BulkSaveChanges();
                    db.SaveChanges();
                    return Ok(vENDA);
                }
                else
                {
                    db.VENDAs.Remove(vENDA);
                    //db.BulkSaveChanges();
                    db.SaveChanges();
                    return Ok(vENDA);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VENDAExists(int id)
        {
            return db.VENDAs.Count(e => e.ID_VENDA == id) > 0;
        }
    }
}