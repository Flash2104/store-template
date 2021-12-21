namespace StoreApi.Models;

public class ServerResponseDto
{
    public ServerResponseDto()
    {
        Errors = new List<ErrorDto>();
    }

    public ServerResponseDto(ErrorDto? error)
    {
        Errors = new List<ErrorDto>();

        if (error == null)
            return;

        Errors.Add(error);
        IsSuccess = false;
    }

    /// <summary>
    /// for many errors
    /// </summary>
    public List<ErrorDto>? Errors { get; set; }

    /// <summary>
    /// technical message
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// true = good, false = error
    /// </summary>
    public bool IsSuccess { get; set; }
}

/// <inheritdoc />
public class ServerResponseDto<T> : ServerResponseDto
{
    public ServerResponseDto() : base()
    {
    }

    public ServerResponseDto(ErrorDto? error) : base(error)
    {
    }

    public ServerResponseDto(ErrorDto? error, string? message) : base(error)
    {
        Message = message;
    }

    public ServerResponseDto(List<ErrorDto>? errors) : base()
    {
        if (errors != null)
            Errors = errors;
    }

    public ServerResponseDto(List<ErrorDto>? errors, string? message) : base()
    {
        Errors = errors;
        Message = message;
    }

    public ServerResponseDto(ServerResponseDto response) : base()
    {
        IsSuccess = response.IsSuccess;
        Errors = response.Errors;
        Message = response.Message;
    }

    public ServerResponseDto(T? data) : base()
    {
        Data = data;
        IsSuccess = true;
    }

    public T? Data { get; set; }
}

/// <inheritdoc />
public class ServerResponseDto<T, TError> : ServerResponseDto
{
    public ServerResponseDto() : base()
    {
    }

    public ServerResponseDto(ErrorDto? error) : base(error)
    {
        Errors = new List<ErrorDto<TError>>() { new ErrorDto<TError>(error) };
    }

    public ServerResponseDto(ErrorDto<TError>? error) : base(error)
    {
        Errors = error != null ? new List<ErrorDto<TError>>() { error } : null;
    }

    public ServerResponseDto(ErrorDto<TError>? error, string message) : base(error)
    {
        Message = message;
        Errors = error != null ? new List<ErrorDto<TError>>() { error } : null;
    }

    public ServerResponseDto(List<ErrorDto>? errors) : base()
    {
        if (errors != null)
        {
            Errors = errors.Select(e => new ErrorDto<TError>(e)).ToList();
        }
    }

    public ServerResponseDto(List<ErrorDto<TError>>? errors) : base()
    {
        if (errors != null)
        {
            Errors = errors.ToList();
        }
    }

    public ServerResponseDto(List<ErrorDto<TError>>? errors, string? message) : base()
    {
        if (errors != null)
            Errors = errors.ToList();
        Message = message;
    }

    public ServerResponseDto(ServerResponseDto<T, TError>? response) : base()
    {
        IsSuccess = response?.IsSuccess ?? false;
        Errors = response?.Errors?.ToList();
        Message = response?.Message;
    }

    public ServerResponseDto(ServerResponseDto<T, TError>? response, T data) : base()
    {
        IsSuccess = response?.IsSuccess ?? false;
        Errors = response?.Errors?.ToList();
        Message = response?.Message;
        Data = data;
    }

    public T? Data { get; set; }

    /// <summary>
    /// for many errors
    /// </summary>
    public new List<ErrorDto<TError>>? Errors { get; set; }
}