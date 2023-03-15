using System;
using System.Web;
using System.Collections.Generic;
using GSA.Security;
using GSA.Security.Models;
using NetTrackModel;
using NetTrackModel.Params;


namespace NetTrackBiz
{
    public interface IFacadeBiz
    {
        #region "helper methods"

        List<IDdlSourceModel> GetDdlDataSource(IDdlSourceModel ddlSourceModel);
        List<IDdlSourceModel> GetDdlDataSource(string spName, string param);

        #endregion "helper methods"

        #region UserBiz

        // Get user information
        UserModel GetUserInfo(UserModel userModel);

        UserModel GetUserLoginInfo(UserModel userModel);

        // Get user detail information (including features);
        UserModel GetUserDetailInfo(UserModel userModel);

        //Get User List
        List<UserModel> GetUserList(UserModel userModel);

        UgUserModelWrapper GetUserList(UgUserModel ugUserModel);

        //Get User Detail info
        UserModel GetDetailUserInfo(UserModel userModel);

        //Get User Preference info
        PreferenceModel GetPreferenceDetailInfo(UserModel userModel);

        //Get User Type List
        List<DdlSourceModel> GetUserTypeList(UserModel userModel);

        //Get Time Zone List
        List<DdlSourceModel> GettimeZoneList(UserModel userModel);

        //Get Map Zoom List
        List<DdlSourceModel> GetMapZoomList(UserModel userModel);

        //Get Map Center List
        List<DdlSourceModel> GetMapCenterList(UserModel userModel);

        //Get Feature List
        List<DdlSourceModel> GetFeatureList(UserModel userModel);

        //Get Selected Feature List
        List<DdlSourceModel> GetSelectedFeatureList(UserModel userModel);

        //Get Map Class List
        List<ChkBoxListSourceModel> GetMapClassList(UserModel userModel);

        //Get Contact Type List
        List<DdlSourceModel> GetContactTypeList(UserModel userModel);

        //Get Employee Contacts
        List<ContactModel> GetEmployeeContacts(UserModel userModel);

        //Insert Employee Contact 
        int InsertEmployeeContact(UserModel userModel);

        //Delete Employee Contact
        int DeleteEmployeeContact(UserModel userModel);

        //Insert Features
        int InsertFeatures(UserModel userModel);

        //Delete Features 
        int DeleteFeatures(UserModel userModel);

        //Save Employee
        UserModel SaveEmployee(UserModel userModel);

        //Save Preferences
        int SavePreferences(UserModel userModel);

        string GetGeneratedPassword(int SessionId);

        string GetUserSessionStatus(int sessionId);

        string GetFeedUrl(UserModel userModel);

        int UpdateLegends(int SessionId, int EmployeeId, string legends);

        #endregion

        #region UserStateBiz

        // Get user state information
        UserStateModel MaintainUserStateGridPage(UserStateModel userStateModel);

        UserStateModel MaintainUserState(UserStateModel userStateModel);

        #endregion

        #region StatusCode Business

        OutParams saveStatusCode(StatusCodesInputParams ip, UgsStatusCodeModel m);

        List<UgsStatusCodeModel> getStatusCodeList(StatusCodesInputParams ip);

        UgsStatusCodeModel getStatusCode(StatusCodesInputParams ip);

        #endregion

        #region TSS - Product

        List<ProductModel> GetProductList(int sessionId);

        ProductModel GetDetailProductInfo(ProductModel productModel);

        List<DdlSourceModel> GetProductTypeList(ProductModel productModel);

        ProductModel SaveProduct(ProductModel model);

        QuoteProductModel SaveQuoteProduct(QuoteProductModel model);

        SalesOrderModel SaveSalesOrder(SalesOrderModel model);

        QuoteModel SaveQuote(QuoteModel model);

        void ResetQuoteStartDate(int quoteId);

        void ApproveQuote(QuoteModel model);

        QuoteOrderModel SaveQuoteOrder(QuoteOrderModel model);

        List<SalesPerson> GetSalesPersonList(int sessionId);

        List<SalesPerson> GetSalesPersonList(string clientIdList);

        void SaveSalesPerson(int employeeId, bool isSet);

        List<QuoteModel> GetQuoteList(QuoteModel model);

        List<QuoteOrderModel> GetSalesList(QuoteOrderModel model);

        QuoteModel GetQuoteInfo(QuoteModel quoteModel);

        QuoteOrderModel GetQuoteOrderInfo(QuoteOrderModel quoteOrderModel);

        List<QuoteProductModel> GetQuoteProductList(QuoteProductModel quoteProductModel);

        List<SalesOrderModel> GetSalesOrderList(SalesOrderModel salesOrderModel);

