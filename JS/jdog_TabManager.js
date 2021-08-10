export var TabManager = TabManager || {};

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