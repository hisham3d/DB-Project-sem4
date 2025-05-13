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
    public partial class ServiceProviderMainPage : Form
    {
        public ServiceProviderMainPage()
        {
            InitializeComponent();
        }
        private int providerID;
        public ServiceProviderMainPage(int userID)
        {
            InitializeComponent();
            providerID = userID;

        }

        string connectionString = "Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False";

        private void LoadUnapprovedTripResources()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT TR.ResourceID, TR.ResourceType, TR.TripID, SP.BusinessName, TR.Status
            FROM TripResource TR
            INNER JOIN ServiceProvider SP ON TR.ProviderID = SP.ProviderID
            WHERE TR.Status = 'Unapproved' AND TR.ProviderID = @ProviderID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProviderID", providerID);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void LoadResourceProfiles()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ResourceProfile WHERE ProviderID = @ProviderID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProviderID", providerID);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView4.DataSource = dt;
            }
        }

        private void dataGridView4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView4.CurrentRow != null)
            {
                int providerID = Convert.ToInt32(dataGridView4.CurrentRow.Cells["ProviderID"].Value);
                string availability = dataGridView4.CurrentRow.Cells["Availability"].Value.ToString();
                string details = dataGridView4.CurrentRow.Cells["Details"].Value.ToString();
                decimal rating = Convert.ToDecimal(dataGridView4.CurrentRow.Cells["Rating"].Value);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string updateQuery = @"
                UPDATE ResourceProfile 
                SET Availability = @availability, 
                    Details = @details, 
                    Rating = @rating 
                WHERE ProviderID = @providerID";

                    SqlCommand cmd = new SqlCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@availability", availability);
                    cmd.Parameters.AddWithValue("@details", details);
                    cmd.Parameters.AddWithValue("@rating", rating);
                    cmd.Parameters.AddWithValue("@providerID", providerID);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Service updated successfully.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error updating service: " + ex.Message);
                    }
                }
            }
        }

        private void LoadPendingBookings(int providerID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT b.BookingID, b.BookingDate, b.TotalAmount, b.Status, 
                   t.TripID, t.Description, t.Capacity
            FROM Booking b
            JOIN Trip t ON b.TripID = t.TripID
            JOIN TripResource tr ON t.TripID = tr.TripID
            WHERE b.Status = 'Unpaid' AND tr.ProviderID = @providerID";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                adapter.SelectCommand.Parameters.AddWithValue("@providerID", providerID);

                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView5.DataSource = table;
            }
        }

        private void LoadPerformanceData(int providerID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Query for Occupancy Rate
                string occupancyQuery = @"
            SELECT SUM(t.Capacity) AS TotalCapacity, COUNT(b.BookingID) AS TotalBookings
            FROM Booking b
            JOIN Trip t ON b.TripID = t.TripID
            JOIN TripResource tr ON t.TripID = tr.TripID
            WHERE tr.ProviderID = @providerID AND b.Status = 'Paid'";

                SqlCommand occupancyCmd = new SqlCommand(occupancyQuery, con);
                occupancyCmd.Parameters.AddWithValue("@providerID", providerID);

                SqlDataReader occupancyReader = occupancyCmd.ExecuteReader();
                if (occupancyReader.Read())
                {
                    int totalCapacity = Convert.ToInt32(occupancyReader["TotalCapacity"]);
                    int totalBookings = Convert.ToInt32(occupancyReader["TotalBookings"]);

                    // Calculate occupancy rate (Booked / Total Capacity)
                    double occupancyRate = totalCapacity > 0 ? (double)totalBookings / totalCapacity * 100 : 0;
                    lblOccupancyRate.Text = $"Occupancy Rate: {occupancyRate:F2}%";  // Display on a label
                }
                occupancyReader.Close();

                // Query for Average Traveler Feedback (Rating)
                string feedbackQuery = @"
            SELECT AVG(r.Rating) AS AvgRating
            FROM Review r
            JOIN Trip t ON r.TripID = t.TripID
            JOIN TripResource tr ON t.TripID = tr.TripID
            WHERE tr.ProviderID = @providerID";

                SqlCommand feedbackCmd = new SqlCommand(feedbackQuery, con);
                feedbackCmd.Parameters.AddWithValue("@providerID", providerID);

                SqlDataReader feedbackReader = feedbackCmd.ExecuteReader();
                if (feedbackReader.Read())
                {
                    decimal avgRating = feedbackReader["AvgRating"] != DBNull.Value ? Convert.ToDecimal(feedbackReader["AvgRating"]) : 0;
                    lblFeedback.Text = $"Average Traveler Feedback: {avgRating:F1} / 10";  // Display on a label
                }
                feedbackReader.Close();

                // Query for Total Revenue
                string revenueQuery = @"
            SELECT SUM(b.TotalAmount) AS TotalRevenue
            FROM Booking b
            JOIN Trip t ON b.TripID = t.TripID
            JOIN TripResource tr ON t.TripID = tr.TripID
            WHERE tr.ProviderID = @providerID AND b.Status = 'Paid'";

                SqlCommand revenueCmd = new SqlCommand(revenueQuery, con);
                revenueCmd.Parameters.AddWithValue("@providerID", providerID);

                SqlDataReader revenueReader = revenueCmd.ExecuteReader();
                if (revenueReader.Read())
                {
                    decimal totalRevenue = revenueReader["TotalRevenue"] != DBNull.Value ? Convert.ToDecimal(revenueReader["TotalRevenue"]) : 0;
                    lblRevenue.Text = $"Total Revenue: ${totalRevenue:F2}";  // Display on a label
                }
                revenueReader.Close();
            }
        }

        private void LoadTripFeedback(int providerID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT r.Rating, r.ReviewDate, r.Comment, r.TripID, u.Name AS ReviewerName
            FROM Review r
            JOIN Trip t ON r.TripID = t.TripID
            JOIN TripResource tr ON t.TripID = tr.TripID
            JOIN [User] u ON r.UserID = u.UserID
            WHERE tr.ProviderID = @providerID";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                adapter.SelectCommand.Parameters.AddWithValue("@providerID", providerID);

                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView3.DataSource = table;
            }
        }

        private void ServiceProviderMainPage_Load(object sender, EventArgs e)
        {
            LoadUnapprovedTripResources();
            LoadResourceProfiles();
            dataGridView4.CellEndEdit += dataGridView4_CellEndEdit;
            LoadPendingBookings(providerID);
            LoadPerformanceData(providerID);
            LoadTripFeedback(providerID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int resourceId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ResourceID"].Value);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string updateQuery = "UPDATE TripResource SET Status = 'Approved' WHERE ResourceID = @ResourceID";
                    SqlCommand cmd = new SqlCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@ResourceID", resourceId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                MessageBox.Show("Resource approved successfully!");
                LoadUnapprovedTripResources(); // Refresh grid
            }
            else
            {
                MessageBox.Show("Please select a resource to approve.");
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private int GetNextPaymentId(SqlConnection con, SqlTransaction transaction)
        {
            string query = "SELECT ISNULL(MAX(PaymentID), 0) + 1 FROM Paid";

            using (SqlCommand cmd = new SqlCommand(query, con, transaction))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView5.CurrentRow == null)
            {
                MessageBox.Show("Please select a booking to approve.");
                return;
            }

            // Get booking and trip IDs
            int bookingId = Convert.ToInt32(dataGridView5.CurrentRow.Cells["BookingID"].Value);
            int tripId = Convert.ToInt32(dataGridView5.CurrentRow.Cells["TripID"].Value);
            decimal totalAmount = Convert.ToDecimal(dataGridView5.CurrentRow.Cells["TotalAmount"].Value);

            // Determine payment method
            string paymentMethod = "";
            if (radioButton1.Checked)
                paymentMethod = "Credit/Debit Card";
            else if (radioButton2.Checked)
                paymentMethod = "Online";
            else if (radioButton3.Checked)
                paymentMethod = "Cash";
            else
            {
                MessageBox.Show("Please select a payment method.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // 1. Update Booking status to Paid
                    string updateBooking = "UPDATE Booking SET Status = 'Paid' WHERE BookingID = @bookingId";
                    SqlCommand cmd1 = new SqlCommand(updateBooking, con, transaction);
                    cmd1.Parameters.AddWithValue("@bookingId", bookingId);
                    cmd1.ExecuteNonQuery();

                    // 2. Reduce Trip capacity
                    string updateTrip = "UPDATE Trip SET Capacity = Capacity - 1 WHERE TripID = @tripId AND Capacity > 0";
                    SqlCommand cmd2 = new SqlCommand(updateTrip, con, transaction);
                    cmd2.Parameters.AddWithValue("@tripId", tripId);
                    int rowsAffected = cmd2.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new Exception("No available capacity for this trip.");

                    // 3. Get UserID from Booking
                    SqlCommand getUserCmd = new SqlCommand("SELECT UserID FROM Booking WHERE BookingID = @bookingId", con, transaction);
                    getUserCmd.Parameters.AddWithValue("@bookingId", bookingId);
                    int userId = Convert.ToInt32(getUserCmd.ExecuteScalar());

                    // 4. Get ProviderID from TripResource
                    SqlCommand getProviderCmd = new SqlCommand("SELECT ProviderID FROM TripResource WHERE TripID = @tripId", con, transaction);
                    getProviderCmd.Parameters.AddWithValue("@tripId", tripId);
                    int providerId = Convert.ToInt32(getProviderCmd.ExecuteScalar());

                    // 5. Insert into Paid
                    int newPaymentId = GetNextPaymentId(con, transaction);
                    string insertPaid = @"INSERT INTO Paid (PaymentID, PaymentDate, PaymentMethod, Amount, TripID, ProviderID, UserID)
                      VALUES (@id, @date, @method, @amount, @tripId, @providerId, @userId)";
                    SqlCommand cmdPaid = new SqlCommand(insertPaid, con, transaction);
                    cmdPaid.Parameters.AddWithValue("@id", newPaymentId);
                    cmdPaid.Parameters.AddWithValue("@date", DateTime.Today);
                    cmdPaid.Parameters.AddWithValue("@method", paymentMethod);
                    cmdPaid.Parameters.AddWithValue("@amount", totalAmount);
                    cmdPaid.Parameters.AddWithValue("@tripId", tripId);
                    cmdPaid.Parameters.AddWithValue("@providerId", providerId);
                    cmdPaid.Parameters.AddWithValue("@userId", userId);
                    cmdPaid.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Booking approved and payment recorded successfully.");
                    LoadPendingBookings(providerId);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MainPage main = new MainPage();
            main.Show();
            this.Hide();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
