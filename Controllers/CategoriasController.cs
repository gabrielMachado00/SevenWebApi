using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using SisMedApi.Models;

namespace SevenMedicalApi.Controllers
{
    public class CATEGORIADTO
    {
        public int ID_CATEGORIA { get; set; }
        public string DESCRICAO { get; set; }
        public string TIPO { get; set; }
        public string DESC_TIPO { get; set; }
        public string SUB_TIPO { get; set; }
        public string DESC_SUB_TIPO { get; set; }
    }

    public class CategoriasController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/subcategorias
        [Route("api/categorias")]
        public IEnumerable<CATEGORIADTO> GetCATEGORIAs()
        {
            return (from c in db.CATEGORIAs
                    select new CATEGORIADTO()
                    {
                        ID_CATEGORIA = c.ID_CATEGORIA,
                        DESCRICAO = c.DESCRICAO,
                        TIPO = c.TIPO,
                        DESC_TIPO = c.TIPO == "B" ? "SAÍDA" : 
                                    c.TIPO == "A" ? "ENTRADA" :
                                    c.TIPO == "C" ? "INVESTIMENTO" :
                                    c.TIPO == "D" ? "DISTRIBUIÇÃO DE LUCRO" : "",
                        SUB_TIPO = c.SUB_TIPO,
                        DESC_SUB_TIPO = c.SUB_TIPO == "A" ? "CUSTO DIRETO" :
                                        c.SUB_TIPO == "B" ? "CUSTOS E SERVIÇOS OPERACIONAIS " :
                                        c.SUB_TIPO == "C" ? "CUSTOS E SERVIÇOS NÃO OPERACIONAIS" :
                                        c.SUB_TIPO == "D" ? "DGA - DESPESAS GERAIS E ADMINISTRATIVAS" :
                                        c.SUB_TIPO == "E" ? "RECEITAS OPERACIONAIS" :
                                        c.SUB_TIPO == "F" ? "RECEITAS NÃO OPERACIONAIS" : ""
                    }).ToList().OrderBy(c=>c.TIPO).OrderBy(c=>c.SUB_TIPO);
        }

        // GET api/subcategoria/1
        [Route("api/categoria/{idCategoria}")]
        public IEnumerable<CATEGORIADTO> GetCATEGORIA(int idCategoria)
        {
            return (from c in db.CATEGORIAs
                    select new CATEGORIADTO()
                    {
                        ID_CATEGORIA = c.ID_CATEGORIA,
                        DESCRICAO = c.DESCRICAO,
                        TIPO = c.TIPO,
                        DESC_TIPO = c.TIPO == "B" ? "SAÍDA" :
                                    c.TIPO == "A" ? "ENTRADA" :
                                    c.TIPO == "C" ? "INVESTIMENTO" :
                                    c.TIPO == "D" ? "DISTRIBUIÇÃO DE LUCRO" : "",
                        SUB_TIPO = c.SUB_TIPO,
                        DESC_SUB_TIPO = c.SUB_TIPO == "A" ? "CUSTO DIRETO" :
                                        c.SUB_TIPO == "B" ? "CUSTOS E SERVIÇOS OPERACIONAIS " :
                                        c.SUB_TIPO == "C" ? "CUSTOS E SERVIÇOS NÃO OPERACIONAIS" :
                                        c.SUB_TIPO == "D" ? "DGA - DESPESAS GERAIS E ADMINISTRATIVAS" :
                                        c.SUB_TIPO == "E" ? "RECEITAS OPERACIONAIS" :
                                        c.SUB_TIPO == "F" ? "RECEITAS NÃO OPERACIONAIS" : ""
                    }).Where(c => c.ID_CATEGORIA == idCategoria).ToList().OrderBy(c => c.TIPO).OrderBy(c => c.SUB_TIPO);
        }

