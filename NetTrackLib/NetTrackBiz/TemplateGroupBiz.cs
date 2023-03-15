using NetTrackModel;
using NetTrackRepository;
using System.Collections.Generic;

namespace NetTrackBiz
{
    public class TemplateGroupBiz
    {
        private TemplateGroupRepository _TemplateGroupRepository;

        public TemplateGroupBiz()
        {
            _TemplateGroupRepository = new TemplateGroupRepository();
        }

        public List<TemplateGroupModel> GetAllTemplateGroup()
        {
            return _TemplateGroupRepository.GetAllTemplateGroup();
        }

        public void SaveTemplateGroup(TemplateGroupModel model)
        {
            _TemplateGroupRepository.SaveTemplateGroup(model);
        }

        public void DeleteTemplateGroup(TemplateGroupModel model)
        {
            _TemplateGroupRepository.DeleteTemplateGroup(model);
        }
    }
}
