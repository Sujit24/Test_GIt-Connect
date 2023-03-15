using System.Data;
using System.Data.SqlClient;
using NetTrackModel;
using System;

namespace NetTrackDBContext
{
    public class DBClient : DBContext
    {
        #region private property

        private SqlDataReader _dataReader;
        private SqlParameter[] _spParameters;
        private DataTable _dataTable;
        private DataSet _dataSet;
        private string _spName;

        #endregion private property

        #region Constructor

        // default constructor
        public DBClient()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion Constructor

        #region DBLogin public method
       
        public void UpdateWebSession(ClientModel clientModel)
        {
            _spName = "us_client_view";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
							  new SqlParameter("@sessionid",clientModel.SessionID),
                              new SqlParameter("@viewclientid", clientModel.ClientID)
						};
            int rslt = ExecuteNoResult(_spName, _spParameters);
        }

       

        public DataTable GetDetailClientInfo(UgsClientModel clientModel)
        {
            _spName = "ug_client_nettrack2_usrqty";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[]{
                                                new SqlParameter("@sessionid", clientModel.SessionId),
                                                new SqlParameter("@clientid", clientModel.ClientID)
                                              };
            _dataTable = ExecuteDataTable(_spName, _spParameters);
            return _dataTable;
        }

       

    
       

     

     

        public int SaveClient(UgsClientModel ClientModel)
        {
            _spName = "us_logon_nettrack2";
            _dataSet = new DataSet();
            _spParameters = new SqlParameter[]{
							  new SqlParameter("@login", "TSS"),
							  new SqlParameter("@pin", "QGRQX6"),
							  new SqlParameter("@newpin", ""),
						};
            _dataSet = ExecuteDataSet(_spName, _spParameters);
            ClientModel.SessionId = int.Parse(_dataSet.Tables[0].Rows[0]["sessionid"].ToString());

            _spName = "us_client_nettrack2_withNewUserNOrderId";
 			SqlParameter sptemp = new SqlParameter("@ClientID", SqlDbType.Int);
            sptemp.Value = ClientModel.ClientID;
            sptemp.Direction = ParameterDirection.InputOutput;
            _spParameters = new SqlParameter[]{
									  new SqlParameter("@sessionid",ClientModel.SessionId),
                                      new SqlParameter("@AccID",ClientModel.AccID),
									  new SqlParameter("@action",ClientModel.action),
									  /*new SqlParameter("@ClientID",ClientModel.ClientID)*/ sptemp,
									  new SqlParameter("@ClientName",ClientModel.ClientName),
									  new SqlParameter("@DateCreated",ClientModel.DateCreated.ToShortDateString()),
									  new SqlParameter("@address1",ClientModel.Address1),
									  new SqlParameter("@address2",ClientModel.Address2),
									  new SqlParameter("@City",ClientModel.City),
									  new SqlParameter("@State",ClientModel.State),
									  new SqlParameter("@Zip",ClientModel.Zip),
									  new SqlParameter("@shippingaddress1",ClientModel.ShippingAddress1), // txtShippingAddress_1.Text
									  new SqlParameter("@shippingaddress2",ClientModel.ShippingAddress2), //txtShippingAddress_2.Text
									  new SqlParameter("@shippingCity",ClientModel.ShippingCity), //txtShippingCity.Text
									  new SqlParameter("@shippingState",ClientModel.ShippingState), //txtShippingState.Text
									  new SqlParameter("@shippingZip",ClientModel.ShippingZip), //txtShippingZip.Text
									  new SqlParameter("@EMail",ClientModel.EMail),
									  new SqlParameter("@Phones",ClientModel.ContactPhones),
									  new SqlParameter("@Fax",ClientModel.Fax),
									  new SqlParameter("@ContactPerson",ClientModel.ContactPerson),
									  new SqlParameter("@Comments",ClientModel.Comments),
									  new SqlParameter("@ClientStatus",ClientModel.ClientStatus),
									  new SqlParameter("@retention",ClientModel.Retention),
									  new SqlParameter("@ProxyClientID",ClientModel.ProxyClientID),
									  new SqlParameter("@MapExpirationHours",ClientModel.MapExpirationHours),
									  new SqlParameter("@Mobile",ClientModel.MobilePhone),
									  new SqlParameter("@FTIN",ClientModel.FTIN),
									  new SqlParameter("@MapAccessPIN",ClientModel.MapAccessPIN),
									  new SqlParameter("@DataServicePIN",ClientModel.DataServicePIN),
									  new SqlParameter("@password_expire",ClientModel.PasswordExpire),
									  new SqlParameter("@clienttype",ClientModel.clienttype),
									  new SqlParameter("@unitname",ClientModel.unitname),
                                      new SqlParameter("@homeLat",string.Empty),
                                      new SqlParameter("@homeLon",string.Empty),
                                      new SqlParameter("@masterclientid",ClientModel.masterclientid),
                                      new SqlParameter("@defaultradius",ClientModel.defaultradius),
                                      new SqlParameter("@KML",ClientModel.KML),
                                      new SqlParameter("@showkmlnames",ClientModel.showkmlnames),
                                      new SqlParameter("@allowemergencypopup",ClientModel.allowemergencypopup),
                                      new SqlParameter("@distanceunit", ClientModel.distanceunit),
                                      new SqlParameter("@SubscriptionLevelId", ClientModel.SubscriptionLevelId),
                                      new SqlParameter("@TYTVer", ClientModel.TYTVer),
                                      new SqlParameter("@DaysToKeepDataFor", ClientModel.DaysToKeepDataFor),
									  new SqlParameter("@UserQty", ClientModel.UsrQty),
                                      new SqlParameter("@ZohoAccID", ClientModel.ZohoAccountID),
                                      new SqlParameter("@ZohoContactID", ClientModel.ZohoContactID),
                                      new SqlParameter("@ZohoUserTz", ClientModel.ZohoUserTz),
                                      new SqlParameter("@OrderId",ClientModel.OrderId)
								  };

            int result = ExecuteNoResult(_spName, _spParameters);
           // return ExecuteNonQuery(CommandType.StoredProcedure, _spName, _spParameters);

            ClientModel.ClientID = Convert.ToInt32(sptemp.Value);

            return ClientModel.ClientID;
        }

      


        

        public DataTable getNewZohoContacts(UgsNewZohoContacts m)
        {
            _spName = "ug_new_Zoho_Users";
            _spParameters = new SqlParameter[] {new SqlParameter("@ZohoAccID", m.ZohoAccID)};

            return ExecuteDataTable(_spName, _spParameters);
        }

        #endregion DBLogin public method
    }
}