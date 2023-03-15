using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NetTrackDBContext;
using NetTrackModel;
using System.Collections;

namespace NetTrackRepository
{
    public class UserRepository
    {
        private UserModel _userModel;
        private DBLogin _dbLogin;
        private DBUser _dbUser;
        UserModel _userModelDetails;

        // default constructor
        public UserRepository()
        {
            _userModel = new UserModel();
            _dbLogin = new DBLogin();
            _userModelDetails = new UserModel();
        }

        public UserModel GetUserInfo(UserModel userModel)
        {
            // Create an object of LoginBiz class
            _userModel = new UserModel();
            _dbLogin = new DBLogin();
            try
            {
                DataTable dtUser = _dbLogin.GetUserInfo(userModel);
                foreach (DataRow dr in dtUser.Rows)
                {
                    _userModel.ClientId = Int32.Parse(dr["clientid"].ToString());
                    _userModel.EmployeeId = Int32.Parse(dr["employeeid"].ToString());
                    _userModel.SessionId = Int32.Parse(dr["sessionid"].ToString());
                    _userModel.ErrorId = Int32.Parse(dr["errorid"].ToString());
                    _userModel.WinResizable = Int32.Parse(dr["winresizable"].ToString());
                    _userModel.UserName = dr["username"].ToString();
                    _userModel.EmployeeName = dr["emp_name"].ToString();
                    _userModel.ErrorDescription = dr["errorDescr"].ToString();
                    _userModel.FirstUrl = dr["firsturl"].ToString();
                    _userModel.FirstName = dr["firstname"].ToString();
                    _userModel.UnitName = dr["unitname"].ToString();

                }
            }
            catch (Exception exception) { }

            return _userModel;
        }

        public UserModel GetUserLoginInfo(UserModel userModel)
        {
            _userModel = new UserModel();
            _dbLogin = new DBLogin();
            try
            {
                SqlDataReader dr = _dbLogin.GetUserLoginInfo(userModel);
                if (dr.Read())
                {
                    userModel.Email = dr["useremail"].ToString();
                    userModel.FirstName = dr["first_name"].ToString();
                    userModel.LastName = dr["last_name"].ToString();
                    userModel.Login = dr["login"].ToString();
                    userModel.Pin = dr["pin"].ToString();
                }
            }
            catch (Exception exception)
            { }
            return userModel;

        }

        public UserModel GetUserDetailInfo(UserModel userModel)
        {
            // Create an object of user model list
            _userModelDetails = new UserModel();
            _dbLogin = new DBLogin();
            try
            {
                DataTable dsUser = _dbLogin.GetUserDetailInfo(userModel);

                if (dsUser.Rows.Count > 0)
                {
                    _userModelDetails.EmployeeId = Int32.Parse(dsUser.Rows[0]["EmployeeID"].ToString());
                    _userModelDetails.ClientId = Int32.Parse(dsUser.Rows[0]["ClientId"].ToString());
                    _userModelDetails.Login = dsUser.Rows[0]["Login"].ToString();
                    _userModelDetails.Pin = dsUser.Rows[0]["PIN"].ToString();
                    _userModelDetails.Email = dsUser.Rows[0]["useremail"] == DBNull.Value ? "" : dsUser.Rows[0]["useremail"].ToString();
                    _userModelDetails.HaveRemoteControl = dsUser.Rows[0]["HaveRemoteControl"].ToString();
                    _userModelDetails.IsTSSAdmin = Convert.ToBoolean(dsUser.Rows[0]["IsTSSAdmin"]);
                    _userModelDetails.IsTSSUser = Convert.ToBoolean(dsUser.Rows[0]["IsTSSUser"]);
                    _userModelDetails.IsTSSQuoteApproval = Convert.ToBoolean(dsUser.Rows[0]["IsTSSQuoteApproval"]);
                    _userModelDetails.SessionId = 0;

                    if (_userModelDetails.IsTSSUser)
                    {
                        _userModelDetails.Roles = new string[] { "TSSUser" };
                    }
                    else if (_userModelDetails.IsTSSAdmin)
                    {
                        _userModelDetails.Roles = new string[] { "TSSAdmin" };
                    }
                }
            }
            catch { }

            return _userModelDetails;
        }

