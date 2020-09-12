using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMC.Taxes.TaxJar.Tests
{
    public class FakeMessageHandler : DelegatingHandler
    {
        public HttpResponseMessage FakeResponse { get; }
        public FakeMessageHandler(HttpResponseMessage fakeResponse)
        {
            FakeResponse = fakeResponse;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(FakeResponse);
        }
    }
}
