using System;
using System.ComponentModel;

namespace NetTrackModel
{
    public class UserModel
    {
        // private property 
        private string _login;
        private string _pin;
        private string _newPin;
        private string _browserInfo;
        private string _urlInfo;
        private int _timeDiff;

        // user information
        private int _clientId;
        private int _employeeId;
        private int _sessionId;
        private int _errorId;
        private int _winResizable;
        private string _userName;
        private string _employeeName;
        private string _errorDescription;
        private string _firstUrl;
        private string _firstName;
        private string _lastName;
        private string _unitName;
        private string _type;
        private string _timeZone;
        private string _email;
        private string _tokenURL;

        // user features--
        private int _haveReports;
        private int _haveOptions;
        private int _haveMessaging;
        private int _haveMapReplay;
        private string _feedUrl;
        private int _haveP2S2Status;
        private int _isHomePoint;
        private int _haveLiteAdmin;
        private int _haveUserPropstatus;
        private int _havePolygonSave;
        private int _haveGoogleEarth;
        private int _haveChat;
        private int _haveOpenStreet;
        private int _haveReports60;
        private int _haveReportsOld;
        private string _haveRemoteControl;
        private int _haveCustomMessage;


        // public property
        [DisplayName("Web Logon:")]
        public string Login
        {
            get { return this._login; }
            set { this._login = value; }
        }
        [DisplayName("Web Password:")]
        public string Pin
        {
            get { return this._pin; }
            set { this._pin = value; }
        }
        public string NewPin
        {
            get { return this._newPin; }
            set { this._newPin = value; }
        }
        public string BrowserInfo
        {
            get { return this._browserInfo; }
            set { this._browserInfo = value; }
        }
        public string UrlInfo
        {
            get { return this._urlInfo; }
            set { this._urlInfo = value; }
        }
        public int TimeDiff
        {
            get { return this._timeDiff; }
            set { this._timeDiff = value; }
        }

        public int ClientId
        {
            get { return this._clientId; }
            set { this._clientId = value; }
        }
        public int EmployeeId
        {
            get { return this._employeeId; }
            set { this._employeeId = value; }
        }
        public int SessionId
        {
            get { return this._sessionId; }
            set { this._sessionId = value; }
        }
        public int ErrorId
        {
            get { return this._errorId; }
            set { this._errorId = value; }
        }
        public int WinResizable
        {
            get { return this._winResizable; }
            set { this._winResizable = value; }
        }
        public string UserName
        {
            get { return this._userName; }
            set { this._userName = value; }
        }
        public string EmployeeName
        {
            get { return this._employeeName; }
            set { this._employeeName = value; }
        }
        public string ErrorDescription
        {
            get { return this._errorDescription; }
            set { this._errorDescription = value; }
        }
        public string FirstUrl
        {
            get { return this._firstUrl; }
            set { this._firstUrl = value; }
        }
        [DisplayName("First Name:")]
        public string FirstName
        {
            get { return this._firstName; }
            set { this._firstName = value; }
        }
        [DisplayName("Last Name:")]
        public string LastName
        {
            get { return this._lastName; }
            set { this._lastName = value; }
        }
        public string UnitName
        {
            get { return this._unitName; }
            set { this._unitName = value; }
        }
        public string Type
        {
            get { return this._type; }
            set { this._type = value; }
        }
        [DisplayName("Time Zone:")]
        public string TimeZone
        {
            get { return this._timeZone; }
            set { this._timeZone = value; }
        }
        [DisplayName("Email:")]
        public string Email
        {
            get { return this._email; }
            set { this._email = value; }
        }
        [DisplayName("Token URL:")]
        public string TokenURL
        {
            get { return this._tokenURL; }
            set { this._tokenURL = value; }
        }
        public string Action { get; set; }
        public string SelectedFeatures { get; set; }
        public PreferenceModel Preference { get; set; }
        public ContactModel Contact { get; set; }

