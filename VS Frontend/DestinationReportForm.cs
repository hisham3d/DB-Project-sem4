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
    public partial class DestinationReportForm : Form
    {
        public DestinationReportForm()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False");
        private void DestinationReportForm_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        


        DataTable destinationTable;

        
        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("select * from Destination", con);
            SqlDataAdapter d = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            d.Fill(dt);
            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource source = new ReportDataSource("Destination", dt);
            reportViewer1.LocalReport.ReportPath = "D:\\Project\\Project\\Project_FrontEnd\\DestinationReport.rdlc";
            reportViewer1.LocalReport.DataSources.Add(source);
            reportViewer1.RefreshReport();
          
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source = RUHAB - LENOVOTHI\\SQLEXPRESS; Initial Catalog = TravelEase; Integrated Security = True; Encrypt=False";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Query to get Top 3 Most Booked Destinations by Country
                string queryCountry = @"
            SELECT  TOP 3 Country, COUNT(*) AS BookedCount
            FROM Destination
            
            GROUP BY Country
            ORDER BY BookedCount DESC";

                SqlDataAdapter daCountry = new SqlDataAdapter(queryCountry, con);
                DataTable dtCountry = new DataTable();
                daCountry.Fill(dtCountry);

                // Clear any previous data from the chart
                chart1.Series.Clear();
                chart1.Titles.Clear();
                chart1.Titles.Add("Top 3 Most Booked Destinations by Country");

                // Create a series for the pie chart
                Series series = new Series
                {
                    Name = "TopCountries",
                    ChartType = SeriesChartType.Pie,  // Set chart type to Pie
                    IsValueShownAsLabel = true        // Show values on pie chart
                };

                chart1.Series.Add(series);
                chart1.Legends[0].Enabled = true;

                // Add data points to the pie chart for each country
                foreach (DataRow row in dtCountry.Rows)
                {
                    string country = row["Country"].ToString();
                    int count = Convert.ToInt32(row["BookedCount"]);
                    series.Points.AddXY(country, count);
                }

                // Apply a colorful palette to the pie chart
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
