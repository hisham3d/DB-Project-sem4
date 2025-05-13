using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.Reporting.Map.WebForms.BingMaps;

namespace Project_FrontEnd
{
    public partial class AdminAddNewAdmin : Form
    {
        public AdminAddNewAdmin()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("Data Source = RUHAB - LENOVOTHI\\SQLEXPRESS; Initial Catalog = TravelEase; Integrated Security = True; Encrypt=False");

        private void AddButton_Click_1(object sender, EventArgs e)
        {
            con.Open();

            string role = "Admin";

            // Step 1: Insert into [User] table
            string insertUserQuery = "INSERT INTO [User] (Name, Email, Phone, Password, Role) " +
                                     "VALUES (@name, @email, @phone, @password, @role); " +
                                     "SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(insertUserQuery, con);
            cmd.Parameters.AddWithValue("@name", textBox1.Text);
            cmd.Parameters.AddWithValue("@email", textBox3.Text);
            cmd.Parameters.AddWithValue("@phone", textBox4.Text);
            cmd.Parameters.AddWithValue("@password", textBox2.Text);
            cmd.Parameters.AddWithValue("@role", role);

            try
            {
                int newUserID = Convert.ToInt32(cmd.ExecuteScalar());

                if (role == "Admin")
                {
                    string insertAdminQuery = "INSERT INTO Admin (AdminID, AssignedRegion) VALUES (@adminID, @region)";
                    SqlCommand adminCmd = new SqlCommand(insertAdminQuery, con);
                    adminCmd.Parameters.AddWithValue("@adminID", newUserID);
                    adminCmd.Parameters.AddWithValue("@region", textBox5.Text);
                    adminCmd.ExecuteNonQuery();
                }

                MessageBox.Show("User registration completed.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void AdminAddNewAdmin_Load(object sender, EventArgs e)
        {

        }
    }
}