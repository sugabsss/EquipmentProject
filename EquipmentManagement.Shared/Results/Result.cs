namespace EquipmentManagement.Shared.Results
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public string? Error { get; protected set; }
        public ResultStatus Status { get; protected set; }

        // ─── Factories ───────────────────────────────────────────

        public static Result Ok() =>
            new() { IsSuccess = true, Status = ResultStatus.Ok };

        public static Result Fail(string error) =>
            new() { IsSuccess = false, Error = error, Status = ResultStatus.Fail };

        public static Result NotFound(string error) =>
            new() { IsSuccess = false, Error = error, Status = ResultStatus.NotFound };

        public static Result Unauthorized(string error) =>
            new() { IsSuccess = false, Error = error, Status = ResultStatus.Unauthorized };

        public static Result Invalid(string error) =>
            new() { IsSuccess = false, Error = error, Status = ResultStatus.Invalid };

        // ─── Factories com genérico ───────────────────────────────

        public static Result<T> Ok<T>(T value) => Result<T>.Ok(value);
        public static Result<T> Fail<T>(string error) => Result<T>.Fail(error);
        public static Result<T> NotFound<T>(string error) => Result<T>.NotFound(error);
        public static Result<T> Unauthorized<T>(string error) => Result<T>.Unauthorized(error);
        public static Result<T> Invalid<T>(string error) => Result<T>.Invalid(error);
    }

    public class Result<T> : Result
    {
        public T? Value { get; private set; }

        // ─── Factories ───────────────────────────────────────────

        public static Result<T> Ok(T value) =>
            new() { IsSuccess = true, Value = value, Status = ResultStatus.Ok };

        public static new Result<T> Fail(string error) =>
            new() { IsSuccess = false, Error = error, Status = ResultStatus.Fail };

        public static new Result<T> NotFound(string error) =>
            new() { IsSuccess = false, Error = error, Status = ResultStatus.NotFound };

        public static new Result<T> Unauthorized(string error) =>
            new() { IsSuccess = false, Error = error, Status = ResultStatus.Unauthorized };

        public static new Result<T> Invalid(string error) =>
            new() { IsSuccess = false, Error = error, Status = ResultStatus.Invalid };

        // ─── Implicit operator ────────────────────────────────────
        // Permite retornar T diretamente sem precisar chamar Result.Ok(value)

        public static implicit operator Result<T>(T value) => Ok(value);
    }

    public enum ResultStatus
    {
        Ok,
        Fail,
        NotFound,
        Unauthorized,
        Invalid
    }
}