        public UserModel GetUserModel(DataSet dsUser)
        {
            _dbUser = new DBUser();

            /*
            //get subscription model and set it to user model
            SubscriptionLevelModel subscriptionLevelModel = new SubscriptionLevelModel();
            subscriptionLevelModel.ClientID = Int32.Parse(dsUser.Tables[0].Rows[0]["clientid"].ToString());
            int SubscriptionLevelId = 0;
            try
            {
                SubscriptionLevelId = int.Parse(new SubscriptionLevelRepository().GetSubscriptionLevel(subscriptionLevelModel).subscriptionLevelId);
            }
            catch (Exception) { }
            */
            if (dsUser.Tables.Count > 0)
            {
                int SubscriptionLevelId = 0;
                if (dsUser.Tables.Count >= 3)
                {
                    try
                    {
                        DataTable dtSubscription = dsUser.Tables[3];
                        foreach (DataRow dr in dtSubscription.Rows)
                        {
                            SubscriptionLevelId = Convert.ToInt32(dr["SubscriptionLevelId"]);
                        }
                    }
                    catch (Exception ex) { }
                }

                DataTable dtUserFeatures = new DataTable();
                List<string> _roles = new List<string>();
                int role = 0;

                for (int i = 0; i < dsUser.Tables.Count; i++)
                {
                    // first table user basic information
                    if (i == 0)
                    {
                        // user info table
                        DataTable dtUser = dsUser.Tables[i];
                        try
                        {
                            foreach (DataRow dr in dtUser.Rows)
                            {
                                _userModel = new UserModel(
                                                Int32.Parse(dr["clientid"].ToString()),
                                                Int32.Parse(dr["employeeid"].ToString()),
                                                Int32.Parse(dr["sessionid"].ToString()),
                                                Int32.Parse(dr["errorid"].ToString()),
                                                Int32.Parse(dr["winresizable"].ToString()),
                                                dr["username"].ToString(),
                                                dr["emp_name"].ToString(),
                                                dr["errorDescr"].ToString(),
                                                dr["firsturl"].ToString(),
                                                dr["firstname"].ToString(),
                                                dr["unitname"].ToString()
                                                );
                                //userModelList.Add(_userModel);
                            }
                            //set subscription level 
                            _userModel.SubscriptionLevelId = SubscriptionLevelId;
                            _userModel.EmployeeClientId = _userModel.ClientId;
                        }
                        catch (Exception exception) { }
                    }

                    // second table user features detail
                    if (i == 1)
                    {
                        // user feature table
                        dtUserFeatures = dsUser.Tables[i];
                        try
                        {
                            //List<string> _roles = new List<string>();
                            foreach (DataRow dr in dtUserFeatures.Rows)
                            {
                                role = Convert.ToInt32(dr["HaveReports"]);

                                if (role > 0) _roles.Add("HaveReports");


                                role = Convert.ToInt32(dr["HaveOptions"]);
                                if (role > 0) _roles.Add("HaveOptions");

                                if (_userModel.SubscriptionLevelId != 1 && _userModel.SubscriptionLevelId != 2)
                                {
                                    role = Convert.ToInt32(dr["HaveMessaging"]);
                                    if (role > 0) _roles.Add("HaveMessaging");
                                }

                                if (_userModel.SubscriptionLevelId != 2 && _userModel.SubscriptionLevelId != 3)
                                {
                                    role = Convert.ToInt32(dr["HaveMapReplay"]);
                                    if (role > 0) _roles.Add("HaveMapReplay");
                                }


                                if (!string.IsNullOrEmpty(dr["feedurl"].ToString())) _roles.Add("feedurl");


                                role = Convert.ToInt32(dr["haveP2S2status"]);
                                if (role > 0) _roles.Add("haveP2S2status");

                                role = string.IsNullOrEmpty(dr["ishomepoint"].ToString()) ? 0 : Convert.ToInt32(dr["ishomepoint"]);
                                if (role > 0) _roles.Add("ishomepoint");

                                role = Convert.ToInt32(dr["HaveLiteAdmin"]);
                                if (role > 0) _roles.Add("HaveLiteAdmin");

                                role = Convert.ToInt32(dr["haveUserPropstatus"]);
                                if (role > 0) _roles.Add("haveUserPropstatus");

                                role = Convert.ToInt32(dr["HavePolygonSave"]);
                                if (role > 0) _roles.Add("HavePolygonSave");

                                role = Convert.ToInt32(dr["HaveGoogleEarth"]);
                                if (role > 0) _roles.Add("HaveGoogleEarth");

                                role = Convert.ToInt32(dr["HaveChat"]);
                                if (role > 0) _roles.Add("HaveChat");

                                role = Convert.ToInt32(dr["haveopenstreet"]);
                                if (role > 0) _roles.Add("haveopenstreet");

                                role = Convert.ToInt32(dr["HaveReports60"]);
                                if (role > 0) _roles.Add("HaveReports60");


                                role = Convert.ToInt32(dr["HaveReportsOld"]);
                                if (role > 0) _roles.Add("HaveReportsOld");


                                role = Convert.ToInt32(dr["HaveRemoteControl"]);
                                if (role > 0) _roles.Add("HaveRemoteControl");


                                role = Convert.ToInt32(dr["HaveCustomMessage"]);
                                if (role > 0) _roles.Add("HaveCustomMessage");

                                role = Convert.ToInt32(dr["HaveAdmin"]);
                                if (role > 0) _roles.Add("HaveAdmin");

                                role = Convert.ToInt32(dr["HaveClientAdmin"]);
                                if (role > 0) _roles.Add("HaveClientAdmin");

                                role = Convert.ToInt32(dr["HaveCemtekOrP2S2Admin"]);
                                if (role > 0) _roles.Add("HaveCemtekOrP2S2Admin");

                                role = Convert.ToInt32(dr["HaveGoogleRouting"]);
                                if (role > 0) _roles.Add("HaveGoogleRouting");


                                //userModelList.Add(_userModel);
                            }

                            //if (CheckAdministrativeRights(_userModel, "3")) { _roles.Add("HaveAdmin"); }
                            //if (CheckAdministrativeRights(_userModel, "1")) { _roles.Add("HaveClientAdmin"); }
                            //if (CheckAdministrativeRights(_userModel, "2")) { _roles.Add("HaveCemtekOrP2S2Admin"); }
                            /*
                            DataTable dt = _dbUser.GetAdminMenu(_userModel);

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                //twAdminMenu.Nodes.Add(CreateNode(dt.Rows[j])); row["value"].
                                _roles.Add(dt.Rows[j]["value"].ToString());
                            }

                            if (_userModel.SubscriptionLevelId ==1)
                            {
                                _roles.Remove("Classes");
                            }

                            if (dtUserFeatures.Rows.Count > 0)
                            {
                                _roles.Add("HaveFeatures");
                            }

                            // check role for preventive maintenance
                            _roles.Add("HavePreventiveMaintenance");
                            if (_userModel.SubscriptionLevelId == 1)
                            {
                                _roles.Remove("HavePreventiveMaintenance"); 
                            }
                            _userModel.Roles = _roles.ToArray();
                            */
                        }
                        catch (Exception exception) { }
                    }
                    if (i == 2)
                    {
                        // user feature table
                        DataTable dt = dsUser.Tables[i];
                        try
                        {
                            //List<string> _roles = new List<string>();
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                //twAdminMenu.Nodes.Add(CreateNode(dt.Rows[j])); row["value"].
                                _roles.Add(dt.Rows[j]["value"].ToString());
                            }

                            if (_userModel.SubscriptionLevelId == 1)
                            {
                                _roles.Remove("Classes");
                            }

                            if (dtUserFeatures.Rows.Count > 0)
                            {
                                _roles.Add("HaveFeatures");
                            }

                            // check role for preventive maintenance
                            _roles.Add("HavePreventiveMaintenance");
                            if (_userModel.SubscriptionLevelId == 1)
                            {
                                _roles.Remove("HavePreventiveMaintenance");
                            }
                        }
                        catch (Exception exception) { }
                    }
                }
                if (_roles.Count > 0)
                {
                    _userModel.Roles = _roles.ToArray();
                }
            }