        List<ClientModel> GetNetTrackClientList(ClientModel clientModel);

        List<DdlSourceModel> GetOrderStatusList(int sessionId);

        List<QuotePaymentMethodModel> GetQuotePaymentMethodList();

        void SetQuoteOrderStatus(QuoteOrderModel model);

        #endregion

        #region Client

        int SaveClient(UgsClientModel clientModel);

        void getNewZohoContacts(UgsNewZohoContacts m);

        UgsClientModel GetDetailClientInfo(UgsClientModel clientModel);

        #endregion

        #region TSS-QuoteTemplate

        QuoteProductModel SaveQuoteTemplateProduct(QuoteProductModel model);

        List<QuoteTemplateModel> GetQuoteTemplateList(int sessionId);

        QuoteTemplateModel SaveQuoteTemplate(QuoteTemplateModel model);

        QuoteTemplateModel GetQuoteTemplateInfo(QuoteTemplateModel quoteModel);

        List<QuoteProductModel> GetQuoteProductTemplateList(QuoteProductModel quoteProductModel);

        #endregion

        #region BluePaySettings

        BluePaySettingsModel GetBluePaySettingsInfo(string serverName);

        BluePaySettingsModel SaveBluePaySettings(BluePaySettingsModel model);

        BluePayLogModel SaveBluePayLog(BluePayLogModel model);

        #endregion

        #region SalesTax

        List<SalesTaxModel> GetSalesTaxList();

        SalesTaxModel SaveSalesTax(SalesTaxModel model);
        #endregion

        QuoteTemplateModel SetQuoteTemplateActive(QuoteTemplateModel model);

        #region BluePayTrans

        BluePayTransactionModel SaveBluePayTrans(BluePayTransactionModel model);

        #endregion

        #region My Account

        MyAccountModel GetMyAccountInfo(int clientID);

        int SaveMyAccountInfo(MyAccountModel myAccountModel);

        #endregion

        #region QuoteLastView

        List<QuoteLastViewModel> GetQuoteLastViewList(int quoteId);

        #endregion

        #region TssSentEmail

        List<TSSSentEmailModel> GetSentEmailList(int quoteId);

        List<TSSSentEmailModel> GetSentEmailReportList(TSSSentEmailModel sentEmailModel);

        List<TSSSentEmailModel> GetResendEmailList();

        TSSSentEmailModel SaveTSSSentEmail(TSSSentEmailModel model);

        #endregion

        #region EmailTemplate

        List<EmailTemplateModel> GetEmailTemplateList();

        EmailTemplateModel GetEmailTemplate(int emailTemplateId);

        EmailTemplateModel SaveEmailTemplate(EmailTemplateModel model);

        void UpdateEmailTemplate(EmailTemplateModel model);

        void DeleteEmailTemplate(int emailTemplateId);

        #endregion

        #region OrderStatusHistory

        OrderStatusHistory SaveOrderStatusHistory(int quoteOrderId, int orderStatusId);

        List<OrderStatusHistory> GetOrderStatusHistoryList(int quoteOrderId);


        #endregion

        #region QuoteEmailUnsubscribe

        QuoteEmailUnsubscribeModel SaveQuoteEmailUnsubscribe(QuoteEmailUnsubscribeModel model);

        List<QuoteEmailUnsubscribeModel> GetQuoteEmailUnsubscribeList(int quoteId);

        #endregion

        List<UserModel> GetTSSAdminList();

        #region TSS Settings

        TSSSettings GetSettings(string settingsName);

        void SetSettings(TSSSettings model);

        #endregion

        #region Security Group

        List<SecurityGroupModel> GetAllSecurityGroup();

        SecurityGroupModel GetSecurityGroupById(long securityGroupId);

        void SaveSecurityGroup(SecurityGroupModel model);

        void DeleteSecurityGroup(SecurityGroupModel model);

        #endregion

        #region Security Groups Sales Person

        List<SecurityGroupSalesPersonModel> GetSalesPersonsBySecurityGroupId(SecurityGroupSalesPersonModel model);

        List<SecurityGroupSalesPersonModel> GetSalesPersonsByEmployeeId(SecurityGroupSalesPersonModel model);

        void SaveSalesPersonSecurityGroup(SecurityGroupSalesPersonModel model);

        void DeleteSalesPersonSecurityGroup(SecurityGroupSalesPersonModel model);

        #endregion

        #region QuoteLinkMapping

        QuoteLinkMappingModel GetQuoteLinkMapping(string encryptValue);
        void SaveQuoteLinkMapping(QuoteLinkMappingModel model);

        #endregion
    }
}
