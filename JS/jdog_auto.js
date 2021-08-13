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
    }
    
})();

var JunkyardDog = JunkyardDog || {};
JunkyardDog.jdog_auto= (function()
{
    function setVisibleUsedControls(formContext,visible)
    {
        ControlManager.setVisible(formContext,"jdog_ownerscount",visible);
        ControlManager.setVisible(formContext,"jdog_km",visible);
        ControlManager.setVisible(formContext,"jdog_isdamaged",visible);
    }
    let usedOnChange = function(context)
    {
        let formContext = context.getFormContext();
        let usedAttr = formContext.getAttribute("jdog_used");
        if(usedAttr!=null)
        {
            setVisibleUsedControls(formContext,usedAttr.getValue());
        }
    }
    return {
        onLoad : function(context)
            {
                let formContext = context.getFormContext();
                console.log(context);
                let usedAttr = formContext.getAttribute("jdog_used");
                setVisibleUsedControls(formContext,usedAttr.getValue())
                usedAttr.addOnChange(usedOnChange);
            }
    }
})();