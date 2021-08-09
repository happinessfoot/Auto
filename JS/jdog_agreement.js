// import{TabManager} from './jdog_TabManager.js';
// import{ControlManager} from './jdog_ControlManager.js';

var ControlManager = TabManager || {};

ControlManager = (function()
{
    return{
        hideControl: function (formContext,controlName)
        {
            let control = formContext.getControl(controlName);
            console.log(control);
            if(control!=null)
            {
                control.setVisible(false);
            }
        },
        setDisableControl : function(formContext,controlName,disable)
        {
            let control = formContext.getControl(controlName);
            if(control!=null)
            {
                control.setDisabled(disable);
            }
        }
    }
    
})();

var AttributeManager = AttributeManager || {};

AttributeManager = (function()
{
    return{
        
    }
    
})();

var TabManager = TabManager || {};

TabManager = (function()
{
    return{
        setTabVisible : function (formContext,tabName,visible)
        {
            let creditTab = formContext.ui.tabs.get(tabName);
            console.log(creditTab);
            if(creditTab!=null)
            {
                creditTab.setVisible(visible);
            }
        }
    }
    
})();


var JunkyardDog = JunkyardDog || {};

JunkyardDog.jdog_agreement = (function()
{
    function disableCalcCredControls(formContext,disable)
    {
        ControlManager.setDisableControl(formContext,"jdog_creditperiod",disable);
        ControlManager.setDisableControl(formContext,"jdog_creditamount",disable);
        ControlManager.setDisableControl(formContext,"jdog_fullcreditamount",disable);
        ControlManager.setDisableControl(formContext,"jdog_initialfee",disable);
    }
    function checkContactAndAuto(context)
    {
        let formContext = context.getFormContext();
        let contactAttr = formContext.getAttribute("jdog_contact");
        let autoAttr = formContext.getAttribute("jdog_auto");
        if(contactAttr!=null && autoAttr!=null)
        {
            if(contactAttr.getValue() != null && autoAttr.getValue() != null)
            {
                return true;
            }
        }
        return false;
    }
    function showTabCredit(context)
    {
        let formContext = context.getFormContext();
        if(checkContactAndAuto(context))
        {
            
            TabManager.setTabVisible(formContext,"tabCredit",true);
            ControlManager.setDisableControl(formContext,"jdog_creditid",false);
        }else
        {
            ControlManager.setDisableControl(formContext,"jdog_creditid",true);
        }
        
        
    }

    var contactOnChange = function(context)
    {
        showTabCredit(context);
        
    }
    
    var autoOnChange = function(context)
    {
        showTabCredit(context);
    }
    var creditidOnChange = function(context)
    {
        let formContext = context.getFormContext();
        let creditAttr= formContext.getAttribute("jdog_creditid");
        if(creditAttr!=null)
        {
            console.log("creditattr:"+creditAttr);
            if(creditAttr.getValue()!=null)
            {
                disableCalcCredControls(formContext,false);
            }else
            {
                disableCalcCredControls(formContext,true);
    
            }
            
        }
    }  
    return {
        onLoad : function(context)
        {
            console.log(context);
            let formContext = context.getFormContext();

            TabManager.setTabVisible(formContext,"tabCredit",false);
            ControlManager.hideControl(formContext,"jdog_summa");
            ControlManager.hideControl(formContext,"jdog_fact");
            ControlManager.setDisableControl(formContext,"jdog_creditid",true);
            disableCalcCredControls(formContext,true);
            let contactAttr = formContext.getAttribute("jdog_contact");
            let autoAttr = formContext.getAttribute("jdog_auto");
            let creditAttr = formContext.getAttribute("jdog_creditid");
            contactAttr.addOnChange(contactOnChange);
            autoAttr.addOnChange(autoOnChange);
            creditAttr.addOnChange(creditidOnChange);
        }
    }
})();