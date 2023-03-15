using NetTrackModel;
using NetTrackRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackBiz
{
    public class SecurityGroupSalesPersonBiz
    {
        private SecurityGroupSalesPersonRepository _SecurityGroupSalesPersonRepository;

        public SecurityGroupSalesPersonBiz()
        {
            _SecurityGroupSalesPersonRepository = new SecurityGroupSalesPersonRepository();
        }

        public List<SecurityGroupSalesPersonModel> GetSalesPersonsBySecurityGroupId(SecurityGroupSalesPersonModel model)
        {
            return _SecurityGroupSalesPersonRepository.GetSalesPersonsBySecurityGroupId(model);
        }

        public List<SecurityGroupSalesPersonModel> GetSalesPersonsByEmployeeId(SecurityGroupSalesPersonModel model)
        {
            return _SecurityGroupSalesPersonRepository.GetSalesPersonsByEmployeeId(model);
        }

        public void SaveSalesPersonSecurityGroup(SecurityGroupSalesPersonModel model)
        {
            _SecurityGroupSalesPersonRepository.SaveSalesPersonSecurityGroup(model);
        }

        public void DeleteSalesPersonSecurityGroup(SecurityGroupSalesPersonModel model)
        {
            _SecurityGroupSalesPersonRepository.DeleteSalesPersonSecurityGroup(model);
        }
    }
}
