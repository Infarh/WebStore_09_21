using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Interfaces.TestAPI;
using Assert = Xunit.Assert;

namespace WebStore.WebAPI.Tests.Controllers
{
    [TestClass]
    public class WebAPIIntegrationTest
    {
        private readonly string[] _ExpectedValues = Enumerable.Range(1, 10).Select(i => $"TestValue - {i}").ToArray();

        private WebApplicationFactory<Startup> _Host;

        [TestInitialize]
        public void Initialize()
        {
            var values_service_mock = new Mock<IValuesService>();
            values_service_mock.Setup(s => s.GetAll()).Returns(_ExpectedValues);

            _Host = new WebApplicationFactory<Startup>()
               .WithWebHostBuilder(host => host
                   .ConfigureServices(services => services
                       .AddSingleton(values_service_mock.Object)));
        }

        [TestMethod, Timeout(3000), Ignore]
        public async Task GetValues()
        {
            var client = _Host.CreateClient();

            var response = await client.GetAsync("/WebAPI");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var parser = new HtmlParser();

            var content_stream = await response.Content.ReadAsStreamAsync();
            var html = parser.ParseDocument(content_stream);

            var items = html.QuerySelectorAll(".container table.table tbody tr td:last-child");

            var actual_values = items.Select(item => item.Text());

            Assert.Equal(_ExpectedValues, actual_values);
        }
    }
}
