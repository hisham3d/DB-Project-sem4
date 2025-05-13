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

namespace Project_FrontEnd
{
    public partial class OperatorLogin : Form
    {
        public OperatorLogin()
        {
            InitializeComponent();
            textBox1.Focus(); // Set focus immediately after form loads
        }

        SqlConnection con = new SqlConnection("Data Source = RUHAB - LENOVOTHI\\SQLEXPRESS; Initial Catalog = TravelEase; Integrated Security = True; Encrypt=False");

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter both ID and Password.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int OperatorID;
            if (!int.TryParse(textBox1.Text, out OperatorID))
            {
                MessageBox.Show("ID must be a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string password = textBox2.Text;

            try
            {
                con.Open();

                string query = "SELECT U.Role FROM [User] U " +
                               "JOIN Operator O ON U.UserID = O.OperatorID " +
                               "WHERE U.UserID = @ID AND U.Password = @Password";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", OperatorID);
                cmd.Parameters.AddWithValue("@Password", password);

                object result = cmd.ExecuteScalar();

                if (result != null && result.ToString() == "Operator")
                {
                    this.Hide();
                    OperatorMainPage operatorMain = new OperatorMainPage(OperatorID);
                    operatorMain.Show();
                }
                else
                {
                    MessageBox.Show("Operator does not exist or credentials are incorrect.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainPage main = new MainPage();
            main.Show();
            this.Hide();
        }

        private void OperatorLogin_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OperatorRegistration operatorRegistration = new OperatorRegistration();
            operatorRegistration.Show();
            this.Hide();
        }
    }
}
