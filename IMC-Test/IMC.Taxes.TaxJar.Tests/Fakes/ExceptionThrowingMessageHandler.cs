using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMC.Taxes.TaxJar.Tests.Fakes
{
    public class ExceptionThrowingMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new TimeoutException("This is a testing environment exeption. You should encounter this elsewhere");
        }
    }
}
