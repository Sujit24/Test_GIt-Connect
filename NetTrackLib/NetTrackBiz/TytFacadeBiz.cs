using System;
using System.Web;
using System.Collections.Generic;
using GSA.Security;
using GSA.Security.Models;
using NetTrackModel;
using NetTrackModel.Params;

namespace NetTrackBiz
{
    public class TytFacadeBiz : IMembershipService, IFacadeBiz
    {
        #region "helper methods"

        public List<IDdlSourceModel> GetDdlDataSource(IDdlSourceModel ddlSourceModel)
        {
            return new CommonBiz().GetDdlDataSource(ddlSourceModel);
        }
        public List<IDdlSourceModel> GetDdlDataSource(string spName, string param)
        {
            return new CommonBiz().GetDdlDataSource(spName, param);
        }

        #endregion "helper methods"

        #region Private property

        private UserBiz _userBiz;
        private UserModel _userModel;
        private UserStateBiz _userStateBiz;
        private StatusCodesBiz _statusCodeBiz;
        private ProductBiz _ProductBiz;
        private QuoteTemplateBiz _quoteTemplateBiz;
        private BluePaySettingsBiz _BluePaySettingsBiz;
        private SalesTaxBiz _SalesTaxBiz;
        private BluePayTransBiz _BluePayTransBiz;
        private MyAccountBiz _MyAccountBiz;
        private EmailTemplateBiz _EmailTemplateBiz;
        private TSSSettingsBiz _TSSSettingsBiz;
        private SecurityGroupBiz _SecurityGroupBiz;
        private SecurityGroupSalesPersonBiz _SecurityGroupSalesPersonBiz;
        private TemplateGroupBiz _TemplateGroupBiz;

        #endregion

        #region Default constructor

        // Default constructor..
        public TytFacadeBiz()
        {
            _userBiz = new UserBiz();
            _userModel = new UserModel();
            _userStateBiz = new UserStateBiz();

            _statusCodeBiz = new StatusCodesBiz();

            _ProductBiz = new ProductBiz();
            _quoteTemplateBiz = new QuoteTemplateBiz();

            _BluePaySettingsBiz = new BluePaySettingsBiz();
            _BluePayTransBiz = new BluePayTransBiz();
            _SalesTaxBiz = new SalesTaxBiz();
            _MyAccountBiz = new MyAccountBiz();
            _EmailTemplateBiz = new EmailTemplateBiz();
            _TSSSettingsBiz = new TSSSettingsBiz();
            _SecurityGroupBiz = new SecurityGroupBiz();
            _SecurityGroupSalesPersonBiz = new SecurityGroupSalesPersonBiz();

            _TemplateGroupBiz = new TemplateGroupBiz();
        }

        #endregion

        #region

        // Public property

        #endregion

        #region UserBiz

        // Get user information
        public UserModel GetUserInfo(UserModel userModel)
        {
            return _userBiz.GetUserInfo(userModel);
        }

        public UserModel GetUserLoginInfo(UserModel userModel)
        {
            return _userBiz.GetUserLoginInfo(userModel);
        }
        // Get user detail information (including features)
        public UserModel GetUserDetailInfo(UserModel userModel)
        {
            UserModel userModelL = _userBiz.GetUserDetailInfo(userModel);
            return userModelL;
        }

        //Get User List
        public List<UserModel> GetUserList(UserModel userModel)
        {
            return _userBiz.GetUserList(userModel); ;
        }

        public UgUserModelWrapper GetUserList(UgUserModel ugUserModel)
        {
            return _userBiz.GetUserList(ugUserModel); ;
        }

        //Get User Detail info
        public UserModel GetDetailUserInfo(UserModel userModel)
        {
            return _userBiz.GetDetailUserInfo(userModel); ;
        }

        //Get User Preference info
        public PreferenceModel GetPreferenceDetailInfo(UserModel userModel)
        {
            return _userBiz.GetPreferenceDetailInfo(userModel); ;
        }

        //Get User Type List
        public List<DdlSourceModel> GetUserTypeList(UserModel userModel)
        {
            return _userBiz.GetUserTypeList(userModel); ;
        }

        //Get Time Zone List
        public List<DdlSourceModel> GettimeZoneList(UserModel userModel)
        {
            return _userBiz.GetTimeZoneList(userModel); ;
        }

