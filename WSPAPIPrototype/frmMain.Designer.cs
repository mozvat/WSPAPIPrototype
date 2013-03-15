namespace WSPAPIPrototype
{
    partial class frmMain
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
            this.btnPayment = new System.Windows.Forms.Button();
            this.btnGift = new System.Windows.Forms.Button();
            this.btnLoyaltyCredit = new System.Windows.Forms.Button();
            this.btnHostedCheckout = new System.Windows.Forms.Button();
            this.btnLoyalty = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPayment
            // 
            this.btnPayment.Location = new System.Drawing.Point(56, 54);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(163, 70);
            this.btnPayment.TabIndex = 0;
            this.btnPayment.Text = "&Payment";
            this.btnPayment.UseVisualStyleBackColor = true;
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            // 
            // btnGift
            // 
            this.btnGift.Location = new System.Drawing.Point(56, 130);
            this.btnGift.Name = "btnGift";
            this.btnGift.Size = new System.Drawing.Size(163, 70);
            this.btnGift.TabIndex = 1;
            this.btnGift.Text = "&Gift";
            this.btnGift.UseVisualStyleBackColor = true;
            this.btnGift.Click += new System.EventHandler(this.btnGift_Click);
            // 
            // btnLoyaltyCredit
            // 
            this.btnLoyaltyCredit.Location = new System.Drawing.Point(56, 206);
            this.btnLoyaltyCredit.Name = "btnLoyaltyCredit";
            this.btnLoyaltyCredit.Size = new System.Drawing.Size(163, 70);
            this.btnLoyaltyCredit.TabIndex = 2;
            this.btnLoyaltyCredit.Text = "Loyalty &Credit";
            this.btnLoyaltyCredit.UseVisualStyleBackColor = true;
            this.btnLoyaltyCredit.Click += new System.EventHandler(this.btnLoyaltyCredit_Click);
            // 
            // btnHostedCheckout
            // 
            this.btnHostedCheckout.Location = new System.Drawing.Point(56, 358);
            this.btnHostedCheckout.Name = "btnHostedCheckout";
            this.btnHostedCheckout.Size = new System.Drawing.Size(163, 70);
            this.btnHostedCheckout.TabIndex = 3;
            this.btnHostedCheckout.Text = "&HostedCheckout";
            this.btnHostedCheckout.UseVisualStyleBackColor = true;
            this.btnHostedCheckout.Click += new System.EventHandler(this.btnHostedCheckout_Click);
            // 
            // btnLoyalty
            // 
            this.btnLoyalty.Location = new System.Drawing.Point(56, 282);
            this.btnLoyalty.Name = "btnLoyalty";
            this.btnLoyalty.Size = new System.Drawing.Size(163, 70);
            this.btnLoyalty.TabIndex = 4;
            this.btnLoyalty.Text = "&Loyalty";
            this.btnLoyalty.UseVisualStyleBackColor = true;
            this.btnLoyalty.Click += new System.EventHandler(this.btnLoyalty_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 460);
            this.Controls.Add(this.btnLoyalty);
            this.Controls.Add(this.btnHostedCheckout);
            this.Controls.Add(this.btnLoyaltyCredit);
            this.Controls.Add(this.btnGift);
            this.Controls.Add(this.btnPayment);
            this.Name = "frmMain";
            this.Text = "API Prototype";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPayment;
        private System.Windows.Forms.Button btnGift;
        private System.Windows.Forms.Button btnLoyaltyCredit;
        private System.Windows.Forms.Button btnHostedCheckout;
        private System.Windows.Forms.Button btnLoyalty;
    }
}

