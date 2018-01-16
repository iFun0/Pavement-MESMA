namespace PavementDetection
{
    partial class EMCollection
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SelecteallEMbt = new System.Windows.Forms.Button();
            this.DataShowbt = new System.Windows.Forms.Button();
            this.DeleteEMbt = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.EMCProgressBar = new System.Windows.Forms.ProgressBar();
            this.EARoptbt = new System.Windows.Forms.Button();
            this.selectinversebt = new System.Windows.Forms.Button();
            this.EMColeChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SaveFileBt = new System.Windows.Forms.Button();
            this.collectMESMAbt = new System.Windows.Forms.Button();
            this.FileAddbutton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EMColeChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1026, 318);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "端元集";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1020, 298);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // SelecteallEMbt
            // 
            this.SelecteallEMbt.Location = new System.Drawing.Point(29, 18);
            this.SelecteallEMbt.Name = "SelecteallEMbt";
            this.SelecteallEMbt.Size = new System.Drawing.Size(75, 23);
            this.SelecteallEMbt.TabIndex = 0;
            this.SelecteallEMbt.Text = "选择全部";
            this.SelecteallEMbt.UseVisualStyleBackColor = true;
            this.SelecteallEMbt.Click += new System.EventHandler(this.SelecteallEMbt_Click);
            // 
            // DataShowbt
            // 
            this.DataShowbt.Location = new System.Drawing.Point(308, 18);
            this.DataShowbt.Name = "DataShowbt";
            this.DataShowbt.Size = new System.Drawing.Size(75, 23);
            this.DataShowbt.TabIndex = 0;
            this.DataShowbt.Text = "显示";
            this.DataShowbt.UseVisualStyleBackColor = true;
            this.DataShowbt.Click += new System.EventHandler(this.DataShowbt_Click);
            // 
            // DeleteEMbt
            // 
            this.DeleteEMbt.Location = new System.Drawing.Point(215, 18);
            this.DeleteEMbt.Name = "DeleteEMbt";
            this.DeleteEMbt.Size = new System.Drawing.Size(75, 23);
            this.DeleteEMbt.TabIndex = 0;
            this.DeleteEMbt.Text = "删除";
            this.DeleteEMbt.UseVisualStyleBackColor = true;
            this.DeleteEMbt.Click += new System.EventHandler(this.DeleteEMbt_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SaveFileBt);
            this.panel1.Controls.Add(this.collectMESMAbt);
            this.panel1.Controls.Add(this.FileAddbutton);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1026, 60);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.EMCProgressBar);
            this.panel2.Controls.Add(this.SelecteallEMbt);
            this.panel2.Controls.Add(this.EARoptbt);
            this.panel2.Controls.Add(this.DeleteEMbt);
            this.panel2.Controls.Add(this.selectinversebt);
            this.panel2.Controls.Add(this.DataShowbt);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 704);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1026, 56);
            this.panel2.TabIndex = 3;
            // 
            // EMCProgressBar
            // 
            this.EMCProgressBar.Location = new System.Drawing.Point(542, 18);
            this.EMCProgressBar.Name = "EMCProgressBar";
            this.EMCProgressBar.Size = new System.Drawing.Size(472, 23);
            this.EMCProgressBar.TabIndex = 1;
            // 
            // EARoptbt
            // 
            this.EARoptbt.Location = new System.Drawing.Point(401, 18);
            this.EARoptbt.Name = "EARoptbt";
            this.EARoptbt.Size = new System.Drawing.Size(75, 23);
            this.EARoptbt.TabIndex = 0;
            this.EARoptbt.Text = "EAR优化";
            this.EARoptbt.UseVisualStyleBackColor = true;
            this.EARoptbt.Click += new System.EventHandler(this.EARoptbt_Click);
            // 
            // selectinversebt
            // 
            this.selectinversebt.Location = new System.Drawing.Point(122, 18);
            this.selectinversebt.Name = "selectinversebt";
            this.selectinversebt.Size = new System.Drawing.Size(75, 23);
            this.selectinversebt.TabIndex = 0;
            this.selectinversebt.Text = "反向选择";
            this.selectinversebt.UseVisualStyleBackColor = true;
            this.selectinversebt.Click += new System.EventHandler(this.selectinversebt_Click);
            // 
            // EMColeChart
            // 
            chartArea2.AxisX.TitleFont = new System.Drawing.Font("新宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chartArea2.AxisY.TitleFont = new System.Drawing.Font("新宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chartArea2.Name = "ChartArea1";
            this.EMColeChart.ChartAreas.Add(chartArea2);
            this.EMColeChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.BackColor = System.Drawing.Color.Transparent;
            legend2.Font = new System.Drawing.Font("新宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend2.IsTextAutoFit = false;
            legend2.Name = "Legend1";
            this.EMColeChart.Legends.Add(legend2);
            this.EMColeChart.Location = new System.Drawing.Point(0, 0);
            this.EMColeChart.Name = "EMColeChart";
            this.EMColeChart.Size = new System.Drawing.Size(1026, 322);
            this.EMColeChart.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 60);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.EMColeChart);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1026, 644);
            this.splitContainer1.SplitterDistance = 322;
            this.splitContainer1.TabIndex = 5;
            // 
            // SaveFileBt
            // 
            this.SaveFileBt.Image = global::PavementDetection.Properties.Resources.disk_save_all_32px;
            this.SaveFileBt.Location = new System.Drawing.Point(158, 10);
            this.SaveFileBt.Name = "SaveFileBt";
            this.SaveFileBt.Size = new System.Drawing.Size(45, 41);
            this.SaveFileBt.TabIndex = 0;
            this.SaveFileBt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.SaveFileBt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.SaveFileBt.UseVisualStyleBackColor = true;
            this.SaveFileBt.Click += new System.EventHandler(this.SaveFileBt_Click);
            // 
            // collectMESMAbt
            // 
            this.collectMESMAbt.Image = global::PavementDetection.Properties.Resources.MESMA_m_32px;
            this.collectMESMAbt.Location = new System.Drawing.Point(228, 10);
            this.collectMESMAbt.Name = "collectMESMAbt";
            this.collectMESMAbt.Size = new System.Drawing.Size(45, 41);
            this.collectMESMAbt.TabIndex = 0;
            this.collectMESMAbt.UseVisualStyleBackColor = true;
            this.collectMESMAbt.Click += new System.EventHandler(this.collectMESMAbt_Click);
            // 
            // FileAddbutton
            // 
            this.FileAddbutton.Image = global::PavementDetection.Properties.Resources.add_to_photos_32px;
            this.FileAddbutton.Location = new System.Drawing.Point(18, 10);
            this.FileAddbutton.Name = "FileAddbutton";
            this.FileAddbutton.Size = new System.Drawing.Size(45, 41);
            this.FileAddbutton.TabIndex = 0;
            this.FileAddbutton.Tag = "文件添加";
            this.FileAddbutton.UseVisualStyleBackColor = true;
            this.FileAddbutton.Click += new System.EventHandler(this.FileAddbutton_Click);
            // 
            // button2
            // 
            this.button2.Image = global::PavementDetection.Properties.Resources.add_to_database_32px;
            this.button2.Location = new System.Drawing.Point(88, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 41);
            this.button2.TabIndex = 0;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // EMCollection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 760);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "EMCollection";
            this.Text = "端元收集器";
            this.Load += new System.EventHandler(this.EMCollection_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EMColeChart)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button FileAddbutton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button SaveFileBt;
        private System.Windows.Forms.Button SelecteallEMbt;
        private System.Windows.Forms.Button DataShowbt;
        private System.Windows.Forms.Button DeleteEMbt;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button EARoptbt;
        private System.Windows.Forms.Button selectinversebt;
        private System.Windows.Forms.Button collectMESMAbt;
        private System.Windows.Forms.DataVisualization.Charting.Chart EMColeChart;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ProgressBar EMCProgressBar;
    }
}

