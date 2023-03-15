using NetTrackDBContext;
using NetTrackModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace NetTrackRepository
{
    public class TSSSettingsRepository
    {
        private TSSSettings _TSSSettings;
        private DBTSSSettings _DBTSSSettings;

        public TSSSettingsRepository()
        {
            this._DBTSSSettings = new DBTSSSettings();
        }

        public TSSSettings GetSettings(string settingsName)
        {
            DataTable dtTSSSettings = _DBTSSSettings.GetSettings(settingsName);

            foreach (DataRow dr in dtTSSSettings.Rows)
            {
                _TSSSettings = new TSSSettings();
                _TSSSettings.SettingsName = dr["SettingsName"].ToString();
                _TSSSettings.SettingsValue = dr["SettingsValue"] == DBNull.Value ? "" : dr["SettingsValue"].ToString();
            }

            return _TSSSettings;
        }

        public List<TSSSettings> GetAllSettings()
        {
            List<TSSSettings> settings = new List<TSSSettings>();
            DataTable dtTSSSettings = _DBTSSSettings.GetSettings("");

            foreach (DataRow dr in dtTSSSettings.Rows)
            {
                _TSSSettings = new TSSSettings();
                _TSSSettings.SettingsName = dr["SettingsName"].ToString();
                _TSSSettings.SettingsValue = dr["SettingsValue"] == DBNull.Value ? "" : dr["SettingsValue"].ToString();

                settings.Add(_TSSSettings);
            }

            return settings;
        }

        public void SetSettings(TSSSettings model)
        {
            _DBTSSSettings.SetSettings(model);
        }

    }
}
