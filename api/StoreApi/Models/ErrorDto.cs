namespace StoreApi.Models;

public class ErrorDto
{
    public ErrorDto()
    {
    }

    public ErrorDto(ErrorDto? error)
    {
        Code = error?.Code ?? 0;
        Message = error?.Message;
    }

    public ErrorDto(int code, string? msg)
    {
        Code = code;
        Message = msg;
    }

    public int Code { get; set; }

    public string? Message { get; set; }
}

public class ErrorDto<T> : ErrorDto
{
    public T? Data { get; }

    public ErrorDto(ErrorDto? error)
    {
        Code = error?.Code ?? 0;
        Message = error?.Message;
    }

    public ErrorDto(ErrorDto error, T data)
    {
        Code = error.Code;
        Message = error.Message;
        Data = data;
    }
}