using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SevenMedicalApi.Models;
using SisMedApi.Models;
using System;
using System.Collections.Generic;

namespace SevenMedicalApi.Controllers
{
    public class PROFISSIONAL_UNIDADEDTO
    {
        public int ID_PROFISSIONAL { get; set; }
        public int ID_UNIDADE { get; set; }
        public string PROFISSIONAL { get; set; }
        public string UNIDADE { get; set; }
        public string TITULO { get; set; }
        public Nullable<bool> STATUS { get; set; }
        public Nullable<bool> EXIBE_AGENDA { get; set; }
    }
    public class ProfissionalUnidadeController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/profissional/1/unidade
        [Route("api/profissional/{idProfissional}/unidade")]
        public IEnumerable<PROFISSIONAL_UNIDADEDTO> GetPROFISSIONAL_UNIDADE(int idProfissional)
        {
            return (from pu in db.PROFISSIONAL_UNIDADE
                    join p in db.PROFISSIONALs on pu.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    join u in db.UNIDADEs on pu.ID_UNIDADE equals u.ID_UNIDADE
                    select new PROFISSIONAL_UNIDADEDTO()
                    {
                        ID_PROFISSIONAL = pu.ID_PROFISSIONAL,
                        ID_UNIDADE = pu.ID_UNIDADE,
                        PROFISSIONAL = p.NOME,
                        UNIDADE = u.NOME_FANTASIA,
                        TITULO = p.TITULO,
                        STATUS = p.STATUS,
                        EXIBE_AGENDA = p.EXIBE_AGENDA
                    }).Where(pu => pu.ID_PROFISSIONAL == idProfissional).ToList();
        }

        // GET api/profissional/1/unidade
        [Route("api/unidade/{idUnidade}/profissional")]
        public IEnumerable<PROFISSIONAL_UNIDADEDTO> GetPROFISSIONAIS_UNIDADE(int idUnidade)
        {
            return (from pu in db.PROFISSIONAL_UNIDADE
                    join p in db.PROFISSIONALs on pu.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    join u in db.UNIDADEs on pu.ID_UNIDADE equals u.ID_UNIDADE
                    select new PROFISSIONAL_UNIDADEDTO()
                    {
                        ID_PROFISSIONAL = pu.ID_PROFISSIONAL,
                        ID_UNIDADE = pu.ID_UNIDADE,
                        PROFISSIONAL = p.NOME,
                        UNIDADE = u.NOME_FANTASIA,
                        TITULO = p.TITULO,
                        STATUS = p.STATUS,
                        EXIBE_AGENDA = p.EXIBE_AGENDA
                    }).Where(pu => pu.ID_UNIDADE == idUnidade).ToList();
        }

        // GET api/profissional/1/unidade
        [Route("api/unidade/{idUnidade}/profissional/ativo")]
        public IEnumerable<PROFISSIONAL_UNIDADEDTO> GetPROFISSIONAIS_UNIDADE_ATIVO(int idUnidade)
        {
            return (from pu in db.PROFISSIONAL_UNIDADE
                    join p in db.PROFISSIONALs on pu.ID_PROFISSIONAL equals p.ID_PROFISSIONAL
                    join u in db.UNIDADEs on pu.ID_UNIDADE equals u.ID_UNIDADE
                    select new PROFISSIONAL_UNIDADEDTO()
                    {
                        ID_PROFISSIONAL = pu.ID_PROFISSIONAL,
                        ID_UNIDADE = pu.ID_UNIDADE,
                        PROFISSIONAL = p.NOME,
                        UNIDADE = u.NOME_FANTASIA,
                        TITULO = p.TITULO,
                        STATUS = p.STATUS,
                        EXIBE_AGENDA = p.EXIBE_AGENDA
                    }).Where(pu => pu.ID_UNIDADE == idUnidade && pu.EXIBE_AGENDA == true && pu.STATUS == true).ToList();
        }

        // PUT: api/ProfissionalUnidade/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPROFISSIONAL_UNIDADE(int id, PROFISSIONAL_UNIDADE pROFISSIONAL_UNIDADE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pROFISSIONAL_UNIDADE.ID_PROFISSIONAL)
            {
                return BadRequest();
            }

            db.Entry(pROFISSIONAL_UNIDADE).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PROFISSIONAL_UNIDADEExists(id))
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

        // POST: api/ProfissionalUnidade
        [ResponseType(typeof(PROFISSIONAL_UNIDADE))]
        public IHttpActionResult PostPROFISSIONAL_UNIDADE(PROFISSIONAL_UNIDADE pROFISSIONAL_UNIDADE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PROFISSIONAL_UNIDADE.Add(pROFISSIONAL_UNIDADE);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PROFISSIONAL_UNIDADEExists(pROFISSIONAL_UNIDADE.ID_PROFISSIONAL))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROFISSIONAL_UNIDADE.ID_PROFISSIONAL }, pROFISSIONAL_UNIDADE);
        }

        // DELETE: api/profissional/1/unidade/2
        [ResponseType(typeof(PROFISSIONAL_UNIDADE))]
        [Route("api/deleteprofissional/{idProfissional}/unidade/{idUnidade}", Name = "DeleteUnidadeProfissional")]
        public IHttpActionResult DeletePROFISSIONAL_UNIDADE(int idProfissional, int idUnidade)
        {
            //    PROCEDIMENTO_EQUIPAMENTO pROCEDIMENTO_EQUIPAMENTO = db.PROCEDIMENTO_EQUIPAMENTO.Find(idProcedimento,idEquipamento);
            PROFISSIONAL_UNIDADE pROFISSIONAL_UNIDADE = db.PROFISSIONAL_UNIDADE.Where(pu => pu.ID_PROFISSIONAL == idProfissional && pu.ID_UNIDADE == idUnidade).FirstOrDefault();
            if (pROFISSIONAL_UNIDADE == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM PROFISSIONAL_UNIDADE WHERE ID_PROFISSIONAL = {0} AND ID_UNIDADE = {1}";

            if (db.Database.ExecuteSqlCommand(sql, idProfissional, idUnidade) > 0)
                return Ok(pROFISSIONAL_UNIDADE);
            else
                return NotFound();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PROFISSIONAL_UNIDADEExists(int id)
        {
            return db.PROFISSIONAL_UNIDADE.Count(e => e.ID_PROFISSIONAL == id) > 0;
        }
    }
}