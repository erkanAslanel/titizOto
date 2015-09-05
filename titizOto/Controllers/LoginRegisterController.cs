using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelperSite.Attribute;
using HelperSite.DbController;
using HelperSite.Shared;
using ViewModel.LoginRegister;
using ViewModel.Shared;
using titizOto.Models;
using Facebook;


namespace titizOto.Controllers
{
    public class LoginRegisterController : DbWithControllerWithMaster
    {
        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult Index(int pageId)
        {
            pageShared ps = new pageShared(db);

            var helperItem = new ViewModel.LoginRegister.helperLoginRegister();

            // Registered User Redirect
            if (ViewData["topCart"] != null)
            {
                var cartItem = (topCart)ViewData["topCart"];
                if (cartItem.isRegisteredUser)
                {
                    return Redirect("~/");
                }
            }

            helperItem.register = new ViewModel.LoginRegister.registerItem();
            var aggrementModule = ps.getModuleByType(moduleType.registerAgreement, langId);
            if (aggrementModule != null)
            {
                helperItem.register.agreementContent = aggrementModule.htmlContent;
            }


            helperItem.login = new ViewModel.LoginRegister.loginItem();


            var pageItem = ps.getPageById(pageId);
            if (pageItem == null)
            {
                return null;
            }

            ps.pageTitleBind(pageItem, helperItem, langId);
            var pageName = pageItem.name;
            if (!string.IsNullOrWhiteSpace(pageItem.title))
            {
                pageName = pageItem.title;
            }

            // forget Password Link
            var forgetPasswordPage = ps.getPageByType(pageType.forgetPassword, langId);
            if (forgetPasswordPage != null)
            {
                helperItem.login.forgetPasswordUrl = langCode + "/" + forgetPasswordPage.url + ".html";
            }

            // Facebook Register & login Error
            if (Request.QueryString["facebookError"] != null && Request.QueryString["facebookError"].ToString() == "yes")
            {
                helperItem.isFacebookError = true;
                helperItem.facebookErrorMessage = getErrorMessage(App_GlobalResources.lang.unexpectedErrorMsg);
            }


            if (Request.QueryString["needLogin"] != null && Request.QueryString["needLogin"].ToString() == "yes")
            {
                helperItem.isLoginRequeredShown = true;
                helperItem.isLoginRequeredMessage = getErrorMessage(App_GlobalResources.lang.needLogin, "mTop10 mBottom0 autoHide");
            }

            helperItem.breadCrumbItem = getBreadCrumbStaticPage(pageName);

            return View(helperItem);
        }

        #region Register && Login && Facebook

