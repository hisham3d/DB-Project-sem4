using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_FrontEnd
{
    public partial class ReportsShower : Form
    {
        public ReportsShower()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminReportForm adminReport = new AdminReportForm();
            adminReport.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           DestinationReportForm destinationReport = new DestinationReportForm();
            destinationReport.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PaymentReportForm paymentReport = new PaymentReportForm();
            paymentReport.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CancelledReportForm cancelledReport = new CancelledReportForm();
            cancelledReport.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OperatorReportForm operatorReport = new OperatorReportForm();
            operatorReport.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            PlatformReportForm platformReport = new PlatformReportForm();
            platformReport.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TravelerReportForm travelerReport = new TravelerReportForm();
            travelerReport.Show();
            this.Hide();
        }
    }
}
