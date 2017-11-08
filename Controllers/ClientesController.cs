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
using System.Web.Http.Cors;

namespace SevenMedicalApi.Controllers
{

    public class CLIENTEDTO
    {
        public int ID_CLIENTE { get; set; }
        public string NOME { get; set; }
        public DateTime DATA_CADASTRO { get; set; }
        public string TELEFONE { get; set; }
        public string CELULAR { get; set; }
        public string OBS { get; set; }
        public string EMAIL { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string RG { get; set; }
        public Nullable<DateTime> DATA_NASCIMENTO { get; set; }
        public string INDICADO { get; set; }
        public string SEXO { get; set; }
        public string COR_PELE { get; set; }
        public string ESTADO_CIVIL { get; set; }
        public string NATURALIDADE { get; set; }
        public string PROFISSAO { get; set; }
        public string NOME_MAE { get; set; }
        public string NOME_PAI { get; set; }
        public bool BASICO { get; set; }
        public string CLASSIFICACAO { get; set; }
        public Nullable<int> ID_INDICACAO { get; set; }
        public string ALERTA_CLINICO { get; set; }
        public string INDICACAO_OUTRO { get; set; }
        public string DESC_SEXO { get; set; }
        public string DESC_BASICO { get; set; }
        public bool MENOR { get; set; }
        public string RESPONSAVEL { get; set; }
        public string CPF_RESPONSAVEL { get; set; }
        public string DIR_FOTO { get; set; }
        public string NOME_PREFERENCIA { get; set; }
        public string DESC_INDICACAO { get; set; }
    }

    public class ANIVERSARIANTES
    {
        public int ID_CLIENTE { get; set; }
        public string NOME { get; set; }        
        public Nullable<DateTime> DATA_ANIVERSARIO { get; set; }
        public string TELEFONE { get; set; }
        public string CELULAR { get; set; }
    }

    public class ClientesController : ApiController
    {
        private SisMedContext db = new SisMedContext();


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/clientes
        [Route("api/clientes")]
        public IEnumerable<CLIENTEDTO> GetCLIENTEs()
        {
            return (from c in db.CLIENTES
                    join i in db.INDICACAOs on c.ID_INDICACAO equals i.ID_INDICACAO into _i
                    from i in _i.DefaultIfEmpty()
                    select new CLIENTEDTO()
                    {
                        ID_CLIENTE = c.ID_CLIENTE,
                        NOME = c.NOME,
                        TELEFONE = c.TELEFONE,
                        CELULAR = c.CELULAR,
                        EMAIL = c.EMAIL,
                        CPF = c.CPF,
                        CNPJ = c.CNPJ,
                        RG = c.RG,
                        DATA_NASCIMENTO = c.DATA_NASCIMENTO,
                        SEXO = c.SEXO,
                        PROFISSAO = c.PROFISSAO,
                        NOME_MAE = c.NOME_MAE,
                        BASICO = c.BASICO,
                        DESC_SEXO = c.SEXO == "M" ? "MASCULINO" :
                                    c.SEXO == "F" ? "FEMININO" : "",
                        DESC_BASICO = c.BASICO == true ? "LEAD" :
                                      c.BASICO == false ? "CLIENTE" : "",
                        NOME_PREFERENCIA = c.NOME_PREFERENCIA,
                        INDICACAO_OUTRO = c.INDICACAO_OUTRO,
                        DESC_INDICACAO = i.DESCRICAO
                    }).ToList();
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]

