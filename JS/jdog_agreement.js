// import{TabManager} from './jdog_TabManager.js';
// import{ControlManager} from './jdog_ControlManager.js';


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
        setVisible : function (formContext,tabName,visible)
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

var ControlManager = ControlManager || {};

ControlManager = (function()
{
    return{
        setVisible: function (formContext,controlName,visible)
        {
            let control = formContext.getControl(controlName);
            console.log(control);
            if(control!=null)
            {
                control.setVisible(visible);
            }
        },
        setDisabled : function(formContext,controlName,disable)
        {
            let control = formContext.getControl(controlName);
            if(control!=null)
            {
                control.setDisabled(disable);
            }
        },
        addCustomView : function(formContext,controlName,viewid,entityLogicaName,viewName,fetchXml,layoutXml,defaultView=false)
        {
            let control = formContext.getControl(controlName);
            if(control!=null)
            {
                control.addCustomView(viewid,entityLogicaName,viewName,fetchXml,layoutXml,defaultView);
            }
        },
        addCustomFilter: function(formContext,controlName,filter,entityLogicaName)
        {
            let control = formContext.getControl(controlName);
            if(control!=null)
            {
                control.addCustomFilter(filter,entityLogicaName);
            }
        }
        //setLookupFilter : function(formContext,controlName,)
    }
    
})();


var JunkyardDog = JunkyardDog || {};

JunkyardDog.jdog_agreement = (function()
{
    let filterCreditAuto = "";
    let fetchXmlCreditid=`<fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="true">
            <entity name="jdog_credit">
              <attribute name="jdog_creditid" />
              <attribute name="jdog_name" />
              <attribute name="createdon" />
              <order attribute="jdog_name" descending="false" />
              <filter type="and">
                <condition attribute="jdog_name" operator="not-null" />
              </filter>
              <link-entity name="jdog_jdog_credit_jdog_auto" from="jdog_creditid" to="jdog_creditid" visible="false" intersect="true">
                <link-entity name="jdog_auto" from="jdog_autoid" to="jdog_autoid" alias="ae">
                    @filter
                </link-entity>
              </link-entity>
            </entity>
          </fetch>`;
    
    let layoutXmlCreditid = `<grid name="resultset" object="4230" jump="name" select="1" icon="1" preview="1">
    <row name="result" id="userqueryid">
    <cell name="jdog_name" width="300" />
    <cell name="createdon" width="100" />
    </row></grid>`;
    function disableCalcCredControls(formContext,disable)
    {
        ControlManager.setDisabled(formContext,"jdog_creditperiod",disable);
        ControlManager.setDisabled(formContext,"jdog_creditamount",disable);
        ControlManager.setDisabled(formContext,"jdog_fullcreditamount",disable);
        ControlManager.setDisabled(formContext,"jdog_initialfee",disable);
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
            
            TabManager.setVisible(formContext,"tabCredit",true);
            ControlManager.setDisabled(formContext,"jdog_creditid",false);
        }else
        {
            ControlManager.setDisabled(formContext,"jdog_creditid",true);
        }
        
        
    }

    let contactOnChange = function(context)
    {
        showTabCredit(context);

    }
    
    let autoOnChange = function(context)
    {
        let formContext = context.getFormContext();
        showTabCredit(context);
        let autoAttr = formContext.getAttribute("jdog_auto");
        let autoValue = autoAttr.getValue();
        if(autoValue!=null)
        {
            filterCreditAuto = `<filter type="and">
                                    <condition attribute="jdog_autoid" operator="eq" value="${autoValue[0].id}" />
                                </filter>`;
            fetchXml = fetchXmlCreditid.replace("@filter",filterCreditAuto);
            ControlManager.addCustomView(formContext,"jdog_creditid","{70E2F896-09E1-4634-AA2D-2DD10D43424D}","jdog_credit","Особое представление",fetchXml,layoutXmlCreditid,true);
            
        }
    }
    
    let creditidOnChange = function(context)
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
    let agreementNameOnChange = function(context)
    {
        let formContext = context.getFormContext();
        let agreementNameAttr = formContext.getAttribute("jdog_name");
        if(agreementNameAttr!=null)
        {
            agreementNameValue = agreementNameAttr.getValue();
            if(agreementNameValue!=null)
            {
                agreementNameAttr.setValue(agreementNameValue.replace(/[^\d.-]/g, ''));
            }
        }
    }  
    return {
        onLoad : function(context)
        {
            console.log(context);
            let formContext = context.getFormContext();
            TabManager.setVisible(formContext,"tabCredit",false);
            ControlManager.setVisible(formContext,"jdog_summa",false);
            ControlManager.setVisible(formContext,"jdog_fact",false);
            ControlManager.setDisabled(formContext,"jdog_creditid",true);
            disableCalcCredControls(formContext,true);
            let contactAttr = formContext.getAttribute("jdog_contact");
            let autoAttr = formContext.getAttribute("jdog_auto");
            let creditAttr = formContext.getAttribute("jdog_creditid");
            let agreementNameAttr = formContext.getAttribute("jdog_name")//.replace(/[^\d.-]/g, '')
            ControlManager.addCustomView(formContext,"jdog_creditid","{70E2F896-09E1-4634-AA2D-2DD10D43424D}","jdog_credit","Особое представление",fetchXmlCreditid,layoutXmlCreditid,true);
            filterCreditAuto = `<filter type="and">
                                    <condition attribute="jdog_autoid" operator="eq" value="{831F7011-C8F9-EB11-94EF-000D3A29AC6D}" />
                                </filter>`;
            agreementNameAttr.addOnChange(agreementNameOnChange);
            contactAttr.addOnChange(contactOnChange);
            autoAttr.addOnChange(autoOnChange);
            creditAttr.addOnChange(creditidOnChange);
        }
    }
})();