        //Get Map Zoom List
        public List<DdlSourceModel> GetMapZoomList(UserModel userModel)
        {
            return _userBiz.GetMapZoomList(userModel); ;
        }

        //Get Map Center List
        public List<DdlSourceModel> GetMapCenterList(UserModel userModel)
        {
            return _userBiz.GetMapCenterList(userModel); ;
        }

        //Get Feature List
        public List<DdlSourceModel> GetFeatureList(UserModel userModel)
        {
            return _userBiz.GetFeatureList(userModel); ;
        }

        //Get Selected Feature List
        public List<DdlSourceModel> GetSelectedFeatureList(UserModel userModel)
        {
            return _userBiz.GetSelectedFeatureList(userModel); ;
        }

        //Get Map Class List
        public List<ChkBoxListSourceModel> GetMapClassList(UserModel userModel)
        {
            return _userBiz.GetMapClassList(userModel); ;
        }

        //Get Contact Type List
        public List<DdlSourceModel> GetContactTypeList(UserModel userModel)
        {
            return _userBiz.GetContactTypeList(userModel); ;
        }

        //Get Employee Contacts
        public List<ContactModel> GetEmployeeContacts(UserModel userModel)
        {
            return _userBiz.GetEmployeeContacts(userModel); ;
        }

        //Insert Employee Contact 
        public int InsertEmployeeContact(UserModel userModel)
        {
            return _userBiz.InsertEmployeeContact(userModel); ;
        }

        //Delete Employee Contact
        public int DeleteEmployeeContact(UserModel userModel)
        {
            return _userBiz.DeleteEmployeeContact(userModel); ;
        }

        //Insert Features
        public int InsertFeatures(UserModel userModel)
        {
            return _userBiz.InsertFeatures(userModel); ;
        }

        //Delete Features 
        public int DeleteFeatures(UserModel userModel)
        {
            return _userBiz.DeleteFeatures(userModel); ;
        }

        //Save Employee
        public UserModel SaveEmployee(UserModel userModel)
        {
            return _userBiz.SaveEmployee(userModel); ;
        }

        //Save Preferences
        public int SavePreferences(UserModel userModel)
        {
            return _userBiz.SavePreferences(userModel); ;
        }

        public string GetGeneratedPassword(int SessionId)
        {
            return _userBiz.GetGeneratedPassword(SessionId); ;
        }

        public string GetUserSessionStatus(int sessionId)
        {
            return new UserSessionBiz().GetUserSessionStatus(sessionId); ;
        }

        public string GetFeedUrl(UserModel userModel)
        {
            return _userBiz.GetFeedUrl(userModel);
        }

        public int UpdateLegends(int SessionId, int EmployeeId, string legends)
        {
            return _userBiz.UpdateLegends(SessionId, EmployeeId, legends);
        }
        #endregion

        #region UserStateBiz

        // Get user state information
        public UserStateModel MaintainUserStateGridPage(UserStateModel userStateModel)
        {
            return _userStateBiz.MaintainUserStateGridPage(userStateModel);
        }
        public UserStateModel MaintainUserState(UserStateModel userStateModel)
        {
            return _userStateBiz.MaintainUserState(userStateModel);
        }

        #endregion

        #region IMembershipService methods

        public static class Role
        {
            public static string HaveFeatures { get { return "HaveFeatures"; } }
            public static string HaveReports { get { return "HaveReports"; } }
            public static string HaveOptions { get { return "HaveOptions"; } }

            public static string HaveMessaging { get { return "HaveMessaging"; } }
            public static string HaveMapReplay { get { return "HaveMapReplay"; } }
            public static string feedurl { get { return "feedurl"; } }
            public static string haveP2S2status { get { return "haveP2S2status"; } }
            public static string ishomepoint { get { return "ishomepoint"; } }
            public static string HaveLiteAdmin { get { return "HaveLiteAdmin"; } }
            public static string haveUserPropstatus { get { return "haveUserPropstatus"; } }
            public static string HavePolygonSave { get { return "HavePolygonSave"; } }

