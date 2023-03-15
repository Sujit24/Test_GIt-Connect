using NetTrackModel;
using NetTrackRepository;
using System.Collections.Generic;

namespace NetTrackBiz
{
    public class TSSSettingsBiz
    {
        private TSSSettingsRepository _TSSSettingsRepository;

        public TSSSettingsBiz()
        {
            _TSSSettingsRepository = new TSSSettingsRepository();
        }

        public TSSSettings GetSettings(string settingsName)
        {
            return _TSSSettingsRepository.GetSettings(settingsName);
        }

        public List<TSSSettings> GetAllSettings()
        {
            return _TSSSettingsRepository.GetAllSettings();
        }

        public void SetSettings(TSSSettings model)
        {
            _TSSSettingsRepository.SetSettings(model);
        }
    }
}
