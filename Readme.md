<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128582707/21.2.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E2351)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [Form1.cs](./CS/WindowsApplication34/Form1.cs) (VB: [Form1.vb](./VB/WindowsApplication34/Form1.vb))
* [Program.cs](./CS/WindowsApplication34/Program.cs) (VB: [Program.vb](./VB/WindowsApplication34/Program.vb))
<!-- default file list end -->
# Invoke Summary Type menu when user hovers over the field header with a mouse


<p>It is possible to change Data Field Summary Type at runtime via the context menu if  the <a href="http://documentation.devexpress.com/#CoreLibraries/DevExpressXtraPivotGridPivotGridFieldOptions_AllowRunTimeSummaryChangetopic"><u>PivotGridField.Options.AllowRunTimeSummaryChange</u></a> property is set. By default the context menu can be shown by the left mouse click.  This example demonstrates how to invoke the context menu via the <strong>Control.MouseMove</strong> event. The <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraPivotGridPivotGridControl_CalcHitInfotopic"><u>PivotGridControl.CalcHitInfo</u></a> method is used to access corresponding view info. Please note that in this example the Reflection technology is used. In addition, this example allows you to set the <a href="http://documentation.devexpress.com/#CoreLibraries/DevExpressXtraPivotGridPivotGridFieldBase_SummaryDisplayTypetopic"><u>SummaryDisplayType</u></a> property as well. </p>

<br/>


