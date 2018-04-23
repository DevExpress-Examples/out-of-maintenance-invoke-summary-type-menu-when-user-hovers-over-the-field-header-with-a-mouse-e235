# Invoke Summary Type menu when user hovers over the field header with a mouse


<p>It is possible to change Data Field Summary Type at runtime via the context menu if  the <a href="http://documentation.devexpress.com/#CoreLibraries/DevExpressXtraPivotGridPivotGridFieldOptions_AllowRunTimeSummaryChangetopic"><u>PivotGridField.Options.AllowRunTimeSummaryChange</u></a> property is set. By default the context menu can be shown by the left mouse click.  This example demonstrates how to invoke the context menu via the <strong>Control.MouseMove</strong> event. The <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraPivotGridPivotGridControl_CalcHitInfotopic"><u>PivotGridControl.CalcHitInfo</u></a> method is used to access corresponding view info. Please note that in this example the Reflection technology is used. In addition, this example allows you to set the <a href="http://documentation.devexpress.com/#CoreLibraries/DevExpressXtraPivotGridPivotGridFieldBase_SummaryDisplayTypetopic"><u>SummaryDisplayType</u></a> property as well. </p>

<br/>


