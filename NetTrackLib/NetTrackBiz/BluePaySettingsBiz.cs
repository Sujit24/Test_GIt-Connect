using NetTrackModel;
using NetTrackRepository;

namespace NetTrackBiz
{
    internal class BluePaySettingsBiz
    {
        private BluePaySettingsRepository _BluePaySettingsRepository;

        public BluePaySettingsBiz()
        {
            _BluePaySettingsRepository = new BluePaySettingsRepository();
        }

        public BluePaySettingsModel GetBluePaySettingsInfo(string serverName)
        {
            return _BluePaySettingsRepository.GetBluePaySettingsInfo(serverName);
        }

        public BluePaySettingsModel SaveBluePaySettings(BluePaySettingsModel model)
        {
            return _BluePaySettingsRepository.SaveBluePaySettings(model);
        }

        public BluePayLogModel SaveBluePayLog(BluePayLogModel model)
        {
            return _BluePaySettingsRepository.SaveBluePayLog(model);
        }
    }
}
