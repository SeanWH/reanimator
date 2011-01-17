﻿namespace Reanimator.Forms
{
    partial class ExcelTableForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExcelTableForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tableDataPage = new System.Windows.Forms.TabPage();
            this._tableData_DataGridView = new System.Windows.Forms.DataGridView();
            this.indexArraysPage = new System.Windows.Forms.TabPage();
            this.indexArrays_DataGridView = new System.Windows.Forms.DataGridView();
            this.stringsPage = new System.Windows.Forms.TabPage();
            this.strings_ListBox = new System.Windows.Forms.ListBox();
            this.rowViewPage = new System.Windows.Forms.TabPage();
            this.rows_LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.rows_ListBox = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.copyScriptLabel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripButton();
            this.tabControl1.SuspendLayout();
            this.tableDataPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableData_DataGridView)).BeginInit();
            this.indexArraysPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.indexArrays_DataGridView)).BeginInit();
            this.stringsPage.SuspendLayout();
            this.rowViewPage.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tableDataPage);
            this.tabControl1.Controls.Add(this.indexArraysPage);
            this.tabControl1.Controls.Add(this.stringsPage);
            this.tabControl1.Controls.Add(this.rowViewPage);
            this.tabControl1.Location = new System.Drawing.Point(0, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(817, 580);
            this.tabControl1.TabIndex = 4;
            // 
            // tableDataPage
            // 
            this.tableDataPage.Controls.Add(this._tableData_DataGridView);
            this.tableDataPage.Location = new System.Drawing.Point(4, 24);
            this.tableDataPage.Name = "tableDataPage";
            this.tableDataPage.Padding = new System.Windows.Forms.Padding(3);
            this.tableDataPage.Size = new System.Drawing.Size(809, 552);
            this.tableDataPage.TabIndex = 0;
            this.tableDataPage.Text = "Table Data";
            this.tableDataPage.UseVisualStyleBackColor = true;
            // 
            // _tableData_DataGridView
            // 
            this._tableData_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableData_DataGridView.Location = new System.Drawing.Point(3, 3);
            this._tableData_DataGridView.Name = "_tableData_DataGridView";
            this._tableData_DataGridView.Size = new System.Drawing.Size(803, 546);
            this._tableData_DataGridView.TabIndex = 1;
            this._tableData_DataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._TableData_DataGridView_CellDoubleClick);
            this._tableData_DataGridView.KeyUp += new System.Windows.Forms.KeyEventHandler(this._TableData_DataGridView_KeyUp);
            // 
            // indexArraysPage
            // 
            this.indexArraysPage.Controls.Add(this.indexArrays_DataGridView);
            this.indexArraysPage.Location = new System.Drawing.Point(4, 24);
            this.indexArraysPage.Name = "indexArraysPage";
            this.indexArraysPage.Padding = new System.Windows.Forms.Padding(3);
            this.indexArraysPage.Size = new System.Drawing.Size(809, 552);
            this.indexArraysPage.TabIndex = 1;
            this.indexArraysPage.Text = "Index Arrays";
            this.indexArraysPage.UseVisualStyleBackColor = true;
            // 
            // indexArrays_DataGridView
            // 
            this.indexArrays_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.indexArrays_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.indexArrays_DataGridView.Location = new System.Drawing.Point(-5, 0);
            this.indexArrays_DataGridView.Name = "indexArrays_DataGridView";
            this.indexArrays_DataGridView.Size = new System.Drawing.Size(743, 561);
            this.indexArrays_DataGridView.TabIndex = 0;
            // 
            // stringsPage
            // 
            this.stringsPage.Controls.Add(this.strings_ListBox);
            this.stringsPage.Location = new System.Drawing.Point(4, 24);
            this.stringsPage.Name = "stringsPage";
            this.stringsPage.Padding = new System.Windows.Forms.Padding(3);
            this.stringsPage.Size = new System.Drawing.Size(809, 552);
            this.stringsPage.TabIndex = 2;
            this.stringsPage.Text = "Strings";
            this.stringsPage.UseVisualStyleBackColor = true;
            // 
            // strings_ListBox
            // 
            this.strings_ListBox.FormattingEnabled = true;
            this.strings_ListBox.ItemHeight = 15;
            this.strings_ListBox.Location = new System.Drawing.Point(7, 7);
            this.strings_ListBox.Name = "strings_ListBox";
            this.strings_ListBox.Size = new System.Drawing.Size(245, 649);
            this.strings_ListBox.TabIndex = 0;
            // 
            // rowViewPage
            // 
            this.rowViewPage.Controls.Add(this.rows_LayoutPanel);
            this.rowViewPage.Controls.Add(this.rows_ListBox);
            this.rowViewPage.Location = new System.Drawing.Point(4, 24);
            this.rowViewPage.Name = "rowViewPage";
            this.rowViewPage.Padding = new System.Windows.Forms.Padding(3);
            this.rowViewPage.Size = new System.Drawing.Size(809, 552);
            this.rowViewPage.TabIndex = 3;
            this.rowViewPage.Text = "Row View";
            this.rowViewPage.UseVisualStyleBackColor = true;
            // 
            // rows_LayoutPanel
            // 
            this.rows_LayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rows_LayoutPanel.AutoScroll = true;
            this.rows_LayoutPanel.ColumnCount = 2;
            this.rows_LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.rows_LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.rows_LayoutPanel.Location = new System.Drawing.Point(231, 7);
            this.rows_LayoutPanel.Name = "rows_LayoutPanel";
            this.rows_LayoutPanel.RowCount = 1;
            this.rows_LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.rows_LayoutPanel.Size = new System.Drawing.Size(542, 531);
            this.rows_LayoutPanel.TabIndex = 1;
            // 
            // rows_ListBox
            // 
            this.rows_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.rows_ListBox.FormattingEnabled = true;
            this.rows_ListBox.ItemHeight = 15;
            this.rows_ListBox.Location = new System.Drawing.Point(7, 7);
            this.rows_ListBox.Name = "rows_ListBox";
            this.rows_ListBox.Size = new System.Drawing.Size(216, 514);
            this.rows_ListBox.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.toolStripSeparator2,
            this.copyScriptLabel,
            this.toolStripSeparator4,
            this.toolStripButton3,
            this.toolStripSeparator3,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 593);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip1.Size = new System.Drawing.Size(817, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(47, 22);
            this.toolStripButton1.Text = "Reload";
            this.toolStripButton1.ToolTipText = "Reloads this table.";
            this.toolStripButton1.Click += new System.EventHandler(this.regenTable_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(39, 22);
            this.toolStripButton2.Text = "Dupe";
            this.toolStripButton2.ToolTipText = "Duplicates the selected rows and appends them to the end of the table.";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // copyScriptLabel
            // 
            this.copyScriptLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.copyScriptLabel.Image = ((System.Drawing.Image)(resources.GetObject("copyScriptLabel.Image")));
            this.copyScriptLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyScriptLabel.Name = "copyScriptLabel";
            this.copyScriptLabel.Size = new System.Drawing.Size(86, 22);
            this.copyScriptLabel.Text = "Copy as Script";
            this.copyScriptLabel.ToolTipText = "Copy the selected rows as a Reanimator script.";
            this.copyScriptLabel.Click += new System.EventHandler(this.copyScriptLabel_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(44, 22);
            this.toolStripButton3.Text = "Export";
            this.toolStripButton3.ToolTipText = "Export a tab delimited txt file of this table.";
            this.toolStripButton3.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLabel1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripLabel1.Image")));
            this.toolStripLabel1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(47, 22);
            this.toolStripLabel1.Text = "Import";
            this.toolStripLabel1.ToolTipText = "Open a tab delimited txt file of this table.";
            this.toolStripLabel1.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ExcelTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 618);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ExcelTableForm";
            this.Text = "ExcelTable";
            this.tabControl1.ResumeLayout(false);
            this.tableDataPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._tableData_DataGridView)).EndInit();
            this.indexArraysPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.indexArrays_DataGridView)).EndInit();
            this.stringsPage.ResumeLayout(false);
            this.rowViewPage.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tableDataPage;
        private System.Windows.Forms.TabPage indexArraysPage;
        private System.Windows.Forms.DataGridView indexArrays_DataGridView;
        public System.Windows.Forms.DataGridView _tableData_DataGridView;
        private System.Windows.Forms.TabPage stringsPage;
        private System.Windows.Forms.ListBox strings_ListBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabPage rowViewPage;
        private System.Windows.Forms.ListBox rows_ListBox;
        private System.Windows.Forms.TableLayoutPanel rows_LayoutPanel;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton copyScriptLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripLabel1;
    }
}