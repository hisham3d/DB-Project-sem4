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
    public partial class ProviderReportForm : Form
    {
        public ProviderReportForm()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False");
        private void ProviderReportForm_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("select * from ServiceProvider", con);
            SqlDataAdapter d = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            d.Fill(dt);
            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource source = new ReportDataSource("ProviderDataSet", dt);
            reportViewer1.LocalReport.ReportPath = "D:\\Project\\Project\\Project_FrontEnd\\ProviderReport.rdlc";
            reportViewer1.LocalReport.DataSources.Add(source);
            reportViewer1.RefreshReport();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT BusinessName, COUNT(*) AS Count 
                         FROM ServiceProvider 
                         GROUP BY BusinessName";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No data found to display.");
                    return;
                }

                chart1.Series.Clear();
                chart1.Titles.Clear();
                chart1.Titles.Add("Service Provider Distribution by Business Type");

                Series series = new Series
                {
                    Name = "BusinessType",
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = true,
                    Label = "#PERCENT{P1}",        // Show percentage
                    LegendText = "#VALX"           // Show BusinessName in legend
                };

                chart1.Series.Add(series);
                chart1.Legends[0].Enabled = true;
                chart1.Palette = ChartColorPalette.BrightPastel;

                foreach (DataRow row in dt.Rows)
                {
                    string businessType = row["BusinessName"].ToString();
                    int count = Convert.ToInt32(row["Count"]);
                    series.Points.AddXY(businessType, count);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT BusinessName, COUNT(*) AS Count 
                         FROM ServiceProvider 
                         GROUP BY BusinessName";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No data found to display.");
                    return;
                }

                chart1.Series.Clear();
                chart1.Titles.Clear();
                chart1.Titles.Add("Service Provider Distribution by Business Type");

                Series series = new Series
                {
                    Name = "BusinessType",
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = true,
                    Label = "#PERCENT{P1}",        // Show percentage
                    LegendText = "#VALX"           // Show BusinessName in legend
                };

                chart1.Series.Add(series);
                chart1.Legends[0].Enabled = true;
                chart1.Palette = ChartColorPalette.BrightPastel;

                foreach (DataRow row in dt.Rows)
                {
                    string businessType = row["BusinessName"].ToString();
                    int count = Convert.ToInt32(row["Count"]);
                    series.Points.AddXY(businessType, count);
                }
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