        // GET api/clientes
        [Route("api/clientes/nomes/{nome}")]
        public IEnumerable<CLIENTEDTO> GetCLIENTEs_NOMES(string nome)
        {
            return (from c in db.CLIENTES
                    select new CLIENTEDTO()
                    {
                        ID_CLIENTE = c.ID_CLIENTE,
                        NOME = c.NOME
                    }).Where(c => c.NOME.Contains(nome)).ToList();
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/cliente/1
        [Route("api/cliente/{idCliente}")]
        public IEnumerable<CLIENTEDTO> GetCLIENTE(int idCliente)
        {
            return (from c in db.CLIENTES
                    join i in db.INDICACAOs on c.ID_INDICACAO equals i.ID_INDICACAO into _i
                    from i in _i.DefaultIfEmpty()
                    select new CLIENTEDTO()
                    {
                        ID_CLIENTE = c.ID_CLIENTE,
                        NOME = c.NOME,
                        DATA_CADASTRO = c.DATA_CADASTRO,
                        TELEFONE = c.TELEFONE,
                        CELULAR = c.CELULAR,
                        OBS = c.OBS,
                        EMAIL = c.EMAIL,
                        CPF = c.CPF,
                        CNPJ = c.CNPJ,
                        RG = c.RG,
                        DATA_NASCIMENTO = c.DATA_NASCIMENTO,
                        CLASSIFICACAO = c.CLASSIFICACAO,
                        INDICADO = c.INDICADO,
                        SEXO = c.SEXO,
                        ESTADO_CIVIL = c.ESTADO_CIVIL,
                        NATURALIDADE = c.NATURALIDADE,
                        PROFISSAO = c.PROFISSAO,
                        NOME_MAE = c.NOME_MAE,
                        NOME_PAI = c.NOME_PAI,
                        ID_INDICACAO = c.ID_INDICACAO,
                        ALERTA_CLINICO = c.ALERTA_CLINICO,
                        INDICACAO_OUTRO = c.INDICACAO_OUTRO,
                        DESC_SEXO = c.SEXO == "M" ? "MASCULINO" :
                                    c.SEXO == "F" ? "FEMININO" : "",
                        MENOR = c.MENOR,
                        RESPONSAVEL = c.RESPONSAVEL,
                        CPF_RESPONSAVEL = c.CPF_RESPONSAVEL,
                        DIR_FOTO = c.DIR_FOTO,
                        NOME_PREFERENCIA = c.NOME_PREFERENCIA,
                        BASICO = c.BASICO,
                        DESC_INDICACAO = i.DESCRICAO
                    }).Where(c => c.ID_CLIENTE == idCliente).ToList();
        }

        // GET api/cliente/Aniversario

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/cliente/aniversario/{dtInicial}/{dtFinal}")]
        public IEnumerable<ANIVERSARIANTES> GetAniversariantes(DateTime dtInicial, DateTime dtFinal)
        {
            string sql = @"SELECT ID_CLIENTE, NOME, DATA_NASCIMENTO,
							CAST( CAST(DATEPART ( YEAR , CAST(@DataFinal AS DATE)) as nvarchar)+'-'+
                             CAST(DATEPART ( MONTH , DATA_NASCIMENTO ) AS nvarchar)+'-'+	                         
							 CAST(DATEPART ( DAY , DATA_NASCIMENTO) AS nvarchar) AS date) AS DATA_ANIVERSARIO,
                            TELEFONE, CELULAR FROM CLIENTE 
                            WHERE DATEPART ( DAY , DATA_NASCIMENTO) between DATEPART ( DAY , CAST(@DataInicial AS DATE)) and DATEPART ( DAY , CAST(@DataFinal AS DATE)) 
                            AND DATEPART ( MONTH , DATA_NASCIMENTO) between DATEPART ( MONTH , CAST(@DataInicial AS DATE)) and DATEPART ( MONTH , CAST(@DataFinal AS DATE))  
							AND DATA_NASCIMENTO IS NOT NULL
                          ORDER BY CAST( CAST(DATEPART ( YEAR , CAST(@DataFinal AS DATE)) as nvarchar)+'-'+
                             CAST(DATEPART ( MONTH , DATA_NASCIMENTO ) AS nvarchar)+'-'+	                         
							 CAST(DATEPART ( DAY , DATA_NASCIMENTO) AS nvarchar) AS date), NOME";
            return db.Database.SqlQuery<ANIVERSARIANTES>(sql,
                                                                new SqlParameter("@DataInicial", dtInicial),
                                                                new SqlParameter("@DataFinal", dtFinal)).ToList();
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // PUT: api/Clientes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCLIENTE(int id, CLIENTE cLIENTE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cLIENTE.ID_CLIENTE)
            {
                return BadRequest();
            }

            db.Entry(cLIENTE).State = EntityState.Modified;

            try
            {
               // db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CLIENTEExists(id))
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


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // POST: api/Clientes
        [ResponseType(typeof(CLIENTE))]
        public IHttpActionResult PostCLIENTE(CLIENTE cLIENTE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_CLIENTE;").First();

            cLIENTE.ID_CLIENTE = NextValue;

            db.CLIENTES.Add(cLIENTE);

            try
            {
               // db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (CLIENTEExists(cLIENTE.ID_CLIENTE))
                {
                    return Conflict();
                }
                else
                {
                    if (ex.InnerException.InnerException.Message.Substring(74, 16) == "'UQ_CPF_CLIENTE'")
                        return Conflict();
                    else
                        throw new Exception(ex.InnerException.InnerException.Message);
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cLIENTE.ID_CLIENTE }, cLIENTE);
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // DELETE: api/Clientes/5
        [ResponseType(typeof(CLIENTE))]
        public IHttpActionResult DeleteCLIENTE(int id)
        {
            CLIENTE cLIENTE = db.CLIENTES.Find(id);
            if (cLIENTE == null)
            {
                return NotFound();
            }

            db.CLIENTES.Remove(cLIENTE);
            try
            {
               // db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch  (Exception e)
            {
                string s = e.Message;
            }

            return Ok(cLIENTE);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CLIENTEExists(int id)
        {
            return db.CLIENTES.Count(e => e.ID_CLIENTE == id) > 0;
        }
    }
}