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
    public class ESTOQUE_MOVIMENTACAODTO
    {
        public int ID_ESTOQUE_MOVIMENTACAO { get; set; }
        public int ID_ESTOQUE { get; set; }    
        public int ID_PRODUTO { get; set; }
        public string PRODUTO { get; set; }
        public string TIPO { get; set; }
        public int ORIGEM { get; set; }
        public decimal QUANTIDADE_ESTOQUE { get; set; }
        public decimal QUANTIDADE_MOVIMENTACAO { get; set; }
        public DateTime DATA_MOVIMENTACAO { get; set; }
        public string OBSERVACAO { get; set; }
        public Nullable<int> ID_USUARIO { get; set; }
        public string DESC_TIPO { get; set; }
        public string DESC_ORIGEM { get; set; }
        public string LOGIN { get; set; }
        public int ID_UNIDADE { get; set; }
        public string ENTIDADE { get; set; }
        
    }

    public class EstoqueMovimentacaoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        string Sql_EstoqueMovimentacao = @"SELECT ID_ESTOQUE_MOVIMENTACAO, ID_ESTOQUE, ID_PRODUTO, PRODUTO, TIPO, ORIGEM, QUANTIDADE_ESTOQUE, 
                                                  QUANTIDADE_MOVIMENTACAO, DATA_MOVIMENTACAO, OBSERVACAO, ID_USUARIO, DESC_TIPO, DESC_ORIGEM, 
                                                  LOGIN, ID_UNIDADE, ENTIDADE
                                             FROM V_ESTOQUE_MOVIMENTACAO
                                            WHERE 1 = 1";

        [Route("api/EstoqueMovimentacao", Name = "GetEstoqueMovimentacoes")]
        public IEnumerable<ESTOQUE_MOVIMENTACAODTO> GetEstoqueMovimentacoes()
        {
            System.Text.StringBuilder sbsql = new System.Text.StringBuilder();
            sbsql.Append(Sql_EstoqueMovimentacao);
            sbsql.Append(" ORDER BY ID_ESTOQUE, DATA_MOVIMENTACAO");
            return db.Database.SqlQuery<ESTOQUE_MOVIMENTACAODTO>(sbsql.ToString()).ToList();
        }

        //api/EstoqueMovimentacao/{0}/{1}/{2}/{3}
        [Route("api/EstoqueMovimentacao/{dtInicial}/{dtFinal}/{tipo}/{idEntidade}", Name = "GetEstoqueMovimentacao")]
        public IEnumerable<ESTOQUE_MOVIMENTACAODTO> GetEstoqueMovimentacao(DateTime dtInicial, DateTime dtFinal, string tipo, int idEntidade)
        {
            System.Text.StringBuilder sbsql = new System.Text.StringBuilder();
            sbsql.Append(Sql_EstoqueMovimentacao);
            sbsql.Append(string.Format(" AND CAST(DATA_MOVIMENTACAO AS DATE) BETWEEN CAST('{0}' AS DATE) AND CAST('{1}' AS DATE) ", dtInicial, dtFinal));

            if (tipo != "T")
                sbsql.Append(string.Format(" AND TIPO = '{0}'", tipo));

            if(idEntidade > 0)
            {
                sbsql.Append(string.Format(" AND ID_UNIDADE = {0}", idEntidade));
            }

            sbsql.Append(" ORDER BY ID_ESTOQUE, DATA_MOVIMENTACAO");
            return db.Database.SqlQuery<ESTOQUE_MOVIMENTACAODTO>(sbsql.ToString()).ToList();
        }

        [Route("api/EstoqueMovimentacao/Estoque/{idEstoque}", Name = "GetEstoqueMovimentacaoPorEstoque")]
        public IEnumerable<ESTOQUE_MOVIMENTACAODTO> GetEstoqueMovimentacaoPorEstoque(int idEstoque)
        {
            System.Text.StringBuilder sbsql = new System.Text.StringBuilder();
            sbsql.Append(Sql_EstoqueMovimentacao);
            sbsql.Append(string.Format(" AND ID_ESTOQUE = {0}", idEstoque));
            sbsql.Append(" ORDER BY ID_ESTOQUE, DATA_MOVIMENTACAO");
            return db.Database.SqlQuery<ESTOQUE_MOVIMENTACAODTO>(sbsql.ToString()).ToList();
        }

        [Route("api/EstoqueMovimentacao/Entidade/{idUnidade}", Name = "GetEstoqueMovimentacaoPorEntidade")]
        public IEnumerable<ESTOQUE_MOVIMENTACAODTO> GetEstoqueMovimentacaoPorEntidade(int idUnidade)
        {
            System.Text.StringBuilder sbsql = new System.Text.StringBuilder();
            sbsql.Append(Sql_EstoqueMovimentacao);
            sbsql.Append(string.Format(" AND ID_UNIDADE = {0}", idUnidade));
            sbsql.Append(" ORDER BY ID_ESTOQUE, DATA_MOVIMENTACAO");
            return db.Database.SqlQuery<ESTOQUE_MOVIMENTACAODTO>(sbsql.ToString()).ToList();
        }
    }
}
