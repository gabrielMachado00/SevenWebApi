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
    public class CRM_LIGACAODTO
    {
        public int ID_CRM_LIGACAO { get; set; }
        public int ID_APPOINTMENT { get; set; }
        public int ID_REGIAO_CORPO_ALERTA { get; set; }
        public DateTime DATA_LIGACAO { get; set; }
        public DateTime DATA_ATUALIZACAO { get; set; }
        public int STATUS_LIGACAO { get; set; }
        public string OBSERVACAO { get; set; }
        public string LOGIN { get; set; }
        public string CLIENTE { get; set; }
        public string PROCEDIMENTO { get; set; }
        public string REGIAO_CORPO { get; set; }
        public string TELEFONE { get; set; }
        public string CELULAR { get; set; }
        public string SITUACAO { get; set; }
        public string DESC_ALERTA { get; set; }
        public String PROFISSIONAL { get; set; }
    }

    public class TOTAL_ALERTA
    {
        public int Tipo { get; set; } //0 = CLINICO  1 = ANIVERSARIANTES
        public int TotalAlerta { get; set; }
    }

    public class CrmController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        string sql = @"SELECT LIG.ID_CRM_LIGACAO, LIG.ID_APPOINTMENT, LIG.ID_REGIAO_CORPO_ALERTA, LIG.DATA_LIGACAO, LIG.DATA_ATUALIZACAO,
                                   LIG.STATUS_LIGACAO, LIG.OBSERVACAO, LIG.LOGIN, C.NOME CLIENTE, P.DESCRICAO AS PROCEDIMENTO,
	                               RC.REGIAO AS REGIAO_CORPO, C.TELEFONE, C.CELULAR, 
	                               CASE WHEN LIG.STATUS_LIGACAO = 0 THEN  'Ligar' 
			                            WHEN LIG.STATUS_LIGACAO = 1 THEN 'Ligação Realizada' 
			                            WHEN LIG.STATUS_LIGACAO = 2 THEN 'Não Atendidida'
                                        WHEN LIG.STATUS_LIGACAO = 3 THEN 'Cancelar Ligação' END AS SITUACAO,
	                               RCA.DESCRICAO AS DESC_ALERTA, PR.NOME PROFISSIONAL
                            FROM Appointments A, CLIENTE C, CRM_LIGACAO LIG, 
                                 REGIAO_CORPO_ALERTA RCA, PROCEDIMENTO P, REGIAO_CORPO RC,
                                 PROFISSIONAL PR
                            WHERE PR.ID_PROFISSIONAL = A.ResourceID 
                            AND C.ID_CLIENTE = A.ID_CLIENTE  
                            AND RC.ID_REGIAO_CORPO = RCA.ID_REGIAO_CORPO
                            AND P.ID_PROCEDIMENTO = RCA.ID_PROCEDIMENTO   
                            AND RCA.ID_REGIAO_CORPO_ALERTA = LIG.ID_REGIAO_CORPO_ALERTA   
                            AND A.UniqueID = LIG.ID_APPOINTMENT  ";

        [Route("api/crm/TotalAlertas/")]
        public IEnumerable<TOTAL_ALERTA> GetTotalAlertas()
        {
            string sql = @" SELECT TIPO, SUM(TOT) TotalAlerta
                              FROM 
                             (SELECT 0 TIPO, COUNT(*) TOT FROM CRM_LIGACAO
                               WHERE CAST(DATA_LIGACAO AS DATE) = CAST(GETDATE() AS DATE)
                                 AND STATUS_LIGACAO <> 1
                              UNION ALL
                              SELECT 1 TIPO, COUNT(*) FROM CLIENTE C
                               WHERE DATEPART ( DAY , DATA_NASCIMENTO) = DATEPART ( DAY , GETDATE())
							     AND DATEPART ( MONTH , DATA_NASCIMENTO) = DATEPART ( MONTH , GETDATE())) A
                           GROUP BY TIPO";
            return db.Database.SqlQuery<TOTAL_ALERTA>(sql).ToList();
        }

        //// GET api/crm/
        //[Route("api/crm/")]
        //public IEnumerable<CRM_LIGACAODTO> GetCRM_LIGACACAO()
        //{            
        //    return db.Database.SqlQuery<CRM_LIGACAODTO>(sql).ToList();
        //}

        // GET api/crm/
        [Route("api/crm/id/{idCrmLigacao}")]
        public IEnumerable<CRM_LIGACAODTO> GetCRM_LIGACAO(int idCrmLigacao)
        {
            sql += " AND LIG.ID_CRM_LIGACAO = @ID_CRM_LIGACAO";
            return db.Database.SqlQuery<CRM_LIGACAODTO>(sql, new SqlParameter("@ID_CRM_LIGACAO", idCrmLigacao)).ToList();
        }

        // GET api/crm/
        [Route("api/crm/{dtInicial}/{dtFinal}/{status}")]
        public IEnumerable<CRM_LIGACAODTO> GetCRM_LIGACAO(DateTime dtInicial, DateTime dtFinal, string status)
        {            
            if (status != string.Empty)
                sql += " AND LIG.STATUS_LIGACAO IN(" + status + ")";

            sql += " AND CAST(LIG.DATA_LIGACAO AS DATE) BETWEEN CAST(@DATA_INICIAL AS DATE) AND CAST(@DATA_FINAL AS DATE)";
            return db.Database.SqlQuery<CRM_LIGACAODTO>(sql, new SqlParameter("@DATA_INICIAL", dtInicial),
                                                             new SqlParameter("@DATA_FINAL", dtFinal)).ToList();
        }

        // PUT: api/Crm/5
        //[Route("api/crm/PutCRM_LIGACAO/{idCrmLigacao}/{statusLigacao}/{obs}/{login}")]
        [ResponseType(typeof(void))]        
        [Route("api/crm/PutCRM_LIGACAO/{id}")]
        public IHttpActionResult PutCRM_LIGACAO(int id, CRM_LIGACAO cRM_LIGACAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Entry(cRM_LIGACAO).State = EntityState.Modified;
            try
            {
                //  db.SaveChanges();
                var sql = @"UPDATE CRM_LIGACAO 
                               SET STATUS_LIGACAO = {0}, 
                                   OBSERVACAO = {1},
                                   LOGIN = {2},
                                   DATA_ATUALIZACAO = GETDATE() 
                             WHERE ID_CRM_LIGACAO = {3} ";

                if (db.Database.ExecuteSqlCommand(sql, cRM_LIGACAO.STATUS_LIGACAO.ToString(), cRM_LIGACAO.OBSERVACAO.ToString(), cRM_LIGACAO.LOGIN.ToString(), id.ToString()) > 0)
                    return Ok(cRM_LIGACAO);
                else
                    return NotFound();
            }
            catch (DbUpdateException)
            {
                if (!CRM_LIGACAOExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
        }
        //[ResponseType(typeof(void))]
        //public IHttpActionResult (int id, CRM_LIGACAO cRM_LIGACAO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != cRM_LIGACAO.ID_CRM_LIGACAO)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(cRM_LIGACAO).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CRM_LIGACAOExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        private bool CRM_LIGACAOExists(int id)
        {
            return db.CRM_LIGACAO.Count(e => e.ID_CRM_LIGACAO == id) > 0;
        }
    }
}
