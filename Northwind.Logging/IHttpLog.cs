using System;

namespace Northwind.Logging
{
    public interface IHttpLog
    {
        string Id { get; set; }
        DateTime RequestDate { get; set; }
        string RequestMethod { get; set; }
        string RequestUrl { get; set; }
        int HttpStatusCode { get; set; }
        int ThreadId { get; set; }
        string RemoteAddress { get; set; }
        string UserName { get; set; }
        string Message { get; set; }
        long ResponseTime { get; set; }

        string Resource { get; set; }
    }
}