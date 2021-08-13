$(document).ready(function()
{
    "use strict";
    var oParameters = GetGlobalContext().getQueryStringParameters();
    var sParentCustId = oParameters.id;
    fnOnPageLoad(sParentCustId)
});
function fnOnPageLoad(sParentCustId)
{
    console.log("sParentCustId:"+sParentCustId);
    "use strict";
    var conFetchXML = `<fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="true">
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
            <filter type="and">
                <condition attribute="jdog_autoid" operator="eq" value="${sParentCustId}" />
            </filter>
        </link-entity>
      </link-entity>
    </entity>
  </fetch>`;
  path = "?fetchXml="+conFetchXML;
  //Xrm.WebApi.retrieveMultiple
}