using AirSoft.Service.Common;
using AirSoft.Service.Contracts;

namespace AirSoft.Service.Implementations
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
