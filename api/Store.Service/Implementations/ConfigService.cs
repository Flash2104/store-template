using Store.Service.Common;
using Store.Service.Contracts;

namespace Store.Service.Implementations
{
    public class ConfigService : IConfigService
    {
        private readonly AppSettings _appSettings;

        public ConfigService(AppSettings settings)
        {
            _appSettings = settings;
        }

        public AppSettings GetSettings()
        {
            return _appSettings;
        }
    }
}
