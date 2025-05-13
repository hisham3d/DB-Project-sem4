using System;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_FrontEnd
{
    public partial class TravelerMainPage : Form
    {
        public TravelerMainPage()
        {
            InitializeComponent();
        }
        private int travelerID;
        string connectionString = "Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False";
        public TravelerMainPage(int userID)
        {
            InitializeComponent();
            travelerID = userID;

            // You can now use travelerID inside this form
            // e.g., LoadTravelerDetails(travelerID);
        }

        // Optional: method to load traveler details using travelerID
        // private void LoadTravelerDetails(int id)
        // {
        //     // Fetch from database if needed
        // }

        private int selectedRating = 0;

        private void LoadAvailableTrips()
        {
            string connectionString = "Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT TripID, Capacity, Description, Duration, PricePerPerson FROM Trip";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView2.DataSource = dt;
            }
        }
        private void LoadTravelerBookings()
        {
            string connectionString = "Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT BookingID, BookingDate, Status, TotalAmount, TripID FROM Booking WHERE UserID = @userID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userID", travelerID);

                    try
                    {
                        con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable bookingTable = new DataTable();
                        adapter.Fill(bookingTable);

                        dataGridView1.DataSource = bookingTable;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error loading bookings: " + ex.Message);
                    }
                }
            }
        }
        private void LoadTravelerDetails()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                u.Name, 
                u.UserID, 
                t.Age, 
                t.Gender, 
                t.Nationality, 
                u.Phone
            FROM [User] u
            INNER JOIN Traveler t ON u.UserID = t.TravelerID
            WHERE u.UserID = @travelerID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@travelerID", travelerID);

                    try
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            label11.Text = reader["Name"].ToString();
                            label12.Text = reader["UserID"].ToString();
                            label13.Text = reader["Age"].ToString();
                            label14.Text = reader["Gender"].ToString();
                            label15.Text = reader["Nationality"].ToString();
                            label16.Text = reader["Phone"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Traveler details not found.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error fetching traveler details: " + ex.Message);
                    }
                }
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void TravelerMainPage_Load(object sender, EventArgs e)
        {
            LoadTravelerBookings();
            LoadAvailableTrips();
            LoadTravelerDetails();
            dataGridView2.ReadOnly = true;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox2.Text, out int rating) || rating < 1 || rating > 10)
            {
                MessageBox.Show("Please enter a valid rating (1-10).");
                return;
            }

            if (!int.TryParse(textBox1.Text, out int tripID))
            {
                MessageBox.Show("Invalid Trip ID.");
                return;
            }

            string comment = richTextBox1.Text;
            
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Review (TripID, UserID, Rating, ReviewDate, Comment) VALUES (@tripID, @userID, @rating, @reviewDate, @comment)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@tripID", tripID);
                    cmd.Parameters.AddWithValue("@userID", travelerID);
                    cmd.Parameters.AddWithValue("@rating", rating);
                    cmd.Parameters.AddWithValue("@reviewDate", DateTime.Today);
                    cmd.Parameters.AddWithValue("@comment", comment);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Review submitted successfully!");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a trip to book.");
                return;
            }

            int tripID = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["TripID"].Value);
            decimal pricePerPerson = Convert.ToDecimal(dataGridView2.SelectedRows[0].Cells["PricePerPerson"].Value);
            DateTime today = DateTime.Today;
            string status = "Unpaid"; // Change status to 'Unpaid'

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Insert into Booking only
                    string insertBookingQuery = "INSERT INTO Booking (BookingDate, Status, TotalAmount, TripID, UserID) " +
                                                "VALUES (@date, @status, @amount, @tripID, @userID);";

                    SqlCommand bookingCmd = new SqlCommand(insertBookingQuery, con, transaction);
                    bookingCmd.Parameters.AddWithValue("@date", today);
                    bookingCmd.Parameters.AddWithValue("@status", status);
                    bookingCmd.Parameters.AddWithValue("@amount", pricePerPerson);
                    bookingCmd.Parameters.AddWithValue("@tripID", tripID);
                    bookingCmd.Parameters.AddWithValue("@userID", travelerID);

                    bookingCmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Trip booked successfully with status 'Unpaid'.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private int GetNewCancellationID(SqlConnection con, SqlTransaction transaction)
        {
            string query = "SELECT ISNULL(MAX(CancellationID), 0) + 1 FROM Cancelled";
            SqlCommand cmd = new SqlCommand(query, con, transaction);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a booking to cancel.");
                return;
            }

            int bookingId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["BookingID"].Value);
            decimal refundAmount = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["TotalAmount"].Value);
            string reason = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("Please enter a cancellation reason.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // 1. Update Booking status
                    string updateQuery = "UPDATE Booking SET Status = 'Cancelled' WHERE BookingID = @bookingId";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con, transaction);
                    updateCmd.Parameters.AddWithValue("@bookingId", bookingId);
                    updateCmd.ExecuteNonQuery();

                    // 2. Insert into Cancelled table
                    int cancellationId = GetNewCancellationID(con, transaction);
                    string insertCancelledQuery = "INSERT INTO Cancelled (CancellationID, RefundAmount, Reason, BookingID) " +
                                                  "VALUES (@id, @refund, @reason, @bookingId)";

                    SqlCommand insertCmd = new SqlCommand(insertCancelledQuery, con, transaction);
                    insertCmd.Parameters.AddWithValue("@id", cancellationId);
                    insertCmd.Parameters.AddWithValue("@refund", refundAmount);
                    insertCmd.Parameters.AddWithValue("@reason", reason);
                    insertCmd.Parameters.AddWithValue("@bookingId", bookingId);
                    insertCmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Booking cancelled and recorded successfully.");

                    LoadTravelerBookings();
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
    }
}
