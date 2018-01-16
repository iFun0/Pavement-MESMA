namespace PavementDetection
{
    partial class MainForm
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
            this.EM_collection_bt = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.TXTBuild = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EM_collection_bt
            // 
            this.EM_collection_bt.Location = new System.Drawing.Point(71, 95);
            this.EM_collection_bt.Name = "EM_collection_bt";
            this.EM_collection_bt.Size = new System.Drawing.Size(104, 23);
            this.EM_collection_bt.TabIndex = 0;
            this.EM_collection_bt.Text = "端元收集器";
            this.EM_collection_bt.UseVisualStyleBackColor = true;
            this.EM_collection_bt.Click += new System.EventHandler(this.EM_collection_bt_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(71, 148);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "MESMA";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TXTBuild
            // 
            this.TXTBuild.Location = new System.Drawing.Point(71, 39);
            this.TXTBuild.Name = "TXTBuild";
            this.TXTBuild.Size = new System.Drawing.Size(104, 23);
            this.TXTBuild.TabIndex = 0;
            this.TXTBuild.Text = "创建标准TXT";
            this.TXTBuild.UseVisualStyleBackColor = true;
            this.TXTBuild.Click += new System.EventHandler(this.TXTBuild_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 221);
            this.Controls.Add(this.TXTBuild);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.EM_collection_bt);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button EM_collection_bt;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button TXTBuild;
    }
}