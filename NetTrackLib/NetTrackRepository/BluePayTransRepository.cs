using System;
using System.Data;
using NetTrackDBContext;
using NetTrackModel;
using System.Collections.Generic;

namespace NetTrackRepository
{
    public class BluePayTransRepository
    {
        private BluePayTransactionModel _BluePayTransactionModel;
        private DBBluePayTrans _DBBluePayTrans;

        public BluePayTransRepository()
        {
            _DBBluePayTrans = new DBBluePayTrans();
        }

        public BluePayTransactionModel SaveBluePayTrans(BluePayTransactionModel model)
        {
            return _DBBluePayTrans.SaveBluePayTrans(model);
        }

        public BluePayACHTransactionModel SaveBluePayACHTrans(BluePayACHTransactionModel model)
        {
            return _DBBluePayTrans.SaveBluePayACHTrans(model);
        }
    }
}
