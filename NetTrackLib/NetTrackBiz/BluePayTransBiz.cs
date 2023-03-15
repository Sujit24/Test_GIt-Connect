using NetTrackModel;
using NetTrackRepository;
using System.Collections.Generic;

namespace NetTrackBiz
{
    public class BluePayTransBiz
    {
        private BluePayTransRepository _BluePayTransRepository;

        public BluePayTransBiz()
        {
            _BluePayTransRepository = new BluePayTransRepository();
        }

        public BluePayTransactionModel SaveBluePayTrans(BluePayTransactionModel model)
        {
            return _BluePayTransRepository.SaveBluePayTrans(model);
        }

        public BluePayACHTransactionModel SaveBluePayACHTrans(BluePayACHTransactionModel model)
        {
            return _BluePayTransRepository.SaveBluePayACHTrans(model);
        }

    }
}
