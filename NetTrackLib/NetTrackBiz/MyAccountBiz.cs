using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetTrackModel;
using NetTrackRepository;

namespace NetTrackBiz
{
    internal class MyAccountBiz
    {
        private List<DdlSourceModel> _ddlSourceModelList;
        private MyAccountModel _myAccountModel;
        private MyAccountRepository _myAccountRepository;

        public MyAccountBiz()
        {
            _myAccountRepository = new MyAccountRepository();
        }

        public MyAccountModel GetMyAccountInfo(int clientID)
        {
            return _myAccountRepository.GetMyAccountInfo(clientID);
        }

        public int SaveMyAccountInfo(MyAccountModel myAccountModel)
        {
            return _myAccountRepository.SaveMyAccountInfo(myAccountModel);
        }
    }
}
