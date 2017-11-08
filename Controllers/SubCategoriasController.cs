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

    public class SUB_CATEGORIADTO
    {
        public int ID_SUB_CATEGORIA { get; set; }
        public int ID_CATEGORIA { get; set; }
        public string DESCRICAO { get; set; }
        public string DESC_CATEGORIA { get; set; }
        public Nullable<int> ID_CENTRO_CUSTO { get; set; }
        public string CENTRO_CUSTO { get; set; }
        public Nullable<bool> COMPOR_TAXA_SALA { get; set; }
        public Nullable<bool> COMPOR_TAXA_EQUIPAMENTO { get; set; }

    }
    public class SubCategoriasController : ApiController
    {
        private SisMedContext db = new SisMedContext();

        // GET api/subcategorias
        [Route("api/subcategorias")]
        public IEnumerable<SUB_CATEGORIADTO> GetSUBCATEGORIAs()
        {
            return (from sc in db.SUB_CATEGORIA
                    join c in db.CATEGORIAs on sc.ID_CATEGORIA equals c.ID_CATEGORIA
                    join cc in db.CENTRO_CUSTO on sc.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    select new SUB_CATEGORIADTO()
                    {
                        ID_SUB_CATEGORIA = sc.ID_SUB_CATEGORIA,
                        ID_CATEGORIA = sc.ID_CATEGORIA,
                        DESCRICAO = sc.DESCRICAO,
                        DESC_CATEGORIA = c.DESCRICAO,
                        ID_CENTRO_CUSTO = sc.ID_CENTRO_CUSTO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        COMPOR_TAXA_SALA = sc.COMPOR_TAXA_SALA,
                        COMPOR_TAXA_EQUIPAMENTO = sc.COMPOR_TAXA_EQUIPAMENTO
                    }).ToList();
        }

        // GET api/subcategoria/1
        [Route("api/subcategoria/{idSubCategoria}")]
        public IEnumerable<SUB_CATEGORIADTO> GetSUBCATEGORIA(int idSubCategoria)
        {
            return (from sc in db.SUB_CATEGORIA
                    join c in db.CATEGORIAs on sc.ID_CATEGORIA equals c.ID_CATEGORIA
                    join cc in db.CENTRO_CUSTO on sc.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    select new SUB_CATEGORIADTO()
                    {
                        ID_SUB_CATEGORIA = sc.ID_SUB_CATEGORIA,
                        ID_CATEGORIA = sc.ID_CATEGORIA,
                        DESCRICAO = sc.DESCRICAO,
                        DESC_CATEGORIA = c.DESCRICAO,
                        ID_CENTRO_CUSTO = sc.ID_CENTRO_CUSTO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        COMPOR_TAXA_SALA = sc.COMPOR_TAXA_SALA,
                        COMPOR_TAXA_EQUIPAMENTO = sc.COMPOR_TAXA_EQUIPAMENTO
                    }).Where(sc => sc.ID_SUB_CATEGORIA == idSubCategoria).ToList();
        }

        // GET api/categoria/1/subcategorias
        [Route("api/categoria/{idCategoria}/subcategorias")]
        public IEnumerable<SUB_CATEGORIADTO> GetCATEGORIA_SUB_CATEGORIAS(int idCategoria)
        {
            return (from sc in db.SUB_CATEGORIA
                    join c in db.CATEGORIAs on sc.ID_CATEGORIA equals c.ID_CATEGORIA
                    join cc in db.CENTRO_CUSTO on sc.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    select new SUB_CATEGORIADTO()
                    {
                        ID_SUB_CATEGORIA = sc.ID_SUB_CATEGORIA,
                        ID_CATEGORIA = sc.ID_CATEGORIA,
                        DESCRICAO = sc.DESCRICAO,
                        DESC_CATEGORIA = c.DESCRICAO,
                        ID_CENTRO_CUSTO = sc.ID_CENTRO_CUSTO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        COMPOR_TAXA_SALA = sc.COMPOR_TAXA_SALA,
                        COMPOR_TAXA_EQUIPAMENTO = sc.COMPOR_TAXA_EQUIPAMENTO
                    }).Where(sc => sc.ID_CATEGORIA == idCategoria).ToList().OrderBy(c => c.DESCRICAO);
        }

        // GET api/categoria/1/subcategoria/1
        [Route("api/categoria/{idCategoria}/subcategoria/{idSubCategoria}")]
        public IEnumerable<SUB_CATEGORIADTO> GetCATEGORIA_SUB_CATEGORIA(int idCategoria, int idSubCategoria)
        {
            return (from sc in db.SUB_CATEGORIA
                    join c in db.CATEGORIAs on sc.ID_CATEGORIA equals c.ID_CATEGORIA
                    join cc in db.CENTRO_CUSTO on sc.ID_CENTRO_CUSTO equals cc.ID_CENTRO_CUSTO into _cc
                    from cc in _cc.DefaultIfEmpty()
                    select new SUB_CATEGORIADTO()
                    {
                        ID_SUB_CATEGORIA = sc.ID_SUB_CATEGORIA,
                        ID_CATEGORIA = sc.ID_CATEGORIA,
                        DESCRICAO = sc.DESCRICAO,
                        DESC_CATEGORIA = c.DESCRICAO,
                        ID_CENTRO_CUSTO = sc.ID_CENTRO_CUSTO,
                        CENTRO_CUSTO = cc.DESCRICAO,
                        COMPOR_TAXA_SALA = sc.COMPOR_TAXA_SALA,
                        COMPOR_TAXA_EQUIPAMENTO = sc.COMPOR_TAXA_EQUIPAMENTO
                    }).Where(sc => sc.ID_CATEGORIA == idCategoria && sc.ID_SUB_CATEGORIA == idSubCategoria).ToList();
        }


        // PUT: api/SubCategorias/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSUB_CATEGORIA(int id, SUB_CATEGORIA sUB_CATEGORIA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sUB_CATEGORIA.ID_SUB_CATEGORIA)
            {
                return BadRequest();
            }

            db.Entry(sUB_CATEGORIA).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SUB_CATEGORIAExists(id))
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

        // POST: api/SubCategorias
        [ResponseType(typeof(SUB_CATEGORIA))]
        public IHttpActionResult PostSUB_CATEGORIA(SUB_CATEGORIA sUB_CATEGORIA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int NextValue = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.SQ_ID_SUB_CATEGORIA;").First();

            sUB_CATEGORIA.ID_SUB_CATEGORIA = NextValue;

            db.SUB_CATEGORIA.Add(sUB_CATEGORIA);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SUB_CATEGORIAExists(sUB_CATEGORIA.ID_SUB_CATEGORIA))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = sUB_CATEGORIA.ID_SUB_CATEGORIA }, sUB_CATEGORIA);
        }

        // DELETE: api/SubCategorias/5
        [ResponseType(typeof(SUB_CATEGORIA))]
        public IHttpActionResult DeleteSUB_CATEGORIA(int id)
        {
            SUB_CATEGORIA sUB_CATEGORIA = db.SUB_CATEGORIA.Find(id);
            if (sUB_CATEGORIA == null)
            {
                return NotFound();
            }

            db.SUB_CATEGORIA.Remove(sUB_CATEGORIA);
            db.SaveChanges();

            return Ok(sUB_CATEGORIA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SUB_CATEGORIAExists(int id)
        {
            return db.SUB_CATEGORIA.Count(e => e.ID_SUB_CATEGORIA == id) > 0;
        }
    }
}