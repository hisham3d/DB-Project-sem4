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
    public partial class OperatorReportForm : Form
    {
        public OperatorReportForm()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False");

        private void OperatorReportForm_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("select * from Operator", con);
            SqlDataAdapter d = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            d.Fill(dt);
            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource source = new ReportDataSource("OperatorDataSet", dt);
            reportViewer1.LocalReport.ReportPath = "D:\\Project\\Project\\Project_FrontEnd\\OperatorReport.rdlc";
            reportViewer1.LocalReport.DataSources.Add(source);
            reportViewer1.RefreshReport();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT CompanyName, COUNT(*) AS Count 
                         FROM Operator 
                         GROUP BY CompanyName";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                chart1.Series.Clear();
                chart1.Titles.Clear();
                chart1.Titles.Add("Proportion of Operators by Company");

                Series series = new Series
                {
                    Name = "OperatorCompany",
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = true
                };

                chart1.Series.Add(series);
                chart1.Legends[0].Enabled = true;

                foreach (DataRow row in dt.Rows)
                {
                    string company = row["CompanyName"].ToString();
                    int count = Convert.ToInt32(row["Count"]);
                    series.Points.AddXY(company, count);
                }

                chart1.Palette = ChartColorPalette.BrightPastel;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            ReportsShower reportsShower = new ReportsShower();
            reportsShower.Show();
            this.Hide();
        }
    }
}
