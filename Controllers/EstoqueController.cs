using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SevenMedicalApi.Models;
using SisMedApi.Models;
using System.Data.SqlClient;
using System.Configuration;
using System;

namespace SevenMedicalApi.Controllers
{
    public class ESTOQUEDTO
    {
        public int ID_ESTOQUE { get; set; }
        public int ID_PRODUTO { get; set; }
        public decimal QUANTIDADE { get; set; }
        public int ID_UNIDADE { get; set; }
        public string PRODUTO { get; set; }
        public string UNIDADE { get; set; }
        public decimal ESTOQUE_MINIMO { get; set; }
    }

    public class ATUALIZA_ESTOQUE
    {
        public int ID_UNIDADE { get; set; }
        public int? ID_FARMACIA { get; set; }
        public int ID_PRODUTO { get; set; }
        public string TIPO { get; set; }
        public int ORIGEM { get; set; }
        public int? ID_USUARIO { get; set; }
        public bool ATUALIZA_SOMENTE_FARMACIA { get; set; }
        public string DESCRICAO { get; set; }
        public bool GERA_SOMENTE_MOVIMENTACAO { get; set; }
        public decimal QUANTIDADE { get; set; }
    }

    public class EstoqueController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [Route("api/estoque", Name = "GetEstoque")]
        public IEnumerable<ESTOQUEDTO> GetESTOQUEs()
        {
            return (from e in db.ESTOQUEs
                    join p in db.PRODUTOes on e.ID_PRODUTO equals p.ID_PRODUTO
                    join u in db.UNIDADEs on e.ID_UNIDADE equals u.ID_UNIDADE
                    select new ESTOQUEDTO()
                    {
                        ID_ESTOQUE = e.ID_ESTOQUE,
                        ID_PRODUTO = e.ID_PRODUTO,
                        QUANTIDADE = e.QUANTIDADE,
                        ID_UNIDADE = e.ID_UNIDADE,
                        PRODUTO = p.DESC_PRODUTO,
                        UNIDADE = u.NOME_FANTASIA,
                        ESTOQUE_MINIMO = p.QUANTIDADE_MINIMA
                    }).ToList();
        }

        [Route("api/estoque/{idEstoque}", Name = "GetEstoqueByID")]
        public IEnumerable<ESTOQUEDTO> GetESTOQUEs(int idEstoque)
        {
            return (from e in db.ESTOQUEs
                    join p in db.PRODUTOes on e.ID_PRODUTO equals p.ID_PRODUTO
                    join u in db.UNIDADEs on e.ID_UNIDADE equals u.ID_UNIDADE
                    select new ESTOQUEDTO()
                    {
                        ID_ESTOQUE = e.ID_ESTOQUE,
                        ID_PRODUTO = e.ID_PRODUTO,
                        QUANTIDADE = e.QUANTIDADE,
                        ID_UNIDADE = e.ID_UNIDADE,
                        PRODUTO = p.DESC_PRODUTO,
                        UNIDADE = u.NOME_FANTASIA,
                        ESTOQUE_MINIMO = p.QUANTIDADE_MINIMA
                    }).Where(e => e.ID_ESTOQUE == idEstoque).ToList();
        }

        [Route("api/estoque/unidade/{idUnidade}/produto/{idProduto}", Name = "GetEstoqueProduto")]
        public IEnumerable<ESTOQUEDTO> GetESTOQUEs_PRODUTO_UNIDADE(int idUnidade, int idProduto)
        {
            return (from e in db.ESTOQUEs
                    join p in db.PRODUTOes on e.ID_PRODUTO equals p.ID_PRODUTO
                    join u in db.UNIDADEs on e.ID_UNIDADE equals u.ID_UNIDADE
                    select new ESTOQUEDTO()
                    {
                        ID_ESTOQUE = e.ID_ESTOQUE,
                        ID_PRODUTO = e.ID_PRODUTO,
                        QUANTIDADE = e.QUANTIDADE,
                        ID_UNIDADE = e.ID_UNIDADE,
                        PRODUTO = p.DESC_PRODUTO,
                        UNIDADE = u.NOME_FANTASIA,
                        ESTOQUE_MINIMO = p.QUANTIDADE_MINIMA
                    }).Where(e => e.ID_PRODUTO == idProduto && e.ID_UNIDADE == idUnidade).ToList();
        }

