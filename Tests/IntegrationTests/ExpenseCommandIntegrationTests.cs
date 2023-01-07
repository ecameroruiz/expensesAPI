using System.Text;
using Application.Dtos;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebAPI;
using Xunit;

namespace Tests.IntegrationTests
{
    public class ExpenseCommandIntegrationTests : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _client;

        public ExpenseCommandIntegrationTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        private static string BaseUrl => "/api/expense";

        private static Fixture Fixture => new();

        private static StringContent GetRequestStringContent(object responseBody) => new(JsonConvert.SerializeObject(responseBody), Encoding.Default, "application/json");

        [Fact]
        public async Task CreateExpenseApi_GivenCorrectExpense_ThenReturn201CreatedResponse()
        {
            // Setup valid command dto with existing user id
            var expenseCommandDto = Fixture.Build<ExpenseCommandDto>()
                .With(e => e.Date, DateTime.UtcNow.AddDays(-10))
                .With(e => e.UserId, 1)
                .With(e => e.Currency, "USD")
                .With(e => e.ExpenseType, "Restaurant")
                .With(e => e.Comment, "Expense description")
                .Create();

            // Act
            var response = await GetPostResponse(expenseCommandDto);
            
            // Assert created response
            Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateExpenseApi_GivenIncorrectExpense_ThenReturn400BadRequestResponse()
        {
            // Setup command dto with empty comment
            var expenseCommandDto = Fixture.Build<ExpenseCommandDto>()
                .With(e => e.Date, DateTime.UtcNow.AddDays(-10))
                .With(e => e.Currency, "USD")
                .With(e => e.ExpenseType, "Restaurant")
                .Without(e => e.Comment)
                .Create();

            // Act
            var response = await GetPostResponse(expenseCommandDto);

            // Assert bad request response
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateExpenseApi_GivenNonExistingUser_ThenReturn404NotFoundResponse()
        {
            // Setup command dto with non existing user id
            var expenseCommandDto = Fixture.Build<ExpenseCommandDto>()
                .With(e => e.UserId, 100)
                .With(e => e.Date, DateTime.UtcNow.AddDays(-10))
                .Create();

            // Act
            var response = await GetPostResponse(expenseCommandDto);

            // Assert not found response
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateExpenseApi_GivenDuplicatedExpense_ThenReturn409ConflictResponse()
        {
            // Setup 2 command dtos with same amount and date for same user

            var date = DateTime.UtcNow;
            var amount = 50;

            var expenseCommandDto1 = Fixture.Build<ExpenseCommandDto>()
                .With(e => e.UserId, 2)
                .With(e => e.Currency, "RUB")
                .With(e => e.Date, date)
                .With(e => e.Amount, amount)
                .Create();

            var expenseCommandDto2 = Fixture.Build<ExpenseCommandDto>()
               .With(e => e.UserId, 2)
               .With(e => e.Currency, "RUB")
               .With(e => e.Date, date)
               .With(e => e.Amount, amount)
               .Create();

            // Create expense 1
            var response1 = await GetPostResponse(expenseCommandDto1);

            // Assert created response
            Assert.Equal(StatusCodes.Status201Created, (int)response1.StatusCode);

            // Try to create expense 2
            var response2 = await GetPostResponse(expenseCommandDto1);

            // Assert conflict response
            Assert.Equal(StatusCodes.Status409Conflict, (int)response2.StatusCode);
        }

        private async Task<HttpResponseMessage> GetPostResponse(ExpenseCommandDto expenseCommandDto)
        {
            // Setup post request and set command dto into body
            var request = new
            {
                Url = BaseUrl,
                Body = new
                {
                    expenseCommandDto.UserId,
                    expenseCommandDto.Date,
                    expenseCommandDto.Amount,
                    expenseCommandDto.Currency,
                    expenseCommandDto.ExpenseType,
                    expenseCommandDto.Comment
                }
            };

            // Serialize body and send request
            return await _client.PostAsync(request.Url, GetRequestStringContent(request.Body));
        }
    }
}
