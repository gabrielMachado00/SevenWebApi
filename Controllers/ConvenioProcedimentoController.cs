using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;

namespace SisMedApi.Controllers
{
    public class CONVENIO_PROCEDIMENTODTO
    {
        public int ID_CONVENIO { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public System.Decimal VALOR { get; set; }
        public string CONVENIO { get; set; }
        public string PROCEDIMENTO { get; set; }
        public string REGIAO_CORPO { get; set; }
        public string CODIGO_CONVENIO { get; set; }
    }

    public class ConvenioProcedimentoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/ConvenioProcedimentos/1
        [Route("api/ConvenioProcedimentos/{idConvenio}")]
        public IEnumerable<CONVENIO_PROCEDIMENTODTO> GetCONVENIO_PROCEDIMENTOS(int idConvenio)
        {
            return (from cp in db.CONVENIO_PROCEDIMENTO
                    join c in db.CONVENIOs on cp.ID_CONVENIO equals c.ID_CONVENIO
                    join p in db.PROCEDIMENTOes on cp.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO 
                    join r in db.REGIAO_CORPO on cp.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    select new CONVENIO_PROCEDIMENTODTO()
                    {
                        ID_CONVENIO = cp.ID_CONVENIO,
                        ID_PROCEDIMENTO = cp.ID_PROCEDIMENTO,                        
                        VALOR = cp.VALOR,
                        CONVENIO = c.DESCRICAO,
                        PROCEDIMENTO = p.DESCRICAO,
                        REGIAO_CORPO = r.REGIAO,
                        ID_REGIAO_CORPO = cp.ID_REGIAO_CORPO,
                        CODIGO_CONVENIO = cp.CODIGO_CONVENIO
                    }).Where(cp => cp.ID_CONVENIO == idConvenio).ToList();
        }

        [Route("api/ConvenioProcedimentos/Regioes/{idConvProc}")]
        public IEnumerable<CONVENIO_PROCEDIMENTODTO> GetCONVENIO_REGIOES(int idConvProc)
        {
            return (from cp in db.CONVENIO_PROCEDIMENTO
                    join c in db.CONVENIOs on cp.ID_CONVENIO equals c.ID_CONVENIO
                    join p in db.PROCEDIMENTOes on cp.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    join r in db.REGIAO_CORPO on cp.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    select new CONVENIO_PROCEDIMENTODTO()
                    {
                        ID_CONVENIO = cp.ID_CONVENIO,
                        ID_PROCEDIMENTO = cp.ID_PROCEDIMENTO,
                        VALOR = cp.VALOR,
                        CONVENIO = c.DESCRICAO,
                        PROCEDIMENTO = p.DESCRICAO,
                        REGIAO_CORPO = r.REGIAO,
                        ID_REGIAO_CORPO = cp.ID_REGIAO_CORPO,
                        CODIGO_CONVENIO = cp.CODIGO_CONVENIO
                    }).Where(cp => cp.ID_CONVENIO == idConvProc).ToList();
        }

        // GET api/Convenio/1/Procedimento/1
        [Route("api/Convenio/{idConvenio}/Procedimento/{idProcedimento}/{idRegiaoCorpo}")]
        public IEnumerable<CONVENIO_PROCEDIMENTODTO> GetCONVENIO_PROCEDIMENTOS(int idConvenio, int idProcedimento, int idRegiaoCorpo)
        {
            return (from cp in db.CONVENIO_PROCEDIMENTO
                    join c in db.CONVENIOs on cp.ID_CONVENIO equals c.ID_CONVENIO
                    join p in db.PROCEDIMENTOes on cp.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO
                    join r in db.REGIAO_CORPO on cp.ID_REGIAO_CORPO equals r.ID_REGIAO_CORPO
                    select new CONVENIO_PROCEDIMENTODTO()
                    {
                        ID_CONVENIO = cp.ID_CONVENIO,
                        ID_PROCEDIMENTO = cp.ID_PROCEDIMENTO,
                        VALOR = cp.VALOR,
                        CONVENIO = c.DESCRICAO,
                        PROCEDIMENTO = p.DESCRICAO,
                        REGIAO_CORPO = r.REGIAO,
                        ID_REGIAO_CORPO = cp.ID_REGIAO_CORPO,
                        CODIGO_CONVENIO = cp.CODIGO_CONVENIO
                    }).Where(cp => cp.ID_CONVENIO == idConvenio && 
                             cp.ID_PROCEDIMENTO == idProcedimento &&
                             cp.ID_REGIAO_CORPO == idRegiaoCorpo).ToList();
        }

