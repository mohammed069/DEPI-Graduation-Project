namespace MissingPersons.BLL.Models;

public record Error(string Code, string Description) {
    public static readonly Error None = new(string.Empty, string.Empty);
}

public class Result {
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error) {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result Failure(string description) => new(false, new Error("Error", description));
}

public class Result<T> : Result {
    public T? Value { get; }

    private Result(bool isSuccess, T? value, Error error)
        : base(isSuccess, error) {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value, Error.None);
    public new static Result<T> Failure(Error error) => new(false, default, error);
    public new static Result<T> Failure(string description) => new(false, default, new Error("Error", description));
}