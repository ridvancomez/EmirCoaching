namespace EmirCoaching.Business.Common;

public enum ResultErrorType
{
    None = 0,
    Validation = 1, // 400
    NotFound = 2,   // 404
    Conflict = 3,   // 409
    TooManyRequests = 4, // 429
    Unauthorized = 5     // 401
}

public class Result
{
    protected Result(bool isSuccess, ResultErrorType errorType, IReadOnlyList<string> errors)
    {
        IsSuccess = isSuccess;
        ErrorType = errorType;
        Errors = errors;
    }

    public bool IsSuccess { get; }
    public ResultErrorType ErrorType { get; }
    public IReadOnlyList<string> Errors { get; }

    public static Result Success() => new(true, ResultErrorType.None, []);
    public static Result Invalid(IEnumerable<string> errors) => new(false, ResultErrorType.Validation, errors.ToList());
    public static Result Invalid(string error) => Invalid([error]);
    public static Result NotFound(string error = "Kayıt bulunamadı.") => new(false, ResultErrorType.NotFound, [error]);
    public static Result Conflict(string error) => Conflict([error]);
    public static Result Conflict(IEnumerable<string> errors) => new(false, ResultErrorType.Conflict, errors.ToList());
    public static Result TooManyRequests(string error) => new(false, ResultErrorType.TooManyRequests, [error]);
    public static Result Unauthorized(string error) => new(false, ResultErrorType.Unauthorized, [error]);
}

public class Result<T> : Result
{
    private Result(bool isSuccess, ResultErrorType errorType, IReadOnlyList<string> errors, T? value)
        : base(isSuccess, errorType, errors)
    {
        Value = value;
    }

    public T? Value { get; }

    public static Result<T> Success(T value) => new(true, ResultErrorType.None, [], value);
    public new static Result<T> Invalid(IEnumerable<string> errors) => new(false, ResultErrorType.Validation, errors.ToList(), default);
    public new static Result<T> Invalid(string error) => Invalid([error]);
    public new static Result<T> NotFound(string error = "Kayıt bulunamadı.") => new(false, ResultErrorType.NotFound, [error], default);
    public new static Result<T> Conflict(string error) => new(false, ResultErrorType.Conflict, [error], default);
    public new static Result<T> TooManyRequests(string error) => new(false, ResultErrorType.TooManyRequests, [error], default);
    public new static Result<T> Unauthorized(string error) => new(false, ResultErrorType.Unauthorized, [error], default);

    // Başarısız bir Result'ı (doğrulama, kontrol) aynı hata türü ve mesajlarla Result<T>'ye taşır
    public static Result<T> Failure(Result failure) =>
        failure.IsSuccess
            ? throw new ArgumentException("Başarılı sonuç Failure'a verilemez.", nameof(failure))
            : new(false, failure.ErrorType, failure.Errors, default);
}
