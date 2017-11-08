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
    public class ATENDIMENTO_MEDIDADTO
    {
        public int ID_MEDIDA { get; set; }
        public int ID_ATENDIMENTO { get; set; }
        public string LOGIN { get; set; }
        public string VALOR { get; set; }
        public string DESCRICAO { get; set; }
        public string UNIDADE { get; set; }
        public Nullable<int> ID_CLIENTE { get; set; }
        public Nullable<DateTime> DATA_ATENDIMENTO { get; set; }
    }

    public class AtendimentoMedidaController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/Atendimento/1/Medidas
        [Route("api/Atendimento/{idAtendimento}/Medidas")]
        public IEnumerable<ATENDIMENTO_MEDIDADTO> GetATENDIMENTO_MEDIDAS(int idAtendimento)
        {
            return (from am in db.ATENDIMENTO_MEDIDA
                    join m in db.MEDIDAs on am.ID_MEDIDA equals m.ID_MEDIDA
                    select new ATENDIMENTO_MEDIDADTO()
                    {
                        ID_ATENDIMENTO = am.ID_ATENDIMENTO,
                        ID_MEDIDA = am.ID_MEDIDA,
                        LOGIN = am.LOGIN,
                        VALOR = am.VALOR,
                        DESCRICAO = m.DESCRICAO,
                        UNIDADE = m.UNIDADE
                    }).Where(am => am.ID_ATENDIMENTO == idAtendimento).ToList();
        }

        // GET api/Atendimento/1/Medida/1
        [Route("api/Atendimento/{idAtendimento}/Medida/{idMedida}")]
        public IEnumerable<ATENDIMENTO_MEDIDADTO> GetATENDIMENTO_MEDIDA(int idAtendimento, int idMedida)
        {
            return (from am in db.ATENDIMENTO_MEDIDA
                    join m in db.MEDIDAs on am.ID_MEDIDA equals m.ID_MEDIDA
                    select new ATENDIMENTO_MEDIDADTO()
                    {
                        ID_ATENDIMENTO = am.ID_ATENDIMENTO,
                        ID_MEDIDA = am.ID_MEDIDA,
                        LOGIN = am.LOGIN,
                        VALOR = am.VALOR,
                        DESCRICAO = m.DESCRICAO,
                        UNIDADE = m.UNIDADE
                    }).Where(am => am.ID_ATENDIMENTO == idAtendimento && am.ID_MEDIDA == idMedida).ToList();
        }

        // GET api/Atendimento/1/Medida/1
        [Route("api/Atendimento/Medida/{idCliente}")]
        public IEnumerable<ATENDIMENTO_MEDIDADTO> GetATENDIMENTO_MEDIDA_CLIENTE(int idCliente)
        {
            return (from am in db.ATENDIMENTO_MEDIDA
                    join m in db.MEDIDAs on am.ID_MEDIDA equals m.ID_MEDIDA
                    join a in db.ATENDIMENTOes on am.ID_ATENDIMENTO equals a.ID_ATENDIMENTO
                    select new ATENDIMENTO_MEDIDADTO()
                    {
                        ID_ATENDIMENTO = am.ID_ATENDIMENTO,
                        ID_MEDIDA = am.ID_MEDIDA,
                        LOGIN = am.LOGIN,
                        VALOR = am.VALOR,
                        DESCRICAO = m.DESCRICAO,
                        UNIDADE = m.UNIDADE,
                        ID_CLIENTE = a.ID_CLIENTE,
                        DATA_ATENDIMENTO = a.DATA_HORA_INI
                    }).Where(am => am.ID_CLIENTE == idCliente).ToList();
        }

        // PUT: api/AtendimentoMedida/PutATENDIMENTO_MEDIDA/1/1
        [ResponseType(typeof(void))]
        [Route("api/AtendimentoMedida/PutATENDIMENTO_MEDIDA/{idAtendimento}/{idMedida}")]
        public IHttpActionResult PutATENDIMENTO_MEDIDA(int idAtendimento, int idMedida, ATENDIMENTO_MEDIDA aTENDIMENTO_MEDIDA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idAtendimento != aTENDIMENTO_MEDIDA.ID_ATENDIMENTO)
            {
                return BadRequest();
            }

            if (idMedida != aTENDIMENTO_MEDIDA.ID_MEDIDA)
            {
                return BadRequest();
            }

            db.Entry(aTENDIMENTO_MEDIDA).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ATENDIMENTO_MEDIDAExists(idAtendimento, idMedida))
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

        
        // POST: api/AtendimentoMedida
        [ResponseType(typeof(ATENDIMENTO_MEDIDA))]
        public async Task<IHttpActionResult> PostATENDIMENTO_MEDIDA(ATENDIMENTO_MEDIDA aTENDIMENTO_MEDIDA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ATENDIMENTO_MEDIDA.Add(aTENDIMENTO_MEDIDA);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ATENDIMENTO_MEDIDAExists(aTENDIMENTO_MEDIDA.ID_ATENDIMENTO, aTENDIMENTO_MEDIDA.ID_MEDIDA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aTENDIMENTO_MEDIDA.ID_MEDIDA }, aTENDIMENTO_MEDIDA);
        }

        // DELETE: api/Atendimento/Receita/Delete/1/2
        [ResponseType(typeof(ATENDIMENTO_MEDIDA))]
        [Route("api/Atendimento/Medida/Delete/{idAtendimento}/{idMedida}")]
        public async Task<IHttpActionResult> DeleteATENDIMENTO_MEDIDA(int idAtendimento, int idMedida)
        {
            ATENDIMENTO_MEDIDA aTENDIMENTO_MEDIDA = await db.ATENDIMENTO_MEDIDA.Where(am => am.ID_ATENDIMENTO == idAtendimento && am.ID_MEDIDA == idMedida).FirstOrDefaultAsync();
            if (aTENDIMENTO_MEDIDA == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM ATENDIMENTO_MEDIDA WHERE ID_ATENDIMENTO = {0} AND ID_MEDIDA = {1}";

            if (db.Database.ExecuteSqlCommand(sql, idAtendimento, idMedida) > 0)
                return Ok(aTENDIMENTO_MEDIDA);
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

        private bool ATENDIMENTO_MEDIDAExists(int idAtendimento, int idMedida)
        {
            return db.ATENDIMENTO_MEDIDA.Count(e => e.ID_MEDIDA == idMedida && e.ID_ATENDIMENTO == idAtendimento) > 0;
        }
    }
}