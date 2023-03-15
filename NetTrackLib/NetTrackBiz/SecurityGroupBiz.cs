using NetTrackModel;
using NetTrackRepository;
using System.Collections.Generic;

namespace NetTrackBiz
{
    public class SecurityGroupBiz
    {
        private SecurityGroupRepository _SecurityGroupRepository;

        public SecurityGroupBiz()
        {
            _SecurityGroupRepository = new SecurityGroupRepository();
        }

        public List<SecurityGroupModel> GetAllSecurityGroup()
        {
            return _SecurityGroupRepository.GetAllSecurityGroup();
        }

        public SecurityGroupModel GetSecurityGroupById(long securityGroupId)
        {
            return _SecurityGroupRepository.GetSecurityGroupById(securityGroupId);
        }

        public void SaveSecurityGroup(SecurityGroupModel model)
        {
            _SecurityGroupRepository.SaveSecurityGroup(model);
        }

        public void DeleteSecurityGroup(SecurityGroupModel model)
        {
            _SecurityGroupRepository.DeleteSecurityGroup(model);
        }
    }
}
