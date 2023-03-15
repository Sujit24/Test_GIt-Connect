using System;
using System.Collections.Generic;
using NetTrackModel;
using NetTrackRepository;

namespace NetTrackBiz
{
    internal class CommonBiz
    {
        private CommonRepository _commonRepository;

        // Default constructor
        public CommonBiz()
        {
            _commonRepository = new CommonRepository();
        }

        // Get point info from PointRepository class
        public List<IDdlSourceModel> GetDdlDataSource(IDdlSourceModel ddlSourceModel)
        {
            CommonRepository _commonRepository = new CommonRepository();

            return _commonRepository.GetDdlsource(ddlSourceModel);
        }
        public List<IDdlSourceModel> GetDdlDataSource(string spName,string param)
        {
            CommonRepository _commonRepository = new CommonRepository();

            return _commonRepository.GetDdlsource(spName,param);
        }
    }
}