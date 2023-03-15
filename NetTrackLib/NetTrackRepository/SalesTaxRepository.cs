using System;
using System.Data;
using NetTrackDBContext;
using NetTrackModel;
using System.Collections.Generic;

namespace NetTrackRepository
{
    public class SalesTaxRepository
    {
        private SalesTaxModel _SalesTaxModel;
        private DBSalesTax _DBSalesTax;

        public SalesTaxRepository()
        {
            this._DBSalesTax = new DBSalesTax();
        }

        public List<SalesTaxModel> GetSalesTaxList()
        {
            List<SalesTaxModel> salesTaxList = new List<SalesTaxModel>();
            DataTable dtBluePaySettings = _DBSalesTax.GetSalesTaxList();
            foreach (DataRow dr in dtBluePaySettings.Rows)
            {
                _SalesTaxModel = new SalesTaxModel();
                _SalesTaxModel.SalesTaxId = int.Parse(dr["SalesTaxId"].ToString());
                _SalesTaxModel.Country = dr["Country"].ToString();
                _SalesTaxModel.StateFullName = dr["StateFullName"].ToString();
                _SalesTaxModel.StateShortName = dr["StateShortName"].ToString();
                _SalesTaxModel.TaxRate = double.Parse(dr["TaxRate"].ToString());

                salesTaxList.Add(_SalesTaxModel);
            }

            return salesTaxList;
        }

        public SalesTaxModel SaveSalesTax(SalesTaxModel model)
        {
            return _DBSalesTax.SaveSalesTax(model);
        }
    }
}
