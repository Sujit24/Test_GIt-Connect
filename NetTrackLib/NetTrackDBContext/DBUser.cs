using System;
using System.Data;
using System.Data.SqlClient;
using NetTrackModel;

namespace NetTrackDBContext
{
    public class DBUser : DBContext
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
        public DBUser()
        {
            this._dataReader = null;
            this._spParameters = null;
            this._dataTable = null;
            this._dataSet = null;
            this._spName = null;
        }

        #endregion Constructor

        #region DBUser public method

        public DataTable GetUserList(UserModel userModel)
        {
            _spName = "ug_employees";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetDetailUserInfo(UserModel userModel)
        {
            //_spName = "ug_employee";
            _spName = "ug_employee_NetTrack2"; 
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId),
												 new SqlParameter("@EmployeeID",userModel.EmployeeId)};

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetUserTypeList(UserModel userModel)
        {
            _spName = "ug_user_Type";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId), 
												 new SqlParameter("employeeid", userModel.EmployeeId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetTimeZoneList(UserModel userModel)
        {
            _spName = "ug_Time_Zone";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetMapZoomList(UserModel userModel)
        {
            _spName = "ug_preferences_zoom";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId), 
												 new SqlParameter("@zoom", DBNull.Value) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetMapCenterList(UserModel userModel)
        {
            _spName = "ug_preferences_mapcenter_admin";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId), 
												 new SqlParameter("@lat", DBNull.Value),
												new SqlParameter("@lon", DBNull.Value),
												new SqlParameter("@EmployeeID", userModel.EmployeeId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetFeatureList(UserModel userModel)
        {
            _spName = "ug_features";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId), 
												 new SqlParameter("@employeeid", userModel.EmployeeId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetSelectedFeatureList(UserModel userModel)
        {
            _spName = "ug_features_user";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId), 
												 new SqlParameter("@employeeid", userModel.EmployeeId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetPreferenceDetailInfo(UserModel userModel)
        {
            _spName = "ug_preferences_admin";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId),
												 new SqlParameter("@EmployeeID",userModel.EmployeeId)};

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetMapClassList(UserModel userModel)
        {
            _spName = "ug_map_classes_List";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId), 
												 new SqlParameter("@mapmode", "Navigator"),
												 new SqlParameter("@EmployeeID", userModel.EmployeeId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetContactTypeList(UserModel userModel)
        {
            _spName = "ug_contact_type";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public DataTable GetEmployeeContacts(UserModel userModel)
        {
            _spName = "ug_employee_contact_list";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId),
										        new SqlParameter("@EmployeeID",userModel.EmployeeId)};

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public int InsertEmployeeContact(UserModel userModel)
        {
            _spName = "us_employeecontact";
            _spParameters = new SqlParameter[]{
							    new SqlParameter("@sessionid", userModel.SessionId),
							    new SqlParameter("@action", "I"),
							    new SqlParameter("@employeeid", userModel.EmployeeId),
							    new SqlParameter("@contacttype", userModel.Contact.ContactType),
							    new SqlParameter("@contactvalue", userModel.Contact.ContactValue)
						};
            return ExecuteNoResult(_spName, _spParameters);
        }

        public int DeleteEmployeeContact(UserModel userModel)
        {
            _spName = "us_employeecontact";
            _spParameters = new SqlParameter[]{
							               new SqlParameter("@sessionid", userModel.SessionId),
										   new SqlParameter("@action", "D"),
										   new SqlParameter("@employeeid", userModel.EmployeeId),
										   new SqlParameter("@contactid", userModel.Contact.ContactId),
										   new SqlParameter("@contacttype", ""),		   
										   new SqlParameter("@contactvalue", "")
						};
            return ExecuteNoResult(_spName, _spParameters);
        }

        public int InsertFeatures(UserModel userModel)
        {
            _spName = "us_features_current";
            _spParameters = new SqlParameter[]{
							          new SqlParameter("@sessionid",userModel.SessionId),
									  new SqlParameter("@employeeid",userModel.EmployeeId),
									  new SqlParameter("@featurelist",userModel.SelectedFeatures)
						};
            return ExecuteNoResult(_spName, _spParameters);
        }

        public int DeleteFeatures(UserModel userModel)
        {
            _spName = "us_features_current";
            _spParameters = new SqlParameter[]{
							          new SqlParameter("@sessionid",userModel.SessionId),
									  new SqlParameter("@employeeid",userModel.EmployeeId),
									  new SqlParameter("@featurelist",userModel.SelectedFeatures)
						};
            return ExecuteNoResult(_spName, _spParameters);
        }

        public UserModel SaveEmployee(UserModel userModel)
        {
            SqlParameter spemp = new SqlParameter("@employeeid", userModel.EmployeeId);
            spemp.Direction = ParameterDirection.InputOutput;

            SqlParameter tokenurl = new SqlParameter("@urltoken", userModel.TokenURL);
            tokenurl.Direction = ParameterDirection.InputOutput;
            tokenurl.Size = 100;

            //_spName = "us_employee";
            _spName = "us_employee_NetTrack2";
            _spParameters = new SqlParameter[]{
							           new SqlParameter("@action",userModel.Action),
			                            spemp,
			                            new SqlParameter("@sessionid",userModel.SessionId),
			                            new SqlParameter("@employeetype",userModel.Type),
			                            new SqlParameter("@firstname",userModel.FirstName),					                      
			                            new SqlParameter("@lastname",userModel.LastName),
			                            new SqlParameter("@login",userModel.Login),
			                            new SqlParameter("@PN",userModel.Pin),
			                            new SqlParameter("@timezone", userModel.TimeZone),
			                            new SqlParameter("@email", userModel.Email),
                                        new SqlParameter("@cellphoneaddr", userModel.CellPhoneAddr),
                                        tokenurl
						};

            int result = ExecuteNoResult(_spName, _spParameters);


            if (userModel.Action == "I")
            {
                userModel.EmployeeId = Convert.ToInt32(spemp.Value);
            }

            userModel.TokenURL = tokenurl.Value.ToString();
            return userModel;
        }

        public int SavePreferences(UserModel userModel)
        {
            _spName = "us_preferences_admin";
            _spParameters = new SqlParameter[]{
							          new SqlParameter("@sessionid", userModel.SessionId),
									   new SqlParameter("@statuswindow", userModel.Preference.StatusWindow),
									   new SqlParameter("@legendwindow", userModel.Preference.LegendsWindow),
									   new SqlParameter("@scaledmap", userModel.Preference.MapSize),	 
									   new SqlParameter("@lat", String.Empty),
									   new SqlParameter("@lon", String.Empty),
									   new SqlParameter("@pointid", userModel.Preference.MapCenter),
									   new SqlParameter("@zoom", userModel.Preference.MapZoom),			   
									   new SqlParameter("@ChkCodes", userModel.Preference.MapClassses),
									   new SqlParameter("@ChkDescr", ""),
									   new SqlParameter("@PointsShowNum", userModel.Preference.MaxPointsDisplayed),
									   new SqlParameter("@gridpaging", userModel.Preference.AllowPaging),
									   new SqlParameter("@maprefresh", userModel.Preference.MapRefresh),
									   //new SqlParameter("@showvml", ""),
									   new SqlParameter("@showtruckclass", userModel.Preference.ShowClasses),
									   new SqlParameter("@primaryreportview", userModel.Preference.DefaultReportFormat),
									   new SqlParameter("@mainmap", userModel.Preference.PreferredMap),
                                       new SqlParameter("@winresizable", userModel.Preference.WindowResizable),
									 new SqlParameter("@employeeid", userModel.EmployeeId) 
						};
            return ExecuteNoResult(_spName, _spParameters);
        }

        public string GetGeneratedPassword(int SessionId)
        {
            SqlParameter sppass = new SqlParameter("@rslt", "");
            sppass.Direction = ParameterDirection.InputOutput;
            sppass.Size = 100;

            SqlParameter sptoken = new SqlParameter("@urltoken", "");
            sptoken.Direction = ParameterDirection.InputOutput;
            sptoken.Size = 100;

            _spName = "ug_InitPassword";
            _spParameters = new SqlParameter[]{
							          new SqlParameter("@sessionid", SessionId),
                                      sppass,
					                  sptoken  
						};
            ExecuteNoResult(_spName, _spParameters);

            return sppass.Value.ToString();
        }

        public  bool CheckAdministrativeRights(UserModel userModel,string strRoleID)
        {
            try
            {
                 _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId) };
                SqlDataReader dr = ExecuteReader("ug_adminrights",_spParameters);
                if (dr.Read())
                {
                    if (Convert.ToInt32(dr["haverights"].ToString()) >= Convert.ToInt32(strRoleID))
                    {
                        dr.Close();
                       
                        return true;
                    }
                    else
                    {
                        dr.Close();                        
                        return false;
                    }
                }
            }
            catch (Exception) { };
            return false;
        }

        public DataTable GetAdminMenu(UserModel userModel)
        {
            _spName = "ug_AdminMenu";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] { new SqlParameter("@sessionid", userModel.SessionId) };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }

        public int UpdateLegends(int SessionId, int EmployeeId, string legends)
        {
            _spName = "us_UpdateLegends_nettrack2";
            _spParameters = new SqlParameter[]{
							          new SqlParameter("@sessionid", SessionId),									   	   
									  new SqlParameter("@ChkCodes", legends),									  
									  new SqlParameter("@employeeid", EmployeeId) 
						};
            return ExecuteNoResult(_spName, _spParameters);
        }

        #endregion DBuser public method

        public string GetFeedUrl(UserModel userModel)
        {
            string strFeedURL = string.Empty;
            _spName = "ug_have_features";
            _spParameters = new SqlParameter[]{
							          new SqlParameter("@sessionid", userModel.SessionId)
						};

            SqlDataReader dr = ExecuteReader(_spName, _spParameters);
            if (dr.Read())
            {              
                strFeedURL = dr["feedurl"].ToString();   
            }
            dr.Close();
          

            return strFeedURL;
        }

        public DataTable GetTSSAdminList()
        {
            _spName = "ug_TSSAdmin";
            _dataTable = new DataTable();
            _spParameters = new SqlParameter[] {  };

            return _dataTable = ExecuteDataTable(_spName, _spParameters);
        }
    }
}
