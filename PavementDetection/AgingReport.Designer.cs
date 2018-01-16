namespace PavementDetection
{
    partial class AgingReport
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
            this.label1 = new System.Windows.Forms.Label();
            this.PavementBandsComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ModelInfodataGridView = new System.Windows.Forms.DataGridView();
            this.PAModellistBox = new System.Windows.Forms.ListBox();
            this.MAModellistBox = new System.Windows.Forms.ListBox();
            this.PAModelAddBt = new System.Windows.Forms.Button();
            this.MAModelAddBt = new System.Windows.Forms.Button();
            this.HAModelAddBt = new System.Windows.Forms.Button();
            this.HAModellistBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.PixelSizeXTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PixelSizeYTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ModelParamOkbt = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ReportRoadNameTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ReportRoadTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.ReportRoadLevelcomboBox = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.ReportUseLennumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ReportPaveDatedateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.ReportPaveTypecomboBox = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.ReportYanghudateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.ReportUselencheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModelInfodataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReportUseLennumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "路面丰度影像波段";
            // 
            // PavementBandsComboBox
            // 
            this.PavementBandsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PavementBandsComboBox.FormattingEnabled = true;
            this.PavementBandsComboBox.Location = new System.Drawing.Point(128, 22);
            this.PavementBandsComboBox.Name = "PavementBandsComboBox";
            this.PavementBandsComboBox.Size = new System.Drawing.Size(60, 20);
            this.PavementBandsComboBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ModelInfodataGridView);
            this.groupBox1.Location = new System.Drawing.Point(12, 201);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(577, 182);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "模型信息";
            // 
            // ModelInfodataGridView
            // 
            this.ModelInfodataGridView.AllowUserToAddRows = false;
            this.ModelInfodataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ModelInfodataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModelInfodataGridView.Location = new System.Drawing.Point(3, 17);
            this.ModelInfodataGridView.Name = "ModelInfodataGridView";
            this.ModelInfodataGridView.RowTemplate.Height = 23;
            this.ModelInfodataGridView.Size = new System.Drawing.Size(571, 162);
            this.ModelInfodataGridView.TabIndex = 0;
            // 
            // PAModellistBox
            // 
            this.PAModellistBox.FormattingEnabled = true;
            this.PAModellistBox.ItemHeight = 12;
            this.PAModellistBox.Location = new System.Drawing.Point(33, 428);
            this.PAModellistBox.Name = "PAModellistBox";
            this.PAModellistBox.Size = new System.Drawing.Size(161, 112);
            this.PAModellistBox.TabIndex = 4;
            this.PAModellistBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PAModellistBox_MouseDoubleClick);
            // 
            // MAModellistBox
            // 
            this.MAModellistBox.FormattingEnabled = true;
            this.MAModellistBox.ItemHeight = 12;
            this.MAModellistBox.Location = new System.Drawing.Point(215, 428);
            this.MAModellistBox.Name = "MAModellistBox";
            this.MAModellistBox.Size = new System.Drawing.Size(161, 112);
            this.MAModellistBox.TabIndex = 4;
            this.MAModellistBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MAModellistBox_MouseDoubleClick);
            // 
            // PAModelAddBt
            // 
            this.PAModelAddBt.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PAModelAddBt.Image = global::PavementDetection.Properties.Resources.arrow_down;
            this.PAModelAddBt.Location = new System.Drawing.Point(88, 386);
            this.PAModelAddBt.Name = "PAModelAddBt";
            this.PAModelAddBt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PAModelAddBt.Size = new System.Drawing.Size(37, 34);
            this.PAModelAddBt.TabIndex = 6;
            this.PAModelAddBt.Tag = "";
            this.PAModelAddBt.UseVisualStyleBackColor = true;
            this.PAModelAddBt.Click += new System.EventHandler(this.PAModelAddBt_Click);
            // 
            // MAModelAddBt
            // 
            this.MAModelAddBt.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MAModelAddBt.Image = global::PavementDetection.Properties.Resources.arrow_down;
            this.MAModelAddBt.Location = new System.Drawing.Point(274, 386);
            this.MAModelAddBt.Name = "MAModelAddBt";
            this.MAModelAddBt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MAModelAddBt.Size = new System.Drawing.Size(37, 34);
            this.MAModelAddBt.TabIndex = 6;
            this.MAModelAddBt.Tag = "";
            this.MAModelAddBt.UseVisualStyleBackColor = true;
            this.MAModelAddBt.Click += new System.EventHandler(this.MAModelAddBt_Click);
            // 
            // HAModelAddBt
            // 
            this.HAModelAddBt.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HAModelAddBt.Image = global::PavementDetection.Properties.Resources.arrow_down;
            this.HAModelAddBt.Location = new System.Drawing.Point(458, 386);
            this.HAModelAddBt.Name = "HAModelAddBt";
            this.HAModelAddBt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.HAModelAddBt.Size = new System.Drawing.Size(37, 34);
            this.HAModelAddBt.TabIndex = 6;
            this.HAModelAddBt.Tag = "";
            this.HAModelAddBt.UseVisualStyleBackColor = true;
            this.HAModelAddBt.Click += new System.EventHandler(this.HAModelAddBt_Click);
            // 
            // HAModellistBox
            // 
            this.HAModellistBox.FormattingEnabled = true;
            this.HAModellistBox.ItemHeight = 12;
            this.HAModellistBox.Location = new System.Drawing.Point(398, 428);
            this.HAModellistBox.Name = "HAModellistBox";
            this.HAModellistBox.Size = new System.Drawing.Size(161, 112);
            this.HAModellistBox.TabIndex = 4;
            this.HAModellistBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.HAModellistBox_MouseDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(416, 552);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 14);
            this.label2.TabIndex = 7;
            this.label2.Text = "老化后期路面模型";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(233, 552);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 14);
            this.label3.TabIndex = 7;
            this.label3.Text = "老化中期路面模型";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(48, 552);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 14);
            this.label4.TabIndex = 7;
            this.label4.Text = "老化初期路面模型";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(216, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "像元分辨率X";
            // 
            // PixelSizeXTextBox
            // 
            this.PixelSizeXTextBox.Location = new System.Drawing.Point(293, 22);
            this.PixelSizeXTextBox.Name = "PixelSizeXTextBox";
            this.PixelSizeXTextBox.ReadOnly = true;
            this.PixelSizeXTextBox.Size = new System.Drawing.Size(62, 21);
            this.PixelSizeXTextBox.TabIndex = 8;
            this.PixelSizeXTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(361, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "m";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.PixelSizeXTextBox);
            this.groupBox2.Controls.Add(this.PixelSizeYTextBox);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.PavementBandsComboBox);
            this.groupBox2.Location = new System.Drawing.Point(15, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(574, 59);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "影像参数";
            // 
            // PixelSizeYTextBox
            // 
            this.PixelSizeYTextBox.Location = new System.Drawing.Point(466, 22);
            this.PixelSizeYTextBox.Name = "PixelSizeYTextBox";
            this.PixelSizeYTextBox.ReadOnly = true;
            this.PixelSizeYTextBox.Size = new System.Drawing.Size(56, 21);
            this.PixelSizeYTextBox.TabIndex = 8;
            this.PixelSizeYTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(528, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "m";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(389, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "像元分辨率Y";
            // 
            // ModelParamOkbt
            // 
            this.ModelParamOkbt.Location = new System.Drawing.Point(254, 588);
            this.ModelParamOkbt.Name = "ModelParamOkbt";
            this.ModelParamOkbt.Size = new System.Drawing.Size(75, 23);
            this.ModelParamOkbt.TabIndex = 10;
            this.ModelParamOkbt.Text = "确定";
            this.ModelParamOkbt.UseVisualStyleBackColor = true;
            this.ModelParamOkbt.Click += new System.EventHandler(this.ModelParamOkbt_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ReportUselencheckBox);
            this.groupBox3.Controls.Add(this.ReportYanghudateTimePicker);
            this.groupBox3.Controls.Add(this.ReportPaveDatedateTimePicker);
            this.groupBox3.Controls.Add(this.ReportUseLennumericUpDown);
            this.groupBox3.Controls.Add(this.ReportPaveTypecomboBox);
            this.groupBox3.Controls.Add(this.ReportRoadLevelcomboBox);
            this.groupBox3.Controls.Add(this.ReportRoadTypeComboBox);
            this.groupBox3.Controls.Add(this.ReportRoadNameTextBox);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(15, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(574, 118);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "公路属性信息";
            // 
            // ReportRoadNameTextBox
            // 
            this.ReportRoadNameTextBox.Location = new System.Drawing.Point(68, 24);
            this.ReportRoadNameTextBox.Name = "ReportRoadNameTextBox";
            this.ReportRoadNameTextBox.Size = new System.Drawing.Size(318, 21);
            this.ReportRoadNameTextBox.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "公路名称";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 58);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "公路类型";
            // 
            // ReportRoadTypeComboBox
            // 
            this.ReportRoadTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReportRoadTypeComboBox.FormattingEnabled = true;
            this.ReportRoadTypeComboBox.Items.AddRange(new object[] {
            "未知",
            "国道",
            "省道",
            "县道",
            "乡道",
            "专用公路"});
            this.ReportRoadTypeComboBox.Location = new System.Drawing.Point(68, 54);
            this.ReportRoadTypeComboBox.Name = "ReportRoadTypeComboBox";
            this.ReportRoadTypeComboBox.Size = new System.Drawing.Size(119, 20);
            this.ReportRoadTypeComboBox.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(202, 58);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "公路等级";
            // 
            // ReportRoadLevelcomboBox
            // 
            this.ReportRoadLevelcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReportRoadLevelcomboBox.FormattingEnabled = true;
            this.ReportRoadLevelcomboBox.Items.AddRange(new object[] {
            "未知",
            "高速公路",
            "一级公路",
            "二级公路",
            "三级公路",
            "四级公路"});
            this.ReportRoadLevelcomboBox.Location = new System.Drawing.Point(263, 54);
            this.ReportRoadLevelcomboBox.Name = "ReportRoadLevelcomboBox";
            this.ReportRoadLevelcomboBox.Size = new System.Drawing.Size(123, 20);
            this.ReportRoadLevelcomboBox.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 90);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "铺设日期";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(403, 90);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "使用年数";
            // 
            // ReportUseLennumericUpDown
            // 
            this.ReportUseLennumericUpDown.Location = new System.Drawing.Point(464, 86);
            this.ReportUseLennumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ReportUseLennumericUpDown.Name = "ReportUseLennumericUpDown";
            this.ReportUseLennumericUpDown.Size = new System.Drawing.Size(77, 21);
            this.ReportUseLennumericUpDown.TabIndex = 2;
            this.ReportUseLennumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ReportPaveDatedateTimePicker
            // 
            this.ReportPaveDatedateTimePicker.CustomFormat = "yyyy-MM-dd";
            this.ReportPaveDatedateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ReportPaveDatedateTimePicker.Location = new System.Drawing.Point(65, 86);
            this.ReportPaveDatedateTimePicker.Name = "ReportPaveDatedateTimePicker";
            this.ReportPaveDatedateTimePicker.ShowCheckBox = true;
            this.ReportPaveDatedateTimePicker.Size = new System.Drawing.Size(122, 21);
            this.ReportPaveDatedateTimePicker.TabIndex = 3;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(403, 58);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 0;
            this.label14.Text = "路面材质";
            // 
            // ReportPaveTypecomboBox
            // 
            this.ReportPaveTypecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReportPaveTypecomboBox.FormattingEnabled = true;
            this.ReportPaveTypecomboBox.Items.AddRange(new object[] {
            "未知",
            "沥青",
            "水泥",
            "砂石",
            "黄土"});
            this.ReportPaveTypecomboBox.Location = new System.Drawing.Point(464, 54);
            this.ReportPaveTypecomboBox.Name = "ReportPaveTypecomboBox";
            this.ReportPaveTypecomboBox.Size = new System.Drawing.Size(77, 20);
            this.ReportPaveTypecomboBox.TabIndex = 1;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(202, 90);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 0;
            this.label15.Text = "养护日期";
            // 
            // ReportYanghudateTimePicker
            // 
            this.ReportYanghudateTimePicker.CustomFormat = "yyyy-MM-dd";
            this.ReportYanghudateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ReportYanghudateTimePicker.Location = new System.Drawing.Point(264, 86);
            this.ReportYanghudateTimePicker.Name = "ReportYanghudateTimePicker";
            this.ReportYanghudateTimePicker.ShowCheckBox = true;
            this.ReportYanghudateTimePicker.Size = new System.Drawing.Size(122, 21);
            this.ReportYanghudateTimePicker.TabIndex = 3;
            // 
            // ReportUselencheckBox
            // 
            this.ReportUselencheckBox.AutoSize = true;
            this.ReportUselencheckBox.Location = new System.Drawing.Point(550, 90);
            this.ReportUselencheckBox.Name = "ReportUselencheckBox";
            this.ReportUselencheckBox.Size = new System.Drawing.Size(15, 14);
            this.ReportUselencheckBox.TabIndex = 4;
            this.ReportUselencheckBox.UseVisualStyleBackColor = true;
            this.ReportUselencheckBox.CheckedChanged += new System.EventHandler(this.ReportUselencheckBox_CheckedChanged);
            // 
            // AgingReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 623);
            this.Controls.Add(this.ModelParamOkbt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.HAModellistBox);
            this.Controls.Add(this.MAModellistBox);
            this.Controls.Add(this.PAModellistBox);
            this.Controls.Add(this.HAModelAddBt);
            this.Controls.Add(this.MAModelAddBt);
            this.Controls.Add(this.PAModelAddBt);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AgingReport";
            this.Text = "统计报告参数设置";
            this.Load += new System.EventHandler(this.AgingReport_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ModelInfodataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReportUseLennumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox PavementBandsComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox PAModellistBox;
        private System.Windows.Forms.ListBox MAModellistBox;
        private System.Windows.Forms.Button PAModelAddBt;
        private System.Windows.Forms.Button MAModelAddBt;
        private System.Windows.Forms.Button HAModelAddBt;
        private System.Windows.Forms.ListBox HAModellistBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView ModelInfodataGridView;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox PixelSizeXTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox PixelSizeYTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button ModelParamOkbt;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DateTimePicker ReportPaveDatedateTimePicker;
        private System.Windows.Forms.NumericUpDown ReportUseLennumericUpDown;
        private System.Windows.Forms.ComboBox ReportRoadLevelcomboBox;
        private System.Windows.Forms.ComboBox ReportRoadTypeComboBox;
        private System.Windows.Forms.TextBox ReportRoadNameTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker ReportYanghudateTimePicker;
        private System.Windows.Forms.ComboBox ReportPaveTypecomboBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox ReportUselencheckBox;

    }
}