            public static string HaveGoogleEarth { get { return "HaveGoogleEarth"; } }
            public static string HaveChat { get { return "HaveChat"; } }
            public static string haveopenstreet { get { return "haveopenstreet"; } }
            public static string HaveReports60 { get { return "HaveReports60"; } }
            public static string HaveReportsOld { get { return "HaveReportsOld"; } }
            public static string HaveRemoteControl { get { return "HaveRemoteControl"; } }
            public static string HaveCustomMessage { get { return "HaveCustomMessage"; } }
            public static string HaveAdmin { get { return "HaveAdmin"; } }
            public static string HaveClientAdmin { get { return "HaveClientAdmin"; } }
            public static string HaveCemtekOrP2S2Admin { get { return "HaveCemtekOrP2S2Admin"; } }

            public static string HaveOptTrucks { get { return "Trucks"; } }
            public static string HaveOptTerminals { get { return "Terminals"; } }
            public static string HaveOptUsers { get { return "Users"; } }
            public static string HaveOptClients { get { return "Clients"; } }
            public static string HaveOptPoints { get { return "Points"; } }
            public static string HaveOptClasses { get { return "Classes"; } }
            public static string HaveOptAlertsFull { get { return "Alerts(Full)"; } }
            public static string HaveOptAlerts { get { return "Alerts"; } }
            public static string HaveOptScheduledReportsFull { get { return "Scheduled Reports(Full)"; } }
            public static string HaveOptSensors { get { return "Sensors"; } }
            public static string HaveOptImageSets { get { return "Image Sets"; } }
            public static string HaveOptStatusCodes { get { return "Status Codes"; } }
            public static string HaveOptBroadcast { get { return "Broadcast"; } }
            public static string HaveOptPreferences { get { return "Preferences"; } }
            public static string HaveOptBrowserDebugInfo { get { return "Browser Debug Info"; } }
            public static string HaveOptSupport { get { return "Support"; } }
            public static string HaveOptTruckProperties { get { return "Truck Properties"; } }
            public static string HaveOptApplicationConfig { get { return "Application Config"; } }
            public static string HaveOptCannedMessages { get { return "Canned Messages"; } }
            public static string HaveOptHolidays { get { return "Holidays"; } }
            public static string HaveOptListDebugFiles { get { return "List Debug Files"; } }
            public static string HavePreventiveMaintenance { get { return "HavePreventiveMaintenance"; } }
            public static string HaveGoogleRouting { get { return "HaveGoogleRouting"; } }





        }

        public GSA.Security.Models.AuthStatus ValidateUser(string userName, string password)
        {
            _userModel = new UserModel(userName, password, "", "", "", 0);
            _userModel = GetUserDetailInfo(_userModel);

            AuthStatus authStatus = new AuthStatus();
            if (_userModel.ErrorDescription.Equals(string.Empty))
            {
                authStatus.Code = AuthStatusCode.SUCCESS;
                authStatus.Message = "Login successful!";
                authStatus.Data = _userModel;
            }
            else
            {
                authStatus.Code = AuthStatusCode.FAILED;
                authStatus.Message = _userModel.ErrorDescription;
            }

            return authStatus;
        }

        public string[] GetRoles(string userName)
        {
            return _userModel.Roles;
        }

        public static bool IsUserInRole(string roleName)
        {
            return HttpContext.Current.User.IsInRole(roleName);
        }

        #endregion

        #region StatusCode Business

        public OutParams saveStatusCode(StatusCodesInputParams ip, UgsStatusCodeModel m)
        {
            return _statusCodeBiz.saveStatusCode(ip, m);
        }

        public List<UgsStatusCodeModel> getStatusCodeList(StatusCodesInputParams ip)
        {
            return _statusCodeBiz.getStatusCodeList(ip);
        }

        public UgsStatusCodeModel getStatusCode(StatusCodesInputParams ip)
        {
            return _statusCodeBiz.getStatusCode(ip);
        }

        #endregion

        #region TSS - Product

        public List<ProductModel> GetProductList(int sessionId)
        {
            return _ProductBiz.GetProductList(sessionId);
        }

        public ProductModel GetDetailProductInfo(ProductModel productModel)
        {
            return _ProductBiz.GetDetailProductInfo(productModel);
        }

        public List<DdlSourceModel> GetProductTypeList(ProductModel productModel)
        {
            return _ProductBiz.GetProductTypeList(productModel); ;
        }

        public ProductModel SaveProduct(ProductModel model)
        {
            return _ProductBiz.SaveProduct(model);
        }

        public QuoteProductModel SaveQuoteProduct(QuoteProductModel model)
        {
            return _ProductBiz.SaveQuoteProduct(model);
        }

