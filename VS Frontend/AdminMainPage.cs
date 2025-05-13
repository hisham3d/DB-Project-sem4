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

namespace Project_FrontEnd
{
    public partial class AdminMainPage : Form
    {
        public AdminMainPage()
        {
            InitializeComponent();
        }
        private int adminID;
        public AdminMainPage(int userID)
        {
            InitializeComponent();
            adminID = userID;

            // You can now use travelerID inside this form
            // e.g., LoadTravelerDetails(travelerID);
        }

        string connectionString = "Data Source = RUHAB - LENOVOTHI\\SQLEXPRESS; Initial Catalog = TravelEase; Integrated Security = True; Encrypt=False";

        private void LoadTripsByRegion(string adminRegion)
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT T.TripID, T.Description, T.Duration, T.PricePerPerson, T.Capacity,
                   D.Name AS DestinationName, D.Region, T.OperatorID
            FROM Trip T
            INNER JOIN Destination D ON T.DestinationID = D.DestinationID
            WHERE D.Region = @region";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                adapter.SelectCommand.Parameters.AddWithValue("@region", adminRegion);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView3.DataSource = dt;
            }
        }

        private string GetAdminRegion(int adminId)
        {
            string region = "";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT AssignedRegion FROM Admin WHERE AdminID = @adminId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@adminId", adminId);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        region = result.ToString();
                }
            }

            return region;
        }

        private void LoadMonthlyRevenueChart()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT FORMAT(BookingDate, 'yyyy-MM') AS Month,
                   SUM(TotalAmount) AS Revenue
            FROM Booking
            WHERE Status = 'Paid'
            GROUP BY FORMAT(BookingDate, 'yyyy-MM')
            ORDER BY Month";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                chart1.Series.Clear();
                chart1.ChartAreas[0].AxisX.Title = "Month";
                chart1.ChartAreas[0].AxisY.Title = "Revenue";
                chart1.Series.Add("Revenue");

                foreach (DataRow row in dt.Rows)
                {
                    string month = row["Month"].ToString();
                    decimal revenue = Convert.ToDecimal(row["Revenue"]);
                    chart1.Series["Revenue"].Points.AddXY(month, revenue);
                }
            }
        }

        private void LoadMonthlyBookingCountChart()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT FORMAT(BookingDate, 'yyyy-MM') AS Month,
                   COUNT(*) AS BookingCount
            FROM Booking
            GROUP BY FORMAT(BookingDate, 'yyyy-MM')
            ORDER BY Month";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                chart2.Series.Clear();
                chart2.ChartAreas[0].AxisX.Title = "Month";
                chart2.ChartAreas[0].AxisY.Title = "Number of Bookings";
                chart2.Series.Add("Bookings");

                foreach (DataRow row in dt.Rows)
                {
                    string month = row["Month"].ToString();
                    int count = Convert.ToInt32(row["BookingCount"]);
                    chart2.Series["Bookings"].Points.AddXY(month, count);
                }
            }
        }

        private void LoadInappropriateReviews()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT ReviewID, Comment, Rating, ReviewDate
            FROM Review
            WHERE Rating <= 2
               OR Comment LIKE '%bad%'
               OR Comment LIKE '%terrible%'
               OR Comment LIKE '%worst%'
               OR Comment LIKE '%offensive%'";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void LoadUnapprovedUsers()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Name, Email, Phone, Role, Status FROM [User] WHERE Status = 'Unapproved'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView2.DataSource = table;
            }
        }

        private void AddAdminLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AdminAddNewAdmin addNewAdmin = new AdminAddNewAdmin();
            addNewAdmin.Show();
            this.Hide();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
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
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AdminMainPage_Load(object sender, EventArgs e)
        {
            LoadUnapprovedUsers();
            LoadTripsByRegion(GetAdminRegion(adminID));
            LoadMonthlyRevenueChart();
            LoadMonthlyBookingCountChart();
            LoadInappropriateReviews();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a trip to delete.");
                return;
            }

            int tripID = Convert.ToInt32(dataGridView3.SelectedRows[0].Cells["TripID"].Value);

            DialogResult result = MessageBox.Show("Are you sure you want to delete this trip?",
                                                  "Confirm Deletion", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string deleteQuery = "DELETE FROM Trip WHERE TripID = @tripID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@tripID", tripID);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Trip deleted successfully.");
                    }
                }

                // Refresh the grid
                string region = GetAdminRegion(adminID);
                LoadTripsByRegion(region);
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int reviewId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ReviewID"].Value);
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string deleteQuery = "DELETE FROM Review WHERE ReviewID = @ReviewID";
                    SqlCommand cmd = new SqlCommand(deleteQuery, con);
                    cmd.Parameters.AddWithValue("@ReviewID", reviewId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                MessageBox.Show("Review deleted successfully.");
                LoadInappropriateReviews(); // Refresh after deletion
            }
            else
            {
                MessageBox.Show("Please select a review to delete.");
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow != null)
            {
                int userId = Convert.ToInt32(dataGridView2.CurrentRow.Cells["UserID"].Value);
                
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string updateQuery = "UPDATE [User] SET Status = 'Approved' WHERE UserID = @userId";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("User approved successfully.");
                            LoadUnapprovedUsers(); // Refresh the grid
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Error approving user: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MainPage main = new MainPage();
            main.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
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
            }
        }
    }
}
