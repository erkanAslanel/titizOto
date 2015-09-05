using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelperSite.Shared;
using titizOto.Controllers;
using ViewModel.Checkout;

namespace titizTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            CheckoutController cc = new CheckoutController();
            checkoutProcess checkoutItem = new checkoutProcess(new System.Collections.Generic.List<checkoutPageItem>(), new ViewModel.Shared.topCart());

            string output = cc.serializeObject(checkoutItem);
            System.Diagnostics.Debug.WriteLine(output);

        }

    }
}
