using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace Project_FrontEnd
{
    public partial class OperatorRegistration : Form
    {
        public OperatorRegistration()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False");
        private void roundedButton1_Click(object sender, EventArgs e)
        {
            con.Open();

            string role = "Operator";

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

                if (role == "Operator")
                {
                    string insertOperatorQuery = "INSERT INTO Operator (OperatorID, LicenseNumber, CompanyName, Description) " +
                                                 "VALUES (@operatorID, @license, @company, @desc)";
                    SqlCommand operatorCmd = new SqlCommand(insertOperatorQuery, con);
                    operatorCmd.Parameters.AddWithValue("@operatorID", newUserID);
                    operatorCmd.Parameters.AddWithValue("@license", textBox3.Text);
                    operatorCmd.Parameters.AddWithValue("@company", textBox1.Text);
                    operatorCmd.Parameters.AddWithValue("@desc", textBox4.Text);
                    operatorCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Operator registration completed.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void OperatorRegistration_Load(object sender, EventArgs e)
        {

        }
    }
}
