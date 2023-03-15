using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetTrackModel;
using NetTrackDBContext;
using System.Data;

namespace NetTrackRepository
{
    public class MyAccountRepository
    {
        private DBMyAccount _dbMyAccount;

        public MyAccountRepository()
        {
            _dbMyAccount = new DBMyAccount();
        }

        public MyAccountModel GetMyAccountInfo(int clientID)
        {
            MyAccountModel myAccountModel = new MyAccountModel();
            DataTable dtAccount = _dbMyAccount.GetMyAccountInfo(clientID);

            if (dtAccount.Rows.Count > 0)
            {
                DataRow dr = dtAccount.Rows[0];

                myAccountModel.MyAccountId = dr["MyAccountId"] == DBNull.Value ? 0:  Convert.ToInt32(dr["MyAccountId"]);
                myAccountModel.ClientId = dr["ClientID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ClientID"]);
                myAccountModel.ClientName = dr["ClientName"] == DBNull.Value ? "" : Convert.ToString(dr["ClientName"]);
                myAccountModel.CardNumber = dr["CardNumber"] == DBNull.Value ? "" : Convert.ToString(dr["CardNumber"]);
                myAccountModel.CVV2 = dr["CVV2"] == DBNull.Value ? "" : Convert.ToString(dr["CVV2"]);
                myAccountModel.CardExpireYear = dr["CardExpireYear"] == DBNull.Value ? "" : Convert.ToString(dr["CardExpireYear"]);
                myAccountModel.CardExpireMonth = dr["CardExpireMonth"] == DBNull.Value ? "" : Convert.ToString(dr["CardExpireMonth"]);
                myAccountModel.CardHolderFirstName = dr["CardHolderFirstName"] == DBNull.Value ? "" : Convert.ToString(dr["CardHolderFirstName"]);
                myAccountModel.CardHolderLastName = dr["CardHolderLastName"] == DBNull.Value ? "" : Convert.ToString(dr["CardHolderLastName"]);
                myAccountModel.Country = dr["Country"] == DBNull.Value ? "" : Convert.ToString(dr["Country"]);
                myAccountModel.Address = dr["Address"] == DBNull.Value ? "" : Convert.ToString(dr["Address"]);
                myAccountModel.City = dr["City"] == DBNull.Value ? "" : Convert.ToString(dr["City"]);
                myAccountModel.State = dr["STATE"] == DBNull.Value ? "" : Convert.ToString(dr["STATE"]);
                myAccountModel.ZipCode = dr["CardNumber"] == DBNull.Value ? "" : Convert.ToString(dr["ZipCode"]);
            }

            return myAccountModel;
        }

        public int SaveMyAccountInfo(MyAccountModel myAccountModel)
        {
            return _dbMyAccount.SaveMyAccountInfo(myAccountModel);
        }
    }
}