        public int HaveReports
        {
            get { return this._haveReports; }
            set { this._haveReports = value; }
        }
        public int HaveOptions
        {
            get { return this._haveOptions; }
            set { this._haveOptions = value; }
        }
        public int HaveMessaging
        {
            get { return this._haveMessaging; }
            set { this._haveMessaging = value; }
        }
        public int HaveMapReplay
        {
            get { return this._haveMapReplay; }
            set { this._haveMapReplay = value; }
        }
        public string FeedUrl
        {
            get { return this._feedUrl; }
            set { this._feedUrl = value; }
        }
        public int HaveP2S2Status
        {
            get { return this._haveP2S2Status; }
            set { this._haveP2S2Status = value; }
        }
        public int IsHomePoint
        {
            get { return this._isHomePoint; }
            set { this._isHomePoint = value; }
        }
        public int HaveLiteAdmin
        {
            get { return this._haveLiteAdmin; }
            set { this._haveLiteAdmin = value; }
        }
        public int HaveUserPropstatus
        {
            get { return this._haveUserPropstatus; }
            set { this._haveUserPropstatus = value; }
        }
        public int HavePolygonSave
        {
            get { return this._havePolygonSave; }
            set { this._havePolygonSave = value; }
        }
        public int HaveGoogleEarth
        {
            get { return this._haveGoogleEarth; }
            set { this._haveGoogleEarth = value; }
        }
        public int HaveChat
        {
            get { return this._haveChat; }
            set { this._haveChat = value; }
        }
        public int HaveOpenStreet
        {
            get { return this._haveOpenStreet; }
            set { this._haveOpenStreet = value; }
        }
        public int HaveReports60
        {
            get { return this._haveReports60; }
            set { this._haveReports60 = value; }
        }
        public int HaveReportsOld
        {
            get { return this._haveReportsOld; }
            set { this._haveReportsOld = value; }
        }
        public string HaveRemoteControl
        {
            get { return this._haveRemoteControl; }
            set { this._haveRemoteControl = value; }
        }
        public int HaveCustomMessage
        {
            get { return this._haveCustomMessage; }
            set { this._haveCustomMessage = value; }
        }

        public string[] Roles { get; set; }

        public int SubscriptionLevelId{ get; set; }

        // default constructor
        public UserModel()
        {
            this._login = "";
            this._pin = "";
            this._newPin = "";
            this._browserInfo = "";
            this._urlInfo = "";
            this._timeDiff = 0;

            // user info
            this._clientId = 0;
            this._employeeId = 0;
            this._sessionId = 0;
            this._errorId = 0;
            this._winResizable = 0;
            this._userName = "";
            this._employeeName = "";
            this._errorDescription = "";
            this._firstUrl = "";
            this._firstName = "";
            this._unitName = "";

            // user features
            this._haveReports = 0;
            this._haveOptions = 0;
            this._haveMessaging = 0;
            this._haveMapReplay = 0;
            this._feedUrl = "";
            this._haveP2S2Status = 0;
            this._isHomePoint = 0;
            this._haveLiteAdmin = 0;
            this._haveUserPropstatus = 0;
            this._havePolygonSave = 0;
            this._haveGoogleEarth = 0;
            this._haveChat = 0;
            this._haveOpenStreet = 0;
            this._haveReports60 = 0;
            this._haveReportsOld = 0;
            //this._haveRemoteControl = 0;
            this._haveCustomMessage = 0;
        }
        //parametarized constructor
        public UserModel(string login, string pin, string newPin, string browserInfo, string urlInfo, int timeDiff)
        {
            this._login = login;
            this._pin = pin;
            this._newPin = newPin;
            this._browserInfo = browserInfo;
            this._urlInfo = urlInfo;
            this._timeDiff = timeDiff;
        }

        //parametarized constructor
        public UserModel(int clientId, int employeeId, int sessionId, int errorId, int winResizable, string userName, string employeeName,
            string errorDescription, string firstUrl, string firstName, string unitName)
        {
            this._clientId = clientId;
            this._employeeId = employeeId;
            this._sessionId = sessionId;
            this._errorId = errorId;
            this._winResizable = winResizable;
            this._userName = userName;
            this._employeeName = employeeName;
            this._errorDescription = errorDescription;
            this._firstUrl = firstUrl;
            this._firstName = firstName;
            this._unitName = unitName;
        }

        //parametarized constructor
        public UserModel(int haveReports, int haveOptions, int haveMessaging, int haveMapReplay, string feedUrl, int haveP2S2Status, int isHomePoint,
                        int haveLiteAdmin, int haveUserPropstatus, int havePolygonSave, int haveGoogleEarth, int haveChat, int haveOpenStreet, int haveReports60,
                        int haveReportsOld, string haveRemoteControl, int haveCustomMessage)
        {
            this._haveReports = haveReports;
            this._haveOptions = haveOptions;
            this._haveMessaging = haveMessaging;
            this._haveMapReplay = haveMapReplay;
            this._feedUrl = feedUrl;
            this._haveP2S2Status = haveP2S2Status;
            this._isHomePoint = isHomePoint;
            this._haveLiteAdmin = haveLiteAdmin;
            this._haveUserPropstatus = haveUserPropstatus;
            this._havePolygonSave = havePolygonSave;
            this._haveGoogleEarth = haveGoogleEarth;
            this._haveChat = haveChat;
            this._haveOpenStreet = haveOpenStreet;
            this._haveReports60 = haveReports60;
            this._haveReportsOld = haveReportsOld;
            this._haveRemoteControl = haveRemoteControl;
            this._haveCustomMessage = haveCustomMessage;
        }

        public int EmployeeClientId { get; set; }

        [DisplayName("Cell Phone Address:")]
        public string CellPhoneAddr { get; set; }

        public bool IsTSSAdmin { get; set; }
        public bool IsTSSUser { get; set; }
        public bool IsTSSQuoteApproval { get; set; }
    }
}
