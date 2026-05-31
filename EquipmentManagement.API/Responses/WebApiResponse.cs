namespace EquipmentManagement.API.Responses
{
    public class WebApiResponse<T>
    {
        public bool Success { get; private set; }
        public T? Data { get; private set; }
        public string? Message { get; private set; }
        public IEnumerable<string>? Errors { get; private set; }
        public int StatusCode { get; private set; }
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

        // ─── Factories ───────────────────────────────────────────

        public static WebApiResponse<T> Ok(T data, string? message = null) =>
            new()
            {
                Success = true,
                Data = data,
                Message = message,
                StatusCode = HttpStatusCodeResponse.Ok
            };

        public static WebApiResponse<T> Fail(string message, int statusCode = HttpStatusCodeResponse.BadRequest) =>
            new()
            {
                Success = false,
                Message = message,
                StatusCode = statusCode
            };

        public static WebApiResponse<T> Fail(IEnumerable<string> errors, int statusCode = HttpStatusCodeResponse.BadRequest) =>
            new()
            {
                Success = false,
                Errors = errors,
                StatusCode = statusCode
            };

        public static WebApiResponse<T> InternalError(string message = "Erro interno no servidor.") =>
            new()
            {
                Success = false,
                Message = message,
                StatusCode = HttpStatusCodeResponse.InternalServerError
            };

        public static WebApiResponse<T> Partial(string message) =>
            new()
            {
                Success = false,
                Message = message,
                StatusCode = HttpStatusCodeResponse.PartialContent
            };

        public static WebApiResponse<T> NotFound(string message = "Recurso não encontrado.") =>
            new()
            {
                Success = false,
                Message = message,
                StatusCode = HttpStatusCodeResponse.NotFound
            };
    }
}