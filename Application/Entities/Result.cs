#nullable enable

using Application.Enums;

namespace Application.Entities
{
    public class Result
    {
        public object? ObjectResult { get; }

        public string? ErrorMessage { get; }

        public bool IsError { get; }

        public ResultType Type { get; }

        public string? Location { get; set; } = string.Empty;

        public Result(ResultType resultType, string errorMessage)
        {
            Type = resultType;
            ErrorMessage = errorMessage;
            IsError = true;
        }

        public Result(ResultType resultType, object objectResult)
        {
            Type = resultType;
            ObjectResult = objectResult;
            IsError = false;
        }

        public Result()
        {
            Type = ResultType.Ok;
            IsError = false;
        }
    }
}
