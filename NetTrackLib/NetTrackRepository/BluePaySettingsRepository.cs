using System;
using System.Data;
using NetTrackDBContext;
using NetTrackModel;

namespace NetTrackRepository
{
    public class BluePaySettingsRepository
    {
        private BluePaySettingsModel _BluePaySettingsModel;
        private DBBluePaySettings _DBBluePaySettings;

        public BluePaySettingsRepository()
        {
            this._DBBluePaySettings = new DBBluePaySettings();
        }

        public BluePaySettingsModel GetBluePaySettingsInfo(string serverName)
        {
            DataTable dtBluePaySettings = _DBBluePaySettings.GetBluePaySettingsInfo(serverName);
            foreach (DataRow dr in dtBluePaySettings.Rows)
            {
                _BluePaySettingsModel = new BluePaySettingsModel();
                _BluePaySettingsModel.ServerName = dr["ServerName"].ToString();
                _BluePaySettingsModel.AccountName = dr["AccountName"].ToString();
                _BluePaySettingsModel.AccountId = dr["AccountId"].ToString();
                _BluePaySettingsModel.SecretKey = dr["SecretKey"].ToString();
                _BluePaySettingsModel.CurrentMode = dr["CurrentMode"].ToString();
                _BluePaySettingsModel.LiveUrl = dr["LiveUrl"] == DBNull.Value ? "" : dr["LiveUrl"].ToString();
                _BluePaySettingsModel.TestUrl = dr["TestUrl"] == DBNull.Value ? "" : dr["TestUrl"].ToString();
            }

            return _BluePaySettingsModel;
        }

        public BluePaySettingsModel SaveBluePaySettings(BluePaySettingsModel model)
        {
            return _DBBluePaySettings.SaveBluePaySettings(model); ;
        }

        public BluePayLogModel SaveBluePayLog(BluePayLogModel model)
        {
            return _DBBluePaySettings.SaveBluePayLog(model); ;
        }
    }
}
