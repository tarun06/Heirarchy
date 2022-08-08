using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PrismApp.Services.Tests
{
    public class UserServiceTest
    {
        public const string url = "https://sanas-test-api.azurewebsites.net/";
        public IEnumerable<Supervisor> ExpectedSupervisorList { get; }

        public UserServiceTest()
        {
            ExpectedSupervisorList = new List<Supervisor>
            {
                new Supervisor(1, "Edgar mosie", "null"),
                new Supervisor(2, "Robin Bears", "null"),
                new Supervisor(3, "Heath Brian", "null"),
                new Supervisor(4, "Aaron Packers", "null"),
            };
        }

        [Fact]
        public void ThrowArgumentNullExceptionWhenUriIsNull()
        {
            //arrange
            HttpClientBase httpClientBase = SetUp(() => new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK
            });

            var userService = new UserService<Supervisor>(httpClientBase);

            // assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userService.GetUsers(string.Empty);
            });
        }

        [Fact]
        public async void Test1()
        {
            //arrange
            var json = JsonConvert.SerializeObject(ExpectedSupervisorList);

            HttpClientBase httpClientBase = SetUp(() => new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

            var userService = new UserService<Supervisor>(httpClientBase);


            //act
            var actualTeamList = await userService.GetUsers(url);

            //assert
            Assert.Equal(ExpectedSupervisorList, actualTeamList);
        }

        private static HttpClientBase SetUp(Func<HttpResponseMessage> httpResponse)
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>();

            // Setup Protected method on HttpMessageHandler mock.
            httpMessageHandler
                 .Protected()
                 .Setup<Task<HttpResponseMessage>>(
                     "SendAsync",
                     ItExpr.IsAny<HttpRequestMessage>(),
                     ItExpr.IsAny<CancellationToken>()
                 )
                 .ReturnsAsync(httpResponse());

            HttpClientBase httpClientBase = new HttpClientBase(httpMessageHandler.Object);
            return httpClientBase;
        }
    }
}