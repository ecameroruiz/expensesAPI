using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Extensions
{
    public static class ExceptionExtensions
    {
        public static ObjectResult GetErrorResponse(this Exception exception)
        {
            var errorObjectResult = new ObjectResult(exception.Message)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            return errorObjectResult;
        }
    }
}

