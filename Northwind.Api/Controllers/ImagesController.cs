using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Data.Edm.Library;
using Northwind.Data;
using Northwind.Model;

namespace Northwind.Api.Controllers
{
    [RoutePrefix("api/images")]
    [ApiExplorerSettings(IgnoreApi = true)]
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [Route("~/api/employees/{employeeId:int}/images")]
        [HttpGet]
        public IHttpActionResult GetEmployeesImage(int employeeId)
        {
            var emp = _repository.Queryable().SingleOrDefault(e => e.EmployeeID == employeeId);
            if (emp == null) return NotFound();

            byte[] imgData = emp.Photo;
            FileStream fs = new FileStream("", FileMode.Create);
            fs.Write(imgData,0,imgData.Length);
            var ms = new MemoryStream(imgData);
            Image img = Image.FromStream(ms);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Content = new StreamContent(ms);
            response.Content = new ByteArrayContent(ms.ToArray());
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
           // var en = Convert.ToBase64String(emp.Photo);
            //return Ok("data:image/png;base64," + en);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostAsync()
        {
            if (!Request.Content.IsMimeMultipartContent()) return BadRequest();

            //var streamProvider = new MultipartFormDataStreamProvider(Path.GetTempPath());
            var streamProvider = new MultipartMemoryStreamProvider();
            var path = HostingEnvironment.MapPath("~/Content/");
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            foreach (HttpContent context in streamProvider.Contents)
            {
                using (Stream stream = context.ReadAsStreamAsync().Result)
                {
                    var name = context.Headers.ContentDisposition.FileName.Replace("\"", "");
                    using (var fs = new FileStream(Path.Combine(path, name), FileMode.Create))
                    {
                        await stream.CopyToAsync(fs);
                    }

                    var photoByte = StreamToByteArray(stream);
                    var emp = _repository.Queryable()
                        .SingleOrDefault(e => e.EmployeeID == 1);

                    if (emp == null) continue;
                    emp.Photo = photoByte;
                    emp.PhotoPath = name;

                    var saved = await _repository.SaveChangesAsync();
                }
            }

            //var fileNames = streamProvider.FileData.Select(entry => entry.LocalFileName);
            //var names = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.FileName);
            //var contextTypes = streamProvider.FileData.Select(entry => entry.Headers.ContentType.MediaType);

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
