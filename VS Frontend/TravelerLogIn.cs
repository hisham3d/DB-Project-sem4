using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using Microsoft.Data.SqlClient;

namespace Project_FrontEnd
{
    public partial class TravelerLogIn : Form
    {
        public TravelerLogIn()
        {
            InitializeComponent();
            textBox1.Focus(); // Set focus immediately after form loads
        }

        SqlConnection con = new SqlConnection("Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False");
        private void BackButton_Click(object sender, EventArgs e)
        {
            MainPage main = new MainPage();
            main.Show();
            this.Hide();
        }

        private void RegisterLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TravelerRegistration travelerRegistration = new TravelerRegistration();
            travelerRegistration.Show();
            this.Hide();
        }

        private void TravelerLogIn_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter both ID and Password.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userID;
            if (!int.TryParse(textBox1.Text, out userID))
            {
                MessageBox.Show("ID must be a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string password = textBox2.Text;

            try
            {
                con.Open();

                string query = "SELECT Role FROM [User] WHERE UserID = @UserID AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@Password", password);

                object result = cmd.ExecuteScalar();

                if (result != null && result.ToString() == "Traveler")
                {
                    // Success — open TravelerMainPage
                    this.Hide();
                    TravelerMainPage travelerMain = new TravelerMainPage(userID);
                    travelerMain.Show();
                }
                else
                {
                    MessageBox.Show("Traveler does not exist or credentials are incorrect.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
