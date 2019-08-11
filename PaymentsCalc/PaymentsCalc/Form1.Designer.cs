namespace PaymentsCalc
{
    partial class Calc
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.fileSelectbtn = new System.Windows.Forms.Button();
            this.lbPay = new System.Windows.Forms.ListBox();
            this.clbBank = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbResult = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // fileSelectbtn
            // 
            this.fileSelectbtn.Location = new System.Drawing.Point(12, 12);
            this.fileSelectbtn.Name = "fileSelectbtn";
            this.fileSelectbtn.Size = new System.Drawing.Size(103, 33);
            this.fileSelectbtn.TabIndex = 0;
            this.fileSelectbtn.Text = "选择文件";
            this.fileSelectbtn.UseVisualStyleBackColor = true;
            this.fileSelectbtn.Click += new System.EventHandler(this.fileSelectbtn_Click);
            // 
            // lbPay
            // 
            this.lbPay.FormattingEnabled = true;
            this.lbPay.ItemHeight = 20;
            this.lbPay.Location = new System.Drawing.Point(156, 75);
            this.lbPay.Name = "lbPay";
            this.lbPay.Size = new System.Drawing.Size(209, 424);
            this.lbPay.TabIndex = 4;
            // 
            // clbBank
            // 
            this.clbBank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clbBank.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clbBank.FormattingEnabled = true;
            this.clbBank.Location = new System.Drawing.Point(534, 75);
            this.clbBank.Name = "clbBank";
            this.clbBank.Size = new System.Drawing.Size(150, 32);
            this.clbBank.TabIndex = 7;
            this.clbBank.SelectionChangeCommitted += new System.EventHandler(this.clbBank_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Due Payments：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(422, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Bank Transfer";
            // 
            // lbResult
            // 
            this.lbResult.FormattingEnabled = true;
            this.lbResult.ItemHeight = 20;
            this.lbResult.Location = new System.Drawing.Point(426, 215);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(321, 284);
            this.lbResult.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(426, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "label3";
            // 
            // Calc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbResult);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clbBank);
            this.Controls.Add(this.lbPay);
            this.Controls.Add(this.fileSelectbtn);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Calc";
            this.Text = "Calc";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button fileSelectbtn;
        private System.Windows.Forms.ListBox lbPay;
        private System.Windows.Forms.ComboBox clbBank;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lbResult;
        private System.Windows.Forms.Label label3;
    }
}

