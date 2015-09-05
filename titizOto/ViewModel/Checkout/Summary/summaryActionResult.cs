using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.Checkout.Summary
{
    public enum summaryActionResult
    {
        beginProcess,
        trackInfoAddError,
        deliveryAddError,
        billingAddError,
        posCodeError,
        posError,
        posSuccessful,
        orderAddError,
        orderDetailAddError,
        allSuccessful
    }
}