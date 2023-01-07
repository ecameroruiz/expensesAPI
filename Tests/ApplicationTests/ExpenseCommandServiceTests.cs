using Application.Converters;
using Application.Dtos;
using Application.Entities;
using Application.Enums;
using Application.Interfaces;
using Application.Services;
using AutoFixture;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace Tests.ApplicationTests
{
    public class ExpenseCommandServiceTests
	{
        private readonly MockRepository _mockRepository;

        private readonly Mock<IExpenseRepository> _expenseRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        private readonly IExpenseCommandConverter _expenseCommandConverter;
        private readonly IExpenseQueryConverter _expenseQueryConverter;

        private readonly IExpenseCommandService _sut;

        public ExpenseCommandServiceTests()
		{
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _expenseRepositoryMock = _mockRepository.Create<IExpenseRepository>();
            _userRepositoryMock = _mockRepository.Create<IUserRepository>();

            _expenseCommandConverter = new ExpenseCommandConverter();
            _expenseQueryConverter = new ExpenseQueryConverter();

            _sut = new ExpenseCommandService(
                _expenseRepositoryMock.Object,
                _userRepositoryMock.Object,
                _expenseCommandConverter,
                _expenseQueryConverter);
        }

        private static Fixture Fixture => new();

        private static DateTime DefaultValidDate => DateTime.UtcNow.AddDays(-10);

        public static IEnumerable<object[]> InvalidCommandDtos
        {
            get
            {
                {
                    // Empty comment
                    var expenseCommandDto = Fixture.Build<ExpenseCommandDto>()
                        .With(e => e.Date, DefaultValidDate)
                        .Without(e => e.Comment)
                        .Create();
                    yield return new object[] { expenseCommandDto };
                }
                {
                    // Date older than 3 months
                    var expenseCommandDto = Fixture.Build<ExpenseCommandDto>()
                        .With(e => e.Date, DateTime.Now.AddMonths(-4))
                        .Create();
                    yield return new object[] { expenseCommandDto };
                }
                {
                    // Date in the future
                    var expenseCommandDto = Fixture.Build<ExpenseCommandDto>()
                        .With(e => e.Date, DateTime.Now.AddMonths(1))
                        .Create();
                    yield return new object[] { expenseCommandDto };
                }
                {
                    // Invalid expense type
                    var expenseCommandDto = Fixture.Build<ExpenseCommandDto>()
                        .With(e => e.Date, DefaultValidDate)
                        .With(e => e.ExpenseType, "InvalidExpenseType")
                        .Create();
                    yield return new object[] { expenseCommandDto };
                }
                {
                    // Invalid currency type
                    var expenseCommandDto = Fixture.Build<ExpenseCommandDto>()
                        .With(e => e.Date, DefaultValidDate)
                        .With(e => e.Currency, "InvalidCurrency")
                        .Create();
                    yield return new object[] { expenseCommandDto };
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidCommandDtos))]
        public async void CreateExpense_GivenInvalidCommandDto_ThenReturnBadRequest(ExpenseCommandDto expenseCommandDto)
        {
            // Act
            Result result = await _sut.CreateExpense(expenseCommandDto);

            // Assert result is error and bad request type
            Assert.Equal(ResultType.BadRequest, result.Type);
            Assert.True(result.IsError);
        }

        [Fact]
        public async void CreateExpense_GivenNonExistingUser_ThenReturnNotFound()
        {
            // Setup command dto
            var expenseCommandDto = Fixture.Build<ExpenseCommandDto>()
                .With(e => e.Date, DefaultValidDate)
                .Create();

            // Setup user repo to return null for given user id
            _userRepositoryMock.Setup(mockRepo => mockRepo.GetUserById(expenseCommandDto.UserId)).ReturnsAsync((User?)null);

            // Act
            Result result = await _sut.CreateExpense(expenseCommandDto);

            // Assert result type is not found
            Assert.Equal(ResultType.NotFound, result.Type);
            Assert.True(result.IsError);
        }

        [Fact]
        public async void CreateExpense_GivenNonMatchingCurrency_ThenReturnBadRequest()
        {
            // Setup command dto
            var expenseCommandDto = Fixture.Build<ExpenseCommandDto>()
                .With(e => e.Date, DefaultValidDate)
                .With(e => e.UserId, 1)
                .With(e => e.Currency, "RUB")
                .Create();

            // Setup existing user with different currency
            var user = Fixture.Build<User>()
                .With(u => u.Id, 1)
                .With(u => u.Currency, Currency.USD)
                .Create();

            // Setup user repo to return user
            _userRepositoryMock.Setup(mockRepo => mockRepo.GetUserById(expenseCommandDto.UserId)).ReturnsAsync(user);

            // Act
            Result result = await _sut.CreateExpense(expenseCommandDto);

            // Assert result type is bad request
            Assert.Equal(ResultType.BadRequest, result.Type);
            Assert.True(result.IsError);
        }

        [Fact]
        public async void CreateExpense_GivenExistingExpenseWithSameAmountAndDate_ThenReturnDuplicated()
        {
            // Setup command dto
            var expenseCommandDto = Fixture.Build<ExpenseCommandDto>()
                .With(e => e.Date, DefaultValidDate)
                .With(e => e.UserId, 1)
                .With(e => e.Currency, "USD")
                .Create();

            // Setup existing user
            var user = Fixture.Build<User>()
                .With(u => u.Id, 1)
                .With(u => u.Currency, Currency.USD)
                .Create();

            // Setup user repo to return existing user
            _userRepositoryMock.Setup(mockRepo => mockRepo.GetUserById(expenseCommandDto.UserId)).ReturnsAsync(user);

            // Setup existing expense with same amount and date
            var expense = Fixture.Build<Expense>()
                .With(u => u.Id, 1)
                .With(u => u.Currency, Currency.USD)
                .With(u => u.Date, expenseCommandDto.Date)
                .With(u => u.Amount, expenseCommandDto.Amount)
                .Create();

            // Setup expense repo to return existing duplicated expense
            _expenseRepositoryMock.Setup(mockRepo => mockRepo.GetExpensesByUserId(expenseCommandDto.UserId)).ReturnsAsync(new[] { expense });

            // Act
            Result result = await _sut.CreateExpense(expenseCommandDto);

            // Assert result type is duplicated
            Assert.Equal(ResultType.Duplicated, result.Type);
            Assert.True(result.IsError);
        }

        [Fact]
        public async void CreateExpense_GivenCorrectExpense_ThenReturnCreated()
        {
            // Setup command dto
            var expenseCommandDto = Fixture.Build<ExpenseCommandDto>()
                .With(e => e.Date, DefaultValidDate)
                .With(e => e.UserId, 1)
                .With(e => e.Currency, "USD")
                .Create();

            // Setup existing user
            var user = Fixture.Build<User>()
                .With(u => u.Id, 1)
                .With(u => u.Currency, Currency.USD)
                .Create();

            // Setup user repo to return existing user
            _userRepositoryMock.Setup(mockRepo => mockRepo.GetUserById(expenseCommandDto.UserId)).ReturnsAsync(user);

            // Setup expense repo as empty so there are no duplicated expenses
            _expenseRepositoryMock.Setup(mockRepo => mockRepo.GetExpensesByUserId(expenseCommandDto.UserId)).ReturnsAsync(new Expense[] {});

            // Setup expense repo for creation
            _expenseRepositoryMock.Setup(mockRepo => mockRepo.CreateExpense(It.IsAny<Expense>())).ReturnsAsync(_expenseCommandConverter.FromCommandDto(expenseCommandDto));

            // Act
            Result result = await _sut.CreateExpense(expenseCommandDto);

            // Assert result type is created
            Assert.Equal(ResultType.Created, result.Type);
            Assert.False(result.IsError);
            
            // Verify created result is returned
            var createdExpense = result.ObjectResult as ExpenseQueryDto;
            Assert.NotNull(createdExpense);

            // Verify expense location
            Assert.Equal($"expense/{createdExpense.Id}", result.Location);

            // Verify created result matches creation conditions
            Assert.Equal(expenseCommandDto.Currency, createdExpense.Currency);
            Assert.Equal(expenseCommandDto.ExpenseType, createdExpense.ExpenseType);
            Assert.Equal(expenseCommandDto.Amount, createdExpense.Amount);
            Assert.Equal(expenseCommandDto.Comment, createdExpense.Comment);
            Assert.Equal(expenseCommandDto.Date, createdExpense.Date);
            Assert.Equal($"{user.FirstName} {user.LastName}", createdExpense.UserFullName);
        }
    }
}

