var JunkyardDog = JunkyardDog || {};
JunkyardDog.jdog_credit= (function()
{
    function checkDeltaDates(formContext)
    {
        let dateStartAttr = formContext.getAttribute("jdog_datestart");
        let dateEndAttr = formContext.getAttribute("jdog_dateend");
        if(dateStartAttr!=null && dateEndAttr!=null)
        {
            let dateStartValue = dateStartAttr.getValue();
            let dateEndValue = dateEndAttr.getValue();
            if(dateEndValue!=null && dateStartValue!=null)
            {
                dateStartValue.setFullYear(dateStartValue.getFullYear()+1);
                console.log(dateStartValue);
                console.log(dateEndValue);
                if(dateStartValue<=dateEndValue)
                {
                    return true;
                }
            }
        }
        return false
    }
    let onSave = function(context)
    {
        var saveEvent = context.getEventArgs();
        console.log(checkDeltaDates(context.getFormContext()));
        if(!checkDeltaDates(context.getFormContext()))
        {
            alert("Сохранение прервано!\nДата окончания должна быть больше даты начала, не менее, чем на год.")
            saveEvent.preventDefault();
        }
        
    }
    return {
        onLoad : function(context)
            {
                let formContext = context.getFormContext();
                console.log(context);
                console.log(checkDeltaDates(formContext));
                formContext.data.entity.addOnSave(onSave);
            }
    }
})();