        [cartSummaryBind]
        [HttpPost]
        public ActionResult RegisterUser(registerItem item)
        {
            System.Threading.Thread.Sleep(1500);

            var enCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            var trCulture = System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR");

            // lower Emal
            item.email = item.email.ToLower(enCulture);

            // Upper case Name And Surname 
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            item.name = currentCulture.TextInfo.ToTitleCase(item.name);
            item.surname = currentCulture.TextInfo.ToTitleCase(item.surname);

            string redirectPage = "";
            pageShared ps = new pageShared(db);
            userShared us = new userShared(db);
            string html = "";

            var aggrementModule = ps.getModuleByType(moduleType.registerAgreement, langId);
            if (aggrementModule != null)
            {
                item.agreementContent = aggrementModule.htmlContent;
            }

            // Input Validate
            #region Validate

            if (item.gender == 0)
            {
                ModelState.AddModelError("gender", App_GlobalResources.lang.formGenderRequired);
            }

            if (item.day == 0)
            {
                ModelState.AddModelError("day", App_GlobalResources.lang.formDayRequired);
            }
            if (item.month == 0)
            {
                ModelState.AddModelError("month", App_GlobalResources.lang.formMonthRequired);
            }

            if (item.year == 0)
            {
                ModelState.AddModelError("year", App_GlobalResources.lang.formYearRequired);
            }

            if (!item.isAggrementCheck)
            {
                ModelState.AddModelError("isAggrementCheck", App_GlobalResources.lang.formUserDocumentRequired);
            }

            DateTime birthday = DateTime.Now;

            try
            {
                birthday = new DateTime(item.year, item.month, item.day);
            }
            catch
            {
                ModelState.AddModelError("validDate", App_GlobalResources.lang.formValidDate);
            }

            try
            {
                System.Net.Mail.MailAddress mailItem = new System.Net.Mail.MailAddress(item.email);
            }
            catch
            {
                ModelState.AddModelError("email", App_GlobalResources.lang.formValidEmail);
            }

            if (item.password != item.passwordRep)
            {
                ModelState.AddModelError("passwordRep", App_GlobalResources.lang.formPassworRepSame);
            }

            #endregion

            // Register Statu Validate
            #region RegisterControl

            if (ModelState.IsValid)
            {
                var registerStatuItem = us.getUserRegisterStatuByEmail(item.email);

                string errorMessage = "";
                bool isRegisterStatuValid = false;

                switch (registerStatuItem)
                {
                    case registerStatu.registered:

                        var forgetPassword = ps.getPageByType(pageType.forgetPassword, langId);
                        errorMessage = App_GlobalResources.lang.activationHasRegisteredUser;
                        errorMessage = errorMessage.Replace("[email]", item.email);

                        if (forgetPassword != null)
                        {
                            errorMessage = errorMessage.Replace("[forgetPasswordUrl]", (Url.Content("~/") + langCode + "/" + forgetPassword.url + ".html"));
                        }

                        break;
                    case registerStatu.waitingActivation:

                        var pageMailResent = ps.getPageByType(pageType.activationResent, langId);

                        // Send Error Message
                        // Replace [email],[activationResent] errorMessage
                        errorMessage = App_GlobalResources.lang.activationNoApprove;
                        errorMessage = errorMessage.Replace("[email]", item.email);
                        if (pageMailResent != null)
                        {
                            errorMessage = errorMessage.Replace("[activationResent]", (Url.Content("~/") + langCode + "/" + pageMailResent.url + ".html"));
                        }

                        break;
                    case registerStatu.ban:

                        errorMessage = App_GlobalResources.lang.unexpectedErrorMsg;

                        break;
                    case registerStatu.unregistered:

                        isRegisterStatuValid = true;
                        break;

                }

                if (!isRegisterStatuValid)
                {
                    item.isMessageExist = true;
                    item.message = getErrorMessage(errorMessage);
                    html = RenderRazorViewToString("Register", item);
                    return Json(new { htmlText = html });
                }

            }


            #endregion

            if (ModelState.IsValid)
            {
                bool isProoceessError = false;

                #region Shared

                var settingItem = db.tbl_settings.Where(a => a.langId == langId).FirstOrDefault();
                var userItem = new tbl_user();
                mailShared ms = new mailShared(db, langId);

                userItem.birthday = birthday;
                userItem.email = item.email;
                userItem.gender = item.gender;
                userItem.guid = Guid.NewGuid().ToString();
                userItem.isPasswordUpdate = false;
                userItem.name = item.name;
                userItem.password = MD5(item.password);
                userItem.registerStatuId = (int)registerStatu.waitingActivation;
                userItem.surname = item.surname;
                userItem.userTypeId = (int)userType.normalMember;
                userItem.createDate = DateTime.Now;
                #endregion


                // Has Activation
                if (settingItem.registerIsActivationExist)
                {
                    #region Add User

                    try
                    {
                        db.tbl_user.Add(userItem);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        errorSend(ex, "User Aktivasyonlu Ekleme");
                        isProoceessError = true;
                    }

                    #endregion

                    #region Activation Add

                    tbl_activation activationItem = new tbl_activation();

                    try
                    {
                        activationItem = us.addActivationWithItem(userItem.guid, userItem.userId);
                    }
                    catch (Exception ex)
                    {
                        errorSend(ex, "User Aktivation Ekleme");
                        isProoceessError = true;

                    }

                    #endregion

                    #region Action

                    if (!isProoceessError)
                    {
                        var mailItem = ms.getActivationMailContent(userItem.name, userItem.surname, activationItem.code, getSiteName(Request), langCode);
                        string mailSubject = mailItem.Item1;
                        string mailBody = mailItem.Item2;

                        try
                        {
                            mailSend(userItem.email, mailSubject, mailBody);

                            ModelState.Clear();
                            item.isMessageExist = true;
                            item.message = getSuccesMessage(App_GlobalResources.lang.activationMailSend.Replace("[email]", userItem.email));


                            // Add Resent Mail Link
                            var pageMailResent = ps.getPageByType(pageType.activationResent, langId);
                            string resentMailMsg = "";

                            if (pageMailResent != null)
                            {
                                resentMailMsg = App_GlobalResources.lang.activationMailResend;
                                resentMailMsg = resentMailMsg.Replace("[resentMailLink]", Url.Content("~/" + langCode + "/" + pageMailResent.url + ".html"));
                            }

                            item.message = item.message + resentMailMsg;
                        }
                        catch (Exception ex)
                        {
                            errorSend(ex, "Send Activation Mail");
                            isProoceessError = true;
                        }
                    }

                    #endregion
                }

                else   // No Activation
                {
                    userItem.registerStatuId = (int)registerStatu.registered;

                    #region Add User

                    try
                    {
                        db.tbl_user.Add(userItem);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        errorSend(ex, "User Aktivasyonsuz Ekleme");
                        isProoceessError = true;
                    }

                    #endregion

                    #region Thank Message

                    if (!isProoceessError && settingItem.registerIsThankMessageSend)
                    {
                        var mailItem = ms.getRegisterThankMailContent(userItem.name, userItem.surname);
                        string mailSubject = mailItem.Item1;
                        string mailBody = mailItem.Item2;

                        try
                        {
                            mailSend(userItem.email, mailSubject, mailBody);
                        }
                        catch (Exception ex)
                        {
                            errorSend(ex, "Send Thank Mail");
                        }

                    }

                    #endregion

                    #region Action

                    item.isMessageExist = true;
                    item.message = getSuccesMessage("Üyeliğiniz başarıyla oluşturuldu.Kaldığınız sayfaya yönlendiriliyorsunuz...");

                    // Set UserId, User Role , CheckoutProcess
                    setLoginSession(userItem);

                    // Switch Guest to Basket && Redirect
                    redirectPage = basketSwitchAndRedirect(userItem);

                    #endregion
                }

                // Process Has Error
                if (isProoceessError)
                {
                    item.isMessageExist = true;
                    item.message = getErrorMessage(App_GlobalResources.lang.unexpectedErrorMsg, "");
                    errorSend(new Exception("Kayıt Sırasında Hata"), "Kayıt Sırasında Hata", true);
                }
            }

            html = RenderRazorViewToString("Register", item);
            return Json(new { htmlText = html, redirectPage = redirectPage });

        }


