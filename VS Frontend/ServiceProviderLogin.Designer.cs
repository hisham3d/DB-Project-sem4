using System.Drawing;
using System.Windows.Forms;

namespace Project_FrontEnd
{
    partial class ServiceProviderLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceProviderLogin));
            this.ReisterLinkLabel = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.SingInLabel2 = new System.Windows.Forms.Label();
            this.SignInLabel1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.BackButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ReisterLinkLabel
            // 
            this.ReisterLinkLabel.ActiveLinkColor = System.Drawing.Color.DodgerBlue;
            this.ReisterLinkLabel.AutoSize = true;
            this.ReisterLinkLabel.BackColor = System.Drawing.Color.Black;
            this.ReisterLinkLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReisterLinkLabel.LinkColor = System.Drawing.Color.SteelBlue;
            this.ReisterLinkLabel.Location = new System.Drawing.Point(78, 323);
            this.ReisterLinkLabel.Name = "ReisterLinkLabel";
            this.ReisterLinkLabel.Size = new System.Drawing.Size(242, 20);
            this.ReisterLinkLabel.TabIndex = 14;
            this.ReisterLinkLabel.TabStop = true;
            this.ReisterLinkLabel.Text = "Don\'t have an account yet? Register!";
            this.ReisterLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ReisterLinkLabel_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(45, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(329, 46);
            this.label1.TabIndex = 13;
            this.label1.Text = "SERVICE PROVIDER";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // SingInLabel2
            // 
            this.SingInLabel2.AutoSize = true;
            this.SingInLabel2.BackColor = System.Drawing.Color.Black;
            this.SingInLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SingInLabel2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.SingInLabel2.Location = new System.Drawing.Point(65, 196);
            this.SingInLabel2.Name = "SingInLabel2";
            this.SingInLabel2.Size = new System.Drawing.Size(73, 20);
            this.SingInLabel2.TabIndex = 12;
            this.SingInLabel2.Text = "Password:";
            // 
            // SignInLabel1
            // 
            this.SignInLabel1.AutoSize = true;
            this.SignInLabel1.BackColor = System.Drawing.Color.Black;
            this.SignInLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignInLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.SignInLabel1.Location = new System.Drawing.Point(111, 126);
            this.SignInLabel1.Name = "SignInLabel1";
            this.SignInLabel1.Size = new System.Drawing.Size(27, 20);
            this.SignInLabel1.TabIndex = 11;
            this.SignInLabel1.Text = "ID:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(144, 194);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(126, 22);
            this.textBox2.TabIndex = 10;
            this.textBox2.UseSystemPasswordChar = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(144, 126);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(126, 22);
            this.textBox1.TabIndex = 9;
            // 
            // BackButton
            // 
            this.BackButton.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.BackButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.BackButton.FlatAppearance.BorderSize = 2;
            this.BackButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackButton.ForeColor = System.Drawing.Color.White;
            this.BackButton.Location = new System.Drawing.Point(31, 393);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(83, 28);
            this.BackButton.TabIndex = 8;
            this.BackButton.Text = "BACK";
            this.BackButton.UseVisualStyleBackColor = false;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(132, 241);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 68);
            this.button1.TabIndex = 69;
            this.button1.Text = "LOGIN";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ServiceProviderLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.ReisterLinkLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SingInLabel2);
            this.Controls.Add(this.SignInLabel1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.ForeColor = System.Drawing.Color.DarkGray;
            this.Name = "ServiceProviderLogin";
            this.Text = "ServiceProviderLogin";
            this.Load += new System.EventHandler(this.ServiceProviderLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel ReisterLinkLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label SingInLabel2;
        private System.Windows.Forms.Label SignInLabel1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button BackButton;
        private Button button1;
    }
}