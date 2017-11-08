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
    public class AgendaDia
    {
        public int DIA { get; set; }
        public int AGENDADOS { get; set; }
        public int CONFIRMADOS { get; set; }
        public int ATENDIDOS { get; set; }
        public int CANCELADOS { get; set; }
        public int FALTOU { get; set; }
        public int OUTROS { get; set; }
    }
    public class Grafico
    {
        public int Tipo { get; set; }
        public Object Dados { get; set; }
        public Grafico(int tipo, Object dados)
        {
            Tipo = tipo;
            Dados = dados;
        }
    }

    public class Datadecimal
    {
        public string ColumnName { get; set; }
        public decimal Value { get; set; }        
    }

    public class Dataint
    {
        public string ColumnName { get; set; }
        public int Value { get; set; }
    }

    public class GestaoVista
    {
        public Nullable<decimal> ARECEBER { get; set; }
        public Nullable<decimal> RECEBIDO { get; set; }
        public Nullable<decimal> APAGAR { get; set; }
        public Nullable<decimal> PAGOS { get; set; }

        public Nullable<int> NOVOS_PACIENTES { get; set; }
        public Nullable<decimal> TICKET_MEDIO { get; set; }
        public Nullable<decimal> BASKET { get; set; }
        public Nullable<decimal> TAXA_OCUPACAO { get; set; }
        public Nullable<int> REATIVADOS { get; set; }
        public Nullable<int> PARETTO { get; set; }
        public Nullable<int> PACIENTES_ATENDIDOS { get; set; }
        public Nullable<int> PACIENTES_FATURADOS { get; set; }
        public Nullable<int> SERVICOS_ATENDIDOS { get; set; }
        public Nullable<int> SERVICOS_FATURADOS { get; set; }
        public Nullable<decimal> COMISSAO { get; set; }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GestaoAVistaController : ApiController
    {
        private SisMedContext db = new SisMedContext();
        List<Grafico> graficos = new List<Grafico>();

        // GET api/Appointment/Profissional/1/Cliente/1/Appointment/123
        [Route("api/gestaoavista/graficos/{dtInicial}/{dtFinal}")]
        public IEnumerable<Grafico> GetGraficos(DateTime dtInicial, DateTime dtFinal)
        {
            //ResumoAgenda itemResumo = new ResumoAgenda();
            //List<ResumoAgenda> resumo = new List<ResumoAgenda>();
                        
            List<AgendaDia> dataListAgenda = new List<AgendaDia>();
            GetGraficoPerfilPorSexo(-1, dtInicial, dtFinal);
            GetGraficoPerfilPorIdade(-1, dtInicial, dtFinal);
            GetGraficoTaxaOcupacaoProfissional(-1, dtInicial, dtFinal);
            GetGraficoAgendaDia(-1, dtInicial, dtFinal);
            GetGraficoFaturamentoServico(-1, dtInicial, dtFinal);
            GetGraficoUltimaMarcacao(-1, dtInicial, dtFinal);
            //resumo.Add(itemResumo);
            return graficos;              
        }

        public void GetGraficoPerfilPorSexo(int idProfissional, DateTime dtInicial, DateTime dtFinal)
        {
            List<Dataint> dataList = new List<Dataint>();                        
            //Gráfico Perfil Sexo
            //dataList.Add(new Data("FEMININO", 5000));
            //dataList.Add(new Data("MASCULINO", 3500));
            //graficos.Add(new Grafico(1, dataList));            
            dataList = db.Database.SqlQuery<Dataint>(@"SELECT 'FEMININO' ColumnName, COUNT(*) Value 
                                                          FROM CLIENTE C 
                                                         WHERE C.SEXO = 'F'
                                                           AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))
                                                         union all
                                                        SELECT 'MASCULINO' ColumnName,  COUNT(*) Value 
                                                          FROM CLIENTE C 
                                                         WHERE C.SEXO = 'M'
                                                           AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))",
                                                      new SqlParameter("@ID_PROFISSIONAL", idProfissional)).ToList();

            graficos.Add(new Grafico(1, dataList));            
        }

        public void GetGraficoPerfilPorIdade(int idProfissional, DateTime dtInicial, DateTime dtFinal)
        {
            List<Dataint> dataList = new List<Dataint>();
            //Gráfico Perfil Sexo
            //dataList.Add(new Data("FEMININO", 5000));
            //dataList.Add(new Data("MASCULINO", 3500));
            //graficos.Add(new Grafico(1, dataList));            
            dataList = db.Database.SqlQuery<Dataint>(@"SELECT 'Até 10 anos' ColumnName, COUNT(*) Value FROM CLIENTE C 
                                                     WHERE YEAR(GETDATE()) - YEAR(C.DATA_NASCIMENTO) <= 10
                                                      AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))
                                                    UNION ALL
                                                    SELECT 'Até 15 anos' ColumnName, COUNT(*) Value FROM CLIENTE C 
                                                     WHERE YEAR(GETDATE()) - YEAR(C.DATA_NASCIMENTO) BETWEEN 11 AND 15
                                                      AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))
                                                    UNION ALL
                                                    SELECT 'Até 20 anos' ColumnName, COUNT(*) Value FROM CLIENTE C 
                                                     WHERE YEAR(GETDATE()) - YEAR(C.DATA_NASCIMENTO) BETWEEN 16 AND 20
                                                      AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))
                                                     UNION ALL
                                                    SELECT 'Até 25 anos' ColumnName, COUNT(*) Value FROM CLIENTE C 
                                                     WHERE YEAR(GETDATE()) - YEAR(C.DATA_NASCIMENTO) BETWEEN 21 AND 25
                                                      AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))
                                                     UNION ALL
                                                    SELECT 'Até 30 anos' ColumnName, COUNT(*) Value FROM CLIENTE C 
                                                     WHERE YEAR(GETDATE()) - YEAR(C.DATA_NASCIMENTO) BETWEEN 26 AND 30
                                                      AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))
                                                    UNION ALL
                                                    SELECT 'Até 35 anos' ColumnName, COUNT(*) Value FROM CLIENTE C 
                                                     WHERE YEAR(GETDATE()) - YEAR(C.DATA_NASCIMENTO) BETWEEN 31 AND 35
                                                      AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))
                                                    UNION ALL
                                                    SELECT 'Até 40 anos' ColumnName, COUNT(*) Value FROM CLIENTE C 
                                                     WHERE YEAR(GETDATE()) - YEAR(C.DATA_NASCIMENTO) BETWEEN 36 AND 40
                                                      AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))
                                                    UNION ALL
                                                    SELECT 'Até 45 anos' ColumnName, COUNT(*) Value FROM CLIENTE C 
                                                     WHERE YEAR(GETDATE()) - YEAR(C.DATA_NASCIMENTO) BETWEEN 41 AND 45
                                                      AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))
                                                    UNION ALL
                                                    SELECT 'Até 50 anos' ColumnName, COUNT(*) Value FROM CLIENTE C 
                                                     WHERE YEAR(GETDATE()) - YEAR(C.DATA_NASCIMENTO) BETWEEN 46 AND 50
                                                      AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))
                                                    UNION ALL
                                                    SELECT 'Até 55 anos' ColumnName, COUNT(*) Value FROM CLIENTE C 
                                                     WHERE YEAR(GETDATE()) - YEAR(C.DATA_NASCIMENTO) BETWEEN 51 AND 55
                                                      AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))
                                                    UNION ALL
                                                    SELECT 'Até 60 anos' ColumnName, COUNT(*) Value FROM CLIENTE C 
                                                     WHERE YEAR(GETDATE()) - YEAR(C.DATA_NASCIMENTO) BETWEEN 56 AND 60
                                                      AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))
                                                    UNION ALL
                                                    SELECT 'Acima de 60 anos' ColumnName, COUNT(*) Value FROM CLIENTE C 
                                                     WHERE YEAR(GETDATE()) - YEAR(C.DATA_NASCIMENTO) > 61
                                                      AND (@ID_PROFISSIONAL = -1 OR EXISTS (SELECT * 
                                                                         FROM ATENDIMENTO A 
				                                                        WHERE A.ID_CLIENTE = C.ID_CLIENTE
				                                                          AND A.ID_PROFISSIONAL = @ID_PROFISSIONAL))",
                                                   new SqlParameter("@ID_PROFISSIONAL", idProfissional)).ToList();

            graficos.Add(new Grafico(2, dataList));
        }

        public void GetGraficoTaxaOcupacaoProfissional(int idProfissional, DateTime dtInicial, DateTime dtFinal)
        {
            List<Datadecimal> dataList = new List<Datadecimal>();                     
            dataList = db.Database.SqlQuery<Datadecimal>(@"select pp.NOME as ColumnName, cast(isnull(avg(pp1.perc),0)as decimal(15,2)) as VALUE                                                                    
                                                            from profissional pp
                                                            left join (select p.id_profissional, 
                                                                        cast( ((select Sum(DATEDIFF(MINUTE, StartDate, EndDate)) / 
				                                                                        nullif( (datediff(week, dateadd(month, datediff(month, 0, getdate()),
							                                                            0), getdate()) + 1),0) 
                                                                                from dbo.Appointments a 
			   	                                                                where cast(a.StartDate as date) >= cast(@DATA_INI as date) 
						                                                            and cast(a.StartDate as date) <= cast(GetDate() as date)
                                                                                    and datepart(DW, a.StartDate) = p.COD_DIA_SEMANA 
						                                                            and a.ResourceID = p.ID_PROFISSIONAL
																					AND (@ID_PROFISSIONAL = -1 OR a.ResourceID = @ID_PROFISSIONAL)
                                                                                group by  datepart(DW, StartDate)) * 100) / 
                                                                                ((select  SUM(DATEDIFF(MINUTE, HORA_INICIAL_1, HORA_FINAL_1) + 
                                                                                                DATEDIFF(MINUTE, HORA_INICIAL_2, HORA_FINAL_2) + 
                                                                                                DATEDIFF(MINUTE, HORA_INICIAL_3, HORA_FINAL_3))
                                                                                    from dbo.PROFISSIONAL_HORARIO P1 
                                                                                    WHERE P1.COD_DIA_SEMANA = P.COD_DIA_SEMANA
						                                                            and P1.ID_PROFISSIONAL = p.ID_PROFISSIONAL
																					and (@ID_PROFISSIONAL = -1 OR P1.ID_PROFISSIONAL = @ID_PROFISSIONAL) ) ) as decimal(15,2)) AS PERC 
                                                                    from dbo.PROFISSIONAL_HORARIO P 
																	where (@ID_PROFISSIONAL = -1 OR P.ID_PROFISSIONAL = @ID_PROFISSIONAL)
                                                                    group by P.ID_PROFISSIONAL, P.COD_DIA_SEMANA
                                                            ) as pp1 on pp1.ID_PROFISSIONAL = pp.ID_PROFISSIONAL
															where (@ID_PROFISSIONAL = -1 OR pp.ID_PROFISSIONAL = @ID_PROFISSIONAL)
                                                            group by pp.ID_PROFISSIONAL, pp.NOME 
                                                            having isnull(avg(pp1.perc),0) > 0
                                                            order by isnull(avg(pp1.perc),0) desc",
                                                            new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                            new SqlParameter("@DATA_INI", dtInicial.Date),
                                                            new SqlParameter("@DATA_FIM", dtFinal.Date)).ToList();
            graficos.Add(new Grafico(3, dataList));
        }
        
        public void GetGraficoAgendaDia(int idProfissional, DateTime dtInicial, DateTime dtFinal)
        {
            List<AgendaDia> dataList = new List<AgendaDia>();

            dataList = db.Database.SqlQuery<AgendaDia>(@"select day(A1.StartDate) AS DIA, (select count(*) from dbo.Appointments a 
                                                        where a.ResourceID is not null and a.Status = 0 
														  AND day(a.StartDate) = day(A1.StartDate) 
														  AND (@ID_PROFISSIONAL = -1 OR a.ResourceID = @ID_PROFISSIONAL)
														  AND cast(A.startdate as date) >= @DATA_INI 
														  AND cast(A.startdate as date) <= @DATA_FIM) AS AGENDADOS, 
                                                    (select count(*) from dbo.Appointments a 
                                                    where a.ResourceID is not null 
													 and a.Status = 1 and day(a.StartDate) = day(A1.StartDate)
													 AND (@ID_PROFISSIONAL = -1 OR a.ResourceID = @ID_PROFISSIONAL) 
													 AND cast(A.startdate as date) >= @DATA_INI 
													 AND cast(A.startdate as date) <= @DATA_FIM) AS CONFIRMADOS, 
                                                    (select count(*) from dbo.Appointments a 
                                                    where a.ResourceID is not null 
													  AND a.Status = 4 and day(a.StartDate) = day(A1.StartDate) 
													  AND (@ID_PROFISSIONAL = -1 OR a.ResourceID = @ID_PROFISSIONAL)
													  AND cast(A.startdate as date) >= @DATA_INI 
													  AND cast(A.startdate as date) <= @DATA_FIM) AS ATENDIDOS, 
                                                    (select count(*) from dbo.Appointments a 
                                                    where a.ResourceID is not null 
													  and a.Status = 6 
													  and day(a.StartDate) = day(A1.StartDate) 
													  AND (@ID_PROFISSIONAL = -1 OR a.ResourceID = @ID_PROFISSIONAL)
													  AND cast(A.startdate as date) >= @DATA_INI 
													  AND cast(A.startdate as date) <= @DATA_FIM) AS CANCELADOS, 
                                                    (select count(*) from dbo.Appointments a 
                                                    where a.ResourceID is not null 
													  and a.Status = 7 and day(a.StartDate) = day(A1.StartDate) 
													  AND (@ID_PROFISSIONAL = -1 OR a.ResourceID = @ID_PROFISSIONAL)
													  AND cast(A.startdate as date) >= @DATA_INI 
													  AND cast(A.startdate as date) <= @DATA_FIM) AS FALTOU, 
                                                    (select count(*) from dbo.Appointments a 
                                                    where a.ResourceID is not null 
													 and a.Status not in (0, 1, 4, 6) 
													 and day(a.StartDate) = day(A1.StartDate)
													 AND (@ID_PROFISSIONAL = -1 OR a.ResourceID = @ID_PROFISSIONAL) 
													 AND cast(A.startdate as date) >= @DATA_INI 
													 AND cast(A.startdate as date) <= @DATA_FIM) AS OUTROS 
                                                    FROM Appointments A1 
                                                    where a1.ResourceID is not null 
													  AND (@ID_PROFISSIONAL = -1 OR a1.ResourceID = @ID_PROFISSIONAL)
													  AND cast(A1.startdate as date) >= @DATA_INI 
													  AND cast(A1.startdate as date) <= @DATA_FIM 
                                                    group by day(A1.StartDate) 
                                                    ORDER BY day(A1.StartDate)",
                                                    new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                    new SqlParameter("@DATA_INI", dtInicial.Date),
                                                    new SqlParameter("@DATA_FIM", dtFinal.Date)).ToList();                   
            graficos.Add(new Grafico(4, dataList));
        }

        public void GetGraficoFaturamentoServico(int idProfissional, DateTime dtInicial, DateTime dtFinal)
        {
            List<Datadecimal> dataList = new List<Datadecimal>();          
            dataList = db.Database.SqlQuery<Datadecimal>(@"SELECT P.DESCRICAO ColumnName, SUM(VI.VALOR_PAGO) Value
                                                              FROM VENDA V, VENDA_ITEM VI, PROCEDIMENTO P
                                                             WHERE P.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO
                                                               AND V.ID_VENDA = VI.ID_VENDA 
                                                               AND VI.VALOR_PAGO > 0
                                                               AND STATUS = 'C'
                                                               AND (@ID_PROFISSIONAL = -1 OR VI.ID_PROFISSIONAL = @ID_PROFISSIONAL)
                                                               --AND CAST(V.DATA AS DATE) BETWEEN CAST('01/01/2016' AS DATE) AND CAST('31/01/2016' AS DATE)
                                                               AND CAST(V.DATA AS DATE) BETWEEN @DATA_INI AND @DATA_FIM
                                                              GROUP BY P.DESCRICAO 
                                                              order by SUM(VI.VALOR_PAGO) desc ",
                                                            new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                            new SqlParameter("@DATA_INI", dtInicial.Date),
                                                            new SqlParameter("@DATA_FIM", dtFinal.Date)
                                                            ).ToList();
            graficos.Add(new Grafico(5, dataList));
        }                

        public void GetGraficoUltimaMarcacao(int idProfissional, DateTime dtInicial, DateTime dtFinal)
        {
            List<Dataint> dataList = new List<Dataint>();
            dataList = db.Database.SqlQuery<Dataint>(@"SELECT '1 MES' ColumnName, COUNT(DISTINCT ID_CLIENTE) VALUE FROM Appointments A
                                                            WHERE EXISTS(SELECT * FROM Appointments B 
                                                                WHERE B.ID_CLIENTE = A.ID_CLIENTE 
                                                                AND (@ID_PROFISSIONAL = -1 OR B.ResourceID = @ID_PROFISSIONAL)
                                                                AND CAST(B.StartDate AS DATE) BETWEEN DATEADD(MONTH,-1, @DATA_INI) and 
                                                                                                        DATEADD(DAY,-1, @DATA_INI) )
                                                            AND CAST(A.StartDate AS DATE) BETWEEN @DATA_INI and @DATA_FIM 
                                                            UNION ALL
                                                            SELECT '6 MESES' ColumnName, COUNT(DISTINCT ID_CLIENTE) VALUE FROM Appointments A
                                                            WHERE EXISTS(SELECT * FROM Appointments B 
                                                                WHERE B.ID_CLIENTE = A.ID_CLIENTE 
                                                                AND (@ID_PROFISSIONAL = -1 OR B.ResourceID = @ID_PROFISSIONAL)
                                                                AND CAST(B.StartDate AS DATE) BETWEEN DATEADD(MONTH,-6, @DATA_INI) and 
                                                                                                        DATEADD(DAY,-1, DATEADD(MONTH,-1, @DATA_INI)) )
                                                            AND CAST(A.StartDate AS DATE) BETWEEN @DATA_INI and @DATA_FIM
                                                            UNION ALL
                                                            SELECT '1 ANO' ColumnName, COUNT(DISTINCT ID_CLIENTE) VALUE FROM Appointments A
                                                            WHERE EXISTS(SELECT * FROM Appointments B 
                                                                WHERE B.ID_CLIENTE = A.ID_CLIENTE 
                                                                AND (@ID_PROFISSIONAL = -1 OR B.ResourceID = @ID_PROFISSIONAL)
                                                                AND CAST(B.StartDate AS DATE) BETWEEN DATEADD(YEAR,-1, @DATA_INI) and 
                                                                                                      DATEADD(DAY,-1, DATEADD(MONTH,-6, @DATA_INI)) )
                                                            AND CAST(A.StartDate AS DATE) BETWEEN @DATA_INI and @DATA_FIM
                                                            UNION ALL
                                                            SELECT '3 ANOS' ColumnName, COUNT(DISTINCT ID_CLIENTE) VALUE FROM Appointments A
                                                            WHERE EXISTS(SELECT * FROM Appointments B 
                                                                WHERE B.ID_CLIENTE = A.ID_CLIENTE 
                                                                AND (@ID_PROFISSIONAL = -1 OR B.ResourceID = @ID_PROFISSIONAL)
                                                                AND CAST(B.StartDate AS DATE) BETWEEN DATEADD(YEAR,-3, @DATA_INI) and 
                                                                                                       DATEADD(DAY,-1, DATEADD(YEAR,-1, @DATA_INI)) )
                                                            AND CAST(A.StartDate AS DATE) BETWEEN @DATA_INI and @DATA_FIM
                                                            UNION ALL
                                                            SELECT '5 ANOS' ColumnName, COUNT(DISTINCT ID_CLIENTE) VALUE FROM Appointments A
                                                            WHERE EXISTS(SELECT * FROM Appointments B 
                                                                WHERE B.ID_CLIENTE = A.ID_CLIENTE 
                                                                AND (@ID_PROFISSIONAL = -1 OR B.ResourceID = @ID_PROFISSIONAL)
                                                                AND CAST(B.StartDate AS DATE) BETWEEN DATEADD(YEAR,-5, @DATA_INI) and 
                                                                                                      DATEADD(DAY,-3, DATEADD(YEAR,-4, @DATA_INI)) )
                                                            AND (@ID_PROFISSIONAL = -1 OR A.ResourceID = @ID_PROFISSIONAL)
                                                            AND CAST(A.StartDate AS DATE) BETWEEN @DATA_INI and @DATA_FIM         ",
                                                            new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                            new SqlParameter("@DATA_INI", dtInicial.Date),
                                                            new SqlParameter("@DATA_FIM", dtFinal.Date)
                                                            ).ToList();
            graficos.Add(new Grafico(6, dataList));            
        }
              
        [Route("api/gestaovista/indicadores/{dtInicial}/{dtFinal}")]
        public IEnumerable<GestaoVista> GetGESTAO_VISTA(DateTime dtInicial, DateTime dtFinal)
        {
            GestaoVista gestaoVista = new GestaoVista();
            List<GestaoVista> gv = new List<GestaoVista>();

            gestaoVista.ARECEBER = db.Database.SqlQuery<decimal?>(@"SELECT ISNULL(SUM(SALDO),0) ARECEBER
                                                                      FROM DOCUMENTO_FINANCEIRO 
                                                                     WHERE STATUS <> 'Q'  
                                                                       AND TIPO = 'C'
                                                                       AND CAST(DATA_VENCIMENTO AS DATE) >= @DATA_INI 
                                                                       AND CAST(DATA_VENCIMENTO AS DATE) <= @DATA_FIM", 
                                                                       new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                       new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            gestaoVista.ARECEBER = gestaoVista.ARECEBER == null ? 0 : gestaoVista.ARECEBER;

            gestaoVista.APAGAR = db.Database.SqlQuery<decimal?>(@"SELECT ISNULL(SUM(SALDO),0) APAGAR
                                                                      FROM DOCUMENTO_FINANCEIRO 
                                                                     WHERE STATUS <> 'Q'  
                                                                       AND TIPO = 'D'
                                                                       AND CAST(DATA_VENCIMENTO AS DATE) >= @DATA_INI 
                                                                       AND CAST(DATA_VENCIMENTO AS DATE) <= @DATA_FIM",
                                                                       new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                       new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            gestaoVista.APAGAR = gestaoVista.APAGAR == null ? 0 : gestaoVista.APAGAR;

            gestaoVista.RECEBIDO = db.Database.SqlQuery<decimal?>(@"SELECT ISNULL(SUM(VALOR),0) RECEBIDO 
                                                                      FROM DOCUMENTO_FINANCEIRO 
                                                                     WHERE STATUS = 'Q'  
                                                                       AND TIPO = 'C'
                                                                       AND CAST(DATA_VENCIMENTO AS DATE) >= @DATA_INI 
                                                                       AND CAST(DATA_VENCIMENTO AS DATE) <= @DATA_FIM",
                                                                       new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                       new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            gestaoVista.RECEBIDO = gestaoVista.RECEBIDO == null ? 0 : gestaoVista.RECEBIDO;

            gestaoVista.PAGOS = db.Database.SqlQuery<decimal?>(@"SELECT ISNULL(SUM(VALOR),0) PAGOS 
                                                                      FROM DOCUMENTO_FINANCEIRO 
                                                                     WHERE STATUS = 'Q'  
                                                                       AND TIPO = 'D'
                                                                       AND CAST(DATA_VENCIMENTO AS DATE) >= @DATA_INI 
                                                                       AND CAST(DATA_VENCIMENTO AS DATE) <= @DATA_FIM",
                                                                       new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                       new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            gestaoVista.PAGOS = gestaoVista.PAGOS == null ? 0 : gestaoVista.PAGOS;

            gestaoVista.NOVOS_PACIENTES = db.Database.SqlQuery<int?>(@"SELECT COUNT(V.ID_CLIENTE) FROM VENDA V
                                                                        WHERE cast(V.DATA as date) BETWEEN @DATA_INI and @DATA_FIM 
                                                                          AND V.STATUS = 'C'
                                                                          AND NOT EXISTS(SELECT * FROM VENDA VI
                                                                                          WHERE cast(VI.DATA as date) BETWEEN DATEADD(mm, -24, @DATA_INI) AND DATEADD(dd, -1, @DATA_INI)
				                                                                            and VI.ID_CLIENTE = V.ID_CLIENTE )",
                                                                        new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            gestaoVista.NOVOS_PACIENTES = gestaoVista.NOVOS_PACIENTES == null ? 0 : gestaoVista.NOVOS_PACIENTES;

            gestaoVista.TICKET_MEDIO = db.Database.SqlQuery<decimal?>("select cast( (sum(valor)/count(*)) as decimal(15,2)) AS TICKET_MEDIO from VENDA where status = 'C' and cast(data as date) >= @DATA_INI and cast(data as date) <= @DATA_FIM",
                                                                         new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            gestaoVista.TICKET_MEDIO = gestaoVista.TICKET_MEDIO == null ? 0 : gestaoVista.TICKET_MEDIO;            

            gestaoVista.BASKET = db.Database.SqlQuery<decimal?>(@"select cast( (select CAST(count(vi.ID_ITEM_VENDA) AS decimal) from dbo.VENDA_ITEM vi 
                                                                                 left join dbo.venda v on v.ID_VENDA = vi.ID_VENDA 
                                                                                 where cast(data as date) >= @DATA_INI and cast(data as date) <= @DATA_FIM and v.STATUS = 'C') / 
                                                                                 nullif( (select CAST(count(*) AS DECIMAL) from dbo.venda where cast(data as date) >= @DATA_INI and cast(data as date) <= @DATA_FIM and STATUS = 'C'), 0) as decimal(15,2)) AS BASKET", 
                                                                         new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            gestaoVista.BASKET = gestaoVista.BASKET == null ? 0 : gestaoVista.BASKET;

            gestaoVista.TAXA_OCUPACAO = db.Database.SqlQuery<decimal?>(@"SELECT AVG(VALUE) VALOR
                                                                FROM (
                                                                select pp.NOME as ColumnName, cast(isnull(avg(pp1.perc),0)as decimal(15,2)) as VALUE                                                                    
                                                                from profissional pp
                                                                left join (select p.id_profissional, 
                                                                            cast( ((select Sum(DATEDIFF(MINUTE, StartDate, EndDate)) / 
				                                                                            nullif( (datediff(week, dateadd(month, datediff(month, 0, getdate()),
							                                                                0), getdate()) + 1),0) 
                                                                                    from dbo.Appointments a 
			   	                                                                    where cast(a.StartDate as date) >= cast(@DATA_INI as date) 
						                                                                and cast(a.StartDate as date) <= cast(GetDate() as date)
                                                                                        and datepart(DW, a.StartDate) = p.COD_DIA_SEMANA 
						                                                                and a.ResourceID = p.ID_PROFISSIONAL
						                                                                AND (@ID_PROFISSIONAL = -1 OR a.ResourceID = @ID_PROFISSIONAL)
                                                                                    group by  datepart(DW, StartDate)) * 100) / 
                                                                                    ((select  SUM(DATEDIFF(MINUTE, HORA_INICIAL_1, HORA_FINAL_1) + 
                                                                                                    DATEDIFF(MINUTE, HORA_INICIAL_2, HORA_FINAL_2) + 
                                                                                                    DATEDIFF(MINUTE, HORA_INICIAL_3, HORA_FINAL_3))
                                                                                        from dbo.PROFISSIONAL_HORARIO P1 
                                                                                        WHERE P1.COD_DIA_SEMANA = P.COD_DIA_SEMANA
						                                                                and P1.ID_PROFISSIONAL = p.ID_PROFISSIONAL
						                                                                and (@ID_PROFISSIONAL = -1 OR P1.ID_PROFISSIONAL = @ID_PROFISSIONAL) ) ) as decimal(15,2)) AS PERC 
                                                                        from dbo.PROFISSIONAL_HORARIO P 
		                                                                where (@ID_PROFISSIONAL = -1 OR P.ID_PROFISSIONAL = @ID_PROFISSIONAL)
                                                                        group by P.ID_PROFISSIONAL, P.COD_DIA_SEMANA
                                                                ) as pp1 on pp1.ID_PROFISSIONAL = pp.ID_PROFISSIONAL
                                                                where (@ID_PROFISSIONAL = -1 OR pp.ID_PROFISSIONAL = @ID_PROFISSIONAL)
                                                                group by pp.ID_PROFISSIONAL, pp.NOME 
                                                                having isnull(avg(pp1.perc),0) > 0
                                                                )A",
                                                                  new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                  new SqlParameter("@ID_PROFISSIONAL", -1)).First();
            gestaoVista.TAXA_OCUPACAO = gestaoVista.TAXA_OCUPACAO == null ? 0 : gestaoVista.TAXA_OCUPACAO;

            gestaoVista.REATIVADOS = db.Database.SqlQuery<int?>(@"SELECT COUNT(V.ID_CLIENTE) FROM VENDA V
                                                                   WHERE cast(V.DATA as date) BETWEEN @DATA_INI and @DATA_FIM 
                                                                    AND V.STATUS = 'C'
                                                                    AND NOT EXISTS(SELECT * FROM VENDA VI
                                                                                    WHERE cast(VI.DATA as date) BETWEEN DATEADD(mm, -24, @DATA_INI) AND DATEADD(dd, -1, @DATA_INI)
				                                                                    and VI.ID_CLIENTE = V.ID_CLIENTE )
																    AND EXISTS (SELECT * FROM VENDA VI
                                                                                    WHERE cast(VI.DATA as date) < DATEADD(mm, -24, @DATA_INI)
				                                                                    and VI.ID_CLIENTE = V.ID_CLIENTE)",
                                                                        new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            gestaoVista.REATIVADOS = gestaoVista.REATIVADOS == null ? 0 : gestaoVista.REATIVADOS;

            IEnumerable<Dataint> dadosPacientes;
            dadosPacientes = db.Database.SqlQuery<Dataint>(@"SELECT 'ATENDIDO' ColumnName, COUNT(A.UniqueID) VALUE
                                                                            FROM Appointments A 
                                                                            WHERE A.Status = 4
                                                                            AND CAST(A.StartDate AS DATE) BETWEEN @DATA_INI AND @DATA_FIM
                                                                            UNION ALL
                                                                            SELECT 'FATURADO' ColumnName, COUNT(A.UniqueID) VALUE
                                                                                FROM Appointments A, VENDA_ITEM VI, VENDA V
                                                                                WHERE VI.ID_APPOINTMENT = A.UniqueID
																				AND VI.ID_VENDA = V.ID_VENDA
																				AND V.STATUS = 'C'
																				AND A.Status = 4                                                                                
                                                                                AND CAST(A.StartDate AS DATE) BETWEEN @DATA_INI AND @DATA_FIM
                                                                            UNION ALL
                                                                            SELECT 'SERVICO_ATENDIDO' ColumnName, COUNT(DISTINCT A.ID_PROCEDIMENTO) VALUE 
                                                                                FROM Appointments A
                                                                                WHERE A.Status = 4
                                                                                AND A.ID_PROCEDIMENTO IS NOT NULL
                                                                                AND CAST(A.StartDate AS DATE) BETWEEN @DATA_INI AND @DATA_FIM
                                                                            UNION ALL
                                                                            SELECT 'SERVICO_FATURADO' ColumnName, COUNT(DISTINCT VI.ID_PROCEDIMENTO) VALUE
                                                                                FROM VENDA V, VENDA_ITEM VI
                                                                                WHERE V.ID_VENDA = VI.ID_VENDA
																				AND V.STATUS = 'C'                                                                                
                                                                                AND VI.ID_PROCEDIMENTO IS NOT NULL
                                                                                AND CAST(V.DATA AS DATE) BETWEEN @DATA_INI AND @DATA_FIM",
                                                                        new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).ToList();

            gestaoVista.PACIENTES_ATENDIDOS = 0;
            gestaoVista.PACIENTES_FATURADOS = 0;
            gestaoVista.SERVICOS_ATENDIDOS  = 0;
            gestaoVista.SERVICOS_FATURADOS  = 0;
            foreach (Dataint reg in dadosPacientes)
            {
                if (reg.ColumnName == "ATENDIDO")
                    gestaoVista.PACIENTES_ATENDIDOS = reg.Value;
                else if (reg.ColumnName == "FATURADO")
                    gestaoVista.PACIENTES_FATURADOS = reg.Value;
                else if (reg.ColumnName == "SERVICO_ATENDIDO")
                    gestaoVista.SERVICOS_ATENDIDOS = reg.Value;
                else if (reg.ColumnName == "SERVICO_FATURADO")
                    gestaoVista.SERVICOS_FATURADOS = reg.Value;
            }
            
            decimal? percent_faturamento = db.Database.SqlQuery<decimal?>(@"SELECT (SUM(VI.VALOR_PAGO) * 80) / 100                                                                      
                                                                             FROM VENDA V, VENDA_ITEM VI
                                                                            WHERE VI.ID_VENDA = V.ID_VENDA
                                                                                AND V.STATUS = 'C' 
                                                                                AND CAST(V.DATA AS DATE) BETWEEN @DATA_INI and @DATA_FIM ",
                                                                        new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();

            IEnumerable<ClientePagamento> cliPagto;
            cliPagto = db.Database.SqlQuery<ClientePagamento>(@"SELECT V.ID_CLIENTE,  SUM(VI.VALOR_PAGO) VALOR_PAGO                                                                     
                                                                  FROM VENDA V, VENDA_ITEM VI
                                                                WHERE VI.ID_VENDA = V.ID_VENDA
                                                                 AND V.STATUS = 'C' 
                                                                 AND CAST(V.DATA AS DATE) BETWEEN @DATA_INI and @DATA_FIM 
                                                                GROUP BY V.ID_CLIENTE 
                                                                order by 2 desc",
                                                                  new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                  new SqlParameter("@DATA_FIM", dtFinal.Date)).ToList();
            int qtd_Cli = 0;
            decimal? valor_acumulado = 0;
            if (cliPagto.Count() > 0)
            {
                foreach (ClientePagamento tx in cliPagto)
                {
                    qtd_Cli++;
                    valor_acumulado =+ tx.VALOR_PAGO;
                    if (valor_acumulado >= percent_faturamento)
                        break;                         
                }                
            }
            gestaoVista.PARETTO = qtd_Cli;
            gestaoVista.PARETTO = gestaoVista.PARETTO == null ? 0 : gestaoVista.PARETTO;

            gv.Add(gestaoVista);

            return gv;
        }
                
        [Route("api/gestaoavista/graficos_evolutivos/{dtInicial}/{dtFinal}")]
        public IEnumerable<Grafico> GetGraficosEvolutivos(DateTime dtInicial, DateTime dtFinal)
        {            
            GetGraficoTiketMedio(dtInicial, dtFinal);
            GetGraficoQtdItensFaturados(dtInicial, dtFinal);            
            return graficos;
        }

        public void GetGraficoTiketMedio(DateTime dtInicial, DateTime dtFinal)
        {
            List<Datadecimal> dataList = new List<Datadecimal>();
            dataList = db.Database.SqlQuery<Datadecimal>(@"SELECT CAST(MONTH(DATA) AS NVARCHAR) +'/'+CAST(YEAR(DATA) AS NVARCHAR) ColumnName,
				                                                  CAST( (sum(valor)/count(*)) as decimal(15,2)) AS VALUE 
			                                                 FROM VENDA where status = 'C' and cast(data as date) >= @DATA_INI and cast(data as date) <= @DATA_FIM
			                                                 GROUP BY CAST(YEAR(DATA) AS NVARCHAR), MONTH(DATA) ",
                                                            new SqlParameter("@DATA_INI", dtInicial.Date),
                                                            new SqlParameter("@DATA_FIM", dtFinal.Date)
                                                            ).ToList();
            graficos.Add(new Grafico(7, dataList));
        }

        public void GetGraficoQtdItensFaturados(DateTime dtInicial, DateTime dtFinal)
        {
            List<Datadecimal> dataList = new List<Datadecimal>();
            //Gráfico Perfil Sexo
            //dataList.Add(new Data("FEMININO", 5000));
            //dataList.Add(new Data("MASCULINO", 3500));
            //graficos.Add(new Grafico(1, dataList));            
            dataList = db.Database.SqlQuery<Datadecimal>(@"select CAST(MES AS NVARCHAR) +'/'+CAST(ANO AS NVARCHAR) ColumnName,
	                                                            C.VALOR VALUE
			                                                FROM(SELECT A.MES, A.ANO, CAST(A.QTD/B.TOTAL AS decimal(15,2)) VALOR
					                                              FROM (select MONTH(DATA) MES, YEAR(DATA) ANO, count(vi.ID_ITEM_VENDA) QTD 
							                                        from dbo.VENDA_ITEM vi 
							                                        left join dbo.venda v on v.ID_VENDA = vi.ID_VENDA 
						                                            where cast(data as date) >= @DATA_INI
							                                            and cast(data as date) <= @DATA_FIM 
							                                            and v.STATUS = 'C'
							                                        GROUP BY MONTH(DATA), YEAR(DATA) ) A,
						                                            (select MONTH(DATA) MES, YEAR(DATA) ANO, count(*) TOTAL 
							                                            from dbo.venda 
							                                        where cast(data as date) >= @DATA_INI 
							                                            and cast(data as date) <= @DATA_FIM 
							                                            and STATUS = 'C'
							                                        GROUP BY MONTH(DATA), YEAR(DATA) ) B
					                                        WHERE A.MES = B.MES
					                                          AND A.ANO = A.ANO) C
                                                          ORDER BY ANO, MES",
                                                            new SqlParameter("@DATA_INI", dtInicial.Date),
                                                            new SqlParameter("@DATA_FIM", dtFinal.Date)).ToList();

            graficos.Add(new Grafico(8, dataList));
        }

        [Route("api/gestaovista/indicadores/profissional/{idProfissional}/{dtInicial}/{dtFinal}")]
        public IEnumerable<GestaoVista> GetGESTAO_VISTAPROFISSIONAL(int idProfissional, DateTime dtInicial, DateTime dtFinal)
        {
            GestaoVista gestaoVista = new GestaoVista();
            List<GestaoVista> gv = new List<GestaoVista>();            

            gestaoVista.NOVOS_PACIENTES = db.Database.SqlQuery<int?>(@"select count(*) AS TOTAL from dbo.Appointments a 
                                                                        where cast(a.StartDate as date) >= @DATA_INI 
                                                                         and cast(a.StartDate as date) <= @DATA_FIM 
                                                                         and a.Status = 4 
                                                                         and a.ResourceID = @ID_PROFISSIONAL
                                                                         and not exists (select b.id_cliente 
                                                                                          from Appointments b   
                                                                                         where cast(b.StartDate as date) < @DATA_INI 
                                                                                           and b.Status = 4
					                                                                       and b.ID_CLIENTE = a.ID_CLIENTE)",
                                                                        new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                                        new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            gestaoVista.NOVOS_PACIENTES = gestaoVista.NOVOS_PACIENTES == null ? 0 : gestaoVista.NOVOS_PACIENTES;

            gestaoVista.TAXA_OCUPACAO = db.Database.SqlQuery<decimal?>(@"SELECT AVG(VALUE) VALOR
                                                                FROM (
                                                                select pp.NOME as ColumnName, cast(isnull(avg(pp1.perc),0)as decimal(15,2)) as VALUE                                                                    
                                                                from profissional pp
                                                                left join (select p.id_profissional, 
                                                                            cast( ((select Sum(DATEDIFF(MINUTE, StartDate, EndDate)) / 
				                                                                            nullif( (datediff(week, dateadd(month, datediff(month, 0, getdate()),
							                                                                0), getdate()) + 1),0) 
                                                                                    from dbo.Appointments a 
			   	                                                                    where cast(a.StartDate as date) >= cast(@DATA_INI as date) 
						                                                                and cast(a.StartDate as date) <= cast(GetDate() as date)
                                                                                        and datepart(DW, a.StartDate) = p.COD_DIA_SEMANA 
						                                                                and a.ResourceID = p.ID_PROFISSIONAL
						                                                                AND (@ID_PROFISSIONAL = -1 OR a.ResourceID = @ID_PROFISSIONAL)
                                                                                    group by  datepart(DW, StartDate)) * 100) / 
                                                                                    ((select  SUM(DATEDIFF(MINUTE, HORA_INICIAL_1, HORA_FINAL_1) + 
                                                                                                    DATEDIFF(MINUTE, HORA_INICIAL_2, HORA_FINAL_2) + 
                                                                                                    DATEDIFF(MINUTE, HORA_INICIAL_3, HORA_FINAL_3))
                                                                                        from dbo.PROFISSIONAL_HORARIO P1 
                                                                                        WHERE P1.COD_DIA_SEMANA = P.COD_DIA_SEMANA
						                                                                and P1.ID_PROFISSIONAL = p.ID_PROFISSIONAL
						                                                                and (@ID_PROFISSIONAL = -1 OR P1.ID_PROFISSIONAL = @ID_PROFISSIONAL) ) ) as decimal(15,2)) AS PERC 
                                                                        from dbo.PROFISSIONAL_HORARIO P 
		                                                                where (@ID_PROFISSIONAL = -1 OR P.ID_PROFISSIONAL = @ID_PROFISSIONAL)
                                                                        group by P.ID_PROFISSIONAL, P.COD_DIA_SEMANA
                                                                ) as pp1 on pp1.ID_PROFISSIONAL = pp.ID_PROFISSIONAL
                                                                where (@ID_PROFISSIONAL = -1 OR pp.ID_PROFISSIONAL = @ID_PROFISSIONAL)
                                                                group by pp.ID_PROFISSIONAL, pp.NOME 
                                                                having isnull(avg(pp1.perc),0) > 0
                                                                )A",
                                                                  new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                  new SqlParameter("@ID_PROFISSIONAL", idProfissional)).First();
            gestaoVista.TAXA_OCUPACAO = gestaoVista.TAXA_OCUPACAO == null ? 0 : gestaoVista.TAXA_OCUPACAO;

            gestaoVista.REATIVADOS = db.Database.SqlQuery<int?>(@"SELECT COUNT(V.ID_CLIENTE) FROM VENDA V
                                                                   WHERE cast(V.DATA as date) BETWEEN @DATA_INI and @DATA_FIM 
                                                                    AND V.STATUS = 'C'
                                                                    AND NOT EXISTS(SELECT * FROM VENDA VI
                                                                                    WHERE cast(VI.DATA as date) BETWEEN DATEADD(mm, -24, @DATA_INI) AND DATEADD(dd, -1, @DATA_INI)
				                                                                    and VI.ID_CLIENTE = V.ID_CLIENTE )
																    AND EXISTS (SELECT * FROM VENDA VI
                                                                                    WHERE cast(VI.DATA as date) < DATEADD(mm, -24, @DATA_INI)
				                                                                    and VI.ID_CLIENTE = V.ID_CLIENTE)",
                                                                        new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            gestaoVista.REATIVADOS = gestaoVista.REATIVADOS == null ? 0 : gestaoVista.REATIVADOS;

            IEnumerable<Dataint> dadosPacientes;
            dadosPacientes = db.Database.SqlQuery<Dataint>(@"SELECT 'ATENDIDO' ColumnName, COUNT(A.UniqueID) VALUE
                                                            FROM Appointments A 
                                                            WHERE A.Status = 4
															AND A.ResourceID = @ID_PROFISSIONAL
                                                            AND CAST(A.StartDate AS DATE) BETWEEN @DATA_INI AND @DATA_FIM
                                                            UNION ALL
                                                            SELECT 'FATURADO' ColumnName, COUNT(A.UniqueID) VALUE
                                                            FROM Appointments A, VENDA_ITEM VI, VENDA V
                                                            WHERE VI.ID_APPOINTMENT = A.UniqueID
															AND VI.ID_VENDA = V.ID_VENDA
															AND V.STATUS = 'C'
															AND A.Status = 4
															AND VI.ID_PROFISSIONAL = @ID_PROFISSIONAL                                                                                
                                                            AND CAST(A.StartDate AS DATE) BETWEEN @DATA_INI AND @DATA_FIM
                                                            UNION ALL
                                                            SELECT 'SERVICO_ATENDIDO' ColumnName, COUNT(DISTINCT A.ID_PROCEDIMENTO) VALUE 
                                                                FROM Appointments A
                                                                WHERE A.Status = 4
                                                                AND A.ID_PROCEDIMENTO IS NOT NULL
																AND A.ResourceID = @ID_PROFISSIONAL
                                                                AND CAST(A.StartDate AS DATE) BETWEEN @DATA_INI AND @DATA_FIM
                                                            UNION ALL
                                                            SELECT 'SERVICO_FATURADO' ColumnName, COUNT(DISTINCT VI.ID_PROCEDIMENTO) VALUE
                                                            FROM VENDA V, VENDA_ITEM VI
                                                            WHERE V.ID_VENDA = VI.ID_VENDA
															AND V.STATUS = 'C'                                                                                
                                                            AND VI.ID_PROCEDIMENTO IS NOT NULL
															AND VI.ID_PROFISSIONAL = @ID_PROFISSIONAL
                                                            AND CAST(V.DATA AS DATE) BETWEEN @DATA_INI AND @DATA_FIM",
                                                                        new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                                        new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).ToList();

            gestaoVista.PACIENTES_ATENDIDOS = 0;
            gestaoVista.PACIENTES_FATURADOS = 0;
            gestaoVista.SERVICOS_ATENDIDOS = 0;
            gestaoVista.SERVICOS_FATURADOS = 0;
            foreach (Dataint reg in dadosPacientes)
            {
                if (reg.ColumnName == "ATENDIDO")
                    gestaoVista.PACIENTES_ATENDIDOS = reg.Value;
                else if (reg.ColumnName == "FATURADO")
                    gestaoVista.PACIENTES_FATURADOS = reg.Value;
                else if (reg.ColumnName == "SERVICO_ATENDIDO")
                    gestaoVista.SERVICOS_ATENDIDOS = reg.Value;
                else if (reg.ColumnName == "SERVICO_FATURADO")
                    gestaoVista.SERVICOS_FATURADOS = reg.Value;
            }

            decimal? percent_faturamento = db.Database.SqlQuery<decimal?>(@"SELECT (SUM(VI.VALOR_PAGO) * 80) / 100                                                                      
                                                                             FROM VENDA V, VENDA_ITEM VI
                                                                            WHERE VI.ID_VENDA = V.ID_VENDA
                                                                             AND V.STATUS = 'C' 
                                                                             AND VI.ID_PROFISSIONAL = @ID_PROFISSIONAL
                                                                             AND CAST(V.DATA AS DATE) BETWEEN @DATA_INI and @DATA_FIM ",
                                                                        new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                                        new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();

            IEnumerable<ClientePagamento> cliPagto;
            cliPagto = db.Database.SqlQuery<ClientePagamento>(@"SELECT V.ID_CLIENTE,  SUM(VI.VALOR_PAGO) VALOR_PAGO                                                                     
                                                                  FROM VENDA V, VENDA_ITEM VI
                                                                WHERE VI.ID_VENDA = V.ID_VENDA
                                                                 AND V.STATUS = 'C' 
                                                                 AND VI.ID_PROFISSIONAL = @ID_PROFISSIONAL
                                                                 AND CAST(V.DATA AS DATE) BETWEEN @DATA_INI and @DATA_FIM 
                                                                GROUP BY V.ID_CLIENTE 
                                                                order by 2 desc",
                                                                new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                                new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                new SqlParameter("@DATA_FIM", dtFinal.Date)).ToList();
            int qtd_Cli = 0;
            decimal? valor_acumulado = 0;
            if (cliPagto.Count() > 0)
            {
                foreach (ClientePagamento tx in cliPagto)
                {
                    qtd_Cli++;
                    valor_acumulado = +tx.VALOR_PAGO;
                    if (valor_acumulado >= percent_faturamento)
                        break;
                }
            }
            gestaoVista.PARETTO = qtd_Cli;
            gestaoVista.PARETTO = gestaoVista.PARETTO == null ? 0 : gestaoVista.PARETTO;

            gestaoVista.TICKET_MEDIO = db.Database.SqlQuery<decimal?>(@"select cast( (sum(valor)/count(*)) as decimal(15,2)) AS TICKET_MEDIO 
                                                                        from VENDA V
                                                                        where exists (select * 
                                                                                        from VENDA_ITEM VI 
                                                                                       WHERE VI.ID_VENDA = V.ID_VENDA
			                                                                             AND VI.ID_PROFISSIONAL = @ID_PROFISSIONAL)
                                                                        and status = 'C' 
                                                                        and cast(data as date) >= @DATA_INI 
                                                                        and cast(data as date) <= @DATA_FIM",
                                                             new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                             new SqlParameter("@DATA_INI", dtInicial.Date),
                                                             new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            gestaoVista.TICKET_MEDIO = gestaoVista.TICKET_MEDIO == null ? 0 : gestaoVista.TICKET_MEDIO;

            gestaoVista.BASKET = db.Database.SqlQuery<decimal?>(@"select cast( 
                                                                    cast( (select count(vi.ID_ITEM_VENDA) 
			                                                           from dbo.VENDA_ITEM vi 
                                                                        left join dbo.venda v on v.ID_VENDA = vi.ID_VENDA 
                                                                        where cast(data as date) >= @DATA_INI 
				                                                          and cast(data as date) <= @DATA_FIM 
				                                                          and v.STATUS = 'C'
                                                                          AND VI.ID_PROFISSIONAL = @ID_PROFISSIONAL) as decimal) / 
                                                                 cast( nullif(select cast(count(*)
		                                                                    from dbo.venda v 
				                                                           where cast(data as date) >= @DATA_INI 
				                                                             and cast(data as date) <= @DATA_FIM 
					                                                         and STATUS = 'C'
                                                                             and exists (select * 
								                                                         from VENDA_ITEM VI 
								                                                          WHERE VI.ID_VENDA = V.ID_VENDA
									                                                        AND VI.ID_PROFISSIONAL = @ID_PROFISSIONAL)), 0) as decimal) as decimal(15,2)) AS BASKET",
                                                                      new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                                      new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                      new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            gestaoVista.BASKET = gestaoVista.BASKET == null ? 0 : gestaoVista.BASKET;

            gestaoVista.COMISSAO = db.Database.SqlQuery<decimal?>(@"SELECT SUM(VALOR_COMISSAO)VALUE FROM V_COMISSOES VC
                                                                     WHERE CAST(VC.DATA_PAGAMENTO AS DATE) BETWEEN @DATA_INI and @DATA_FIM
                                                                      AND ID_PROFISSIONAL = @ID_PROFISSIONAL",
                                                                        new SqlParameter("@ID_PROFISSIONAL", idProfissional),
                                                                        new SqlParameter("@DATA_INI", dtInicial.Date),
                                                                        new SqlParameter("@DATA_FIM", dtFinal.Date)).First();
            gestaoVista.COMISSAO = gestaoVista.COMISSAO == null ? 0 : gestaoVista.COMISSAO;

            gv.Add(gestaoVista);

            return gv;
        }

        [Route("api/gestaoavista/graficos/profissional/{idProfissional}/{dtInicial}/{dtFinal}")]
        public IEnumerable<Grafico> GetGraficosProfissional(int idProfissional, DateTime dtInicial, DateTime dtFinal)
        {
            //ResumoAgenda itemResumo = new ResumoAgenda();
            //List<ResumoAgenda> resumo = new List<ResumoAgenda>();

            List<AgendaDia> dataListAgenda = new List<AgendaDia>();
            GetGraficoPerfilPorSexo(idProfissional, dtInicial, dtFinal);
            GetGraficoPerfilPorIdade(idProfissional, dtInicial, dtFinal);
            GetGraficoTaxaOcupacaoProfissional(idProfissional, dtInicial, dtFinal);
            GetGraficoAgendaDia(idProfissional, dtInicial, dtFinal);
            GetGraficoFaturamentoServico(idProfissional, dtInicial, dtFinal);
            GetGraficoUltimaMarcacao(idProfissional, dtInicial, dtFinal);
            return graficos;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentsExists(int id)
        {
            return db.Appointments.Count(e => e.UniqueID == id) > 0;
        }
    }
}