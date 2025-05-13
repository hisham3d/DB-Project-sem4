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
    public partial class ServiceProviderLogin : Form
    {
        public ServiceProviderLogin()
        {
            InitializeComponent(); 
            textBox1.Focus(); // Set focus immediately after form loads
        }

        SqlConnection con = new SqlConnection("Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False");

        private void LoginButton_Click(object sender, EventArgs e)
        {
            int providerId = Convert.ToInt32(textBox1.Text); // unsafe if input is not a valid number
            string password = textBox2.Text;

            con.Open();

            string querycheck = "SELECT * FROM random_table WHERE ID = @providerId AND password = @password"; // var changes

            SqlCommand cmd = new SqlCommand(querycheck, con);
            cmd.Parameters.AddWithValue("@providerId", providerId);
            cmd.Parameters.AddWithValue("@password", password);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            con.Close();

            if (dt.Rows.Count != 0)
            {
                TravelerMainPage traveler = new TravelerMainPage();
                traveler.Show();
                this.Hide();
            }

            else
            {
                MessageBox.Show("Wrong ID or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainPage main = new MainPage();
            main.Show();
            this.Hide();
        }

        private void ReisterLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ServiceProviderRegistration providerRegistration = new ServiceProviderRegistration();
            providerRegistration.Show();
            this.Hide();
        }

        private void ServiceProviderLogin_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter both ID and Password.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int ServiceProviderID;
            if (!int.TryParse(textBox1.Text, out ServiceProviderID))
            {
                MessageBox.Show("ID must be a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string password = textBox2.Text;

            try
            {
                con.Open();

                string query = "SELECT U.Role FROM [User] U " +
                               "JOIN ServiceProvider S ON U.UserID = S.ProviderID " +
                               "WHERE U.UserID = @ID AND U.Password = @Password";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", ServiceProviderID);
                cmd.Parameters.AddWithValue("@Password", password);

                object result = cmd.ExecuteScalar();

                if (result != null && result.ToString() == "Service Provider")
                {
                    this.Hide();
                    ServiceProviderMainPage providerMain = new ServiceProviderMainPage(ServiceProviderID);
                    providerMain.Show();
                }
                else
                {
                    MessageBox.Show("ServiceProvider does not exist or credentials are incorrect.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