            return _userModel;
        }

        public List<UserModel> GetUserList(UserModel userModel)
        {
            _dbUser = new DBUser();
            UserModel userModelLocal = null;
            List<UserModel> userModelList = new List<UserModel>();

            DataTable dtUser = _dbUser.GetUserList(userModel);

            foreach (DataRow dr in dtUser.Rows)
            {
                userModelLocal = new UserModel();
                userModelLocal.EmployeeId = Int32.Parse(dr["employeeid"].ToString());
                userModelLocal.Type = dr["Type"].ToString();
                userModelLocal.LastName = dr["Last Name"].ToString();
                userModelLocal.FirstName = dr["First Name"].ToString();
                userModelLocal.Login = dr["Login"].ToString();

                userModelList.Add(userModelLocal);
            }

            return userModelList;
        }
        public UgUserModelWrapper GetUserList(UgUserModel ugUserModel)
        {
            _dbUser = new DBUser();

            UgUserModelWrapper userModelWrapper = new UgUserModelWrapper();
            List<UgUserModel> userModelList = new List<UgUserModel>();
            UserModel usermodel = new UserModel();
            usermodel.SessionId = ugUserModel.SessionId;
            DataTable dtUser = _dbUser.GetUserList(usermodel);
            ArrayList ar = RepositoryUtility.GetColumnList(dtUser);
            userModelWrapper.GridColumnList = ar;
            foreach (DataRow dr in dtUser.Rows)
            {
                UgUserModel ugsUserModel = new UgUserModel();
                ugsUserModel.EmployeeId = ar.Contains("employeeid") ? Convert.ToInt32(dr["employeeid"].ToString()) : 0;
                ugsUserModel.FirstName = ar.Contains("First Name") ? dr["First Name"].ToString() : "";
                ugsUserModel.LastName = ar.Contains("Last Name") ? dr["Last Name"].ToString() : "";
                ugsUserModel.Type = ar.Contains("Type") ? dr["Type"].ToString() : "";
                ugsUserModel.Login = ar.Contains("Login") ? dr["Login"].ToString() : "";
                userModelList.Add(ugsUserModel);
            }
            userModelWrapper.userModelList = userModelList;
            return userModelWrapper;
        }
        public UserModel GetDetailUserInfo(UserModel userModel)
        {
            _dbUser = new DBUser();
            UserModel userModelLocal = new UserModel();

            DataTable dtUser = _dbUser.GetDetailUserInfo(userModel);

            if (dtUser.Rows.Count > 0)
            {
                DataRow dr = dtUser.Rows[0];

                userModelLocal.EmployeeId = Int32.Parse(dr["EmployeeID"].ToString());
                userModelLocal.ClientId = Int32.Parse(dr["ClientID"].ToString());
                userModelLocal.FirstName = dr["First_Name"].ToString();
                userModelLocal.LastName = dr["Last_Name"].ToString();
                userModelLocal.Login = dr["Login"].ToString();
                userModelLocal.Pin = dr["PIN"].ToString();
                userModelLocal.Type = dr["EmployeeType"].ToString();
                userModelLocal.TimeZone = dr["timezone"].ToString();
                userModelLocal.Email = dr["useremail"].ToString();
                userModelLocal.CellPhoneAddr = dr["CellphoneAddr"].ToString();
                userModelLocal.TokenURL = dr["tokenurl"].ToString();
            }

            return userModelLocal;
        }

