using NetTrackDBContext;
using NetTrackModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace NetTrackRepository
{
    public class TemplateGroupRepository
    {
        private TemplateGroupModel _TemplateGroupModel;
        private DBTemplateGroup _DBTemplateGroup;

        public TemplateGroupRepository()
        {
            this._DBTemplateGroup = new DBTemplateGroup();
        }

        public List<TemplateGroupModel> GetAllTemplateGroup()
        {
            List<TemplateGroupModel> TemplateGroupList = new List<TemplateGroupModel>();
            DataTable dtTemplateGroup = _DBTemplateGroup.GetAllTemplateGroup();

            foreach (DataRow dr in dtTemplateGroup.Rows)
            {
                _TemplateGroupModel = new TemplateGroupModel();
                _TemplateGroupModel.TemplateGroupId = Convert.ToInt32(dr["QuoteTemplateGroupId"]);
                _TemplateGroupModel.GroupName = dr["GroupName"].ToString();

                TemplateGroupList.Add(_TemplateGroupModel);
            }

            return TemplateGroupList;
        }

        public void SaveTemplateGroup(TemplateGroupModel model)
        {
            model.Action = model.TemplateGroupId == 0 ? "I" : "U";

            _DBTemplateGroup.SaveTemplateGroup(model);
        }

        public void DeleteTemplateGroup(TemplateGroupModel model)
        {
            model.Action = "D";

            _DBTemplateGroup.SaveTemplateGroup(model);
        }
    }
}