        [Route("api/estoque/unidade/{idUnidade}", Name = "GetEstoqueUnidadeById")]
        public IEnumerable<ESTOQUEDTO> GetESTOQUEs_UNIDADE(int idUnidade)
        {
            return (from e in db.ESTOQUEs
                    join p in db.PRODUTOes on e.ID_PRODUTO equals p.ID_PRODUTO
                    join u in db.UNIDADEs on e.ID_UNIDADE equals u.ID_UNIDADE
                    select new ESTOQUEDTO()
                    {
                        ID_ESTOQUE = e.ID_ESTOQUE,
                        ID_PRODUTO = e.ID_PRODUTO,
                        QUANTIDADE = e.QUANTIDADE,
                        ID_UNIDADE = e.ID_UNIDADE,
                        PRODUTO = p.DESC_PRODUTO,
                        UNIDADE = u.NOME_FANTASIA,
                        ESTOQUE_MINIMO = p.QUANTIDADE_MINIMA
                    }).Where(e => e.ID_UNIDADE == idUnidade).ToList();
        }

        [Route("api/estoque/transferencia/{idUnidadeOrigem}/{idUnidadeDestino}/{idFarmaciaOrigem}/{idFarmaciaDestino}/{idProduto}/{quantidade}/{idUsuario}")]
        public bool GetTRANSFERE_ESTOQUE(int idUnidadeOrigem, int idUnidadeDestino, int idFarmaciaOrigem, int idFarmaciaDestino, int idProduto, decimal quantidade, int idUsuario)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SisMedContext"].ConnectionString))
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("[dbo].[TRANSFERE_ESTOQUE]", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PID_UNIDADE_ORIGEM", SqlDbType.Int)).Value = idUnidadeOrigem;
                        command.Parameters.Add(new SqlParameter("@PID_UNIDADE_DESTINO", SqlDbType.Int)).Value = idUnidadeDestino;
                        if(idFarmaciaOrigem > 0)
                            command.Parameters.Add(new SqlParameter("@PID_FARMACIA_ORIGEM", SqlDbType.Int)).Value = idFarmaciaOrigem;

                        if(idFarmaciaDestino > 0)
                            command.Parameters.Add(new SqlParameter("@PID_FARMACIA_DESTINO", SqlDbType.Int)).Value = idFarmaciaDestino;

                        command.Parameters.Add(new SqlParameter("@PID_PRODUTO", SqlDbType.Int)).Value = idProduto;
                        command.Parameters.Add(new SqlParameter("@PQUANTIDADE", SqlDbType.Decimal)).Value = quantidade;

                        if(idUsuario > 0)
                            command.Parameters.Add(new SqlParameter("@PID_USUARIO", SqlDbType.Int)).Value = idUsuario;

                        conn.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch { return false; }
        }

        [Route("api/estoque/transferencia/unidade/{idUnidade}/farmacia/{idFarmacia}/{idProduto}/{quantidade}")]
        public bool GetTRANSFERE_ESTOQUE_FARMACIA(int idUnidade, int idFarmacia, int idProduto, decimal quantidade)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SisMedContext"].ConnectionString))
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("[dbo].[TRANSFERE_ESTOQUE]", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PID_UNIDADE_ORIGEM", SqlDbType.Int)).Value = idUnidade;
                        command.Parameters.Add(new SqlParameter("@PID_FARMACIA_DESTINO", SqlDbType.Int)).Value = idFarmacia;
                        command.Parameters.Add(new SqlParameter("@PID_PRODUTO", SqlDbType.Int)).Value = idProduto;
                        command.Parameters.Add(new SqlParameter("@PQUANTIDADE", SqlDbType.Decimal)).Value = quantidade;
                        conn.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch { return false; }
        }

        [Route("api/estoque/transferencia/unidade/{idUnidadeOrigem}/unidade/{idUnidadeDestino}/{idProduto}/{quantidade}")]
        public bool GetTRANSFERE_ESTOQUE_UNIDADE(int idUnidadeOrigem, int idUnidadeDestino, int idProduto, decimal quantidade)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SisMedContext"].ConnectionString))
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("[dbo].[TRANSFERE_ESTOQUE]", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PID_UNIDADE_ORIGEM", SqlDbType.Int)).Value = idUnidadeOrigem;
                        command.Parameters.Add(new SqlParameter("@PID_UNIDADE_DESTINO", SqlDbType.Int)).Value = idUnidadeDestino;
                        command.Parameters.Add(new SqlParameter("@PID_PRODUTO", SqlDbType.Int)).Value = idProduto;
                        command.Parameters.Add(new SqlParameter("@PQUANTIDADE", SqlDbType.Decimal)).Value = quantidade;
                        conn.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch { return false; }
        }

