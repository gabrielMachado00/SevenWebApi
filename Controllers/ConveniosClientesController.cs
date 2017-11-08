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
using System.Threading.Tasks;

namespace SevenMedicalApi.Controllers
{
    public class CONVENIOS_CLIENTEDTO
    {
        public int ID_CONVENIO { get; set; }
        public int ID_CLIENTE { get; set; }
        public string NUMERO_CONVENIO { get; set; }
        public string ABRANGENCIA { get; set; }
        public Nullable<DateTime> VIGENCIA { get; set; }
        public DateTime VALIDADE { get; set; }
        public string ACOMODACAO { get; set; }
        public string PRODUTO { get; set; }
        public string CONTRATANTE { get; set; }
        public string NOME { get; set; }
        public string DESCRICAO { get; set; }
    }
    public class ConveniosClientesController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/ProdutoFornecedores/Convenios/1
        [Route("api/ConveniosCliente/Convenios/{idCliente}")]
        public IEnumerable<CONVENIOS_CLIENTEDTO> GetCLIENTE_CONVENIOS(int idCliente)
        {
            return ((from cc in db.CONVENIOS_CLIENTE
                     join c in db.CLIENTES on cc.ID_CLIENTE equals c.ID_CLIENTE
                     join co in db.CONVENIOs on cc.ID_CONVENIO equals co.ID_CONVENIO
                     select new CONVENIOS_CLIENTEDTO()
                     {
                         ID_CONVENIO = cc.ID_CONVENIO,
                         ID_CLIENTE = cc.ID_CLIENTE,
                         NUMERO_CONVENIO = cc.NUMERO_CONVENIO,
                         ABRANGENCIA = cc.ABRANGENCIA,
                         VIGENCIA = cc.VIGENCIA,
                         VALIDADE = cc.VALIDADE,
                         ACOMODACAO = cc.ACOMODACAO,
                         PRODUTO = cc.PRODUTO,
                         CONTRATANTE = cc.CONTRATANTE,
                         NOME = c.NOME,
                         DESCRICAO = co.DESCRICAO
                     }).Where(cc => cc.ID_CLIENTE == idCliente))
                    .Concat((from co in db.CONVENIOs
                             select new CONVENIOS_CLIENTEDTO()
                             {
                                 ID_CONVENIO = co.ID_CONVENIO,
                                 ID_CLIENTE = 0,
                                 NUMERO_CONVENIO = "",
                                 ABRANGENCIA = "",
                                 VIGENCIA = DateTime.Now,
                                 VALIDADE = DateTime.Now,
                                 ACOMODACAO = "",
                                 PRODUTO = "",
                                 CONTRATANTE = "",
                                 NOME = "",
                                 DESCRICAO = co.DESCRICAO
                             }).Where(co => co.ID_CONVENIO == 9999)).ToList();
        }


        // GET api/ConveniosCliente/Clientes/1
        [Route("api/ConveniosCliente/Clientes/{idConvenio}")]
        public IEnumerable<CONVENIOS_CLIENTEDTO> GetCONVENIO_CLIENTES(int idConvenio)
        {
            return (from cc in db.CONVENIOS_CLIENTE
                    join c in db.CLIENTES on cc.ID_CLIENTE equals c.ID_CLIENTE
                    join co in db.CONVENIOs on cc.ID_CONVENIO equals co.ID_CONVENIO
                    select new CONVENIOS_CLIENTEDTO()
                    {
                        ID_CONVENIO = cc.ID_CONVENIO,
                        ID_CLIENTE = cc.ID_CLIENTE,
                        NUMERO_CONVENIO = cc.NUMERO_CONVENIO,
                        ABRANGENCIA = cc.ABRANGENCIA,
                        VIGENCIA = cc.VIGENCIA,
                        VALIDADE = cc.VALIDADE,
                        ACOMODACAO = cc.ACOMODACAO,
                        PRODUTO = cc.PRODUTO,
                        CONTRATANTE = cc.CONTRATANTE,
                        NOME = c.NOME,
                        DESCRICAO = co.DESCRICAO
                    }).Where(cc => cc.ID_CONVENIO == idConvenio).ToList();
        }

