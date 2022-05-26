Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraPivotGrid
Imports DevExpress.XtraPivotGrid.ViewInfo
Imports System.Reflection

Namespace WindowsApplication34

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs)
            pivotGridControl1.DataSource = CreateTable(20)
            AddHandler pivotGridControl1.PopupMenuShowing, New PopupMenuShowingEventHandler(AddressOf pivotGridControl1_PopupMenuShowing)
            pivotGridControl1.Fields.AddDataSourceColumn("Type", PivotArea.RowArea)
            pivotGridControl1.Fields.AddDataSourceColumn("Product", PivotArea.RowArea)
            Dim fieldYear As PivotGridField = New PivotGridField("", PivotArea.ColumnArea)
            fieldYear.DataBinding = New DataSourceColumnBinding("Date", PivotGroupInterval.DateYear)
            fieldYear.Name = "FieldYear"
            fieldYear.Caption = fieldYear.Name
            pivotGridControl1.Fields.AddRange(New PivotGridField() {fieldYear})
            Dim dataField As PivotGridField = pivotGridControl1.Fields.AddDataSourceColumn("Number", PivotArea.DataArea)
            dataField.Options.AllowRunTimeSummaryChange = True
        End Sub

        Private necessaryItems As List(Of String) = New List(Of String)(New String() {"Sum", "Max", "Average", "Min", "Count"})

        Private Sub pivotGridControl1_PopupMenuShowing(ByVal sender As Object, ByVal e As PopupMenuShowingEventArgs)
            If e.MenuType = PivotGridMenuType.HeaderSummaries Then
                Dim i As Integer = 0
                While i < e.Menu.Items.Count
                    If Not necessaryItems.Contains(e.Menu.Items(i).Caption) Then
                        e.Menu.Items.RemoveAt(i)
                    Else
                        i += 1
                    End If
                End While
            End If
        End Sub

        Private Function CreateTable(ByVal RowCount As Integer) As DataTable
            Dim tbl As DataTable = New DataTable()
            tbl.Columns.Add("Type", GetType(String))
            tbl.Columns.Add("Product", GetType(String))
            tbl.Columns.Add("Date", GetType(Date))
            tbl.Columns.Add("Number", GetType(Integer))
            Dim r As Random = New Random()
            For i As Integer = 0 To RowCount - 1
                For j As Integer = 0 To RowCount - 1
                    tbl.Rows.Add(New Object() {String.Format("Type {0}", i Mod 2), String.Format("Product {0}", i Mod 3), Date.Now.AddYears(j Mod 3), r.Next(2)})
                Next
            Next

            Return tbl
        End Function

        Private Sub pivotGridControl1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
            Dim pivot As PivotGridControl = CType(sender, PivotGridControl)
            Dim hitInfo As PivotGridHitInfo = pivot.CalcHitInfo(e.Location)
            If hitInfo.HitTest <> PivotGridHitTest.HeadersArea Then Return
            If hitInfo.HeaderField IsNot Nothing AndAlso hitInfo.HeaderField.Area = PivotArea.DataArea AndAlso hitInfo.HeaderField.Options.AllowRunTimeSummaryChange Then
                Dim pInfo As PropertyInfo = GetType(PivotGridHeadersAreaHitInfo).GetProperty("HeadersViewInfo", BindingFlags.Instance Or BindingFlags.NonPublic)
                Dim headersViewInfo As PivotHeadersViewInfoBase = TryCast(pInfo.GetValue(hitInfo.HeadersAreaInfo, Nothing), PivotHeadersViewInfoBase)
                Dim headerViewInfo As PivotHeaderViewInfo = TryCast(headersViewInfo(GetFieldItem(pivot, hitInfo.HeaderField)), PivotHeaderViewInfo)
                Dim mInfo As MethodInfo = GetType(PivotHeaderViewInfo).GetMethod("ShowSummariesMenu", BindingFlags.NonPublic Or BindingFlags.Instance)
                mInfo.Invoke(headerViewInfo, New Object() {})
            End If
        End Sub

        Private Function GetFieldItem(ByVal pivot As PivotGridControl, ByVal field As PivotGridField) As DevExpress.XtraPivotGrid.Data.PivotFieldItemBase
            Return CType(pivot, IPivotGridViewInfoDataOwner).DataViewInfo.GetFieldItem(field)
        End Function
    End Class
End Namespace
