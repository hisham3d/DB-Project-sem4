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

namespace Project_FrontEnd
{
    public partial class ServiceProviderRegistration : Form
    {
        public ServiceProviderRegistration()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False");

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            //sql query to insert in db and validation checks
            con.Open();

            string role = "Service Provider";

            string insertUserQuery = "INSERT INTO [User] (Name, Email, Phone, Password, Role) " +
                                     "VALUES (@name, @email, @phone, @password, @role); " +
                                     "SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(insertUserQuery, con);
            cmd.Parameters.AddWithValue("@name", textBox1.Text);
            cmd.Parameters.AddWithValue("@email", textBox3.Text);
            cmd.Parameters.AddWithValue("@phone", textBox4.Text);
            cmd.Parameters.AddWithValue("@password", textBox2.Text);
            cmd.Parameters.AddWithValue("@role", role);
            comboBox1.Items.AddRange(new string[] { "Hotel", "Transport", "Guide" });
            try
            {
                int newUserID = Convert.ToInt32(cmd.ExecuteScalar());

                if (role == "Service Provider")
                {
                    string insertSPQuery = "INSERT INTO ServiceProvider (ProviderID, ContactInfo, Type, BusinessName) " +
                                           "VALUES (@providerID, @contactInfo, @type, @businessName)";
                    SqlCommand spCmd = new SqlCommand(insertSPQuery, con);
                    spCmd.Parameters.AddWithValue("@providerID", newUserID);
                    spCmd.Parameters.AddWithValue("@contactInfo", textBox4.Text);
                    spCmd.Parameters.AddWithValue("@type", comboBox1.SelectedItem.ToString());
                    spCmd.Parameters.AddWithValue("@businessName", textBox1.Text);
                    spCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Service Provider registered successfully.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ServiceProviderRegistration_Load(object sender, EventArgs e)
        {

        }
    }
}