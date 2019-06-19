namespace Syn2DB
{
    partial class DONHANG
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
            this.btnLoadDonHang = new System.Windows.Forms.Button();
            this.dataGridViewDonHang = new System.Windows.Forms.DataGridView();
            this.btnInsertDonHang = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDonHang)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadDonHang
            // 
            this.btnLoadDonHang.Location = new System.Drawing.Point(9, 10);
            this.btnLoadDonHang.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLoadDonHang.Name = "btnLoadDonHang";
            this.btnLoadDonHang.Size = new System.Drawing.Size(106, 32);
            this.btnLoadDonHang.TabIndex = 0;
            this.btnLoadDonHang.Text = "LOAD DONHANG";
            this.btnLoadDonHang.UseVisualStyleBackColor = true;
            this.btnLoadDonHang.Click += new System.EventHandler(this.BtnLoadDonHang_Click);
            // 
            // dataGridViewDonHang
            // 
            this.dataGridViewDonHang.AllowUserToAddRows = false;
            this.dataGridViewDonHang.AllowUserToDeleteRows = false;
            this.dataGridViewDonHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDonHang.Location = new System.Drawing.Point(9, 57);
            this.dataGridViewDonHang.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridViewDonHang.Name = "dataGridViewDonHang";
            this.dataGridViewDonHang.ReadOnly = true;
            this.dataGridViewDonHang.RowHeadersWidth = 51;
            this.dataGridViewDonHang.RowTemplate.Height = 24;
            this.dataGridViewDonHang.Size = new System.Drawing.Size(682, 403);
            this.dataGridViewDonHang.TabIndex = 1;
            // 
            // btnInsertDonHang
            // 
            this.btnInsertDonHang.Location = new System.Drawing.Point(575, 10);
            this.btnInsertDonHang.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnInsertDonHang.Name = "btnInsertDonHang";
            this.btnInsertDonHang.Size = new System.Drawing.Size(116, 32);
            this.btnInsertDonHang.TabIndex = 0;
            this.btnInsertDonHang.Text = "INSERT DONHANG";
            this.btnInsertDonHang.UseVisualStyleBackColor = true;
            this.btnInsertDonHang.Click += new System.EventHandler(this.BtnInsertDonHang_Click);
            // 
            // DONHANG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 470);
            this.Controls.Add(this.dataGridViewDonHang);
            this.Controls.Add(this.btnInsertDonHang);
            this.Controls.Add(this.btnLoadDonHang);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "DONHANG";
            this.Text = "DONHANG";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DONHANG_FormClosing);
            this.Load += new System.EventHandler(this.DONHANG_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDonHang)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadDonHang;
        private System.Windows.Forms.DataGridView dataGridViewDonHang;
        private System.Windows.Forms.Button btnInsertDonHang;
    }
}