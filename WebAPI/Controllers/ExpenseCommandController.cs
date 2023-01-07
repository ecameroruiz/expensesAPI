using System;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/expense")]
    public class ExpenseCommandController : ControllerBase
    {
        private readonly IExpenseCommandService _expenseCommandService;

        public ExpenseCommandController(IExpenseCommandService expenseCommandService)
        {
            _expenseCommandService = expenseCommandService;
        }

        /// <summary>
        ///     Creates new expense
        /// </summary>
        /// <response code="201">Expense successfully created</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">Associated user not found</response>
        /// <response code="409">Expense already exists for associated user</response>
        /// <response code="500">An unknown error occurred while processing the request</response>
        [HttpPost]
        [ProducesResponseType(typeof(ExpenseQueryDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseCommandDto expenseCommandDto)
        {
            try
            {
                var result = await _expenseCommandService.CreateExpense(expenseCommandDto);
                return result.GetResponse();
            }
            catch (Exception e)
            {
                return e.GetErrorResponse();
            }
        }     
    }
}