using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Logging
{
    public interface IHttpLogger : IDisposable
    {
        bool DebugEnabled { get; set; }

        void WriteDebug(string title, string message);

        void WriteInfo(string title, string message);

        void WriteError(string title, Exception exception);

        Task WriteHttpResponseAsync(IHttpLog log);
    }
}
