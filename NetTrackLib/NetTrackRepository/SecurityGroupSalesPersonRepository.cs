using NetTrackDBContext;
using NetTrackModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace NetTrackRepository
{
    public class SecurityGroupSalesPersonRepository
    {
        private SecurityGroupSalesPersonModel _SecurityGroupSalesPersonModel;
        private DBSecurityGroupSalesPerson _DBSecurityGroupSalesPerson;

        public SecurityGroupSalesPersonRepository()
        {
            this._DBSecurityGroupSalesPerson = new DBSecurityGroupSalesPerson();
        }

        public List<SecurityGroupSalesPersonModel> GetSalesPersonsBySecurityGroupId(SecurityGroupSalesPersonModel model)
        {
            List<SecurityGroupSalesPersonModel> securityGroupSalesPersonModelList = new List<SecurityGroupSalesPersonModel>();

            model.Action = "ByGroupId";
            DataTable dtSecurityGroup = _DBSecurityGroupSalesPerson.GetSalesPersons(model);

            foreach (DataRow dr in dtSecurityGroup.Rows)
            {
                _SecurityGroupSalesPersonModel = new SecurityGroupSalesPersonModel();
                _SecurityGroupSalesPersonModel.SecurityGroupSalesPersonId = Convert.ToInt32(dr["TssGroupSalesPersonId"]);
                _SecurityGroupSalesPersonModel.SecurityGroupId = Convert.ToInt32(dr["TssGroupId"]);
                _SecurityGroupSalesPersonModel.EmployeeId = Convert.ToInt32(dr["EmployeeId"]);
                _SecurityGroupSalesPersonModel.FirstName = dr["First_Name"] == DBNull.Value ? "" :  dr["First_Name"].ToString();
                _SecurityGroupSalesPersonModel.LastName = dr["Last_Name"] == DBNull.Value ? "" :  dr["Last_Name"].ToString();
                _SecurityGroupSalesPersonModel.WebLogin = dr["LOGIN"] == DBNull.Value ? "" : dr["LOGIN"].ToString();
                _SecurityGroupSalesPersonModel.IsInGroup = Convert.ToBoolean(dr["IsInGroup"]);

                securityGroupSalesPersonModelList.Add(_SecurityGroupSalesPersonModel);
            }

            return securityGroupSalesPersonModelList;
        }

        public List<SecurityGroupSalesPersonModel> GetSalesPersonsByEmployeeId(SecurityGroupSalesPersonModel model)
        {
            List<SecurityGroupSalesPersonModel> securityGroupSalesPersonModelList = new List<SecurityGroupSalesPersonModel>();

            model.Action = "ByEmployeeId";
            DataTable dtSecurityGroup = _DBSecurityGroupSalesPerson.GetSalesPersons(model);

            foreach (DataRow dr in dtSecurityGroup.Rows)
            {
                _SecurityGroupSalesPersonModel = new SecurityGroupSalesPersonModel();                
                _SecurityGroupSalesPersonModel.EmployeeId = Convert.ToInt32(dr["EmployeeId"]);

                securityGroupSalesPersonModelList.Add(_SecurityGroupSalesPersonModel);
            }

            return securityGroupSalesPersonModelList;
        }

        public void SaveSalesPersonSecurityGroup(SecurityGroupSalesPersonModel model)
        {
            model.Action = model.SecurityGroupSalesPersonId == 0 ? "I" : "U";

            _DBSecurityGroupSalesPerson.SaveSecurityGroupSalesPerson(model);
        }

        public void DeleteSalesPersonSecurityGroup(SecurityGroupSalesPersonModel model)
        {
            model.Action = "D";

            _DBSecurityGroupSalesPerson.SaveSecurityGroupSalesPerson(model);
        }
    }
}