        public SalesOrderModel SaveSalesOrder(SalesOrderModel model)
        {
            return _ProductBiz.SaveSalesOrder(model);
        }

        public QuoteModel SaveQuote(QuoteModel model)
        {
            return _ProductBiz.SaveQuote(model);
        }

        public void ResetQuoteStartDate(int quoteId)
        {
            _ProductBiz.ResetQuoteStartDate(quoteId);
        }

        public void ApproveQuote(QuoteModel model)
        {
            _ProductBiz.ApproveQuote(model);
        }

        public QuoteOrderModel SaveQuoteOrder(QuoteOrderModel model)
        {
            return _ProductBiz.SaveQuoteOrder(model);
        }

        public List<SalesPerson> GetSalesPersonList(int sessionId)
        {
            return _ProductBiz.GetSalesPersonList(sessionId);
        }

        public List<SalesPerson> GetSalesPersonList(string clientIdList)
        {
            return _ProductBiz.GetSalesPersonList(clientIdList);
        }

        public void SaveSalesPerson(int employeeId, bool isSet)
        {
            _ProductBiz.SaveSalesPerson(employeeId, isSet);
        }

        public List<QuoteModel> GetQuoteList(QuoteModel model)
        {
            return _ProductBiz.GetQuoteList(model);
        }

        public List<QuoteOrderModel> GetSalesList(QuoteOrderModel model)
        {
            return _ProductBiz.GetSalesList(model);
        }

        public QuoteModel GetQuoteInfo(QuoteModel quoteModel)
        {
            return _ProductBiz.GetQuoteInfo(quoteModel);
        }

        public QuoteOrderModel GetQuoteOrderInfo(QuoteOrderModel quoteOrderModel)
        {
            return _ProductBiz.GetQuoteOrderInfo(quoteOrderModel);
        }

        public List<QuoteProductModel> GetQuoteProductList(QuoteProductModel quoteProductModel)
        {
            return _ProductBiz.GetQuoteProductList(quoteProductModel);
        }

        public List<SalesOrderModel> GetSalesOrderList(SalesOrderModel salesOrderModel)
        {
            return _ProductBiz.GetSalesOrderList(salesOrderModel);
        }

        public List<ClientModel> GetNetTrackClientList(ClientModel clientModel)
        {
            return _ProductBiz.GetNetTrackClientList(clientModel);
        }

        public List<DdlSourceModel> GetOrderStatusList(int sessionId)
        {
            return _ProductBiz.GetOrderStatusList(sessionId);
        }

        public List<QuotePaymentMethodModel> GetQuotePaymentMethodList()
        {
            return _ProductBiz.GetQuotePaymentMethodList();
        }

        public void SetQuoteOrderStatus(QuoteOrderModel model)
        {
            _ProductBiz.SetQuoteOrderStatus(model);
        }
        #endregion

        #region Client
        public int SaveClient(UgsClientModel clientModel)
        {
            ClientBiz clientBiz = new ClientBiz();
            return clientBiz.SaveClient(clientModel);
        }
        public void getNewZohoContacts(UgsNewZohoContacts m)
        {
            ClientBiz _clientBiz = new ClientBiz();
            _clientBiz.getNewZohoContacts(m);
        }
        public UgsClientModel GetDetailClientInfo(UgsClientModel clientModel)
        {
            ClientBiz clientBiz = new ClientBiz();
            return clientBiz.GetDetailClientInfo(clientModel);
        }
        #endregion

        #region TSS-QuoteTemplate

        public QuoteProductModel SaveQuoteTemplateProduct(QuoteProductModel model)
        {
            return _quoteTemplateBiz.SaveQuoteTemplateProduct(model);
        }
        public List<QuoteTemplateModel> GetQuoteTemplateList(int sessionId)
        {
            return _quoteTemplateBiz.GetQuoteTemplateList(sessionId);
        }
        public QuoteTemplateModel SaveQuoteTemplate(QuoteTemplateModel model)
        {
            return _quoteTemplateBiz.SaveQuoteTemplate(model);
        }
        public QuoteTemplateModel GetQuoteTemplateInfo(QuoteTemplateModel quoteModel)
        {
            return _quoteTemplateBiz.GetQuoteTemplateInfo(quoteModel);
        }
        public List<QuoteProductModel> GetQuoteProductTemplateList(QuoteProductModel quoteProductModel)
        {
            return _quoteTemplateBiz.GetQuoteProductTemplateList(quoteProductModel);
        }
        public List<DdlSourceModel> GetQuoteTemplateGroupList()
        {
            return _quoteTemplateBiz.GetQuoteTemplateGroupList();
        }

