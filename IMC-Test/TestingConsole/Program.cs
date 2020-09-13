using IMC.Taxes.Services;
using IMC.Taxes.Services.Models;
using IMC.Taxes.TaxJar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace TestingConsole
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }

        static void Main(string[] args)
        {
            BuildConfiguration();
            ConfigureServices();

            var calculator = ServiceProvider.GetService<ITaxCalculator>();
            var locationInfo = new TaxLocationInfo
            {
                Country = "US",
                City = "Santa Monica",
                ZipCode = "90404"
            };
            var orderInfo = new OrderInfo
            {
                From = new Address
                {
                    Country = "US",
                    State = "NJ",
                    ZipCode = "07001"
                },
                To = new Address
                {
                    Country = "US",
                    State = "NJ",
                    ZipCode = "07446"
                },
                Amount = 16.5m,
                Shipping = 1.5m,
                LineItems = new[]
                {
                    new LineItem 
                    {
                        Quantity = 1,
                        UnitPrice = 15m,
                        ProductTaxCode = "31000"
                    }
                }
            };

            var rates = calculator.TaxRatesForLocation(locationInfo);
            Console.WriteLine(rates.StateRate);

            var taxes = calculator.CalculateTaxesForOrder(orderInfo);
            Console.WriteLine(taxes.AmountToCollect);

            System.Console.ReadLine();
        }

        private static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder();

            // this is only in development must be changed to use the production config
            builder.AddUserSecrets<Program>();

            Configuration = builder.Build();
        }

        private static void ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services
                .AddOptions()
                .AddTransient<ITaxCalculator, TaxJarTaxCalculator>()
                .AddTransient<TaxService>()
                .BuildServiceProvider();

            ConfigureHttpClients(services);

            ServiceProvider = services.BuildServiceProvider();
        }

        private static void ConfigureHttpClients(IServiceCollection services)
        {
            var baseUrl = Configuration["TaxJarConfig:BaseUrl"];
            var key = Configuration["TaxJarConfig:Key"];
            services.AddHttpClient("TaxJar", client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", $"token=\"{key}\"");
            });
        }

    }
}
