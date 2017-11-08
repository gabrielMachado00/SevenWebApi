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
using Elmah;
using System.Data.SqlClient;

namespace SevenMedicalApi.Controllers
{
    public class VENDA_ITEMDTO
    {
        public int ID_ITEM_VENDA { get; set; }
        public int ID_VENDA { get; set; }
        public Nullable<int> ID_APPOINTMENT { get; set; }
        public int ID_PROCEDIMENTO { get; set; }
        public Nullable<int> ID_REGIAO_CORPO { get; set; }
        public Nullable<int> NUM_SESSOES { get; set; }
        public decimal VALOR { get; set; }
        public Nullable<DateTime> DATA_ATUALIZACAO { get; set; }
        public Nullable<int> ID_ATENDIMENTO { get; set; }
        public string PROCEDIMENTO { get; set; }
        public string PRODUTO { get; set; }
        public string PROFISSIONAL { get; set; }
        public string CONVENIO { get; set; }
        public string REGIAO { get; set; }
        public Nullable<int> ID_ATENDIMENTO_PROCEDIMENTO { get; set; }
        public Nullable<int> ID_PROFISSIONAL { get; set; }
        public bool PACOTE { get; set; }
        public Nullable<decimal> VALOR_PAGO { get; set; }
        public Nullable<int> NUM_PACOTES { get; set; }
        public string CLIENTE { get; set; }
        public Nullable<int> NUM_SESSOES_REALIZADAS { get; set; }
        public Nullable<decimal> VALOR_PAGO_CARTAO { get; set; }
    }
    public class VendaItensController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/Venda/1/Itens
        [Route("api/Vendas/{idVenda}/Itens")]
        public IEnumerable<VENDA_ITEMDTO> GetVENDA_ITENS(int idVenda)
        {   
            string sql = @"SELECT VI.ID_ITEM_VENDA, VI.ID_VENDA, VI.ID_APPOINTMENT, VI.ID_PROCEDIMENTO,
                                   VI.ID_REGIAO_CORPO, VI.NUM_SESSOES, VI.VALOR, VI.DATA_ATUALIZACAO, VI.ID_ATENDIMENTO, 
	                               P.DESCRICAO PROCEDIMENTO, PR.NOME PROFISSIONAL, CV.DESCRICAO CONVENIO, RC.REGIAO,
	                               VI.ID_ATENDIMENTO_PROCEDIMENTO, VI.ID_PROFISSIONAL, VI.PACOTE, VI.VALOR_PAGO,
	                               VI.NUM_PACOTES, C.NOME CLIENTE, VI.NUM_SESSOES_REALIZADAS,
	                               dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) VALOR_PAGO_CARTAO
                              FROM VENDA_ITEM VI
	                               LEFT JOIN ATENDIMENTO A ON A.ID_ATENDIMENTO = VI.ID_ATENDIMENTO
                                   LEFT JOIN CONVENIO CV ON CV.ID_CONVENIO = A.ID_CONVENIO,
	                               VENDA V, PROCEDIMENTO P, PROFISSIONAL PR, REGIAO_CORPO RC, CLIENTE C
                             WHERE C.ID_CLIENTE = V.ID_CLIENTE
                               AND RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
                               AND P.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO
                               AND PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
                               AND VI.ID_VENDA = V.ID_VENDA
                               AND V.ID_VENDA = @ID_VENDA";
            return db.Database.SqlQuery<VENDA_ITEMDTO>(sql, new SqlParameter("@ID_VENDA", idVenda)).ToList();
        }

        // GET api/Venda/1/Itens
        [Route("api/Vendas/{idVenda}/Item/{idItem}")]
        public IEnumerable<VENDA_ITEMDTO> GetVENDA_ITEM(int idVenda, int idItem)
        {
            string sql = @"SELECT VI.ID_ITEM_VENDA, VI.ID_VENDA, VI.ID_APPOINTMENT, VI.ID_PROCEDIMENTO,
                                   VI.ID_REGIAO_CORPO, VI.NUM_SESSOES, VI.VALOR, VI.DATA_ATUALIZACAO, VI.ID_ATENDIMENTO, 
	                               P.DESCRICAO PROCEDIMENTO, PR.NOME PROFISSIONAL, CV.DESCRICAO CONVENIO, RC.REGIAO,
	                               VI.ID_ATENDIMENTO_PROCEDIMENTO, VI.ID_PROFISSIONAL, VI.PACOTE, VI.VALOR_PAGO,
	                               VI.NUM_PACOTES, C.NOME CLIENTE, VI.NUM_SESSOES_REALIZADAS,
	                               dbo.GET_TAXA_CARTAO_POR_ITEM(VI.ID_VENDA, VI.ID_ITEM_VENDA) VALOR_PAGO_CARTAO
                              FROM VENDA_ITEM VI
	                               LEFT JOIN ATENDIMENTO A ON A.ID_ATENDIMENTO = VI.ID_ATENDIMENTO
                                   LEFT JOIN CONVENIO CV ON CV.ID_CONVENIO = A.ID_CONVENIO,
	                               VENDA V, PROCEDIMENTO P, PROFISSIONAL PR, REGIAO_CORPO RC, CLIENTE C
                             WHERE C.ID_CLIENTE = V.ID_CLIENTE
                               AND RC.ID_REGIAO_CORPO = VI.ID_REGIAO_CORPO
                               AND P.ID_PROCEDIMENTO = VI.ID_PROCEDIMENTO
                               AND PR.ID_PROFISSIONAL = VI.ID_PROFISSIONAL
                               AND VI.ID_VENDA = V.ID_VENDA
                               AND V.ID_VENDA = @ID_VENDA
                               AND VI.ID_ITEM_VENDA = @ID_ITEM_VENDA";
            return db.Database.SqlQuery<VENDA_ITEMDTO>(sql, new SqlParameter("@ID_VENDA", idVenda),
                                                            new SqlParameter("@ID_ITEM_VENDA", idItem)).ToList();
        }

        // GET api/Orcamento/Itens
        [Route("api/Orcamento/Itens")]
        public IEnumerable<VENDA_ITEMDTO> GetORCAMENTO()
        {
            return (from vi in db.VENDA_ITEM
                    join v in db.VENDAs on vi.ID_VENDA equals v.ID_VENDA into _v
                    from v in _v.Where(v => v.STATUS == "O")
                    join a in db.ATENDIMENTOes on vi.ID_ATENDIMENTO equals a.ID_ATENDIMENTO into _a
                    from a in _a.DefaultIfEmpty()
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join p in db.PROCEDIMENTOes on vi.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO into _p
                    from p in _p.DefaultIfEmpty()
                    join pf in db.PROFISSIONALs on vi.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL into _pf
                    from pf in _pf.DefaultIfEmpty()
                    join rc in db.REGIAO_CORPO on vi.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    join c in db.CLIENTES on v.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    select new VENDA_ITEMDTO()
                    {
                        ID_ITEM_VENDA = vi.ID_ITEM_VENDA,
                        ID_VENDA = vi.ID_VENDA,
                        ID_APPOINTMENT = vi.ID_APPOINTMENT,
                        ID_PROCEDIMENTO = vi.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = vi.ID_REGIAO_CORPO,
                        NUM_SESSOES = vi.NUM_SESSOES,
                        VALOR = vi.VALOR,
                        DATA_ATUALIZACAO = vi.DATA_ATUALIZACAO,
                        ID_ATENDIMENTO = vi.ID_ATENDIMENTO,
                        PROCEDIMENTO = p.DESCRICAO,
                        PROFISSIONAL = pf.NOME,
                        CONVENIO = cv.DESCRICAO,
                        REGIAO = rc.REGIAO,
                        ID_ATENDIMENTO_PROCEDIMENTO = vi.ID_ATENDIMENTO_PROCEDIMENTO,
                        ID_PROFISSIONAL = vi.ID_PROFISSIONAL,
                        PACOTE = vi.PACOTE,
                        VALOR_PAGO = vi.VALOR_PAGO,
                        NUM_PACOTES = vi.NUM_PACOTES,
                        CLIENTE = c.NOME,
                        NUM_SESSOES_REALIZADAS = vi.NUM_SESSOES_REALIZADAS
                    }).ToList();
        }

        // GET api/Orcamento/1/Itens
        [Route("api/Orcamento/{idVenda}/Itens")]
        public IEnumerable<VENDA_ITEMDTO> GetORCAMENTO_ITENS(int idVenda)
        {
            return (from vi in db.VENDA_ITEM
                    join v in db.VENDAs on vi.ID_VENDA equals v.ID_VENDA into _v
                    from v in _v.Where(v => v.STATUS == "O")
                    join a in db.ATENDIMENTOes on vi.ID_ATENDIMENTO equals a.ID_ATENDIMENTO into _a
                    from a in _a.DefaultIfEmpty()
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join p in db.PROCEDIMENTOes on vi.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO into _p
                    from p in _p.DefaultIfEmpty()
                    join pf in db.PROFISSIONALs on vi.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL into _pf
                    from pf in _pf.DefaultIfEmpty()
                    join rc in db.REGIAO_CORPO on vi.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    join c in db.CLIENTES on v.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    select new VENDA_ITEMDTO()
                    {
                        ID_ITEM_VENDA = vi.ID_ITEM_VENDA,
                        ID_VENDA = vi.ID_VENDA,
                        ID_APPOINTMENT = vi.ID_APPOINTMENT,
                        ID_PROCEDIMENTO = vi.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = vi.ID_REGIAO_CORPO,
                        NUM_SESSOES = vi.NUM_SESSOES,
                        VALOR = vi.VALOR,
                        DATA_ATUALIZACAO = vi.DATA_ATUALIZACAO,
                        ID_ATENDIMENTO = vi.ID_ATENDIMENTO,
                        PROCEDIMENTO = p.DESCRICAO,
                        PROFISSIONAL = pf.NOME,
                        CONVENIO = cv.DESCRICAO,
                        REGIAO = rc.REGIAO,
                        ID_ATENDIMENTO_PROCEDIMENTO = vi.ID_ATENDIMENTO_PROCEDIMENTO,
                        ID_PROFISSIONAL = vi.ID_PROFISSIONAL,
                        PACOTE = vi.PACOTE,
                        VALOR_PAGO = vi.VALOR_PAGO,
                        NUM_PACOTES = vi.NUM_PACOTES,
                        CLIENTE = c.NOME,
                        NUM_SESSOES_REALIZADAS = vi.NUM_SESSOES_REALIZADAS
                    }).Where(vi => vi.ID_VENDA == idVenda).ToList();
        }

        // GET api/Orcamento/1/Atendimento
        [Route("api/Orcamento/{idAtendimento}/Atendimento")]
        public IEnumerable<VENDA_ITEMDTO> GetOrcamento_Atendimento(int idAtendimento)
        {
            return (from vi in db.VENDA_ITEM
                    join v in db.VENDAs on vi.ID_VENDA equals v.ID_VENDA into _v
                    from v in _v.Where(v => v.STATUS == "O")
                    join a in db.ATENDIMENTOes on vi.ID_ATENDIMENTO equals a.ID_ATENDIMENTO into _a
                    from a in _a.DefaultIfEmpty()
                    join cv in db.CONVENIOs on a.ID_CONVENIO equals cv.ID_CONVENIO into _cv
                    from cv in _cv.DefaultIfEmpty()
                    join p in db.PROCEDIMENTOes on vi.ID_PROCEDIMENTO equals p.ID_PROCEDIMENTO into _p
                    from p in _p.DefaultIfEmpty()
                    join pf in db.PROFISSIONALs on vi.ID_PROFISSIONAL equals pf.ID_PROFISSIONAL into _pf
                    from pf in _pf.DefaultIfEmpty()
                    join rc in db.REGIAO_CORPO on vi.ID_REGIAO_CORPO equals rc.ID_REGIAO_CORPO into _rc
                    from rc in _rc.DefaultIfEmpty()
                    join c in db.CLIENTES on v.ID_CLIENTE equals c.ID_CLIENTE into _c
                    from c in _c.DefaultIfEmpty()
                    select new VENDA_ITEMDTO()
                    {
                        ID_ITEM_VENDA = vi.ID_ITEM_VENDA,
                        ID_VENDA = vi.ID_VENDA,
                        ID_APPOINTMENT = vi.ID_APPOINTMENT,
                        ID_PROCEDIMENTO = vi.ID_PROCEDIMENTO,
                        ID_REGIAO_CORPO = vi.ID_REGIAO_CORPO,
                        NUM_SESSOES = vi.NUM_SESSOES,
                        VALOR = vi.VALOR,
                        DATA_ATUALIZACAO = vi.DATA_ATUALIZACAO,
                        ID_ATENDIMENTO = vi.ID_ATENDIMENTO,
                        PROCEDIMENTO = p.DESCRICAO,
                        PROFISSIONAL = pf.NOME,
                        CONVENIO = cv.DESCRICAO,
                        REGIAO = rc.REGIAO,
                        ID_ATENDIMENTO_PROCEDIMENTO = vi.ID_ATENDIMENTO_PROCEDIMENTO,
                        ID_PROFISSIONAL = vi.ID_PROFISSIONAL,
                        PACOTE = vi.PACOTE,
                        VALOR_PAGO = vi.VALOR_PAGO,
                        NUM_PACOTES = vi.NUM_PACOTES,
                        CLIENTE = c.NOME,
                        NUM_SESSOES_REALIZADAS = vi.NUM_SESSOES_REALIZADAS
                    }).Where(vi => vi.ID_ATENDIMENTO == idAtendimento).ToList();
        }

        // PUT: api/VendaItens/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVENDA_ITEM(int id, VENDA_ITEM vENDA_ITEM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vENDA_ITEM.ID_ITEM_VENDA)
            {
                return BadRequest();
            }

            db.Entry(vENDA_ITEM).State = EntityState.Modified;

            try
            {
                //db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                ErrorLog.GetDefault(null).Log(new Error(e));
                if (!VENDA_ITEMExists(id))
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

        // POST: api/VendaItens
        [ResponseType(typeof(VENDA_ITEM))]
        public IHttpActionResult PostVENDA_ITEM(VENDA_ITEM vENDA_ITEM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ITEM_VENDA;").First();

            vENDA_ITEM.ID_ITEM_VENDA = NextValue;

            db.VENDA_ITEM.Add(vENDA_ITEM);

            try
            {
               // db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                ErrorLog.GetDefault(null).Log(new Error(e));
                if (VENDA_ITEMExists(vENDA_ITEM.ID_ITEM_VENDA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = vENDA_ITEM.ID_ITEM_VENDA }, vENDA_ITEM);
        }

        // DELETE: api/VendaItens/5
        [ResponseType(typeof(VENDA_ITEM))]
        public IHttpActionResult DeleteVENDA_ITEM(int id)
        {
            VENDA_ITEM vENDA_ITEM = db.VENDA_ITEM.Find(id);
            if (vENDA_ITEM == null)
            {
                return NotFound();
            }

            db.VENDA_ITEM.Remove(vENDA_ITEM);
            //db.BulkSaveChanges();
            db.SaveChanges();

            return Ok(vENDA_ITEM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VENDA_ITEMExists(int id)
        {
            return db.VENDA_ITEM.Count(e => e.ID_ITEM_VENDA == id) > 0;
        }
    }
}