namespace StocksManagement
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnGetCode = new Button();
            btnKetQuaKinhDoanh = new Button();
            btnBalanceSheet = new Button();
            SuspendLayout();
            // 
            // btnGetCode
            // 
            btnGetCode.Location = new Point(22, 22);
            btnGetCode.Name = "btnGetCode";
            btnGetCode.Size = new Size(82, 29);
            btnGetCode.TabIndex = 0;
            btnGetCode.Text = "Lấy mã chứng khoán";
            btnGetCode.UseVisualStyleBackColor = true;
            btnGetCode.Click += btnGetCode_Click;
            // 
            // btnKetQuaKinhDoanh
            // 
            btnKetQuaKinhDoanh.Location = new Point(22, 70);
            btnKetQuaKinhDoanh.Name = "btnKetQuaKinhDoanh";
            btnKetQuaKinhDoanh.Size = new Size(152, 31);
            btnKetQuaKinhDoanh.TabIndex = 0;
            btnKetQuaKinhDoanh.Text = "Ket Qua Kinh Doanh";
            btnKetQuaKinhDoanh.TextAlign = ContentAlignment.TopCenter;
            btnKetQuaKinhDoanh.UseMnemonic = false;
            btnKetQuaKinhDoanh.UseVisualStyleBackColor = true;
            btnKetQuaKinhDoanh.Click += btnKetQuaKinhDoanh_Click;
            // 
            // btnBalanceSheet
            // 
            btnBalanceSheet.Location = new Point(22, 116);
            btnBalanceSheet.Name = "btnBalanceSheet";
            btnBalanceSheet.Size = new Size(152, 31);
            btnBalanceSheet.TabIndex = 0;
            btnBalanceSheet.Text = "Cân đối kế toán";
            btnBalanceSheet.TextAlign = ContentAlignment.TopCenter;
            btnBalanceSheet.UseMnemonic = false;
            btnBalanceSheet.UseVisualStyleBackColor = true;
            btnBalanceSheet.Click += btnBalanceSheet_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnBalanceSheet);
            Controls.Add(btnKetQuaKinhDoanh);
            Controls.Add(btnGetCode);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnGetCode;
        private Button btnKetQuaKinhDoanh;
        private Button btnBalanceSheet;
    }
}