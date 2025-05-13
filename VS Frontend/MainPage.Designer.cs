namespace Project_FrontEnd
{
    partial class MainPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPage));
            this.MainPageTravelerButton = new System.Windows.Forms.Button();
            this.MainPageOperatorButton = new System.Windows.Forms.Button();
            this.MainPageAdminButton = new System.Windows.Forms.Button();
            this.MainPageServiceProviderButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MainPageTravelerButton
            // 
            this.MainPageTravelerButton.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.MainPageTravelerButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.MainPageTravelerButton.Location = new System.Drawing.Point(78, 299);
            this.MainPageTravelerButton.Name = "MainPageTravelerButton";
            this.MainPageTravelerButton.Size = new System.Drawing.Size(112, 53);
            this.MainPageTravelerButton.TabIndex = 0;
            this.MainPageTravelerButton.Text = "Traveler";
            this.MainPageTravelerButton.UseVisualStyleBackColor = false;
            this.MainPageTravelerButton.Click += new System.EventHandler(this.MainPageTravelerButton_Click);
            // 
            // MainPageOperatorButton
            // 
            this.MainPageOperatorButton.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.MainPageOperatorButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.MainPageOperatorButton.Location = new System.Drawing.Point(240, 299);
            this.MainPageOperatorButton.Name = "MainPageOperatorButton";
            this.MainPageOperatorButton.Size = new System.Drawing.Size(112, 53);
            this.MainPageOperatorButton.TabIndex = 1;
            this.MainPageOperatorButton.Text = "Tour Operator";
            this.MainPageOperatorButton.UseVisualStyleBackColor = false;
            this.MainPageOperatorButton.Click += new System.EventHandler(this.MainPageOperatorButton_Click);
            // 
            // MainPageAdminButton
            // 
            this.MainPageAdminButton.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.MainPageAdminButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.MainPageAdminButton.Location = new System.Drawing.Point(409, 299);
            this.MainPageAdminButton.Name = "MainPageAdminButton";
            this.MainPageAdminButton.Size = new System.Drawing.Size(112, 53);
            this.MainPageAdminButton.TabIndex = 2;
            this.MainPageAdminButton.Text = "Admin";
            this.MainPageAdminButton.UseVisualStyleBackColor = false;
            this.MainPageAdminButton.Click += new System.EventHandler(this.MainPageAdminButton_Click);
            // 
            // MainPageServiceProviderButton
            // 
            this.MainPageServiceProviderButton.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.MainPageServiceProviderButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.MainPageServiceProviderButton.Location = new System.Drawing.Point(581, 299);
            this.MainPageServiceProviderButton.Name = "MainPageServiceProviderButton";
            this.MainPageServiceProviderButton.Size = new System.Drawing.Size(112, 53);
            this.MainPageServiceProviderButton.TabIndex = 3;
            this.MainPageServiceProviderButton.Text = "Service Provider";
            this.MainPageServiceProviderButton.UseVisualStyleBackColor = false;
            this.MainPageServiceProviderButton.Click += new System.EventHandler(this.MainPageServiceProviderButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(300, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "PLEASE SELECT YOUR ROLE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Location = new System.Drawing.Point(302, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 41);
            this.label2.TabIndex = 5;
            this.label2.Text = "TRAVEL EASE";
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MainPageServiceProviderButton);
            this.Controls.Add(this.MainPageAdminButton);
            this.Controls.Add(this.MainPageOperatorButton);
            this.Controls.Add(this.MainPageTravelerButton);
            this.Name = "MainPage";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MainPageTravelerButton;
        private System.Windows.Forms.Button MainPageOperatorButton;
        private System.Windows.Forms.Button MainPageAdminButton;
        private System.Windows.Forms.Button MainPageServiceProviderButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

