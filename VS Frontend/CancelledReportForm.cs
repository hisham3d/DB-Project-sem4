using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.IdentityModel.Protocols;

namespace Project_FrontEnd
{
    public partial class CancelledReportForm : Form
    {
        public CancelledReportForm()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False");
        private void CancelledReportForm_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("select * from Cancelled", con);
            SqlDataAdapter d = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            d.Fill(dt);
            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource source = new ReportDataSource("Cancelled", dt);
            reportViewer1.LocalReport.ReportPath = "D:\\Project\\Project\\Project_FrontEnd\\CancelledReport.rdlc";
            reportViewer1.LocalReport.DataSources.Add(source);
            reportViewer1.RefreshReport();
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source = RUHAB - LENOVOTHI\\SQLEXPRESS; Initial Catalog = TravelEase; Integrated Security = True; Encrypt=False";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Query to get the number of cancellations per year
                string query = @"
            SELECT YEAR(BookingDate) AS Year, COUNT(*) AS Cancellations
            FROM Booking
            WHERE Status = 'Cancelled'
            GROUP BY YEAR(BookingDate)
            ORDER BY Year";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Clear any previous data from the chart
                chart1.Series.Clear();
                chart1.Titles.Clear();
                chart1.Titles.Add("Number of Cancellations Per Year");

                // Create a series for the bar chart
                Series series = new Series
                {
                    Name = "CancellationsPerYear",
                    ChartType = SeriesChartType.Column,  // Set chart type to Column (Bar Chart)
                    IsValueShownAsLabel = true           // Display values above the bars
                };

                chart1.Series.Add(series);
                chart1.Legends[0].Enabled = true;

                // Add data points to the bar chart
                foreach (DataRow row in dt.Rows)
                {
                    int year = Convert.ToInt32(row["Year"]);
                    int cancellations = Convert.ToInt32(row["Cancellations"]);
                    series.Points.AddXY(year, cancellations);
                }

                // Apply a color palette for better visualization
                chart1.Palette = ChartColorPalette.BrightPastel;

                // Refresh the chart to display the data
                chart1.Refresh();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReportsShower reportsShower = new ReportsShower();
            reportsShower.Show();
            this.Hide();
        }
    }
}
