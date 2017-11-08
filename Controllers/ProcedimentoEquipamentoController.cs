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

namespace SisMedApi.Controllers
{
    public class PROCEDIMENTO_EQUIPAMENTODTO
    {
        public int ID_PROCEDIMENTO { get; set; }
        public int ID_EQUIPAMENTO { get; set; }
        public Nullable<decimal> VALOR { get; set; }
        public string DESCRICAO { get; set; }
        public Nullable<int> QUANTIDADE { get; set; }
    }

    public class ProcedimentoEquipamentoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/ProcedimentoEquipamento/Equipamentos/1
        [Route("api/ProcedimentoEquipamento/Equipamentos/{idProcedimento}")]
        public IEnumerable<PROCEDIMENTO_EQUIPAMENTODTO> GetPROCEDIMENTO_EQUIPAMENTOS(int idProcedimento)
        {
            return (from pe in db.PROCEDIMENTO_EQUIPAMENTO
                    join e in db.EQUIPAMENTOes on pe.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO
                    select new PROCEDIMENTO_EQUIPAMENTODTO()
                    {
                        ID_PROCEDIMENTO = pe.ID_PROCEDIMENTO,
                        ID_EQUIPAMENTO = pe.ID_EQUIPAMENTO,
                        VALOR = pe.VALOR,
                        DESCRICAO = e.DESCRICAO,
                        QUANTIDADE = e.QUANTIDADE
                    }).Where(pe => pe.ID_PROCEDIMENTO == idProcedimento).ToList();
        }

        // GET api/ProcedimentoEquipamento/1/1
        [Route("api/ProcedimentoEquipamento/{idProcedimento}/{idEquipamento}")]
        public IEnumerable<PROCEDIMENTO_EQUIPAMENTODTO> GetPROCEDIMENTO_PRO_EQUIPAMENTO(int idProcedimento, int idEquipamento)
        {
            return (from pe in db.PROCEDIMENTO_EQUIPAMENTO
                    join e in db.EQUIPAMENTOes on pe.ID_EQUIPAMENTO equals e.ID_EQUIPAMENTO
                    select new PROCEDIMENTO_EQUIPAMENTODTO()
                    {
                        ID_PROCEDIMENTO = pe.ID_PROCEDIMENTO,
                        ID_EQUIPAMENTO = pe.ID_EQUIPAMENTO,
                        VALOR = pe.VALOR,
                        DESCRICAO = e.DESCRICAO,
                        QUANTIDADE = e.QUANTIDADE
                    }).Where(pe => pe.ID_PROCEDIMENTO == idProcedimento && pe.ID_EQUIPAMENTO == idEquipamento).ToList();
        }

        // PUT: api/ProcedimentoEquipamento/5/1
        [ResponseType(typeof(void))]
        [Route("api/ProcedimentoEquipamento/PutPROCEDIMENTO_EQUIPAMENTOS/{idProcedimento}/{idEquipamento}")]
        public IHttpActionResult PutPROCEDIMENTO_EQUIPAMENTO(int idProcedimento, int idEquipamento, PROCEDIMENTO_EQUIPAMENTO pROCEDIMENTO_EQUIPAMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idProcedimento != pROCEDIMENTO_EQUIPAMENTO.ID_PROCEDIMENTO)
            {
                return BadRequest();
            }

            if (idEquipamento != pROCEDIMENTO_EQUIPAMENTO.ID_EQUIPAMENTO)
            {
                return BadRequest();
            }

            db.Entry(pROCEDIMENTO_EQUIPAMENTO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PROCEDIMENTO_EQUIPAMENTOExists(idProcedimento, idEquipamento))
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

        
        // POST: api/ProcedimentoEquipamento
        [ResponseType(typeof(PROCEDIMENTO_EQUIPAMENTO))]
        public IHttpActionResult PostPROCEDIMENTO_EQUIPAMENTO(PROCEDIMENTO_EQUIPAMENTO pROCEDIMENTO_EQUIPAMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PROCEDIMENTO_EQUIPAMENTO.Add(pROCEDIMENTO_EQUIPAMENTO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PROCEDIMENTO_EQUIPAMENTOExists(pROCEDIMENTO_EQUIPAMENTO.ID_PROCEDIMENTO, pROCEDIMENTO_EQUIPAMENTO.ID_EQUIPAMENTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROCEDIMENTO_EQUIPAMENTO.ID_PROCEDIMENTO }, pROCEDIMENTO_EQUIPAMENTO);
        }

        // DELETE: api/ProcedimentoEquipamento/5
        [ResponseType(typeof(PROCEDIMENTO_EQUIPAMENTO))]
        [Route("api/ProcedimentoEquipamento/DeletePROCEDIMENTO_EQUIPAMENTOS/{idProcedimento}/{idEquipamento}")]
        public IHttpActionResult DeletePROCEDIMENTO_EQUIPAMENTO(int idProcedimento, int idEquipamento)
        {
            //    PROCEDIMENTO_EQUIPAMENTO pROCEDIMENTO_EQUIPAMENTO = db.PROCEDIMENTO_EQUIPAMENTO.Find(idProcedimento,idEquipamento);
            PROCEDIMENTO_EQUIPAMENTO pROCEDIMENTO_EQUIPAMENTO = db.PROCEDIMENTO_EQUIPAMENTO.Where(pe => pe.ID_PROCEDIMENTO == idProcedimento && pe.ID_EQUIPAMENTO == idEquipamento).FirstOrDefault();
            if (pROCEDIMENTO_EQUIPAMENTO == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM PROCEDIMENTO_EQUIPAMENTO WHERE ID_PROCEDIMENTO = {0} AND ID_EQUIPAMENTO = {1}";

            if (db.Database.ExecuteSqlCommand(sql, idProcedimento, idEquipamento) > 0)
                return Ok(pROCEDIMENTO_EQUIPAMENTO);
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

        private bool PROCEDIMENTO_EQUIPAMENTOExists(int idProcedimento, int idEquipamento)
        {
            return db.PROCEDIMENTO_EQUIPAMENTO.Count(e => e.ID_PROCEDIMENTO == idProcedimento && e.ID_EQUIPAMENTO == idEquipamento) > 0;
        }
    }
}