export var ControlManager = ControlManager || {};

ControlManager = (function()
{
    return{
        hide: function (formContext,controlName)
        {
            let control = formContext.getControl(controlName);
            console.log(control);
            if(control!=null)
            {
                control.setVisible(false);
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