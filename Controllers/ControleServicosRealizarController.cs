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

namespace SevenMedicalApi.Controllers
{
    public class CONTROLE_SERVICO_REALIZARDTO
    {
        public int ID_CLIENTE { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public int ID_REGIAO_CORPO { get; set; }
        public int ID_PROFISSIONAL { get; set; }
        public int NUM_SESSOES { get; set; }
        public int SESSOES_REALIZADAS { get; set; }
        public int ID_VENDA { get; set; }
        public string CLIENTE { get; set; }
        public string PROCEDIMENTO { get; set; }
        public string REGIAO { get; set; }
        public string PROFISSIONAL { get; set; }
    }
    public class ControleServicosRealizarController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/Venda/1/Itens
        [Route("api/controleservicosrealizar/{idCliente}/{idProcedimento}/{idRegiaoCorpo}/{idProfissional}")]
        public IEnumerable<CONTROLE_SERVICO_REALIZARDTO> GetSERVICOS_REALIZAR(int idCliente, int idProcedimento, int idRegiaoCorpo, int idProfissional)
        {
            return (from cs in db.CONTROLE_SERVICO_REALIZAR
                    join pf in db.PROFISSIONALs on cs.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL into _pf
                    from pf in _pf.DefaultIfEmpty()
                    join rc in db.REGIAO_CORPO on cs.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    join pr in db.PROCEDIMENTOes on cs.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO into _pr
                    from pr in _pr.DefaultIfEmpty()
                    join c in db.CLIENTES on cs.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    select new CONTROLE_SERVICO_REALIZARDTO()
                    {
                        ID_CLIENTE = cs.ID_CLIENTE,
                        ID_PROCEDIMENTO = cs.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = cs.ID_REGIAO_CORPO,
                        ID_PROFISSIONAL = cs.ID_PROFISSIONAL,
                        NUM_SESSOES = cs.NUM_SESSOES,
                        SESSOES_REALIZADAS = cs.SESSOES_REALIZADAS,
                        //ID_VENDA = cs.ID_VENDA,
                        CLIENTE = c.NOME,
                        PROCEDIMENTO = pr.DESCRICAO,
                        REGIAO = rc.REGIAO,
                        PROFISSIONAL = pf.NOME
                    }).Where(cs => cs.ID_CLIENTE == idCliente && cs.ID_PROCEDIMENTO == idProcedimento && cs.ID_REGIAO_CORPO == idRegiaoCorpo && cs.ID_PROFISSIONAL == idProfissional && cs.SESSOES_REALIZADAS < cs.NUM_SESSOES).ToList();
        }

        // GET api/Venda/1/Itens
        [Route("api/controleservicosrealizar/{idCliente}")]
        public IEnumerable<CONTROLE_SERVICO_REALIZARDTO> GetSERVICOS_REALIZAR(int idCliente)
        {
            return (from cs in db.CONTROLE_SERVICO_REALIZAR
                    join pf in db.PROFISSIONALs on cs.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL into _pf
                    from pf in _pf.DefaultIfEmpty()
                    join rc in db.REGIAO_CORPO on cs.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    join pr in db.PROCEDIMENTOes on cs.ID_PROCEDIMENTO equals pr.ID_PROCEDIMENTO into _pr
                    from pr in _pr.DefaultIfEmpty()
                    join c in db.CLIENTES on cs.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    select new CONTROLE_SERVICO_REALIZARDTO()
                    {
                        ID_CLIENTE = cs.ID_CLIENTE,
                        ID_PROCEDIMENTO = cs.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = cs.ID_REGIAO_CORPO,
                        ID_PROFISSIONAL = cs.ID_PROFISSIONAL,
                        NUM_SESSOES = cs.NUM_SESSOES,
                        SESSOES_REALIZADAS = cs.SESSOES_REALIZADAS,
                        //ID_VENDA = cs.ID_VENDA,
                        CLIENTE = c.NOME,
                        PROCEDIMENTO = pr.DESCRICAO,
                        REGIAO = rc.REGIAO,
                        PROFISSIONAL = pf.NOME
                    }).Where(cs => cs.ID_CLIENTE == idCliente).ToList();
        }

        // PUT: api/ControleServicosRealizar/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCONTROLE_SERVICO_REALIZAR(int id, CONTROLE_SERVICO_REALIZAR cONTROLE_SERVICO_REALIZAR)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cONTROLE_SERVICO_REALIZAR.ID_CLIENTE)
            {
                return BadRequest();
            }

            db.Entry(cONTROLE_SERVICO_REALIZAR).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CONTROLE_SERVICO_REALIZARExists(id))
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

        // POST: api/ControleServicosRealizar
        [ResponseType(typeof(CONTROLE_SERVICO_REALIZAR))]
        public IHttpActionResult PostCONTROLE_SERVICO_REALIZAR(CONTROLE_SERVICO_REALIZAR cONTROLE_SERVICO_REALIZAR)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CONTROLE_SERVICO_REALIZAR.Add(cONTROLE_SERVICO_REALIZAR);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CONTROLE_SERVICO_REALIZARExists(cONTROLE_SERVICO_REALIZAR.ID_CLIENTE))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cONTROLE_SERVICO_REALIZAR.ID_CLIENTE }, cONTROLE_SERVICO_REALIZAR);
        }

        // DELETE: api/ControleServicosRealizar/5
        [ResponseType(typeof(CONTROLE_SERVICO_REALIZAR))]
        public IHttpActionResult DeleteCONTROLE_SERVICO_REALIZAR(int id)
        {
            CONTROLE_SERVICO_REALIZAR cONTROLE_SERVICO_REALIZAR = db.CONTROLE_SERVICO_REALIZAR.Find(id);
            if (cONTROLE_SERVICO_REALIZAR == null)
            {
                return NotFound();
            }

            db.CONTROLE_SERVICO_REALIZAR.Remove(cONTROLE_SERVICO_REALIZAR);
            db.SaveChanges();

            return Ok(cONTROLE_SERVICO_REALIZAR);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CONTROLE_SERVICO_REALIZARExists(int id)
        {
            return db.CONTROLE_SERVICO_REALIZAR.Count(e => e.ID_CLIENTE == id) > 0;
        }
    }
}