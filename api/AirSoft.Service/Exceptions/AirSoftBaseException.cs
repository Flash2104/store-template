
namespace Store.Service.Exceptions;

public class AirSoftBaseException : ApplicationException
{
    public AirSoftBaseException(int code, string message, string logPath = "")
        : base(message)
    {
        Code = code;
        LogPath = logPath;
    }

    public int Code { get; }

    public string LogPath { get; }
}
