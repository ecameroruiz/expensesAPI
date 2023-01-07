using Microsoft.AspNetCore.Http;
using WebAPI;
using Xunit;

namespace Tests.IntegrationTests
{
    public class ExpenseQueryIntegrationTests : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _client;

        public ExpenseQueryIntegrationTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        private static string BaseUrl => "/api/expense";

        private static string GetQueryParams(bool orderByAmount = false, bool orderByDate = false) => $"?orderByAmount={orderByAmount}&orderByDate={orderByDate}";

        [Fact]
        public async Task GetExpensesByUserApi_GivenExistingUserAndExistingExpenses_ThenReturn200OkResponse()
        {
            await VerifyGetRequest($"{BaseUrl}/user/1{GetQueryParams()}", StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetExpensesByUserApi_GivenNonExistingUser_ThenReturn404NotFoundResponse()
        {
            await VerifyGetRequest($"{BaseUrl}/user/3{GetQueryParams()}", StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GetExpensesByUserApi_GivenMoreThanOneSortingOptionSet_ThenReturn400BadReques()
        {
            await VerifyGetRequest($"{BaseUrl}/user/1{GetQueryParams(orderByAmount:true, orderByDate:true)}", StatusCodes.Status400BadRequest);
        }

        private async Task VerifyGetRequest(string url, int expectedStatusCode)
        {
            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(expectedStatusCode, (int)response.StatusCode);
        }
    }
}
