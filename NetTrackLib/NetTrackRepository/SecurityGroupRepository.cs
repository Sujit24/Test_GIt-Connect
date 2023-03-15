using NetTrackDBContext;
using NetTrackModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace NetTrackRepository
{
    public class SecurityGroupRepository
    {
        private SecurityGroupModel _SecurityGroupModel;
        private DBSecurityGroup _DBSecurityGroup;

        public SecurityGroupRepository()
        {
            this._DBSecurityGroup = new DBSecurityGroup();
        }

        public List<SecurityGroupModel> GetAllSecurityGroup()
        {
            List<SecurityGroupModel> securityGroupList = new List<SecurityGroupModel>();
            DataTable dtSecurityGroup = _DBSecurityGroup.GetAllSecurityGroup();

            foreach (DataRow dr in dtSecurityGroup.Rows)
            {
                _SecurityGroupModel = new SecurityGroupModel();
                _SecurityGroupModel.SecurityGroupId = Convert.ToInt32(dr["TssGroupId"]);
                _SecurityGroupModel.GroupName = dr["GroupName"].ToString();

                securityGroupList.Add(_SecurityGroupModel);
            }

            return securityGroupList;
        }

        public SecurityGroupModel GetSecurityGroupById(long securityGroupId)
        {
            DataTable dtSecurityGroup = _DBSecurityGroup.GetAllSecurityGroup();

            foreach (DataRow dr in dtSecurityGroup.Rows)
            {
                _SecurityGroupModel = new SecurityGroupModel();
                _SecurityGroupModel.SecurityGroupId = Convert.ToInt32(dr["TssGroupId"]);
                _SecurityGroupModel.GroupName = dr["GroupName"].ToString();

                break;
            }

            return _SecurityGroupModel;
        }

        public void SaveSecurityGroup(SecurityGroupModel model)
        {
            model.Action = model.SecurityGroupId == 0 ? "I" : "U";

            _DBSecurityGroup.SaveSecurityGroup(model);
        }

        public void DeleteSecurityGroup(SecurityGroupModel model)
        {
            model.Action = "D";

            _DBSecurityGroup.SaveSecurityGroup(model);
        }
    }
}
