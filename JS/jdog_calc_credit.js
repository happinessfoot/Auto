var JunkyardDog = JunkyardDog || {};

JunkyardDog.jdog_calc_credit = (function()
{
    return{
        CalculateCredit:function(context)
        {
            let formContext = Xrm.Page;
            let creditAttr = formContext.getAttribute("jdog_creditid");
            if(creditAttr.getValue()!=null)
            {
                let creditAmountAttr = formContext.getAttribute("jdog_creditamount");
                let initialFeeAttr = formContext.getAttribute("jdog_initialfee");
                let fullCreditAmountAttr = formContext.getAttribute("jdog_fullcreditamount");
                let creditPeriod = formContext.getAttribute("jdog_creditperiod");
                let newCreditAmountValue = creditAmountAttr.getValue()-(initialFeeAttr.getValue()||0);
                creditAmountAttr.setValue(newCreditAmountValue);
                
                let creditPromise = Xrm.WebApi.retrieveRecord("jdog_credit",creditAttr.getValue()[0].id,`?$select=jdog_creditperiod,jdog_percent,jdog_dateend`);
                creditPromise.then(function(creditResult)
                {
                    let newFullAmountValue = (creditResult.jdog_percent/100 * creditPeriod.getValue() * newCreditAmountValue)+newCreditAmountValue;
                    if(newFullAmountValue)
                    {
                        fullCreditAmountAttr.setValue(newFullAmountValue);
                    }
                });
                

            }
        }
    }
})();