        [Route("api/estoque/transferencia/farmacia/{idFarmaciaOrigem}/farmacia/{idFarmaciaDestino}/{idProduto}/{quantidade}")]
        public bool GetTRANSFERE_FARMACIA_FARMACIA(int idFarmaciaOrigem, int idFarmaciaDestino, int idProduto, decimal quantidade)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SisMedContext"].ConnectionString))
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("[dbo].[TRANSFERE_ESTOQUE]", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PID_FARMACIA_ORIGEM", SqlDbType.Int)).Value = idFarmaciaOrigem;
                        command.Parameters.Add(new SqlParameter("@PID_FARMACIA_DESTINO", SqlDbType.Int)).Value = idFarmaciaDestino;
                        command.Parameters.Add(new SqlParameter("@PID_PRODUTO", SqlDbType.Int)).Value = idProduto;
                        command.Parameters.Add(new SqlParameter("@PQUANTIDADE", SqlDbType.Decimal)).Value = quantidade;
                        conn.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch { return false; }
        }

        [Route("api/estoque/transferencia/farmacia/{idFarmaciaOrigem}/unidade/{idUnidade}/{idProduto}/{quantidade}")]
        public bool GetTRANSFERE_FARMACIA_UNIDADE(int idFarmaciaOrigem, int idUnidade, int idProduto, decimal quantidade)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SisMedContext"].ConnectionString))
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("[dbo].[TRANSFERE_ESTOQUE]", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@PID_FARMACIA_ORIGEM", SqlDbType.Int)).Value = idFarmaciaOrigem;
                        command.Parameters.Add(new SqlParameter("@PID_UNIDADE_DESTINO", SqlDbType.Int)).Value = idUnidade;
                        command.Parameters.Add(new SqlParameter("@PID_PRODUTO", SqlDbType.Int)).Value = idProduto;
                        command.Parameters.Add(new SqlParameter("@PQUANTIDADE", SqlDbType.Decimal)).Value = quantidade;
                        conn.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch { return false; }
        }

        //api/estoque/atualizaEstoque/1/0/41/E/3/-1/0/novo%20teste/1/20,0000
        [Route("api/estoque/atualizaEstoque/{idUnidade}/{idFarmacia}/{idProduto}/{tipo}/{origem}/{idUsuario}/{atualizaSomenteFarmacia}/{descricao}/{geraSomenteMovimentacao}/{quantidade}")]
        public bool GetAtualizaEstoque(int idUnidade, int? idFarmacia, int idProduto, string tipo, int origem, int? idUsuario, int atualizaSomenteFarmacia, string descricao, int geraSomenteMovimentacao, int quantidade)
        {
            try
            {
                int? usuario = null;
                int? farmacia = null;
                if (idUsuario > 0)
                    usuario = idUsuario;

                if (idFarmacia > 0)
                    farmacia = idFarmacia;
                //decimal qtd = Convert.ToDecimal(quantidade);

                AtualizaEstoque(idUnidade, farmacia, idProduto, quantidade, tipo, origem, usuario, (atualizaSomenteFarmacia == 1), descricao, (geraSomenteMovimentacao == 1));
                return true;
            }
            catch { return false; }
        }

        // PUT: api/Estoque/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutATUALIZA_ESTOQUE(int id, ATUALIZA_ESTOQUE atualizaEstoque)
        {
            try
            {
                int? usuario = null;
                int? farmacia = null;
                if (atualizaEstoque.ID_USUARIO.Value > 0)
                    usuario = atualizaEstoque.ID_USUARIO.Value;

                if (atualizaEstoque.ID_FARMACIA.Value > 0)
                    farmacia = atualizaEstoque.ID_FARMACIA.Value;
                //decimal qtd = Convert.ToDecimal(quantidade);

                AtualizaEstoque(atualizaEstoque.ID_UNIDADE, farmacia, atualizaEstoque.ID_PRODUTO, atualizaEstoque.QUANTIDADE, atualizaEstoque.TIPO, atualizaEstoque.ORIGEM, usuario, atualizaEstoque.ATUALIZA_SOMENTE_FARMACIA, atualizaEstoque.DESCRICAO, atualizaEstoque.GERA_SOMENTE_MOVIMENTACAO);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch {
                return StatusCode(HttpStatusCode.InternalServerError);
            }            
        }

        // PUT: api/Estoque/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutESTOQUE(int id, ESTOQUE eSTOQUE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eSTOQUE.ID_ESTOQUE)
            {
                return BadRequest();
            }

            db.Entry(eSTOQUE).State = EntityState.Modified;            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ESTOQUEExists(id))
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
                
