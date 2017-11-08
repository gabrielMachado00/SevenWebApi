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

namespace SisMedApi.Controllers
{       
    public class CustosDTO
    {
        public string Servico { get; set; }        
        public decimal Faturamento { get; set; }
        public decimal Imposto { get; set; }
        public decimal DespesaCartao { get; set; }
        public decimal Consumiveis { get; set; }
        public decimal Comissoes { get; set; }
        public decimal Contr_Marginal { get; set; }
        public decimal Margem_Contribuicao { get; set; }
        public decimal Custos_Despesas_Fixa { get; set; }
        public decimal Resultado_Operacional { get; set; }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustosController : ApiController
    {
        private SisMedContext db = new SisMedContext();        
        // GET api/Appointment/Profissional/1/Cliente/1/Appointment/123
        //[Route("api/custos/{dtInicial}/{dtFinal}")]
        //public IEnumerable<CustosDTO> GetCustos(DateTime dtInicial, DateTime dtFinal)
        //{

        //    List<CustosDTO> custos = new List<CustosDTO>();
        //    CustosDTO custo = new CustosDTO();
        //    custos = db.Database.SqlQuery<CustosDTO>(@"SELECT 'TOTAL' SERVICO, 
        //                                               (SELECT SUM(VI.VALOR_PAGO) FROM VENDA_ITEM VI, VENDA V WHERE V.ID_VENDA = VI.ID_VENDA

        //                                                 AND CAST(V.DATA AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM) FATURAMENTO, 	   
        //                                               (SELECT CAST(SUM((FAT_TOTAL.TOTAL * PERCENTUAL) / 100) AS decimal(15, 2))
        //                                                FROM UNIDADE_IMPOSTO,

        //                                                     (SELECT SUM(VI.VALOR_PAGO) TOTAL

        //                                                        FROM VENDA_ITEM VI, VENDA V WHERE V.ID_VENDA = VI.ID_VENDA

        //                                                       AND CAST(V.DATA AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM) FAT_TOTAL
		      //                                          ) IMPOSTO,
		      //                                          (SELECT CAST(SUM(((VP.VALOR + VALOR_ACRESCIMO - VP.VALOR_DESCONTO) * ISNULL(VP.DESCONTO_CARTAO, CF.DESCONTO)) / 100) AS DECIMAL(15, 2)) TAXA_CARTAO
        //                                                  FROM VENDA_PGTOS VP, VENDA V, CARTAO_FAIXA_DESCONTO CF

        //                                                 WHERE CF.ID_CARTAO = VP.ID_CARTAO

        //                                                   AND VP.NUM_PARCELAS BETWEEN CF.PARCELA_INICIAL AND CF.PARCELA_FINAL
        //                                                   AND VP.ID_VENDA = V.ID_VENDA

        //                                                   AND VP.TIPO_PGTO = 2

        //                                                   AND V.STATUS = 'C'

        //                                                   AND CAST(V.DATA AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM) DESPESACARTAO,
		      //                                          (select CAST(SUM(AP.QUANTIDADE * DBO.GET_CUSTO_REAL(AP.ID_PRODUTO, A.DATA_HORA_FIM)) AS DECIMAL(15, 2))

        //                                                   from ATENDIMENTO A, ATENDIMENTO_PRODUTO AP

        //                                                  WHERE A.ID_ATENDIMENTO = AP.ID_ATENDIMENTO
        //                                                    AND A.STATUS = 'C'
        //                                                    AND CAST(A.DATA_HORA_FIM AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM) CONSUMIVEIS,
		      //                                          (SELECT ISNULL(SUM(CP.VALOR_COMISSAO), 0)
        //                                                   FROM COMISSAO_PAGAMENTO CP
        //                                                  WHERE CAST(CP.DATA_PAGAMENTO AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM
        //                                                    AND CP.STATUS = 'P') COMISSOES,
		      //                                          CAST(0 AS DECIMAL) Contr_Marginal,
		      //                                          CAST(0 AS DECIMAL) Margem_Contribuicao,
		      //                                          CAST(0 AS DECIMAL) Custos_Despesas_Fixa,
		      //                                          CAST(0 AS DECIMAL) Resultado_Operacional
        //                                        UNION ALL
        //                                        SELECT SERVICO, CAST(SUM(FATURAMENTO) AS DECIMAL(15, 2)) FATURAMENTO, 
        //                                                CAST(0 AS DECIMAL) IMPOSTO,
		      //                                          CAST(0 AS DECIMAL) DESPESACARTAO,
		      //                                          CAST(SUM(CONSUMIVEIS) AS DECIMAL(15, 2)) CONSUMIVEIS,
		      //                                          CAST(SUM(COMISSOES) AS DECIMAL(15, 2)) COMISSOES,
		      //                                          CAST(0 AS DECIMAL) Contr_Marginal,
		      //                                          CAST(0 AS DECIMAL) Margem_Contribuicao,
		      //                                          CAST(0 AS DECIMAL) Custos_Despesas_Fixa,
		      //                                          CAST(0 AS DECIMAL) Resultado_Operacional
        //                                        FROM(
        //                                        SELECT P.DESCRICAO SERVICO, VI.VALOR_PAGO FATURAMENTO,
        //                                                0 IMPOSTO,
        //                                                0 DESPESACARTAO,
        //                                                0 CONSUMIVEIS,
        //                                                0 COMISSOES,
        //                                                0 Contr_Marginal,
        //                                                0 Margem_Contribuicao,
        //                                                0 Custos_Despesas_Fixa,
        //                                                0 Resultado_Operacional
        //                                          FROM VENDA_ITEM VI, VENDA V, PROCEDIMENTO P
        //                                         WHERE P.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO
        //                                           AND VI.ID_VENDA = V.ID_VENDA
        //                                           AND V.STATUS = 'C'
        //                                           AND CAST(V.DATA AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM
        //                                         UNION ALL
        //                                         select P.DESCRICAO,
        //                                                0 FATURAMENTO,
        //                                                0 IMPOSTO,
        //                                                0 DESPESACARTAO,
        //                                                AP.QUANTIDADE * DBO.GET_CUSTO_REAL(AP.ID_PRODUTO, A.DATA_HORA_FIM) CONSUMIVEIS,
        //                                                0 COMISSOES,
        //                                                0 Contr_Marginal,
        //                                                0 Margem_Contribuicao,
        //                                                0 Custos_Despesas_Fixa,
        //                                                0 Resultado_Operacional
        //                                         from ATENDIMENTO A, Appointments AGE, ATENDIMENTO_PRODUTO AP, PROCEDIMENTO P
        //                                        WHERE P.ID_PROCEDIMENTO = AGE.ID_PROCEDIMENTO
        //                                          AND AGE.UniqueID = A.ID_APPOINTMENT
        //                                          AND A.ID_ATENDIMENTO = AP.ID_ATENDIMENTO
        //                                          AND A.STATUS = 'C'
        //                                          AND CAST(A.DATA_HORA_FIM AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM
        //                                        UNION ALL
        //                                        SELECT P.DESCRICAO,
        //                                               0 FATURAMENTO,
        //                                               0 IMPOSTO,
        //                                               0 DESPESACARTAO,
        //                                               0 CONSUMIVEIS,
        //                                               CP.VALOR_COMISSAO AS COMISSOES,
        //                                               0 Contr_Marginal,
        //                                               0 Margem_Contribuicao,
        //                                               0 Custos_Despesas_Fixa,
        //                                               0 Resultado_Operacional
        //                                          FROM COMISSAO_PAGAMENTO CP, PROCEDIMENTO P
        //                                         WHERE P.ID_PROCEDIMENTO = CP.ID_PROCEDIMENTO
        //                                           AND CP.STATUS = 'P'
        //                                           AND CAST(CP.DATA_PAGAMENTO AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM
        //                                          ) A
        //                                          GROUP BY SERVICO").ToList();                        
        //    return custos;              
        //}

        [Route("api/custos/{dtInicial}/{dtFinal}")]
        public IEnumerable<DataTable> GetCustos(DateTime dtInicial, DateTime dtFinal)
        {
            List<CustosDTO> custos = new List<CustosDTO>();            
            custos = db.Database.SqlQuery<CustosDTO>(@"SELECT 'TOTAL' SERVICO, 
                                                       (SELECT SUM(VI.VALOR_PAGO) FROM VENDA_ITEM VI, VENDA V WHERE V.ID_VENDA = VI.ID_VENDA
                                                         AND CAST(V.DATA AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM
                                                         AND V.STATUS = 'C') FATURAMENTO, 	   
                                                       ISNULL((SELECT CAST(SUM((FAT_TOTAL.TOTAL * PERCENTUAL) / 100) AS decimal(15, 2))
                                                        FROM UNIDADE_IMPOSTO,
                                                             (SELECT SUM(VI.VALOR_PAGO) TOTAL
                                                                FROM VENDA_ITEM VI, VENDA V WHERE V.ID_VENDA = VI.ID_VENDA
                                                               AND CAST(V.DATA AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM) FAT_TOTAL
		                                                ),0) IMPOSTO,
		                                                (SELECT CAST(SUM(((VP.VALOR + VALOR_ACRESCIMO - VP.VALOR_DESCONTO) * ISNULL(VP.DESCONTO_CARTAO, CF.DESCONTO)) / 100) AS DECIMAL(15, 2)) TAXA_CARTAO
                                                          FROM VENDA_PGTOS VP, VENDA V, CARTAO_FAIXA_DESCONTO CF
                                                         WHERE CF.ID_CARTAO = VP.ID_CARTAO
                                                           AND VP.NUM_PARCELAS BETWEEN CF.PARCELA_INICIAL AND CF.PARCELA_FINAL
                                                           AND VP.ID_VENDA = V.ID_VENDA
                                                           AND VP.TIPO_PGTO = 2
                                                           AND V.STATUS = 'C'
                                                           AND CAST(V.DATA AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM) DESPESACARTAO,
		                                                (select CAST(ISNULL(SUM(AP.QUANTIDADE * DBO.GET_CUSTO_REAL(AP.ID_PRODUTO, A.DATA_HORA_FIM)), 0) AS DECIMAL(15, 2))
                                                           from ATENDIMENTO A, ATENDIMENTO_PRODUTO AP
                                                          WHERE A.ID_ATENDIMENTO = AP.ID_ATENDIMENTO
                                                            AND A.STATUS = 'C'
                                                            AND CAST(A.DATA_HORA_FIM AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM) CONSUMIVEIS,
		                                                (SELECT ISNULL(SUM(CP.VALOR_COMISSAO), 0)
                                                           FROM COMISSAO_PAGAMENTO CP
                                                          WHERE CAST(CP.DATA_PAGAMENTO AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM
                                                            AND CP.STATUS = 'P') COMISSOES,
		                                                CAST(0 AS DECIMAL) Contr_Marginal,
		                                                CAST(0 AS DECIMAL) Margem_Contribuicao,
		                                                CAST((SELECT ISNULL(SUM(DF.VALOR), 0) 
                                                              FROM DOCUMENTO_FINANCEIRO DF, CATEGORIA C
                                                             WHERE C.ID_CATEGORIA = DF.ID_CATEGORIA
                                                               AND C.SUB_TIPO = 'D'
                                                               AND CAST(DF.DATA_VENCIMENTO AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM) AS DECIMAL(15,2)) Custos_Despesas_Fixa,
		                                                CAST(0 AS DECIMAL) Resultado_Operacional
                                                UNION ALL
                                                SELECT SERVICO, CAST(SUM(FATURAMENTO) AS DECIMAL(15, 2)) FATURAMENTO, 
                                                        CAST(0 AS DECIMAL) IMPOSTO,
		                                                CAST(SUM(DESPESACARTAO) AS DECIMAL(15, 2)) DESPESACARTAO,
		                                                CAST(SUM(CONSUMIVEIS) AS DECIMAL(15, 2)) CONSUMIVEIS,
		                                                CAST(SUM(COMISSOES) AS DECIMAL(15, 2)) COMISSOES,
		                                                CAST(0 AS DECIMAL) Contr_Marginal,
		                                                CAST(0 AS DECIMAL) Margem_Contribuicao,
		                                                CAST(0 AS DECIMAL) Custos_Despesas_Fixa,
		                                                CAST(0 AS DECIMAL) Resultado_Operacional
                                                FROM(
                                                SELECT P.DESCRICAO SERVICO, VI.VALOR_PAGO FATURAMENTO,
                                                        0 IMPOSTO,
                                                        0 DESPESACARTAO,
                                                        0 CONSUMIVEIS,
                                                        0 COMISSOES,
                                                        0 Contr_Marginal,
                                                        0 Margem_Contribuicao,
                                                        0 Custos_Despesas_Fixa,
                                                        0 Resultado_Operacional
                                                  FROM VENDA_ITEM VI, VENDA V, PROCEDIMENTO P
                                                 WHERE P.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO
                                                   AND VI.ID_VENDA = V.ID_VENDA
                                                   AND V.STATUS = 'C'
                                                   AND CAST(V.DATA AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM
                                                 UNION ALL	
												 SELECT P.DESCRICAO SERVICO, 0 FATURAMENTO,
                                                        0 IMPOSTO,
                                                        dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) DESPESACARTAO,
                                                        0 CONSUMIVEIS,
                                                        0 COMISSOES,
                                                        0 Contr_Marginal,
                                                        0 Margem_Contribuicao,
                                                        0 Custos_Despesas_Fixa,
                                                        0 Resultado_Operacional
                                                  FROM VENDA_ITEM VI, VENDA V, PROCEDIMENTO P
                                                 WHERE P.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO												   
                                                   AND VI.ID_VENDA = V.ID_VENDA
                                                   AND V.STATUS = 'C'
                                                   AND CAST(V.DATA AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM
                                                 UNION ALL												 
                                                 select P.DESCRICAO,
                                                        0 FATURAMENTO,
                                                        0 IMPOSTO,
                                                        0 DESPESACARTAO,
                                                        AP.QUANTIDADE * DBO.GET_CUSTO_REAL(AP.ID_PRODUTO, A.DATA_HORA_FIM) CONSUMIVEIS,
                                                        0 COMISSOES,
                                                        0 Contr_Marginal,
                                                        0 Margem_Contribuicao,
                                                        0 Custos_Despesas_Fixa,
                                                        0 Resultado_Operacional
                                                 from ATENDIMENTO A, Appointments AGE, ATENDIMENTO_PRODUTO AP, PROCEDIMENTO P
                                                WHERE P.ID_PROCEDIMENTO = AGE.ID_PROCEDIMENTO
                                                  AND AGE.UniqueID = A.ID_APPOINTMENT
                                                  AND A.ID_ATENDIMENTO = AP.ID_ATENDIMENTO
                                                  AND A.STATUS = 'C'
                                                  AND CAST(A.DATA_HORA_FIM AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM
                                                UNION ALL
                                                SELECT P.DESCRICAO,
                                                       0 FATURAMENTO,
                                                       0 IMPOSTO,
                                                       0 DESPESACARTAO,
                                                       0 CONSUMIVEIS,
                                                       CP.VALOR_COMISSAO AS COMISSOES,
                                                       0 Contr_Marginal,
                                                       0 Margem_Contribuicao,
                                                       0 Custos_Despesas_Fixa,
                                                       0 Resultado_Operacional
                                                  FROM COMISSAO_PAGAMENTO CP, PROCEDIMENTO P
                                                 WHERE P.ID_PROCEDIMENTO = CP.ID_PROCEDIMENTO
                                                   AND CP.STATUS = 'P'
                                                   AND CAST(CP.DATA_PAGAMENTO AS DATE) BETWEEN @PDATA_INI AND @PDATA_FIM
                                                  ) A
                                                  GROUP BY SERVICO",
                                                  new SqlParameter("@PDATA_INI", dtInicial.Date),
                                                  new SqlParameter("@PDATA_FIM", dtFinal.Date)).ToList();
            return GetDadosCustos(custos);
        }
        DataTable dtCustos;
        public List<DataTable> GetDadosCustos(List<CustosDTO> dados)
        {
            List<DataTable> resultado = new List<DataTable>();
            DataColumn col;
            dtCustos = new DataTable("tb_custos");
            preenhcheColunasNome();
            decimal total = 0;
            decimal total_contrib_servico = 0;
            decimal total_contrib = 0;
            decimal total_custos_desp = 0;
            decimal custos_desp = 0;

            foreach (CustosDTO dr in dados)
            {
                if (dr.Faturamento > 0)
                {
                    total = 0;
                    col = new DataColumn();
                    col.ColumnName = dr.Servico.ToString();
                    dtCustos.Columns.Add(col);
                    dtCustos.Rows[0][dr.Servico.ToString()] = dr.Faturamento.ToString("N2"); //string.Format("{0:C}", dr.Faturamento.ToString());
                    dtCustos.Rows[1][dr.Servico.ToString()] = dr.Imposto.ToString("N2"); 
                    dtCustos.Rows[2][dr.Servico.ToString()] = dr.DespesaCartao.ToString("N2"); 
                    dtCustos.Rows[3][dr.Servico.ToString()] = dr.Consumiveis.ToString("N2");
                    dtCustos.Rows[4][dr.Servico.ToString()] = dr.Comissoes.ToString("N2"); 
                    total = dr.Faturamento - dr.Imposto - dr.DespesaCartao - dr.Consumiveis - dr.Comissoes;
                    total_contrib_servico = total;
                    if (dr.Servico.ToString() == "TOTAL")
                        total_contrib = total;
                    dtCustos.Rows[5][dr.Servico.ToString()] = total.ToString("N2");
                    total = total / total_contrib;
                    if (dr.Servico.ToString() == "TOTAL")
                        total = 100;
                    dtCustos.Rows[6][dr.Servico.ToString()] = total.ToString("N2");

                    custos_desp = total * total_custos_desp;
                    if (dr.Servico.ToString() == "TOTAL")
                    {
                        total_custos_desp = dr.Custos_Despesas_Fixa;
                        custos_desp = total_custos_desp;
                    }
                    dtCustos.Rows[7][dr.Servico.ToString()] = custos_desp.ToString("N2");

                    total = total_contrib_servico - custos_desp;
                    dtCustos.Rows[8][dr.Servico.ToString()] = total.ToString("N2");
                }
            }
            resultado.Add(dtCustos);
            return resultado;
        }

        private void preenhcheColunasNome()
        {
            DataColumn col;
            DataRow row;

            col = new DataColumn();
            col.ColumnName = "Totalizadores";
            dtCustos.Columns.Add(col);

            row = dtCustos.NewRow();
            row["Totalizadores"] = "Faturamento";
            dtCustos.Rows.Add(row);

            row = dtCustos.NewRow();
            row["Totalizadores"] = "Impostos";
            dtCustos.Rows.Add(row);

            row = dtCustos.NewRow();
            row["Totalizadores"] = "Taxa Cartão";
            dtCustos.Rows.Add(row);

            row = dtCustos.NewRow();
            row["Totalizadores"] = "Consumíveis";
            dtCustos.Rows.Add(row);

            row = dtCustos.NewRow();
            row["Totalizadores"] = "Comissões";
            dtCustos.Rows.Add(row);

            row = dtCustos.NewRow();
            row["Totalizadores"] = "Contr. Marginal Total";
            dtCustos.Rows.Add(row);

            row = dtCustos.NewRow();
            row["Totalizadores"] = "% Margem Contr.";
            dtCustos.Rows.Add(row);

            row = dtCustos.NewRow();
            row["Totalizadores"] = "Custos e Desp. Fixas";
            dtCustos.Rows.Add(row);

            row = dtCustos.NewRow();
            row["Totalizadores"] = "Resultado Operacional";
            dtCustos.Rows.Add(row);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
                
    }
}