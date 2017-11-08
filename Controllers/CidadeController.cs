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
using System.Data.Entity.Core.Objects;

namespace SisMedApi.Controllers
{
    public class CidadeController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/Cidade
        public IEnumerable<CIDADE> GetCIDADEs()
        {
            return db.CIDADEs.AsEnumerable();
        }

        // GET api/Cidade/5
        public CIDADE GetCIDADE(int id)
        {
            CIDADE cidade = db.CIDADEs.Find(id);
            if (cidade == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return cidade;
        }

        // PUT api/Cidade/5
        public HttpResponseMessage PutCIDADE(int id, CIDADE cidade)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != cidade.ID_CIDADE)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(cidade).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Cidade
        public HttpResponseMessage PostCIDADE(CIDADE cidade)
        {
            if (ModelState.IsValid)
            {
                db.CIDADEs.Add(cidade);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, cidade);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = cidade.ID_CIDADE }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Cidade/5
        public HttpResponseMessage DeleteCIDADE(int id)
        {
            CIDADE cidade = db.CIDADEs.Find(id);
            if (cidade == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.CIDADEs.Remove(cidade);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, cidade);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}