        //PUT api/ConvenioProcedimentos/PutCONVENIO_PROCEDIMENTO/1/1
        [ResponseType(typeof(void))]
        [Route("api/ConvenioProcedimentos/PutCONVENIO_PROCEDIMENTO/{idConvenio}/{idProcedimento}/{idRegiaoCorpo}")]
        public IHttpActionResult PutCONVENIO_PROCEDIMENTO(int idConvenio, int idProcedimento, int idRegiaoCorpo, CONVENIO_PROCEDIMENTO cONVENIO_PROCEDIMENTO)
        {            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idConvenio != cONVENIO_PROCEDIMENTO.ID_CONVENIO || idProcedimento != cONVENIO_PROCEDIMENTO.ID_PROCEDIMENTO || idRegiaoCorpo != cONVENIO_PROCEDIMENTO.ID_REGIAO_CORPO)
            {
                return BadRequest();
            }

            //if (idProcedimento != cONVENIO_PROCEDIMENTO.ID_PROCEDIMENTO && id)
            //{
            //    return BadRequest();
            //}

            db.Entry(cONVENIO_PROCEDIMENTO).State = EntityState.Modified;

            try
            {
                //  db.SaveChanges();
                var sql = @"UPDATE CONVENIO_PROCEDIMENTO 
                               SET VALOR = {0}, 
                                   CODIGO_CONVENIO = {1}
                             WHERE ID_CONVENIO = {2}
                               AND ID_PROCEDIMENTO = {3} 
                               AND ID_REGIAO_CORPO = {4} ";

                if (db.Database.ExecuteSqlCommand(sql, cONVENIO_PROCEDIMENTO.VALOR, cONVENIO_PROCEDIMENTO.CODIGO_CONVENIO,
                                                       idConvenio, idProcedimento, idRegiaoCorpo) > 0)
                    return Ok(cONVENIO_PROCEDIMENTO);
                else
                    return NotFound();
            }
            catch (DbUpdateException)
            {                
                if (!CONVENIO_PROCEDIMENTOExists(idConvenio, idProcedimento, idRegiaoCorpo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }            
        }

        // POST: api/ConvenioProcedimento
        [ResponseType(typeof(CONVENIO_PROCEDIMENTO))]
        public async Task<IHttpActionResult> PostCONVENIO_PROCEDIMENTO(CONVENIO_PROCEDIMENTO cONVENIO_PROCEDIMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CONVENIO_PROCEDIMENTO.Add(cONVENIO_PROCEDIMENTO);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CONVENIO_PROCEDIMENTOExists(cONVENIO_PROCEDIMENTO.ID_CONVENIO, cONVENIO_PROCEDIMENTO.ID_PROCEDIMENTO, cONVENIO_PROCEDIMENTO.ID_REGIAO_CORPO))
                {
                    return Conflict();
                }
                else
                {                
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cONVENIO_PROCEDIMENTO.ID_CONVENIO }, cONVENIO_PROCEDIMENTO);
        }

        // DELETE: api/ConvenioProcedimento/5
        [ResponseType(typeof(CONVENIO_PROCEDIMENTO))]
        [Route("api/ConvenioProcedimento/DeleteCONVENIO_PROCEDIMENTO/{idConvenio}/{idProcedimento}/{idRegiaoCorpo}")]
        public IHttpActionResult DeleteCONVENIO_PROCEDIMENTO(int idConvenio, int idProcedimento, int idRegiaoCorpo)
        {
            CONVENIO_PROCEDIMENTO cONVENIO_PROCEDIMENTO = db.CONVENIO_PROCEDIMENTO.Where(pr => pr.ID_CONVENIO == idConvenio && pr.ID_PROCEDIMENTO == idProcedimento && pr.ID_REGIAO_CORPO == idRegiaoCorpo).FirstOrDefault();
            if (cONVENIO_PROCEDIMENTO == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM CONVENIO_PROCEDIMENTO WHERE ID_CONVENIO = {0} AND ID_PROCEDIMENTO = {1} AND ID_REGIAO_CORPO = {2}";

            if (db.Database.ExecuteSqlCommand(sql, idConvenio, idProcedimento, idRegiaoCorpo) > 0)
                return Ok(cONVENIO_PROCEDIMENTO);
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

        private bool CONVENIO_PROCEDIMENTOExists(int idConvenio, int idProcedimento, int idRegiaoCorpo)
        {
            return db.CONVENIO_PROCEDIMENTO.Count(e => e.ID_CONVENIO == idConvenio && e.ID_PROCEDIMENTO == idProcedimento && e.ID_REGIAO_CORPO == idRegiaoCorpo) > 0;
        }
    }
}