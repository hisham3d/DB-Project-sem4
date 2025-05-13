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
    public partial class OperatorMainPage : Form
    {
        public OperatorMainPage()
        {
            InitializeComponent();
        }
        private int operatorID;
        string connectionString = "Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False";
        public OperatorMainPage(int userID)
        {
            InitializeComponent();
            operatorID = userID;

            // You can now use travelerID inside this form
            // e.g., LoadTravelerDetails(travelerID);
        }

        // Optional: method to load traveler details using travelerID
        // private void LoadTravelerDetails(int id)
        // {
        //     // Fetch from database if needed
        // }

        private void LoadDestinations()
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Destination";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error loading destinations: " + ex.Message);
                    }
                }
            }
        }

        private void LoadOperatorTrips()
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT TripID, Capacity, Description, Duration, PricePerPerson, DestinationID FROM Trip WHERE OperatorID = @operatorID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@operatorID", operatorID);

                    try
                    {
                        con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView2.DataSource = dt;

                        dataGridView2.ReadOnly = false;
                        dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error loading trips: " + ex.Message);
                    }
                }
            }
        }
        private void LoadBookings()
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT B.BookingID, B.TripID, B.UserID, B.BookingDate, B.Status, B.TotalAmount 
                         FROM Booking B 
                         INNER JOIN Trip T ON B.TripID = T.TripID 
                         WHERE T.OperatorID = @operatorID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@operatorID", operatorID);

                    try
                    {
                        con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView3.DataSource = dt;

                        dataGridView3.ReadOnly = true;
                        dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error loading bookings: " + ex.Message);
                    }
                }
            }
        }

        private void LoadAnalytics()
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // Total Bookings
                    SqlCommand cmdBookings = new SqlCommand(@"
                SELECT COUNT(*) 
                FROM Booking B 
                INNER JOIN Trip T ON B.TripID = T.TripID 
                WHERE T.OperatorID = @operatorID", con);
                    cmdBookings.Parameters.AddWithValue("@operatorID", operatorID);
                    int totalBookings = (int)cmdBookings.ExecuteScalar();
                    labelTotalBookings.Text = "Total Bookings: " + totalBookings;

                    // Total Revenue
                    SqlCommand cmdRevenue = new SqlCommand(@"
                SELECT ISNULL(SUM(B.TotalAmount), 0) 
                FROM Booking B 
                INNER JOIN Trip T ON B.TripID = T.TripID 
                WHERE T.OperatorID = @operatorID AND B.Status = 'Paid'", con);
                    cmdRevenue.Parameters.AddWithValue("@operatorID", operatorID);
                    decimal revenue = (decimal)cmdRevenue.ExecuteScalar();
                    labelRevenue.Text = "Total Revenue: Rs. " + revenue;

                    // Average Rating
                    SqlCommand cmdRating = new SqlCommand(@"
                SELECT AVG(CAST(Rating AS FLOAT)) 
                FROM Review R 
                INNER JOIN Trip T ON R.TripID = T.TripID 
                WHERE T.OperatorID = @operatorID", con);
                    cmdRating.Parameters.AddWithValue("@operatorID", operatorID);
                    object avgRatingObj = cmdRating.ExecuteScalar();
                    string avgRatingText = avgRatingObj != DBNull.Value ? ((double)avgRatingObj).ToString("0.0") : "N/A";
                    labelAvgRating.Text = "Average Rating: " + avgRatingText;

                    // Optional: Review Comments
                    SqlCommand cmdReviews = new SqlCommand(@"
                SELECT R.ReviewID, R.Rating, R.Comment, R.TripID 
                FROM Review R 
                INNER JOIN Trip T ON R.TripID = T.TripID 
                WHERE T.OperatorID = @operatorID", con);
                    cmdReviews.Parameters.AddWithValue("@operatorID", operatorID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmdReviews);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewReviews.DataSource = dt;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error loading analytics: " + ex.Message);
                }
            }
        }
        private void LoadServiceProviders()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT ProviderID, BusinessName, Type AS ResourceType, ContactInfo FROM ServiceProvider";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView4.DataSource = dt;
            }
        }

        private void LoadTripResourceAssignments()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT TR.ResourceID, TR.ResourceType, SP.BusinessName, TR.TripID
            FROM TripResource TR
            INNER JOIN ServiceProvider SP ON TR.ProviderID = SP.ProviderID
            INNER JOIN Trip T ON TR.TripID = T.TripID
            WHERE T.OperatorID = @operatorID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@operatorID", operatorID);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView5.DataSource = dt;
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void OperatorMainPage_Load(object sender, EventArgs e)
        {
            LoadDestinations();
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LoadOperatorTrips();
            dataGridView2.CellEndEdit += dataGridView2_CellEndEdit;

            LoadBookings();

            LoadAnalytics();

            LoadServiceProviders();

            LoadTripResourceAssignments();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate and parse inputs
            if (!int.TryParse(textBox3.Text, out int capacity) || capacity <= 0)
            {
                MessageBox.Show("Please enter a valid capacity.");
                return;
            }

            string description = textBox4.Text;
            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please enter a description.");
                return;
            }

            if (!int.TryParse(textBox5.Text, out int duration) || duration <= 0)
            {
                MessageBox.Show("Please enter a valid duration.");
                return;
            }

            if (!int.TryParse(textBox6.Text, out int destinationID))
            {
                MessageBox.Show("Please enter a valid Destination ID.");
                return;
            }

            if (!decimal.TryParse(textBox7.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            // Connection string
           
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Trip (Capacity, Description, Duration, PricePerPerson, OperatorID, DestinationID) " +
                               "VALUES (@capacity, @description, @duration, @price, @operatorID, @destinationID)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@capacity", capacity);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@duration", duration);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@operatorID", operatorID); // from form constructor
                    cmd.Parameters.AddWithValue("@destinationID", destinationID);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Trip added successfully!");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error inserting trip: " + ex.Message);
                    }
                }
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentRow != null)
            {
                int tripID = Convert.ToInt32(dataGridView2.CurrentRow.Cells["TripID"].Value);
                int capacity = Convert.ToInt32(dataGridView2.CurrentRow.Cells["Capacity"].Value);
                string description = dataGridView2.CurrentRow.Cells["Description"].Value.ToString();
                int duration = Convert.ToInt32(dataGridView2.CurrentRow.Cells["Duration"].Value);
                decimal price = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["PricePerPerson"].Value);
                int destinationID = Convert.ToInt32(dataGridView2.CurrentRow.Cells["DestinationID"].Value);

                string connectionString = "Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string updateQuery = @"UPDATE Trip 
                                   SET Capacity = @capacity, Description = @description, 
                                       Duration = @duration, PricePerPerson = @price, 
                                       DestinationID = @destinationID 
                                   WHERE TripID = @tripID AND OperatorID = @operatorID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@capacity", capacity);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@duration", duration);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@destinationID", destinationID);
                        cmd.Parameters.AddWithValue("@tripID", tripID);
                        cmd.Parameters.AddWithValue("@operatorID", operatorID);

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Trip updated successfully.");
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Error updating trip: " + ex.Message);
                        }
                    }
                }
            }
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a booking to cancel.");
                return;
            }

            int bookingID = Convert.ToInt32(dataGridView3.SelectedRows[0].Cells["BookingID"].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Booking SET Status = 'Cancelled' WHERE BookingID = @bookingID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@bookingID", bookingID);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Booking cancelled.");
                        LoadBookings(); // Refresh
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error cancelling booking: " + ex.Message);
                    }
                }
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void labelRevenue1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void labelTotalBookings_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int tripID))
            {
                MessageBox.Show("Invalid Trip ID");
                return;
            }

            if (!int.TryParse(textBox2.Text, out int providerID))
            {
                MessageBox.Show("Invalid Provider ID");
                return;
            }

            string resourceType = "";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    // First, get the resource type
                    string typeQuery = "SELECT Type FROM ServiceProvider WHERE ProviderID = @providerID";
                    SqlCommand typeCmd = new SqlCommand(typeQuery, con);
                    typeCmd.Parameters.AddWithValue("@providerID", providerID);

                    con.Open();
                    object result = typeCmd.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("Provider not found.");
                        return;
                    }
                    resourceType = result.ToString();

                    // Then insert into TripResource
                    string insertQuery = @"INSERT INTO TripResource (ResourceType, ProviderID, TripID)
                                   VALUES (@type, @providerID, @tripID)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                    insertCmd.Parameters.AddWithValue("@type", resourceType);
                    insertCmd.Parameters.AddWithValue("@providerID", providerID);
                    insertCmd.Parameters.AddWithValue("@tripID", tripID);

                    insertCmd.ExecuteNonQuery();
                    MessageBox.Show("Resource assigned to trip successfully.");

                    LoadTripResourceAssignments();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error assigning resource: " + ex.Message);
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