        public PreferenceModel GetPreferenceDetailInfo(UserModel userModel)
        {
            _dbUser = new DBUser();
            PreferenceModel preferenceModel = new PreferenceModel();

            DataTable dtUser = _dbUser.GetPreferenceDetailInfo(userModel);

            if (dtUser.Rows.Count > 0)
            {
                DataRow dr = dtUser.Rows[0];

                preferenceModel.PreferredMap = Int32.Parse(dr["MainMap"].ToString());
                preferenceModel.MapSize = Int32.Parse(dr["ScaledMap"].ToString());
                preferenceModel.MapZoom = dr["DefaultZoom"].ToString();
                preferenceModel.MapRefresh = Int32.Parse(dr["MapRefresh"].ToString());
                preferenceModel.MapCenter = Int32.Parse(dr["DefaultCenter"].ToString());
                preferenceModel.LegendsWindow = Int32.Parse(dr["LegendWindow"].ToString());
                preferenceModel.StatusWindow = Int32.Parse(dr["StatusWindow"].ToString());
                preferenceModel.MaxPointsDisplayed = dr["PointsNumber"].ToString();
                preferenceModel.AllowPaging = dr["gridpaging"].ToString() == "False" ? 0 : 1;
                preferenceModel.DefaultReportFormat = dr["PrimaryReportView"].ToString();
                preferenceModel.ShowClasses = Int32.Parse(dr["showTruckClass"].ToString());
                preferenceModel.WindowResizable = Int32.Parse(dr["winresizable"].ToString());
            }

            return preferenceModel;
        }