        [cartSummaryBind]
        [HttpPost]
        public JsonResult Login(loginItem item)
        {
            System.Threading.Thread.Sleep(1500);

            string html = "";
            string msg = "";
            string redirectPage = "";
            userShared us = new userShared(db);
            pageShared ps = new pageShared(db);


            // forget Password Link
            var forgetPasswordPage = ps.getPageByType(pageType.forgetPassword, langId);
            if (forgetPasswordPage != null)
            {
                item.forgetPasswordUrl = langCode + "/" + forgetPasswordPage.url + ".html";
            }


            var enCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            var trCulture = System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR");



            item.email = item.email.ToLower(enCulture);


            if (ModelState.IsValid)
            {
                var statuItem = us.getUserRegisterStatuByEmail(item.email);
                switch (statuItem)
                {
                    case registerStatu.registered:

                        var userItem = us.getUserItemByEmailAndPassword(item.email, MD5(item.password));

                        if (userItem != null)
                        {
                            msg = App_GlobalResources.lang.loginSuccess;

                            // Set UserId, User Role , CheckoutProcess
                            setLoginSession(userItem);

                            // Switch Guest to Basket && Redirect
                            redirectPage = basketSwitchAndRedirect(userItem);

                            // Add Cookie 
                            if (item.isRememberMe)
                            {
                                HttpCookie myCookie = new HttpCookie("userCookie");
                                myCookie["userHashVal"] = MD5(userItem.email).Substring(0, 7);
                                myCookie["userHashValTwo"] = userItem.password.Substring(0, 7);
                                myCookie.Expires = DateTime.Now.AddMonths(9);
                                Response.Cookies.Add(myCookie);
                            }

                            msg = App_GlobalResources.lang.loginSuccess;
                            msg = getSuccesMessage(msg);

                        }
                        else
                        {
                            msg = App_GlobalResources.lang.loginUnregisteredOrPassword;
                            msg = getErrorMessage(msg);
                        }

                        break;
                    case registerStatu.waitingActivation:

                        var pageMailResent = ps.getPageByType(pageType.activationResent, langId);

                        msg = App_GlobalResources.lang.activationNoApprove;
                        msg = msg.Replace("[email]", item.email);
                        if (pageMailResent != null)
                        {
                            msg = msg.Replace("[activationResent]", (Url.Content("~/") + langCode + "/" + pageMailResent.url + ".html"));
                        }

                        msg = getErrorMessage(msg);

                        break;
                    case registerStatu.ban:

                        msg = getErrorMessage(App_GlobalResources.lang.loginUnregisteredOrPassword);

                        break;
                    case registerStatu.unregistered:
                        msg = getErrorMessage(App_GlobalResources.lang.loginUnregisteredOrPassword);

                        break;

                }

                item.message = msg;
                item.isMessageExist = true;
            }


            html = RenderRazorViewToString("Login", item);
            return Json(new { htmlText = html, redirectPage = redirectPage });
        }


