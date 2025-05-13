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
    public partial class TravelerReportForm : Form
    {
        public TravelerReportForm()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False");
        private void TravelerReportForm_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("select * from Traveler", con);
            SqlDataAdapter d = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            d.Fill(dt);
            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource source = new ReportDataSource("Traveler", dt);
            reportViewer1.LocalReport.ReportPath = "D:\\Project\\Project\\Project_FrontEnd\\TravelerReport.rdlc";
            reportViewer1.LocalReport.DataSources.Add(source);
            reportViewer1.RefreshReport();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=RUHAB-LENOVOTHI\\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // ----- Chart 1: Gender Proportionality -----
                string genderQuery = @"SELECT Gender, COUNT(*) AS Count 
                               FROM Traveler 
                               GROUP BY Gender";
                SqlDataAdapter genderAdapter = new SqlDataAdapter(genderQuery, con);
                DataTable genderTable = new DataTable();
                genderAdapter.Fill(genderTable);

                chart1.Series.Clear();
                chart1.Titles.Clear();
                chart1.Titles.Add("Gender Proportionality");
                Series genderSeries = new Series
                {
                    Name = "Gender",
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = true,
                    Label = "#PERCENT{P1}",
                    LegendText = "#VALX"
                };
                chart1.Series.Add(genderSeries);
                chart1.Legends[0].Enabled = true;
                chart1.Palette = ChartColorPalette.Excel;

                foreach (DataRow row in genderTable.Rows)
                {
                    string gender = row["Gender"].ToString();
                    int count = Convert.ToInt32(row["Count"]);
                    genderSeries.Points.AddXY(gender, count);
                }

                // ----- Chart 2: Nationality Proportionality -----
                string nationalityQuery = @"SELECT Nationality, COUNT(*) AS Count 
                                    FROM Traveler 
                                    GROUP BY Nationality";
                SqlDataAdapter nationalityAdapter = new SqlDataAdapter(nationalityQuery, con);
                DataTable nationalityTable = new DataTable();
                nationalityAdapter.Fill(nationalityTable);

                chart2.Series.Clear();
                chart2.Titles.Clear();
                chart2.Titles.Add("Nationality Proportionality");
                Series nationalitySeries = new Series
                {
                    Name = "Nationality",
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = true,
                    Label = "#PERCENT{P1}",
                    LegendText = "#VALX"
                };
                chart2.Series.Add(nationalitySeries);
                chart2.Legends[0].Enabled = true;
                chart2.Palette = ChartColorPalette.BrightPastel;

                foreach (DataRow row in nationalityTable.Rows)
                {
                    string nationality = row["Nationality"].ToString();
                    int count = Convert.ToInt32(row["Count"]);
                    nationalitySeries.Points.AddXY(nationality, count);
                }
                // bar chart 
                string query = @"
            SELECT 
                CASE 
                    WHEN Age BETWEEN 10 AND 20 THEN '10-20'
                    WHEN Age BETWEEN 21 AND 30 THEN '21-30'
                    WHEN Age BETWEEN 31 AND 40 THEN '31-40'
                    WHEN Age BETWEEN 41 AND 50 THEN '41-50'
                    ELSE '51+' 
                END AS AgeGroup,
                COUNT(*) AS Count
            FROM Traveler
            GROUP BY 
                CASE 
                    WHEN Age BETWEEN 10 AND 20 THEN '10-20'
                    WHEN Age BETWEEN 21 AND 30 THEN '21-30'
                    WHEN Age BETWEEN 31 AND 40 THEN '31-40'
                    WHEN Age BETWEEN 41 AND 50 THEN '41-50'
                    ELSE '51+' 
                END";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                chart3.Series.Clear();
                chart3.Titles.Clear();
                chart3.Titles.Add("Traveler Age Group Proportions");

                Series series = new Series
                {
                    Name = "AgeGroup",
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = true,
                    Label = "#PERCENT{P1}",
                    LegendText = "#VALX"
                };

                chart3.Series.Add(series);
                chart3.Legends[0].Enabled = true;
                chart3.Palette = ChartColorPalette.Bright;

                foreach (DataRow row in dt.Rows)
                {
                    string group = row["AgeGroup"].ToString();
                    int count = Convert.ToInt32(row["Count"]);
                    series.Points.AddXY(group, count);
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}