        public List<DdlSourceModel> GetUserTypeList(UserModel userModel)
        {
            _dbUser = new DBUser();
            DdlSourceModel userTypeModel = null;
            List<DdlSourceModel> userTypeModelList = new List<DdlSourceModel>();

            DataTable dtUser = _dbUser.GetUserTypeList(userModel);

            foreach (DataRow dr in dtUser.Rows)
            {
                userTypeModel = new DdlSourceModel();
                userTypeModel.keyfield = dr["keyfield"].ToString();
                userTypeModel.value = dr["value"].ToString();

                userTypeModelList.Add(userTypeModel);
            }

            return userTypeModelList;
        }

        public List<DdlSourceModel> GetTimeZoneList(UserModel userModel)
        {
            _dbUser = new DBUser();
            DdlSourceModel timeZoneModel = null;
            List<DdlSourceModel> timeZoneModelList = new List<DdlSourceModel>();

            DataTable dtUser = _dbUser.GetTimeZoneList(userModel);

            foreach (DataRow dr in dtUser.Rows)
            {
                timeZoneModel = new DdlSourceModel();
                timeZoneModel.keyfield = dr["keyfield"].ToString();
                timeZoneModel.value = dr["value"].ToString();

                timeZoneModelList.Add(timeZoneModel);
            }

            return timeZoneModelList;
        }

        public List<DdlSourceModel> GetMapZoomList(UserModel userModel)
        {
            _dbUser = new DBUser();
            DdlSourceModel mapZoomModel = null;
            List<DdlSourceModel> mapZoomModelList = new List<DdlSourceModel>();

            DataTable dtUser = _dbUser.GetMapZoomList(userModel);

            foreach (DataRow dr in dtUser.Rows)
            {
                mapZoomModel = new DdlSourceModel();
                mapZoomModel.keyfield = dr["keyfield"].ToString();
                mapZoomModel.value = dr["value"].ToString();

                mapZoomModelList.Add(mapZoomModel);
            }

            return mapZoomModelList;
        }

        public List<DdlSourceModel> GetMapCenterList(UserModel userModel)
        {
            _dbUser = new DBUser();
            DdlSourceModel mapCenterModel = null;
            List<DdlSourceModel> mapCenterModelList = new List<DdlSourceModel>();

            DataTable dtUser = _dbUser.GetMapCenterList(userModel);

            foreach (DataRow dr in dtUser.Rows)
            {
                mapCenterModel = new DdlSourceModel();
                mapCenterModel.keyfield = dr["keyfield"].ToString();
                mapCenterModel.value = dr["value"].ToString();

                mapCenterModelList.Add(mapCenterModel);
            }

            return mapCenterModelList;
        }

        public List<DdlSourceModel> GetFeatureList(UserModel userModel)
        {
            _dbUser = new DBUser();
            DdlSourceModel featureModel = null;
            List<DdlSourceModel> featureList = new List<DdlSourceModel>();

            DataTable dtUser = _dbUser.GetFeatureList(userModel);

            foreach (DataRow dr in dtUser.Rows)
            {
                featureModel = new DdlSourceModel();
                featureModel.keyfield = dr["keyfield"].ToString();
                featureModel.value = dr["value"].ToString();

                featureList.Add(featureModel);
            }

            return featureList;
        }

        public List<DdlSourceModel> GetSelectedFeatureList(UserModel userModel)
        {
            _dbUser = new DBUser();
            DdlSourceModel featureModel = null;
            List<DdlSourceModel> featureList = new List<DdlSourceModel>();

            DataTable dtUser = _dbUser.GetSelectedFeatureList(userModel);

            foreach (DataRow dr in dtUser.Rows)
            {
                featureModel = new DdlSourceModel();
                featureModel.keyfield = dr["keyfield"].ToString();
                featureModel.value = dr["value"].ToString();

                featureList.Add(featureModel);
            }

            return featureList;
        }

        public List<DdlSourceModel> GetContactTypeList(UserModel userModel)
        {
            _dbUser = new DBUser();
            DdlSourceModel contactTypeModel = null;
            List<DdlSourceModel> contactTypeList = new List<DdlSourceModel>();

            DataTable dtUser = _dbUser.GetContactTypeList(userModel);

            foreach (DataRow dr in dtUser.Rows)
            {
                contactTypeModel = new DdlSourceModel();
                contactTypeModel.keyfield = dr["keyfield"].ToString();
                contactTypeModel.value = dr["value"].ToString();

                contactTypeList.Add(contactTypeModel);
            }

            return contactTypeList;
        }