        [HttpPost]
        [cartSummaryBind]
        public ActionResult Facebook(string accessToken)
        {
            var client = new FacebookClient(accessToken);
            dynamic result = client.Get("me", new { fields = "first_name,id,gender,last_name,email" });

            facebookItem faceItem = new facebookItem(result);
            pageShared ps = new pageShared(db);
            userShared us = new userShared(db);

            var userItemStatu = us.getUserRegisterStatuByEmail(faceItem.email);


            // Registered User
            if (userItemStatu == registerStatu.registered)
            {
                var userItem = db.tbl_user.Where(a => a.email == faceItem.email).FirstOrDefault();

                // Set UserId, User Role , CheckoutProcess
                setLoginSession(userItem);

                // Switch Guest to Basket && Redirect
                string redirectPage = basketSwitchAndRedirect(userItem);

                return Redirect(redirectPage);
            }

            if (userItemStatu == registerStatu.unregistered)
            {
                var userItem = new tbl_user();
                mailShared ms = new mailShared(db, langId);

                try
                {
                    userItem.birthday = faceItem.birthday;
                    userItem.email = faceItem.email;
                    userItem.gender = faceItem.gender;
                    userItem.guid = Guid.NewGuid().ToString();
                    userItem.isPasswordUpdate = false;
                    userItem.name = faceItem.firstName;
                    userItem.password = MD5(Guid.NewGuid().ToString().Substring(0, 7));
                    userItem.registerStatuId = (int)registerStatu.registered;
                    userItem.surname = faceItem.last_name;
                    userItem.userTypeId = (int)userType.facebookMember;
                    userItem.createDate = DateTime.Now;

                    db.tbl_user.Add(userItem);
                    db.SaveChanges();

                    var settingItem = db.tbl_settings.Where(a => a.langId == langId).FirstOrDefault();

                    if (settingItem != null && settingItem.registerIsThankMessageSend)
                    {
                        var mailItem = ms.getRegisterThankMailContent(userItem.name, userItem.surname);
                        string mailSubject = mailItem.Item1;
                        string mailBody = mailItem.Item2;

                        try
                        {
                            mailSend(userItem.email, mailSubject, mailBody);
                        }
                        catch (Exception ex)
                        {
                            errorSend(ex, "Send Thank Mail With Facebook");
                        }
                    }

                    // Set UserId, User Role , CheckoutProcess
                    setLoginSession(userItem);

                    // Switch Guest to Basket && Redirect
                    string redirectPage = basketSwitchAndRedirect(userItem);

                    return Redirect(redirectPage);

                }
                catch (Exception ex)
                {

                    errorSend(ex, "Facebook Register", true);

                    #region ErrorFacebook - Redirect Facebook Error Link

                    var registerLoginPage = ps.getPageByType(pageType.registerLogin, langId);
                    string redirectErrorPage = "~/";

                    if (registerLoginPage != null)
                    {
                        redirectErrorPage = getSiteName(Request) + langCode + "/" + registerLoginPage.url + ".html?facebookError=yes";
                        return Redirect(redirectErrorPage);
                    }
                    else
                    {
                        return null;
                    }

                    #endregion

                }

            }

            if (userItemStatu == registerStatu.waitingActivation)
            {
                try
                {
                    var userItem = db.tbl_user.Where(a => a.email == faceItem.email).FirstOrDefault();
                    userItem.registerStatuId = (int)registerStatu.registered;

                    db.SaveChanges();

                    // Set UserId, User Role , CheckoutProcess
                    setLoginSession(userItem);

                    // Switch Guest to Basket && Redirect
                    string redirectPage = basketSwitchAndRedirect(userItem);

                    return Redirect(redirectPage);
                }
                catch (Exception ex)
                {

                    errorSend(ex, "Facebook Register Waiting Activation", true);

                    #region ErrorFacebook - Redirect Facebook Error Link

                    var registerLoginPage = ps.getPageByType(pageType.registerLogin, langId);
                    string redirectErrorPage = "~/";

                    if (registerLoginPage != null)
                    {
                        redirectErrorPage = getSiteName(Request) + langCode + "/" + registerLoginPage.url + ".html?facebookError=yes";
                        return Redirect(redirectErrorPage);
                    }
                    else
                    {
                        return null;
                    }

                    #endregion
                }

            }

            return null;
        }

