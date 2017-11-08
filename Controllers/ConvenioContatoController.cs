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

namespace SisMedApi.Controllers
{

    public class CONVENIO_CONTATODTO
    {
        public int ID_CONTATO_CONVENIO { get; set; }
        public int ID_CONVENIO { get; set; }
        public string NOME { get; set; }
        public string TELEFONE { get; set; }
        public string CELULAR { get; set; }
        public string EMAIL { get; set; }
        public string DESCRICAO { get; set; }
    }

    public class ConvenioContatoController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/ConvenioContatos/Contatos/1
        [Route("api/ConvenioContatos/Contatos/{idConvenio}")]
        public IEnumerable<CONVENIO_CONTATODTO> GetCONVENIO_CONTATOS(int idConvenio)
        {
            return (from cc in db.CONVENIO_CONTATO
                    join c in db.CONVENIOs on cc.ID_CONVENIO equals c.ID_CONVENIO
                    select new CONVENIO_CONTATODTO()
                    {
                        ID_CONTATO_CONVENIO = cc.ID_CONTATO_CONVENIO,
                        ID_CONVENIO = cc.ID_CONVENIO,
                        NOME = cc.NOME,
                        TELEFONE = cc.TELEFONE,
                        CELULAR = cc.CELULAR,
                        EMAIL = cc.EMAIL,
                        DESCRICAO = c.DESCRICAO
                    }).Where(cc => cc.ID_CONVENIO == idConvenio).ToList();
        }

        // GET api/ConvenioContatos/1/1
        [Route("api/ConvenioContatos/{idConvenio}/{idContato}")]
        public IEnumerable<CONVENIO_CONTATODTO> GetCONVENIO_CONV_CONTATO(int idConvenio, int idContato)
        {
            return (from cc in db.CONVENIO_CONTATO
                    join c in db.CONVENIOs on cc.ID_CONVENIO equals c.ID_CONVENIO
                    select new CONVENIO_CONTATODTO()
                    {
                        ID_CONTATO_CONVENIO = cc.ID_CONTATO_CONVENIO,
                        ID_CONVENIO = cc.ID_CONVENIO,
                        NOME = cc.NOME,
                        TELEFONE = cc.TELEFONE,
                        CELULAR = cc.CELULAR,
                        EMAIL = cc.EMAIL,
                        DESCRICAO = c.DESCRICAO
                    }).Where(cc => cc.ID_CONVENIO == idConvenio && cc.ID_CONTATO_CONVENIO == idContato).ToList();
        }

        //PUT api/ConvenioContatos/PutCONVENIO_CONTATO/1/1
        [ResponseType(typeof(void))]
        [Route("api/ConvenioContatos/PutCONVENIO_CONTATO/{idConvenio}/{idContato}")]
        public IHttpActionResult PutCONVENIO_CONTATO(int idConvenio, int idContato, CONVENIO_CONTATO cONVENIO_CONTATO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idConvenio != cONVENIO_CONTATO.ID_CONVENIO)
            {
                return BadRequest();
            }

            if (idContato != cONVENIO_CONTATO.ID_CONTATO_CONVENIO)
            {
                return BadRequest();
            }

            db.Entry(cONVENIO_CONTATO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CONVENIO_CONTATOExists(idConvenio, idContato))
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

        // POST: api/ConvenioContato
        [ResponseType(typeof(CONVENIO_CONTATO))]
        public IHttpActionResult PostCONVENIO_CONTATO(CONVENIO_CONTATO cONVENIO_CONTATO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_CONTATO_CONVENIO;").First();

            cONVENIO_CONTATO.ID_CONTATO_CONVENIO = NextValue;

            db.CONVENIO_CONTATO.Add(cONVENIO_CONTATO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CONVENIO_CONTATOExists(cONVENIO_CONTATO.ID_CONVENIO, cONVENIO_CONTATO.ID_CONTATO_CONVENIO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cONVENIO_CONTATO.ID_CONTATO_CONVENIO }, cONVENIO_CONTATO);
        }

        // DELETE: api/ConvenioContato/5
        [ResponseType(typeof(CONVENIO_CONTATO))]
        public IHttpActionResult DeleteCONVENIO_CONTATO(int id)
        {
            CONVENIO_CONTATO cONVENIO_CONTATO = db.CONVENIO_CONTATO.Find(id);
            if (cONVENIO_CONTATO == null)
            {
                return NotFound();
            }

            db.CONVENIO_CONTATO.Remove(cONVENIO_CONTATO);
            db.SaveChanges();

            return Ok(cONVENIO_CONTATO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CONVENIO_CONTATOExists(int idConvenio, int idContato)
        {
            return db.CONVENIO_CONTATO.Count(e => e.ID_CONTATO_CONVENIO == idContato && e.ID_CONVENIO == idConvenio ) > 0;
        }

    }
}