        public List<ChkBoxListSourceModel> GetMapClassList(UserModel userModel)
        {
            _dbUser = new DBUser();
            ChkBoxListSourceModel mapClassModel = null;
            List<ChkBoxListSourceModel> mapClassModelList = new List<ChkBoxListSourceModel>();

            DataTable dtUser = _dbUser.GetMapClassList(userModel);

            foreach (DataRow dr in dtUser.Rows)
            {
                mapClassModel = new ChkBoxListSourceModel();
                mapClassModel.keyfield = dr["classid"].ToString();
                mapClassModel.value = dr["classname"].ToString();
                mapClassModel.IsChecked = int.Parse(dr["selected"].ToString());

                mapClassModelList.Add(mapClassModel);
            }

            return mapClassModelList;
        }

        public List<ContactModel> GetEmployeeContacts(UserModel userModel)
        {
            _dbUser = new DBUser();
            ContactModel contactModel = null;
            List<ContactModel> ContactModellist = new List<ContactModel>();

            DataTable dtUser = _dbUser.GetEmployeeContacts(userModel);

            foreach (DataRow dr in dtUser.Rows)
            {
                contactModel = new ContactModel();
                contactModel.ContactId = int.Parse(dr["employeecontactsid"].ToString());
                contactModel.Description = dr["description"].ToString();
                contactModel.ContactValue = dr["Contact"].ToString();

                ContactModellist.Add(contactModel);
            }

            return ContactModellist;
        }

        public int InsertEmployeeContact(UserModel userModel)
        {
            _dbUser = new DBUser();
            return _dbUser.InsertEmployeeContact(userModel);
        }

        public int DeleteEmployeeContact(UserModel userModel)
        {
            _dbUser = new DBUser();
            return _dbUser.DeleteEmployeeContact(userModel);
        }

        public int InsertFeatures(UserModel userModel)
        {
            _dbUser = new DBUser();
            return _dbUser.InsertFeatures(userModel);
        }

        public int DeleteFeatures(UserModel userModel)
        {
            _dbUser = new DBUser();
            return _dbUser.DeleteFeatures(userModel);
        }

        public UserModel SaveEmployee(UserModel userModel)
        {
            _dbUser = new DBUser();
            return _dbUser.SaveEmployee(userModel);
        }

        public int SavePreferences(UserModel userModel)
        {
            _dbUser = new DBUser();
            return _dbUser.SavePreferences(userModel);
        }

        public string GetGeneratedPassword(int SessionId)
        {
            _dbUser = new DBUser();
            return _dbUser.GetGeneratedPassword(SessionId);
        }

        public int UpdateLegends(int SessionId, int EmployeeId, string legends)
        {
            _dbUser = new DBUser();
            return _dbUser.UpdateLegends(SessionId, EmployeeId, legends);
        }

        public bool CheckAdministrativeRights(UserModel userModel, string strRoleID)
        {
            _dbUser = new DBUser();
            return _dbUser.CheckAdministrativeRights(userModel, strRoleID);
        }


        public string GetFeedUrl(UserModel userModel)
        {
            _dbUser = new DBUser();
            return _dbUser.GetFeedUrl(userModel);
        }

        public List<UserModel> GetTSSAdminList()
        {
            List<UserModel> userList = new List<UserModel>();
            
            var _dbUser = new DBUser();

            try
            {
                DataTable dtUser = _dbUser.GetTSSAdminList();
                foreach (DataRow dr in dtUser.Rows)
                {
                    _userModel = new UserModel();
                    _userModel.EmployeeId = Convert.ToInt32(dr["EmployeeId"]);
                    _userModel.FirstName = dr["First_Name"] == DBNull.Value ? "" : dr["First_Name"].ToString();
                    _userModel.LastName = dr["Last_Name"] == DBNull.Value ? "" : dr["Last_Name"].ToString();
                    _userModel.Email = dr["UserEmail"] == DBNull.Value ? "" : dr["UserEmail"].ToString();

                    userList.Add(_userModel);
                }
            }
            catch { }

            return userList;
        }
    }

}