        #endregion

        #region Forget Password & Reset Password

        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult ForgetPassword(int pageId)
        {
            helperForgetPassword helperPage = new helperForgetPassword();
            pageShared ps = new pageShared(db);

            var pageItem = ps.getPageById(pageId);

            if (pageItem == null)
            {
                return null;
            }

            ps.pageTitleBind(pageItem, helperPage, langId);
            helperPage.setTitle(pageItem.name);
            helperPage.breadCrumbItem = getBreadCrumbStaticPage(pageItem.name);

            var loginPage = ps.getPageByType(pageType.registerLogin, langId);

            if (loginPage != null)
            {
                helperPage.loginLink = langCode + "/" + loginPage.url + ".html";
            }

            return View(helperPage);

        }

        [cartSummaryBind]
        [titleDescriptionBinder]
        [HttpPost]
        public ActionResult ForgetPassword(int pageId, string email)
        {
            System.Threading.Thread.Sleep(1500);

            helperForgetPassword helperPage = new helperForgetPassword();

            pageShared ps = new pageShared(db);
            userShared us = new userShared(db);

            email = email.ToLower(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

            var pageItem = ps.getPageById(pageId);
            var pageLoginRegister = ps.getPageByType(pageType.registerLogin, langId);

            if (pageItem == null | pageLoginRegister == null)
            {
                return null;
            }


            helperPage.loginLink = langCode + "/" + pageLoginRegister.url + ".html";
            ps.pageTitleBind(pageItem, helperPage, langId);
            helperPage.setTitle(pageItem.name);
            helperPage.breadCrumbItem = getBreadCrumbStaticPage(pageItem.name);

            if (string.IsNullOrWhiteSpace(email))
            {
                helperPage.isMessageExist = true;
                helperPage.message = getErrorMessage(App_GlobalResources.lang.formValidEmail);
                return View(helperPage);
            }


            var userRegisterStatuItem = us.getUserRegisterStatuByEmail(email);
            string message = "";

            switch (userRegisterStatuItem)
            {
                case registerStatu.registered:

                    try
                    {
                        mailShared ms = new mailShared(db, langId);

                        var userItem = db.tbl_user.Where(a => a.email == email).FirstOrDefault();

                        // Add Forget Table
                        var forgetCodeItem = us.addForgetPasswordWithItem(userItem.userId);

                        // Send Mail 
                        var mailItem = ms.getResetPasswordMailContent(userItem.name, userItem.surname, forgetCodeItem.code, getSiteName(Request), langCode);
                        mailSend(userItem.email, mailItem.Item1, mailItem.Item2);

                        message = getSuccesMessage(App_GlobalResources.lang.forgetPasswordSuccess).Replace("[email]", email);
                        helperPage.email = "";
                    }
                    catch (Exception ex)
                    {

                        errorSend(ex, "Şifre sıfırlama", true);
                    }

                    break;
                case registerStatu.waitingActivation:

                    var pageMailResent = ps.getPageByType(pageType.activationResent, langId);

                    message = App_GlobalResources.lang.activationNoApprove;
                    message = message.Replace("[email]", email);
                    if (pageMailResent != null)
                    {
                        message = message.Replace("[activationResent]", (Url.Content("~/") + langCode + "/" + pageMailResent.url + ".html"));
                    }

                    message = getErrorMessage(message);

                    break;
                case registerStatu.ban:

                    message = getErrorMessage(App_GlobalResources.lang.forgetPasswordError);

                    break;
                case registerStatu.unregistered:

                    message = getErrorMessage(App_GlobalResources.lang.forgetPasswordError);

                    break;

            }


            helperPage.isMessageExist = true;
            helperPage.message = message;
            return View(helperPage);

        }


        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult ResetPassword(int pageId, string resetcode)
        {
            helperResetPassword helperPage = new helperResetPassword();
            pageShared ps = new pageShared(db);

            var pageItem = ps.getPageById(pageId);

            if (pageItem == null)
            {
                return null;
            }

            ps.pageTitleBind(pageItem, helperPage, langId);
            helperPage.setTitle(pageItem.name);
            helperPage.breadCrumbItem = getBreadCrumbStaticPage(pageItem.name);

            helperPage.detail = pageItem.detail;

            bool isErrorExist = false;

            if (string.IsNullOrWhiteSpace(resetcode))
            {

                helperPage.message = App_GlobalResources.lang.resetPasswordNull;
                isErrorExist = true;
            }
            else
            {
                var forgetPasswordItem = db.tbl_forgetPassword.Where(a => a.code == resetcode).FirstOrDefault();

                if (forgetPasswordItem != null)
                {
                    helperPage.resetCode = forgetPasswordItem.code;
                    helperPage.userId = forgetPasswordItem.userId;
                    return View(helperPage);

                }
                else
                {
                    helperPage.message = App_GlobalResources.lang.resetPasswordWrong;
                    isErrorExist = true;
                }

            }


            if (isErrorExist)
            {
                string forgetPasswordLink = "";
                var forgetPassPage = ps.getPageByType(pageType.forgetPassword, langId);

                if (forgetPassPage != null)
                {
                    forgetPasswordLink = getSiteName(Request) + langCode + "/" + forgetPassPage.url + ".html";
                }

                helperPage.message = helperPage.message.Replace("[forgetPasswordLink]", forgetPasswordLink);
                helperPage.isMessageExist = true;
                helperPage.isErrorExist = true;

                return View(helperPage);
            }




            return null;
        }

        [cartSummaryBind]
        [titleDescriptionBinder]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int pageId, helperResetPassword helperPage)
        {
            pageShared ps = new pageShared(db);
            userShared us = new userShared(db);

            if (helperPage.password != helperPage.passwordRep)
            {
                ModelState.AddModelError("passwordRep", App_GlobalResources.lang.formPassworRepSame);
            }

            var pageItem = ps.getPageById(pageId);

            if (pageItem == null)
            {
                return null;
            }

            ps.pageTitleBind(pageItem, helperPage, langId);
            helperPage.setTitle(pageItem.name);
            helperPage.breadCrumbItem = getBreadCrumbStaticPage(pageItem.name);
            helperPage.detail = pageItem.detail;

            if (ModelState.IsValid)
            {
                // password - code reControl
                var forgetItem = db.tbl_forgetPassword.Where(a => a.code == helperPage.resetCode).FirstOrDefault();

                if (forgetItem == null || forgetItem.userId != helperPage.userId)
                {
                    helperPage.isErrorExist = true;
                    helperPage.isMessageExist = true;
                    helperPage.message = getErrorMessage(App_GlobalResources.lang.unexpectedErrorMsg);
                    return View(helperPage);
                }

                // Reset Password
                try
                {
                    us.updateUserPassword(helperPage.userId, MD5(helperPage.password));
                    helperPage.isMessageExist = true;
                    helperPage.message = getSuccesMessage(App_GlobalResources.lang.resetPasswordSuccess);

                    string loginUrl = "";

                    var loginPage = ps.getPageByType(pageType.registerLogin, langId);

                    if (loginPage != null)
                    {
                        loginUrl = getSiteName(Request) + langCode + "/" + loginPage.url + ".html";
                    }

                    helperPage.message = helperPage.message.Replace("[loginPage]", loginUrl);

                }
                catch (Exception ex)
                {
                    errorSend(ex, "updatePassword", true);

                    helperPage.isErrorExist = false;
                    helperPage.isMessageExist = true;
                    helperPage.message = getErrorMessage(App_GlobalResources.lang.unexpectedErrorMsg);
                }

                try
                {
                    us.deleteForgetPasswordByUserId(helperPage.userId);
                }
                catch (Exception ex)
                {
                    errorSend(ex, "ForgetPasswordDelete");
                }
            }

            return View(helperPage);
        }

