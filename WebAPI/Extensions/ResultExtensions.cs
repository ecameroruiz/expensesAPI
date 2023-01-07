using Microsoft.AspNetCore.Mvc;
using Application.Enums;
using Application.Entities;
using System.ComponentModel;

namespace WebAPI.Extensions
{
    public static class ResultExtensions
	{
		public static ObjectResult GetResponse(this Result result)
		{
            return result.Type switch
            {
                ResultType.Ok => new OkObjectResult(result.ObjectResult),
                ResultType.Created => new CreatedResult(result.Location, result.ObjectResult),
                ResultType.BadRequest => new BadRequestObjectResult(result.ErrorMessage),
                ResultType.Duplicated => new ConflictObjectResult(result.ErrorMessage),
                ResultType.NotFound => new NotFoundObjectResult(result.ErrorMessage),
                _ => throw new InvalidEnumArgumentException($"Invalid value for result type {result.Type}"),
            };
        }
	}
}

