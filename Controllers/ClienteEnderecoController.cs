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
using System.Web.Http.Cors;

namespace SisMedApi.Controllers
{
    public class CLIENTE_ENDERECODTO
    {
        public int ID_ENDERECO { get; set; }
        public int ID_CLIENTE { get; set; }
        public string ENDERECO { get; set; }
        public string BAIRRO { get; set; }
        public string CEP { get; set; }
        public Nullable<int> ID_CIDADE { get; set; }
        public string TIPO_ENDERECO { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
    }

    public class ClienteEnderecoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        [EnableCors(origins:"*", headers: "*", methods: "*")]
        // GET api/cliente/1/endereco
        [Route("api/cliente/{idCliente}/endereco")]
        public IEnumerable<CLIENTE_ENDERECODTO> GetCLIENTE_ENDERECO(int idCliente)
        {
            return (from ce in db.CLIENTE_ENDERECO
                    join ci in db.CIDADEs on ce.ID_CIDADE equals ci.ID_CIDADE into _ci
                    from ci in _ci.DefaultIfEmpty()
                    select new CLIENTE_ENDERECODTO()
                    {
                        ID_ENDERECO = ce.ID_ENDERECO,
                        ID_CLIENTE = ce.ID_CLIENTE,
                        BAIRRO = ce.BAIRRO,
                        CEP = ce.CEP,
                        ENDERECO = ce.ENDERECO,
                        ID_CIDADE = ce.ID_CIDADE,
                        TIPO_ENDERECO = ce.TIPO_ENDERECO,
                        CIDADE = ci.NOME,
                        UF = ci.UF
                    }).Where(ce => ce.ID_CLIENTE == idCliente).ToList();
          
          //    return db.CLIENTE_ENDERECO.Where(c => c.ID_CLIENTE == idCliente);
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/cliente/endereco/5
        [Route("api/cliente/endereco/{id}")]
        public CLIENTE_ENDERECO GetENDERECO(int id)
        {
            CLIENTE_ENDERECO cliente_endereco = db.CLIENTE_ENDERECO.Find(id);
            if (cliente_endereco == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return cliente_endereco;
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // PUT api/ClienteEndereco/5
        public HttpResponseMessage PutCLIENTE_ENDERECO(int id, CLIENTE_ENDERECO cliente_endereco)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != cliente_endereco.ID_ENDERECO)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(cliente_endereco).State = System.Data.Entity.EntityState.Modified;

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
        // POST api/ClienteEndereco
        public HttpResponseMessage PostCLIENTE_ENDERECO(CLIENTE_ENDERECO cliente_endereco)
        {
            if (ModelState.IsValid)
            {
                int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_ENDERECO;").First();

                cliente_endereco.ID_ENDERECO = NextValue;

                db.CLIENTE_ENDERECO.Add(cliente_endereco);
               // db.BulkSaveChanges();
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, cliente_endereco);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = cliente_endereco.ID_ENDERECO }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]

        // DELETE api/ClienteEndereco/5
        public HttpResponseMessage DeleteCLIENTE_ENDERECO(int id)
        {
            CLIENTE_ENDERECO cliente_endereco = db.CLIENTE_ENDERECO.Find(id);
            if (cliente_endereco == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.CLIENTE_ENDERECO.Remove(cliente_endereco);

            try
            {
               // db.BulkSaveChanges();
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, cliente_endereco);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}