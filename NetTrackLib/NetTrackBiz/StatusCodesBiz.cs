using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetTrackModel;
using NetTrackModel.Params;
using NetTrackRepository;

namespace NetTrackBiz
{
    public class StatusCodesBiz
    {
        StatusCodesRepository _SR;
        
        public StatusCodesBiz()
        {
            _SR = new StatusCodesRepository();
        }
        
        public OutParams saveStatusCode(StatusCodesInputParams ip, UgsStatusCodeModel m)
        {
            return _SR.saveStatusCode(ip, m);
        }

        public List<UgsStatusCodeModel> getStatusCodeList(StatusCodesInputParams ip)
        {
            return _SR.getStatusCodeList(ip);
        }

        public UgsStatusCodeModel getStatusCode(StatusCodesInputParams ip)
        {
            return _SR.getStatusCode(ip);
        }

    }
}
