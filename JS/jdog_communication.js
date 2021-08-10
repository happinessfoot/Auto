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
JunkyardDog.jdog_communication= (function()
{
    let typeCommunOnChange = function(context)
    {
        let formContext = context.getFormContext();
        let typeCommunAttr = formContext.getAttribute("jdog_type");
        
        if(typeCommunAttr!=null)
        {
            let typeCommunSelOption = typeCommunAttr.getSelectedOption();
            let phoneControl = formContext.getControl("jdog_phone");
            let emailControl = formContext.getControl("jdog_email");
            if(typeCommunSelOption!=null)
            {
                
                if(phoneControl!=null || emailControl!=null)
                {
                    if(typeCommunSelOption.text==phoneControl.getLabel())
                    {
                        phoneControl.setVisible(true);
                        emailControl.setVisible(false);
                    }
                    else if(typeCommunSelOption.text==emailControl.getLabel())
                    {
                        emailControl.setVisible(true);
                        phoneControl.setVisible(false);
                    }
                }
            }else
            {
                emailControl.setVisible(formContext,"jdog_email",false);
                phoneControl.setVisible(formContext,"jdog_phone",false);
            }
        }
    }
    return {
        onLoad : function(context)
            {
                let formContext = context.getFormContext();
                console.log(context);
                let typeCommunAttr = formContext.getAttribute("jdog_type");//.getAttribute("jdog_type").getSelectedOption().text
                ControlManager.setVisible(formContext,"jdog_email",false);
                ControlManager.setVisible(formContext,"jdog_phone",false);
                typeCommunAttr.addOnChange(typeCommunOnChange);
            }
    }
})();