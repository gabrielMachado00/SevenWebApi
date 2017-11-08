using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace TesteAPI.Controllers
{
    public class FilesController : ApiController
    {
        System.Timers.Timer tm = new System.Timers.Timer();

        [Route("api/postimagem/{idAtendimento}/{login}")]
        public HttpResponseMessage PostVideo(int idAtendimento, string login)
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                string[] login_split = login.Split(Convert.ToChar("@"));
                string[] cliente_split = login_split[1].Split(Convert.ToChar("."));
                foreach (string file in httpRequest.Files)
                {
                    if (!Directory.Exists(@"C:\inetpub\wwwroot\Arquivos\" + cliente_split[0] + @"\imagens\" + idAtendimento))
                    {
                        Directory.CreateDirectory(@"C:\inetpub\wwwroot\Arquivos\" + cliente_split[0] + @"\imagens\" + idAtendimento);
                    }
                    var postedFile = httpRequest.Files[file];
                    var filePath = @"C:\inetpub\wwwroot\Arquivos\" + cliente_split[0] + @"\imagens\" + idAtendimento + @"\" + postedFile.FileName;
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);
                    result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
                }                
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }

        [Route("api/postexame/{idAtendimento}/{login}")]
        public HttpResponseMessage PostExame(int idAtendimento, string login)
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                string[] login_split = login.Split(Convert.ToChar("@"));
                string[] cliente_split = login_split[1].Split(Convert.ToChar("."));
                foreach (string file in httpRequest.Files)
                {
                    if (!Directory.Exists(@"C:\inetpub\wwwroot\Arquivos\" + cliente_split[0] + @"\exames\" + idAtendimento))
                    {
                        Directory.CreateDirectory(@"C:\inetpub\wwwroot\Arquivos\" + cliente_split[0] + @"\exames\" + idAtendimento);
                    }
                    var postedFile = httpRequest.Files[file];
                    var filePath = @"C:\inetpub\wwwroot\Arquivos\" + cliente_split[0] + @"\exames\" + idAtendimento + @"\" + postedFile.FileName;
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;

        }
    }
}
