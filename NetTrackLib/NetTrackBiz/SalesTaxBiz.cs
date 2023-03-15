using NetTrackModel;
using NetTrackRepository;
using System.Collections.Generic;

namespace NetTrackBiz
{
    internal class SalesTaxBiz
    {
        private SalesTaxRepository _SalesTaxRepository;

        public SalesTaxBiz()
        {
            _SalesTaxRepository = new SalesTaxRepository();
        }

        public List<SalesTaxModel> GetSalesTaxList()
        {
            return _SalesTaxRepository.GetSalesTaxList();
        }

        public SalesTaxModel SaveSalesTax(SalesTaxModel model)
        {
            return _SalesTaxRepository.SaveSalesTax(model);
        }
    }
}
