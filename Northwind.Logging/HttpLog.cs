using System;

namespace Northwind.Logging
{
    public class HttpLog : IHttpLog
    {

        public string Id { get; set; }

        public DateTime RequestDate { get; set; }

        public string RequestMethod { get; set; }

        public string RequestUrl { get; set; }

        public int HttpStatusCode { get; set; }

        public int ThreadId { get; set; }

        public string RemoteAddress { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public long ResponseTime { get; set; }

        public string Resource { get; set; }
    }
}