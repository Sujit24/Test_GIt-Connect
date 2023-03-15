using System;
using System.Collections.Generic;
using NetTrackRepository;
using NetTrackModel;

namespace NetTrackBiz
{
    internal class UserBiz
    {
        private UserRepository _userRepository;
        private UserModel _userModel;
        private UserModel _userModelDetails;
        private PreferenceModel _userpreferenceModel;
        private List<UserModel> _userModelList;
        private List<DdlSourceModel> _ddlSourceModelList;

        //default constructor
        public UserBiz()
        {
            _userRepository = new UserRepository();
            _userModel = new UserModel();
            _userModelDetails = new UserModel();
        }

        // this function creates user model
        public UserModel GetUserInfo(UserModel userModel)
        {
            _userRepository = new UserRepository();
            _userModel = new UserModel();
            _userModel = _userRepository.GetUserInfo(userModel);

            return _userModel;
        }

        public UserModel GetUserLoginInfo(UserModel userModel)
        {
            _userRepository = new UserRepository();
            _userModel = userModel;
            _userModel = _userRepository.GetUserLoginInfo(userModel);
            return _userModel;
        }

        // this function creates list of user models including user features
        public UserModel GetUserDetailInfo(UserModel userModel)
        {
            _userRepository = new UserRepository();
            _userModelDetails = _userRepository.GetUserDetailInfo(userModel);
            return _userModelDetails;
        }

        public List<UserModel> GetUserList(UserModel userModel)
        {
            _userRepository = new UserRepository();
            _userModelList = _userRepository.GetUserList(userModel);

            return _userModelList;
        }
        public UgUserModelWrapper GetUserList(UgUserModel ugUserModel)
        {
            _userRepository = new UserRepository();
            return _userRepository.GetUserList(ugUserModel);
        }
        public UserModel GetDetailUserInfo(UserModel userModel)
        {
            _userRepository = new UserRepository();
            _userModel = _userRepository.GetDetailUserInfo(userModel);

            return _userModel;
        }

        public PreferenceModel GetPreferenceDetailInfo(UserModel userModel)
        {
            _userpreferenceModel = new PreferenceModel();
            _userRepository = new UserRepository();
            _userpreferenceModel = _userRepository.GetPreferenceDetailInfo(userModel);

            return _userpreferenceModel;
        }

        public List<DdlSourceModel> GetUserTypeList(UserModel userModel)
        {
            _userRepository = new UserRepository();
            _ddlSourceModelList = _userRepository.GetUserTypeList(userModel);

            return _ddlSourceModelList;
        }

        public List<DdlSourceModel> GetTimeZoneList(UserModel userModel)
        {
            _userRepository = new UserRepository();
            _ddlSourceModelList = _userRepository.GetTimeZoneList(userModel);

            return _ddlSourceModelList;
        }

        public List<DdlSourceModel> GetMapZoomList(UserModel userModel)
        {
            _userRepository = new UserRepository();
            _ddlSourceModelList = _userRepository.GetMapZoomList(userModel);

            return _ddlSourceModelList;
        }

        public List<DdlSourceModel> GetMapCenterList(UserModel userModel)
        {
            _userRepository = new UserRepository();
            _ddlSourceModelList = _userRepository.GetMapCenterList(userModel);

            return _ddlSourceModelList;
        }

        public List<DdlSourceModel> GetFeatureList(UserModel userModel)
        {
            _userRepository = new UserRepository();
            _ddlSourceModelList = _userRepository.GetFeatureList(userModel);

            return _ddlSourceModelList;
        }

        public List<DdlSourceModel> GetSelectedFeatureList(UserModel userModel)
        {
            _userRepository = new UserRepository();
            _ddlSourceModelList = _userRepository.GetSelectedFeatureList(userModel);

            return _ddlSourceModelList;
        }

        public List<DdlSourceModel> GetContactTypeList(UserModel userModel)
        {
            _userRepository = new UserRepository();
            _ddlSourceModelList = _userRepository.GetContactTypeList(userModel);

            return _ddlSourceModelList;
        }

        public List<ChkBoxListSourceModel> GetMapClassList(UserModel userModel)
        {
            _userRepository = new UserRepository();
            List<ChkBoxListSourceModel> chkBoxListModel = new List<ChkBoxListSourceModel>();
            chkBoxListModel = _userRepository.GetMapClassList(userModel);

            return chkBoxListModel;
        }

        public List<ContactModel> GetEmployeeContacts(UserModel userModel)
        {
            _userRepository = new UserRepository();
            List<ContactModel> contactModelList = new List<ContactModel>();
            contactModelList = _userRepository.GetEmployeeContacts(userModel);

            return contactModelList;
        }

        public int InsertEmployeeContact(UserModel userModel)
        {
            _userRepository = new UserRepository();
            return _userRepository.InsertEmployeeContact(userModel);
        }

        public int DeleteEmployeeContact(UserModel userModel)
        {
            _userRepository = new UserRepository();
            return _userRepository.DeleteEmployeeContact(userModel);
        }

        public int InsertFeatures(UserModel userModel)
        {
            _userRepository = new UserRepository();
            return _userRepository.InsertFeatures(userModel);
        }

        public int DeleteFeatures(UserModel userModel)
        {
            _userRepository = new UserRepository();
            return _userRepository.DeleteFeatures(userModel);
        }

        public UserModel SaveEmployee(UserModel userModel)
        {
            _userRepository = new UserRepository();
            return _userRepository.SaveEmployee(userModel);
        }

        public int SavePreferences(UserModel userModel)
        {
            _userRepository = new UserRepository();
            return _userRepository.SavePreferences(userModel);
        }

        public string GetGeneratedPassword(int SessionId)
        {
            _userRepository = new UserRepository();
            return _userRepository.GetGeneratedPassword(SessionId);
        }

        public string GetFeedUrl(UserModel userModel)
        {
            _userRepository = new UserRepository();
            return _userRepository.GetFeedUrl(userModel);
        }

        public int UpdateLegends(int SessionId, int EmployeeId, string legends)
        {
            _userRepository = new UserRepository();
            return _userRepository.UpdateLegends(SessionId, EmployeeId, legends);
        }

        public List<UserModel> GetTSSAdminList()
        {
            return new UserRepository().GetTSSAdminList();
        }
    }
}
