using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSA.Security;
using TSS.Helper;
using NetTrackBiz;
using TSS.Models.ViewModel;
using NetTrackModel;
using System.Web.Script.Serialization;
using System.Configuration;
using TYT.Helper;
using System.Linq;
using System.IO;
using ExcelLibrary.SpreadSheet;
using UPS;
using UPS.Models;
using TSS.Models;
using System.Text;

namespace TSS.Controllers
{
    [RequireHttpsCustomAttribute]
    public class SalesOrderController : Controller
    {
        [GSAAuthorizeAttribute()]
        public ActionResult Index()
        {
            return View();
        }

        [GSAAuthorizeAttribute()]
        public ActionResult List()
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            /*ViewBag.SearchDateFrom = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).ToString("MM/dd/yyyy");
            ViewBag.SearchDateTo = DateTime.Today.AddDays(7).AddSeconds(-1).ToString("MM/dd/yyyy");*/
            ViewBag.SearchDateFrom = DateTime.Today.AddDays(-9).ToString("MM/dd/yyyy");
            ViewBag.SearchDateTo = DateTime.Today.ToString("MM/dd/yyyy");

            CrystalReportPrefetch.Prefetch(Server);

            return View();
        }

        [HttpPost, ActionName("GetSalesList"), GSAAuthorizeAttribute()]
        public ContentResult GetSalesList(FormCollection form, string exactMatch)
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            List<QuoteOrderModel> quoteOrderModelList = new List<QuoteOrderModel>();

            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

                QuoteOrderModel quoteOrderModel = new QuoteOrderModel();
                quoteOrderModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                quoteOrderModel.SearchFromDate = DateTime.Parse(Request.QueryString["dateFrom"]);
                quoteOrderModel.SearchToDate = DateTime.Parse(Request.QueryString["dateTo"]);

                if (SessionVars.CurrentLoggedInUser.IsTSSAdmin)
                {
                    quoteOrderModelList = tytFacadeBiz.GetSalesList(quoteOrderModel);
                }
                else
                {
                    quoteOrderModelList = tytFacadeBiz.GetSalesList(quoteOrderModel).Where(q => q.SalesPersonId == SessionVars.CurrentLoggedInUser.EmployeeId).ToList();
                }

