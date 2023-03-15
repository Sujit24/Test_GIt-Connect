using System;
using System.Collections.Generic;
using System.Data;
using NetTrackDBContext;
using NetTrackModel;
using System.Collections;

namespace NetTrackRepository
{
    public class ClientRepository
    {
        private DBClient _dbClient;
        private ClientModel _clientModel;
        private List<ClientModel> _clientModelList;

        // default constructor
        public ClientRepository()
        {
            _dbClient = new DBClient();
            _clientModel = new ClientModel();
            _clientModelList = new List<ClientModel>();
        }

     

        public void UpdateWebSession(ClientModel clientModel)
        {
            _dbClient = new DBClient();
            try
            {
                _dbClient.UpdateWebSession(clientModel);
            }
            catch (Exception exception) { 
                
            }
        }

        
       
        public UgsClientModel GetDetailClientInfo(UgsClientModel ugsClientModel)
        {
            //UgsClientModel ugsClientModel = null;
            DataTable dtClient = _dbClient.GetDetailClientInfo(ugsClientModel);

            if (dtClient.Rows.Count > 0)
            {
                System.Collections.ArrayList ar = RepositoryUtility.GetColumnList(dtClient);
                DataRow dr = dtClient.Rows[0];

                // ugsClientModel = new UgsClientModel();
                ugsClientModel.ClientID = Int32.Parse(dr["ClientID"].ToString());
                ugsClientModel.AccID = dr["AccID"].ToString();
                ugsClientModel.ClientName = dr["ClientName"].ToString();
                ugsClientModel.Address1 = dr["Address1"].ToString();
                ugsClientModel.Address2 = dr["Address2"].ToString();

                ugsClientModel.City = dr["City"].ToString();
                ugsClientModel.State = dr["State"].ToString();
                ugsClientModel.Zip = dr["Zip"].ToString();
                ugsClientModel.Fax = dr["Fax"].ToString();
                ugsClientModel.EMail = dr["EMail"].ToString();
                ugsClientModel.DateCreated = DateTime.Parse(dr["DateCreated"].ToString() == "" ? "1/1/1990" : dr["DateCreated"].ToString());

                ugsClientModel.ContactPhones = dr["ContactPhones"].ToString();
                ugsClientModel.ContactPerson = dr["ContactPerson"].ToString();
                ugsClientModel.ClientStatus = dr["ClientStatus"].ToString();

                ugsClientModel.DaysToKeepDataFor = dr.Field<int?>("DaysToKeepDataFor");
                ugsClientModel.Retention = Int32.Parse(dr["Retention"].ToString());
                ugsClientModel.Comments = dr["Comments"].ToString();
                ugsClientModel.ProxyClientID = Int32.Parse(dr["ProxyClientID"].ToString() == "" ? "0" : dr["ProxyClientID"].ToString());

                ugsClientModel.MapExpirationHours = Int32.Parse(dr["MapExpirationHours"].ToString());
                ugsClientModel.ShippingAddress1 = dr["ShippingAddress1"].ToString();
                ugsClientModel.ShippingAddress2 = dr["ShippingAddress2"].ToString();
                ugsClientModel.ShippingCity = dr["ShippingCity"].ToString();
                ugsClientModel.ShippingState = dr["ShippingState"].ToString();
                ugsClientModel.ShippingZip = dr["ShippingZip"].ToString();
                ugsClientModel.MobilePhone = dr["MobilePhone"].ToString();
                ugsClientModel.FTIN = dr["FTIN"].ToString();

                ugsClientModel.MapAccessPIN = dr["MapAccessPIN"].ToString();
                ugsClientModel.DataServicePIN = dr["DataServicePIN"].ToString();

                ugsClientModel.clienttype = dr["clienttype"].ToString();
                ugsClientModel.unitname = dr["unitname"].ToString();
                ugsClientModel.masterclientid = Int32.Parse(dr["masterclientid"].ToString());

                ugsClientModel.iaccessurl = dr["iaccessurl"].ToString();
                ugsClientModel.defaultradius = Int32.Parse(dr["defaultradius"].ToString());
                ugsClientModel.KML = dr["KML"].ToString();
                ugsClientModel.showkmlnames = Int32.Parse(dr["showkmlnames"].ToString());
                ugsClientModel.distanceunit = Int32.Parse(dr["distanceunit"].ToString());
                ugsClientModel.allowemergencypopup = Int32.Parse(dr["allowemergencypopup"].ToString());
                ugsClientModel.SubscriptionLevelId = Int32.Parse(dr["SubscriptionLevelId"].ToString() == "" ? "0" : dr["SubscriptionLevelId"].ToString());                 
                ugsClientModel.TYTVer = dr["TYTVer"].ToString();
                ugsClientModel.UsrQty = Convert.ToInt32(dr["UserQty"]);

                if (ugsClientModel.ClientID != 0)
                {
                    ugsClientModel.PasswordExpire = Int32.Parse(dr["PasswordExpire"].ToString());
                }
            }
            return ugsClientModel;
        }           

        public int SaveClient(UgsClientModel clientModel)
        {
            return _dbClient.SaveClient(clientModel);
        }

      

   
       
        public void getNewZohoContacts(UgsNewZohoContacts m)
        {
            DataTable dt = _dbClient.getNewZohoContacts(m);

            newZohoUser newUser = null;
            m.newUserList = new List<newZohoUser>();
            foreach (DataRow dr in dt.Rows)
            {
                newUser = new newZohoUser();
                newUser.LogIn = dr["login"].ToString();
                newUser.Pin = dr["pin"].ToString();
                m.newUserList.Add(newUser);
            }
        }
    }
}