        #endregion

        #region BluePaySettings

        public BluePaySettingsModel GetBluePaySettingsInfo(string serverName)
        {
            return _BluePaySettingsBiz.GetBluePaySettingsInfo(serverName);
        }

        public BluePaySettingsModel SaveBluePaySettings(BluePaySettingsModel model)
        {
            return _BluePaySettingsBiz.SaveBluePaySettings(model);
        }

        public BluePayLogModel SaveBluePayLog(BluePayLogModel model)
        {
            return _BluePaySettingsBiz.SaveBluePayLog(model);
        }

        #endregion

        #region SalesTax

        public List<SalesTaxModel> GetSalesTaxList()
        {
            return _SalesTaxBiz.GetSalesTaxList();
        }

        public SalesTaxModel SaveSalesTax(SalesTaxModel model)
        {
            return _SalesTaxBiz.SaveSalesTax(model);
        }
        #endregion

        public QuoteTemplateModel SetQuoteTemplateActive(QuoteTemplateModel model)
        {
            return _quoteTemplateBiz.SetQuoteTemplateActive(model);
        }

        #region BluePayTrans

        public BluePayTransactionModel SaveBluePayTrans(BluePayTransactionModel model)
        {
            return _BluePayTransBiz.SaveBluePayTrans(model);
        }

        public BluePayACHTransactionModel SaveBluePayACHTrans(BluePayACHTransactionModel model)
        {
            return _BluePayTransBiz.SaveBluePayACHTrans(model);
        }

        #endregion

        #region My Account

        public MyAccountModel GetMyAccountInfo(int clientID)
        {
            return _MyAccountBiz.GetMyAccountInfo(clientID);
        }

        public int SaveMyAccountInfo(MyAccountModel myAccountModel)
        {
            return _MyAccountBiz.SaveMyAccountInfo(myAccountModel);
        }

        #endregion

        #region QuoteLastView

        public List<QuoteLastViewModel> GetQuoteLastViewList(int quoteId)
        {
            return _ProductBiz.GetQuoteLastViewList(quoteId);
        }

        #endregion

        #region TssSentEmail

        public List<TSSSentEmailModel> GetSentEmailList(int quoteId)
        {
            return _ProductBiz.GetSentEmailList(quoteId);
        }

        public List<TSSSentEmailModel> GetSentEmailReportList(TSSSentEmailModel sentEmailModel)
        {
            return _ProductBiz.GetSentEmailReportList(sentEmailModel);
        }

        public List<TSSSentEmailModel> GetResendEmailList()
        {
            return _ProductBiz.GetResendEmailList();
        }

        public TSSSentEmailModel SaveTSSSentEmail(TSSSentEmailModel model)
        {
            return _ProductBiz.SaveTSSSentEmail(model);
        }

        #endregion

        #region EmailTemplate

        public List<EmailTemplateModel> GetEmailTemplateList()
        {
            return _EmailTemplateBiz.GetEmailTemplateList();
        }

        public EmailTemplateModel GetEmailTemplate(int emailTemplateId)
        {
            return _EmailTemplateBiz.GetEmailTemplate(emailTemplateId);
        }

        public EmailTemplateModel SaveEmailTemplate(EmailTemplateModel model)
        {
            return _EmailTemplateBiz.SaveEmailTemplate(model);
        }

        public void UpdateEmailTemplate(EmailTemplateModel model)
        {
            _EmailTemplateBiz.UpdateEmailTemplate(model);
        }

        public void DeleteEmailTemplate(int emailTemplateId)
        {
            _EmailTemplateBiz.DeleteEmailTemplate(emailTemplateId);
        }

        #endregion

        #region OrderStatusHistory

        public OrderStatusHistory SaveOrderStatusHistory(int quoteOrderId, int orderStatusId)
        {
            return _ProductBiz.SaveOrderStatusHistory(quoteOrderId, orderStatusId);
        }

        public List<OrderStatusHistory> GetOrderStatusHistoryList(int quoteOrderId)
        {
            return _ProductBiz.GetOrderStatusHistoryList(quoteOrderId);
        }


