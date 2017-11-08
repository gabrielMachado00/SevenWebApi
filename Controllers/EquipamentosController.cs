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
using SisMedApi.Models;

namespace SevenMedicalApi.Controllers
{
    public class EQUIPAMENTODTO
    {
        public int ID_EQUIPAMENTO { get; set; }
        public string DESCRICAO { get; set; }
        public string TIPO { get; set; }
        public string NUM_SERIE { get; set; }
        public Nullable<decimal> VALOR_COMPRA { get; set; }
        public Nullable<int> ANO { get; set; }
        public Nullable<int> ANO_COMPRA { get; set; }
        public string ANVISA { get; set; }
        public string ACESSORIOS_COMPONENTES { get; set; }
        public Nullable<int> QUANTIDADE { get; set; }
        public string DESC_TIPO { get; set; }
        public Nullable<int> TEMPO_DEPRECIACAO { get; set; }
    }
    public class EquipamentosController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [Route("api/equipamentos")]
        public IEnumerable<EQUIPAMENTODTO> GetEQUIPAMENTOs()
        {
            return (from e in db.EQUIPAMENTOes
                    select new EQUIPAMENTODTO()
                    {
                        ID_EQUIPAMENTO = e.ID_EQUIPAMENTO,
                        DESCRICAO = e.DESCRICAO,
                        TIPO = e.TIPO,
                        NUM_SERIE = e.NUM_SERIE,
                        VALOR_COMPRA = e.VALOR_COMPRA,
                        ANO = e.ANO,
                        ANO_COMPRA = e.ANO_COMPRA,
                        ANVISA = e.ANVISA,
                        ACESSORIOS_COMPONENTES = e.ACESSORIOS_COMPONENTES,
                        QUANTIDADE = e.QUANTIDADE,
                        DESC_TIPO = e.TIPO == "N" ? "NOVO" :
                                    e.TIPO == "U" ? "USADO" : "",
                        TEMPO_DEPRECIACAO = e.TEMPO_DEPRECIACAO

                    }).ToList();
        }

        [Route("api/equipamento/{idEquipamento}")]
        public IEnumerable<EQUIPAMENTODTO> GetEQUIPAMENTO(int idEquipamento)
        {
            return (from e in db.EQUIPAMENTOes
                    select new EQUIPAMENTODTO()
                    {
                        ID_EQUIPAMENTO = e.ID_EQUIPAMENTO,
                        DESCRICAO = e.DESCRICAO,
                        TIPO = e.TIPO,
                        NUM_SERIE = e.NUM_SERIE,
                        VALOR_COMPRA = e.VALOR_COMPRA,
                        ANO = e.ANO,
                        ANO_COMPRA = e.ANO_COMPRA,
                        ANVISA = e.ANVISA,
                        ACESSORIOS_COMPONENTES = e.ACESSORIOS_COMPONENTES,
                        QUANTIDADE = e.QUANTIDADE,
                        DESC_TIPO = e.TIPO == "N" ? "NOVO" :
                                    e.TIPO == "U" ? "USADO" : "",
                        TEMPO_DEPRECIACAO = e.TEMPO_DEPRECIACAO
                    }).Where(e => e.ID_EQUIPAMENTO == idEquipamento).ToList();
        }

        // PUT: api/Equipamentos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEQUIPAMENTO(int id, EQUIPAMENTO eQUIPAMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eQUIPAMENTO.ID_EQUIPAMENTO)
            {
                return BadRequest();
            }

            db.Entry(eQUIPAMENTO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EQUIPAMENTOExists(id))
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

        // POST: api/Equipamentos
        [ResponseType(typeof(EQUIPAMENTO))]
        public IHttpActionResult PostEQUIPAMENTO(EQUIPAMENTO eQUIPAMENTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_EQUIPAMENTO;").First();

            eQUIPAMENTO.ID_EQUIPAMENTO = NextValue;

            db.EQUIPAMENTOes.Add(eQUIPAMENTO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EQUIPAMENTOExists(eQUIPAMENTO.ID_EQUIPAMENTO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = eQUIPAMENTO.ID_EQUIPAMENTO }, eQUIPAMENTO);
        }

        // DELETE: api/Equipamentos/5
        [ResponseType(typeof(EQUIPAMENTO))]
        public IHttpActionResult DeleteEQUIPAMENTO(int id)
        {
            EQUIPAMENTO eQUIPAMENTO = db.EQUIPAMENTOes.Find(id);
            if (eQUIPAMENTO == null)
            {
                return NotFound();
            }

            db.EQUIPAMENTOes.Remove(eQUIPAMENTO);
            db.SaveChanges();

            return Ok(eQUIPAMENTO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EQUIPAMENTOExists(int id)
        {
            return db.EQUIPAMENTOes.Count(e => e.ID_EQUIPAMENTO == id) > 0;
        }
    }
}