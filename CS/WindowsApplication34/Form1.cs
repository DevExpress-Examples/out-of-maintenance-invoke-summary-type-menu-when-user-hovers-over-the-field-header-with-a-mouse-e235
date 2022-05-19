using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraPivotGrid;
using DevExpress.Data.PivotGrid;
using DevExpress.Utils.Menu;
using DevExpress.XtraPivotGrid.ViewInfo;
using System.Reflection;

namespace WindowsApplication34
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pivotGridControl1.DataSource = CreateTable(20);
            pivotGridControl1.PopupMenuShowing +=new PopupMenuShowingEventHandler(pivotGridControl1_PopupMenuShowing); 

            pivotGridControl1.Fields.AddDataSourceColumn("Type", DevExpress.XtraPivotGrid.PivotArea.RowArea);
            pivotGridControl1.Fields.AddDataSourceColumn("Product", DevExpress.XtraPivotGrid.PivotArea.RowArea);
            PivotGridField fieldYear = new PivotGridField("", PivotArea.ColumnArea);
            fieldYear.DataBinding = new DataSourceColumnBinding("Date", PivotGroupInterval.DateYear);
            fieldYear.Name = "FieldYear";
            fieldYear.Caption = fieldYear.Name;
            pivotGridControl1.Fields.AddRange(new PivotGridField[] { fieldYear });

            PivotGridField dataField = pivotGridControl1.Fields.AddDataSourceColumn("Number", DevExpress.XtraPivotGrid.PivotArea.DataArea);
            dataField.Options.AllowRunTimeSummaryChange = true;

        }

        List<string> necessaryItems = new List<string>(new string[] { "Sum", "Max", "Average", "Min", "Count" });
        void pivotGridControl1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {         
            if (e.MenuType == PivotGridMenuType.HeaderSummaries)
            {
                int i = 0;
                while (i < e.Menu.Items.Count)
                {
                    if (!necessaryItems.Contains(e.Menu.Items[i].Caption))
                        e.Menu.Items.RemoveAt(i);
                    else
                        i++;                    
                }
            }                
        }

        private DataTable CreateTable(int RowCount)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Type", typeof(string));
            tbl.Columns.Add("Product", typeof(string));
            tbl.Columns.Add("Date", typeof(DateTime));
            tbl.Columns.Add("Number", typeof(int));

            Random r = new Random();
            for (int i = 0; i < RowCount; i++)

                for (int j = 0; j < RowCount; j++)
                    tbl.Rows.Add(new object[] { String.Format("Type {0}", i % 2), String.Format("Product {0}", i % 3), DateTime.Now.AddYears(j % 3), r.Next(2) });
            return tbl;
        }

        private void pivotGridControl1_MouseMove(object sender, MouseEventArgs e)
        {
            PivotGridControl pivot = (PivotGridControl)sender;
            PivotGridHitInfo hitInfo = pivot.CalcHitInfo(e.Location);
            if (hitInfo.HitTest != PivotGridHitTest.HeadersArea) return;
            if (hitInfo.HeaderField != null && hitInfo.HeaderField.Area == PivotArea.DataArea && hitInfo.HeaderField.Options.AllowRunTimeSummaryChange)
            {
                PropertyInfo pInfo = typeof(PivotGridHeadersAreaHitInfo).GetProperty("HeadersViewInfo", BindingFlags.Instance | BindingFlags.NonPublic);
                PivotHeadersViewInfoBase headersViewInfo = pInfo.GetValue(hitInfo.HeadersAreaInfo, null) as PivotHeadersViewInfoBase;
                PivotHeaderViewInfo headerViewInfo = headersViewInfo[ GetFieldItem( pivot, hitInfo.HeaderField ) ] as PivotHeaderViewInfo;

                MethodInfo mInfo = typeof(PivotHeaderViewInfo).GetMethod("ShowSummariesMenu", BindingFlags.NonPublic | BindingFlags.Instance );
                mInfo.Invoke(headerViewInfo, new object[] { });
            }            
        }

        private DevExpress.XtraPivotGrid.Data.PivotFieldItemBase GetFieldItem(PivotGridControl pivot, PivotGridField field)
        {
            return ((IPivotGridViewInfoDataOwner)pivot).DataViewInfo.GetFieldItem(field);
        }

        

    }
}