        #endregion

        #region QuoteEmailUnsubscribe

        public QuoteEmailUnsubscribeModel SaveQuoteEmailUnsubscribe(QuoteEmailUnsubscribeModel model)
        {
            return _ProductBiz.SaveQuoteEmailUnsubscribe(model);
        }

        public List<QuoteEmailUnsubscribeModel> GetQuoteEmailUnsubscribeList(int quoteId)
        {
            return _ProductBiz.GetQuoteEmailUnsubscribeList(quoteId);
        }

        #endregion

        public List<UserModel> GetTSSAdminList()
        {
            return _userBiz.GetTSSAdminList();
        }

        #region TSS Settings
        public TSSSettings GetSettings(string settingsName)
        {
            return _TSSSettingsBiz.GetSettings(settingsName);
        }

        public List<TSSSettings> GetAllSettings()
        {
            return _TSSSettingsBiz.GetAllSettings();
        }

        public void SetSettings(TSSSettings model)
        {
            _TSSSettingsBiz.SetSettings(model);
        }

        #endregion

        #region Security Group

        public List<SecurityGroupModel> GetAllSecurityGroup()
        {
            return _SecurityGroupBiz.GetAllSecurityGroup();
        }

        public SecurityGroupModel GetSecurityGroupById(long securityGroupId)
        {
            return _SecurityGroupBiz.GetSecurityGroupById(securityGroupId);
        }

        public void SaveSecurityGroup(SecurityGroupModel model)
        {
            _SecurityGroupBiz.SaveSecurityGroup(model);
        }

        public void DeleteSecurityGroup(SecurityGroupModel model)
        {
            _SecurityGroupBiz.DeleteSecurityGroup(model);
        }

        #endregion

        #region Security Groups Sales Person

        public List<SecurityGroupSalesPersonModel> GetSalesPersonsBySecurityGroupId(SecurityGroupSalesPersonModel model)
        {
            return _SecurityGroupSalesPersonBiz.GetSalesPersonsBySecurityGroupId(model);
        }

        public List<SecurityGroupSalesPersonModel> GetSalesPersonsByEmployeeId(SecurityGroupSalesPersonModel model)
        {
            return _SecurityGroupSalesPersonBiz.GetSalesPersonsByEmployeeId(model);
        }

        public void SaveSalesPersonSecurityGroup(SecurityGroupSalesPersonModel model)
        {
            _SecurityGroupSalesPersonBiz.SaveSalesPersonSecurityGroup(model);
        }

        public void DeleteSalesPersonSecurityGroup(SecurityGroupSalesPersonModel model)
        {
            _SecurityGroupSalesPersonBiz.DeleteSalesPersonSecurityGroup(model);
        }

        #endregion

        #region QuoteLinkMapping

        public QuoteLinkMappingModel GetQuoteLinkMapping(string encryptValue)
        {
            return _ProductBiz.GetQuoteLinkMapping(encryptValue);
        }

        public void SaveQuoteLinkMapping(QuoteLinkMappingModel model)
        {
            _ProductBiz.SaveQuoteLinkMapping(model);
        }

        #endregion

        #region Template Group

        public List<TemplateGroupModel> GetAllTemplateGroup()
        {
            return _TemplateGroupBiz.GetAllTemplateGroup();
        }

        public void SaveTemplateGroup(TemplateGroupModel model)
        {
            _TemplateGroupBiz.SaveTemplateGroup(model);
        }

        public void DeleteTemplateGroup(TemplateGroupModel model)
        {
            _TemplateGroupBiz.DeleteTemplateGroup(model);
        }

        #endregion

        #region TSS Shipping

        public void SaveTssShipping(TssShippingModel model)
        {
            _ProductBiz.SaveTssShipping(model);
        }

        public List<TssShippingModel> GetTssShippingHistory(TssShippingModel model)
        {
            return _ProductBiz.GetTssShippingHistory(model);
        }

        public void UpdateTssShipSentEmail(TssShippingModel model)
        {
            _ProductBiz.UpdateTssShipSentEmail(model);
        }

        public List<TssShippingEmailLogModel> GetTssShippingEmailHistory(TssShippingModel model)
        {
            return _ProductBiz.GetTssShippingEmailHistory(model);
        }

        #endregion
    }
}
