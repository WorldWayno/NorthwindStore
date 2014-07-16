using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Data.Edm.Library;
using Northwind.Data;
using Northwind.Model;

namespace Northwind.Api.Controllers
{
    [RoutePrefix("api/images")]
    public class ImagesController : ApiController
    {
        private readonly IRepository<Employee> _repository;

        /// <summary>
        /// </summary>
        /// <param name="repository"></param>
        public ImagesController(IRepository<Employee> repository)
        {
            _repository = repository;
        }


        [Route("~/api/employees/{employeeId:int}/images")]
        [HttpGet]
        public HttpResponseMessage GetEmployeesImage(int employeeId)
        {
            var emp = _repository.Queryable().SingleOrDefault(e => e.EmployeeID == employeeId);
            if (emp == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            byte[] imgData = emp.Photo;
            var ms = new MemoryStream(imgData);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(ms);
            //response.Content = new ByteArrayContent(ms.ToArray());
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");

            return response;
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostAsync()
        {
            if (Request.Content.IsMimeMultipartContent())
            {
                var streamProvider = new MultipartFormDataStreamProvider(Path.GetTempPath());
                var mp = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(mp);

                foreach (HttpContent context in mp.Contents)
                {
                    using (Stream stream = context.ReadAsStreamAsync().Result)
                    {
                        Image image = Image.FromStream(stream);
                        var name = context.Headers.ContentDisposition.FileName.Replace("\"", "");

                        var bte = StreamToByteArray(stream);
                        var emp = _repository.Queryable()
                            .SingleOrDefault(e => e.EmployeeID == 1);
                        if (emp != null)
                        {
                            emp.Photo = bte;
                            emp.PhotoPath = name;

                            var saved = _repository.SaveChanges();
                        }
                    }
                }

                //var fileNames = streamProvider.FileData.Select(entry => entry.LocalFileName);
                //var names = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.FileName);
                //var contextTypes = streamProvider.FileData.Select(entry => entry.Headers.ContentType.MediaType);
            }

            return Ok();
        }

        private byte[] StreamToByteArray(Stream inputStream)
        {
            if (!inputStream.CanRead)
            {
                throw new ArgumentException();
            }

            // This is optional
            if (inputStream.CanSeek)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            byte[] output = new byte[inputStream.Length];
            int bytesRead = inputStream.Read(output, 0, output.Length);

            Debug.Assert(bytesRead == output.Length, "Bytes read from stream matches stream length");
            return output;
        }
        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            Image img = Image.FromStream(ms);
            return img;
        }
    }
}
