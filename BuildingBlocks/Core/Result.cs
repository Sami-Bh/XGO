namespace BuildingBlocks.Core
{
    public class Result<T>
    {
        public bool IsValid { get; set; }
        public T? Value { get; set; }
        public string? ErrorMessage { get; set; }
        public int ErrorCode { get; set; }

        public static Result<T> Success(T value) => new Result<T> { Value = value, IsValid = true };
        public static Result<T> Failure(int errorCode, string errorMessage) => new Result<T> { ErrorCode = errorCode, ErrorMessage = errorMessage };
    }
}
