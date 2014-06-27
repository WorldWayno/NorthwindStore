using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using NUnit.Framework;

namespace Northwind.Api.Tests
{
    [TestFixture]
    public class TestBase
    {
        internal HttpClient Client;

        protected TestBase()
        {
            Client = new HttpClient(new HttpClientHandler());
        }


        [TestFixtureTearDown]
        public void Dispose()
        {
            Client.Dispose();
        }

    }
}
