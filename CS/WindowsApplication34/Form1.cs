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
            pivotGridControl1.ShowMenu += new PivotGridMenuEventHandler(pivotGridControl1_ShowMenu);

            pivotGridControl1.Fields.Add("Type", DevExpress.XtraPivotGrid.PivotArea.RowArea);
            pivotGridControl1.Fields.Add("Product", DevExpress.XtraPivotGrid.PivotArea.RowArea);
            PivotGridField fieldYear = new PivotGridField("Date", PivotArea.ColumnArea);
            fieldYear.Name = "FieldYear";
            fieldYear.Caption = fieldYear.Name;
            fieldYear.GroupInterval = PivotGroupInterval.DateYear;
            pivotGridControl1.Fields.AddRange(new PivotGridField[] { fieldYear });

            PivotGridField dataField = pivotGridControl1.Fields.Add("Number", DevExpress.XtraPivotGrid.PivotArea.DataArea);
            dataField.Options.AllowRunTimeSummaryChange = true;

        }


        List<string> necessaryItems = new List<string>(new string[] { "Sum", "Max", "Average", "Min", "Count" });
        void pivotGridControl1_ShowMenu(object sender, PivotGridMenuEventArgs e)
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

                DXSubMenuItem displayType = new DXSubMenuItem("SummaryDisplayType");
                e.Menu.Items.Add(displayType);

                string curentSummaryDisplayType = Enum.GetName(typeof(PivotSummaryDisplayType), e.Field.SummaryDisplayType);
                foreach (string str in Enum.GetNames( typeof(PivotSummaryDisplayType)) )
                {
                    DXMenuCheckItem item = new DXMenuCheckItem(str, curentSummaryDisplayType == str);
                    item.Click += new EventHandler(ItemClick);
                    item.Tag = e.Field;
                    displayType.Items.Add(item);
                }
            }                
        }

        void ItemClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            PivotGridField field = item.Tag as PivotGridField;
            field.SummaryDisplayType = (PivotSummaryDisplayType)Enum.Parse(typeof(PivotSummaryDisplayType), item.Caption);
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
                PivotHeaderViewInfo headerViewInfo = headersViewInfo[hitInfo.HeaderField] as PivotHeaderViewInfo;

                MethodInfo mInfo = typeof(PivotHeaderViewInfo).GetMethod("ShowSummariesMenu", BindingFlags.NonPublic | BindingFlags.Instance );
                mInfo.Invoke(headerViewInfo, new object[] { });
            }
            
        }

    }
}