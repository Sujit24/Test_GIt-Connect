using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetTrackModel;
using System.ComponentModel.DataAnnotations;
namespace TSS.Models.ViewModel
{
    public class ProductViewModel : ProductModel
    {
        public List<DdlSourceModel> ProductTypeList { get; set; }
        public List<DdlSourceModel> CarrierList { get; set; }

        //[DataType(DataType.Upload)]
        public HttpPostedFileBase HardwareImageUpload { get; set; }
    }
}