        // GET api/ProdutoFornecedores/1/1
        [Route("api/ConveniosCliente/{idCliente}/{idConvenio}")]
        public IEnumerable<CONVENIOS_CLIENTEDTO> GetCONVENIO_CONV_CLIENTES(int idCliente, int idConvenio)
        {
            return (from cc in db.CONVENIOS_CLIENTE
                    join c in db.CLIENTES on cc.ID_CLIENTE equals c.ID_CLIENTE
                    join co in db.CONVENIOs on cc.ID_CONVENIO equals co.ID_CONVENIO
                    select new CONVENIOS_CLIENTEDTO()
                    {
                        ID_CONVENIO = cc.ID_CONVENIO,
                        ID_CLIENTE = cc.ID_CLIENTE,
                        NUMERO_CONVENIO = cc.NUMERO_CONVENIO,
                        ABRANGENCIA = cc.ABRANGENCIA,
                        VIGENCIA = cc.VIGENCIA,
                        VALIDADE = cc.VALIDADE,
                        ACOMODACAO = cc.ACOMODACAO,
                        PRODUTO = cc.PRODUTO,
                        CONTRATANTE = cc.CONTRATANTE,
                        NOME = c.NOME,
                        DESCRICAO = co.DESCRICAO
                    }).Where(cc => cc.ID_CONVENIO == idConvenio && cc.ID_CLIENTE == idCliente).ToList();
        }

        // PUT: api/ConveniosCliente/5
        [ResponseType(typeof(void))]
        [Route("api/ConveniosCliente/PutCLIENTE_CONVENIOS/{idCliente}/{idConvenio}")]
        public IHttpActionResult PutCLIENTE_CONVENIOS(int idCliente, int idConvenio, CONVENIOS_CLIENTE cONVENIOS_CLIENTE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idCliente != cONVENIOS_CLIENTE.ID_CLIENTE)
            {
                return BadRequest();
            }

            if (idConvenio != cONVENIOS_CLIENTE.ID_CONVENIO)
            {
                return BadRequest();
            }

            db.Entry(cONVENIOS_CLIENTE).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CONVENIOS_CLIENTEExists(idCliente, idConvenio))
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

        // POST: api/ConveniosClientes
        [ResponseType(typeof(CONVENIOS_CLIENTE))]
        public IHttpActionResult PostCONVENIOS_CLIENTE(CONVENIOS_CLIENTE cONVENIOS_CLIENTE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CONVENIOS_CLIENTE.Add(cONVENIOS_CLIENTE);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CONVENIOS_CLIENTEExists(cONVENIOS_CLIENTE.ID_CLIENTE, cONVENIOS_CLIENTE.ID_CONVENIO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cONVENIOS_CLIENTE.ID_CONVENIO }, cONVENIOS_CLIENTE);
        }

     /*   // DELETE: api/ConveniosClientes/5
        [ResponseType(typeof(CONVENIOS_CLIENTE))]
        public IHttpActionResult DeleteCONVENIOS_CLIENTE(int id)
        {
            CONVENIOS_CLIENTE cONVENIOS_CLIENTE = db.CONVENIOS_CLIENTE.Find(id);
            if (cONVENIOS_CLIENTE == null)
            {
                return NotFound();
            }

            db.CONVENIOS_CLIENTE.Remove(cONVENIOS_CLIENTE);
            db.SaveChanges();

            return Ok(cONVENIOS_CLIENTE);
        } */

        // DELETE: api/Profissional/Anamnese/Delete/1/2
        [ResponseType(typeof(CONVENIOS_CLIENTE))]
        [Route("api/Cliente/Convenio/Delete/{idCliente}/{idConvenio}")]
        public async Task<IHttpActionResult> DeleteCONVENIOS_CLIENTE(int idCliente, int idConvenio)
        {
            CONVENIOS_CLIENTE cONVENIOS_CLIENTE = await db.CONVENIOS_CLIENTE.Where(cc => cc.ID_CLIENTE == idCliente && cc.ID_CONVENIO == idConvenio).FirstOrDefaultAsync();
            if (cONVENIOS_CLIENTE == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM CONVENIOS_CLIENTE WHERE ID_CLIENTE = {0} AND ID_CONVENIO = {1}";

            if (db.Database.ExecuteSqlCommand(sql, idCliente, idConvenio) > 0)
                return Ok(cONVENIOS_CLIENTE);
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

        private bool CONVENIOS_CLIENTEExists(int idCliente, int idConvenio)
        {
            return db.CONVENIOS_CLIENTE.Count(e => e.ID_CONVENIO == idConvenio && e.ID_CLIENTE == idCliente) > 0;
        }
    }
}