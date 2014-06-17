using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Owin.Logging;

namespace Northwind.Data.Interceptors
{
    internal class LoggingCommandInterceptor : IDbCommandInterceptor
    {
        private static ILogger logger = LoggerFactory.Default.Create("s");

        private static readonly Regex TableAliasRegex =
     new Regex(@"(?<table>AS \[Extent\d+\](?! WITH \(NOLOCK\)))",
         RegexOptions.Multiline | RegexOptions.IgnoreCase);

        public void NonQueryExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            //LogCommand(command,interceptionContext);
        }

        public void NonQueryExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            LogCommand(command, interceptionContext);
        }

        public void ReaderExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
        {
           // LogCommand(command, interceptionContext);
        }

        public void ReaderExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
        {
            command.CommandText = TableAliasRegex.Replace(command.CommandText, "${table} WITH (NOLOCK)");
            LogCommand(command, interceptionContext);
        }

        public void ScalarExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            //LogCommand(command, interceptionContext);
        }

        public void ScalarExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            LogCommand(command, interceptionContext);
        }

        private void LogCommand<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            Debug.Write(command.CommandText);
            logger.WriteInformation(command.CommandText);
        }
    }
}
