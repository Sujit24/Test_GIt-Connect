using System;
using System.Collections.Generic;
using NetTrackRepository;
using NetTrackModel;

namespace NetTrackBiz
{
    class UserStateBiz 
    {
        private UserStateRepository _userStateRepository;
        private UserStateModel _userstateModel;

        //default constructor
        public UserStateBiz() {
            _userStateRepository = new UserStateRepository();
            _userstateModel = new UserStateModel();
        }

        // this function creates user model
        public UserStateModel MaintainUserStateGridPage(UserStateModel userStateModel)
        {
            _userStateRepository = new UserStateRepository();
            _userstateModel = new UserStateModel();
            _userstateModel = _userStateRepository.MaintainUserStateGridPage(userStateModel);           
           
            return _userstateModel;
        }
        public UserStateModel MaintainUserState(UserStateModel userStateModel)
        {
            _userStateRepository = new UserStateRepository();
            _userstateModel = new UserStateModel();
            _userstateModel = _userStateRepository.MaintainUserState(userStateModel);           
           
            return _userstateModel;
        }
    }
}
