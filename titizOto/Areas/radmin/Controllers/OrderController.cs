using HelperAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titizOto.Models;

namespace titizOto.Areas.radmin.Controllers
{
    [AccessControl(AccessRole.superAdmin)]
    [BindAdminParameters]
    public class OrderController : DbWithControllerWithSorting<tbl_order>
    {

        public override ActionResult Index()
        {
            var os = new HelperSite.Shared.orderShared(db);
            var waitList = os.getWaitingOrderStatuList();
            var list= base.getList().Where(a => waitList.Contains(a.orderStatu)).ToList();

            return View(list);
        }

        public  ActionResult All()
        {
            return View(getList());
        }

        public ActionResult DeleteOrder(int id)
        {
            var item = getById(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOrder(int id, string deleteAction)
        {

            var item = getById(id);

            try
            {
                deleteItem(item);
                ViewBag.success = true;
                ViewBag.resultHtml = getNotificationDefaultSuccess();
                cacheUpdate();
            }
            catch (Exception ex)
            {
                errorSend(ex, item.GetType().Name + " Silme İşlemi");
                ViewBag.success = false;
                ViewBag.resultHtml = getNotificationDefaultError();
            }

            return View(item);

        }

        public ActionResult UpdateCargo(int id)
        {
            var item = getById(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCargo(int id, string shipmentNo)
        {
            var item = getById(id);

            try
            {
                updateShipmentNo(id, shipmentNo);
                ViewBag.success = true;
                ViewBag.resultHtml = getNotificationDefaultSuccess();
            }
            catch (Exception ex)
            {

                errorSend(ex, " ShipmentNo Delete İşlemi");
                ViewBag.success = false;
                ViewBag.resultHtml = getNotificationDefaultError();
            }

            return View(item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection fc)
        {
            HelperSite.Shared.mailShared ms = new HelperSite.Shared.mailShared(db, 1);
            HelperSite.Shared.orderShared os = new HelperSite.Shared.orderShared(db);

            foreach (string item in fc.Keys)
            {
                // orderStatu Update
                if (item.IndexOf("orderStatuUpdateBtn_") != -1)
                {


                    string idStr = item.Replace("orderStatuUpdateBtn_", string.Empty);
                    var itemId = int.Parse(idStr);


                    var valId = fc["orderStatuUpdateDrop_" + idStr];
                    int itemValId = int.Parse(valId);

                    var orderItem = getById(itemId);

                    if ((HelperSite.Shared.orderStatu)itemValId == HelperSite.Shared.orderStatu.onCargo)
                    {
                        if (string.IsNullOrWhiteSpace(orderItem.shipmentNo))
                        {
                            ViewBag.success = false;
                            ViewBag.resultHtml = getNotification("Kargoya verildi olarak güncellemek için siparişe kargo takip no girişi yapınız.", "Failure", "mt10 mb10");


                            continue;

                        }
                    }

                    try
                    {
                        updateOrderStatu(itemId, itemValId);
                        ViewBag.success = true;
                        ViewBag.resultHtml = getNotificationDefaultSuccess();
                    }
                    catch (Exception ex)
                    {
                        errorSend(ex, " Order Statu Update İşlemi");
                        ViewBag.success = false;
                        ViewBag.resultHtml = getNotificationDefaultError();

                    }
                }




                // orderMail  Update
                if (item.IndexOf("mailStatuUpdateBtn_") != -1)
                {



                    string idStr = item.Replace("mailStatuUpdateBtn_", string.Empty);
                    var itemId = int.Parse(idStr);


                    var valId = fc["mailStatuUpdateDrop_" + idStr];
                    int itemValId = int.Parse(valId);

                    var orderItem = getById(itemId);

                    // order Complete Mail none 
                    if (itemValId == 0)
                    {
                        ViewBag.success = false;
                        ViewBag.resultHtml = getNotification("Sipariş tamamlandı maili tekrar gönderilemez.", "Failure", "mt10 mb10");


                        continue;
                    }

                    string orderLink = os.getOrderDetailLink(orderItem.orderGuid, 1, "tr");
                    orderLink = getSiteName(Request) + orderLink;
                    HelperSite.DbController.DbWithController orderHelperController = new HelperSite.DbController.DbWithController();
                    orderHelperController.langId = 1;

                    // shipmentNo Control && Send Cargo Mail
                    if ((HelperSite.Shared.orderStatu)itemValId == HelperSite.Shared.orderStatu.onCargo)
                    {

                        if (orderItem != null)
                        {
                            if (!string.IsNullOrWhiteSpace(orderItem.shipmentNo))
                            {
                                try
                                {
                                    var cargoItem = db.tbl_cargo.Where(a => a.cargoId == orderItem.cargoId).FirstOrDefault();

                                    string cargoLink = "";
                                    string cargoName = "";

                                    if (cargoItem != null)
                                    {
                                        cargoLink = cargoItem.trackUrl;
                                        cargoName = cargoItem.name;
                                    }

                                    var mailStatuItem = ms.getOrderCargo(orderItem.getOrderUserNameSurname(), orderItem.orderNo, orderItem.shipmentNo, cargoLink, cargoName, orderLink);
                                    orderHelperController.mailSend(orderItem.getOrderEmail(), mailStatuItem.Item1, mailStatuItem.Item2);

                                    // Update Order Mail Statu
                                    updateOrderMailStatu(itemId, itemValId);

                                    ViewBag.success = true;
                                    ViewBag.resultHtml = getNotificationDefaultSuccess();
                                }
                                catch (Exception ex)
                                {
                                    errorSend(ex, " Order Mail Statu Cargo Update İşlemi");
                                }
                            }
                            else
                            {
                                ViewBag.success = false;
                                ViewBag.resultHtml = getNotification("Kargo maili yollamak için kargo takip numarası giriniz.", "Failure", "mt10 mb10");

                                continue;
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            // Send Order Mail

                            var mailStatuItem = ms.getOrderStatu(orderItem.getOrderUserNameSurname(), orderItem.orderNo, os.getOrderStatuString(itemValId), orderLink);
                            orderHelperController.mailSend(orderItem.getOrderEmail(), mailStatuItem.Item1, mailStatuItem.Item2);

                            // Update Order Mail Statu
                            updateOrderMailStatu(itemId, itemValId);

                            ViewBag.success = true;
                            ViewBag.resultHtml = getNotificationDefaultSuccess();
                        }
                        catch (Exception ex)
                        {
                            errorSend(ex, " Order Mail Statu Update İşlemi");
                            ViewBag.success = false;
                            ViewBag.resultHtml = getNotificationDefaultError();

                        }
                    }


                }

                continue;

            }

            return View(getList());

        }

        private void updateOrderStatu(int orderId, int statuId)
        {
            var orderItem = db.tbl_order.Where(a => a.orderId == orderId).FirstOrDefault();

            if (orderItem != null)
            {

                orderItem.orderStatu = statuId;
                db.SaveChanges();
            }
        }

        private void updateOrderMailStatu(int orderId, int mailStatuId)
        {
            var orderItem = db.tbl_order.Where(a => a.orderId == orderId).FirstOrDefault();

            if (orderItem != null)
            {

                orderItem.orderMailStatu = mailStatuId;
                db.SaveChanges();
            }
        }

        private void updateShipmentNo(int orderId, string shipmentNo)
        {
            var orderItem = getById(orderId);
            orderItem.shipmentNo = shipmentNo;
            db.SaveChanges();

        }

        public ActionResult Detail(int id)
        {
            var item = getById(id); 
            return View(item);
        }

        public ActionResult OrderAdminDetail(string orderGuid)
        {

            HelperSite.Shared.orderShared os = new HelperSite.Shared.orderShared(db);
            HelperSite.Shared.addressShared ads = new HelperSite.Shared.addressShared(db);

           
                var orderItem = os.getOrderByGuid(orderGuid);

                HelperSite.Shared.pageShared ps = new HelperSite.Shared.pageShared(db);



                ViewModel.Account.Order.helperOrderAdminDetail pageHelper = new ViewModel.Account.Order.helperOrderAdminDetail();
 


                pageHelper.orderSummary = os.getOrderSummary(orderItem);

                pageHelper.orderNo = orderItem.orderNo;
                pageHelper.orderStatuHtml = os.getOrderStatuString(orderItem.orderStatu);

                // On Cargo Add Track Url 
                if ((HelperSite.Shared.orderStatu)orderItem.orderStatu == HelperSite.Shared.orderStatu.onCargo && !string.IsNullOrWhiteSpace(orderItem.shipmentNo))
                {
                    pageHelper.orderStatuHtml = pageHelper.orderStatuHtml + " " + os.getCargoTrackHtml(orderItem);
                }

                pageHelper.salesAgreement = orderItem.salesAgreement;
                pageHelper.preSalesAgreement = orderItem.preSalesAgreement;

                var deliveryAddressItem = ads.getAddressHtmlFromObj(orderItem.deliveryAddressId, orderItem.deliveryAddressObj);
                var billingAddressItem = ads.getAddressHtmlFromObj(orderItem.billingAddressId, orderItem.billingAddressObj);

                pageHelper.deliveryAddress = ads.getAddressHtml(deliveryAddressItem, HelperSite.Shared.AddressHtmlType.adminOrderDetail, this);
                pageHelper.billingAddress = ads.getAddressHtml(billingAddressItem, HelperSite.Shared.AddressHtmlType.adminOrderDetail, this);

                pageHelper.orderNote = orderItem.orderNote;



                return View(pageHelper);

            



          
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }


        public string RenderRazorViewToString(string viewName, object model, ControllerContext ControllerContext)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

    }
}
