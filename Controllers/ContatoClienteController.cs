using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SisMedApi.Models;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace SisMedApi.Controllers
{

    public class ContatoClienteController : ApiController
    {
        private SisMedContext db = new SisMedContext();


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/cliente/1/contato
        [Route("api/cliente/{idCliente}/contato")]
        public IEnumerable<CLIENTE_CONTATO> GetCLIENTE_CONTATO(int idCliente)
        {
            return db.CLIENTE_CONTATO.Where(c => c.ID_CLIENTE == idCliente);
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]

        // GET api/cliente/contato/5
        [Route("api/cliente/contato/{id}")]
        public CLIENTE_CONTATO GetCONTATO(int id)
        {
            CLIENTE_CONTATO cliente_contato = db.CLIENTE_CONTATO.Find(id);
            if (cliente_contato == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return cliente_contato;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // PUT api/ContatoCliente/5
        public HttpResponseMessage PutCLIENTE_CONTATO(int id, CLIENTE_CONTATO cliente_contato)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != cliente_contato.ID_CONTATO)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(cliente_contato).State = System.Data.Entity.EntityState.Modified;

            try
            {
               // db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // POST api/ContatoCliente
        public HttpResponseMessage PostCLIENTE_CONTATO(CLIENTE_CONTATO cliente_contato)
        {
            if (ModelState.IsValid)
            {
                int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_CONTATO;").First();

                cliente_contato.ID_CONTATO = NextValue;

                db.CLIENTE_CONTATO.Add(cliente_contato);
               // db.BulkSaveChanges();
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, cliente_contato);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = cliente_contato.ID_CONTATO }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // DELETE: api/Profissional/Anamnese/Delete/1/2
        [ResponseType(typeof(CLIENTE_CONTATO))]
        [Route("api/Cliente/Contato/Delete/{idCliente}/{idContato}")]
        public async Task<IHttpActionResult> DeleteCONTATO_CLIENTE(int idCliente, int idContato)
        {
            CLIENTE_CONTATO cLIENTE_CONTATO = await db.CLIENTE_CONTATO.Where(cc => cc.ID_CLIENTE == idCliente && cc.ID_CONTATO == idContato).FirstOrDefaultAsync();
            if (cLIENTE_CONTATO == null)
            {
                return NotFound();
            }

            var sql = @"DELETE FROM CONTATOS_CLIENTES WHERE ID_CLIENTE = {0} AND ID_CONTATO = {1}";

            if (db.Database.ExecuteSqlCommand(sql, idCliente, idContato) > 0)
                return Ok(cLIENTE_CONTATO);
            else
                return NotFound();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}