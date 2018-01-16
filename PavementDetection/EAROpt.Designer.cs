namespace PavementDetection
{
    partial class EAROpt
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FullconsRadiobt = new System.Windows.Forms.RadioButton();
            this.PartialconsRadiobt = new System.Windows.Forms.RadioButton();
            this.UnconsRadiobt = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.EARmaxValuecheckBox = new System.Windows.Forms.CheckBox();
            this.EARminValuecheckBox = new System.Windows.Forms.CheckBox();
            this.EARmaxValuetrackBar = new System.Windows.Forms.TrackBar();
            this.EARminValuetrackBar = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.EARmaxValuetextBox = new System.Windows.Forms.TextBox();
            this.EARminValuetextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.RMSEnumericUp = new System.Windows.Forms.NumericUpDown();
            this.RMSEcheckBox = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.EARRunbt = new System.Windows.Forms.Button();
            this.EARCancelbt = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EARmaxValuetrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EARminValuetrackBar)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RMSEnumericUp)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FullconsRadiobt);
            this.groupBox1.Controls.Add(this.PartialconsRadiobt);
            this.groupBox1.Controls.Add(this.UnconsRadiobt);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "运行模式";
            // 
            // FullconsRadiobt
            // 
            this.FullconsRadiobt.AutoSize = true;
            this.FullconsRadiobt.Location = new System.Drawing.Point(329, 20);
            this.FullconsRadiobt.Name = "FullconsRadiobt";
            this.FullconsRadiobt.Size = new System.Drawing.Size(59, 16);
            this.FullconsRadiobt.TabIndex = 0;
            this.FullconsRadiobt.TabStop = true;
            this.FullconsRadiobt.Text = "全约束";
            this.FullconsRadiobt.UseVisualStyleBackColor = true;
            this.FullconsRadiobt.CheckedChanged += new System.EventHandler(this.FullconsRadiobt_CheckedChanged);
            // 
            // PartialconsRadiobt
            // 
            this.PartialconsRadiobt.AutoSize = true;
            this.PartialconsRadiobt.Location = new System.Drawing.Point(216, 20);
            this.PartialconsRadiobt.Name = "PartialconsRadiobt";
            this.PartialconsRadiobt.Size = new System.Drawing.Size(71, 16);
            this.PartialconsRadiobt.TabIndex = 0;
            this.PartialconsRadiobt.TabStop = true;
            this.PartialconsRadiobt.Text = "部分约束";
            this.PartialconsRadiobt.UseVisualStyleBackColor = true;
            this.PartialconsRadiobt.CheckedChanged += new System.EventHandler(this.PartialconsRadiobt_CheckedChanged);
            // 
            // UnconsRadiobt
            // 
            this.UnconsRadiobt.AutoSize = true;
            this.UnconsRadiobt.Location = new System.Drawing.Point(120, 20);
            this.UnconsRadiobt.Name = "UnconsRadiobt";
            this.UnconsRadiobt.Size = new System.Drawing.Size(59, 16);
            this.UnconsRadiobt.TabIndex = 0;
            this.UnconsRadiobt.TabStop = true;
            this.UnconsRadiobt.Text = "无约束";
            this.UnconsRadiobt.UseVisualStyleBackColor = true;
            this.UnconsRadiobt.CheckedChanged += new System.EventHandler(this.UnconsRadiobt_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.EARmaxValuecheckBox);
            this.groupBox2.Controls.Add(this.EARminValuecheckBox);
            this.groupBox2.Controls.Add(this.EARmaxValuetrackBar);
            this.groupBox2.Controls.Add(this.EARminValuetrackBar);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.EARmaxValuetextBox);
            this.groupBox2.Controls.Add(this.EARminValuetextBox);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 68);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(487, 113);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "丰度约束";
            // 
            // EARmaxValuecheckBox
            // 
            this.EARmaxValuecheckBox.AutoSize = true;
            this.EARmaxValuecheckBox.Location = new System.Drawing.Point(459, 68);
            this.EARmaxValuecheckBox.Name = "EARmaxValuecheckBox";
            this.EARmaxValuecheckBox.Size = new System.Drawing.Size(15, 14);
            this.EARmaxValuecheckBox.TabIndex = 13;
            this.EARmaxValuecheckBox.UseVisualStyleBackColor = true;
            this.EARmaxValuecheckBox.CheckedChanged += new System.EventHandler(this.EARmaxValuecheckBox_CheckedChanged);
            // 
            // EARminValuecheckBox
            // 
            this.EARminValuecheckBox.AutoSize = true;
            this.EARminValuecheckBox.Location = new System.Drawing.Point(459, 25);
            this.EARminValuecheckBox.Name = "EARminValuecheckBox";
            this.EARminValuecheckBox.Size = new System.Drawing.Size(15, 14);
            this.EARminValuecheckBox.TabIndex = 14;
            this.EARminValuecheckBox.UseVisualStyleBackColor = true;
            this.EARminValuecheckBox.CheckedChanged += new System.EventHandler(this.EARminValuecheckBox_CheckedChanged);
            // 
            // EARmaxValuetrackBar
            // 
            this.EARmaxValuetrackBar.Location = new System.Drawing.Point(96, 64);
            this.EARmaxValuetrackBar.Maximum = 150;
            this.EARmaxValuetrackBar.Minimum = -50;
            this.EARmaxValuetrackBar.Name = "EARmaxValuetrackBar";
            this.EARmaxValuetrackBar.Size = new System.Drawing.Size(264, 45);
            this.EARmaxValuetrackBar.TabIndex = 9;
            this.EARmaxValuetrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.EARmaxValuetrackBar.Value = 105;
            this.EARmaxValuetrackBar.Scroll += new System.EventHandler(this.EARmaxValuetrackBar_Scroll);
            // 
            // EARminValuetrackBar
            // 
            this.EARminValuetrackBar.Location = new System.Drawing.Point(96, 23);
            this.EARminValuetrackBar.Maximum = 150;
            this.EARminValuetrackBar.Minimum = -50;
            this.EARminValuetrackBar.Name = "EARminValuetrackBar";
            this.EARminValuetrackBar.Size = new System.Drawing.Size(264, 45);
            this.EARminValuetrackBar.TabIndex = 10;
            this.EARminValuetrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.EARminValuetrackBar.Value = -5;
            this.EARminValuetrackBar.Scroll += new System.EventHandler(this.EARminValuetrackBar_Scroll);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "最大值";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(70, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 4;
            this.label8.Text = "-0.5";
            // 
            // EARmaxValuetextBox
            // 
            this.EARmaxValuetextBox.Location = new System.Drawing.Point(390, 65);
            this.EARmaxValuetextBox.Name = "EARmaxValuetextBox";
            this.EARmaxValuetextBox.ReadOnly = true;
            this.EARmaxValuetextBox.Size = new System.Drawing.Size(60, 21);
            this.EARmaxValuetextBox.TabIndex = 11;
            this.EARmaxValuetextBox.Text = "1.05";
            // 
            // EARminValuetextBox
            // 
            this.EARminValuetextBox.Location = new System.Drawing.Point(390, 22);
            this.EARminValuetextBox.Name = "EARminValuetextBox";
            this.EARminValuetextBox.ReadOnly = true;
            this.EARminValuetextBox.Size = new System.Drawing.Size(60, 21);
            this.EARminValuetextBox.TabIndex = 12;
            this.EARminValuetextBox.Text = "-0.05";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(358, 69);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 12);
            this.label10.TabIndex = 5;
            this.label10.Text = "1.5";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(358, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 12);
            this.label9.TabIndex = 6;
            this.label9.Text = "1.5";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(70, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "-0.5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "最小值";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.RMSEnumericUp);
            this.groupBox3.Controls.Add(this.RMSEcheckBox);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Location = new System.Drawing.Point(12, 187);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(487, 52);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "误差约束";
            // 
            // RMSEnumericUp
            // 
            this.RMSEnumericUp.DecimalPlaces = 3;
            this.RMSEnumericUp.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.RMSEnumericUp.Location = new System.Drawing.Point(193, 22);
            this.RMSEnumericUp.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            196608});
            this.RMSEnumericUp.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147287040});
            this.RMSEnumericUp.Name = "RMSEnumericUp";
            this.RMSEnumericUp.Size = new System.Drawing.Size(120, 21);
            this.RMSEnumericUp.TabIndex = 6;
            this.RMSEnumericUp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RMSEnumericUp.Value = new decimal(new int[] {
            25,
            0,
            0,
            196608});
            // 
            // RMSEcheckBox
            // 
            this.RMSEcheckBox.AutoSize = true;
            this.RMSEcheckBox.Location = new System.Drawing.Point(459, 25);
            this.RMSEcheckBox.Name = "RMSEcheckBox";
            this.RMSEcheckBox.Size = new System.Drawing.Size(15, 14);
            this.RMSEcheckBox.TabIndex = 5;
            this.RMSEcheckBox.UseVisualStyleBackColor = true;
            this.RMSEcheckBox.CheckedChanged += new System.EventHandler(this.RMSEcheckBox_CheckedChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(134, 25);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 12);
            this.label17.TabIndex = 3;
            this.label17.Text = "最大RMSE";
            // 
            // EARRunbt
            // 
            this.EARRunbt.Location = new System.Drawing.Point(154, 254);
            this.EARRunbt.Name = "EARRunbt";
            this.EARRunbt.Size = new System.Drawing.Size(75, 23);
            this.EARRunbt.TabIndex = 4;
            this.EARRunbt.Text = "运行";
            this.EARRunbt.UseVisualStyleBackColor = true;
            this.EARRunbt.Click += new System.EventHandler(this.EARRunbt_Click);
            // 
            // EARCancelbt
            // 
            this.EARCancelbt.Location = new System.Drawing.Point(278, 254);
            this.EARCancelbt.Name = "EARCancelbt";
            this.EARCancelbt.Size = new System.Drawing.Size(75, 23);
            this.EARCancelbt.TabIndex = 5;
            this.EARCancelbt.Text = "取消";
            this.EARCancelbt.UseVisualStyleBackColor = true;
            this.EARCancelbt.Click += new System.EventHandler(this.EARCancelbt_Click);
            // 
            // EAROpt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 292);
            this.Controls.Add(this.EARRunbt);
            this.Controls.Add(this.EARCancelbt);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EAROpt";
            this.Text = "EAR端元优化";
            this.Load += new System.EventHandler(this.EAROpt_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EARmaxValuetrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EARminValuetrackBar)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RMSEnumericUp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton FullconsRadiobt;
        private System.Windows.Forms.RadioButton PartialconsRadiobt;
        private System.Windows.Forms.RadioButton UnconsRadiobt;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox EARmaxValuecheckBox;
        private System.Windows.Forms.CheckBox EARminValuecheckBox;
        private System.Windows.Forms.TrackBar EARmaxValuetrackBar;
        private System.Windows.Forms.TrackBar EARminValuetrackBar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox EARmaxValuetextBox;
        private System.Windows.Forms.TextBox EARminValuetextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button EARRunbt;
        private System.Windows.Forms.Button EARCancelbt;
        private System.Windows.Forms.CheckBox RMSEcheckBox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown RMSEnumericUp;
    }
}