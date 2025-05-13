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
    public partial class TravelerRegistration : Form
    {
        public TravelerRegistration()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False");
        private void roundedButton1_Click(object sender, EventArgs e)
        {
            con.Open();

            string role = "Traveler";

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

                if (role == "Traveler")
                {
                    string insertTravelerQuery = "INSERT INTO Traveler (TravelerID, Age, Gender, Nationality) " +
                                                 "VALUES (@travelerID, @age, @gender, @nationality)";
                    SqlCommand travelerCmd = new SqlCommand(insertTravelerQuery, con);
                    travelerCmd.Parameters.AddWithValue("@travelerID", newUserID);
                    travelerCmd.Parameters.AddWithValue("@age", int.Parse(textBox2.Text));
                    travelerCmd.Parameters.AddWithValue("@gender", textBox4.Text);
                    travelerCmd.Parameters.AddWithValue("@nationality", textBox6.Text);
                    travelerCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Traveler registered successfully.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error: " + ex.Message, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid age.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                con.Close();
            }
        }

        private void TravelerRegistration_Load(object sender, EventArgs e)
        {

        }
    }
}
