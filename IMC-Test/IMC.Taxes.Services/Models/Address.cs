using System;
using System.Collections.Generic;
using System.Text;

namespace IMC.Taxes.Services
{
    public class Address
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
    }

    public class NexusAddress : Address
    {
        public string Id { get; set; }
    }
}
