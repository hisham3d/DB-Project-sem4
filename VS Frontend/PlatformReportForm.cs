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
    public partial class PlatformReportForm : Form
    {
        public PlatformReportForm()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False");

        /*private void PlatformReportForm_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }*/

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                FORMAT(BookingDate, 'yyyy-MM') AS MonthYear,
                COUNT(*) AS BookingCount,
                SUM(TotalAmount) AS TotalAmount
            FROM Booking
            GROUP BY FORMAT(BookingDate, 'yyyy-MM')
            ORDER BY MonthYear";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // ----------- CHART 1: Number of Bookings Per Month ------------
                chart1.Series.Clear();
                chart1.Titles.Clear();
                chart1.Titles.Add("Monthly Booking Count");

                Series bookingSeries = new Series
                {
                    Name = "Bookings",
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 3,
                    Color = Color.DodgerBlue,
                    IsValueShownAsLabel = true
                };

                chart1.Series.Add(bookingSeries);
                chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.ChartAreas[0].AxisX.Title = "Month";
                chart1.ChartAreas[0].AxisY.Title = "Number of Bookings";
                chart1.Legends[0].Enabled = true;

                foreach (DataRow row in dt.Rows)
                {
                    string month = row["MonthYear"].ToString();
                    int count = Convert.ToInt32(row["BookingCount"]);
                    bookingSeries.Points.AddXY(month, count);
                }

                chart1.Refresh();

                // ----------- CHART 2: Total Amount Per Month ------------
                chart2.Series.Clear();
                chart2.Titles.Clear();
                chart2.Titles.Add("Monthly Booking Revenue");

                Series amountSeries = new Series
                {
                    Name = "Revenue",
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 3,
                    Color = Color.MediumVioletRed,
                    IsValueShownAsLabel = true
                };

                chart2.Series.Add(amountSeries);
                chart2.ChartAreas[0].AxisX.Interval = 1;
                chart2.ChartAreas[0].AxisX.Title = "Month";
                chart2.ChartAreas[0].AxisY.Title = "Total Amount ($)";
                chart2.Legends[0].Enabled = true;

                foreach (DataRow row in dt.Rows)
                {
                    string month = row["MonthYear"].ToString();
                    decimal total = Convert.ToDecimal(row["TotalAmount"]);
                    amountSeries.Points.AddXY(month, total);
                }

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
