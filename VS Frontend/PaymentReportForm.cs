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
    public partial class PaymentReportForm : Form
    {
        public PaymentReportForm()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False");
        private void PaymentReportForm_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("select * from Paid", con);
            SqlDataAdapter d = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            d.Fill(dt);
            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource source = new ReportDataSource("Payment", dt);
            reportViewer1.LocalReport.ReportPath = "D:\\Project\\Project\\Project_FrontEnd\\PaymentReport.rdlc";
            reportViewer1.LocalReport.DataSources.Add(source);
            reportViewer1.RefreshReport();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Query to get the percentage of each payment method
                string query = @"
        SELECT PaymentMethod, COUNT(*) AS PaymentCount
        FROM Paid
        GROUP BY PaymentMethod";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Clear any previous data from the chart
                chart1.Series.Clear();
                chart1.Titles.Clear();
                chart1.Titles.Add("Payment Method Distribution");

                // Create a series for the pie chart
                Series series = new Series
                {
                    Name = "PaymentMethods",
                    ChartType = SeriesChartType.Pie,  // Set chart type to Pie
                    IsValueShownAsLabel = true        // Show values on the pie chart
                };

                chart1.Series.Add(series);
                chart1.Legends[0].Enabled = true;

                // Add data points to the pie chart
                foreach (DataRow row in dt.Rows)
                {
                    string paymentMethod = row["PaymentMethod"].ToString();
                    int count = Convert.ToInt32(row["PaymentCount"]);
                    series.Points.AddXY(paymentMethod, count);
                }

                // Apply a colorful palette to the pie chart
                chart1.Palette = ChartColorPalette.BrightPastel;

                // Refresh the chart to display the data
                chart1.Refresh();

                // Query to get the total amount paid per year
                string query1 = @"
        SELECT YEAR(PaymentDate) AS Year, SUM(Amount) AS TotalAmount
        FROM Paid
        GROUP BY YEAR(PaymentDate)
        ORDER BY Year";

                SqlDataAdapter da1 = new SqlDataAdapter(query1, con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1); // Use da1 to fill dt1, not da.Fill

                // Clear any previous data from the chart2
                chart2.Series.Clear();
                chart2.Titles.Clear();
                chart2.Titles.Add("Total Amount Paid Per Year");

                // Create a series for the bar chart
                Series series1 = new Series
                {
                    Name = "TotalAmountPerYear",
                    ChartType = SeriesChartType.Column,  // Set chart type to Column (Bar Chart)
                    IsValueShownAsLabel = true           // Show values above the bars
                };

                chart2.Series.Add(series1);
                chart2.Legends[0].Enabled = true;

                // Add data points to the bar chart
                foreach (DataRow row in dt1.Rows)  // Use dt1 for the second query's results
                {
                    int year = Convert.ToInt32(row["Year"]);
                    decimal totalAmount = Convert.ToDecimal(row["TotalAmount"]);
                    series1.Points.AddXY(year, totalAmount);
                }

                // Apply a color palette for better visualization
                chart2.Palette = ChartColorPalette.BrightPastel;

                // Refresh the chart to display the data
                chart2.Refresh();
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
