using Store.Service.Common;

namespace Store.Service.Contracts
{
    public interface IConfigService
    {
        AppSettings GetSettings();
    }
}
