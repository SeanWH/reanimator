﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Excel;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Collections;

namespace Reanimator.Forms
{
    public partial class ExcelTableForm : Form, IMdiChildBase
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class TableIndexDataSource
        {
            public int Unknowns1 { get; set; }
            public int Unknowns2 { get; set; }
            public int Unknowns3 { get; set; }
            public int Unknowns4 { get; set; }
        };

        ExcelTable excelTable;
        StringsFile stringsFile;
        TableDataSet tableDataSet;

        public String GetExcelTableName()
        {
            return excelTable.StringId;
        }

        public ExcelTableForm(Object table, TableDataSet xlsDataSet)
        {
            excelTable = table as ExcelTable;
            stringsFile = table as StringsFile;
            tableDataSet = xlsDataSet;

            Init();

            ProgressForm progress = new ProgressForm(LoadTable, table);
            progress.ShowDialog(this);
        }

        private void Init()
        {
            InitializeComponent();

            dataGridView.DoubleBuffered(true);
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            dataGridView.DataSource = tableDataSet.XlsDataSet;
        }

        private void LoadTable(ProgressForm progress, Object var)
        {
            // this merely checks the table is already in the dataset
            // if not - then it will load it in
            tableDataSet.LoadTable(progress, var);

            if (stringsFile != null)
            {
                dataGridView.DataMember = stringsFile.Name;
                return;
            }

            if (excelTable != null)
            {
                dataGridView.DataMember = excelTable.StringId;
            }
            else
            {
                return;
            }

            // generate the table index data source
            // TODO is there a better way?
            // TODO remove me once unknowns no longer unknowns
            List<TableIndexDataSource> tdsList = new List<TableIndexDataSource>();
            int[][] intArrays = { excelTable.TableIndicies, excelTable.Unknowns1, excelTable.Unknowns2, excelTable.Unknowns3, excelTable.Unknowns4 };
            for (int i = 0; i < intArrays.Length; i++)
            {
                if (intArrays[i] == null)
                {
                    continue;
                }

                for (int j = 0; j < intArrays[i].Length; j++)
                {
                    TableIndexDataSource tds;

                    if (tdsList.Count <= j)
                    {
                        tdsList.Add(new TableIndexDataSource());
                    }

                    tds = tdsList[j];
                    switch (i)
                    {
                        case 0:
                            // should we still use the "official" one?
                            // or leave as autogenerated - has anyone ever seen it NOT be ascending from 0?
                            // TODO
                            //dataGridView[i, j].Value = intArrays[i][j];
                            break;
                        case 1:
                            tds.Unknowns1 = intArrays[i][j];
                            break;
                        case 2:
                            tds.Unknowns2 = intArrays[i][j];
                            break;
                        case 3:
                            tds.Unknowns3 = intArrays[i][j];
                            break;
                        case 4:
                            tds.Unknowns4 = intArrays[i][j];
                            break;
                    }
                }
            }

            dataGridView2.DataSource = tdsList.ToArray();

            if (intArrays[4] == null)
            {
                dataGridView2.Columns.RemoveAt(3);
            }
            if (intArrays[3] == null)
            {
                dataGridView2.Columns.RemoveAt(2);
            }
            if (intArrays[2] == null)
            {
                dataGridView2.Columns.RemoveAt(1);
            }
            if (intArrays[1] == null)
            {
                dataGridView2.Columns.RemoveAt(0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*DataTable affixTable = xlsDataSet.Tables["AFFIXES"];
            EnumerableRowCollection<DataRow> query = from affix in affixTable.AsEnumerable()
                                                     where affix.Field<string>("affix").CompareTo("-1") != 0
                                                     orderby affix.Field<string>("affix_string")
                                                     select affix;

            DataView view = query.AsDataView();
            */

            /*   EnumerableRowCollection<DataRow> query2 = from affix in view.GetEnumerator()
                                                        where affix.Field<string>("affix_string").StartsWith("Pet")
                                                        orderby affix.Field<string>("affix_string")
                                                        select affix;

               view = query2.AsDataView();
            dataGridView.DataSource = view;
            dataGridView.DataMember = null;*/


            //DataTable dataTable = xlsDataSet.Tables[0];
            //   DataRow[] dataRows = dataTable.Select("name = 'goggles'");
        }

        public void SaveButton()
        {
            byte[] excelFileData = excelTable.GenerateExcelFile((DataSet)this.dataGridView.DataSource);

            using (FileStream fs = new FileStream("test.txt.cooked", FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(excelFileData, 0, excelFileData.Length);
            }
        }
    }

    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}