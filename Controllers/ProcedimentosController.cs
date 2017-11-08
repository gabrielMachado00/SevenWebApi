using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;

namespace SevenMedicalApi.Controllers
{
    public class PROCEDIMENTODTO
    {
        public int ID_PROCEDIMENTO { get; set; }
        public string DESCRICAO { get; set; }
        public string COR { get; set; }
        public string CODIGO_CONVENIO { get; set; }
        public int ID_TIPO_SERVICO { get; set; }
        public string DESC_TIPO_SERVICO { get; set; }
        public bool MOVIMENTA_ESTOQUE { get; set; }
        public bool? CRP { get; set; }
    }

    public class ProcedimentosController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/procedimentos
        [Route("api/servicos")]
        public IEnumerable<PROCEDIMENTODTO> GetPROCEDIMENTOs()
        {
            return (from p in db.PROCEDIMENTOes
                    join ts in db.TIPO_SERVICO on p.ID_TIPO_SERVICO equals ts.ID_TIPO_SERVICO
                    select new PROCEDIMENTODTO()
                    {
                        ID_PROCEDIMENTO = p.ID_PROCEDIMENTO,
                        DESCRICAO = p.DESCRICAO,
                        COR = p.COR,
                        //CODIGO_CONVENIO = cp.CODIGO_CONVENIO,
                        ID_TIPO_SERVICO = p.ID_TIPO_SERVICO,
                        DESC_TIPO_SERVICO = ts.DESCRICAO,
                        MOVIMENTA_ESTOQUE = p.MOVIMENTA_ESTOQUE,
                        CRP = p.CRP
                    }).ToList().OrderBy(a => a.DESCRICAO);
        }

        // GET api/procedimento/1
        [Route("api/procedimento/{idProcedimento}")]
        public IEnumerable<PROCEDIMENTODTO> GetPROCEDIMENTO(int idProcedimento)
        {
            return (from p in db.PROCEDIMENTOes
                    join ts in db.TIPO_SERVICO on p.ID_TIPO_SERVICO equals ts.ID_TIPO_SERVICO
                    select new PROCEDIMENTODTO()
                    {
                        ID_PROCEDIMENTO = p.ID_PROCEDIMENTO,
                        DESCRICAO = p.DESCRICAO,
                        COR = p.COR,
                        //CODIGO_CONVENIO = p.CODIGO_CONVENIO,
                        ID_TIPO_SERVICO = p.ID_TIPO_SERVICO,
                        DESC_TIPO_SERVICO = ts.DESCRICAO,
                        MOVIMENTA_ESTOQUE = p.MOVIMENTA_ESTOQUE,
                        CRP = p.CRP
                    }).Where(p => p.ID_PROCEDIMENTO == idProcedimento).ToList().OrderBy(a=>a.DESCRICAO);
        }

        // PUT: api/Procedimentos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPROCEDIMENTO(int id, PROCEDIMENTO pROCEDIMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pROCEDIMENTO.ID_PROCEDIMENTO)
            {
                return BadRequest();
            }

            db.Entry(pROCEDIMENTO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PROCEDIMENTOExists(id))
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

        // POST: api/Procedimentos
        [ResponseType(typeof(PROCEDIMENTO))]
        public IHttpActionResult PostPROCEDIMENTO(PROCEDIMENTO pROCEDIMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_PROCEDIMENTO;").First();

            pROCEDIMENTO.ID_PROCEDIMENTO = NextValue;

            db.PROCEDIMENTOes.Add(pROCEDIMENTO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PROCEDIMENTOExists(pROCEDIMENTO.ID_PROCEDIMENTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROCEDIMENTO.ID_PROCEDIMENTO }, pROCEDIMENTO);
        }

        // DELETE: api/Procedimentos/5
        [ResponseType(typeof(PROCEDIMENTO))]
        public IHttpActionResult DeletePROCEDIMENTO(int id)
        {
            PROCEDIMENTO pROCEDIMENTO = db.PROCEDIMENTOes.Find(id);
            if (pROCEDIMENTO == null)
            {
                return NotFound();
            }

            db.PROCEDIMENTOes.Remove(pROCEDIMENTO);
            db.SaveChanges();

            return Ok(pROCEDIMENTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PROCEDIMENTOExists(int id)
        {
            return db.PROCEDIMENTOes.Count(e => e.ID_PROCEDIMENTO == id) > 0;
        }
    }
}