using Application.Converters;
using Application.Dtos;
using Application.Entities;
using Application.Enums;
using Application.Interfaces;
using Application.Services;
using AutoFixture;
using Domain.Entities;
using Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace Tests.ApplicationTests
{
    public class ExpenseQueryServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IExpenseRepository> _expenseRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        private readonly IExpenseQueryConverter _expenseQueryConverter;

        private readonly IExpenseQueryService _sut;

        public ExpenseQueryServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _expenseRepositoryMock = _mockRepository.Create<IExpenseRepository>();
            _userRepositoryMock = _mockRepository.Create<IUserRepository>();

            _expenseQueryConverter = new ExpenseQueryConverter();

            _sut = new ExpenseQueryService(
                _expenseRepositoryMock.Object,
                _userRepositoryMock.Object,
                _expenseQueryConverter);
        }

        private static Fixture Fixture => new();

        IEnumerable<ExpenseQueryDto> ToQueryDtos(IEnumerable<Expense> expenses, User user) => expenses.Select(expense => _expenseQueryConverter.ToQueryDto(expense, user));

        [Fact]
        public async void GetExpensesByUser_GivenMoreThanOneSortingOptionSet_ThenReturnBadRequest()
        {
            // Act
            Result result = await _sut.GetExpensesByUser(1, orderByAmount: true, orderByDate: true);

            // Assert result type is bad request
            Assert.Equal(ResultType.BadRequest, result.Type);
            Assert.True(result.IsError);
        }

        [Fact]
        public async void GetExpensesByUser_GivenNonExistingUser_ThenReturnNotFound()
        {
            // Setup user repo to return null for given user id
            _userRepositoryMock.Setup(mockRepo => mockRepo.GetUserById(1)).ReturnsAsync((User?)null);

            // Act
            Result result = await _sut.GetExpensesByUser(1, orderByAmount: false, orderByDate: false);

            // Assert result type is not found
            Assert.Equal(ResultType.NotFound, result.Type);
            Assert.True(result.IsError);
        }

        [Fact]
        public async void GetExpensesByUser_GivenNoExistingExpenses_ThenReturnOkWithEmptyList()
        {
            // Setup existing user
            var user = Fixture.Build<User>().Create();

            // Setup user repo to return existing user
            _userRepositoryMock.Setup(mockRepo => mockRepo.GetUserById(user.Id)).ReturnsAsync(user);

            // Setup expense repo as empty for given user
            _expenseRepositoryMock.Setup(mockRepo => mockRepo.GetExpensesByUserId(user.Id)).ReturnsAsync(new Expense[] { });

            // Act
            Result result = await _sut.GetExpensesByUser(user.Id, orderByAmount: false, orderByDate: false);

            // Assert result type is ok
            Assert.Equal(ResultType.Ok, result.Type);
            Assert.False(result.IsError);

            // Verify object result is empty expense list
            var expenses = result.ObjectResult as IEnumerable<ExpenseQueryDto>;
            Assert.NotNull(expenses);
            Assert.Empty(expenses);
        }

        [Fact]
        public async void GetExpensesByUser_GivenExistingExpenses_ThenReturnOkWithExpenseList()
        {
            // Setup existing user
            var user = Fixture.Build<User>().Create();

            // Setup user repo to return existing user
            _userRepositoryMock.Setup(mockRepo => mockRepo.GetUserById(user.Id)).ReturnsAsync(user);

            var expenses = Fixture.Build<Expense>().CreateMany(count: 3);

            // Setup expense repo with existing list of expenses
            _expenseRepositoryMock.Setup(mockRepo => mockRepo.GetExpensesByUserId(user.Id)).ReturnsAsync(expenses);

            // Act
            Result result = await _sut.GetExpensesByUser(user.Id, orderByAmount: false, orderByDate: false);

            // Assert result type is ok
            Assert.Equal(ResultType.Ok, result.Type);
            Assert.False(result.IsError);

            // Verify object result is expense list
            var retrievedExpenses = result.ObjectResult as IEnumerable<ExpenseQueryDto>;
            Assert.NotNull(retrievedExpenses);

            // Verify expense list is as expected
            var expectedQueryDtos = ToQueryDtos(expenses, user);
            CompareQueryDtos(expectedQueryDtos.ToArray(), retrievedExpenses.ToArray());
        }

        [Fact]
        public async void GetExpensesByUser_GivenExistingExpensesAndSortingSetToDate_ThenReturnOkWithExpenseListSortedByDate()
        {
            // Setup existing user
            var user = Fixture.Build<User>().Create();

            // Setup user repo to return existing user
            _userRepositoryMock.Setup(mockRepo => mockRepo.GetUserById(user.Id)).ReturnsAsync(user);

            var expenses = Fixture.Build<Expense>().CreateMany(count: 3);

            // Setup expense repo with existing list of expenses
            _expenseRepositoryMock.Setup(mockRepo => mockRepo.GetExpensesByUserId(user.Id)).ReturnsAsync(expenses);

            // Act
            Result result = await _sut.GetExpensesByUser(user.Id, orderByAmount: false, orderByDate: true);

            // Assert result type is ok
            Assert.Equal(ResultType.Ok, result.Type);
            Assert.False(result.IsError);

            // Verify object result is expense list
            var retrievedExpenses = result.ObjectResult as IEnumerable<ExpenseQueryDto>;
            Assert.NotNull(retrievedExpenses);

            // Verify expense list is as expected
            var expectedQueryDtos = ToQueryDtos(expenses, user).OrderBy(e => e.Date);
            CompareQueryDtos(expectedQueryDtos.ToArray(), retrievedExpenses.ToArray());
        }

        [Fact]
        public async void GetExpensesByUser_GivenExistingExpensesAndSortingSetToAmount_ThenReturnOkWithExpenseListSortedByAmount()
        {
            // Setup existing user
            var user = Fixture.Build<User>().Create();

            // Setup user repo to return existing user
            _userRepositoryMock.Setup(mockRepo => mockRepo.GetUserById(user.Id)).ReturnsAsync(user);

            var expenses = Fixture.Build<Expense>().CreateMany(count: 3);

            // Setup expense repo with existing list of expenses
            _expenseRepositoryMock.Setup(mockRepo => mockRepo.GetExpensesByUserId(user.Id)).ReturnsAsync(expenses);

            // Act
            Result result = await _sut.GetExpensesByUser(user.Id, orderByAmount: true, orderByDate: false);

            // Assert result type is ok
            Assert.Equal(ResultType.Ok, result.Type);
            Assert.False(result.IsError);

            // Verify object result is expense list
            var retrievedExpenses = result.ObjectResult as IEnumerable<ExpenseQueryDto>;
            Assert.NotNull(retrievedExpenses);

            // Verify expense list is as expected
            var expectedQueryDtos = ToQueryDtos(expenses, user).OrderBy(e => e.Amount);
            CompareQueryDtos(expectedQueryDtos.ToArray(), retrievedExpenses.ToArray());
        }

        private void CompareQueryDtos(ExpenseQueryDto[] expected, ExpenseQueryDto[] actual)
        {
            for (var i=0; i < expected.Length; i++)
            {
                Compare(expected[i], actual[i]);
            }
            void Compare(ExpenseQueryDto expected, ExpenseQueryDto actual)
            {
                Assert.Equal(expected.Amount, actual.Amount);
                Assert.Equal(expected.Comment, actual.Comment);
                Assert.Equal(expected.Currency, actual.Currency);
                Assert.Equal(expected.UserFullName, actual.UserFullName);
                Assert.Equal(expected.Date, actual.Date);
            }
        }
    }
}