        [Route("api/categoria/receber")]
        public IEnumerable<CATEGORIADTO> GetCATEGORIA_RECEBER()
        {
            return (from c in db.CATEGORIAs
                    select new CATEGORIADTO()
                    {
                        ID_CATEGORIA = c.ID_CATEGORIA,
                        DESCRICAO = c.DESCRICAO,
                        TIPO = c.TIPO,
                        DESC_TIPO = c.TIPO == "B" ? "SAÍDA" :
                                    c.TIPO == "A" ? "ENTRADA" :
                                    c.TIPO == "C" ? "INVESTIMENTO" :
                                    c.TIPO == "D" ? "DISTRIBUIÇÃO DE LUCRO" : "",
                        SUB_TIPO = c.SUB_TIPO,
                        DESC_SUB_TIPO = c.SUB_TIPO == "A" ? "CUSTO DIRETO" :
                                        c.SUB_TIPO == "B" ? "CUSTOS E SERVIÇOS OPERACIONAIS " :
                                        c.SUB_TIPO == "C" ? "CUSTOS E SERVIÇOS NÃO OPERACIONAIS" :
                                        c.SUB_TIPO == "D" ? "DGA - DESPESAS GERAIS E ADMINISTRATIVAS" :
                                        c.SUB_TIPO == "E" ? "RECEITAS OPERACIONAIS" :
                                        c.SUB_TIPO == "F" ? "RECEITAS NÃO OPERACIONAIS" : ""
                    }).Where(c => c.TIPO == "A").ToList().OrderBy(c => c.TIPO).OrderBy(c => c.SUB_TIPO);
        }

        [Route("api/categoria/pagar")]
        public IEnumerable<CATEGORIADTO> GetCATEGORIA_PAGAR()
        {
            return (from c in db.CATEGORIAs
                    select new CATEGORIADTO()
                    {
                        ID_CATEGORIA = c.ID_CATEGORIA,
                        DESCRICAO = c.DESCRICAO,
                        TIPO = c.TIPO,
                        DESC_TIPO = c.TIPO == "B" ? "SAÍDA" :
                                    c.TIPO == "A" ? "ENTRADA" :
                                    c.TIPO == "C" ? "INVESTIMENTO" :
                                    c.TIPO == "D" ? "DISTRIBUIÇÃO DE LUCRO" : "",
                        SUB_TIPO = c.SUB_TIPO,
                        DESC_SUB_TIPO = c.SUB_TIPO == "A" ? "CUSTO DIRETO" :
                                        c.SUB_TIPO == "B" ? "CUSTOS E SERVIÇOS OPERACIONAIS " :
                                        c.SUB_TIPO == "C" ? "CUSTOS E SERVIÇOS NÃO OPERACIONAIS" :
                                        c.SUB_TIPO == "D" ? "DGA - DESPESAS GERAIS E ADMINISTRATIVAS" :
                                        c.SUB_TIPO == "E" ? "RECEITAS OPERACIONAIS" :
                                        c.SUB_TIPO == "F" ? "RECEITAS NÃO OPERACIONAIS" : ""
                    }).Where(c => c.TIPO == "B" || c.TIPO == "C" || c.TIPO == "D").ToList().OrderBy(c => c.TIPO).OrderBy(c => c.SUB_TIPO);
        }

        // PUT: api/Categorias/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCATEGORIA(int id, CATEGORIA cATEGORIA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cATEGORIA.ID_CATEGORIA)
            {
                return BadRequest();
            }

            db.Entry(cATEGORIA).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CATEGORIAExists(id))
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

        // POST: api/Categorias
        [ResponseType(typeof(CATEGORIA))]
        public IHttpActionResult PostCATEGORIA(CATEGORIA cATEGORIA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_CATEGORIA;").First();

            cATEGORIA.ID_CATEGORIA = NextValue;

            db.CATEGORIAs.Add(cATEGORIA);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CATEGORIAExists(cATEGORIA.ID_CATEGORIA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cATEGORIA.ID_CATEGORIA }, cATEGORIA);
        }

        // DELETE: api/Categorias/5
        [ResponseType(typeof(CATEGORIA))]
        public IHttpActionResult DeleteCATEGORIA(int id)
        {
            CATEGORIA cATEGORIA = db.CATEGORIAs.Find(id);
            if (cATEGORIA == null)
            {
                return NotFound();
            }

            db.CATEGORIAs.Remove(cATEGORIA);
            db.SaveChanges();

            return Ok(cATEGORIA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CATEGORIAExists(int id)
        {
            return db.CATEGORIAs.Count(e => e.ID_CATEGORIA == id) > 0;
        }
    }
}