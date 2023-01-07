using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.Interfaces;
using WebAPI.Extensions;
using Microsoft.AspNetCore.Http;
using Application.Dtos;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/expense")]
    public class ExpenseQueryController : ControllerBase
    {
        private readonly IExpenseQueryService _expenseQueryService;

        public ExpenseQueryController(IExpenseQueryService expenseQueryService)
        {
            _expenseQueryService = expenseQueryService;
        }

        /// <summary>
        ///     Gets all expenses by given user
        /// </summary>
        /// <param name="userId">The id of the user associated to the expense</param>
        /// <param name="orderByAmount">List expenses ordered by amount</param>
        /// <param name="orderByDate">List expenses ordered by date</param>
        /// <response code="200">Returns expenses</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">Associated user not found or no expenses found for given user</response>
        /// <response code="500">An unknown error occurred while processing the request</response>
        [HttpGet("user/{userId:long}")]
        [ProducesResponseType(typeof(ExpenseQueryDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExpensesByUserId([FromRoute] long userId, [FromQuery] bool orderByAmount = false, [FromQuery] bool orderByDate = false)
        {
            try
            {
                var result = await _expenseQueryService.GetExpensesByUser(userId, orderByAmount, orderByDate);
                return result.GetResponse();
            }
            catch (Exception e)
            {
                return e.GetErrorResponse();
            }
        }
    }
}