        // POST: api/Estoque
        [ResponseType(typeof(ESTOQUE))]
        [Route("api/estoque", Name = "PostEstoque")]
        public IHttpActionResult PostESTOQUE(ESTOQUE eSTOQUE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ESTOQUE;").First();

            eSTOQUE.ID_ESTOQUE = NextValue;

            db.ESTOQUEs.Add(eSTOQUE);

            try
            {
                db.SaveChanges();
                AtualizaEstoque(eSTOQUE.ID_UNIDADE, null, eSTOQUE.ID_PRODUTO, eSTOQUE.QUANTIDADE, "E", 0, null, false, "Implantação de Estoque", true);
            }
            catch (DbUpdateException)
            {
                if (ESTOQUEExists(eSTOQUE.ID_ESTOQUE))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetEstoqueByID", new { idEstoque = eSTOQUE.ID_ESTOQUE }, eSTOQUE);
        }

        // DELETE: api/Estoque/5
        [ResponseType(typeof(ESTOQUE))]
        public IHttpActionResult DeleteESTOQUE(int id)
        {
            ESTOQUE eSTOQUE = db.ESTOQUEs.Find(id);
            if (eSTOQUE == null)
            {
                return NotFound();
            }

            db.ESTOQUEs.Remove(eSTOQUE);
            db.SaveChanges();

            return Ok(eSTOQUE);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ESTOQUEExists(int id)
        {
            return db.ESTOQUEs.Count(e => e.ID_ESTOQUE == id) > 0;
        }

        private void AtualizaEstoque(int idUnidade, int? idFarmacia, int idProduto, decimal quantidade, string tipo, int origem, int? idUsuario, bool atualizaSomenteFarmacia, string descricao, bool GeraSomenteMovimentacao)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SisMedContext"].ConnectionString))
            {
                try
                {
                    int SomenteFarmacia = atualizaSomenteFarmacia ? 1 : 0;
                    int SomenteMovimentacao = GeraSomenteMovimentacao ? 1 : 0;

                    SqlCommand command = new SqlCommand("[dbo].[ATUALIZA_ESTOQUE]", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@PID_UNIDADE", SqlDbType.Int)).Value = idUnidade;
                    if (idFarmacia.HasValue)
                        command.Parameters.Add(new SqlParameter("@PID_FARMACIA", SqlDbType.Int)).Value = idFarmacia.Value;

                    command.Parameters.Add(new SqlParameter("@PID_PRODUTO", SqlDbType.Int)).Value = idProduto;
                    command.Parameters.Add(new SqlParameter("@PQUANTIDADE", SqlDbType.Decimal)).Value = quantidade;
                    command.Parameters.Add(new SqlParameter("@PTIPO", SqlDbType.VarChar)).Value = tipo;
                    command.Parameters.Add(new SqlParameter("@PORIGEM", SqlDbType.Int)).Value = origem;
                    if (idUsuario.HasValue)
                        command.Parameters.Add(new SqlParameter("@PID_USUARIO", SqlDbType.Int)).Value = idUsuario.Value;

                    command.Parameters.Add(new SqlParameter("@PSOMENTE_FARMACIA", SqlDbType.Int)).Value = SomenteFarmacia;
                    command.Parameters.Add(new SqlParameter("@PDESCRICAO", SqlDbType.VarChar)).Value = descricao;
                    command.Parameters.Add(new SqlParameter("@PSOMENTE_GERA_MOVIMENTACAO", SqlDbType.Int)).Value = SomenteMovimentacao;
                    //@PSOMENTE_GERA_MOVIMENTACAO

                    conn.Open();
                    command.ExecuteNonQuery();

                }
                catch
                {
                    throw;
                }
            }
        }
    }
}