                if (!string.IsNullOrWhiteSpace(Request.QueryString["searchText"]))
                {
                    if (Request.QueryString["searchSelect"] == "CustomerName")
                    {
                        quoteOrderModelList = quoteOrderModelList.Where(w => w.CustomerName.ToLower().Contains(Request.QueryString["searchText"].ToLower())).ToList();
                    }
                    else if (Request.QueryString["searchSelect"] == "SearchQuoteId")
                    {
                        quoteOrderModelList = quoteOrderModelList.Where(w => w.SearchQuoteId.Contains(Request.QueryString["searchText"])).ToList();
                    }
                    else if (Request.QueryString["searchSelect"] == "StatusTitle")
                    {
                        quoteOrderModelList = quoteOrderModelList.Where(w => w.StatusTitle.IndexOf(Request.QueryString["searchText"], StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    }
                    else if(Request.QueryString["searchSelect"] == "MethodOfPayment")
                    {
                        quoteOrderModelList = quoteOrderModelList.Where(w => w.PaymentMethod.ToLower().Contains(Request.QueryString["searchText"].ToLower())).ToList();
                    }
                }

                Session["GetSalesList_QuoteOrderModelList"] = quoteOrderModelList;

                foreach (QuoteOrderModel item in quoteOrderModelList)
                {
                    if (!item.IsShipped)
                    {
                        if (item.PurchaseDate.Value.AddHours(48) < DateTime.Now)
                        {
                            item.SalesOrderRowColor = "red";
                        }
                        else if (item.PurchaseDate.Value.AddHours(24) < DateTime.Now)
                        {
                            item.SalesOrderRowColor = "lightyellow";
                        }
                        else if (item.ShippingAndHandlingType == "2Day" || item.ShippingAndHandlingType == "NextAir")
                        {
                            item.SalesOrderRowColor = "lightgreen";
                        }
                        /*else if ((item.OrderStatusId == 2 || item.OrderStatusId == 4 || item.OrderStatusId == 1)
                            && item.ShippingAndHandlingType != "2Day" || item.ShippingAndHandlingType != "NextAir")
                        {
                            item.SalesOrderRowColor = "lightyellow";
                        }*/
                    }
                    else
                    {
                        item.SalesOrderRowColor = "";
                    }

                    item.Url = ConfigurationManager.AppSettings["UserViewUrl"] + "SalesOrder/UserView/?id=" + Server.UrlEncode(Cipher.Encrypt(item.QuoteOrderId.ToString()));
                    if (!string.IsNullOrWhiteSpace(item.CardNumber))
                    {
                        item.CardNumber = string.IsNullOrWhiteSpace(item.CardNumber) ? "" : Cipher.Decrypt(item.CardNumber);
                        item.CardNumber = item.CardNumber.Substring(item.CardNumber.Length - 4);
                    }
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return LargeJsonResult(quoteOrderModelList);
        }

        public ContentResult LargeJsonResult(List<QuoteOrderModel> quoteModelList)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here  
            ContentResult result = new ContentResult();
            result.Content = serializer.Serialize(quoteModelList);
            result.ContentType = "application/json";

            return result;
        }

        [HttpPost, ActionName("Save"), GSAAuthorizeAttribute()]
        public ActionResult SaveDataToDB(QuoteViewModel model)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            try
            {
                model.QuoteOrderModel.Action = model.QuoteOrderModel.QuoteOrderId == 0 ? "I" : "U";
                model.QuoteOrderModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;

                model.QuoteOrderModel = tytFacadeBiz.SaveQuoteOrder(model.QuoteOrderModel);
                if (model.QuoteOrderModel.QuoteOrderId <= 0)
                {
                    throw new Exception("save failed");
                }

                if (Session["GetQuoteSales_OrderStatusId"] != null & Convert.ToInt32(Session["GetQuoteSales_OrderStatusId"]) != model.QuoteOrderModel.OrderStatusId)
                {
                    tytFacadeBiz.SaveOrderStatusHistory(model.QuoteOrderModel.QuoteOrderId, model.QuoteOrderModel.OrderStatusId);
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Save failed." });
            }

            return Json(new AjaxResponse { PrimaryKey = model.QuoteOrderModel.QuoteOrderId, Message = "Successfully Saved." });
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public ActionResult SaveNettrackStatus(QuoteViewModel model)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            try
            {
                QuoteOrderModel quoteOrderModel = new QuoteOrderModel();
                quoteOrderModel.QuoteOrderId = model.QuoteOrderModel.QuoteOrderId;
                quoteOrderModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                quoteOrderModel = tytFacadeBiz.GetQuoteOrderInfo(quoteOrderModel);

                quoteOrderModel.ClientId = model.QuoteOrderModel.ClientId;
                quoteOrderModel.NettrackClientStatusId = model.QuoteOrderModel.NettrackClientStatusId;
                quoteOrderModel.Action = "U";
                quoteOrderModel = tytFacadeBiz.SaveQuoteOrder(quoteOrderModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Status save failed." });
            }

            return Json(new AjaxResponse { PrimaryKey = model.QuoteOrderModel.QuoteOrderId, Message = "Successfully Saved." });
        }

        [ActionName("Edit"), GSAAuthorizeAttribute()]
        public ActionResult EditQuote(long id)
        {
            if (SessionVars.CurrentLoggedInUser == null)
            {
                UserModel userModel = CookieManager.ReloadSessionFromCookie();
                SessionVars.CurrentLoggedInUser = userModel;
            }

            ViewBag.Title = "View/Edit Sales Order";
            ViewBag.QuoteOrderId = id;

            return View("Index");
        }

        [ActionName("GetQuoteSales"), GSAAuthorizeAttribute()]
        public JsonResult GetQuoteSales(int id)
        {
            QuoteOrderModel quoteOrderModel = new QuoteOrderModel();
            SalesOrderModel salesOrderModel = new SalesOrderModel();
            List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();

            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                quoteOrderModel.QuoteOrderId = id;
                quoteOrderModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                quoteOrderModel = tytFacadeBiz.GetQuoteOrderInfo(quoteOrderModel);

                Session["GetQuoteSales_OrderStatusId"] = quoteOrderModel.OrderStatusId;

                salesOrderModel.QuoteOrderId = id;
                salesOrderModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                salesOrderModelList = tytFacadeBiz.GetSalesOrderList(salesOrderModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(new { Quote = quoteOrderModel, SalesOrderList = salesOrderModelList });
        }

        [HttpPost, ActionName("Delete"), GSAAuthorizeAttribute()]
        public JsonResult Delete(int PrimaryKey)
        {
            try
            {
                QuoteOrderModel item = new QuoteOrderModel();
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                item.QuoteOrderId = PrimaryKey;

                if (item.QuoteOrderId > 0)
                {
                    item.Action = "D";
                    item.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                    item = tytFacadeBiz.SaveQuoteOrder(item);

                    if (item.QuoteOrderId <= 0)
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

        [ActionName("UserView")]
        public ActionResult UserView(string id)
        {
            ViewBag.Title = "Your Order Detail";
            ViewBag.QuoteOrderId = Cipher.Decrypt(id);
            ViewBag.SalesOrderPDF = ConfigurationManager.AppSettings["UserViewUrl"] + "SalesOrderRpt/SalesOrderPDF/?id=" + id;

            CrystalReportPrefetch.Prefetch(Server);

            return View();
        }

        [ActionName("UserViewGetOrder")]
        public JsonResult UserViewGetOrder(int id)
        {
            QuoteOrderModel quoteOrderModel = new QuoteOrderModel();
            SalesOrderModel salesOrderModel = new SalesOrderModel();
            List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();

            try
            {
                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                quoteOrderModel.QuoteOrderId = id;
                quoteOrderModel.SessionId = 0;
                quoteOrderModel = tytFacadeBiz.GetQuoteOrderInfo(quoteOrderModel);

                salesOrderModel.QuoteOrderId = id;
                salesOrderModel.SessionId = 0;
                salesOrderModelList = tytFacadeBiz.GetSalesOrderList(salesOrderModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(new { QuoteOrder = quoteOrderModel, SalesOrderList = salesOrderModelList });
        }

        [HttpPost, ActionName("GetOrderStatus"), GSAAuthorizeAttribute()]
        public JsonResult GetOrderStatus()
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            return Json(tytFacadeBiz.GetOrderStatusList(SessionVars.CurrentLoggedInUser.SessionId));
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public ActionResult SetQuoteOrderStatus(QuoteOrderModel model)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            try
            {
                tytFacadeBiz.SetQuoteOrderStatus(model);

                tytFacadeBiz.SaveOrderStatusHistory(model.QuoteOrderId, model.OrderStatusId);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new AjaxResponse { Message = "Save failed." });
            }

            return Json(new AjaxResponse { Message = "Successfully Saved." });
        }

        public ActionResult ExportToExcel(FormCollection form)
        {
            MemoryStream excelStream = new MemoryStream();
            Workbook workbook = GetExportData(form);
            workbook.SaveToStream(excelStream);

            string attachment = "attachment; filename=SalesOrder.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            excelStream.WriteTo(Response.OutputStream);
            Response.End();

            return View();
        }

        public Workbook GetExportData(FormCollection form)
        {
            try
            {
                string[] columnList = new string[] { "Order No", "Customer Name", "Purchase Date", "NetTrack Status", "Order Status", "Method Of Payment", "BluePay Trans. No", "Last 4 of CC", "CC Exp Date" };
                Workbook workbook = new Workbook();
                Worksheet worksheet = new Worksheet("Sheet1");
                CellFormat cellFormat = new CellFormat();
                cellFormat.HasBorder = true;

                //Build table header
                int courrColIndex = 0;
                for (int i = 0; i < columnList.Length; i++)
                {
                    worksheet.Cells[0, courrColIndex] = new Cell(columnList[i].ToString(), cellFormat);

                    //Auto space
                    SetExcelAutoSpace(worksheet.Cells, 0, courrColIndex);
                    courrColIndex++;
                }

                //Build table body
                int rowIndex = 1;
                List<QuoteOrderModel> salesOrderList = Session["GetSalesList_QuoteOrderModelList"] as List<QuoteOrderModel>;
                foreach (QuoteOrderModel quoteOrder in salesOrderList)
                {
                    courrColIndex = 0;
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(quoteOrder.QuoteId, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(quoteOrder.CustomerName, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(quoteOrder.PurchaseDateFormated, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(quoteOrder.NettrackStatus, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(quoteOrder.StatusTitle, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(quoteOrder.PaymentMethod, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(quoteOrder.TransactionId, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(quoteOrder.CardNumber, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);
                    worksheet.Cells[rowIndex, courrColIndex] = new Cell(quoteOrder.CardExpire, cellFormat);
                    SetExcelAutoSpace(worksheet.Cells, rowIndex, courrColIndex++);

                    rowIndex++;
                }

                #region Excel Fix

                //Column wise
                for (int i = 0; i < rowIndex; i++)
                {
                    for (int j = columnList.Length; j < 26; j++)
                    {
                        worksheet.Cells[i, j] = new Cell(" ");
                    }
                }

                //Row wise
                for (int i = rowIndex; i < 11; i++)
                {
                    for (int j = 0; j < 26; j++)
                    {
                        worksheet.Cells[i, j] = new Cell(" ");
                    }
                }

                #endregion

                workbook.Worksheets.Add(worksheet);

                return workbook;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
        }

        private void SetExcelAutoSpace(CellCollection cells, int row, int col)
        {
            if (cells[row, col].Value != null && ((cells[row, col].Value.ToString().Length * 256) + 512) > cells.ColumnWidth[(ushort)col])
            {
                cells.ColumnWidth[(ushort)col] = (ushort)((cells[row, col].Value.ToString().Length * 256) + 512);
            }
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult GetOrderStatusHistoryList(int id)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            return Json(tytFacadeBiz.GetOrderStatusHistoryList(id));
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult GetOrderStatusHistoryListWithComment(int id)
        {
            QuoteOrderModel quoteOrderModel = new QuoteOrderModel();
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            quoteOrderModel.QuoteOrderId = id;
            quoteOrderModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
            quoteOrderModel = tytFacadeBiz.GetQuoteOrderInfo(quoteOrderModel);



            return Json(new { Comment = quoteOrderModel.PaymentMethodComment, Data = tytFacadeBiz.GetOrderStatusHistoryList(id) });
        }

        [HttpGet, GSAAuthorizeAttribute()]
        public JsonResult GetOrdeShipInfo(int id)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
            QuoteOrderModel quoteOrderModel = new QuoteOrderModel();
            SalesOrderModel salesOrderModel = new SalesOrderModel();
            List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();
            ShipperAddressModel shipperAddress = new ShipperAddressModel();

            try
            {
                quoteOrderModel.QuoteOrderId = id;
                quoteOrderModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                quoteOrderModel = tytFacadeBiz.GetQuoteOrderInfo(quoteOrderModel);

                shipperAddress.ShipperName = ConfigurationManager.AppSettings["ShipperName"];

                shipperAddress.ShipperAddressLine = ConfigurationManager.AppSettings["ShipperAddressLine"];
                shipperAddress.ShipperCity = ConfigurationManager.AppSettings["ShipperCity"];
                shipperAddress.ShipperState = ConfigurationManager.AppSettings["ShipperState"];
                shipperAddress.ShipperZip = ConfigurationManager.AppSettings["ShipperZip"];

                shipperAddress.ShipperAddressLine2 = ConfigurationManager.AppSettings["ShipperAddressLine2"];
                shipperAddress.ShipperCity2 = ConfigurationManager.AppSettings["ShipperCity2"];
                shipperAddress.ShipperState2 = ConfigurationManager.AppSettings["ShipperState2"];
                shipperAddress.ShipperZip2 = ConfigurationManager.AppSettings["ShipperZip2"];

                salesOrderModel.QuoteOrderId = id;
                salesOrderModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                salesOrderModelList = tytFacadeBiz.GetSalesOrderList(salesOrderModel);

                List<double> totalWeight = salesOrderModelList.Where(w => w.ProductCategory == "Hardware" || w.ProductCategory == "Misc Fee").Select(s => s.Weight).ToList();
                quoteOrderModel.TotalWeight = totalWeight.Take(totalWeight.Count).Sum();

                List<TSSSettings> settings = tytFacadeBiz.GetAllSettings();
                var devicesLessThan3 = settings.FirstOrDefault(f => f.SettingsName == "<3Devices").SettingsValue;
                var devices4To6 = settings.FirstOrDefault(f => f.SettingsName == "4-6Devices").SettingsValue;
                var devicesGreaterThan7 = settings.FirstOrDefault(f => f.SettingsName == ">7Devices").SettingsValue;

                List<int> selectedProductList = salesOrderModelList.Where(w => w.ProductCategory == "Hardware" && w.Price > -1).Select(s => s.Quantity).ToList();
                var totalProduct  = selectedProductList.Sum();

                if (totalProduct < 4)
                {
                    quoteOrderModel.ShippingBoxSize = devicesLessThan3;
                }
                else if (totalProduct > 3 && totalProduct < 7)
                {
                    quoteOrderModel.ShippingBoxSize = devices4To6;
                }
                else
                {
                    quoteOrderModel.ShippingBoxSize = devicesGreaterThan7;
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }


            return Json(new { QuoteOrder = quoteOrderModel, ShipperAddress = shipperAddress }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult ProcessUPSShipping(UPSShippingRequestModel model)
        {
            var result = new { };

            try
            {
                var shipperNumberForShipping = ConfigurationManager.AppSettings["ShipperNumberForShipping"];
                var shipperAddressLine = ConfigurationManager.AppSettings["ShipperAddressLine"];
                var shipperCity = ConfigurationManager.AppSettings["ShipperCity"];
                var shipperState = ConfigurationManager.AppSettings["ShipperState"];
                var shipperZip = ConfigurationManager.AppSettings["ShipperZip"];
                var shipperPhone = ConfigurationManager.AppSettings["ShipperPhone"];

                if (model.ShipFromId == 2)
                {
                    shipperNumberForShipping = ConfigurationManager.AppSettings["ShipperNumberForShipping2"];
                    shipperAddressLine = ConfigurationManager.AppSettings["ShipperAddressLine2"];
                    shipperCity = ConfigurationManager.AppSettings["ShipperCity2"];
                    shipperState = ConfigurationManager.AppSettings["ShipperState2"];
                    shipperZip = ConfigurationManager.AppSettings["ShipperZip2"];
                    shipperPhone = ConfigurationManager.AppSettings["ShipperPhone2"];
                }

                TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
                List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();
                QuoteOrderModel quoteOrderModel = new QuoteOrderModel();
                quoteOrderModel.QuoteOrderId = model.QuoteOrderId;
                quoteOrderModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                quoteOrderModel = tytFacadeBiz.GetQuoteOrderInfo(quoteOrderModel);

                SalesOrderModel salesOrderModel = new SalesOrderModel();
                salesOrderModel.QuoteOrderId = model.QuoteOrderId;
                salesOrderModel.SessionId = SessionVars.CurrentLoggedInUser.SessionId;
                salesOrderModelList = tytFacadeBiz.GetSalesOrderList(salesOrderModel);

                string shippingTypeCode = "03"; //Default to Ground
                if (quoteOrderModel.ShipToCountry == "US")
                {
                    if (model.ShippingType == "Ground")
                    {
                        shippingTypeCode = "03"; //Ground
                    }
                    else if (model.ShippingType == "2Day")
                    {
                        shippingTypeCode = "02"; //2nd Day Air
                    }
                    else if (model.ShippingType == "NextAir")
                    {
                        shippingTypeCode = "01"; //Next Day Air
                    }
                }
                else
                {
                    if (model.ShippingType == "Ground")
                    {
                        shippingTypeCode = "11"; //UPS Standard
                    }
                    else if (model.ShippingType == "2Day")
                    {
                        shippingTypeCode = "08"; //Expedited
                    }
                    else if (model.ShippingType == "NextAir")
                    {
                        shippingTypeCode = "65"; //UPS Saver 
                    }
                }

                UPSShipping ups = new UPSShipping(ConfigurationManager.AppSettings["UPSShippingAPI"]);
                UPSShippingRequest upsShippingRequest = new UPSShippingRequest();

                upsShippingRequest.UPSSecurity = new UPSSecurity
                {
                    ServiceAccessToken = new ServiceAccessToken
                    {
                        AccessLicenseNumber = "ED0A5D6A4EDC1E46"
                    },
                    UsernameToken = new UsernameToken
                    {
                        Username = "trackyourtruckVA",
                        Password = "random%22"
                    }
                };

                upsShippingRequest.ShipmentRequest = new ShipmentRequest
                {
                    Request = new UPS.Models.Request
                    {
                        RequestOption = RequestOption.validate,
                        TransactionReference = new TransactionReference
                        {
                            CustomerContext = "TYT_Shipping"
                        }
                    },
                    LabelSpecification = new LabelSpecification
                    {
                        LabelImageFormat = new ImageFormat
                        {
                            ImageFormatType = model.LabelImageType == "ZPL" ? ImageFormatType.ZPL : ImageFormatType.PNG
                        },
                        LabelPrintMethod = new LabelPrintMethod
                        {
                            PrintMethodType = PrintMethodType.ZPL
                        },
                        LabelStockSize = new LabelStockSize
                        {
                            Height = "6",
                            Width = "4"
                        }
                    },
                    Shipment = new Shipment
                    {
                        Service = new Service
                        {
                            Code = shippingTypeCode
                        },
                        PaymentInformation = new PaymentInformation
                        {
                            ShipmentCharge = new ShipmentCharge
                            {
                                Type = "01",
                                BillShipper = new BillShipper
                                {
                                    AccountNumber = shipperNumberForShipping
                                }
                            }
                        },
                        Package = new Package
                        {
                            Packaging = new Packaging
                            {
                                Code = "02"
                            },
                            PackageWeight = new PackageWeight
                            {
                                UnitOfMeasurement = new UnitOfMeasurement
                                {
                                    UnitType = UnitType.LBS
                                },
                                Weight = model.Weight
                            },
                            Dimensions = new Dimensions
                            {
                                UnitOfMeasurement = new UnitOfMeasurement
                                {
                                    UnitType = UnitType.IN
                                },
                                Height = model.Height,
                                Width = model.Width,
                                Length = model.Length
                            }
                        },
                        Shipper = new Shipper
                        {
                            Name = ConfigurationManager.AppSettings["ShipperName"],

                            ShipperNumber = shipperNumberForShipping,

                            Address = new Address
                            {
                                AddressLine = shipperAddressLine,
                                City = shipperCity,
                                StateProvinceCode = shipperState,
                                PostalCode = shipperZip,
                                CountryCode = "US"
                            }
                        },
                        ShipFrom = new ShipFrom
                        {
                            Name = ConfigurationManager.AppSettings["ShipperName"],
                            //PhoneNumber = "8884343848",
                            //AttentionName = ConfigurationManager.AppSettings["ShipperName"],
                            Address = new Address
                            {
                                AddressLine = shipperAddressLine,
                                City = shipperCity,
                                StateProvinceCode = shipperState,
                                PostalCode = shipperZip,
                                CountryCode = "US"
                            }
                        },
                        ShipTo = new ShipTo
                        {
                            Name = quoteOrderModel.ShipToCompanyName.Length > 35 ? quoteOrderModel.ShipToCompanyName.Substring(0, 35) : quoteOrderModel.ShipToCompanyName,
                            AttentionName = quoteOrderModel.ShipToBillingContact.Length > 35 ? quoteOrderModel.ShipToBillingContact.Substring(0, 35) : quoteOrderModel.ShipToBillingContact,
                            Address = new Address
                            {
                                AddressLine = (quoteOrderModel.ShipToAddress1.Replace(",","")).Trim() + (quoteOrderModel.ShipToAddress2 != "" && quoteOrderModel.ShipToAddress2 != null ? ", " + (quoteOrderModel.ShipToAddress2.Replace(",","")).Trim() : ""),/// replace , and Trim spaces -os 1/27/2020
                                City = quoteOrderModel.ShipToCity,
                                StateProvinceCode = quoteOrderModel.ShipToState,
                                PostalCode = quoteOrderModel.ShipToZip,
                                CountryCode = quoteOrderModel.ShipToCountry
                            }
                        },
                        ShipmentServiceOptions = new ShipmentServiceOptions
                        {
                            DirectDeliveryOnlyIndicator = "1"
                            /*Notification = new Notification
                            {
                                NotificationCode = "6",
                                EMail = new EMail
                                {
                                    //FromEMailAddress = "tssorders@trackyourtruck.com",
                                    EMailAddress = quoteOrderModel.ShipToBillingEmail
                                }
                            }*/
                        }
                    }
                };

                if (quoteOrderModel.ShipToCountry != "US")
                {
                    List<TSSSettings> settings = tytFacadeBiz.GetAllSettings();
                    var canadaShipperPhone = settings.FirstOrDefault(f => f.SettingsName == "CanadaShipperPhone").SettingsValue;
                    var canadaProductDescription = settings.FirstOrDefault(f => f.SettingsName == "CanadaProductDescription").SettingsValue;

                    double totalProductValue = quoteOrderModel.SalesTax;
                    foreach (var item in salesOrderModelList)
                    {
                        if (item.ProductCategory == "Hardware" || item.ProductCategory == "Misc Fee")
                        {
                            totalProductValue += item.Price * (double)item.Quantity;
                        }
                    }

                    var productHW = salesOrderModelList.Where(w => w.ProductCategory == "Hardware").FirstOrDefault();
                    var productDescription = canadaProductDescription;

                    if (productDescription.Length > 50)
                    {
                        productDescription = productDescription.Substring(0, 50);
                    }

                    upsShippingRequest.ShipmentRequest.Shipment.Description = productDescription;
                    upsShippingRequest.ShipmentRequest.Shipment.InvoiceLineTotal = new InvoiceLineTotal
                    {
                        CurrencyCode = "USD",
                        MonetaryValue = totalProductValue == 0 ? "1" : totalProductValue.ToString()
                    };
                    upsShippingRequest.ShipmentRequest.Shipment.Shipper.AttentionName = ConfigurationManager.AppSettings["ShipperName"];
                    upsShippingRequest.ShipmentRequest.Shipment.Shipper.Phone = new Phone { Number = canadaShipperPhone.Replace("-", "") };
                    upsShippingRequest.ShipmentRequest.Shipment.ShipTo.Phone = new Phone { Number = quoteOrderModel.ShipToPhone.Replace("-", "") };
                }

                var upsResponse = ups.Shipping(upsShippingRequest);

                if (upsResponse.Fault == null)
                {
                    var upsResult = new
                    {
                        TrackingNo = upsResponse.ShipmentResponse.ShipmentResults.PackageResults.TrackingNumber,
                        TrackingUrl = "https://www.ups.com/track?loc=en_US&tracknum=" + upsResponse.ShipmentResponse.ShipmentResults.PackageResults.TrackingNumber + "&requester=WT/trackdetails",
                        TotalCost = upsResponse.ShipmentResponse.ShipmentResults.ShipmentCharges.TotalCharges.MonetaryValue,
                        LabelImage = upsResponse.ShipmentResponse.ShipmentResults.PackageResults.ShippingLabel.GraphicImage,
                        LabelImageType = upsResponse.ShipmentResponse.ShipmentResults.PackageResults.ShippingLabel.ImageFormat.ImageFormatType.ToString()
                    };

                    tytFacadeBiz.SaveTssShipping(new TssShippingModel
                    {
                        QuoteOrderId = model.QuoteOrderId,
                        ShippingType = model.ShippingType,
                        ShippingCost = upsResult.TotalCost,
                        Weight = Convert.ToDouble(model.Weight),
                        Height = Convert.ToInt32(model.Height),
                        Width = Convert.ToInt32(model.Width),
                        Length = Convert.ToInt32(model.Length),
                        TrackingNo = upsResult.TrackingNo,
                        LabelImage = upsResult.LabelImage
                    });

                    tytFacadeBiz.SetQuoteOrderStatus(new QuoteOrderModel { QuoteOrderId = model.QuoteOrderId, OrderStatusId = 3 });

                    tytFacadeBiz.SaveOrderStatusHistory(model.QuoteOrderId, 3); //OrderStatusId = 3 [Shipped]

                    return Json(upsResult);
                }
                else
                {
                    return Json(new { Error = upsResponse.Fault.Detail.Errors.ErrorDetail.PrimaryErrorCode.Description });
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new { Error = "Shipping process failed.\n" + ex.Message });
            }

            return Json(result);
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public ContentResult GetShippingHistoryList(int id)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
            List<TssShippingModel> tssShippingList = new List<TssShippingModel>();

            tssShippingList = tytFacadeBiz.GetTssShippingHistory(new TssShippingModel { QuoteOrderId = id });

            Session["GetShippingHistoryList_TssShippingList"] = tssShippingList;

            var jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = Int32.MaxValue;


            return new ContentResult
            {
                Content = jsSerializer.Serialize(tssShippingList.Select(s => new TssShippingModel
                {
                    TssShippingId = s.TssShippingId,
                    QuoteId = s.QuoteId,
                    QuoteOrderId = s.QuoteOrderId,

                    TrackingNo = s.TrackingNo,
                    ShippingType = s.ShippingType,
                    ShippingCost = s.ShippingCost,
                    Weight = s.Weight,

                    ShipToCompanyName = s.ShipToCompanyName,
                    ShipToBillingContact = s.ShipToBillingContact,
                    ShipToBillingEmail = s.ShipToBillingEmail,

                    CreatedDateFormated = s.CreatedDateFormated,
                    SentEmailDateFormated = s.SentEmailDateFormated,

                })),
                ContentType = "application/json"
            };
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult DownloadLabel(string id)
        {
            string labelImage = "", imageFormat = "PNG";

            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
            List<TssShippingModel> tssShippingList = Session["GetShippingHistoryList_TssShippingList"] as List<TssShippingModel>;

            if (tssShippingList != null)
            {
                var tssShipping = tssShippingList.SingleOrDefault(s => s.TrackingNo == id);
                if (tssShipping != null)
                {
                    labelImage = tssShipping.LabelImage;

                    string encodedImage = Encoding.UTF8.GetString(Convert.FromBase64String(labelImage));
                    if (encodedImage.Contains("^XA"))
                    {
                        imageFormat = "ZPL";
                        //labelImage = Convert.ToBase64String(Encoding.UTF8.GetBytes(encodedImage.Replace("^XZ^XZ", "^XZ")));
                    }
                }
            }

            return Json(new { ImageFormat = imageFormat, LabelImage = labelImage });
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult SendShipEmail(UPSShipEmailModel model)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();
            QuoteOrderModel quoteOrderModel = new QuoteOrderModel();

            try
            {
                foreach (string to in model.ShippingMailTo.Split(':'))
                {
                    if (!string.IsNullOrWhiteSpace(to))
                    {
                        EmailSender.SendMailUsingMailGunWithCustomVars(false, to, "Track Your Truck Ship Notification", model.ShippingMailBody, "", "tssorders@trackyourtruck.com", "tssorders@trackyourtruck.com", new { TssShippingId = model.TssShippingId });
                    }
                }

                tytFacadeBiz.UpdateTssShipSentEmail(new TssShippingModel { TssShippingId = model.TssShippingId });
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                return Json(new AjaxResponse { Message = ex.Message + "\nException has occurred. Email Send failed." });
            }

            return Json(new { Message = "Email Send Successfully." });
        }

        [HttpPost, GSAAuthorizeAttribute()]
        public JsonResult GetTssShippingEmailHistory(int id)
        {
            TytFacadeBiz tytFacadeBiz = new TytFacadeBiz();

            return Json(tytFacadeBiz.GetTssShippingEmailHistory(new TssShippingModel { TssShippingId = id }));
        }

        [ActionName("ValidateAddress")]
        public JsonResult ValidateAddress(AVRAddress address)
        {
            AVResponse response = new AVResponse();

            try
            {
                UPSShipping upsShipping = new UPSShipping();
                AVRequest request = new AVRequest();

                request.AccessRequest = new AVRAccessRequest
                {
                    AccessLicenseNumber = "ED0A5D6A4EDC1E46",
                    UserId = "trackyourtruckVA",
                    Password = "random%22"
                };
                request.AddressValidationRequest = new AVRAddressValidationRequest
                {
                    Request = new AVRRequest
                    {
                        RequestAction = "AV"
                    },
                    Address = address
                };

                response = upsShipping.ValidateAddress(request);
            }
            catch (Exception ex)
            {

            }

            return Json(response.AddressValidationResponse);
        }

        public static string RenderPartialViewToString(Controller thisController, string viewName, object model)
        {
            // assign the model of the controller from which this method was called to the instance of the passed controller (a new instance, by the way)
            thisController.ViewData.Model = model;

            // initialize a string builder
            using (StringWriter sw = new StringWriter())
            {
                // find and load the view or partial view, pass it through the controller factory
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(thisController.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(thisController.ControllerContext, viewResult.View, thisController.ViewData, thisController.TempData, sw);

                // render it
                viewResult.View.Render(viewContext, sw);

                //return the razorized view/partial-view as a string
                return sw.ToString();
            }
        }
    }
}
