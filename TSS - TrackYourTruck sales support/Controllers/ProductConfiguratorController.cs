using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GSA.Security;
using NetTrackBiz;
using NetTrackModel;
using TSS.Helper;
using TSS.Models.ViewModel;
using System.Web;
using System.IO;
using System.Linq;
using TYT.Helper;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class ProductConfiguratorController : Controller
    {
        [GSAAuthorizeAttribute()]
        public ActionResult Index()
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            if (!SessionVars.CurrentLoggedInUser.IsTSSAdmin)
            {
                return RedirectToAction("List", "Quote");
            }

            return View();
        }

        [HttpPost, ActionName("GetProductList")]
        public ContentResult GetProductList(FormCollection form, string exactMatch)
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            List<ProductModel> productModelList = new List<ProductModel>();

            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                productModelList = tytFacadeBiz.GetProductList(0);

                if (!string.IsNullOrWhiteSpace(Request.QueryString["FromProductPage"]) && Request.QueryString["FromProductPage"] == "1")
                {
                    foreach (ProductModel item in productModelList)
                    {
                        if (item.ProductTypeName == "Service")
                        {
                            item.ProductTypeName = "Optional";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return LargeJsonResult(productModelList);
        }

        public ContentResult LargeJsonResult(List<ProductModel> productModelList)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here  
            ContentResult result = new ContentResult();
            result.Content = serializer.Serialize(productModelList);
            result.ContentType = "application/json";

            return result;
        }

        [HttpPost]
        public JsonResult GetHardwareProducts(List<int> id)
        {
            List<ProductModel> productModelList = new List<ProductModel>();

            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                productModelList = tytFacadeBiz.GetProductList(0);

                if (id.Count > 1)
                {
                    productModelList = productModelList.Where(s => id.Contains(s.ProductId)).ToList();
                }
                else
                {
                    ProductModel selectedProduct = productModelList.FirstOrDefault(s => s.ProductId == id[0]);
                    if (selectedProduct != null)
                    {
                        productModelList = productModelList.Where(w => w.ProductTypeName == "Hardware" && w.Carrier == selectedProduct.Carrier).ToList();
                    }
                    else
                    {
                        productModelList = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(productModelList);
        }

        [GSAAuthorizeAttribute()]
        [ActionName("New")]
        public ActionResult AddNewProduct()
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            if (!SessionVars.CurrentLoggedInUser.IsTSSAdmin)
            {
                return RedirectToAction("List", "Quote");
            }

            ViewBag.Title = "New Product";
            ViewBag.EditMode = true;
            ProductViewModel productViewmodel = new ProductViewModel();

            try
            {
                productViewmodel = LoadProductData(0);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return View("Product", productViewmodel);
        }

        [ActionName("Edit"), GSAAuthorizeAttribute()]
        public ActionResult EditProduct(long id)
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            if (!SessionVars.CurrentLoggedInUser.IsTSSAdmin)
            {
                return RedirectToAction("List", "Quote");
            }

            ViewBag.Title = "View/Edit Prodcuts";
            ViewBag.EditMode = true;
            int productID = (int)id;
            ProductViewModel productviewwmodel = new ProductViewModel();

            try
            {
                productviewwmodel = LoadProductData(productID);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return View("Product", productviewwmodel);
        }

        private ProductViewModel LoadProductData(int productID)
        {
            ProductModel productModel = new ProductModel();
            ProductViewModel productViewModel = new ProductViewModel();

            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
            productModel.ProductId = productID;
            productModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
            if (productID > 0)
            {
                productModel = tytFacadeBiz.GetDetailProductInfo(productModel);
            }

            PropertyCopier<ProductModel, ProductViewModel>.CopyPropertyValues(productModel, productViewModel);

            List<DdlSourceModel> productTypeList = tytFacadeBiz.GetProductTypeList(productModel);
            productViewModel.ProductTypeList = productTypeList;

            productViewModel.CarrierList = new List<DdlSourceModel>();
            productViewModel.CarrierList.Add(new DdlSourceModel { keyfield = "", value = "" });
            productViewModel.CarrierList.Add(new DdlSourceModel { keyfield = "Kore/Verizon", value = "Kore/Verizon" });
            productViewModel.CarrierList.Add(new DdlSourceModel { keyfield = "Kore/AT&T", value = "Kore/AT&T" });
            productViewModel.CarrierList.Add(new DdlSourceModel { keyfield = "GlobalStar", value = "GlobalStar" });
            productViewModel.CarrierList.Add(new DdlSourceModel { keyfield = "Iridium", value = "Iridium" });
            productViewModel.CarrierList.Add(new DdlSourceModel { keyfield = "Kore/T-Mobile", value = "Kore/T-Mobile" });

            return productViewModel;
        }

        [HttpPost, ActionName("Save"), GSAAuthorizeAttribute()]
        public ActionResult Save(ProductViewModel item)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            try
            {
                if (ModelState.IsValid)
                {
                    if (item.HardwareImageUpload != null)
                    {
                        string imageFileName = string.Format("{0}{1}", Guid.NewGuid().ToString("N"), Path.GetExtension(item.HardwareImageUpload.FileName));
                        item.HardwareImageUpload.SaveAs(Server.MapPath("~/Content/ProductImage/") + imageFileName);
                        item.ProductImageFileName = imageFileName;
                    }

                    item.Action = item.ProductId == 0 ? "I" : "U";
                    item.SessionId = SessionVars.CurrentLoggedInUser.SessionId;

                    ProductModel Model = new ProductModel();
                    Model = item;
                    Model = tytFacadeBiz.SaveProduct(Model);
                    item.ProductId = Model.ProductId;

                    if (Model.ProductId <= 0)
                    {
                        throw new Exception("save failed");
                    }
                }
                else
                {
                    return Json(new AjaxResponse { Message = "Save failed." });
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Save failed." });
            }

            return Json(new AjaxResponse { PrimaryKey = item.ProductId, Message = "Successfully Saved." });
        }

        [HttpPost, ActionName("Delete"), GSAAuthorizeAttribute()]
        public JsonResult Delete(int PrimaryKey)
        {
            try
            {
                ProductModel item = new ProductModel();
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                item.ProductId = PrimaryKey;

                if (item.ProductId > 0)
                {
                    item.Action = "D";
                    item.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                    item = tytFacadeBiz.SaveProduct(item);

                    if (item.ProductId <= 0)
                    {
                        throw new Exception("Delete failed");
                    }
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Delete failed." });
            }

            return Json(new AjaxResponse { Message = "Successfully Deleted." });
        }
    }
}