        #endregion

        #region Activation ReSent && Update

        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult ActivationResent(int pageId)
        {
            helperActivationResent helperPage = new helperActivationResent();
            pageShared ps = new pageShared(db);

            var pageItem = ps.getPageById(pageId);

            if (pageItem == null)
            {
                return null;
            }

            ps.pageTitleBind(pageItem, helperPage, langId);
            helperPage.setTitle(pageItem.name);
            helperPage.breadCrumbItem = getBreadCrumbStaticPage(pageItem.name);

            return View(helperPage);


        }

        [cartSummaryBind]
        [titleDescriptionBinder]
        [HttpPost]
        public ActionResult ActivationResent(int pageId, string email)
        {
            System.Threading.Thread.Sleep(1500);

            helperActivationResent helperPage = new helperActivationResent();

            pageShared ps = new pageShared(db);

            email = email.ToLower(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

            var pageItem = ps.getPageById(pageId);
            var pageLoginRegister = ps.getPageByType(pageType.registerLogin, langId);

            if (pageItem == null | pageLoginRegister == null)
            {
                return null;
            }

            helperPage.loginLink = langCode + "/" + pageLoginRegister.url + ".html";
            ps.pageTitleBind(pageItem, helperPage, langId);
            helperPage.setTitle(pageItem.name);
            helperPage.breadCrumbItem = getBreadCrumbStaticPage(pageItem.name);

            if (string.IsNullOrWhiteSpace(email))
            {
                helperPage.isMessageExist = true;
                helperPage.message = getErrorMessage(App_GlobalResources.lang.formValidEmail);
                return View(helperPage);
            }


            var userItem = db.tbl_user.Where(a => a.email == email && a.registerStatuId == (int)registerStatu.waitingActivation).FirstOrDefault();

            if (userItem != null)
            {
                try
                {
                    tbl_activation activationItem = new tbl_activation();

                    activationItem.code = Guid.NewGuid().ToString();
                    activationItem.datetime = DateTime.Now;
                    activationItem.userId = userItem.userId;

                    db.tbl_activation.Add(activationItem);
                    db.SaveChanges();

                    mailShared ms = new mailShared(db, langId);
                    var mailItem = ms.getActivationMailContent(userItem.name, userItem.surname, activationItem.code, getSiteName(Request), langCode);

                    mailSend(userItem.email, mailItem.Item1, mailItem.Item2);

                    helperPage.isMessageExist = true;
                    helperPage.message = getSuccesMessage(App_GlobalResources.lang.activationResentSuccess);
                    helperPage.email = "";
                    return View(helperPage);

                }
                catch (Exception ex)
                {
                    errorSend(ex, "Activation Update", true);
                    helperPage.isMessageExist = true;
                    helperPage.message = getErrorMessage(App_GlobalResources.lang.unexpectedErrorMsg);
                    return View(helperPage);
                }
            }
            else
            {
                helperPage.isMessageExist = true;
                string errorMessage = App_GlobalResources.lang.activationResentMailErrorNoUser;
                var registerPage = db.tbl_page.Where(a => a.langId == langId && a.pageTypeId == (int)pageType.registerLogin).FirstOrDefault();

                if (registerPage != null)
                {
                    errorMessage = errorMessage.Replace("[registerPage]", getSiteName(Request) + langCode + "/" + registerPage.url + ".html");
                }

                helperPage.message = getErrorMessage(errorMessage);
                return View(helperPage);
            }
        }


        [cartSummaryBind]
        [titleDescriptionBinder]
        public ActionResult ActivationUpdate(int pageId, string activationCode)
        {

            helperActivationUpdate helperPage = new helperActivationUpdate();

            pageShared ps = new pageShared(db);

            var pageItem = ps.getPageById(pageId);

            if (pageItem == null)
            {
                return null;
            }

            ps.pageTitleBind(pageItem, helperPage, langId);
            helperPage.setTitle(pageItem.name);
            helperPage.breadCrumbItem = getBreadCrumbStaticPage(pageItem.name);


            var pageResentActivation = ps.getPageByType(pageType.activationResent, langId);

            string resendActivationLink = "";

            if (pageResentActivation != null)
            {
                resendActivationLink = Url.Content("~/") + langCode + "/" + pageResentActivation.url + ".html";
            }


            // activation boş 
            if (string.IsNullOrWhiteSpace(activationCode))
            {
                helperPage.message = getErrorMessage(App_GlobalResources.lang.activationCodeError.Replace("[resendLink]", resendActivationLink));
            }

            var activationItem = db.tbl_activation.Where(a => a.code == activationCode).OrderByDescending(a => a.activationId).FirstOrDefault();

            // activation Null
            if (activationItem == null)
            {
                helperPage.message = getErrorMessage(App_GlobalResources.lang.activationCodeError.Replace("[resendLink]", resendActivationLink));
                return View(helperPage);
            }

            try
            {
                var userItem = db.tbl_user.Where(a => a.userId == activationItem.userId).FirstOrDefault();

                if (userItem != null)
                {
                    userItem.registerStatuId = (int)registerStatu.registered;
                    db.tbl_activation.Remove(activationItem);
                    db.SaveChanges();

                    helperPage.message = getSuccesMessage(App_GlobalResources.lang.activationCodeSucess);

                    // Set UserId, User Role , CheckoutProcess
                    setLoginSession(userItem);

                    // Guest Basket => User Basket
                    if (ViewData["topCart"] != null)
                    {
                        var cartItem = (topCart)ViewData["topCart"];
                        basketShared bs = new basketShared(db);

                        try
                        {
                            bs.updateUserCartFromGuestCode(userItem.userId, cartItem.guestGuid);
                        }
                        catch (Exception ex)
                        {
                            errorSend(ex, "Guest Sepeti , Usera aktarmada ");
                        }
                    }


                    // Thanks Mail
                    var settingItem = db.tbl_settings.Where(a => a.langId == langId).FirstOrDefault();
                    if (settingItem != null && settingItem.registerIsThankMessageSend)
                    {
                        mailShared ms = new mailShared(db, langId);
                        var mailContentItem = ms.getRegisterThankMailContent(userItem.name, userItem.surname);

                        try
                        {
                            mailSend(userItem.email, mailContentItem.Item1, mailContentItem.Item2);
                        }
                        catch (Exception ex)
                        {
                            errorSend(ex, "Thank Mail sent");

                        }
                    }

                }
                else  // activation var Fakat User ile Eşleşmiyor
                {
                    helperPage.message = getErrorMessage(App_GlobalResources.lang.activationCodeError.Replace("[resendLink]", resendActivationLink));
                }

            }
            catch (Exception ex)
            {
                errorSend(ex, "Activation Durumu update etmede", true);
            }

            return View(helperPage);

        }

        #endregion

        #region Shared

        private breadCrumb getBreadCrumbStaticPage(string pageName)
        {
            breadCrumb helperItem = new breadCrumb();

            helperItem.name = pageName;
            helperItem.url = "#";

            return helperItem;
        }

        private string basketSwitchAndRedirect(tbl_user userItem)
        {
            // Guest Basket => User Basket
            if (ViewData["topCart"] != null)
            {
                var cartItem = (topCart)ViewData["topCart"];
                basketShared bs = new basketShared(db);

                try
                {
                    bs.updateUserCartFromGuestCode(userItem.userId, cartItem.guestGuid);
                }
                catch (Exception ex)
                {
                    errorSend(ex, "Guest Sepeti , Usera aktarmada");
                }
            }

            string redirectPage = "";

            if (Session["redirectPage"] != null)
            {
                redirectPage = Session["redirectPage"].ToString();
            }
            else
            {
                redirectPage = getSiteName(Request);
            }

            return redirectPage;

        }

        private void setLoginSession(tbl_user userItem)
        {

            //Set User 
            Session["userId"] = userItem.userId.ToString();

            //Set Role
            Session["userRoleId"] = userItem.userTypeId.ToString();

            //Set Checkout Reset 
            Session["checkoutProcess"] = null;

        }

        #endregion


    }
}
